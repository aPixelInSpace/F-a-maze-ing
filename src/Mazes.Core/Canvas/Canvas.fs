namespace Mazes.Core.Canvas

open System
open System.Text
open Mazes.Core
open Mazes.Core.Array2D

type Canvas = {
    Zones : Zone[,]
    NumberOfRows : int
    NumberOfColumns : int
}

module Canvas =

    let getZone canvas coordinate =
        get canvas.Zones coordinate

    let maxRowIndex canvas =
        getIndex canvas.NumberOfRows

    let maxColumnIndex canvas =
        getIndex canvas.NumberOfColumns

    let existAt canvas coordinate =
        minRowIndex <= coordinate.RowIndex &&
        coordinate.RowIndex <= (maxRowIndex canvas) &&
        minColumnIndex <= coordinate.ColumnIndex &&
        coordinate.ColumnIndex <= (maxColumnIndex canvas)

    let isPartOfMaze canvas coordinate =
        (getZone canvas coordinate) = PartOfMaze

    let countPartOfMazeZones canvas =
        let counter = 0

        canvas.Zones
        |> reduce(fun _ _ counter zone ->
            match zone with
            | PartOfMaze -> counter + 1
            | _ -> counter) counter

    module Convert =

        let private charPartOfMaze = '*'
        let private charNotPartOfMaze = '-'

        let private zoneToChar zone =
            match zone with
            | PartOfMaze -> charPartOfMaze
            | NotPartOfMaze -> charNotPartOfMaze

        let private charToZone char =
            match char with
            | c when c = charPartOfMaze -> PartOfMaze 
            | c when c = charNotPartOfMaze -> NotPartOfMaze
            | _ -> raise(Exception "Canvas character not supported")

        let toString canvas =
            let appendZone (sBuilder : StringBuilder) zone =
                sBuilder.Append(zoneToChar(zone)) |> ignore

            let appendRow (sBuilder : StringBuilder) rowZones =
                rowZones
                |> Array.iter(fun zone -> appendZone sBuilder zone)

                sBuilder.Append('\n') |> ignore

            let sBuilder = StringBuilder()
            sBuilder.Append("Type=Canvas\n") |> ignore
            canvas.Zones
                |> extractByRows
                |> Seq.iter(fun rowZones -> appendRow sBuilder rowZones)

            sBuilder.Append("end") |> ignore

            sBuilder.ToString()

        let fromString (save : string) =
            let lines = save.Split('\n')
            if lines.[0] = "Type=Canvas" then
                let numberOfRows = lines.Length - 2
                let numberOfColumns = lines.[1].Length

                let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create ((charToZone lines.[rowIndex + 1].[columnIndex]) = PartOfMaze))

                Some { Zones = zones; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }            
            else
                None