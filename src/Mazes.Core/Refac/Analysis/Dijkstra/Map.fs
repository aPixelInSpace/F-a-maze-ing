// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Analysis.Dijkstra

open System.Collections.Generic
open Mazes.Core.Refac

[<Struct>]
type FarthestFromRoot = {
        Distance : Distance
        Coordinates : NCoordinate array
    }

type CoordinatesByDistance =
    {
        Container : Dictionary<Distance, HashSet<NCoordinate>>
    }

module CoordinatesByDistance =

    let remove c distance coordinate =
        let removeBase distance coordinate =
            if c.Container.ContainsKey(distance) then
                let distanceArray = c.Container.Item(distance)
                if distanceArray.Remove(coordinate) then
                    if distanceArray.Count = 0 then
                        c.Container.Remove(distance) |> ignore

        removeBase distance coordinate
        removeBase (distance + (Distance 1)) coordinate

    let addUpdate c distance coordinate =
        if c.Container.ContainsKey(distance) then
            let distanceSet = c.Container.Item(distance)
            distanceSet.Add(coordinate) |> ignore
        else
            let distanceSet = HashSet<_>()
            distanceSet.Add(coordinate) |> ignore
            c.Container.Add(distance, distanceSet)

    let maxDistance c =
        c.Container.Keys |> Seq.max

    let coordinatesWithDistance c distance =
        let coordinates = Array.zeroCreate<_>(c.Container.Item(distance).Count)
        c.Container.Item(distance).CopyTo(coordinates)

        coordinates

    let farthest c =
        { Distance = (maxDistance c) - (Distance 1); Coordinates = coordinatesWithDistance c (maxDistance c) }

    let createEmpty =
        { Container = Dictionary<Distance, HashSet<_>>() }

type Map =
    {
        ShortestPathGraph : ShortestPathGraph<NCoordinate>
        ConnectedNodes : int
        Cost : NCoordinate -> Cost
        FarthestFromRoot : FarthestFromRoot
        Leaves : NCoordinate array
    }

module Map =

    let create
        (linkedNeighbors : NCoordinate -> NCoordinate seq)
        (cost : NCoordinate -> Cost)
        (trackerAdd, trackerHasItems, trackerPop)
        rootCoordinate =

        let coordinatesByDistance = CoordinatesByDistance.createEmpty

        let leaves = HashSet<_>()

        trackerAdd rootCoordinate (Distance -1)

        let graph = ShortestPathGraph.createEmpty rootCoordinate
        ShortestPathGraph.addNode graph rootCoordinate

        while trackerHasItems() do

            let coordinate, currentDistance = trackerPop()

            let neighbors = linkedNeighbors coordinate |> Seq.toArray

            if (neighbors |> Seq.length) = 1 then
                leaves.Add(coordinate) |> ignore
            
            let newDistance =
                match (ShortestPathGraph.nodeDistanceFromRoot graph coordinate) with
                | Some distance -> distance + Distance (Cost.value (cost coordinate))
                | None -> currentDistance + Distance 1

            CoordinatesByDistance.addUpdate coordinatesByDistance newDistance coordinate

            for neighbor in neighbors do
                if not (ShortestPathGraph.containsNode graph neighbor) then
                    trackerAdd neighbor newDistance
                    ShortestPathGraph.addNode graph neighbor

                if not (ShortestPathGraph.containsEdge graph coordinate neighbor) then
                    ShortestPathGraph.addEdge graph coordinate neighbor newDistance
                else
                    let edge = ShortestPathGraph.edge graph coordinate neighbor
                    match edge with
                    | Some (_, distance) ->
                        if newDistance < distance then
                            trackerAdd neighbor newDistance
                            CoordinatesByDistance.remove coordinatesByDistance distance neighbor

                            ShortestPathGraph.removeEdge graph coordinate neighbor distance
                            ShortestPathGraph.addEdge graph coordinate neighbor newDistance
                    | None -> ()
        
        let leavesArray = Array.zeroCreate<_>(leaves.Count)
        leaves.CopyTo(leavesArray)

        { ShortestPathGraph = graph
          ConnectedNodes = graph.Graph.VertexCount
          Cost = cost
          FarthestFromRoot = CoordinatesByDistance.farthest coordinatesByDistance
          Leaves = leavesArray }

    let longestPaths m =
        seq {
            let adjacentNodes node =
                    match ShortestPathGraph.adjacentNodes m.ShortestPathGraph node with
                    | Some nodes -> nodes
                    | None -> Seq.empty

            for farthestCoordinate in m.FarthestFromRoot.Coordinates do
                let mapFromFarthest = create adjacentNodes m.Cost PriorityQueueTracker.createEmpty farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield ShortestPathGraph.pathFromGoalToRoot mapFromFarthest.ShortestPathGraph newFarthestCoordinate
        }