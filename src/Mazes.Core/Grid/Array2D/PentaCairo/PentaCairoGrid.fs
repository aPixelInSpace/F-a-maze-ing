// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.PentaCairo

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.PentaCairo

/// Equilateral form of the Cairo tiling
/// Visit https://en.wikipedia.org/wiki/Cairo_pentagonal_tiling for more information
type PentaCairoGrid
    (canvas, cells, nonAdjacentNeighbors, obstacles,
     positionHandler, coordinateHandler) =
    inherit Grid<PentaCairoGrid, PentaCairoPosition, IPositionHandler<PentaCairoPosition>, ICoordinateHandler<PentaCairoPosition>>(
        canvas, cells, nonAdjacentNeighbors, obstacles, positionHandler, coordinateHandler)

        override this.ToString =
            ""

        override this.ToSpecializedGrid =
            this

    static member Create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                PentaCairoCell.Create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        PentaCairoGrid(canvas, cells, NonAdjacentNeighbors.CreateEmpty, Obstacles.CreateEmpty, PentaCairoPositionHandler.Instance,  PentaCairoCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> PentaCairoGrid.Create canvas :> IGrid<PentaCairoGrid>