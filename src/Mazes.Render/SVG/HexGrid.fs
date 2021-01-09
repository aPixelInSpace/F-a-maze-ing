﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.HexGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Hex
open Mazes.Render.SVG.Base

let private calculatePoints (hexEdgeSize, hexHalfEdgeSize, hexWidth, hexHalfHeight, hexHeight, marginWidth, marginHeight) coordinate =
    let lengthAtLeft = (float)coordinate.CIndex * (3.0 * hexHalfEdgeSize) + marginWidth
    let lengthAtTop =
        match (HexPositionHandler.IsEven coordinate) with
        | true -> (float)coordinate.RIndex * hexHeight + hexHalfHeight + marginHeight
        | false -> (float)coordinate.RIndex * hexHeight + marginHeight

    let leftX = lengthAtLeft
    let leftY = lengthAtTop + hexHalfHeight

    let topLeftX = lengthAtLeft + hexHalfEdgeSize
    let topLeftY = lengthAtTop

    let topRightX = lengthAtLeft + hexHalfEdgeSize + hexEdgeSize
    let topRightY = lengthAtTop

    let rightX = lengthAtLeft + hexWidth
    let rightY = lengthAtTop + hexHalfHeight

    let bottomLeftX = lengthAtLeft + hexHalfEdgeSize
    let bottomLeftY = lengthAtTop + hexHeight

    let bottomRightX = lengthAtLeft + hexHalfEdgeSize + hexEdgeSize
    let bottomRightY = lengthAtTop + hexHeight

    ((leftX, leftY), (topLeftX, topLeftY), (topRightX, topRightY), (rightX, rightY), (bottomLeftX, bottomLeftY), (bottomRightX, bottomRightY))

let private appendWallsType calculatePoints (grid : HexGrid) coordinate (sBuilder : StringBuilder) =
    let ((leftX, leftY), (topLeftX, topLeftY), (topRightX, topRightY), (rightX, rightY), (bottomLeftX, bottomLeftY), (bottomRightX, bottomRightY)) =
        calculatePoints coordinate

    let cell = grid.Cell coordinate

    for position in HexPositionHandler.Instance.Values coordinate do
        let lines =
            match position with
            | TopLeft -> $"M {round leftX} {round leftY} L {round topLeftX} {round topLeftY}"
            | Top -> $"M {round topLeftX} {round topLeftY} L {round topRightX} {round topRightY}"
            | TopRight -> $"M {round topRightX} {round topRightY} L {round rightX} {round rightY}"
            | BottomLeft -> $"M {round leftX} {round leftY} L {round bottomLeftX} {round bottomLeftY}"
            | Bottom -> $"M {round bottomLeftX} {round bottomLeftY} L {round bottomRightX} {round bottomRightY}"
            | BottomRight -> $"M {round bottomRightX} {round bottomRightY} L {round rightX} {round rightY}"

        appendWall sBuilder lines (cell.WallTypeAtPosition position) |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftX, leftY), (topLeftX, topLeftY), (topRightX, topRightY), (rightX, rightY), (bottomLeftX, bottomLeftY), (bottomRightX, bottomRightY)) =
        calculatePoints coordinate

    $"M {round leftX} {round leftY} " +
    $"L {round topLeftX} {round topLeftY} " +
    $"L {round topRightX} {round topRightY} " +
    $"L {round rightX} {round rightY} " +
    $"L {round bottomRightX} {round bottomRightY} " +
    $"L {round bottomLeftX} {round bottomLeftY} "

let render (grid : HexGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let hexEdgeSize = 15.0
    let hexHalfEdgeSize = hexEdgeSize / 2.0
    let hexWidth = hexEdgeSize * 2.0
    let hexHalfHeight = (hexEdgeSize * Math.Sqrt(3.0)) / 2.0
    let hexHeight = hexHalfHeight * 2.0

    let width = (3.0 * hexHalfEdgeSize * (float)grid.NumberOfColumns) + hexHalfEdgeSize + (float)(marginWidth * 2)
    let height = (hexHeight * (float)grid.NumberOfRows) + hexHalfHeight + (float)(marginHeight * 2)

    let calculatePoints = calculatePoints (hexEdgeSize, hexHalfEdgeSize, hexWidth, hexHalfHeight, hexHeight, (float)marginWidth, (float)marginHeight)

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map (wholeCellLines calculatePoints)
    |> appendPathWithAnimation path (wholeCellLines calculatePoints)
    //|> appendLeaves map.Leaves (wholeCellLines calculatePoints grid)
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze (appendWallsType calculatePoints grid)
    |> appendFooter
    |> ignore

    sBuilder.ToString()