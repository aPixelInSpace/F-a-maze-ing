// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System.Text
open Priority_Queue
open Mazes.Core
open Mazes.Core.Array2D

type Graph =
    {
        Nodes : (Node option)[,]
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

    member this.UpdateNode coordinate node =
        this.Nodes.[coordinate.RowIndex, coordinate.ColumnIndex] <- node

    member this.NodeNeighbors coordinate =
        match (this.Node coordinate) with
        | Some node -> node.Neighbors
        | None -> Seq.empty

    member this.MinDistanceNode (coordinates : Coordinate seq) =
        let someNodes =
            coordinates
            |> Seq.fold(fun (filtered : SimplePriorityQueue<Node, Distance>) coordinate ->
                    match (this.Node coordinate) with
                    | Some node -> filtered.Enqueue(node, node.DistanceFromRoot)
                    | None -> ()
                    filtered
                ) (SimplePriorityQueue<Node, Distance>())

        if someNodes.Count > 0 then
            Some someNodes.First
        else
            None

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

    member this.ToString =
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
        this.Nodes
            |> extractByRows
            |> Seq.iter(fun rowNodes -> appendRow sBuilder rowNodes)

        sBuilder.ToString()

    static member createEmpty numberOfRows numberOfColumns =
        { Nodes = Array2D.create numberOfRows numberOfColumns None }