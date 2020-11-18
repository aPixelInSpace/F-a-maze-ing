module Mazes.Core.Shape.Ellipse

open Mazes.Core

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

let private getCell rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared numberOfRows numberOfColumns centerRowIndex centerColumnIndex side rowIndex columnIndex =
    let isCellPartOfMaze = isEllipse rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared centerRowIndex centerColumnIndex side
    GridCell.getCellInstance numberOfRows numberOfColumns rowIndex columnIndex isCellPartOfMaze

let create rowRadiusLength columnRadiusLength rowEnlargingFactor columnEnlargingFactor rowTranslationFactor columnTranslationFactor side =
    let rowEnlargingFactor = rowEnlargingFactor * (float)rowRadiusLength
    let columnEnlargingFactor = columnEnlargingFactor * (float)columnRadiusLength
    
    let numberOfRows = (rowRadiusLength * 2) - 1
    let numberOfColumns = (columnRadiusLength * 2) - 1

    let centerRowIndex =  Grid.getIndex rowRadiusLength + rowTranslationFactor
    let centerColumnIndex = Grid.getIndex columnRadiusLength + columnTranslationFactor

    let rowRadiusLengthIndex = Grid.getIndex rowRadiusLength
    let rowRadiusLengthIndexSquared = float (pown rowRadiusLengthIndex 2) + rowEnlargingFactor
    
    let columnRadiusLengthIndex = Grid.getIndex columnRadiusLength
    let columnRadiusLengthIndexSquared = float (pown columnRadiusLengthIndex 2) + columnEnlargingFactor

    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell rowRadiusLengthIndexSquared columnRadiusLengthIndexSquared numberOfRows numberOfColumns centerRowIndex centerColumnIndex side rowIndex columnIndex)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }