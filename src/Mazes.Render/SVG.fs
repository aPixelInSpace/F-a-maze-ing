// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid

let private cellWidth = 15
let private cellHeight = 15
let private marginWidth = 20
let private marginHeight = 20

type private Direction =
    | LeftToRight
    | RightToLeft
    | TopToBottom
    | BottomToTop

let private appendLine direction =
    match direction with
    | LeftToRight -> " h " + cellWidth.ToString()
    | RightToLeft -> " h -" + cellWidth.ToString()
    | TopToBottom -> " v " + cellHeight.ToString()
    | BottomToTop -> " v -" + cellHeight.ToString()

let appendOneCell (sBuilder : StringBuilder) styleClass (append : string) =
    sBuilder.Append("<path d=\"") |> ignore
    sBuilder.Append(append) |> ignore
    sBuilder.Append("\" class=\"" + styleClass + "\"/>\n") |> ignore

let renderWallType (sBuilder : StringBuilder) (grid : Grid) coordinate (wallType : WallType) styleClass =
    let cell = grid.Cell coordinate

    let topLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RowIndex * cellHeight) + marginHeight).ToString())
    let bottomLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RowIndex + 1) * cellHeight) + marginHeight).ToString())
    let bottomRight = lazy ((((coordinate.ColumnIndex + 1) * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RowIndex + 1) * cellHeight) + marginHeight).ToString())

    let appendOneCell = appendOneCell sBuilder styleClass

    match cell.WallLeft.WallType, cell.WallTop.WallType, cell.WallRight.WallType, cell.WallBottom.WallType with
    | (l, t, r, b) when l = wallType && t = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine LeftToRight) + (appendLine TopToBottom) + (appendLine RightToLeft) + (appendLine BottomToTop))
    | (l, t, r, _) when l = wallType && t = wallType && r = wallType ->
        appendOneCell ("M " + bottomRight.Value + (appendLine BottomToTop) + (appendLine RightToLeft) + (appendLine TopToBottom))
    | (l, t, _, b) when l = wallType && t = wallType && b = wallType ->
        appendOneCell ("M " + bottomRight.Value + (appendLine RightToLeft) + (appendLine BottomToTop) + (appendLine LeftToRight))
    | (l, _, r, b) when l = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine TopToBottom) + (appendLine LeftToRight) + (appendLine BottomToTop))
    | (_, t, r, b) when t = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine LeftToRight) + (appendLine TopToBottom) + (appendLine RightToLeft))
    | (l, t, _, _) when l = wallType && t = wallType ->
        appendOneCell ("M " + bottomLeft.Value + (appendLine BottomToTop) + (appendLine LeftToRight))
    | (l, _, r, _) when l = wallType && r = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine TopToBottom))
        appendOneCell ("M " + bottomRight.Value + (appendLine BottomToTop))
    | (_, t, r, _) when t = wallType && r = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine LeftToRight) + (appendLine TopToBottom))
    | (_, t, _, b) when t = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine LeftToRight))
        appendOneCell ("M " + bottomRight.Value + (appendLine RightToLeft))
    | (_, _, r, b) when r = wallType && b = wallType ->
        appendOneCell ("M " + bottomLeft.Value + (appendLine LeftToRight) + (appendLine BottomToTop))
    | (l, _, _, b) when l = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine TopToBottom) + (appendLine LeftToRight))
    | (l, _, _, _) when l = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine TopToBottom))
    | (_, t, _, _) when t = wallType ->
        appendOneCell ("M " + topLeft.Value + (appendLine LeftToRight))
    | (_, _, r, _) when r = wallType ->
        appendOneCell ("M " + bottomRight.Value + (appendLine BottomToTop))
    | (_, _, _, b) when b = wallType ->
        appendOneCell ("M " + bottomRight.Value + (appendLine RightToLeft))
    | _ -> ()

let renderWallTypes (sBuilder : StringBuilder) (grid : Grid) coordinate =
    renderWallType sBuilder grid coordinate Border "b"
    renderWallType sBuilder grid coordinate Normal "n"

let renderCell (sBuilder : StringBuilder) (grid : Grid) coordinate =
    let topLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RowIndex * cellHeight) + marginHeight).ToString())    
    appendOneCell sBuilder "p" ("M " + topLeft.Value + (appendLine LeftToRight) + (appendLine TopToBottom) + (appendLine RightToLeft) + (appendLine BottomToTop))

let renderGrid2 grid (path : Coordinate seq) =
    let sBuilder = StringBuilder()

    let svgStyle =
        """<defs>
                <style>
                    .n {
                        stroke: #333;
                        fill:transparent;
                        stroke-width: 2;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                        //stroke-dasharray: 5;
                    }
                    .b {
                        stroke: #333;
                        fill:transparent;
                        stroke-width: 3;
                        stroke-linecap:round;
                        stroke-linejoin: round;
                        //stroke-dasharray: 5;
                    }
                    .p {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: purple;
                        fill-opacity: 0.4;
                    }
                </style>
            </defs>"""

    let width = ((grid.Canvas.NumberOfColumns) * cellWidth) + (marginWidth * 2)
    let height = ((grid.Canvas.NumberOfRows) * cellHeight) + (marginHeight * 2)

    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>") |> ignore
    sBuilder.Append("<svg width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">") |> ignore
    sBuilder.Append(svgStyle) |> ignore
    sBuilder.Append("<rect width=\"100%\" height=\"100%\" fill=\"#4287f5\"/>") |> ignore
    // #4287f5
    // #63a873

    path
    |> Seq.iter(fun coordinate -> renderCell sBuilder grid coordinate)

    grid.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
    |> Seq.iter(fun (_, coordinate) -> renderWallTypes sBuilder grid coordinate)

    sBuilder.Append("</svg>") |> ignore
    sBuilder.ToString()