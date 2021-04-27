// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.Eller

open System
open System.Collections.Generic
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Structure.Grid
open Mazes.Core.Refac.Maze.Generate.Kruskal
open Mazes.Core.Refac.Maze

type private EllerDirection =
    | Right
    | Bottom

let private toDisposition g ellerDirection =
    match gridStructure g with
    | GridStructureArray2D gridStructure ->
        match gridStructure with
        | GridArray2DOrthogonal _ ->
            match ellerDirection with
            | Right -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Right)
            | Bottom -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Bottom)

// todo : refactor this
// It has the same fundamentals problems as Sidewinder
let createMaze rngSeed ndStruct =

    let slice2D = snd (NDimensionalStructure.firstSlice2D ndStruct)

    let bottom = toDisposition slice2D Bottom
    let right = toDisposition slice2D Right

    let rng = Random(rngSeed)

    // this algorithm uses the same concept of "sets" of the Kruskal's algorithm
    let sets = Sets<Coordinate2D>.createEmpty

    let linkBottom current setKey addOtherBottoms =
        let linkBottom current =
            let neighbor = neighbor slice2D current bottom
            match neighbor with
            | Some neighbor ->
                ifNotAtLimitUpdateConnectionState slice2D Open current neighbor
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

    let linkRight current setKey (listOfRowSetKey : HashSet<Coordinate2D>) =

        let linkRight neighbor setKeyNeighbor =
            if isLimitAtCoordinate slice2D current neighbor then
                linkBottom current setKey true
            else
                ifNotAtLimitUpdateConnectionState slice2D Open current neighbor
                match setKeyNeighbor with
                | Some setKeyNeighbor ->
                    listOfRowSetKey.Remove(setKeyNeighbor) |> ignore
                    sets.MergeSets setKey setKeyNeighbor
                | None ->                    
                    sets.AddToSet setKey neighbor

        let neighbor = neighbor slice2D current right
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

    let lastIndex1 = rIndexes slice2D |> Seq.last
    for index1 in rIndexes slice2D do

        let listOfRowSetKey = HashSet<Coordinate2D>()
        let startIndex2, endIndex2 = dimension2Boundaries slice2D index1

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

    { NDStruct = ndStruct }