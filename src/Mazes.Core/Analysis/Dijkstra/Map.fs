// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System
open System.Collections.Generic
open Priority_Queue
open Mazes.Core

[<Struct>]
type FarthestFromRoot = {
        Distance : Distance
        Coordinates : Coordinate array
    }

type private FarthestFromRootTracker =
    {
        mutable MaxDistance : int
        MaxCoordinates : HashSet<Coordinate>
    }

    member this.Update distance coordinate =
        if distance > this.MaxDistance then
            this.MaxDistance <- distance
            this.MaxCoordinates.Clear()
            this.MaxCoordinates.Add(coordinate) |> ignore
        elif distance = this.MaxDistance then
            this.MaxCoordinates.Add(coordinate) |> ignore

    member this.ToFarthestFromRoot =
        let maxCoordinates = Array.zeroCreate<Coordinate>(this.MaxCoordinates.Count)
        this.MaxCoordinates.CopyTo(maxCoordinates)

        { Distance = this.MaxDistance
          Coordinates = maxCoordinates }

    static member createEmpty =
        { MaxDistance = 0; MaxCoordinates = HashSet<Coordinate>() }

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
        this.AddPayload key payload

    member this.HasItems =
        this.PriorityQueue.Count > 0

    member this.Pop =
        let key = this.PriorityQueue.Dequeue()
        let payload = this.Payloads.Item(key)
        (key, payload)

    static member createEmpty (numberOfElements : int) =
        { PriorityQueue = SimplePriorityQueue<'Key, 'Priority>()
          Payloads = Dictionary<'Key, 'Payload>(numberOfElements) }

type Map =
    {
        ShortestPathGraph : ShortestPathGraph<Coordinate>
        ConnectedNodes : int
        FarthestFromRoot : FarthestFromRoot
        Leaves : Coordinate array
    }

    member this.LongestPaths =
        seq {
            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create this.ShortestPathGraph.AdjacentNodes this.ConnectedNodes farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.ShortestPathGraph.PathFromGoalToRoot newFarthestCoordinate
        }

    static member create (linkedNeighbors : Coordinate -> Coordinate seq) totalOfMazeCells rootCoordinate =

        let farthest = FarthestFromRootTracker.createEmpty
        let leaves = ResizeArray<Coordinate>()

        let unvisitedPrQ = Tracker<Coordinate, Distance, (Distance * Visited)>.createEmpty totalOfMazeCells
        unvisitedPrQ.Add rootCoordinate -1 (-1, false)

        let graph = ShortestPathGraph.createEmpty rootCoordinate
        graph.AddNode(rootCoordinate)

        let connectedNodes = ref 0        

        while unvisitedPrQ.HasItems do

            let (coordinate, (currentDistance, visited)) = unvisitedPrQ.Pop

            if not visited then
                incr connectedNodes

            let neighbors = linkedNeighbors coordinate |> Seq.toArray

            if (neighbors |> Seq.length) = 1 then
                leaves.Add(coordinate)
            
            let newDistance =
                match (graph.MinAdjacentOutNode coordinate) with
                | Some distance -> distance + 1
                | None -> currentDistance + 1

            farthest.Update newDistance coordinate            

            for neighbor in neighbors do
                if not (graph.ContainsNode neighbor) then
                    unvisitedPrQ.Add neighbor newDistance (newDistance, true)
                    graph.AddNode(neighbor)

                if not (graph.ContainsEdge coordinate neighbor) then
                    graph.AddEdge coordinate neighbor newDistance
                //
                // this check does not seems useful but there may be some edge case...
                //
                //else
                //    let edges = graph.Graph.OutEdges(coordinate)
                //    if not (edges |> Seq.isEmpty) then
                //        match edges |> Seq.tryFind(fun e -> e.Target = neighbor) with
                //        | Some nEdge ->
                //            if nEdge.Tag > newDistance then
                //                unvisited.Add neighbor newDistance (newDistance, true)
                //                nEdge.Tag <- newDistance
                //        | None -> ()
        
        { ShortestPathGraph = graph
          ConnectedNodes = graph.Graph.VertexCount
          FarthestFromRoot = farthest.ToFarthestFromRoot
          Leaves = leaves.ToArray() }