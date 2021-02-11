// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra.Tracker

[<Struct>]
type FarthestFromRoot = {
        Distance : Distance
        Coordinates : Coordinate array
    }

type CoordinatesByDistance =
    {
        Container : Dictionary<Distance, HashSet<Coordinate>>
    }

    member private this.RemoveBase distance coordinate =
        if this.Container.ContainsKey(distance) then
            let distanceArray = this.Container.Item(distance)
            if distanceArray.Remove(coordinate) then
                if distanceArray.Count = 0 then
                    this.Container.Remove(distance) |> ignore

    member this.Remove distance coordinate =
        this.RemoveBase distance coordinate
        this.RemoveBase (distance + 1) coordinate

    member this.AddUpdate distance coordinate =
        if this.Container.ContainsKey(distance) then
            let distanceSet = this.Container.Item(distance)
            distanceSet.Add(coordinate) |> ignore
        else
            let distanceSet = HashSet<Coordinate>()
            distanceSet.Add(coordinate) |> ignore
            this.Container.Add(distance, distanceSet)

    member this.MaxDistance =
        this.Container.Keys |> Seq.max

    member this.CoordinatesWithDistance distance =
        let coordinates = Array.zeroCreate<Coordinate>(this.Container.Item(distance).Count)
        this.Container.Item(distance).CopyTo(coordinates)

        coordinates

    member this.Farthest =
        { Distance = this.MaxDistance - 1; Coordinates = this.CoordinatesWithDistance(this.MaxDistance) }

    static member createEmpty =
        { Container = Dictionary<Distance, HashSet<Coordinate>>() }

type Map =
    {
        ShortestPathGraph : ShortestPathGraph<Coordinate>
        ConnectedNodes : int
        Cost : Coordinate -> int
        FarthestFromRoot : FarthestFromRoot
        Leaves : Coordinate array
    }

    member this.LongestPaths =
        seq {
            let adjacentNodes node =
                    match this.ShortestPathGraph.AdjacentNodes node with
                    | Some nodes -> nodes
                    | None -> Seq.empty

            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create adjacentNodes this.Cost PriorityQueueTracker.createEmpty farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.ShortestPathGraph.PathFromGoalToRoot newFarthestCoordinate
        }

    static member create (linkedNeighbors : Coordinate -> Coordinate seq) (cost : Coordinate -> int) (unvisitedTracker : Tracker<Coordinate, Distance>) rootCoordinate =

        let coordinatesByDistance = CoordinatesByDistance.createEmpty

        let leaves = HashSet<Coordinate>()

        unvisitedTracker.Add rootCoordinate -1

        let graph = ShortestPathGraph.createEmpty rootCoordinate
        graph.AddNode(rootCoordinate)

        while unvisitedTracker.HasItems do

            let (coordinate, currentDistance) = unvisitedTracker.Pop

            let neighbors = linkedNeighbors coordinate |> Seq.toArray

            if (neighbors |> Seq.length) = 1 then
                leaves.Add(coordinate) |> ignore
            
            let newDistance =
                match (graph.NodeDistanceFromRoot coordinate) with
                | Some distance -> distance + (cost coordinate)
                | None -> currentDistance + 1

            coordinatesByDistance.AddUpdate newDistance coordinate

            for neighbor in neighbors do
                if not (graph.ContainsNode neighbor) then
                    unvisitedTracker.Add neighbor newDistance
                    graph.AddNode(neighbor)

                if not (graph.ContainsEdge coordinate neighbor) then
                    graph.AddEdge coordinate neighbor newDistance
                else
                    let edge = graph.Edge coordinate neighbor
                    match edge with
                    | Some (_, distance) ->
                        if newDistance < distance then
                            unvisitedTracker.Add neighbor newDistance
                            coordinatesByDistance.Remove distance neighbor

                            graph.RemoveEdge coordinate neighbor distance
                            graph.AddEdge coordinate neighbor newDistance
                    | None -> ()
        
        let leavesArray = Array.zeroCreate<Coordinate>(leaves.Count)
        leaves.CopyTo(leavesArray)

        { ShortestPathGraph = graph
          ConnectedNodes = graph.Graph.VertexCount
          Cost = cost
          FarthestFromRoot = coordinatesByDistance.Farthest
          Leaves = leavesArray }