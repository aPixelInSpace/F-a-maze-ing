// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas

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
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                Cell.Instance.create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RowIndex = rowIndex; ColumnIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        { Canvas = canvas; Cells = cells; }

    let isALimitAt grid coordinate position =
        let zone = grid.Canvas.Zone coordinate
        let cell = get grid.Cells coordinate

        if not zone.IsAPartOfMaze || cell.WallTypeAtPosition position = Border then
            true
        else
            let neighborCoordinate = Cell.getNeighborCoordinateAtPosition coordinate position

            (grid.Canvas.ExistAt neighborCoordinate) &&
            not (grid.Canvas.Zone neighborCoordinate).IsAPartOfMaze

    let isNavigable grid fromCoordinate pos =
        not (isALimitAt grid fromCoordinate pos) &&        
        (get grid.Cells fromCoordinate).WallTypeAtPosition pos = Empty &&
        grid.Canvas.IsZonePartOfMaze (Cell.getNeighborCoordinateAtPosition fromCoordinate pos)

    let getNavigableNeighborsCoordinates grid coordinate =        
        let isNavigable = isNavigable grid coordinate
        let getNeighborCoordinateAtPosition = Cell.getNeighborCoordinateAtPosition coordinate

        seq {
            if (isNavigable Top) then
                yield getNeighborCoordinateAtPosition Top

            if (isNavigable Right) then
                yield getNeighborCoordinateAtPosition Right

            if (isNavigable Bottom) then
                yield getNeighborCoordinateAtPosition Bottom

            if (isNavigable Left) then
                yield getNeighborCoordinateAtPosition Left
        }

    let updateWallAtPosition grid coordinate (position : Position) wallType =               

        let getWalls coordinate position =
            grid.Cells.[coordinate.RowIndex, coordinate.ColumnIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = Wall.wallIndex position then
                    { WallType = wallType; WallPosition = position}
                else
                    wall
                )

        grid.Cells.[coordinate.RowIndex, coordinate.ColumnIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = Cell.getNeighborCoordinateAtPosition coordinate position        
        grid.Cells.[neighbor.RowIndex, neighbor.ColumnIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    let ifNotAtLimitUpdateWallAtPosition grid coordinate position wallType =        
        if not (isALimitAt grid coordinate position) then
            updateWallAtPosition grid coordinate position wallType