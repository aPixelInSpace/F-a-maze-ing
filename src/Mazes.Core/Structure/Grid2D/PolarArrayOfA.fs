// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Structure.Grid2D.PolarArrayOfA

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

let getCell (arrayOfA : 'A[][]) coordinate =
    get arrayOfA coordinate

let getNumberOfCellsAt (arrayOfA : 'A[][]) ringIndex =
    getD2LengthAt arrayOfA ringIndex

let getCellByCell filter (arrayOfA : 'A[][]) =
    arrayOfA |> getItemByItem filter

let getRingByRing (arrayOfA : 'A[][]) =
    seq {
        for rIndex in 0 .. maxD1Index arrayOfA do
            yield arrayOfA.[rIndex]
    }