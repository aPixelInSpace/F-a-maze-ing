// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Eller

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate.Kruskal

// todo : refactor this
// It has the same fundamentals problems as Sidewinder
let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    // this algorithm uses the same concept of "sets" of the Kruskal's algorithm
    let sets = Sets<Coordinate>.createEmpty

    let linkBottom current setKey addOtherBottoms =
        let linkBottom current =
            let neighbor = grid.AdjacentNeighbor current Bottom
            match neighbor with
            | Some neighbor ->
                grid.IfNotAtLimitLinkCells current neighbor
                sets.AddToSet setKey neighbor
            | None -> ()

        let set =
            sets.GetSet setKey
            |> Seq.filter(fun c -> c.RIndex = current.RIndex)
            |> Seq.toArray

        if set.Length > 0 then
            let chosen = set.[rng.Next(set.Length)]
            linkBottom chosen

            if addOtherBottoms then
                for other in set do
                    if rng.NextDouble() < 0.5 && other <> chosen then
                        linkBottom other

    let linkRight current setKey (listOfRowSetKey : HashSet<Coordinate>) =

        let linkRight neighbor setKeyNeighbor =
            if grid.IsLimitAt current neighbor then
                linkBottom current setKey true
            else
                grid.IfNotAtLimitLinkCells current neighbor
                match setKeyNeighbor with
                | Some setKeyNeighbor ->
                    listOfRowSetKey.Remove(setKeyNeighbor) |> ignore
                    sets.MergeSets setKey setKeyNeighbor
                | None ->                    
                    sets.AddToSet setKey neighbor

        let neighbor = grid.AdjacentNeighbor current Right
        match neighbor with
        | Some neighbor ->
            let setKeyNeighbor = sets.GetSetKey neighbor
            match setKeyNeighbor with
            | Some setKeyNeighbor ->
                if setKey <> setKeyNeighbor then
                    linkRight neighbor (Some setKeyNeighbor)
            | None ->
                linkRight neighbor None
                
        | None -> linkBottom current setKey true

    let lastIndex1 = grid.GetRIndexes |> Seq.last
    for index1 in grid.GetRIndexes do

        let listOfRowSetKey = HashSet<Coordinate>()
        let (startIndex2, endIndex2) = grid.Dimension2Boundaries index1

        for index2 in startIndex2 .. endIndex2 - 1 do
            let current = { RIndex = index1; CIndex = index2 }
            let setKey =
                match sets.GetSetKey current with
                | Some setKey -> setKey 
                | None ->
                    sets.AddNewSet current None
                    current

            listOfRowSetKey.Add(setKey) |> ignore

            if index1 = lastIndex1 then
                linkRight current setKey listOfRowSetKey
            else
                if rng.NextDouble() < 0.5 then
                    linkRight current setKey listOfRowSetKey

            if index2 = endIndex2 - 1 then
                for setKey in listOfRowSetKey do
                    let set =
                        sets.GetSet setKey
                        |> Seq.filter(fun c -> c.RIndex = index1)
                    linkBottom (set |> Seq.head) setKey true

    { Grid = grid }