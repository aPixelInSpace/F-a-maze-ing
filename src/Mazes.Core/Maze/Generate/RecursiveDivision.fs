// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveDivision

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

type private Slice = (int * int * int * int)

// todo : refactor this, sadly it only works with rectangular orthogonal grid for now
let createMaze rngSeed rooms roomsHeight roomsWidth (grid : GridNew.IGrid<_>) : MazeNew.MazeNew<_> =

    let rng = Random(rngSeed)

    let slices = Stack<Slice>()

    let rndLinkVerticalCell rndStartRIndex rndEndRIndex cIndex =
        let candidates =
            seq {
                for rIndex in rndStartRIndex .. rndEndRIndex - 1 do
                    let c = { RIndex = rIndex; CIndex = cIndex }
                    match (grid.AdjacentNeighbor c Right) with
                    | Some n ->
                        if not (grid.IsLimitAt c n) then
                            yield (c, n)
                    | None -> ()
            } |> Seq.toArray

        if candidates.Length > 0 then
            let (c, n) = candidates.[rng.Next(candidates.Length)]
            grid.UpdateConnection Open c n

    let sliceVertically (startRIndex, endRIndex, startCIndex, endCIndex) =
        let cIndex = rng.Next(startCIndex, endCIndex)

        let mutable rndStartRIndex = startRIndex
        let mutable rndEndRIndex = startRIndex

        for rIndex in startRIndex .. endRIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let rightCoordinate = grid.AdjacentNeighbor coordinate Right

            match rightCoordinate with
            | Some rightCoordinate ->
                if grid.IsCellPartOfMaze coordinate && grid.IsCellPartOfMaze rightCoordinate then
                    if not (grid.IsLimitAt coordinate rightCoordinate) then
                        grid.UpdateConnection Close coordinate rightCoordinate
                    else
                        rndLinkVerticalCell rndStartRIndex rndEndRIndex cIndex
                        rndStartRIndex <- rndEndRIndex + 1
                else
                    rndLinkVerticalCell rndStartRIndex (rndEndRIndex - 1) cIndex
                    rndStartRIndex <- rndEndRIndex + 1
            | None ->
                rndStartRIndex <- rndEndRIndex + 1

            rndEndRIndex <- rndEndRIndex + 1

        rndLinkVerticalCell rndStartRIndex rndEndRIndex cIndex

        slices.Push(startRIndex, endRIndex, startCIndex, cIndex)
        slices.Push(startRIndex, endRIndex, cIndex + 1, endCIndex)

    let rndLinkHorizontalCell rndStartCIndex rndEndCIndex rIndex =
        let candidates =
            seq {
                for cIndex in rndStartCIndex .. rndEndCIndex - 1 do
                    let c = { RIndex = rIndex; CIndex = cIndex }
                    match (grid.AdjacentNeighbor c Bottom) with
                    | Some n ->
                        if not (grid.IsLimitAt c n) then
                            yield (c, n)
                    | None -> ()
            } |> Seq.toArray

        if candidates.Length > 0 then
            let (c, n) = candidates.[rng.Next(candidates.Length)]
            grid.UpdateConnection Open c n

    let sliceHorizontally (startRIndex, endRIndex, startCIndex, endCIndex) =
        let rIndex = rng.Next(startRIndex, endRIndex)

        let mutable rndStartCIndex = startCIndex
        let mutable rndEndCIndex = startCIndex

        for cIndex in startCIndex .. endCIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let bottomCoordinate = grid.AdjacentNeighbor coordinate Bottom

            match bottomCoordinate with
            | Some bottomCoordinate ->
                if grid.IsCellPartOfMaze coordinate && grid.IsCellPartOfMaze bottomCoordinate then
                    if not (grid.IsLimitAt coordinate bottomCoordinate) then
                        grid.UpdateConnection Close coordinate bottomCoordinate
                    else
                        rndLinkHorizontalCell rndStartCIndex rndEndCIndex rIndex
                        rndStartCIndex <- rndEndCIndex + 1
                else
                    rndLinkHorizontalCell rndStartCIndex (rndEndCIndex - 1) rIndex
                    rndStartCIndex <- rndEndCIndex + 1
            | None ->
                rndStartCIndex <- rndEndCIndex + 1

            rndEndCIndex <- rndEndCIndex + 1

        rndLinkHorizontalCell rndStartCIndex rndEndCIndex rIndex

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
            if not (rng.NextDouble() < rooms && deltaR <= roomsHeight && deltaC <= roomsWidth) then
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