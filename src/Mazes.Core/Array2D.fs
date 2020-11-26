// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

namespace Mazes.Core

module Array2D =

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
        let numberRowsIndex = getIndex (array2d |> Array2D.length1)
        seq {
            for i in 0 .. numberRowsIndex do
                yield array2d.[i, *]
        }

    let extractByColumns array2d =
        let numberColumnsIndex = getIndex (array2d |> Array2D.length2)
        seq {
            for i in 0 .. numberColumnsIndex do
                yield array2d.[*, i]
        }

    let reduce (reducer: int -> int -> 'S -> 'T -> 'S) (state: 'S) (array2d: 'T[,]) =
        let mutable state = state
        for r in 0 .. Array2D.length1 array2d |> getIndex do
            for c in 0 .. Array2D.length2 array2d |> getIndex do
                state <- reducer r c state (array2d.[r, c])
        state