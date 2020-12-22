// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open System
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.ArrayOfA
open Mazes.Core.Grid.Polar.ArrayOfA
open Mazes.Core.Grid.Polar.Canvas

type PolarGrid =
    {
        Canvas : Canvas
        Cells : PolarCell[][]
    }

    interface Grid<PolarGrid> with
        member self.TotalOfMazeCells =
            failwith "Not implemented"

        member self.NumberOfRows =
            failwith "Not implemented"

        member self.NumberOfColumns =
            failwith "Not implemented"

        member self.Cell coordinate =
            failwith "Not implemented"

        member self.IsLimitAt coordinate otherCoordinate =
            failwith "Not implemented"

        member self.IsCellPartOfMaze coordinate =
            failwith "Not implemented"

        member self.GetCellsByRows =
            failwith "Not implemented"

        member self.GetCellsByColumns =
            failwith "Not implemented"

        member self.CoordinatesPartOfMaze =
            failwith "Not implemented"

        member self.LinkCells coordinate otherCoordinate =
            self.LinkCells coordinate otherCoordinate

        member self.IfNotAtLimitLinkCells coordinate otherCoordinate =
            failwith "Not implemented"

        member self.NeighborsThatAreLinked isLinked coordinate =
            self.NeighborsThatAreLinked isLinked coordinate

        member self.LinkedNeighborsWithCoordinates coordinate =
            failwith "Not implemented"

        member self.RandomNeighborFrom rng coordinate =
            failwith "Not implemented"

        member self.RandomCoordinatePartOfMazeAndNotLinked rng =
            self.RandomCoordinatePartOfMazeAndNotLinked rng

        member self.GetFirstTopLeftPartOfMazeZone =
            failwith "Not implemented"

        member self.GetFirstBottomRightPartOfMazeZone =
            failwith "Not implemented"

        member self.ToString =
            failwith "Not implemented"

        member self.ToSpecializedGrid =
            self

    member self.Cell coordinate =
        get self.Cells coordinate

    member self.IsLimitAt coordinate otherCoordinate =
        let zone = self.Canvas.Zone coordinate

        let neighborCondition =
            let neighborPosition = PolarCoordinate.neighborPositionAt self.Cells coordinate otherCoordinate
            let neighborCell = self.Cell otherCoordinate

            if neighborPosition <> Inward then
                not ((self.Canvas.ExistAt otherCoordinate) && (self.Canvas.Zone otherCoordinate).IsAPartOfMaze) ||
                neighborCell.WallTypeAtPosition neighborPosition.Opposite = Border
            else
                let cell = self.Cell coordinate
                if not (isFirstRing coordinate.RIndex) then
                    not ((self.Canvas.ExistAt otherCoordinate) && (self.Canvas.Zone otherCoordinate).IsAPartOfMaze) ||
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

    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY with the coordinate
    member self.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = self.NeighborsFrom coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (self.Cell nCoordinate).IsLinked self.Cells nCoordinate = isLinked)

    member self.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let getCellByCell =
            seq {
                for ringIndex in 0 .. maxD1Index self.Cells do
                    for cellIndex in 0 .. maxD2Index self.Cells ringIndex do
                        let coordinate = { RIndex = ringIndex; CIndex = cellIndex }
                        let cell = self.Cell  coordinate
                        if not (cell.IsLinked self.Cells coordinate) then
                            yield { RIndex = ringIndex; CIndex = cellIndex }
            }
        
        let unlinkedPartOfMazeCells = getCellByCell |> Seq.toArray
        unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)]

module PolarGrid =

    let create (canvas : Canvas) =

        let cells =
            ArrayOfA.create
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