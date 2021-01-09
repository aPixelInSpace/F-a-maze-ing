// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PentaCairoGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.PentaCairo
open Mazes.Render.SVG.Base

let private thetaDegBase = 75.0
let private thetaRadBase = convertToRadian thetaDegBase
let private theta60 = convertToRadian 60.0
let private theta90 = convertToRadian 90.0
let private theta180 = convertToRadian 180.0
let private theta270 = convertToRadian 270.0
let private theta360 = convertToRadian 360.0

let private calculatePointD (marginWidth, marginHeight, hypDoubleGreatSide, lenghtSmallSide) coordinate =
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

let private calculatePointsQuadrantOne calculatePointD calculatePointsQuadrant coordinate =
    let quadrantOnePointD = calculatePointD coordinate
    
    calculatePointsQuadrant
        quadrantOnePointD
        thetaRadBase

let private thetaQuadrantTwoTranslation = convertToRadian (thetaDegBase + 45.0)
let private thetaQuadrantTwoRotation = thetaRadBase + theta270
let private calculatePointsQuadrantTwo calculatePointD calculatePointsQuadrant hypGreatSide coordinate =
    let quadrantOnePointD = calculatePointD {coordinate with CIndex = coordinate.CIndex + 1}
    let quadrantTwoPointD = calculatePoint quadrantOnePointD thetaQuadrantTwoTranslation hypGreatSide

    calculatePointsQuadrant
        quadrantTwoPointD
        thetaQuadrantTwoRotation

let private thetaQuadrantThreeTranslation = convertToRadian (thetaDegBase - 45.0)
let private thetaQuadrantThreeRotation = thetaRadBase + theta90
let private calculatePointsQuadrantThree calculatePointD calculatePointsQuadrant hypGreatSide coordinate =
    let quadrantOnePointD = calculatePointD coordinate
    let quadrantThreePointD = calculatePoint quadrantOnePointD thetaQuadrantThreeTranslation hypGreatSide

    calculatePointsQuadrant
        quadrantThreePointD
        thetaQuadrantThreeRotation

let private thetaQuadrantFourRotation = thetaRadBase + theta180
let private calculatePointsQuadrantFour calculatePointD calculatePointsQuadrant pentGreatSide coordinate =
    let quadrantOnePointD = calculatePointD { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex + 1}
    let quadrantFourPointD = calculatePoint quadrantOnePointD thetaRadBase (2.0 * pentGreatSide)

    calculatePointsQuadrant
        quadrantFourPointD
        thetaQuadrantFourRotation

let private appendWallsType calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide) (grid : PentaCairoGrid) coordinate (sBuilder : StringBuilder) =
    let cell = grid.Cell coordinate

    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        match PentaCairoPositionHandler.Quadrant coordinate with
        | One -> calculatePointsQuadrantOne calculatePointD calculatePointsQuadrant coordinate
        | Two -> calculatePointsQuadrantTwo calculatePointD calculatePointsQuadrant hypGreatSide coordinate
        | Three -> calculatePointsQuadrantThree calculatePointD calculatePointsQuadrant hypGreatSide coordinate
        | Four -> calculatePointsQuadrantFour calculatePointD calculatePointsQuadrant pentGreatSide coordinate

    for position in PentaCairoPositionHandler.Instance.Values coordinate do
        let lines =
            match position with
            | S -> $"M {round dx} {round dy} L {round sx} {round sy}"
            | A -> $"M {round sx} {round sy} L {round ax} {round ay}"
            | B -> $"M {round ax} {round ay} L {round bx} {round by}"
            | C -> $"M {round bx} {round by} L {round cx} {round cy}"
            | D -> $"M {round cx} {round cy} L {round dx} {round dy}"

        appendWall sBuilder lines (cell.WallTypeAtPosition position) |> ignore

let private wholeCellLines calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide) coordinate =
    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        match PentaCairoPositionHandler.Quadrant coordinate with
        | One -> calculatePointsQuadrantOne calculatePointD calculatePointsQuadrant coordinate
        | Two -> calculatePointsQuadrantTwo calculatePointD calculatePointsQuadrant hypGreatSide coordinate
        | Three -> calculatePointsQuadrantThree calculatePointD calculatePointsQuadrant hypGreatSide coordinate
        | Four -> calculatePointsQuadrantFour calculatePointD calculatePointsQuadrant pentGreatSide coordinate

    $"M {round sx} {round sy} " +
    $"L {round ax} {round ay} " +
    $"L {round bx} {round by} " +
    $"L {round cx} {round cy} " +
    $"L {round dx} {round dy}"

let render (grid : PentaCairoGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    // the greater side of the pentagon
    let pentGreatSide = 15.0

    // the hypotenuses of the isosceles right triangle with pentGreatSide
    let hypGreatSide = Math.Sqrt(2.0 * (pentGreatSide ** 2.0))
        
    // the smaller side of the pentagon
    let pentSmallSide = Math.Sqrt(hypGreatSide ** 2.0 + hypGreatSide ** 2.0 - 2.0 * hypGreatSide * hypGreatSide * Math.Cos(convertToRadian 30.0))

    // the hypotenuses of the triangle with the greater side of the pentagon * 2 and the smaller side of the pentagon
    let hypDoubleGreatSide = Math.Sqrt((pentGreatSide * 2.0) ** 2.0 + pentSmallSide ** 2.0 - 2.0 * (pentGreatSide * 2.0) * pentSmallSide * Math.Cos(convertToRadian 120.0))

    // small side up
    let lenghtSmallSide =
        let thetaDegGreatUp = 90.0 - thetaDegBase
        let thetaDegSmallBottom = 180.0 - 120.0 - thetaDegGreatUp
        let thetaDegSmallRight = convertToRadian (180.0 - 90.0 - thetaDegSmallBottom)

        Math.Abs((pentSmallSide * Math.Sin(convertToRadian thetaDegSmallRight)) / Math.Sin(theta90))

    let calculatePointD = calculatePointD (marginWidth, marginHeight, hypDoubleGreatSide, lenghtSmallSide)

    let calculatePointsQuadrant = calculatePointsQuadrant pentGreatSide

    let calculateLength numberOf =
        numberOf * (hypDoubleGreatSide / 2.0) + pentGreatSide

    let width = calculateLength ((float)grid.NumberOfColumns) + marginWidth * 2.0
    let height = calculateLength ((float)grid.NumberOfRows) + marginHeight * 2.0

    let wholeCellLines = wholeCellLines calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide)
    let appendWallsType = appendWallsType calculatePointD calculatePointsQuadrant (pentGreatSide, hypGreatSide) grid

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map wholeCellLines
    |> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves wholeCellLines
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType
    |> appendFooter
    |> ignore

    sBuilder.ToString()