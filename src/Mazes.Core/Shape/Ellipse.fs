module Mazes.Core.Shape.Ellipse

open Mazes.Core

let isInside rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared centerRowIndex centerColumnIndex rowIndex columnIndex =
    let distanceRow = rowIndex - centerRowIndex
    let distanceColumn = columnIndex - centerColumnIndex

    let ellipseFormula =
                  float (pown distanceRow 2) / rowRadiusLengthIndexSquared
                  +
                  float (pown distanceColumn 2) / columnRadiusLengthIndexSquared

    ellipseFormula <= 1.0

let private getCell rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared numberOfRows numberOfColumns centerRowIndex centerColumnIndex rowIndex columnIndex =
    let isCellPartOfMaze = isInside rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared centerRowIndex centerColumnIndex
    GridCell.getCellInstance numberOfRows numberOfColumns rowIndex columnIndex isCellPartOfMaze

let create rowRadiusLength columnRadiusLength =
    let rowEnlargingFactor = 0.0 * (float)rowRadiusLength
    let columnEnlargingFactor = 0.0 * (float)columnRadiusLength
    let rowTranslationFactor = 0
    let columnTranslationFactor = 0

    let numberOfRows = (rowRadiusLength * 2) - 1
    let numberOfColumns = (columnRadiusLength * 2) - 1

    let centerRowIndex =  Grid.getIndex rowRadiusLength + rowTranslationFactor
    let centerColumnIndex = Grid.getIndex columnRadiusLength + columnTranslationFactor

    let rowRadiusLengthIndex = Grid.getIndex rowRadiusLength
    let rowRadiusLengthIndexSquared = float (pown rowRadiusLengthIndex 2) + rowEnlargingFactor
    
    let columnRadiusLengthIndex = Grid.getIndex columnRadiusLength
    let columnRadiusLengthIndexSquared = float (pown columnRadiusLengthIndex 2) + columnEnlargingFactor

    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared numberOfRows numberOfColumns centerRowIndex centerColumnIndex rowIndex columnIndex)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }