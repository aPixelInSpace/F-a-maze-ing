module Mazes.Core.Shape.TriangleIsosceles

open System
open Mazes.Core

type BaseAt =
    | Top
    | Right
    | Bottom
    | Left

let private getCell numberOfRows numberOfColumns baseAt decrementValue heightIncrementValue rowIndex columnIndex =
    
    let isCellPartOfTriangleIsosceles numberOfRows numberOfColumns baseAt baseDecrementValue heightIncrementValue rowIndex columnIndex =
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

    let isCellPartOfMaze = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns baseAt decrementValue heightIncrementValue

    GridCell.getCellInstance numberOfRows numberOfColumns rowIndex columnIndex isCellPartOfMaze

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
    
    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell numberOfRows numberOfColumns baseAt baseDecrementValue heightIncrementValue rowIndex columnIndex)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }