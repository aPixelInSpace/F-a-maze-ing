// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Render.SVG.Base

let private cellWidth = 30
let private cellHeight = 30
let private marginWidth = 20
let private marginHeight = 20

type private Direction =
    | LeftToRight
    | RightToLeft
    | TopToBottom
    | BottomToTop

let private line direction =
    match direction with
    | LeftToRight -> " h " + cellWidth.ToString()
    | RightToLeft -> " h -" + cellWidth.ToString()
    | TopToBottom -> " v " + cellHeight.ToString()
    | BottomToTop -> " v -" + cellHeight.ToString()

let private appendWall (sBuilder : StringBuilder) (grid : OrthoGrid) coordinate (wallType : WallType) styleClass =
    let cell = grid.Cell coordinate

    let topLeft = lazy (((coordinate.CIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RIndex * cellHeight) + marginHeight).ToString())
    let bottomLeft = lazy (((coordinate.CIndex * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RIndex + 1) * cellHeight) + marginHeight).ToString())
    let bottomRight = lazy ((((coordinate.CIndex + 1) * cellWidth) + marginWidth).ToString() + " " + (((coordinate.RIndex + 1) * cellHeight) + marginHeight).ToString())

    let addP = appendPathElement sBuilder None styleClass

    let wallTypeLeft = cell.WallTypeAtPosition Left
    let wallTypeTop = cell.WallTypeAtPosition Top
    let wallTypeRight = cell.WallTypeAtPosition Right
    let wallTypeBottom = cell.WallTypeAtPosition Bottom

    match wallTypeLeft, wallTypeTop, wallTypeRight, wallTypeBottom with
    | (l, t, r, b) when l = wallType && t = wallType && r = wallType && b = wallType ->
        addP ("M " + topLeft.Value + (line LeftToRight) + (line TopToBottom) + (line RightToLeft) + (line BottomToTop))
    | (l, t, r, _) when l = wallType && t = wallType && r = wallType ->
        addP ("M " + bottomRight.Value + (line BottomToTop) + (line RightToLeft) + (line TopToBottom))
    | (l, t, _, b) when l = wallType && t = wallType && b = wallType ->
        addP ("M " + bottomRight.Value + (line RightToLeft) + (line BottomToTop) + (line LeftToRight))
    | (l, _, r, b) when l = wallType && r = wallType && b = wallType ->
        addP ("M " + topLeft.Value + (line TopToBottom) + (line LeftToRight) + (line BottomToTop))
    | (_, t, r, b) when t = wallType && r = wallType && b = wallType ->
        addP ("M " + topLeft.Value + (line LeftToRight) + (line TopToBottom) + (line RightToLeft))
    | (l, t, _, _) when l = wallType && t = wallType ->
        addP ("M " + bottomLeft.Value + (line BottomToTop) + (line LeftToRight))
    | (l, _, r, _) when l = wallType && r = wallType ->
        addP ("M " + topLeft.Value + (line TopToBottom)) |> ignore
        addP ("M " + bottomRight.Value + (line BottomToTop))
    | (_, t, r, _) when t = wallType && r = wallType ->
        addP ("M " + topLeft.Value + (line LeftToRight) + (line TopToBottom))
    | (_, t, _, b) when t = wallType && b = wallType ->
        addP ("M " + topLeft.Value + (line LeftToRight)) |> ignore
        addP ("M " + bottomRight.Value + (line RightToLeft))
    | (_, _, r, b) when r = wallType && b = wallType ->
        addP ("M " + bottomLeft.Value + (line LeftToRight) + (line BottomToTop))
    | (l, _, _, b) when l = wallType && b = wallType ->
        addP ("M " + topLeft.Value + (line TopToBottom) + (line LeftToRight))
    | (l, _, _, _) when l = wallType ->
        addP ("M " + topLeft.Value + (line TopToBottom))
    | (_, t, _, _) when t = wallType ->
        addP ("M " + topLeft.Value + (line LeftToRight))
    | (_, _, r, _) when r = wallType ->
        addP ("M " + bottomRight.Value + (line BottomToTop))
    | (_, _, _, b) when b = wallType ->
        addP ("M " + bottomRight.Value + (line RightToLeft))
    | _ -> sBuilder

let private appendWallsType (grid : OrthoGrid) coordinate (sBuilder : StringBuilder) =
    appendWall sBuilder grid coordinate Border borderWallClass |> ignore
    appendWall sBuilder grid coordinate Normal normalWallClass

let private wholeCellLines coordinate =
    let topLeft = ((coordinate.CIndex * cellWidth) + marginWidth).ToString() + " " + ((coordinate.RIndex * cellHeight) + marginHeight).ToString()
    "M " + topLeft + (line LeftToRight) + (line TopToBottom) + (line RightToLeft) + (line BottomToTop)

let render (grid : OrthoGrid) (path : Coordinate seq) (map : Map) =
    let sBuilder = StringBuilder()

    let width = ((grid.Canvas.NumberOfColumns) * cellWidth) + (marginWidth * 2)
    let height = ((grid.Canvas.NumberOfRows) * cellHeight) + (marginHeight * 2)

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map wholeCellLines
    //|> appendPath path wholeCellLines
    |> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves wholeCellLines
    |> appendWalls grid.CoordinatesPartOfMaze (appendWallsType grid)
    |> appendFooter
    |> ignore
 
    sBuilder.ToString()