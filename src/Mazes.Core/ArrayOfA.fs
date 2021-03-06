﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open System

module ArrayOfA =
    
    let getIndex number =
        number - 1

    let minD1Index =
        0

    let minD2Index =
        0

    let isFirstD1 d1Index =
        d1Index = minD1Index

    let isLastD1 d1Index d1Length =
        d1Index = (getIndex d1Length)
    
    let maxD1Index (arrayOfA : 'A[][]) =
        getIndex arrayOfA.Length

    let getD1LengthAt (arrayOfA : 'A[][]) =
        arrayOfA.Length

    let getD2LengthAt (arrayOfA : 'A[][]) d1Index =
        arrayOfA.[d1Index].Length

    let maxD2Index (arrayOfA : 'A[][]) d1Index =
        getIndex (getD2LengthAt arrayOfA d1Index)

    let isFirstD2 d2Index =
        d2Index = minD2Index

    let isLastD2 (arrayOfA : 'A[][]) d1Index d2Index =
        d2Index = maxD2Index arrayOfA d1Index

    let get (arrayOfA : 'A[][]) coordinate =
        arrayOfA.[coordinate.RIndex].[coordinate.CIndex]

    let getRIndexes (arrayOfA : 'A[][]) =
        let numberRIndex = maxD1Index arrayOfA
        seq {
            for i in 0 .. numberRIndex do
                yield i
        }

    let getCIndexes (arrayOfA : 'A[][]) =
        let last = arrayOfA |> Array.last
        seq {
            for i in 0 .. (getIndex last.Length) do
                yield i
        }

    let getItemByItem filter (arrayOfA : 'A[][]) =
        seq {
            for rIndex in 0 .. maxD1Index arrayOfA do
                for cIndex in 0 .. maxD2Index arrayOfA rIndex do
                    let item = arrayOfA.[rIndex].[cIndex]
                    let coordinate = { RIndex = rIndex; CIndex = cIndex }
                    if (filter item coordinate) then
                        yield (item, coordinate)
        }

    let getItemByItemDesc (arrayOfA : 'A[][]) filter =
        seq {
            for rIndex in maxD1Index arrayOfA .. -1 .. 0 do
                for cIndex in maxD2Index arrayOfA rIndex .. -1 .. 0 do
                    let item = arrayOfA.[rIndex].[cIndex]
                    let coordinate = { RIndex = rIndex; CIndex = cIndex }
                    if (filter item coordinate) then
                        yield (item, coordinate)
        }

    let existAt (arrayOfA : 'A[][]) coordinate =
        coordinate.RIndex >= minD1Index && coordinate.CIndex >= minD2Index &&
        coordinate.RIndex <= maxD1Index arrayOfA && coordinate.CIndex <= maxD2Index arrayOfA coordinate.RIndex

    let createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing (constructor : int -> int -> 'T) =
        let ringHeight = widthHeightRatio / (float)numberOfRings 

        let createRingCells ringNumber numberOfCellsForTheRing =
            [|
                for cellIndex in 0 .. (numberOfCellsForTheRing - 1) ->
                    constructor (ringNumber - 1) cellIndex
            |]

        let cells =
            [|
                let mutable currentNumberOfCellsForTheRing = 0

                for ringNumber in 1 .. numberOfRings ->
                    if ringNumber = 1 then
                        currentNumberOfCellsForTheRing <- numberOfCellsForCenterRing
                    else
                        let radius = ((float)ringNumber - 1.0) / (float)numberOfRings
                        let circumference = 2.0 * Math.PI * radius
                        let estimatedCellWidth = circumference / (float)currentNumberOfCellsForTheRing
                        let ratio = (int)(Math.Round(estimatedCellWidth / ringHeight, 0))
                        currentNumberOfCellsForTheRing <- currentNumberOfCellsForTheRing * ratio

                    createRingCells ringNumber currentNumberOfCellsForTheRing
            |]

        cells