// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.RecursiveDivision

open System
open System.Collections.Generic
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Structure.Grid
open Mazes.Core.Refac.Maze

type private Slice = int * int * int * int

type private RDDirection =
    | Right
    | Bottom

let private toDisposition g rdDirection =
    match gridStructure g with
    | GridStructureArray2D gridStructure ->
        match gridStructure with
        | GridArray2DOrthogonal _ ->
            match rdDirection with
            | Right -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Right)
            | Bottom -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Bottom)

// todo : refactor this, sadly it only works with rectangular orthogonal ndStruct for now
let createMaze rngSeed rooms roomsHeight roomsWidth ndStruct =

    let slice2D = snd (NDimensionalStructure.firstSlice2D ndStruct)

    let bottom = toDisposition slice2D Bottom
    let right = toDisposition slice2D Right

    let rng = Random(rngSeed)

    let slices = Stack<Slice>()

    let rndLinkVerticalCell rndStartRIndex rndEndRIndex cIndex =
        let candidates =
            seq {
                for rIndex in rndStartRIndex .. rndEndRIndex - 1 do
                    let c = { RIndex = rIndex; CIndex = cIndex }
                    match (neighbor slice2D c right) with
                    | Some n ->
                        if not (isLimitAtCoordinate slice2D c n) then
                            yield (c, n)
                    | None -> ()
            } |> Seq.toArray

        if candidates.Length > 0 then
            let c, n = candidates.[rng.Next(candidates.Length)]
            updateConnectionState slice2D Open c n

    let sliceVertically (startRIndex, endRIndex, startCIndex, endCIndex) =
        let cIndex = rng.Next(startCIndex, endCIndex)

        let mutable rndStartRIndex = startRIndex
        let mutable rndEndRIndex = startRIndex

        for rIndex in startRIndex .. endRIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let rightCoordinate = neighbor slice2D coordinate right

            match rightCoordinate with
            | Some rightCoordinate ->
                if isCellPartOfMaze slice2D coordinate && isCellPartOfMaze slice2D rightCoordinate then
                    if not (isLimitAtCoordinate slice2D coordinate rightCoordinate) then
                        updateConnectionState slice2D Close coordinate rightCoordinate
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
                    match (neighbor slice2D c bottom) with
                    | Some n ->
                        if not (isLimitAtCoordinate slice2D c n) then
                            yield (c, n)
                    | None -> ()
            } |> Seq.toArray

        if candidates.Length > 0 then
            let c, n = candidates.[rng.Next(candidates.Length)]
            updateConnectionState slice2D Open c n

    let sliceHorizontally (startRIndex, endRIndex, startCIndex, endCIndex) =
        let rIndex = rng.Next(startRIndex, endRIndex)

        let mutable rndStartCIndex = startCIndex
        let mutable rndEndCIndex = startCIndex

        for cIndex in startCIndex .. endCIndex do
            let coordinate = { RIndex = rIndex; CIndex = cIndex }
            let bottomCoordinate = neighbor slice2D coordinate bottom

            match bottomCoordinate with
            | Some bottomCoordinate ->
                if isCellPartOfMaze slice2D coordinate && isCellPartOfMaze slice2D bottomCoordinate then
                    if not (isLimitAtCoordinate slice2D coordinate bottomCoordinate) then
                        updateConnectionState slice2D Close coordinate bottomCoordinate
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
    let startRIndex, lengthRIndex = dimension1Boundaries slice2D cIndex

    let rIndex = 0
    let startCIndex, lengthCIndex = dimension2Boundaries slice2D rIndex

    sliceVertically (startRIndex, lengthRIndex - 1, startCIndex, lengthCIndex - 1)

    while slices.Count > 0 do
        let startRIndex, endRIndex, startCIndex, endCIndex = slices.Pop()

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

    { NDStruct = ndStruct }