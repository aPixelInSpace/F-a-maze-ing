// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
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

    member private this.UpdateWallAtPosition coordinate (position : Position) wallType =
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

    member private this.IfNotAtLimitUpdateWallAtPosition coordinate position wallType =
        if not (this.IsLimitAt coordinate position) then
            this.UpdateWallAtPosition coordinate position wallType

    member this.LinkCellAtPosition coordinate position =
        this.UpdateWallAtPosition coordinate (position : Position) WallType.Empty

    member this.IfNotAtLimitLinkCellAtPosition coordinate position =
        this.IfNotAtLimitUpdateWallAtPosition coordinate position WallType.Empty

    member private this.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        match otherCoordinate with
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Left) -> this.UpdateWallAtPosition coordinate Left wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Top) -> this.UpdateWallAtPosition coordinate Top wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Right) -> this.UpdateWallAtPosition coordinate Right wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Bottom) -> this.UpdateWallAtPosition coordinate Bottom wallType
        | _ -> failwith "UpdateWallAtCoordinates unable to find a connection between the two coordinates"

    member this.LinkCellsAtCoordinates (coordinate : Coordinate) otherCoordinate =
        this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    member this.NeighborsFrom coordinate =
        this.Canvas.NeighborsPartOfMazeOf coordinate
            |> Seq.filter(fun (_, nPosition) -> not (this.IsLimitAt coordinate nPosition))
            |> Seq.map(fun (coordinate, _) -> coordinate)

    member this.RandomNeighborFrom (rng : Random) coordinate =
        let neighbors = this.NeighborsFrom coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member this.LinkedNeighbors isLinked coordinate =
        let neighbors = this.NeighborsFrom coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (this.Cell nCoordinate).IsLinked = isLinked)

    /// Returns the neighbors coordinates that are linked with the parameter coordinate
    member this.LinkedNeighborsCoordinates coordinate =
        let isLinkedAt pos =
            not (this.IsLimitAt coordinate pos) &&        
            (this.Cell coordinate).IsLinkedAt pos

        seq {
            if (isLinkedAt Top) then
                yield coordinate.NeighborCoordinateAtPosition Top

            if (isLinkedAt Right) then
                yield coordinate.NeighborCoordinateAtPosition Right

            if (isLinkedAt Bottom) then
                yield coordinate.NeighborCoordinateAtPosition Bottom

            if (isLinkedAt Left) then
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