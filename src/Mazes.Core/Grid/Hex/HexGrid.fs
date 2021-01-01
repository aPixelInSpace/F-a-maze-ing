// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Hex

open System
open System.Text
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Teleport
open Mazes.Core.Grid.Hex
open Mazes.Core.Array2D

type HexGrid =
    {
        Canvas : Canvas
        Cells : HexCell[,]
        Teleports : Teleports
    }

    interface IGrid<HexGrid> with

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.NeighborAbstractCoordinate coordinate position =
            (HexCoordinate.neighborCoordinateAt coordinate (HexPosition.map position))

        member this.IsCellLinked coordinate =
            (this.Cell coordinate).IsLinked

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.GetAdjustedExistAt coordinate =
            existAt this.Cells coordinate

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate (HexCoordinate.neighborPositionAt coordinate otherCoordinate)

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.GetRIndexes =
            this.Cells |> getRIndexes

        member this.GetCIndexes =
            this.Cells |> getCIndexes

        member this.GetAdjustedCoordinate coordinate =
            coordinate

        member this.CoordinatesPartOfMaze =
            this.CoordinatesPartOfMaze

        member this.LinkCells coordinate otherCoordinate =
            this.LinkCells coordinate otherCoordinate

        member this.PutBorderBetweenCells coordinate otherCoordinate =
            this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Border

        member this.Neighbor coordinate position =
            Some (this.Neighbor coordinate (HexPosition.map position))

        member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
            this.IfNotAtLimitLinkCells coordinate otherCoordinate

        member this.NeighborsThatAreLinked isLinked coordinate =
            this.NeighborsThatAreLinked isLinked coordinate

        member this.AddTwoWayTeleport fromCoordinate toCoordinate =
            this.AddTwoWayTeleport fromCoordinate toCoordinate

        member this.LinkedNeighbors coordinate =
            this.LinkedNeighbors coordinate

        member this.RandomNeighbor rng coordinate =
            this.RandomNeighbor rng coordinate

        member this.RandomCoordinatePartOfMazeAndNotLinked rng =
            this.RandomCoordinatePartOfMazeAndNotLinked rng

        member this.GetFirstPartOfMazeZone =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastPartOfMazeZone =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToString =
            this.ToString

        member this.ToSpecializedGrid =
            this
    
    member this.NumberOfRows =
        this.Canvas.NumberOfRows

    member this.NumberOfColumns =
        this.Canvas.NumberOfColumns

    member this.Cell coordinate =
        get this.Cells coordinate

    member this.GetCellByCell extractBy filter =
        getItemByItem this.Cells extractBy filter

    member this.IsLimitAt coordinate position =
        let zone = this.Canvas.Zone coordinate
        let cell = this.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = HexCoordinate.neighborCoordinateAt coordinate position

                not ((this.Canvas.ExistAt neighborCoordinate) &&
                    (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member private this.UpdateWallAtPosition coordinate (position : HexPosition) wallType =
        let getWalls coordinate position =
            this.Cells.[coordinate.RIndex, coordinate.CIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = (HexCell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        this.Cells.[coordinate.RIndex, coordinate.CIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = HexCoordinate.neighborCoordinateAt coordinate position        
        this.Cells.[neighbor.RIndex, neighbor.CIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    member this.LinkCells coordinate otherCoordinate =
        this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if not (this.IsLimitAt coordinate (HexCoordinate.neighborPositionAt coordinate otherCoordinate)) then
            this.LinkCells coordinate otherCoordinate

    member private this.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        let neighborCoordinateAt = HexCoordinate.neighborCoordinateAt coordinate

        for position in HexPosition.values do
            if otherCoordinate = (neighborCoordinateAt position) then
                this.UpdateWallAtPosition coordinate position wallType
    member this.Neighbor coordinate position =
        HexCoordinate.neighborCoordinateAt coordinate position

    /// Returns the neighbors that are inside the bound of the grid
    member this.Neighbors coordinate =
        let listOfNeighborCoordinate =
            seq {
                for position in HexPosition.values do
                    yield ((HexCoordinate.neighborCoordinateAt coordinate position), position)
            }

        this.Canvas.NeighborsPartOfMazeOf listOfNeighborCoordinate
            |> Seq.filter(fun (_, nPosition) -> not (this.IsLimitAt coordinate nPosition))
            |> Seq.map(fst)

    member this.RandomNeighbor (rng : Random) coordinate =
        let neighbors = this.Neighbors coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member this.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = this.Neighbors coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (this.Cell nCoordinate).IsLinked = isLinked)

    member this.AddTwoWayTeleport fromCoordinate toCoordinate =
        this.Teleports.AddTwoWayTeleport fromCoordinate toCoordinate

    member this.LinkedNeighbors coordinate =
        let isLinkedAt otherCoordinate =
            not (this.IsLimitAt coordinate (HexCoordinate.neighborPositionAt coordinate otherCoordinate)) &&        
            (this.Cell coordinate).AreLinked coordinate otherCoordinate

        let neighborsCoordinates = this.Neighbors coordinate

        seq {
            for neighborCoordinate in neighborsCoordinates do   
                if (isLinkedAt neighborCoordinate) then
                    yield neighborCoordinate

            for teleport in this.Teleports.Teleports coordinate do
                yield teleport
        }

    member this.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            this.GetCellByCell RowsAscendingColumnsAscending
                (fun cell coordinate ->
                    (this.Canvas.Zone coordinate).IsAPartOfMaze &&
                    not cell.IsLinked
                ) |> Seq.toArray

        snd (unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)])

    member this.CoordinatesPartOfMaze =
        this.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate)

    member this.ToString =
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
        let lastColumnIndex = this.Cells |> maxColumnIndex
        sBuilder.Append(" ") |> ignore
        for columnIndex in 0 .. lastColumnIndex do
            let cell = this.Cell { RIndex = 0; CIndex = columnIndex }
            appendHorizontalWall cell.WallTop.WallType
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
//        for rowIndex in 0 .. this.Cells |> maxRowIndex do
//            for columnIndex in 0 .. lastColumnIndex do
//                let cell = this.Cell { RIndex = rowIndex; CIndex = columnIndex }
//                appendVerticalWall cell.WallLeft.WallType
//                appendHorizontalWall cell.WallBottom.WallType
//                
//                if columnIndex = lastColumnIndex then
//                    appendVerticalWall cell.WallRight.WallType
//
//            sBuilder.Append("\n") |> ignore

        sBuilder.ToString()

module HexGrid =

    let create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                HexCell.create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        { Canvas = canvas; Cells = cells; Teleports = Teleports.createEmpty }

    let createGridFunction canvas =
        fun () -> create canvas :> IGrid<HexGrid>