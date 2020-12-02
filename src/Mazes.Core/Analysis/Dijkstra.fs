// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Analysis.Dijkstra

open Mazes.Core
open Mazes.Core.Array2D

type Distance = int

type Node =
    {    
        DistanceFromRoot : int
        Neighbors :  seq<Coordinate>
    }

type FarthestFromRoot =
    {
        Distance : int
        Coordinates : Coordinate array
    }

type Map =
    {
        Root : Coordinate
        Nodes : (Node option)[,]
        TotalNodesAccessibleFromRoot : int
        FarthestFromRoot : FarthestFromRoot
    }

    member this.NumberOfRows =
        Array2D.length1 this.Nodes

    member this.NumberOfColumns =
        Array2D.length2 this.Nodes

    member this.MaxRowIndex =
        maxRowIndex this.Nodes

    member this.MaxColumnIndex =
        maxColumnIndex this.Nodes

    member this.Node coordinate =
        get this.Nodes coordinate

    member this.NodeNeighbors coordinate =
        match (this.Node coordinate) with
        | Some node -> node.Neighbors
        | None -> Seq.empty

    member this.PathFromGoalToRoot (goalCoordinate : Coordinate option) =
        let nextCoordinate node =
            node.Neighbors
            |> Seq.tryFind(
                fun coordinateNeighbor ->

                let nodeNeighbor = this.Node coordinateNeighbor
                match nodeNeighbor with
                | Some nodeNeighbor -> nodeNeighbor.DistanceFromRoot < node.DistanceFromRoot
                | None -> false)

        seq {
            let mutable currentCoordinate = goalCoordinate
            while currentCoordinate.IsSome do

                let currentCoordinateValue = currentCoordinate.Value

                yield currentCoordinateValue

                let currentNode = this.Node currentCoordinateValue                   

                match currentNode with
                | Some currentNode ->
                    currentCoordinate <- nextCoordinate currentNode                        
                | None ->
                    currentCoordinate <- None
        }

    member this.PathFromRootTo (goalCoordinate : Coordinate option) =
        this.PathFromGoalToRoot goalCoordinate
        |> Seq.rev

    member this.LongestPaths =
        seq {
            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create this.NodeNeighbors this.NumberOfRows this.NumberOfColumns farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.PathFromGoalToRoot (Some newFarthestCoordinate)
        }

    static member create getNeighborsCoordinate numberOfRows numberOfColumns rootCoordinate =

        let mutable farthestDistance = 0
        let farthestCoordinates = ResizeArray<Coordinate>()

        let updateFarthest actualDistance actualCoordinate =
            if actualDistance > farthestDistance then
                farthestCoordinates.Clear()
                farthestCoordinates.Add(actualCoordinate)
                farthestDistance <- actualDistance
            elif actualDistance = farthestDistance then
                farthestCoordinates.Add(actualCoordinate)            
            else
                ()

        let nodes = Array2D.create numberOfRows numberOfColumns None

        let mutable unvisited = Map.empty.Add(rootCoordinate, -1)    

        let counter = ref 0

        while not unvisited.IsEmpty do
            let unvisitedCoordinate = unvisited |> Seq.head

            let coordinate = unvisitedCoordinate.Key
            let actualDistance = unvisitedCoordinate.Value + 1

            // update the nodes
            let neighbors = getNeighborsCoordinate coordinate
            nodes.[coordinate.RowIndex, coordinate.ColumnIndex] <- Some { DistanceFromRoot = actualDistance; Neighbors = neighbors }

            // update the others useful infos
            incr counter
            updateFarthest actualDistance coordinate

            // update the unvisited with the coordinates that are not yet visited
            unvisited <-
                    neighbors
                    |> Seq.filter(fun nCoordinate -> (get nodes nCoordinate).IsNone)
                    |> Seq.fold
                           (fun (m : Map<Coordinate, Distance>) neighborCoordinate -> m.Add(neighborCoordinate, actualDistance))
                           (unvisited.Remove(coordinate))

        { Root = rootCoordinate
          Nodes = nodes
          TotalNodesAccessibleFromRoot = counter.Value
          FarthestFromRoot = { Distance = farthestDistance; Coordinates = farthestCoordinates.ToArray() } }