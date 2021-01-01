// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Hex

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Teleport
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Hex

type HexGrid
    (canvas, cells, teleports,
     positionHandler, coordinateHandler) =
    inherit Grid<HexGrid, HexPosition, IPositionHandler<HexPosition>, ICoordinateHandler<HexPosition>>(
        canvas, cells, teleports, positionHandler, coordinateHandler)

        override this.ToString =
            ""

        override this.ToSpecializedGrid =
            this

    static member Create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                HexCell.Create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        HexGrid(canvas, cells, Teleports.createEmpty, HexPositionHandler.Instance,  HexCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> HexGrid.Create canvas :> IGrid<HexGrid>