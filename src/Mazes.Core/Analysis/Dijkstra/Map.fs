// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System
open System.Collections.Generic
open Priority_Queue
open Mazes.Core

type private Visited = bool

type Tracker<'Key, 'Priority, 'Payload when 'Key : equality and 'Priority :> IComparable<'Priority>> =
    {
        PriorityQueue : SimplePriorityQueue<'Key, 'Priority>
        Payloads : Dictionary<'Key, 'Payload>
    }
    
    member private this.AddQueue key priority =
        if this.PriorityQueue.Contains(key) then
            this.PriorityQueue.UpdatePriority(key, priority)
        else
            this.PriorityQueue.Enqueue(key, priority)

    member private this.AddPayload key payload =        
        if this.Payloads.ContainsKey(key) then
            this.Payloads.Item(key) <- payload
        else
            this.Payloads.Add(key, payload)

    member this.Add key priority payload =
        this.AddQueue key priority
        this.AddPayload key (payload)

    member this.HasItems =
        this.PriorityQueue.Count > 0

    member this.Pop =
        let key = this.PriorityQueue.Dequeue()
        let payload = this.Payloads.Item(key)
        (key, payload)

    static member createEmpty =
        { PriorityQueue = SimplePriorityQueue<'Key, 'Priority>()
          Payloads = Dictionary<'Key, 'Payload>() }

type Map =
    {
        Root : Coordinate
        Graph : Graph
        ConnectedNodes : int
        FarthestFromRoot : FarthestFromRoot
    }

    member this.LongestPaths =
        seq {
            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create this.Graph.NodeNeighbors this.Graph.NumberOfRows this.Graph.NumberOfColumns farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.Graph.PathFromGoalToRoot newFarthestCoordinate
        }

    static member create getNeighborsCoordinate numberOfRows numberOfColumns rootCoordinate =

        let coordinatesByDistance = CoordinatesByDistance.Create

        let graph = Graph.createEmpty numberOfRows numberOfColumns

        let unvisited = Tracker<Coordinate, Distance, (Distance * Visited)>.createEmpty
        unvisited.Add rootCoordinate -1 (-1, false)        

        let connectedNodes = ref 0
        let counter = ref 0

        while unvisited.HasItems do
            incr counter

            let (coordinate, (currentDistance, visited)) = unvisited.Pop

            if not visited then
                incr connectedNodes

            let neighbors = getNeighborsCoordinate coordinate
            
            let newDistance =
                match (graph.MinDistanceNode neighbors) with
                | Some minNeighborNode -> minNeighborNode.DistanceFromRoot + 1
                | None -> currentDistance + 1

            graph.UpdateNode coordinate (Some { DistanceFromRoot = newDistance; Neighbors = neighbors })

            coordinatesByDistance.AddUpdate newDistance coordinate

            neighbors
            |> Seq.iter(fun nCoordinate ->
                    match (graph.Node nCoordinate) with
                    | Some neighborNode ->
                        if neighborNode.DistanceFromRoot >= newDistance then
                            unvisited.Add nCoordinate newDistance (newDistance, true)
                            coordinatesByDistance.Remove neighborNode.DistanceFromRoot nCoordinate
                    | None ->
                        unvisited.Add nCoordinate newDistance (newDistance, false))

        { Root = rootCoordinate
          Graph = graph
          ConnectedNodes = connectedNodes.Value
          FarthestFromRoot =
              { Distance = coordinatesByDistance.MaxDistance
                Coordinates = coordinatesByDistance.CoordinatesWithDistance(coordinatesByDistance.MaxDistance) } }