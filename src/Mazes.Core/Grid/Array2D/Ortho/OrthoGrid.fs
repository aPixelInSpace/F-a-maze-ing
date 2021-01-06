// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Teleport
open Mazes.Core.Grid.Array2D.Ortho

type OrthoGrid
    (canvas, cells, teleports,
     positionHandler, coordinateHandler) =
    inherit Grid<OrthoGrid, OrthoPosition, IPositionHandler<OrthoPosition>, ICoordinateHandler<OrthoPosition>>(
        canvas, cells, teleports, positionHandler, coordinateHandler)

        override this.ToString =
            let sBuilder = StringBuilder()

            let appendHorizontalWall wallType =
                match wallType with
                    | Normal | Border -> sBuilder.Append("_") |> ignore
                    | WallType.Empty -> sBuilder.Append(" ") |> ignore

            let appendVerticalWall wallType =
                match wallType with
                    | Normal | Border -> sBuilder.Append("|") |> ignore
                    | WallType.Empty -> sBuilder.Append(" ") |> ignore

            // first row
            let lastColumnIndex = cells |> maxColumnIndex
            sBuilder.Append(" ") |> ignore
            for columnIndex in 0 .. lastColumnIndex do
                let cell =  get cells { RIndex = 0; CIndex = columnIndex }
                appendHorizontalWall (cell.WallTypeAtPosition Top)
                sBuilder.Append(" ") |> ignore
            sBuilder.Append("\n") |> ignore

            // every row
            for rowIndex in 0 .. cells |> maxRowIndex do
                for columnIndex in 0 .. lastColumnIndex do
                    let cell = get cells { RIndex = rowIndex; CIndex = columnIndex }
                    appendVerticalWall (cell.WallTypeAtPosition Left)
                    appendHorizontalWall (cell.WallTypeAtPosition Bottom)
                    
                    if columnIndex = lastColumnIndex then
                        appendVerticalWall (cell.WallTypeAtPosition Right)

                sBuilder.Append("\n") |> ignore

            sBuilder.ToString()

        override this.ToSpecializedGrid =
            this

    static member Create canvas =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                OrthoCell.Create
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        OrthoGrid(canvas, cells, Teleports.CreateEmpty, OrthoPositionHandler.Instance,  OrthoCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> OrthoGrid.Create canvas :> IGrid<OrthoGrid>