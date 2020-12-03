// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Analysis.Dijkstra

open System.Collections.Generic
open System.Text
open Mazes.Core
open Mazes.Core.Array2D

type Distance = int

type Node = {    
        DistanceFromRoot : int
        Neighbors :  seq<Coordinate>
    }

type FarthestFromRoot = {
        Distance : int
        Coordinates : Coordinate array
    }

type private DistanceVisited = {
        Distance : Distance
        Visited : bool
    }

type Map =
    {
        Root : Coordinate
        Nodes : (Node option)[,]
        ConnectedNodes : int
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

    member this.PathFromGoalToRoot goalCoordinate =
        let nextCoordinate coordinate =            
            match this.Node coordinate with
            | Some node ->
                node.Neighbors
                |> Seq.tryFind(
                    fun coordinateNeighbor ->

                    let nodeNeighbor = this.Node coordinateNeighbor
                    match nodeNeighbor with
                    | Some nodeNeighbor -> nodeNeighbor.DistanceFromRoot < node.DistanceFromRoot
                    | None -> false)
            | None -> None

        seq {
            let mutable (currentCoordinate : Coordinate option) = Some goalCoordinate

            while currentCoordinate.IsSome do
                yield currentCoordinate.Value
                currentCoordinate <- nextCoordinate currentCoordinate.Value
        }

    member this.PathFromRootTo goalCoordinate =
        this.PathFromGoalToRoot goalCoordinate
        |> Seq.rev

    member this.LongestPaths =
        seq {
            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create this.NodeNeighbors this.NumberOfRows this.NumberOfColumns farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.PathFromGoalToRoot newFarthestCoordinate
        }

    static member create getNeighborsCoordinate numberOfRows numberOfColumns rootCoordinate =

        // keeping track of the nodes by distances
        let mutable farthestDistance = 0
        let coordinatesByDistance = Dictionary<Distance, ResizeArray<Coordinate>>()

        let removeCoordinateByDistance coordinate oldDistance newDistance =
            if coordinatesByDistance.ContainsKey(oldDistance) then
                let distanceArray = coordinatesByDistance.Item(oldDistance)
                if distanceArray.Remove(coordinate) then
                    if distanceArray.Count = 0 then
                        farthestDistance <- newDistance
                        coordinatesByDistance.Remove(oldDistance) |> ignore

        let updateCoordinateByDistance coordinate newDistance =
            if coordinatesByDistance.ContainsKey(newDistance) then
                let distanceArray = coordinatesByDistance.Item(newDistance)
                distanceArray.Add(coordinate)
            else
                let distanceArray = ResizeArray<Coordinate>()
                distanceArray.Add(coordinate)
                coordinatesByDistance.Add(newDistance, distanceArray)

            if newDistance > farthestDistance then
                farthestDistance <- newDistance

        // the main array2d nodes
        let nodes = Array2D.create numberOfRows numberOfColumns None
        
        // gives the node which has the min distance from a seq of nodes
        let minDistanceNode (coordinates : Coordinate seq) =
            let someNodes =
                coordinates
                |> Seq.map(fun coordinate -> (get nodes coordinate))
                |> Seq.filter(fun node -> node.IsSome)
                
            if someNodes |> Seq.isEmpty then
                None
            else
                someNodes |> Seq.minBy(fun node -> node.Value.DistanceFromRoot)               

        // keeping track of the nodes to visit
        let unvisited = Dictionary<Coordinate, DistanceVisited>()

        let updateUnvisited coordinate distance visited =
            unvisited.Remove(coordinate) |> ignore
            unvisited.Add(coordinate, { Distance = distance; Visited = visited })

        // initializing with the root node
        unvisited.Add(rootCoordinate, { Distance = -1; Visited = false })

        let connectedNodes = ref 0

        // main loop
        while unvisited.Count > 0 do
            let current = unvisited |> Seq.head

            let coordinate = current.Key                   
            let neighbors = getNeighborsCoordinate coordinate
            
            let newDistance =
                match (minDistanceNode neighbors) with
                | Some minNeighborNode -> minNeighborNode.DistanceFromRoot + 1
                | None -> current.Value.Distance + 1
            
            nodes.[coordinate.RowIndex, coordinate.ColumnIndex] <- Some { DistanceFromRoot = newDistance; Neighbors = neighbors }

            // update the others useful infos
            if not (current.Value.Visited) then
                incr connectedNodes

            updateCoordinateByDistance coordinate newDistance

            // update the unvisited with the coordinates that are not yet visited or if we found a path that is shorter
            unvisited.Remove(coordinate) |> ignore

            neighbors
            |> Seq.iter(fun nCoordinate ->
                    match (get nodes nCoordinate) with
                    | Some neighborNode ->
                        if neighborNode.DistanceFromRoot >= newDistance then
                            updateUnvisited nCoordinate newDistance true
                            removeCoordinateByDistance nCoordinate neighborNode.DistanceFromRoot newDistance
                    | None -> updateUnvisited nCoordinate newDistance false    
                )

        { Root = rootCoordinate
          Nodes = nodes
          ConnectedNodes = connectedNodes.Value
          FarthestFromRoot = { Distance = farthestDistance; Coordinates = coordinatesByDistance.Item(farthestDistance).ToArray() } }

module Map =

    let toString map =
        let appendNode (sBuilder : StringBuilder) (node : Node option) =
            let sNode =
                match node with
                | Some node -> "(" + node.DistanceFromRoot.ToString() + ")"
                | None -> "( )"
            sBuilder.Append(sNode) |> ignore

        let appendRow (sBuilder : StringBuilder) rowNodes =
            rowNodes
            |> Array.iter(fun node -> appendNode sBuilder node)

            sBuilder.Append('\n') |> ignore

        let sBuilder = StringBuilder()        
        map.Nodes
            |> extractByRows
            |> Seq.iter(fun rowNodes -> appendRow sBuilder rowNodes)

        sBuilder.ToString()