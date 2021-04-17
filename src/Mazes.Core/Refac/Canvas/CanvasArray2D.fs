// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Canvas

open System.Text
open Mazes.Core.Refac
open Mazes.Core.Refac.Array2D

type CanvasArray2D =
    {
        Zones : Zone[,]
    }

module CanvasArray2D =
    
    let numberOfRows c =
        Array2D.length1 c.Zones
        
    let numberOfColumns c =
        Array2D.length2 c.Zones

    let maxRowIndex c =
        maxRowIndex c.Zones

    let maxColumnIndex c =
        maxColumnIndex c.Zones

    let existAt c coordinate =
        existAt c.Zones coordinate

    let zone c coordinate =
        get c.Zones coordinate

    let getZoneByZone c extractBy filter =
        c.Zones |> getItemByItem extractBy filter

    let totalOfMazeZones c =
        getZoneByZone c RowsAscendingColumnsAscending (fun (zone : Zone) _ -> zone.IsAPartOfMaze)
        |> Seq.length

    let isZonePartOfMaze c coordinate =
        (zone c coordinate).IsAPartOfMaze

    let neighborsPartOfMazeOf c (listOfNeighborCoordinatePosition : (Coordinate2D * _) seq) =
        seq {
            for (coordinate, position) in listOfNeighborCoordinatePosition do
                if (existAt c coordinate) && (zone c coordinate).IsAPartOfMaze then
                    yield (coordinate, position)
        }

    let firstPartOfMazeZone c =
        getZoneByZone c RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let lastPartOfMazeZone c =
        getZoneByZone c RowsDescendingColumnsDescending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let create numberOfRows numberOfColumns isZonePartOfMaze =
        let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create (isZonePartOfMaze rowIndex columnIndex))
    
        { Zones = zones }

    module Convert =

        let charPartOfMaze = '*'
        let charEmpty = '.'
        
        let startLineTag = "Type=Canvas"
        let endLineTag = "end"

        let private zoneToChar zone =
            match zone with
            | PartOfMaze -> charPartOfMaze
            | Empty -> charEmpty

        let private charToZone char =
            match char with
            | c when c = charPartOfMaze -> Some PartOfMaze 
            | c when c = charEmpty -> Some Empty
            | _ -> None

        let toString canvas =
            let appendZone (sBuilder : StringBuilder) zone =
                sBuilder.Append(zoneToChar(zone)) |> ignore

            let appendRow (sBuilder : StringBuilder) rowZones =
                rowZones
                |> Array.iter(appendZone sBuilder)

                sBuilder.Append('\n') |> ignore

            let sBuilder = StringBuilder()
            sBuilder.Append(startLineTag + "\n") |> ignore
            canvas.Zones
                |> extractByRows
                |> Seq.iter(appendRow sBuilder)

            sBuilder.Append(endLineTag) |> ignore

            sBuilder.ToString()

        let fromString (save : string) =
            let lines = save.Split('\n')
            if lines.[0].StartsWith(startLineTag) then
                let numberOfRows = lines.Length - 2
                let numberOfColumns =
                    match lines.[1].StartsWith(endLineTag) with
                    | false -> lines.[1].Length
                    | true -> 0

                let zones =
                    Array2D.init
                        numberOfRows
                        numberOfColumns
                        (fun rowIndex columnIndex -> Zone.create ((charToZone lines.[rowIndex + 1].[columnIndex]) = Some PartOfMaze))

                Some { Zones = zones; }            
            else
                None