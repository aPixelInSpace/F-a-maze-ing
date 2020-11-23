namespace Mazes.Core

module Array2D =

    let getIndex number =
        number - 1

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

    let get coordinate (array2d : 't[,]) =
        array2d.[coordinate.RowIndex, coordinate.ColumnIndex]