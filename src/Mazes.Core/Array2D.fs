// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

module Array2D =
    
    type ExtractBy =
        | RowsAscendingColumnsAscending
        | RowsAscendingColumnsDescending
        | RowsDescendingColumnsAscending
        | RowsDescendingColumnsDescending
        | ColumnsAscendingRowsAscending
        | ColumnsAscendingRowsDescending
        | ColumnsDescendingRowsAscending
        | ColumnsDescendingRowsDescending

    let getIndex number =
        number - 1

    let minRowIndex =
        0

    let minColumnIndex =
        0

    let isFirstRow rowIndex =
        rowIndex = minRowIndex

    let isLastRow rowIndex numberOfRows =
        rowIndex = (getIndex numberOfRows) 

    let isFirstColumn columnIndex =
        columnIndex = minColumnIndex

    let isLastColumn columnIndex numberOfColumns =
        columnIndex = (getIndex numberOfColumns)

    let maxRowIndex array2d =
        getIndex (array2d |> Array2D.length1)

    let maxColumnIndex array2d =
        getIndex (array2d |> Array2D.length2)

    let existAt array2d coordinate =
        minRowIndex <= coordinate.RowIndex &&
        coordinate.RowIndex <= maxRowIndex array2d &&
        minColumnIndex <= coordinate.ColumnIndex &&
        coordinate.ColumnIndex <= maxColumnIndex array2d

    let get (array2d : 't[,]) coordinate =
        array2d.[coordinate.RowIndex, coordinate.ColumnIndex]

    let extractByRows array2d =
        let numberRowsIndex = maxRowIndex array2d
        seq {
            for i in 0 .. numberRowsIndex do
                yield array2d.[i, *]
        }

    let extractByColumns array2d =
        let numberColumnsIndex = maxColumnIndex array2d
        seq {
            for i in 0 .. numberColumnsIndex do
                yield array2d.[*, i]
        }

    let getItemByItem (array2d: 'T[,]) extractBy filter =

        let getCoordinate dimension1Index dimension2Index =
            match extractBy with
            | RowsAscendingColumnsAscending | RowsAscendingColumnsDescending
            | RowsDescendingColumnsAscending | RowsDescendingColumnsDescending
             -> { RowIndex = dimension1Index; ColumnIndex = dimension2Index }
            | ColumnsAscendingRowsAscending | ColumnsAscendingRowsDescending
            | ColumnsDescendingRowsAscending | ColumnsDescendingRowsDescending
             -> { RowIndex = dimension2Index; ColumnIndex = dimension1Index }

        let (minDimension1Index, maxDimension1Index, incrementDimension1,
             minDimension2Index, maxDimension2Index, incrementDimension2) =
            match extractBy with
            | RowsAscendingColumnsAscending -> (0, maxRowIndex array2d, 1, 0, maxColumnIndex array2d, 1)
            | RowsAscendingColumnsDescending -> (0, maxRowIndex array2d, 1, maxColumnIndex array2d, 0, -1)
            | RowsDescendingColumnsAscending -> (maxRowIndex array2d, 0, -1, 0, maxColumnIndex array2d, 1)
            | RowsDescendingColumnsDescending -> (maxRowIndex array2d, 0, -1, maxColumnIndex array2d, 0, -1)
            | ColumnsAscendingRowsAscending -> (0, maxColumnIndex array2d, 1, 0, maxRowIndex array2d, 1)
            | ColumnsAscendingRowsDescending -> (0, maxColumnIndex array2d, 1, maxRowIndex array2d, 0, -1)
            | ColumnsDescendingRowsAscending -> (maxColumnIndex array2d, 0, -1, 0, maxRowIndex array2d, 1)
            | ColumnsDescendingRowsDescending -> (maxColumnIndex array2d, 0, -1, maxRowIndex array2d, 0, -1)

        seq {
            for dimension1Index in minDimension1Index .. incrementDimension1 .. maxDimension1Index do
                for dimension2Index in minDimension2Index .. incrementDimension2 .. maxDimension2Index do
                    let coordinate = (getCoordinate dimension1Index dimension2Index)
                    let item = get array2d coordinate
                    if (filter item coordinate) then
                        yield (item, coordinate)
        }

    let fold (folding: int -> int -> 'S -> 'T -> 'S) (state: 'S) (array2d: 'T[,]) =
        let mutable state = state
        for r in 0 .. maxRowIndex array2d do
            for c in 0 .. maxColumnIndex array2d do
                state <- folding r c state (array2d.[r, c])
        state