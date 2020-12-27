// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open System
open System.Text
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Grid.Ortho
open Mazes.Core.Array2D
open Mazes.Core.Grid.Ortho.Canvas

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

        member self.Dimension2Length _ =
            self.Canvas.NumberOfColumns

        member self.IsCellLinked coordinate =
            (self.Cell coordinate).IsLinked

        member self.IsLimitAt coordinate otherCoordinate =
            self.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)

        member self.IsCellPartOfMaze coordinate =
            self.Canvas.IsZonePartOfMaze coordinate

        member self.GetRIndexes =
            self.Cells |> getRIndexes

        member self.GetCIndexes =
            self.Cells |> getCIndexes

        member self.CoordinatesPartOfMaze =
            self.CoordinatesPartOfMaze

        member self.LinkCells coordinate otherCoordinate =
            self.LinkCells coordinate otherCoordinate

        member self.PutBorderBetweenCells coordinate otherCoordinate =
            self.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Border

        member self.Neighbor coordinate position =
            Some (self.Neighbor coordinate (OrthoPosition.map position))

        member self.IfNotAtLimitLinkCells coordinate otherCoordinate =
            self.IfNotAtLimitLinkCells coordinate otherCoordinate

        member self.NeighborsThatAreLinked isLinked coordinate =
            self.NeighborsThatAreLinked isLinked coordinate

        member self.LinkedNeighbors coordinate =
            self.LinkedNeighbors coordinate

        member self.RandomNeighbor rng coordinate =
            self.RandomNeighbor rng coordinate

        member self.RandomCoordinatePartOfMazeAndNotLinked rng =
            self.RandomCoordinatePartOfMazeAndNotLinked rng

        member self.GetFirstPartOfMazeZone =
            snd self.Canvas.GetFirstPartOfMazeZone

        member self.GetLastPartOfMazeZone =
            snd self.Canvas.GetLastPartOfMazeZone

        member self.ToString =
            self.ToString

        member self.ToSpecializedGrid =
            self

    member self.Cell coordinate =
        get self.Cells coordinate

    member self.GetCellByCell extractBy filter =
        getItemByItem self.Cells extractBy filter

    member self.IsLimitAt coordinate position =
        let zone = self.Canvas.Zone coordinate
        let cell = self.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = OrthoCoordinate.neighborCoordinateAt coordinate position

                not ((self.Canvas.ExistAt neighborCoordinate) &&
                    (self.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member private self.UpdateWallAtPosition coordinate (position : OrthoPosition) wallType =
        let getWalls coordinate position =
            self.Cells.[coordinate.RIndex, coordinate.CIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = (OrthoCell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        self.Cells.[coordinate.RIndex, coordinate.CIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = OrthoCoordinate.neighborCoordinateAt coordinate position        
        self.Cells.[neighbor.RIndex, neighbor.CIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    member self.LinkCells coordinate otherCoordinate =
        self.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    member self.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if not (self.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)) then
            self.LinkCells coordinate otherCoordinate

    member private self.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        let neighborCoordinateAt = OrthoCoordinate.neighborCoordinateAt coordinate
        match otherCoordinate with
        | oc when oc = (neighborCoordinateAt Left) -> self.UpdateWallAtPosition coordinate Left wallType
        | oc when oc = (neighborCoordinateAt Top) -> self.UpdateWallAtPosition coordinate Top wallType
        | oc when oc = (neighborCoordinateAt Right) -> self.UpdateWallAtPosition coordinate Right wallType
        | oc when oc = (neighborCoordinateAt Bottom) -> self.UpdateWallAtPosition coordinate Bottom wallType
        | _ -> failwith "UpdateWallAtCoordinates unable to find a connection between the two coordinates"

    member self.Neighbor coordinate position =
        OrthoCoordinate.neighborCoordinateAt coordinate position

    /// Returns the neighbors that are inside the bound of the grid
    member self.Neighbors coordinate =
        self.Canvas.NeighborsPartOfMazeOf coordinate
            |> Seq.filter(fun (_, nPosition) -> not (self.IsLimitAt coordinate nPosition))
            |> Seq.map(fst)

    member self.RandomNeighbor (rng : Random) coordinate =
        let neighbors = self.Neighbors coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member self.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = self.Neighbors coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (self.Cell nCoordinate).IsLinked = isLinked)

    member self.LinkedNeighbors coordinate =
        let isLinkedAt otherCoordinate =
            not (self.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)) &&        
            (self.Cell coordinate).AreLinked coordinate otherCoordinate

        let neighborsCoordinates = self.Neighbors coordinate

        seq {
            for neighborCoordinate in neighborsCoordinates do   
                if (isLinkedAt neighborCoordinate) then
                    yield neighborCoordinate
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
            let cell = self.Cell { RIndex = 0; CIndex = columnIndex }
            appendHorizontalWall cell.WallTop.WallType
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
        for rowIndex in 0 .. self.Cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = self.Cell { RIndex = rowIndex; CIndex = columnIndex }
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
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        { Canvas = canvas; Cells = cells; }

    let createGridFunction canvas =
        fun () -> create canvas :> Grid<OrthoGrid>