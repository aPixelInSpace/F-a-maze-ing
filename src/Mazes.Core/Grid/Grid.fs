// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Position
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

    member this.IsLimitAt coordinate position =
        let zone = this.Canvas.Zone coordinate
        let cell = this.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = coordinate.NeighborCoordinateAtPosition position

                not ((this.Canvas.ExistAt neighborCoordinate) &&
                    (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member this.UpdateWallAtPosition coordinate (position : Position) wallType =
        let getWalls coordinate position =
            this.Cells.[coordinate.RowIndex, coordinate.ColumnIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = (Cell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        this.Cells.[coordinate.RowIndex, coordinate.ColumnIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = coordinate.NeighborCoordinateAtPosition position        
        this.Cells.[neighbor.RowIndex, neighbor.ColumnIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    member this.IfNotAtLimitUpdateWallAtPosition coordinate position wallType =
        if not (this.IsLimitAt coordinate position) then
            this.UpdateWallAtPosition coordinate position wallType

    member this.IsNavigable fromCoordinate toPos =
        not (this.IsLimitAt fromCoordinate toPos) &&        
        (this.Cell fromCoordinate).WallTypeAtPosition toPos = WallType.Empty &&
        this.Canvas.IsZonePartOfMaze (fromCoordinate.NeighborCoordinateAtPosition toPos)

    member this.NavigableNeighborsCoordinates coordinate =
        let isNavigable = this.IsNavigable coordinate

        seq {
            if (isNavigable Top) then
                yield coordinate.NeighborCoordinateAtPosition Top

            if (isNavigable Right) then
                yield coordinate.NeighborCoordinateAtPosition Right

            if (isNavigable Bottom) then
                yield coordinate.NeighborCoordinateAtPosition Bottom

            if (isNavigable Left) then
                yield coordinate.NeighborCoordinateAtPosition Left
        }

module Grid =

    let create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                Cell.create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RowIndex = rowIndex; ColumnIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        { Canvas = canvas; Cells = cells; }