// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.HexGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Hex
open Mazes.Render.SVG.Base

let private calculatePoints (hexEdgeSize, hexHalfEdgeSize, hexWidth, hexHalfHeight, hexHeight, marginWidth, marginHeight) (coordinate : NCoordinate) =
    let lengthAtLeft = (float)coordinate.ToCoordinate2D.CIndex * (3.0 * hexHalfEdgeSize) + marginWidth
    let lengthAtTop =
        match (HexPositionHandler.IsEven coordinate.ToCoordinate2D) with
        | true -> (float)coordinate.ToCoordinate2D.RIndex * hexHeight + hexHalfHeight + marginHeight
        | false -> (float)coordinate.ToCoordinate2D.RIndex * hexHeight + marginHeight

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

let center calculatePoints coordinate =
    let (_, topLeft, _, _, _, bottomRight) = calculatePoints coordinate

    middlePoint topLeft bottomRight

let private appendWallsType calculatePoints (grid : IAdjacentStructure<GridArray2D<HexPosition>, HexPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftX, leftY), (topLeftX, topLeftY), (topRightX, topRightY), (rightX, rightY), (bottomLeftX, bottomLeftY), (bottomRightX, bottomRightY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    for position in HexPositionHandler.Instance.Values coordinate2D do
        let lines =
            match position with
            | TopLeft -> $"M {round leftX} {round leftY} L {round topLeftX} {round topLeftY}"
            | Top -> $"M {round topLeftX} {round topLeftY} L {round topRightX} {round topRightY}"
            | TopRight -> $"M {round topRightX} {round topRightY} L {round rightX} {round rightY}"
            | BottomLeft -> $"M {round leftX} {round leftY} L {round bottomLeftX} {round bottomLeftY}"
            | Bottom -> $"M {round bottomLeftX} {round bottomLeftY} L {round bottomRightX} {round bottomRightY}"
            | BottomRight -> $"M {round bottomRightX} {round bottomRightY} L {round rightX} {round rightY}"

        appendWall sBuilder lines (cell.ConnectionTypeAtPosition position) coordinate |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftX, leftY), (topLeftX, topLeftY), (topRightX, topRightY), (rightX, rightY), (bottomLeftX, bottomLeftY), (bottomRightX, bottomRightY)) =
        calculatePoints coordinate

    $"M {round leftX} {round leftY} " +
    $"L {round topLeftX} {round topLeftY} " +
    $"L {round topRightX} {round topRightY} " +
    $"L {round rightX} {round rightY} " +
    $"L {round bottomRightX} {round bottomRightY} " +
    $"L {round bottomLeftX} {round bottomLeftY} "

let render (grid : NDimensionalStructure<GridArray2D<HexPosition>, HexPosition>) path map entrance exit =

    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let hexEdgeSize = 30.0
    let hexHalfEdgeSize = hexEdgeSize / 2.0
    let hexWidth = hexEdgeSize * 2.0
    let hexHalfHeight = (hexEdgeSize * Math.Sqrt(3.0)) / 2.0
    let hexHeight = hexHalfHeight * 2.0

    let bridgeHalfWidth = 6.0
    let bridgeDistanceFromCenter = 20.0

    let (dimension, slice2D) = grid.FirstSlice2D

    let width = (3.0 * hexHalfEdgeSize * (float)slice2D.ToSpecializedStructure.NumberOfColumns) + hexHalfEdgeSize + (float)(marginWidth * 2)
    let height = (hexHeight * (float)slice2D.ToSpecializedStructure.NumberOfRows) + hexHalfHeight + (float)(marginHeight * 2)

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePoints = calculatePoints (hexEdgeSize, hexHalfEdgeSize, hexWidth, hexHalfHeight, hexHeight, (float)marginWidth, (float)marginHeight)

    let calculatePointsBridge = calculatePointsBridge (center calculatePoints) bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors

    let appendWallsType = appendWallsType calculatePoints slice2D
    let wholeCellLines = wholeCellLines calculatePoints
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge

    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"

    |> appendMazeDistanceColoration map wholeCellLines

    //|> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves (wholeCellLines calculatePoints grid)

    |> appendSimpleWalls
    //|> appendWallsWithInset

    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines
    |> appendMazeDistanceBridgeColoration nonAdjacentNeighbors map wholeBridgeLines
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell (center calculatePoints) entrance "start"
    |> textCell (center calculatePoints) exit "exit"

    |> appendFooter
    |> ignore

    sBuilder.ToString()