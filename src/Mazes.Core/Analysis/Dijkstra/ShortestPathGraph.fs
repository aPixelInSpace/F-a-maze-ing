// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System
open System.Text
open QuikGraph

type Distance = int

type ShortestPathGraph<'Node> when 'Node : equality =
    {
        RootNode : 'Node
        Graph : UndirectedGraph<'Node, TaggedEdge<'Node, Distance>>
    }

    member this.ContainsNode node =
        this.Graph.ContainsVertex(node)

    member this.AddNode node =
        this.Graph.AddVertex(node) |> ignore

    member this.ContainsEdge source target =
        this.Graph.ContainsEdge(source, target) || this.Graph.ContainsEdge(target, source)

    member this.AddEdge source target distance =
        this.Graph.AddEdge(TaggedEdge<'Node, Distance>(source, target, distance)) |> ignore

    member this.RemoveEdge source target distance =
        let update node1 node2 =
            if this.Graph.ContainsEdge(node1, node2) then
                let mutable edgeRef = TaggedEdge<'Node, Distance>(node1, node2, distance)
                if this.Graph.TryGetEdge(node1, node2, &edgeRef) then
                    this.Graph.RemoveEdge(edgeRef) |> ignore

        update source target
        update target source

    member this.AdjacentEdges node =
        if not (this.ContainsNode node) then
            None
        else
            let adjacentEdges = this.Graph.AdjacentEdges(node)

            if adjacentEdges |> Seq.isEmpty then
                None
            else
                Some (adjacentEdges
                      |> Seq.map(fun e ->
                          if node = e.Target then
                            (e.Source, e.Tag)
                          else
                            (e.Target, e.Tag)))

    member this.Edge source target =
        match (this.AdjacentEdges source) with
        | Some edges -> edges |> Seq.tryFind(fun e -> (fst e) = target)
        | None -> None
        
    member this.AdjacentNodes node =
        match this.AdjacentEdges node with
        | Some edges -> Some (edges |> Seq.map(fst))
        | None -> None

    member this.ClosestAdjacentNode node =
        match this.AdjacentEdges node with
        | Some edges -> Some (edges |> Seq.minBy(snd))
        | None -> None

    member this.NodeDistanceFromRoot node =
        if node = this.RootNode then Some 0
        else
            let minAdjacentNode = this.ClosestAdjacentNode node
            match minAdjacentNode with
            | Some minAdjacentNode -> Some ((snd minAdjacentNode) + 1)
            | None -> None

    member this.PathFromGoalToRoot goal =
        seq {
            let mutable currentNode = Some goal
            let mutable currentDistance = Int32.MaxValue

            while currentNode.IsSome do
                let currentNodeValue = currentNode.Value 
                yield currentNodeValue
                match this.ClosestAdjacentNode currentNodeValue with
                | Some minNode ->
                    if snd minNode >= currentDistance then
                        currentNode <- None
                    else
                        currentNode <- Some (fst minNode)
                        currentDistance <- snd minNode
                | None -> currentNode <- None
        }

    member this.PathFromRootTo goal =
        this.PathFromGoalToRoot goal
        |> Seq.rev

    member this.ToString sort =
        let sBuilder = StringBuilder()

        let mutable linebreak = 1
        this.Graph.Edges |> Seq.sortBy(sort)
            |> Seq.iter(fun edge ->
                sBuilder.Append($"({edge.Source.ToString()})->{edge.Tag}->({edge.Target.ToString()})") |> ignore
                if linebreak % 5 = 0 then
                    sBuilder.Append('\n') |> ignore
                    linebreak <- 1
                else
                    sBuilder.Append(' ') |> ignore
                    linebreak <- linebreak + 1
                )

        sBuilder.ToString()

    static member createEmpty rootNode =
        {
            RootNode = rootNode
            Graph = UndirectedGraph<'Node, TaggedEdge<'Node, Distance>>()
        }