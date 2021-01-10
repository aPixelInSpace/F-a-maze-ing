// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Hex

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Hex

type HexGrid
    (canvas, cells, teleports, obstacles,
     positionHandler, coordinateHandler) =
    inherit Grid<HexGrid, HexPosition, IPositionHandler<HexPosition>, ICoordinateHandler<HexPosition>>(
        canvas, cells, teleports, obstacles, positionHandler, coordinateHandler)

        override this.ToString =
            let sBuilder = StringBuilder()

//            let getPieceOfWallPos isEven pos =
//                match pos with
//                | TopLeft | BottomRight -> "/"
//                | BottomLeft | TopRight -> "\\"
//                | Top -> if isEven then "_" else "‾"
//                | Bottom -> if isEven then "_" else "‾"
//
//            let getPieceOfWallPosWallType isEven pos wallType =
//                match wallType with
//                | Normal | Border -> getPieceOfWallPos isEven pos
//                | Empty -> " "
//
//            let append isEven (cell : ICell<HexPosition>) pos (sBuilder : StringBuilder) =
//                sBuilder.Append(getPieceOfWallPosWallType isEven pos (cell.WallTypeAtPosition pos))
//
//            // first row
//            let isEven = HexPositionHandler.IsEven
//            sBuilder.Append(" ") |> ignore
//            cells.[0, *]
//            |> Array.iteri (fun c cell ->
//                    let coordinate = { RIndex = 0; CIndex = c }
//                    let isEven = isEven coordinate
//                    let append = append isEven
//                    if isEven then
//                        sBuilder
//                        |> append cell Top
//                        |> ignore
//                    else
//                        sBuilder
//                        |> append cell TopLeft
//                        |> append cell Top
//                        |> append cell TopRight
//                        |> ignore)
//
//            sBuilder.Append("\n") |> ignore
//
//            // every row
//            let lastColumnIndex = cells |> maxColumnIndex
//            cells
//            |> extractByRows
//            |> Seq.iteri(fun r row ->
//                    row
//                    |> Array.iteri(fun c cell ->
//                            let coordinate = { RIndex = r; CIndex = c }
//                            let isEven = isEven coordinate
//                            let append = append isEven
//                            if isEven then
//                                sBuilder
//                                |> append cell TopLeft
//                                |> ignore
//                                sBuilder.Append(" ") |> ignore
//                            else
//                                sBuilder
//                                |> append cell BottomLeft
//                                |> append cell Bottom
//                                |> ignore
//
//                            if c = lastColumnIndex then
//                                if isEven then
//                                    sBuilder
//                                    |> append cell TopRight
//                                    |> ignore
//                                else
//                                    sBuilder
//                                    |> append cell BottomRight
//                                    |> ignore
//                                sBuilder.Append("\n") |> ignore)
//                    
//                    row
//                    |> Array.iteri(fun c cell ->
//                            let coordinate = { RIndex = r; CIndex = c }
//                            let isEven = isEven coordinate
//                            let append = append isEven
//                            if isEven then
//                                sBuilder
//                                |> append cell BottomLeft
//                                |> append cell Bottom
//                                |> ignore
//                            else
//                                sBuilder
//                                |> append cell TopLeft
//                                |> ignore
//                                sBuilder.Append(" ") |> ignore
//
//                            if c = lastColumnIndex then
//                                if isEven then
//                                    sBuilder
//                                    |> append cell BottomRight
//                                    |> ignore
//                                else
//                                    sBuilder
//                                    |> append cell TopRight
//                                    |> ignore
//                                sBuilder.Append("\n") |> ignore))

            sBuilder.ToString()

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

        HexGrid(canvas, cells, Teleports.CreateEmpty, Obstacles.CreateEmpty, HexPositionHandler.Instance,  HexCoordinateHandler.Instance)

    static member CreateFunction canvas =
        fun () -> HexGrid.Create canvas :> IGrid<HexGrid>