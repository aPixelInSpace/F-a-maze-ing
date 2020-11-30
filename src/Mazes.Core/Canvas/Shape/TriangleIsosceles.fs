// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.Shape.TriangleIsosceles

open System
open Mazes.Core
open Mazes.Core.Canvas

type BaseAt =
    | Top
    | Right
    | Bottom
    | Left

let private isCellPartOfTriangleIsosceles baseAt baseDecrement heightIncrement numberOfRows numberOfColumns rowIndex columnIndex =

    let triangleIsoscelesCondition index indexLength numberOfEmptyStartingCell =
        index >= numberOfEmptyStartingCell && index < (indexLength - numberOfEmptyStartingCell)

    let (index, indexLength, numberOfEmptyStartingCell) =
        match baseAt with
        | BaseAt.Top ->
            let currentFloorIndex = rowIndex / heightIncrement
            let numberOfEmptyStartingCellRow = currentFloorIndex * baseDecrement

            (columnIndex, numberOfColumns, numberOfEmptyStartingCellRow)
        | BaseAt.Bottom ->
            let currentFloorIndex = rowIndex / heightIncrement
            let numberOfFloors = numberOfRows / heightIncrement
            let numberOfEmptyStartingCellRow = ((numberOfFloors - 1) - currentFloorIndex) * baseDecrement

            (columnIndex, numberOfColumns, numberOfEmptyStartingCellRow)
        | BaseAt.Left ->
            let currentFloorIndex = columnIndex / heightIncrement
            let numberOfEmptyStartingCellRow = currentFloorIndex * baseDecrement

            (rowIndex, numberOfRows, numberOfEmptyStartingCellRow)
        | BaseAt.Right ->
            let currentFloorIndex = columnIndex / heightIncrement
            let numberOfFloors = numberOfColumns / heightIncrement
            let numberOfEmptyStartingCellRow = ((numberOfFloors - 1) - currentFloorIndex) * baseDecrement

            (rowIndex, numberOfRows, numberOfEmptyStartingCellRow)

    triangleIsoscelesCondition index indexLength numberOfEmptyStartingCell

// todo : allow to have an array of values for baseDecrementValue and heightIncrementValue (or, even, a function)
let create baseLength baseAt baseDecrement heightIncrement =
    let height =
        (int (Math.Ceiling((float baseLength)
                           / (float (baseDecrement * 2)))))
        * heightIncrement
    
    let numberOfRows =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> height
        | BaseAt.Left | BaseAt.Right -> baseLength
    
    let numberOfColumns =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> baseLength
        | BaseAt.Left | BaseAt.Right -> height
    
    let zones =
        Array2D.init numberOfRows numberOfColumns
            (fun rowIndex columnIndex -> Zone.create (isCellPartOfTriangleIsosceles baseAt baseDecrement heightIncrement numberOfRows numberOfColumns rowIndex columnIndex))

    { Zones = zones; }