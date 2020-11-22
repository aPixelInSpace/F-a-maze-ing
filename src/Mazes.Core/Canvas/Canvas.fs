namespace Mazes.Core.Canvas

open System
open System.Text
open Mazes.Core

type Canvas = {
    CellsType : CellType[,]
    NumberOfRows : int
    NumberOfColumns : int
}

module Canvas =
    let private charPartOfMaze = '*'
    let private charNotPartOfMaze = '-'

    let isPartOfMaze canvas coordinate =
        match canvas.CellsType.[coordinate.RowIndex, coordinate.ColumnIndex] with
        | PartOfMaze -> true
        | NotPartOfMaze -> false

    let private cellTypeToChar cellType =
        match cellType with
        | PartOfMaze -> charPartOfMaze
        | NotPartOfMaze -> charNotPartOfMaze

    let private charToCellType char =
        match char with
        | c when c = charPartOfMaze -> PartOfMaze 
        | c when c = charNotPartOfMaze -> NotPartOfMaze
        | _ -> raise(Exception "Canvas character not supported")

    let save canvas =
        let appendCellType (sBuilder : StringBuilder) cellType =
            sBuilder.Append(cellTypeToChar(cellType)) |> ignore

        let appendRow (sBuilder : StringBuilder) rowCellType =
            rowCellType
            |> Array.iter(fun cellType -> appendCellType sBuilder cellType)

            sBuilder.Append('\n') |> ignore

        let sBuilder = StringBuilder()
        sBuilder.Append("Type=Canvas\n") |> ignore
        canvas.CellsType
            |> Array2D.extractByRows
            |> Seq.iter(fun rowCellType -> appendRow sBuilder rowCellType)

        sBuilder.Append("end") |> ignore

        sBuilder.ToString()

    let load (save : string) =
        let lines = save.Split('\n')
        if lines.[0] = "Type=Canvas" then
            let numberOfRows = lines.Length - 2
            let numberOfColumns = lines.[1].Length

            let cellsType = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> CellType.create ((charToCellType lines.[rowIndex + 1].[columnIndex]) = PartOfMaze))

            Some { CellsType = cellsType; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }            
        else
            None