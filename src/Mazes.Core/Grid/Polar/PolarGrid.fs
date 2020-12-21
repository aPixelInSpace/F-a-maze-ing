// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.ArrayOfA
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

        member self.IsLimitAt coordinate position =
            failwith "Not implemented"

        member self.IsCellPartOfMaze coordinate =
            failwith "Not implemented"

        member self.GetCellsByRows =
            failwith "Not implemented"

        member self.GetCellsByColumns =
            failwith "Not implemented"

        member self.CoordinatesPartOfMaze =
            failwith "Not implemented"

        member self.LinkCellAtPosition coordinate position =
            failwith "Not implemented"

        member self.IfNotAtLimitLinkCellAtPosition coordinate position =
            failwith "Not implemented"

        member self.LinkCellsAtCoordinates coordinate otherCoordinate =
            failwith "Not implemented"

        member self.NeighborsThatAreLinked isLinked coordinate =
            failwith "Not implemented"

        member self.LinkedNeighborsWithCoordinates coordinate =
            failwith "Not implemented"

        member self.RandomNeighborFrom rng coordinate =
            failwith "Not implemented"

        member self.RandomCoordinatePartOfMazeAndNotLinked rng =
            failwith "Not implemented"

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

    member self.IsLimitAt coordinate position =
        let zone = self.Canvas.Zone coordinate
        let cell = self.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborsCoordinates = PolarCoordinate.neighborsCoordinateAt self.Canvas.Zones coordinate position
                if neighborsCoordinates |> Seq.isEmpty then
                    true
                else
                    let existCoordinate =
                        neighborsCoordinates
                        |> Seq.tryFind(fun neighborCoordinate -> (self.Canvas.Zone neighborCoordinate).IsAPartOfMaze)
                    match existCoordinate with
                    | Some _ -> true
                    | None -> false

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    /// Returns the neighbors that are inside the bound of the grid
    member self.NeighborsFrom coordinate =
        self.Canvas.NeighborsPartOfMazeOf coordinate
            |> Seq.filter(fun (_, nPosition) -> not (self.IsLimitAt coordinate nPosition))
            |> Seq.map(fun (coordinate, _) -> coordinate)

    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY with the coordinate
    member self.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = self.NeighborsFrom coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (self.Cell nCoordinate).IsLinked self.Cells coordinate = isLinked)

module PolarGrid =

    let create (canvas : Canvas) =

        let cells =
            ArrayOfA.create
                canvas.NumberOfRings
                canvas.WidthHeightRatio
                canvas.NumberOfCellsForCenterRing
                (fun rIndex cIndex -> PolarCell.create canvas { RIndex = rIndex; CIndex = cIndex } canvas.IsZonePartOfMaze )

        {
            Canvas = canvas
            Cells = cells
        }
            