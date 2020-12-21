// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open System
open Mazes.Core.Grid

type PolarGrid =
    {
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

module PolarGrid =

    let create numberOfRings widthHeightRatio =

        let ringHeight = widthHeightRatio / (float)numberOfRings 

        let createRingCells ringNumber previousNumberOfCellsForTheRing numberOfCellsForTheRing =
            [|
                for _ in 1 .. numberOfCellsForTheRing ->
                    { Dummy = true }
            |]

        let cells =
            [|
                let mutable currentNumberOfCellsForTheRing = 0

                for ringNumber in 1 .. numberOfRings ->
                    if ringNumber = 1 then
                        let previousNumberOfCellsForTheRing = currentNumberOfCellsForTheRing
                        currentNumberOfCellsForTheRing <- 1

                        createRingCells ringNumber previousNumberOfCellsForTheRing currentNumberOfCellsForTheRing
                    else
                        let radius = (float)ringNumber / (float)numberOfRings
                        let circumference = 2.0 * Math.PI * radius
                        let estimatedCellWidth = circumference / (float)currentNumberOfCellsForTheRing
                        let ratio = (int)(Math.Round(estimatedCellWidth / ringHeight, 0))
                        let previousNumberOfCellsForTheRing = currentNumberOfCellsForTheRing
                        currentNumberOfCellsForTheRing <- currentNumberOfCellsForTheRing * ratio

                        createRingCells ringNumber previousNumberOfCellsForTheRing currentNumberOfCellsForTheRing
            |]

        { Cells = cells }
            