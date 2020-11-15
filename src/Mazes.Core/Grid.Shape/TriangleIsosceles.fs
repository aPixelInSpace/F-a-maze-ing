module Mazes.Core.Grid.Shape.TriangleIsosceles

open System
open Mazes.Core.Cell
open Mazes.Core.Grid.Grid

type BaseAt =
    | Top
    | Right
    | Bottom
    | Left

let private isCellPartOfTriangleIsosceles numberOfRows numberOfColumns rowIndex columnIndex baseAt decrementValue =
    match baseAt with
    | BaseAt.Top ->
        let numberOfEmptyStartingCell = rowIndex * decrementValue
        columnIndex >= numberOfEmptyStartingCell && columnIndex < (numberOfColumns - numberOfEmptyStartingCell)
    | BaseAt.Bottom ->
        let numberOfEmptyStartingCell = (numberOfRows - 1 - rowIndex) * decrementValue
        columnIndex >= numberOfEmptyStartingCell && columnIndex < (numberOfColumns - numberOfEmptyStartingCell)
    | BaseAt.Left ->
        let numberOfEmptyStartingCell = columnIndex * decrementValue
        rowIndex >= numberOfEmptyStartingCell && rowIndex < (numberOfRows - numberOfEmptyStartingCell)
    | BaseAt.Right ->
        let numberOfEmptyStartingCell = (numberOfColumns - 1 - columnIndex) * decrementValue
        rowIndex >= numberOfEmptyStartingCell && rowIndex < (numberOfRows - numberOfEmptyStartingCell)

let private getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles =
    match isCurrentCellPartOfTriangleIsosceles with
    | true -> Border
    | false -> Empty

let private getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isOtherCellPartOfTriangleIsosceles =
    if not isCurrentCellPartOfTriangleIsosceles && not isOtherCellPartOfTriangleIsosceles then Empty
    elif not isCurrentCellPartOfTriangleIsosceles && isOtherCellPartOfTriangleIsosceles then Border
    elif isCurrentCellPartOfTriangleIsosceles && not isOtherCellPartOfTriangleIsosceles then Border
    else Normal

let private getCell numberOfRows numberOfColumns rowIndex columnIndex baseAt decrementValue =
    let isCurrentCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns rowIndex columnIndex baseAt decrementValue
    
    let cellType =
        match isCurrentCellPartOfTriangleIsosceles with
        | true -> PartOfMaze
        | false -> NotPartOfMaze
    
    let wallTypeTop =
        if rowIndex = 0 then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isTopCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns (rowIndex - 1) columnIndex baseAt decrementValue

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isTopCellPartOfTriangleIsosceles

    let wallTypeRight =
        let isLastColumn = columnIndex = (numberOfColumns - 1)
        if isLastColumn then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isRightCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns rowIndex (columnIndex + 1) baseAt decrementValue

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isRightCellPartOfTriangleIsosceles

    let wallTypeBottom =
        let isLastRow = rowIndex = (numberOfRows - 1)
        if isLastRow then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isBottomCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns (rowIndex + 1) columnIndex baseAt decrementValue

            getBorderOrNormalOrEmptyWall isCurrentCellPartOfTriangleIsosceles isBottomCellPartOfTriangleIsosceles

    let wallTypeLeft =
        if columnIndex = 0 then
            getBorderOrEmptyWall isCurrentCellPartOfTriangleIsosceles
        else
            let isLeftCellPartOfTriangleIsosceles = isCellPartOfTriangleIsosceles numberOfRows numberOfColumns rowIndex (columnIndex - 1) baseAt decrementValue

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
    let height = (int (Math.Ceiling((float baseLength) / (float denominator))))
    
    let numberOfRows =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> height
        | BaseAt.Left | BaseAt.Right -> baseLength
    
    let numberOfColumns =
        match baseAt with
        | BaseAt.Top | BaseAt.Bottom -> baseLength
        | BaseAt.Left | BaseAt.Right -> height
    
    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell numberOfRows numberOfColumns rowIndex columnIndex baseAt baseDecrementValue)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }