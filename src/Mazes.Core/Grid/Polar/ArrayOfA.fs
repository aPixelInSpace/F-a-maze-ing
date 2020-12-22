// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Polar.ArrayOfA

open System
open Mazes.Core.ArrayOfA

let isFirstRing ringIndex =
    isFirstD1 ringIndex

let isLastRing ringIndex numberOfRings =
    isLastD1 ringIndex numberOfRings

let isFirstCellOfRing cellIndex =
    isFirstD2 cellIndex

let isLastCellOfRing (arrayOfA : 'A[][]) ringIndex cellIndex =
    isLastD2 arrayOfA ringIndex cellIndex

let minRingIndex =
    minD1Index

let minCellIndex =
    minD2Index

let numberOfRings (arrayOfA : 'A[][]) =
    getD1LengthAt arrayOfA

let maxRingIndex arrayOfA =
    maxD1Index arrayOfA

let maxCellsIndex (arrayOfA : 'A[][]) ringIndex =
    maxD2Index arrayOfA ringIndex

let getNumberOfCellsAt (arrayOfA : 'A[][]) ringIndex =
    getD2LengthAt arrayOfA ringIndex

let create numberOfRings widthHeightRatio numberOfCellsForCenterRing (constructor : int -> int -> 'T) =
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