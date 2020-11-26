// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

module Mazes.Core.Canvas.Shape.Ellipse

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas

type Side =
    | Inside
    | Outside

let private isEllipse rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared centerRowIndex centerColumnIndex side rowIndex columnIndex =
    let distanceRow = rowIndex - centerRowIndex
    let distanceColumn = columnIndex - centerColumnIndex

    let ellipseMathFormula =
                  float (pown distanceRow 2) / rowRadiusLengthIndexSquared
                  +
                  float (pown distanceColumn 2) / columnRadiusLengthIndexSquared

    match side with
    | Inside -> ellipseMathFormula <= 1.0
    | Outside -> ellipseMathFormula > 1.0    

let create rowRadiusLength columnRadiusLength rowEnlargingFactor columnEnlargingFactor rowTranslationFactor columnTranslationFactor side =
    let rowEnlargingFactor = rowEnlargingFactor * (float)rowRadiusLength
    let columnEnlargingFactor = columnEnlargingFactor * (float)columnRadiusLength
    
    let numberOfRows = (rowRadiusLength * 2) - 1
    let numberOfColumns = (columnRadiusLength * 2) - 1

    let centerRowIndex =  getIndex rowRadiusLength + rowTranslationFactor
    let centerColumnIndex = getIndex columnRadiusLength + columnTranslationFactor

    let rowRadiusLengthIndex = getIndex rowRadiusLength
    let rowRadiusLengthIndexSquared = float (pown rowRadiusLengthIndex 2) + rowEnlargingFactor
    
    let columnRadiusLengthIndex = getIndex columnRadiusLength
    let columnRadiusLengthIndexSquared = float (pown columnRadiusLengthIndex 2) + columnEnlargingFactor

    let zones =
        Array2D.init numberOfRows numberOfColumns
            (fun rowIndex columnIndex -> Zone.create (isEllipse rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared centerRowIndex centerColumnIndex side rowIndex columnIndex))

    { Zones = zones; }