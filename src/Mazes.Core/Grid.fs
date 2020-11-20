namespace Mazes.Core

open Mazes.Core

type Grid = {
    Cells : Cell[,]
    NumberOfRows : int
    NumberOfColumns : int
}

// todo : replace singles rowIndex and columnIndex with GridCoordinate
type GridCoordinate = {    
    RowIndex : int
    ColumnIndex : int
}

module Grid =

    let hasCells grid =
        grid.Cells.Length > 0

    let getIndex number =
        number - 1

    let minRowIndex =
        0

    let minColumnIndex =
        0

    let maxRowIndex grid =
        getIndex grid.NumberOfRows

    let maxColumnIndex grid =
        getIndex grid.NumberOfColumns

    let isFirstRow rowIndex =
        rowIndex = minRowIndex

    let isLastRow rowIndex numberOfRows =
        rowIndex = (getIndex numberOfRows) 

    let isFirstColumn columnIndex =
        columnIndex = minColumnIndex

    let isLastColumn columnIndex numberOfColumns =
        columnIndex = (getIndex numberOfColumns) 

    let existAt rowIndex columnIndex grid =
            minRowIndex <= rowIndex &&
            rowIndex <= (maxRowIndex grid) &&
            minColumnIndex <= columnIndex &&
            columnIndex <= (maxColumnIndex grid)

    let getCell rowIndex columnIndex grid =
        grid.Cells.[rowIndex, columnIndex]