// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open System
open System.Text
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.ArrayOfA
open Mazes.Core.Grid.Polar.PolarArrayOfA
open Mazes.Core.Grid.Polar.Canvas

type PolarGrid =
    {
        Canvas : Canvas
        Cells : PolarCell[][]
    }

    interface Grid<PolarGrid> with
        member self.TotalOfMazeCells =
            self.Canvas.TotalOfMazeZones

        member self.NumberOfRows =
            failwith "Not implemented"

        member self.NumberOfColumns =
            failwith "Not implemented"

        member self.IsCellLinked coordinate =
            (self.Cell coordinate).IsLinked self.Cells coordinate

        member self.IsLimitAt coordinate otherCoordinate =
            failwith "Not implemented"

        member self.IsCellPartOfMaze coordinate =
            failwith "Not implemented"

        member self.GetRIndexes =
            failwith "Not implemented"

        member self.GetCIndexes =
            failwith "Not implemented"

        member self.CoordinatesPartOfMaze =
            self.CoordinatesPartOfMaze

        member self.LinkCells coordinate otherCoordinate =
            self.LinkCells coordinate otherCoordinate

        member self.PutBorderBetweenCells coordinate otherCoordinate =
            let neighborPosition = PolarCoordinate.neighborPositionAt self.Cells coordinate otherCoordinate
            self.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Border

        member self.IfNotAtLimitLinkCells coordinate otherCoordinate =
            failwith "Not implemented"

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

    member self.GetCellByCell filter =
        getItemByItem self.Cells filter

    member self.IsLimitAt coordinate otherCoordinate =
        let zone = self.Canvas.Zone coordinate

        let neighborCondition =
            let neighborPosition = PolarCoordinate.neighborPositionAt self.Cells coordinate otherCoordinate
            let neighborCell = self.Cell otherCoordinate

            if neighborPosition <> Inward then
                not (self.Canvas.ExistAt otherCoordinate) ||
                not (self.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                neighborCell.WallTypeAtPosition neighborPosition.Opposite = Border                
            else
                let cell = self.Cell coordinate
                if not (isFirstRing coordinate.RIndex) then
                    not (self.Canvas.ExistAt otherCoordinate) ||
                    not (self.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                    cell.WallTypeAtPosition neighborPosition = Border
                else
                    true

        (not zone.IsAPartOfMaze) ||
        neighborCondition

    member private self.UpdateWallAtPosition coordinate neighborCoordinate (neighborPosition : PolarPosition) wallType =
        let getWalls coordinate position =
            self.Cells.[coordinate.RIndex].[coordinate.CIndex].Walls
            |> Array.map(fun wall ->
                if wall.WallPosition = position then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        match neighborPosition with
        | Left | Right ->
            self.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
            self.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }
        | Inward ->
            self.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
        | Outward ->
            self.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }

    member self.LinkCells coordinate otherCoordinate =
        let neighborPosition = PolarCoordinate.neighborPositionAt self.Cells coordinate otherCoordinate
        self.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Empty

    /// Returns the neighbors that are inside the bound of the grid
    member self.NeighborsFrom coordinate =
        self.Canvas.NeighborsPartOfMazeOf coordinate
        |> Seq.filter(fun (nCoordinate, _) -> not (self.IsLimitAt coordinate nCoordinate))
        |> Seq.map(fst)

    /// Returns a random neighbor that is inside the bound of the grid
    member self.RandomNeighbor (rng : Random) coordinate =
        let neighbors = self.NeighborsFrom coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY with the coordinate
    member self.NeighborsThatAreLinked isLinked coordinate =
        self.NeighborsFrom coordinate
        |> Seq.filter(fun nCoordinate -> (self.Cell nCoordinate).IsLinked self.Cells nCoordinate = isLinked)

    member self.LinkedNeighbors coordinate =
        let isLinkedAt otherCoordinate =
            not (self.IsLimitAt coordinate otherCoordinate) &&        
            (self.Cell coordinate).AreLinked self.Cells coordinate otherCoordinate

        seq {
            let neighborsCoordinates = self.NeighborsFrom coordinate
            for neighborCoordinate in neighborsCoordinates do   
                if (isLinkedAt neighborCoordinate) then
                    yield neighborCoordinate
        }

    member self.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            self.GetCellByCell (fun cell coordinate -> (self.Canvas.Zone coordinate).IsAPartOfMaze && not (cell.IsLinked self.Cells coordinate))
            |> Seq.toArray
        snd unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)]

    member self.CoordinatesPartOfMaze =
        self.Canvas.GetZoneByZone (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate)

    member self.ToString =
        let sBuilder = StringBuilder()

        let appendHorizontalWall wallType =
            match wallType with
                | Normal | Border -> sBuilder.Append("_ ") |> ignore
                | WallType.Empty -> sBuilder.Append("  ") |> ignore

        let appendVerticalWall wallType =
            match wallType with
                | Normal | Border -> sBuilder.Append("| ") |> ignore
                | WallType.Empty -> sBuilder.Append("  ") |> ignore

        let appendRow appendWallType position (cellsRow : PolarCell array) =
            cellsRow
            |> Array.iter(fun cell -> appendWallType  (cell.WallTypeAtPosition position))
            sBuilder.Append("\n") |> ignore

        // first ring
        getRingByRing self.Cells
        |> Seq.head
        |> appendRow appendVerticalWall Left

        // every other rings
        getRingByRing self.Cells
        |> Seq.iteri(fun ringIndex cells ->
            if ringIndex > 0 then
                sBuilder.Append(" ") |> ignore
                cells |> appendRow appendHorizontalWall Inward
                cells |> appendRow appendVerticalWall Left)

        sBuilder.Append(" ") |> ignore
        getRingByRing self.Cells
        |> Seq.last
        |> appendRow appendHorizontalWall Outward

        sBuilder.ToString()

module PolarGrid =

    let create (canvas : Canvas) =

        let cells =
            PolarArrayOfA.create
                canvas.NumberOfRings
                canvas.WidthHeightRatio
                canvas.NumberOfCellsForCenterRing
                (fun rIndex cIndex -> PolarCell.create canvas { RIndex = rIndex; CIndex = cIndex } canvas.IsZonePartOfMaze)

        {
            Canvas = canvas
            Cells = cells
        }

    let createGridFunction canvas =
        fun () -> create canvas :> Grid<PolarGrid>