module Mazes.Core.Shape.TriangleIsosceles

open System
open Mazes.Core

type BaseAt =
    | Top
    | Right
    | Bottom
    | Left

let private getCell baseAt decrementValue heightIncrementValue numberOfRows numberOfColumns rowIndex columnIndex =
    
    let isCellPartOfTriangleIsosceles baseAt baseDecrementValue heightIncrementValue numberOfRows numberOfColumns rowIndex columnIndex =
        match baseAt with
        | BaseAt.Top ->
            let currentFloorIndex = rowIndex / heightIncrementValue
            let numberOfEmptyStartingCellRow = currentFloorIndex * baseDecrementValue

            columnIndex >= numberOfEmptyStartingCellRow && columnIndex < (numberOfColumns - numberOfEmptyStartingCellRow)

        | BaseAt.Bottom ->
            let numberOfFloors = numberOfRows / heightIncrementValue
            let currentFloorIndex = rowIndex / heightIncrementValue
            let numberOfEmptyStartingCellRow = (numberOfFloors - 1 - currentFloorIndex) * baseDecrementValue

            columnIndex >= numberOfEmptyStartingCellRow && columnIndex < (numberOfColumns - numberOfEmptyStartingCellRow)

        | BaseAt.Left ->
            let currentFloorIndex = columnIndex / heightIncrementValue
            let numberOfEmptyStartingCellRow = currentFloorIndex * baseDecrementValue

            rowIndex >= numberOfEmptyStartingCellRow && rowIndex < (numberOfRows - numberOfEmptyStartingCellRow)

        | BaseAt.Right ->
            let numberOfFloors = numberOfColumns / heightIncrementValue
            let currentFloorIndex = columnIndex / heightIncrementValue
            let numberOfEmptyStartingCellRow = (numberOfFloors - 1 - currentFloorIndex) * baseDecrementValue

            rowIndex >= numberOfEmptyStartingCellRow && rowIndex < (numberOfRows - numberOfEmptyStartingCellRow)

    let isCellPartOfMaze = isCellPartOfTriangleIsosceles baseAt decrementValue heightIncrementValue numberOfRows numberOfColumns

    GridCell.getCellInstance numberOfRows numberOfColumns rowIndex columnIndex isCellPartOfMaze

// todo : allow to have an array of values for baseDecrementValue and heightIncrementValue (or, even, a function)
let create baseLength baseAt baseDecrementValue heightIncrementValue =
    let height =
        (int (Math.Ceiling((float baseLength)
                           / (float (baseDecrementValue * 2)))))
        * heightIncrementValue
    
    let numberOfRows =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> height
        | BaseAt.Left | BaseAt.Right -> baseLength
    
    let numberOfColumns =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> baseLength
        | BaseAt.Left | BaseAt.Right -> height
    
    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell baseAt baseDecrementValue heightIncrementValue numberOfRows numberOfColumns rowIndex columnIndex)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }