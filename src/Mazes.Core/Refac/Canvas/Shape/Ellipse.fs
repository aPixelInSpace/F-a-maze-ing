﻿// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Canvas.Array2D.Shape.Ellipse

open Mazes.Core.Refac.Array2D
open Mazes.Core.Refac.Canvas

type Side =
    | Inside
    | Outside

let create rowRadiusLength columnRadiusLength rowEnlargingFactor columnEnlargingFactor rowTranslationFactor columnTranslationFactor ellipseFactor side =
    let rowEnlargingFactor = rowEnlargingFactor * float rowRadiusLength
    let columnEnlargingFactor = columnEnlargingFactor * float columnRadiusLength
    
    let numberOfRows = (rowRadiusLength * 2) - 1
    let numberOfColumns = (columnRadiusLength * 2) - 1

    let centerRowIndex =  getIndex rowRadiusLength + rowTranslationFactor
    let centerColumnIndex = getIndex columnRadiusLength + columnTranslationFactor

    let rowRadiusLengthIndex = getIndex rowRadiusLength
    let rowRadiusLengthIndexSquared = float (pown rowRadiusLengthIndex 2) + rowEnlargingFactor
    
    let columnRadiusLengthIndex = getIndex columnRadiusLength
    let columnRadiusLengthIndexSquared = float (pown columnRadiusLengthIndex 2) + columnEnlargingFactor

    let isEllipse rowIndex columnIndex =
        let distanceRow = rowIndex - centerRowIndex
        let distanceColumn = columnIndex - centerColumnIndex

        let ellipseMathFormula =
            float (pown distanceRow 2) / rowRadiusLengthIndexSquared
            +
            float (pown distanceColumn 2) / columnRadiusLengthIndexSquared

        let ellipseMathFormulaPart, factorPart =
            match side with
            | Inside ->
                let ellipseMathFormulaPart = ellipseMathFormula <= 1.0
                let factorPart =
                    match ellipseFactor with
                    | Some factor -> ellipseMathFormula >= factor
                    | None -> true
                (ellipseMathFormulaPart, factorPart)
            | Outside ->
                let ellipseMathFormulaPart = ellipseMathFormula > 1.0
                let factorPart =
                    match ellipseFactor with
                    | Some factor -> ellipseMathFormula <= factor
                    | None -> true
                (ellipseMathFormulaPart, factorPart)

        ellipseMathFormulaPart && factorPart
    
    CanvasArray2D.create numberOfRows numberOfColumns isEllipse 