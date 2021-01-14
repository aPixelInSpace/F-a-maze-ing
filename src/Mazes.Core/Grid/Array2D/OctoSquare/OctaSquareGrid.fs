// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.OctaSquare

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.OctaSquare

type OctaSquareGrid
    (canvas, cells, nonAdjacentNeighbors, obstacles,
     positionHandler, coordinateHandler) =
    inherit Grid<OctaSquareGrid, OctaSquarePosition, IPositionHandler<OctaSquarePosition>, ICoordinateHandler<OctaSquarePosition>>(
        canvas, cells, nonAdjacentNeighbors, obstacles, positionHandler, coordinateHandler)

        override this.ToString =
            ""

        override this.ToSpecializedGrid =
            this

    static member Create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                OctaSquareCell.Create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        OctaSquareGrid(canvas, cells, NonAdjacentNeighbors.CreateEmpty, Obstacles.CreateEmpty, OctaSquarePositionHandler.Instance,  OctaSquareCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> OctaSquareGrid.Create canvas :> IGrid<OctaSquareGrid>