// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Kruskal

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

type private Sets<'K> when 'K : equality =
    private
        {
            Container : Dictionary<'K, HashSet<'K>>
        }

    member this.GetSetKey key =
        let kv =
            this.Container
            |> Seq.tryFind(fun kv -> kv.Value.Contains(key))

        match kv with
        | Some kv -> Some kv.Key
        | None -> None

    member this.AddNewSet key1 key2 =
        let sortedSet = HashSet<'K>()
        sortedSet.Add(key1) |> ignore
        sortedSet.Add(key2) |> ignore
        this.Container.Add(key1, sortedSet)

    member this.AddToSet keySet key =
        this.Container.Item(keySet).Add(key) |> ignore

    member this.MergeSets keySet1 keySet2 = 
        this.Container.Item(keySet1).UnionWith(this.Container.Item(keySet2))
        this.Container.Remove(keySet2) |> ignore

    member this.HeadCount =
        if this.Container |> Seq.isEmpty then
            0
        else
            (this.Container
            |> Seq.head).Value.Count

    static member createEmpty =
        { Container = Dictionary<'K, HashSet<'K>>() }

/// Randomized Kruskal's algorithm
let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let possibleLinks =
        grid.CoordinatesPartOfMaze
        |> Seq.collect(fun coordinate ->
            coordinate
            |> grid.NotLinkedNeighbors
            |> Seq.map(fun neighbor -> (coordinate, neighbor)))
        |> Seq.distinctBy(Utils.getKey)
        |> Seq.toArray
        |> Utils.shuffle rng

    let sets = Sets<Coordinate>.createEmpty

    let totalOfMazeCells = grid.TotalOfMazeCells

    let i = ref 0

    while i.Value < possibleLinks.Length &&
          sets.HeadCount < totalOfMazeCells do

        let (coordinate1, coordinate2) = possibleLinks.[i.Value]

        let setKey1 = sets.GetSetKey coordinate1
        let setKey2 = sets.GetSetKey coordinate2

        match setKey1, setKey2 with
        | None, None ->
            grid.LinkCells coordinate1 coordinate2
            sets.AddNewSet coordinate1 coordinate2
        | Some setKey, None ->
            grid.LinkCells coordinate1 coordinate2
            sets.AddToSet setKey coordinate2
        | None, Some setKey ->
            grid.LinkCells coordinate1 coordinate2
            sets.AddToSet setKey coordinate1
        | Some setKey1, Some setKey2 ->
            if setKey1 <> setKey2 then
                grid.LinkCells coordinate1 coordinate2
                sets.MergeSets setKey1 setKey2

        incr i

    { Grid = grid }