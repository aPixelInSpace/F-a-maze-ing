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
        Root : Coordinate
        Graph : Graph
        ConnectedNodes : int
        FarthestFromRoot : FarthestFromRoot
        Leaves : Coordinate array
        Graph2 : QuikGraph.BidirectionalGraph<Coordinate, QuikGraph.TaggedEdge<Coordinate, Distance>>
    }

    member this.LongestPaths =
        seq {
            for farthestCoordinate in this.FarthestFromRoot.Coordinates do
                let mapFromFarthest = Map.create this.Graph.NodeNeighbors this.Graph.NumberOfRows this.Graph.NumberOfColumns farthestCoordinate
                for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                    yield mapFromFarthest.Graph.PathFromGoalToRoot newFarthestCoordinate
        }

    static member create (linkedNeighbors : Coordinate -> Coordinate seq) numberOfRows numberOfColumns rootCoordinate =

        let coordinatesByDistance = CoordinatesByDistance.createEmpty
        let maxCoordinates = HashSet<Coordinate>()
        let mutable maxDistance = -1

        let leaves = ResizeArray<Coordinate>()

        let graph = Graph.createEmpty numberOfRows numberOfColumns
        let graph2 = QuikGraph.BidirectionalGraph<Coordinate, QuikGraph.TaggedEdge<Coordinate, Distance>>()

        let unvisited = Tracker<Coordinate, Distance, (Distance * Visited)>.createEmpty (numberOfRows * numberOfColumns)
        unvisited.Add rootCoordinate -1 (-1, false)
        graph2.AddVertex(rootCoordinate) |> ignore

        let connectedNodes = ref 0        

        while unvisited.HasItems do

            let (coordinate, (currentDistance, visited)) = unvisited.Pop

            if not visited then
                incr connectedNodes

            let neighbors = linkedNeighbors coordinate |> Seq.toArray

            if (neighbors |> Seq.length) = 1 then
                leaves.Add(coordinate)
            
            let newDistance =
                match (graph.MinDistanceNode neighbors) with
                | Some minNeighborNode -> minNeighborNode.DistanceFromRoot + 1
                | None -> currentDistance + 1

            let newDistance2 =
                if graph2.ContainsVertex(coordinate) then
                    let edges = graph2.OutEdges(coordinate)
                    if edges |> Seq.length > 0 then
                        edges
                        |> Seq.map(fun e -> e.Tag)
                        |> Seq.min
                    else
                        currentDistance + 1
                else
                    currentDistance + 1

            if newDistance2 > maxDistance then
                maxDistance <- newDistance2
                maxCoordinates.Clear()
                maxCoordinates.Add(coordinate) |> ignore
            elif newDistance2 = maxDistance then
                maxCoordinates.Add(coordinate) |> ignore                

            graph.UpdateNode coordinate (Some { DistanceFromRoot = newDistance; Neighbors = neighbors })
            for neighbor in neighbors do
                if not (graph2.ContainsVertex(neighbor)) then
                    unvisited.Add neighbor newDistance2 (newDistance2, true)
                    graph2.AddVertex(neighbor) |> ignore

                if not (graph2.ContainsEdge(neighbor, coordinate)) then
                    graph2.AddEdge(QuikGraph.TaggedEdge<Coordinate, Distance>(coordinate, neighbor, newDistance2)) |> ignore
                
            coordinatesByDistance.AddUpdate newDistance coordinate            

//            neighbors
//            |> Seq.iter(fun nCoordinate ->
//                    match (graph.Node nCoordinate) with
//                    | Some neighborNode ->
//                        if neighborNode.DistanceFromRoot >= newDistance then
//                            unvisited.Add nCoordinate newDistance (newDistance, true)
//                            coordinatesByDistance.Remove neighborNode.DistanceFromRoot nCoordinate
//                    | None ->
//                        unvisited.Add nCoordinate newDistance (newDistance, false))

        
        
        
        
//            let edges =
//                let edges = graph2.OutEdges(coordinate)
//                if edges |> Seq.length > 0 then
//                    edges
//                else
//                    Seq.empty

//            neighbors
//            |> Seq.iter(fun nCoordinate ->
//                    unvisited.Add nCoordinate newDistance2 (newDistance2, true))
                    
                    
//                    let nEdge = edges |> Seq.tryFind(fun e -> e.Target = coordinate)
//                    match nEdge with
//                    | Some nEdge ->
//                        if nEdge.Tag >= newDistance2 then
//                            unvisited.Add nCoordinate newDistance2 (newDistance2, true)
//                            coordinatesByDistance.Remove nEdge.Tag nCoordinate
//                    | None ->
//                        unvisited.Add nCoordinate newDistance2 (newDistance2, false))

        let coordinates = Array.zeroCreate<Coordinate>(maxCoordinates.Count)
        maxCoordinates.CopyTo(coordinates)
        
        { Root = rootCoordinate
          Graph = graph
          Graph2 = graph2
          ConnectedNodes = graph2.VertexCount
          FarthestFromRoot =
              { Distance = maxDistance
                Coordinates = coordinates }
          Leaves = leaves.ToArray() }