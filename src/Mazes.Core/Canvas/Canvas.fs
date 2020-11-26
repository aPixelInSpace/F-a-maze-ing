// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Canvas

open System
open System.Text
open Mazes.Core
open Mazes.Core.Array2D

type Canvas =
    { Zones : Zone[,] }

    member this.NumberOfRows =
        Array2D.length1 this.Zones

    member this.NumberOfColumns =
        Array2D.length2 this.Zones

    member this.TotalOfMazeZones =
        let counter = 0

        this.Zones
        |> reduce(fun _ _ counter zone ->
            match zone with
            | PartOfMaze -> counter + 1
            | _ -> counter) counter

module Canvas =

    let getZone canvas coordinate =
        get canvas.Zones coordinate

    let isPartOfMaze canvas coordinate =
        (getZone canvas coordinate).IsAPartOfMaze

    let maxRowIndex canvas =
        maxRowIndex canvas.Zones 

    let maxColumnIndex canvas =
        maxColumnIndex canvas.Zones 

    let existAt canvas coordinate =
        existAt canvas.Zones coordinate

    module Convert =

        let private charPartOfMaze = '*'
        let private charNotPartOfMaze = '.'
        
        let startLineTag = "Type=Canvas\n"
        let endLineTag = "end"

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
            sBuilder.Append(startLineTag) |> ignore
            canvas.Zones
                |> extractByRows
                |> Seq.iter(fun rowZones -> appendRow sBuilder rowZones)

            sBuilder.Append(endLineTag) |> ignore

            sBuilder.ToString()

        let fromString (save : string) =
            let lines = save.Split('\n')
            if lines.[0] = "Type=Canvas" then
                let numberOfRows = lines.Length - 2
                let numberOfColumns =
                    match lines.[1].StartsWith(endLineTag) with
                    | false -> lines.[1].Length
                    | true -> 0

                let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create ((charToZone lines.[rowIndex + 1].[columnIndex]) = PartOfMaze))

                Some { Zones = zones; }            
            else
                None