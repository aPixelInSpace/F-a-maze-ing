// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open System
open System.Text
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Grid.Ortho
open Mazes.Core.Position
open Mazes.Core.Array2D
open Mazes.Core.Canvas

type OrthoGrid =
    {
        Canvas : Canvas
        Cells : OrthoCell[,]
    }

    interface Grid<OrthoGrid> with

        member self.TotalOfMazeCells =
            self.Canvas.TotalOfMazeZones

        member self.NumberOfRows =
            self.Canvas.NumberOfRows

        member self.NumberOfColumns =
            self.Canvas.NumberOfColumns

        member self.Cell coordinate =
            (self.Cell coordinate).ToCell

        member self.IsLimitAt coordinate position =
            self.IsLimitAt coordinate position

        member self.IsCellPartOfMaze coordinate =
            self.Canvas.IsZonePartOfMaze coordinate

        member self.GetCellsByRows =
            self.Cells
                |> Array2D.map(fun cell -> cell.ToCell)
                |> extractByRows

        member self.GetCellsByColumns =
            self.Cells
                |> Array2D.map(fun cell -> cell.ToCell)
                |> extractByColumns

        member self.CoordinatesPartOfMaze =
            self.CoordinatesPartOfMaze

        member self.LinkCellAtPosition coordinate position =
            self.LinkCellAtPosition coordinate position

        member self.IfNotAtLimitLinkCellAtPosition coordinate position =
            self.IfNotAtLimitLinkCellAtPosition coordinate position

        member self.LinkCellsAtCoordinates coordinate otherCoordinate =
            self.LinkCellsAtCoordinates coordinate otherCoordinate

        member self.NeighborsThatAreLinked isLinked coordinate =
            self.NeighborsThatAreLinked isLinked coordinate

        member self.LinkedNeighborsWithCoordinates coordinate =
            self.LinkedNeighborsWithCoordinates coordinate

        member self.RandomNeighborFrom rng coordinate =
            self.RandomNeighborFrom rng coordinate

        member self.RandomCoordinatePartOfMazeAndNotLinked rng =
            self.RandomCoordinatePartOfMazeAndNotLinked rng

        member self.GetFirstTopLeftPartOfMazeZone =
            snd self.Canvas.GetFirstTopLeftPartOfMazeZone

        member self.GetFirstBottomRightPartOfMazeZone =
            snd self.Canvas.GetFirstBottomRightPartOfMazeZone

        member self.ToString =
            self.ToString

        member self.ToSpecializedGrid =
            self

    member self.HasCells =
        self.Cells.Length > 0

    member self.Cell coordinate =
        get self.Cells coordinate

    member self.GetCellByCell extractBy filter =
        getItemByItem self.Cells extractBy filter

    member self.IsLimitAt coordinate position =
        let zone = self.Canvas.Zone coordinate
        let cell = self.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = coordinate.NeighborCoordinateAtPosition position

                not ((self.Canvas.ExistAt neighborCoordinate) &&
                    (self.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member private self.UpdateWallAtPosition coordinate (position : Position) wallType =
        let getWalls coordinate position =
            self.Cells.[coordinate.RowIndex, coordinate.ColumnIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = (OrthoCell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        self.Cells.[coordinate.RowIndex, coordinate.ColumnIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = coordinate.NeighborCoordinateAtPosition position        
        self.Cells.[neighbor.RowIndex, neighbor.ColumnIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    member private self.IfNotAtLimitUpdateWallAtPosition coordinate position wallType =
        if not (self.IsLimitAt coordinate position) then
            self.UpdateWallAtPosition coordinate position wallType

    member self.LinkCellAtPosition coordinate position =
        self.UpdateWallAtPosition coordinate (position : Position) WallType.Empty

    member self.IfNotAtLimitLinkCellAtPosition coordinate position =
        self.IfNotAtLimitUpdateWallAtPosition coordinate position WallType.Empty

    member private self.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        match otherCoordinate with
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Left) -> self.UpdateWallAtPosition coordinate Left wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Top) -> self.UpdateWallAtPosition coordinate Top wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Right) -> self.UpdateWallAtPosition coordinate Right wallType
        | oc when oc = (coordinate.NeighborCoordinateAtPosition Bottom) -> self.UpdateWallAtPosition coordinate Bottom wallType
        | _ -> failwith "UpdateWallAtCoordinates unable to find a connection between the two coordinates"

    member self.LinkCellsAtCoordinates (coordinate : Coordinate) otherCoordinate =
        self.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    /// Returns the neighbors that are inside the bound of the grid
    member self.NeighborsFrom coordinate =
        self.Canvas.NeighborsPartOfMazeOf coordinate
            |> Seq.filter(fun (_, nPosition) -> not (self.IsLimitAt coordinate nPosition))
            |> Seq.map(fun (coordinate, _) -> coordinate)

    /// Returns a random neighbor that is inside the bound of the grid
    member self.RandomNeighborFrom (rng : Random) coordinate =
        let neighbors = self.NeighborsFrom coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    /// Returns a random neighbor that is not currently linked with the coordinate
    member self.RandomUnlinkedNeighborFrom (rng : Random) coordinate =
        let currentCell = self.Cell coordinate
        let neighbors =
            self.NeighborsFrom coordinate
            |> Seq.filter(fun nCoordinate -> not (currentCell.AreLinked coordinate nCoordinate))
            |> Seq.toArray

        if neighbors.Length > 0 then
            Some neighbors.[rng.Next(neighbors.Length)]
        else
            None

    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY with the coordinate
    member self.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = self.NeighborsFrom coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (self.Cell nCoordinate).IsLinked = isLinked)

    /// Returns the neighbors coordinates that are linked with the coordinate
    member self.LinkedNeighborsWithCoordinates coordinate =
        let isLinkedAt pos =
            not (self.IsLimitAt coordinate pos) &&        
            (self.Cell coordinate).IsLinkedAt pos

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

    member self.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            self.GetCellByCell RowsAscendingColumnsAscending
                (fun cell coordinate ->
                    (self.Canvas.Zone coordinate).IsAPartOfMaze &&
                    not cell.IsLinked
                ) |> Seq.toArray

        snd (unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)])

    member self.CoordinatesPartOfMaze =
        self.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate)

    member self.ToString =
        let sBuilder = StringBuilder()

        let appendHorizontalWall wallType =
            match wallType with
                | Normal | Border -> sBuilder.Append("_") |> ignore
                | WallType.Empty -> sBuilder.Append(" ") |> ignore

        let appendVerticalWall wallType =
            match wallType with
                | Normal | Border -> sBuilder.Append("|") |> ignore
                | WallType.Empty -> sBuilder.Append(" ") |> ignore

        // first row
        let lastColumnIndex = self.Cells |> maxColumnIndex
        sBuilder.Append(" ") |> ignore
        for columnIndex in 0 .. lastColumnIndex do
            let cell = self.Cell { RowIndex = 0; ColumnIndex = columnIndex }
            appendHorizontalWall cell.WallTop.WallType
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
        for rowIndex in 0 .. self.Cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = self.Cell { RowIndex = rowIndex; ColumnIndex = columnIndex }
                appendVerticalWall cell.WallLeft.WallType
                appendHorizontalWall cell.WallBottom.WallType
                
                if columnIndex = lastColumnIndex then
                    appendVerticalWall cell.WallRight.WallType

            sBuilder.Append("\n") |> ignore

        sBuilder.ToString()

module OrthoGrid =

    let create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                OrthoCell.create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RowIndex = rowIndex; ColumnIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        { Canvas = canvas; Cells = cells; }

    let createGridFunction canvas =
        fun () -> create canvas :> Grid<OrthoGrid>