// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.Kruskal

open System
open System.Collections.Generic
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Maze

type Sets<'K> when 'K : equality =
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
        match key2 with
        | Some key2 -> sortedSet.Add(key2) |> ignore
        | None -> ()
        this.Container.Add(key1, sortedSet)

    member this.AddToSet keySet key =
        this.Container.Item(keySet).Add(key) |> ignore

    member this.MergeSets keySet1 keySet2 = 
        this.Container.Item(keySet1).UnionWith(this.Container.Item(keySet2))
        this.Container.Remove(keySet2) |> ignore

    member this.GetSet keySet =
        seq {
            for item in this.Container.Item(keySet) do
                yield item
        }

    member this.HeadCount =
        if this.Container |> Seq.isEmpty then
            0
        else
            (this.Container
            |> Seq.head).Value.Count

    static member createEmpty =
        { Container = Dictionary<'K, HashSet<'K>>() }

/// Randomized Kruskal's algorithm
let createMaze rngSeed ndStruct =

    let rng = Random(rngSeed)

    let possibleLinks =
        NDimensionalStructure.coordinatesPartOfMaze ndStruct
        |> Seq.collect(fun coordinate ->
            coordinate
            |> NDimensionalStructure.connectedWithNeighbors ndStruct false
            |> Seq.map(fun neighbor -> (coordinate, neighbor)))
        |> Seq.distinctBy(Utils.getKey)
        |> Seq.toArray
        |> Utils.shuffle rng

    let forests = Sets<_>.createEmpty

    let totalOfMazeCells = NDimensionalStructure.totalOfMazeCells ndStruct

    let connect = NDimensionalStructure.updateConnectionState ndStruct Open

    let i = ref 0

    while i.Value < possibleLinks.Length &&
          forests.HeadCount < totalOfMazeCells do

        let coordinate1, coordinate2 = possibleLinks.[i.Value]

        let setKey1 = forests.GetSetKey coordinate1
        let setKey2 = forests.GetSetKey coordinate2

        match setKey1, setKey2 with
        | None, None ->
            connect coordinate1 coordinate2
            forests.AddNewSet coordinate1 (Some coordinate2)
        | Some setKey, None ->
            connect coordinate1 coordinate2
            forests.AddToSet setKey coordinate2
        | None, Some setKey ->
            connect coordinate1 coordinate2
            forests.AddToSet setKey coordinate1
        | Some setKey1, Some setKey2 ->
            if setKey1 <> setKey2 then
                connect coordinate1 coordinate2
                forests.MergeSets setKey1 setKey2

        incr i

    { NDStruct = ndStruct }