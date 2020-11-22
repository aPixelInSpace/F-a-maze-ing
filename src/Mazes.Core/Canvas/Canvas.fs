namespace Mazes.Core.Canvas

open Mazes.Core

type Canvas = {
    CellsType : CellType[,]
    NumberOfRows : int
    NumberOfColumns : int
}

module Canvas =
    let isPartOfMaze canvas coordinate =
        match canvas.CellsType.[coordinate.RowIndex, coordinate.ColumnIndex] with
        | PartOfMaze -> true
        | NotPartOfMaze -> false