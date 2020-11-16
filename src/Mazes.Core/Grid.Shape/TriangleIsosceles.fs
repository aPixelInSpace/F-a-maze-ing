module Mazes.Core.Grid.Shape.TriangleIsosceles

open System
open Mazes.Core.Cell
open Mazes.Core.Grid.Grid

type BaseAt =
    | Top
    | Right
    | Bottom
    | Left

let private isCellPartOfTriangleIsosceles numberOfRows numberOfColumns baseAt baseDecrementValue heightIncrementValue rowIndex columnIndex =
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

let private getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles =
    match isCurrentCellPartOfTriangleIsosceles with
    | true -> Border
    | false -> Empty

let private getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isOtherCellPartOfTriangleIsosceles =
    if not isCurrentCellPartOfTriangleIsosceles && not isOtherCellPartOfTriangleIsosceles then Empty
    elif not isCurrentCellPartOfTriangleIsosceles && isOtherCellPartOfTriangleIsosceles then Border
    elif isCurrentCellPartOfTriangleIsosceles && not isOtherCellPartOfTriangleIsosceles then Border
    else Normal

let private getCell numberOfRows numberOfColumns baseAt decrementValue heightIncrementValue rowIndex columnIndex =
    let isCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns baseAt decrementValue heightIncrementValue
    
    let isCurrentCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles rowIndex columnIndex
    
    let cellType =
        match isCurrentCellPartOfTriangleIsosceles with
        | true -> PartOfMaze
        | false -> NotPartOfMaze
    
    let wallTypeTop =
        if rowIndex = 0 then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isTopCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles (rowIndex - 1) columnIndex

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isTopCellPartOfTriangleIsosceles

    let wallTypeRight =
        let isLastColumn = columnIndex = (numberOfColumns - 1)
        if isLastColumn then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isRightCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles rowIndex (columnIndex + 1)

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isRightCellPartOfTriangleIsosceles

    let wallTypeBottom =
        let isLastRow = rowIndex = (numberOfRows - 1)
        if isLastRow then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isBottomCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles (rowIndex + 1) columnIndex

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isBottomCellPartOfTriangleIsosceles

    let wallTypeLeft =
        if columnIndex = 0 then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isLeftCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles rowIndex (columnIndex - 1)

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isLeftCellPartOfTriangleIsosceles

    {
        CellType = cellType
        WallTop = { WallType = wallTypeTop; WallPosition = WallPosition.Top }
        WallRight = { WallType = wallTypeRight; WallPosition = WallPosition.Right }
        WallBottom = { WallType = wallTypeBottom; WallPosition = WallPosition.Bottom }
        WallLeft = { WallType = wallTypeLeft; WallPosition = WallPosition.Left }
    }

let create baseLength baseAt baseDecrementValue heightIncrementValue =
    let denominator = baseDecrementValue * 2
    let height = ((int (Math.Ceiling((float baseLength) / (float denominator)))) * heightIncrementValue)
    
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