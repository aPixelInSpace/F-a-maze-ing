// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Tri

open System.Text
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Tri
open Mazes.Core.Array2D

type TriGrid
    (canvas, cells, nonAdjacentNeighbors, obstacles,
     positionHandler, coordinateHandler) =
    inherit Grid<TriGrid, TriPosition, IPositionHandler<TriPosition>, ICoordinateHandler<TriPosition>>(
        canvas, cells, nonAdjacentNeighbors, obstacles, positionHandler, coordinateHandler)

        override this.ToString =
            ""

        override this.ToSpecializedGrid =
            this

    static member Create internalWallType canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                TriCell.Create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    internalWallType
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        TriGrid(canvas, cells, NonAdjacentNeighbors.CreateEmpty, Obstacles.CreateEmpty, TriPositionHandler.Instance,  TriCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> TriGrid.Create Normal canvas :> IGrid<TriGrid>