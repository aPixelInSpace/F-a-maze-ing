// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Analysis.Dijkstra

open System
open System.Text
open QuikGraph

type Distance = Distance of int
//    with
//        override this.Equals o =
//             match o with
//             | :? Distance as o -> (this = o)
//             | _ -> false

type ShortestPathGraph<'Node> when 'Node : equality =
    private
        {
            RootNode : 'Node
            Graph : UndirectedGraph<'Node, TaggedEdge<'Node, Distance>>
        }

module ShortestPathGraphM =

    let containsNode g node =
        g.Graph.ContainsVertex(node)

    let addNode g node =
        g.Graph.AddVertex(node) |> ignore

    let containsEdge g source target =
        g.Graph.ContainsEdge(source, target) || g.Graph.ContainsEdge(target, source)

    let addEdge g source target distance =
        g.Graph.AddEdge(TaggedEdge<'Node, Distance>(source, target, distance)) |> ignore

    let removeEdge g source target distance =
        let update node1 node2 =
            if g.Graph.ContainsEdge(node1, node2) then
                let mutable edgeRef = TaggedEdge<'Node, Distance>(node1, node2, distance)
                if g.Graph.TryGetEdge(node1, node2, &edgeRef) then
                    g.Graph.RemoveEdge(edgeRef) |> ignore

        update source target
        update target source

    let adjacentEdges g node =
        if not (containsNode g node) then
            None
        else
            let adjacentEdges = g.Graph.AdjacentEdges(node)

            if adjacentEdges |> Seq.isEmpty then
                None
            else
                Some (adjacentEdges
                      |> Seq.map(fun e ->
                          if node = e.Target then
                            (e.Source, e.Tag)
                          else
                            (e.Target, e.Tag)))

    let edge g source target =
        match (adjacentEdges g source) with
        | Some edges -> edges |> Seq.tryFind(fun e -> (fst e) = target)
        | None -> None
        
    let adjacentNodes g node =
        match adjacentEdges g node with
        | Some edges -> Some (edges |> Seq.map(fst))
        | None -> None

    let closestAdjacentNode g node =
        match adjacentEdges g node with
        | Some edges -> Some (edges |> Seq.minBy(snd))
        | None -> None

    let nodeDistanceFromRoot g node =
        if node = g.RootNode then Some (Distance 0)
        else
            let minAdjacentNode = closestAdjacentNode g node
            match minAdjacentNode with
            | Some minAdjacentNode -> Some (snd minAdjacentNode)
            | None -> None

    let pathFromGoalToRoot g goal =
        seq {
            let mutable currentNode = Some goal
            let mutable currentDistance = Distance Int32.MaxValue

            while currentNode.IsSome do
                let currentNodeValue = currentNode.Value 
                yield currentNodeValue
                match closestAdjacentNode g currentNodeValue with
                | Some minNode ->
                    if snd minNode >= currentDistance then
                        currentNode <- None
                    else
                        currentNode <- Some (fst minNode)
                        currentDistance <- snd minNode
                | None -> currentNode <- None
        }

    let pathFromRootTo g goal =
        pathFromGoalToRoot g goal
        |> Seq.rev

    let toString g sort =
        let sBuilder = StringBuilder()

        let mutable linebreak = 1
        g.Graph.Edges |> Seq.sortBy(sort)
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

    let createEmpty rootNode =
        {
            RootNode = rootNode
            Graph = UndirectedGraph<'Node, TaggedEdge<'Node, Distance>>()
        }