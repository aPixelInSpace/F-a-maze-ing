// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PentaCairoGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.PentaCairo
open Mazes.Render.SVG.Base

let private thetaDegBase = 75.0
let private thetaRadBase = convertToRadian thetaDegBase
let private theta60 = convertToRadian 60.0
let private theta90 = convertToRadian 90.0
let private theta180 = convertToRadian 180.0
let private theta270 = convertToRadian 270.0
let private theta360 = convertToRadian 360.0

let private calculatePointD (marginWidth, marginHeight, hypDoubleGreatSide, lenghtSmallSide) (coordinate : Coordinate2D) =
    (marginWidth + (float)(coordinate.CIndex / 2) * hypDoubleGreatSide, marginHeight + (float)(coordinate.RIndex / 2) * hypDoubleGreatSide + lenghtSmallSide)

let private calculatePointsQuadrant pentGreatSide pointD thetaRotation =
    let (cx, cy) = calculatePoint pointD thetaRotation pentGreatSide

    let thetaB = theta360 - theta90 + thetaRotation
    let (bx, by) = calculatePoint (cx, cy) thetaB pentGreatSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA pentGreatSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS pentGreatSide

    ((sx,sy), (ax,ay), (bx,by), (cx,cy), pointD)

let private calculatePointsQuadrantOne calculatePointD calculatePointsQuadrant (coordinate : NCoordinate) =
    let quadrantOnePointD = calculatePointD coordinate.ToCoordinate2D
    
    calculatePointsQuadrant
        quadrantOnePointD
        thetaRadBase

let private thetaQuadrantTwoTranslation = convertToRadian (thetaDegBase + 45.0)
let private thetaQuadrantTwoRotation = thetaRadBase + theta270
let private calculatePointsQuadrantTwo calculatePointD calculatePointsQuadrant hypGreatSide (coordinate : NCoordinate) =
    let quadrantOnePointD = calculatePointD {coordinate.ToCoordinate2D with CIndex = coordinate.ToCoordinate2D.CIndex + 1}
    let quadrantTwoPointD = calculatePoint quadrantOnePointD thetaQuadrantTwoTranslation hypGreatSide

    calculatePointsQuadrant
        quadrantTwoPointD
        thetaQuadrantTwoRotation

let private thetaQuadrantThreeTranslation = convertToRadian (thetaDegBase - 45.0)
let private thetaQuadrantThreeRotation = thetaRadBase + theta90
let private calculatePointsQuadrantThree calculatePointD calculatePointsQuadrant hypGreatSide (coordinate : NCoordinate) =
    let quadrantOnePointD = calculatePointD coordinate.ToCoordinate2D
    let quadrantThreePointD = calculatePoint quadrantOnePointD thetaQuadrantThreeTranslation hypGreatSide

    calculatePointsQuadrant
        quadrantThreePointD
        thetaQuadrantThreeRotation

let private thetaQuadrantFourRotation = thetaRadBase + theta180
let private calculatePointsQuadrantFour calculatePointD calculatePointsQuadrant pentGreatSide (coordinate : NCoordinate) =
    let quadrantOnePointD = calculatePointD { RIndex = coordinate.ToCoordinate2D.RIndex - 1; CIndex = coordinate.ToCoordinate2D.CIndex + 1}
    let quadrantFourPointD = calculatePoint quadrantOnePointD thetaRadBase (2.0 * pentGreatSide)

    calculatePointsQuadrant
        quadrantFourPointD
        thetaQuadrantFourRotation

let private calculatePoints calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide) (coordinate : NCoordinate) =
    match PentaCairoPositionHandler.Quadrant coordinate.ToCoordinate2D with
    | One -> calculatePointsQuadrantOne calculatePointD calculatePointsQuadrant coordinate
    | Two -> calculatePointsQuadrantTwo calculatePointD calculatePointsQuadrant hypGreatSide coordinate
    | Three -> calculatePointsQuadrantThree calculatePointD calculatePointsQuadrant hypGreatSide coordinate
    | Four -> calculatePointsQuadrantFour calculatePointD calculatePointsQuadrant pentGreatSide coordinate

let center calculatePoints coordinate =
    let (s, _, b, _, d) = calculatePoints coordinate

    let middleLittleSide =
        middlePoint s d

    middlePoint middleLittleSide b // this is a good enough approximation

let private appendWallsType calculatePoints (grid : IAdjacentStructure<GridArray2D<PentaCairoPosition>, PentaCairoPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        calculatePoints coordinate

    for position in PentaCairoPositionHandler.Instance.Values coordinate2D do
        let lines =
            match position with
            | S -> $"M {round dx} {round dy} L {round sx} {round sy}"
            | A -> $"M {round sx} {round sy} L {round ax} {round ay}"
            | B -> $"M {round ax} {round ay} L {round bx} {round by}"
            | C -> $"M {round bx} {round by} L {round cx} {round cy}"
            | D -> $"M {round cx} {round cy} L {round dx} {round dy}"

        appendWall sBuilder lines (cell.ConnectionTypeAtPosition position) coordinate |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        calculatePoints coordinate

    $"M {round sx} {round sy} " +
    $"L {round ax} {round ay} " +
    $"L {round bx} {round by} " +
    $"L {round cx} {round cy} " +
    $"L {round dx} {round dy}"

let render (grid : NDimensionalStructure<GridArray2D<PentaCairoPosition>, PentaCairoPosition>) path map entrance exit =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    // the greater side of the pentagon
    let pentGreatSide = 30.0

    // the hypotenuses of the isosceles right triangle with pentGreatSide
    let hypGreatSide = Math.Sqrt(2.0 * (pentGreatSide ** 2.0))
        
    // the smaller side of the pentagon
    let pentSmallSide = Math.Sqrt(hypGreatSide ** 2.0 + hypGreatSide ** 2.0 - 2.0 * hypGreatSide * hypGreatSide * Math.Cos(convertToRadian 30.0))

    // the hypotenuses of the triangle with the greater side of the pentagon * 2 and the smaller side of the pentagon
    let hypDoubleGreatSide = Math.Sqrt((pentGreatSide * 2.0) ** 2.0 + pentSmallSide ** 2.0 - 2.0 * (pentGreatSide * 2.0) * pentSmallSide * Math.Cos(convertToRadian 120.0))

    // small side up
    let lenghtSmallSide = sideAngleAngle pentSmallSide (convertToRadian 45.0) (convertToRadian 90.0)

    let bridgeHalfWidth = 7.0
    let bridgeDistanceFromCenter = 5.0

    let (dimension, slice2D) = grid.FirstSlice2D

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePointD = calculatePointD (marginWidth, marginHeight, hypDoubleGreatSide, lenghtSmallSide)
    let calculatePointsQuadrant = calculatePointsQuadrant pentGreatSide
    let calculatePoints = calculatePoints calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide)

    let center = (center calculatePoints)

    let calculatePointsBridge = calculatePointsBridge center bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors

    let calculateLength numberOf =
        numberOf * (hypDoubleGreatSide / 2.0) + pentGreatSide

    let width = calculateLength ((float)slice2D.ToSpecializedStructure.NumberOfColumns) + marginWidth * 2.0
    let height = calculateLength ((float)slice2D.ToSpecializedStructure.NumberOfRows) + marginHeight * 2.0

    let wholeCellLines = wholeCellLines calculatePoints
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge

    let appendWallsType = appendWallsType calculatePoints slice2D

    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    let colorPicker coordinate = None

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"

    |> appendMazeDistanceColoration map wholeCellLines

    //|> appendPathWithAnimation path wholeCellLines
    //|> appendPathAndBridgesWithAnimation
    //|> appendLeaves map.Leaves wholeCellLines

    |> appendSimpleWalls
    //|> appendWallsWithInset

    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines colorPicker
    |> appendMazeDistanceBridgeColoration nonAdjacentNeighbors map wholeBridgeLines
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell center entrance "start"
    |> textCell center exit "exit"

    |> appendFooter
    |> ignore

    sBuilder.ToString()