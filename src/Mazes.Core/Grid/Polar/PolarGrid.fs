// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

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
            