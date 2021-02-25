// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.TriGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid
open Mazes.Core.Grid.Type.Tri
open Mazes.Render.SVG.Base

let private calculatePoints (calculateWidth, calculateHeight, isUpright, triWidth, triHalfWidth, triHeight) (coordinate : NCoordinate) =
    let baseLengthAtRight = calculateWidth ((float)(coordinate.ToCoordinate2D.CIndex + 1))
    let baseLengthAtBottom = calculateHeight ((float)(coordinate.ToCoordinate2D.RIndex + 1))
    let baseLengthAtLeft = baseLengthAtRight - triWidth
    let baseLengthAtTop = baseLengthAtBottom - triHeight

    let isUpright = isUpright coordinate.ToCoordinate2D

    let leftX = baseLengthAtLeft
    let middleX = baseLengthAtLeft + triHalfWidth
    let rightX = baseLengthAtRight

    let (leftY, middleY, rightY) =
        match isUpright with
        | true -> (baseLengthAtBottom, baseLengthAtTop, baseLengthAtBottom)
        | false -> (baseLengthAtTop, baseLengthAtBottom, baseLengthAtTop)

    ((leftX, leftY), (middleX, middleY), (rightX, rightY))

let center calculatePoints isUpright halfTriHeight (coordinate : NCoordinate) =
    let (_, middle, _) = calculatePoints coordinate

    if isUpright coordinate.ToCoordinate2D then
        translatePoint (0.0, halfTriHeight) middle
    else
        translatePoint (0.0, -halfTriHeight) middle

let private appendWallsType calculatePoints (grid : IAdjacentStructure<GridArray2D<TriPosition>, TriPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    for position in TriPositionHandler.Instance.Values coordinate.ToCoordinate2D do
        let lines =
            match position with
            | Left -> $"M {round leftX} {round leftY} L {round middleX} {round middleY}"
            | Right -> $"M {round rightX} {round rightY} L {round middleX} {round middleY}"
            | Top | Bottom -> $"M {round leftX} {round leftY} L {round rightX} {round rightY}"

        appendWall sBuilder lines (cell.ConnectionTypeAtPosition position) coordinate |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    $"M {round leftX} {round leftY} " +
    $"L {round middleX} {round middleY} " +
    $"L {round rightX} {round rightY} "

let render (grid : NDimensionalStructure<GridArray2D<TriPosition>, TriPosition>) path map entrance exit =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    let triWidth = 30.0
    let triHalfWidth = triWidth / 2.0
    let triHeight = (triWidth * Math.Sqrt(3.0)) / 2.0

    let bridgeHalfWidth = 4.0
    let bridgeDistanceFromCenter = 3.0

    let (dimension, slice2D) = grid.FirstSlice2D

    let isNumberOfColumnsEven = slice2D.ToSpecializedStructure.NumberOfColumns % 2 = 0
    let calculateWidth numberOfColumns =
        if isNumberOfColumnsEven then
            marginWidth + ((numberOfColumns - 1.0) / 2.0) * triWidth + triHalfWidth
        else
            marginWidth + ((numberOfColumns + 1.0) / 2.0) * triWidth

    let calculateHeight numberOfRows = (numberOfRows * triHeight) + marginHeight

    let isUpright = TriPositionHandler.IsUpright

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePoints = calculatePoints (calculateWidth, calculateHeight, isUpright, triWidth, triHalfWidth, triHeight)
    let center = (center calculatePoints isUpright (triHeight / 2.0))

    let calculatePointsBridge = calculatePointsBridge center bridgeHalfWidth bridgeDistanceFromCenter
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

    let width = calculateWidth ((float)slice2D.ToSpecializedStructure.NumberOfColumns) + marginWidth
    let height = calculateHeight ((float)slice2D.ToSpecializedStructure.NumberOfRows) + marginHeight

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

    |> textCell center entrance "start"
    |> textCell center exit "exit"

    |> appendFooter
    |> ignore

    sBuilder.ToString()