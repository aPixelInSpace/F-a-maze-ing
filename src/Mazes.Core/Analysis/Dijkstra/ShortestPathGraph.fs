// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System
open System.Text
open QuikGraph

type Distance = int

// todo : use a AdjacencyGraph instead of BidirectionalGraph
type ShortestPathGraph<'Node> when 'Node : equality =
    {
        RootNode : 'Node
        Graph : BidirectionalGraph<'Node, TaggedEdge<'Node, Distance>>
    }

    member this.ContainsNode node =
        this.Graph.ContainsVertex(node)

    member this.AddNode node =
        this.Graph.AddVertex(node) |> ignore

    member this.ContainsEdge source target =
        this.Graph.ContainsEdge(source, target)

    member this.AddEdge source target distance =
        this.Graph.AddEdge(TaggedEdge<'Node, Distance>(source, target, distance)) |> ignore

    member this.AdjacentNodes node =
        let inNodes =
            this.Graph.InEdges(node)
            |> Seq.map(fun e -> e.Source)
        let outNodes =
            this.Graph.OutEdges(node)
            |> Seq.map(fun e -> e.Target)

        Seq.append inNodes outNodes

    member this.MinAdjacentNode node =
        if not (this.ContainsNode node) then None
        else
            let inNodes = this.Graph.InEdges(node)
            if inNodes |> Seq.isEmpty then
                None
            else
                let minNode = (inNodes |> Seq.minBy(fun n -> n.Tag))
                Some (minNode.Source, minNode.Tag)

    member this.MinAdjacentOutNode node =
        if this.Graph.ContainsVertex(node) then
            let edges = this.Graph.OutEdges(node)
            if edges |> Seq.length > 0 then
                Some (edges
                |> Seq.map(fun e -> e.Tag)
                |> Seq.min)
            else None
        else None

    member this.NodeDistanceFromRoot node =
        if node = this.RootNode then Some 0
        else
            let minAdjacentNode = this.MinAdjacentNode node
            match minAdjacentNode with
            | Some minAdjacentNode -> Some (snd minAdjacentNode)
            | None -> None

    member this.PathFromGoalToRoot goal =
        seq {
            let mutable currentNode = Some goal
            let mutable currentDistance = Int32.MaxValue

            while currentNode.IsSome do
                let currentNodeValue = currentNode.Value 
                yield currentNodeValue
                match this.MinAdjacentNode currentNodeValue with
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

    member this.ToString =
        let sBuilder = StringBuilder()

        let mutable linebreak = 1
        this.Graph.Edges
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
            Graph = BidirectionalGraph<'Node, TaggedEdge<'Node, Distance>>()
        }