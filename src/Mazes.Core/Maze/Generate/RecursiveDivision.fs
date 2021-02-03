// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveDivision

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

type private Slice = (int * int * int * int)

// todo : refactor this
let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let slices = Stack<Slice>()

    let rndLinkHorizontalCell rndStartRIndex rndEndRIndex cIndex =
        let rIndexes =
            seq {
                for rIndex in rndStartRIndex .. rndEndRIndex - 1 do
                    // use grid.AdjacentNeighborAbstractCoordinate
                    if not (grid.IsLimitAt { RIndex = rIndex; CIndex = cIndex } { RIndex = rIndex; CIndex = cIndex + 1 }) then
                        yield rIndex
            } |> Seq.toArray

        let rndRIndex = rIndexes.[rng.Next(rIndexes.Length)]
        grid.LinkCells { RIndex = rndRIndex; CIndex = cIndex } { RIndex = rndRIndex; CIndex = cIndex + 1 }

    let sliceVertically (startRIndex, endRIndex, startCIndex, endCIndex) =
        let cIndex = rng.Next(startCIndex, endCIndex)

        let mutable rndStartRIndex = startRIndex
        let mutable rndEndRIndex = startRIndex

        for rIndex in startRIndex .. endRIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let rightCoordinate = { RIndex = rIndex; CIndex = cIndex + 1 }

            if not (grid.IsLimitAt coordinate rightCoordinate) then
                grid.UnLinkCells coordinate rightCoordinate
            else
                rndLinkHorizontalCell rndStartRIndex rndEndRIndex cIndex
                rndStartRIndex <- rndEndRIndex + 1

            rndEndRIndex <- rndEndRIndex + 1

        rndLinkHorizontalCell rndStartRIndex rndEndRIndex cIndex

        slices.Push(startRIndex, endRIndex, startCIndex, cIndex)
        slices.Push(startRIndex, endRIndex, cIndex + 1, endCIndex)

    let rndLinkVerticalCell rndStartCIndex rndEndCIndex rIndex =
        let cIndexes =
            seq {
                for cIndex in rndStartCIndex .. rndEndCIndex - 1 do
                    if not (grid.IsLimitAt { RIndex = rIndex; CIndex = cIndex } { RIndex = rIndex + 1; CIndex = cIndex }) then
                        yield cIndex
            } |> Seq.toArray

        let rndCIndex = cIndexes.[rng.Next(cIndexes.Length)]
        grid.LinkCells { RIndex = rIndex; CIndex = rndCIndex } { RIndex = rIndex + 1; CIndex = rndCIndex }

    let sliceHorizontally (startRIndex, endRIndex, startCIndex, endCIndex) =
        let rIndex = rng.Next(startRIndex, endRIndex)

        let mutable rndStartCIndex = startCIndex
        let mutable rndEndCIndex = startCIndex

        for cIndex in startCIndex .. endCIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let bottomCoordinate = { RIndex = rIndex + 1; CIndex = cIndex }

            if not (grid.IsLimitAt coordinate bottomCoordinate) then
                grid.UnLinkCells coordinate bottomCoordinate
            else
                rndLinkVerticalCell rndStartCIndex rndEndCIndex rIndex
                rndStartCIndex <- rndEndCIndex + 1

            rndEndCIndex <- rndEndCIndex + 1

        rndLinkVerticalCell rndStartCIndex rndEndCIndex rIndex

        slices.Push(startRIndex, rIndex, startCIndex, endCIndex)
        slices.Push(rIndex + 1, endRIndex, startCIndex, endCIndex)

    let cIndex = 0
    let (startRIndex, lengthRIndex) = grid.Dimension1Boundaries cIndex

    let rIndex = 0
    let (startCIndex, lengthCIndex) = grid.Dimension2Boundaries rIndex

    sliceVertically (startRIndex, lengthRIndex - 1, startCIndex, lengthCIndex - 1)

    while slices.Count > 0 do
        let (startRIndex, endRIndex, startCIndex, endCIndex) = slices.Pop()

        let deltaR = endRIndex - startRIndex
        let deltaC = endCIndex - startCIndex
        if deltaR >= 1 && deltaC >= 1 then
            if deltaR < deltaC then
                sliceVertically (startRIndex, endRIndex, startCIndex, endCIndex)
            else
                sliceHorizontally (startRIndex, endRIndex, startCIndex, endCIndex)
        else
            if startCIndex < endCIndex then
                sliceVertically (startRIndex, endRIndex, startCIndex, endCIndex)
            
            if startRIndex < endRIndex then
                sliceHorizontally (startRIndex, endRIndex, startCIndex, endCIndex)

    { Grid = grid }