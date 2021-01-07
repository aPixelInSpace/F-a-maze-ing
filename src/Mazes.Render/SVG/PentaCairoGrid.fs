// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PentaCairoGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.PentaCairo
open Mazes.Render.SVG.Base

let private appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

let private appendPentagonCairo (aSide, bSide, cSide, thetaBase, theta75, theta90, theta45, theta30, theta60, calculatePoint, convertToRadian) (sBuilder : StringBuilder) =
    let theta360 = convertToRadian 360.0

    let (dx, dy) = (50.0, 50.0)
    let (cx, cy) = calculatePoint (dx, dy) thetaBase aSide

    let thetaB = theta360 - theta90 + thetaBase
    let (bx, by) = calculatePoint (cx, cy) thetaB aSide

    let thetaA = theta360 - theta60 + thetaB
    let (ax, ay) = calculatePoint (bx, by) thetaA aSide

    let thetaS = theta360 - theta90 + thetaA
    let (sx, sy) = calculatePoint (ax, ay) thetaS aSide

    let appendWall s position =
        appendWall sBuilder s Normal |> ignore

    appendWall $"M {round dx} {round dy} L {round cx} {round cy} L {round bx} {round by} L {round ax} {round ay} L {round sx} {round sy} L {round dx} {round dy}" S

    appendWall $"M {round ax} {round ay} L {round cx} {round cy}" S

    sBuilder    

let render (grid : PentaCairoGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    // the greater side of the pentagon
    let aSide = 15.0

    // the hypotenuses of the isosceles right triangle with aSide
    let bSide = Math.Sqrt(2.0 * (aSide ** 2.0))
    
    // the smaller side of the pentagon
    let cSide = Math.Sqrt(2.0 * (bSide ** 2.0) * (1.0 - Math.Cos(30.0)))

    let convertToRadian angleInDegree =
        (angleInDegree * Math.PI) / 180.0

    let thetaBase = convertToRadian 70.0
    let theta75 = convertToRadian 75.0
    let theta90 = convertToRadian 90.0
    let theta45 = convertToRadian 45.0
    let theta30 = convertToRadian 30.0
    let theta60 = convertToRadian 60.0

    let calculatePoint (baseX, baseY) theta distance =
        (distance * Math.Cos(theta) + baseX, distance * Math.Sin(theta) + baseY)

    let cairoPentagonValues = (aSide, bSide, cSide, thetaBase, theta75, theta90, theta45, theta30, theta60, calculatePoint, convertToRadian)

    // todo : calculate the right distance
    let calculateLength numberOf =
        numberOf * aSide

    let width = calculateLength ((float)grid.NumberOfColumns) + marginWidth
    let height = calculateLength ((float)grid.NumberOfRows) + marginHeight

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "yellow"
    |> appendPentagonCairo cairoPentagonValues
    //|> appendMazeColoration map (wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize))
    //|> appendPathWithAnimation path (wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize))
    //|> appendWalls grid.ToInterface.CoordinatesPartOfMaze (appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) grid)
    |> appendFooter
    |> ignore

    sBuilder.ToString()