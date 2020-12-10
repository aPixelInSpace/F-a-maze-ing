// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG

open System
open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid

let private cellWidth = 30
let private cellHeight = 30
let private marginWidth = 20
let private marginHeight = 20

type private Direction =
    | LeftToRight
    | RightToLeft
    | TopToBottom
    | BottomToTop

let private drawLine direction =
    match direction with
    | LeftToRight -> " h " + cellWidth.ToString()
    | RightToLeft -> " h -" + cellWidth.ToString()
    | TopToBottom -> " v " + cellHeight.ToString()
    | BottomToTop -> " v -" + cellHeight.ToString()

let private addPathTag (sBuilder : StringBuilder) styleClass (points : string) =
    sBuilder.Append("<path d=\"") |> ignore
    sBuilder.Append(points) |> ignore
    sBuilder.Append("\" class=\"" + styleClass + "\"/>\n") |> ignore

let private addPathColorTag (sBuilder : StringBuilder) styleClass color opacity (points : string) =
    sBuilder.Append("<path fill=\"" + color + "\" fill-opacity=\"" + opacity + "\"  d=\"") |> ignore
    sBuilder.Append(points) |> ignore
    sBuilder.Append("\" class=\"" + styleClass + "\"/>\n") |> ignore

let private addPathTagByWallType (sBuilder : StringBuilder) (grid : Grid) coordinate (wallType : WallType) styleClass =
    let cell = grid.Cell coordinate

    let topLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RowIndex * cellHeight) + marginHeight).ToString())
    let bottomLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RowIndex + 1) * cellHeight) + marginHeight).ToString())
    let bottomRight = lazy ((((coordinate.ColumnIndex + 1) * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RowIndex + 1) * cellHeight) + marginHeight).ToString())

    let appendOneCell = addPathTag sBuilder styleClass

    match cell.WallLeft.WallType, cell.WallTop.WallType, cell.WallRight.WallType, cell.WallBottom.WallType with
    | (l, t, r, b) when l = wallType && t = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine LeftToRight) + (drawLine TopToBottom) + (drawLine RightToLeft) + (drawLine BottomToTop))
    | (l, t, r, _) when l = wallType && t = wallType && r = wallType ->
        appendOneCell ("M " + bottomRight.Value + (drawLine BottomToTop) + (drawLine RightToLeft) + (drawLine TopToBottom))
    | (l, t, _, b) when l = wallType && t = wallType && b = wallType ->
        appendOneCell ("M " + bottomRight.Value + (drawLine RightToLeft) + (drawLine BottomToTop) + (drawLine LeftToRight))
    | (l, _, r, b) when l = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine TopToBottom) + (drawLine LeftToRight) + (drawLine BottomToTop))
    | (_, t, r, b) when t = wallType && r = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine LeftToRight) + (drawLine TopToBottom) + (drawLine RightToLeft))
    | (l, t, _, _) when l = wallType && t = wallType ->
        appendOneCell ("M " + bottomLeft.Value + (drawLine BottomToTop) + (drawLine LeftToRight))
    | (l, _, r, _) when l = wallType && r = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine TopToBottom))
        appendOneCell ("M " + bottomRight.Value + (drawLine BottomToTop))
    | (_, t, r, _) when t = wallType && r = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine LeftToRight) + (drawLine TopToBottom))
    | (_, t, _, b) when t = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine LeftToRight))
        appendOneCell ("M " + bottomRight.Value + (drawLine RightToLeft))
    | (_, _, r, b) when r = wallType && b = wallType ->
        appendOneCell ("M " + bottomLeft.Value + (drawLine LeftToRight) + (drawLine BottomToTop))
    | (l, _, _, b) when l = wallType && b = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine TopToBottom) + (drawLine LeftToRight))
    | (l, _, _, _) when l = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine TopToBottom))
    | (_, t, _, _) when t = wallType ->
        appendOneCell ("M " + topLeft.Value + (drawLine LeftToRight))
    | (_, _, r, _) when r = wallType ->
        appendOneCell ("M " + bottomRight.Value + (drawLine BottomToTop))
    | (_, _, _, b) when b = wallType ->
        appendOneCell ("M " + bottomRight.Value + (drawLine RightToLeft))
    | _ -> ()

let private renderWallTypes (sBuilder : StringBuilder) (grid : Grid) coordinate =
    addPathTagByWallType sBuilder grid coordinate Border "b"
    addPathTagByWallType sBuilder grid coordinate Normal "n"

let private renderFullCellPath (sBuilder : StringBuilder) coordinate =
    let topLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RowIndex * cellHeight) + marginHeight).ToString())    
    addPathTag sBuilder "p" ("M " + topLeft.Value + (drawLine LeftToRight) + (drawLine TopToBottom) + (drawLine RightToLeft) + (drawLine BottomToTop))

let private renderFullCellColor (sBuilder : StringBuilder) color coordinate node maxDistance =
    let topLeft = lazy (((coordinate.ColumnIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RowIndex * cellHeight) + marginHeight).ToString())

    let opacity = Math.Round(1.0 - (float (maxDistance - node.DistanceFromRoot) / float maxDistance), 2)
    let sOpacity = opacity.ToString().Replace(",", ".")

    addPathColorTag sBuilder "c" color sOpacity ("M " + topLeft.Value + (drawLine LeftToRight) + (drawLine TopToBottom) + (drawLine RightToLeft) + (drawLine BottomToTop))

let renderGrid grid (path : Coordinate seq) (map : Map) =
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
                    .c {
                        stroke: transparent;
                        stroke-width: 0;
                    }
                </style>
            </defs>"""

    let width = ((grid.Canvas.NumberOfColumns) * cellWidth) + (marginWidth * 2)
    let height = ((grid.Canvas.NumberOfRows) * cellHeight) + (marginHeight * 2)

    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>") |> ignore
    sBuilder.Append("<svg width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">") |> ignore
    sBuilder.Append(svgStyle) |> ignore
    sBuilder.Append("<rect width=\"100%\" height=\"100%\" fill=\"transparent\"/>") |> ignore
    // #4287f5 : blue
    // #63a873 : green
    // #fffaba : pale yellow
    // #fffef0 : very pale yellow

    map.Graph.GetNodeByNode RowsAscendingColumnsAscending (fun node _ -> node.IsSome)
    |> Seq.iter(fun (node, coordinate) -> renderFullCellColor sBuilder "#4287f5" coordinate node.Value map.FarthestFromRoot.Distance)

    path
    |> Seq.iter(fun coordinate -> renderFullCellPath sBuilder coordinate)

    grid.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
    |> Seq.iter(fun (_, coordinate) -> renderWallTypes sBuilder grid coordinate)

    sBuilder.Append("</svg>") |> ignore
    sBuilder.ToString()