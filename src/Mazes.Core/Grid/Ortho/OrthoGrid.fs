// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open System
open System.Text
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Grid.Teleport
open Mazes.Core.Grid.Ortho
open Mazes.Core.Array2D
open Mazes.Core.Grid.Ortho.Canvas

type OrthoGrid =
    {
        Canvas : Canvas
        Cells : OrthoCell[,]
        Teleports : Teleports
    }

    interface Grid<OrthoGrid> with

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.NeighborAbstractCoordinate coordinate position =
            (OrthoCoordinate.neighborCoordinateAt coordinate (OrthoPosition.map position))

        member this.IsCellLinked coordinate =
            (this.Cell coordinate).IsLinked

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.GetAdjustedExistAt coordinate =
            existAt this.Cells coordinate

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)

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
            Some (this.Neighbor coordinate (OrthoPosition.map position))

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
                let neighborCoordinate = OrthoCoordinate.neighborCoordinateAt coordinate position

                not ((this.Canvas.ExistAt neighborCoordinate) &&
                    (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member private this.UpdateWallAtPosition coordinate (position : OrthoPosition) wallType =
        let getWalls coordinate position =
            this.Cells.[coordinate.RIndex, coordinate.CIndex].Walls
            |> Array.mapi(fun index wall ->
                if index = (OrthoCell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        this.Cells.[coordinate.RIndex, coordinate.CIndex] <- { Walls = (getWalls coordinate position) }

        let neighbor = OrthoCoordinate.neighborCoordinateAt coordinate position        
        this.Cells.[neighbor.RIndex, neighbor.CIndex] <- { Walls = (getWalls neighbor position.Opposite) }

    member this.LinkCells coordinate otherCoordinate =
        this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if not (this.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)) then
            this.LinkCells coordinate otherCoordinate

    member private this.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        let neighborCoordinateAt = OrthoCoordinate.neighborCoordinateAt coordinate
        match otherCoordinate with
        | oc when oc = (neighborCoordinateAt Left) -> this.UpdateWallAtPosition coordinate Left wallType
        | oc when oc = (neighborCoordinateAt Top) -> this.UpdateWallAtPosition coordinate Top wallType
        | oc when oc = (neighborCoordinateAt Right) -> this.UpdateWallAtPosition coordinate Right wallType
        | oc when oc = (neighborCoordinateAt Bottom) -> this.UpdateWallAtPosition coordinate Bottom wallType
        | _ -> failwith "UpdateWallAtCoordinates unable to find a connection between the two coordinates"

    member this.Neighbor coordinate position =
        OrthoCoordinate.neighborCoordinateAt coordinate position

    /// Returns the neighbors that are inside the bound of the grid
    member this.Neighbors coordinate =
        this.Canvas.NeighborsPartOfMazeOf coordinate
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
            not (this.IsLimitAt coordinate (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)) &&        
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
        for rowIndex in 0 .. this.Cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = this.Cell { RIndex = rowIndex; CIndex = columnIndex }
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

        { Canvas = canvas; Cells = cells; Teleports = Teleports.createEmpty }

    let createGridFunction canvas =
        fun () -> create canvas :> Grid<OrthoGrid>