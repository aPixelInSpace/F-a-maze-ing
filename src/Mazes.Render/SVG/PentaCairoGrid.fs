// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PentaCairoGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.PentaCairo
open Mazes.Render.SVG.Base

let convertToRadian angleInDegree =
        (angleInDegree * Math.PI) / 180.0

let private appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

// todo : clean the functions

let calculatePointD (marginWidth, marginHeight, pentSmallSide, hypDoubleGreatSide, thetaDegBase) coordinate =
    let thetaDegGreatUp = (90.0 - thetaDegBase)

    // small side up    
    let thetaDegSmallBottom = 180.0 - 120.0 - thetaDegGreatUp
    let thetaDegSmallRight = 180.0 - 90.0 - thetaDegSmallBottom
    let lenghtSmallSide = Math.Abs((pentSmallSide * Math.Sin(convertToRadian thetaDegSmallRight)) / Math.Sin(convertToRadian 90.0))

    (marginWidth + (float)(coordinate.CIndex / 2) * hypDoubleGreatSide, marginHeight + (float)(coordinate.RIndex / 2) * hypDoubleGreatSide + lenghtSmallSide)
        

let private calculatePointsQuadrantOne (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) coordinate =
    let (dx, dy) = calculatePointD (marginWidth, marginHeight, pentSmallSide, hypDoubleGreatSide, thetaDegBase) coordinate

    let thetaRadBase = convertToRadian thetaDegBase
    let (cx, cy) = calculatePoint (dx, dy) thetaRadBase pentGreatSide

    let theta360 = convertToRadian 360.0
    let thetaB = theta360 - theta90 + thetaRadBase
    let (bx, by) = calculatePoint (cx, cy) thetaB pentGreatSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA pentGreatSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS pentGreatSide

    ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy))

let private calculatePointsQuadrantTwo (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) coordinate =
    let (dxTemp, dyTemp) = calculatePointD (marginWidth, marginHeight, pentSmallSide, hypDoubleGreatSide, thetaDegBase) {coordinate with CIndex = coordinate.CIndex + 1}
    
    let thetaD = thetaDegBase + 45.0
    
    let (dx, dy) = calculatePoint (dxTemp, dyTemp) (convertToRadian thetaD) hypGreatSide

    let thetaRadBase = convertToRadian thetaDegBase + convertToRadian 270.0

    let (cx, cy) = calculatePoint (dx, dy) thetaRadBase pentGreatSide

    let theta360 = convertToRadian 360.0
    let thetaB = theta360 - theta90 + thetaRadBase
    let (bx, by) = calculatePoint (cx, cy) thetaB pentGreatSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA pentGreatSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS pentGreatSide

    ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy))

let private calculatePointsQuadrantThree (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) coordinate =
    let (dxTemp, dyTemp) = calculatePointD (marginWidth, marginHeight, pentSmallSide, hypDoubleGreatSide, thetaDegBase) coordinate
    
    let thetaD = thetaDegBase - 45.0
    
    let (dx, dy) = calculatePoint (dxTemp, dyTemp) (convertToRadian thetaD) hypGreatSide

    let thetaRadBase = convertToRadian thetaDegBase + convertToRadian 90.0

    let (cx, cy) = calculatePoint (dx, dy) thetaRadBase pentGreatSide

    let theta360 = convertToRadian 360.0
    let thetaB = theta360 - theta90 + thetaRadBase
    let (bx, by) = calculatePoint (cx, cy) thetaB pentGreatSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA pentGreatSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS pentGreatSide

    ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy))

let private calculatePointsQuadrantFour (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) coordinate =
    let (dxTemp, dyTemp) = calculatePointD (marginWidth, marginHeight, pentSmallSide, hypDoubleGreatSide, thetaDegBase) { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex + 1}
    
    let thetaD = thetaDegBase    
    let (dx, dy) = calculatePoint (dxTemp, dyTemp) (convertToRadian thetaD) (2.0 * pentGreatSide)

    let thetaRadBase = convertToRadian thetaDegBase + convertToRadian 180.0

    let (cx, cy) = calculatePoint (dx, dy) thetaRadBase pentGreatSide

    let theta360 = convertToRadian 360.0
    let thetaB = theta360 - theta90 + thetaRadBase
    let (bx, by) = calculatePoint (cx, cy) thetaB pentGreatSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA pentGreatSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS pentGreatSide

    ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy))

let private appendWallsType (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) (grid : PentaCairoGrid) coordinate (sBuilder : StringBuilder) =

    let cairoPentagonValues = (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint)
    
    let cell = grid.Cell coordinate

    let appendWall s position =
        appendWall sBuilder s (cell.WallTypeAtPosition position) |> ignore

    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        match PentaCairoPositionHandler.Quadrant coordinate with
        | One -> calculatePointsQuadrantOne cairoPentagonValues coordinate
        | Two -> calculatePointsQuadrantTwo cairoPentagonValues coordinate
        | Three -> calculatePointsQuadrantThree cairoPentagonValues coordinate
        | Four -> calculatePointsQuadrantFour cairoPentagonValues coordinate

    for position in PentaCairoPositionHandler.Instance.Values coordinate do
        match position with
        | S -> appendWall $"M {round dx} {round dy} L {round sx} {round sy}" position
        | A -> appendWall $"M {round sx} {round sy} L {round ax} {round ay}" position
        | B -> appendWall $"M {round ax} {round ay} L {round bx} {round by}" position
        | C -> appendWall $"M {round bx} {round by} L {round cx} {round cy}" position
        | D -> appendWall $"M {round cx} {round cy} L {round dx} {round dy}" position

let private wholeCellLines (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint) coordinate =
    let cairoPentagonValues = (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint)
    
    let ((sx,sy), (ax,ay), (bx,by), (cx,cy), (dx,dy)) =
        match PentaCairoPositionHandler.Quadrant coordinate with
        | One -> calculatePointsQuadrantOne cairoPentagonValues coordinate
        | Two -> calculatePointsQuadrantTwo cairoPentagonValues coordinate
        | Three -> calculatePointsQuadrantThree cairoPentagonValues coordinate
        | Four -> calculatePointsQuadrantFour cairoPentagonValues coordinate

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
    
    // the hypotenuses of the triangle with the greater side of the pentagon and the smaller side of the pentagon  
    let hypSmallSide = Math.Sqrt(pentGreatSide ** 2.0 + pentSmallSide ** 2.0 - 2.0 * pentGreatSide * pentSmallSide * Math.Cos(convertToRadian 120.0))

    // the hypotenuses of the triangle with the greater side of the pentagon * 2 and the smaller side of the pentagon
    let hypDoubleGreatSide = Math.Sqrt((pentGreatSide * 2.0) ** 2.0 + pentSmallSide ** 2.0 - 2.0 * (pentGreatSide * 2.0) * pentSmallSide * Math.Cos(convertToRadian 120.0))

    let thetaDegBase = 75.0 //430.0
    let theta75 = convertToRadian 75.0
    let theta90 = convertToRadian 90.0
    let theta45 = convertToRadian 45.0
    let theta30 = convertToRadian 30.0
    let theta60 = convertToRadian 60.0

    let calculatePoint (baseX, baseY) theta distance =
        (distance * Math.Cos(theta) + baseX, distance * Math.Sin(theta) + baseY)

    let cairoPentagonValues = (marginWidth, marginHeight, pentGreatSide, hypGreatSide, pentSmallSide, hypDoubleGreatSide, thetaDegBase, theta75, theta90, theta45, theta30, theta60, calculatePoint)

    // todo : calculate the right distance
    let calculateLength numberOf =
        numberOf * (hypDoubleGreatSide / 2.0) + pentGreatSide

    let width = calculateLength ((float)grid.NumberOfColumns) + marginWidth * 2.0
    let height = calculateLength ((float)grid.NumberOfRows) + marginHeight * 2.0

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map (wholeCellLines cairoPentagonValues)
    |> appendPathWithAnimation path (wholeCellLines cairoPentagonValues)
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze (appendWallsType cairoPentagonValues grid)
    |> appendFooter
    |> ignore

    sBuilder.ToString()