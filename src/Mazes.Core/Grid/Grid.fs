// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas
open Mazes.Core.Canvas.Canvas

type Grid =
    {
        Canvas : Canvas
        Cells : Cell[,]
    }

    member this.HasCells =
        this.Cells.Length > 0

    member this.Cell coordinate =
        get this.Cells coordinate

module Grid =

    let create canvas =
        let isPartOfMaze = Canvas.isPartOfMaze canvas

        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                Cell.Instance.create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RowIndex = rowIndex; ColumnIndex = columnIndex }
                    isPartOfMaze)

        { Canvas = canvas; Cells = cells; }

    let isALimitAt grid coordinate position =
        let zone = coordinate |> getZone grid.Canvas
        let cell = get grid.Cells coordinate

        if not zone.IsAPartOfMaze || cell.WallTypeAtPosition position = Border then
            true
        else
            let neighborCoordinate = Cell.getNeighborCoordinateAtPosition coordinate position

            (existAt grid.Canvas neighborCoordinate) &&
            not (getZone grid.Canvas neighborCoordinate).IsAPartOfMaze

    let isNavigable grid fromCoordinate toCoordinate pos =
        not (isALimitAt grid fromCoordinate pos) &&        
        (get grid.Cells fromCoordinate).WallTypeAtPosition pos = Empty &&
        isPartOfMaze grid.Canvas toCoordinate

    let getNavigableNeighborsCoordinates grid coordinate =        
        let isNavigable = isNavigable grid coordinate
        let neighborCoordinate = Cell.getNeighborCoordinateAtPosition coordinate

        seq {
            let topCoordinate = neighborCoordinate Top
            if (isNavigable topCoordinate Top) then
                yield topCoordinate

            let rightCoordinate = neighborCoordinate Right
            if (isNavigable rightCoordinate Right) then
                yield  rightCoordinate

            let bottomCoordinate = neighborCoordinate Bottom
            if (isNavigable bottomCoordinate Bottom) then
                yield bottomCoordinate

            let leftCoordinate = neighborCoordinate Left
            if (isNavigable leftCoordinate Left) then
                yield leftCoordinate
        }

    let updateWallAtPosition grid coordinate position wallType =
        let r = coordinate.RowIndex
        let c = coordinate.ColumnIndex

        match position with
        | Top ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallTop = { WallType = wallType; WallPosition = Top } }
            grid.Cells.[r - 1, c] <- { grid.Cells.[r - 1, c] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
        | Right ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallRight = { WallType = wallType; WallPosition = Right } }
            grid.Cells.[r, c + 1] <- { grid.Cells.[r, c + 1] with WallLeft = { WallType = wallType; WallPosition = Left } }
        | Bottom ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
            grid.Cells.[r + 1, c] <- { grid.Cells.[r + 1, c] with WallTop = { WallType = wallType; WallPosition = Top } }
        | Left ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallLeft = { WallType = wallType; WallPosition = Left } }
            grid.Cells.[r, c - 1] <- { grid.Cells.[r, c - 1] with WallRight = { WallType = wallType; WallPosition = Right } }

    let ifNotAtLimitUpdateWallAtPosition grid coordinate position wallType =        
        if not (isALimitAt grid coordinate position) then
            updateWallAtPosition grid coordinate position wallType