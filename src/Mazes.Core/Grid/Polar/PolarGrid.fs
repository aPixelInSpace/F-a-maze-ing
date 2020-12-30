// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open System
open System.Text
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Grid.Teleport
open Mazes.Core.ArrayOfA
open Mazes.Core.Grid.Polar.PolarArrayOfA
open Mazes.Core.Grid.Polar.Canvas

type PolarGrid =
    {
        Canvas : Canvas
        Cells : PolarCell[][]
        Teleports : Teleports
    }

    interface Grid<PolarGrid> with
        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Dimension1Boundaries cellIndex =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length

            let startIndex =
                this.Cells
                |> Array.findIndex(fun ring ->
                    let steps = maxCellsInLastRing / ring.Length
                    cellIndex % steps = 0)

            let length = this.Cells.Length - startIndex

            (startIndex, length)

        member this.Dimension2Boundaries ringIndex =
            (0, this.Cells.[ringIndex].Length)

        member this.NeighborAbstractCoordinate coordinate position =
            (PolarCoordinate.neighborBaseCoordinateAt coordinate (PolarPosition.map position))

        member this.IsCellLinked coordinate =
            (this.Cell coordinate).IsLinked this.Cells coordinate

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.GetAdjustedExistAt coordinate =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length
            let ratio = maxCellsInLastRing / ringLength
            coordinate.CIndex % ratio = 0

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate otherCoordinate

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.GetRIndexes =
            this.Cells |> getRIndexes

        member this.GetCIndexes =
            this.Cells |> getCIndexes

        member this.GetAdjustedCoordinate coordinate =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length

            let ratio = maxCellsInLastRing / ringLength

            { coordinate with CIndex = coordinate.CIndex / ratio }

        member this.CoordinatesPartOfMaze =
            this.CoordinatesPartOfMaze

        member this.LinkCells coordinate otherCoordinate =
            this.LinkCells coordinate otherCoordinate

        member this.PutBorderBetweenCells coordinate otherCoordinate =
            let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
            this.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Border

        member this.Neighbor coordinate position =
            this.Neighbor coordinate (PolarPosition.map position)

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

    member this.Cell coordinate =
        get this.Cells coordinate

    member this.GetCellByCell filter =
        getItemByItem this.Cells filter

    member this.IsLimitAt coordinate otherCoordinate =
        let zone = this.Canvas.Zone coordinate

        let neighborCondition =
            let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
            let neighborCell = this.Cell otherCoordinate

            if neighborPosition <> Inward then
                not (this.Canvas.ExistAt otherCoordinate) ||
                not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                neighborCell.WallTypeAtPosition neighborPosition.Opposite = Border                
            else
                let cell = this.Cell coordinate
                if not (isFirstRing coordinate.RIndex) then
                    not (this.Canvas.ExistAt otherCoordinate) ||
                    not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                    cell.WallTypeAtPosition neighborPosition = Border
                else
                    true

        (not zone.IsAPartOfMaze) ||
        neighborCondition

    member private this.UpdateWallAtPosition coordinate neighborCoordinate (neighborPosition : PolarPosition) wallType =
        let getWalls coordinate position =
            this.Cells.[coordinate.RIndex].[coordinate.CIndex].Walls
            |> Array.map(fun wall ->
                if wall.WallPosition = position then
                    { WallType = wallType; WallPosition = position }
                else
                    wall)

        match neighborPosition with
        | Ccw | Cw ->
            this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
            this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }
            if (PolarCoordinate.neighborsCoordinateAt this.Cells coordinate neighborPosition.Opposite) |> Seq.head = neighborCoordinate then
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition.Opposite) }
                this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition) }            
        | Inward ->
            this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
        | Outward ->
            this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }

    member this.LinkCells coordinate otherCoordinate =
        let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
        this.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Empty

    member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if not (this.IsLimitAt coordinate otherCoordinate) then
            this.LinkCells coordinate otherCoordinate

    member this.Neighbor coordinate position =
        let neighbors = PolarCoordinate.neighborsCoordinateAt this.Cells coordinate position
        if neighbors |> Seq.isEmpty then
            None
        else
            Some (neighbors |> Seq.last)

    /// Returns the neighbors that are inside the bound of the grid
    member this.NeighborsFrom coordinate =
        this.Canvas.NeighborsPartOfMazeOf coordinate
        |> Seq.filter(fun (nCoordinate, _) -> not (this.IsLimitAt coordinate nCoordinate))
        |> Seq.map(fst)

    member this.RandomNeighbor (rng : Random) coordinate =
        let neighbors = this.NeighborsFrom coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member this.NeighborsThatAreLinked isLinked coordinate =
        this.NeighborsFrom coordinate
        |> Seq.filter(fun nCoordinate -> (this.Cell nCoordinate).IsLinked this.Cells nCoordinate = isLinked)

    member this.AddTwoWayTeleport fromCoordinate toCoordinate =
        this.Teleports.AddTwoWayTeleport fromCoordinate toCoordinate

    member this.LinkedNeighbors coordinate =
        let isLinkedAt otherCoordinate =
            not (this.IsLimitAt coordinate otherCoordinate) &&        
            (this.Cell coordinate).AreLinked this.Cells coordinate otherCoordinate

        seq {
            let neighborsCoordinates = this.NeighborsFrom coordinate
            for neighborCoordinate in neighborsCoordinates do   
                if (isLinkedAt neighborCoordinate) then
                    yield neighborCoordinate

            for teleport in this.Teleports.Teleports coordinate do
                yield teleport
        }

    member this.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            this.GetCellByCell (fun cell coordinate -> (this.Canvas.Zone coordinate).IsAPartOfMaze && not (cell.IsLinked this.Cells coordinate))
            |> Seq.toArray
        snd unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)]

    member this.CoordinatesPartOfMaze =
        this.Canvas.GetZoneByZone (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate)

    member this.ToString =
        let sBuilder = StringBuilder()

        let appendHorizontalWall wallType (sBuilder : StringBuilder) =
            match wallType with
                | Normal | Border -> sBuilder.Append("‾")
                | WallType.Empty -> sBuilder.Append("¨")

        let appendVerticalWall wallType (sBuilder : StringBuilder) =
            match wallType with
                | Normal | Border -> sBuilder.Append("|")
                | WallType.Empty -> sBuilder.Append("¦")

        let appendWhiteSpace (sBuilder : StringBuilder) =
            sBuilder.Append(" ")

        let appendRing appendCell appendLastCell (cellsRow : PolarCell array) =
            cellsRow
            |> Array.iter(appendCell)

            cellsRow
            |> Array.last
            |> appendLastCell

            sBuilder.Append("\n") |> ignore

        let lastCell (lastCell : PolarCell) =
            sBuilder
            |> appendVerticalWall (lastCell.WallTypeAtPosition Cw) |> ignore

        // first
        let firstRing (cell : PolarCell) =
            sBuilder
            |> appendVerticalWall (cell.WallTypeAtPosition Ccw)
            |> appendWhiteSpace
            |> ignore

        getRingByRing this.Cells
        |> Seq.head
        |> appendRing firstRing lastCell

        // others
        let everyOtherRing (cell : PolarCell) =
            sBuilder
            |> appendVerticalWall (cell.WallTypeAtPosition Ccw)
            |> appendHorizontalWall (cell.WallTypeAtPosition Inward)
            |> ignore

        getRingByRing this.Cells
        |> Seq.iteri(fun ringIndex cells ->
            if ringIndex > 0 then
                cells
                |> appendRing everyOtherRing lastCell
            else
                ())

        // last
        let lastRing (cell : PolarCell) =
            sBuilder
            |> appendWhiteSpace
            |> appendHorizontalWall (cell.WallTypeAtPosition Outward)
            |> ignore

        getRingByRing this.Cells
        |> Seq.last
        |> appendRing lastRing (fun _ -> ())

        sBuilder.ToString()

module PolarGrid =

    let create (canvas : Canvas) =

        let cells =
            create
                canvas.NumberOfRings
                canvas.WidthHeightRatio
                canvas.NumberOfCellsForCenterRing
                (fun rIndex cIndex -> PolarCell.create canvas { RIndex = rIndex; CIndex = cIndex } canvas.IsZonePartOfMaze)

        {
            Canvas = canvas
            Cells = cells
            Teleports = Teleports.createEmpty
        }

    let createGridFunction canvas =
        fun () -> create canvas :> Grid<PolarGrid>