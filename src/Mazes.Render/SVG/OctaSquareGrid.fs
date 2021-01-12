﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OctaSquareGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.OctaSquare
open Mazes.Render.SVG.Base

let private calculatePointsOctagon (calculateLength, (octaSquareSideSize : float), (otherSideSize : float)) coordinate =
    let baseLengthAtLeft = calculateLength ((float)(coordinate.CIndex))
    let baseLengthAtTop = calculateLength ((float)(coordinate.RIndex))

    let baseLengthAtMiddleLeft = baseLengthAtLeft + otherSideSize
    let baseLengthAtMiddleRight = baseLengthAtMiddleLeft + octaSquareSideSize
    let baseLengthAtRight = baseLengthAtMiddleRight + otherSideSize

    let baseLengthAtMiddleTop = baseLengthAtTop + otherSideSize
    let baseLengthAtMiddleBottom = baseLengthAtMiddleTop + octaSquareSideSize
    let baseLengthAtBottom = baseLengthAtMiddleBottom + otherSideSize

    let leftTopX = baseLengthAtLeft
    let leftTopY = baseLengthAtMiddleTop

    let topLeftX = baseLengthAtMiddleLeft
    let topLeftY = baseLengthAtTop

    let topRightX = baseLengthAtMiddleRight
    let topRightY = baseLengthAtTop

    let rightTopX = baseLengthAtRight
    let rightTopY = baseLengthAtMiddleTop

    let leftBottomX = baseLengthAtLeft
    let leftBottomY = baseLengthAtMiddleBottom

    let bottomLeftX = baseLengthAtMiddleLeft
    let bottomLeftY = baseLengthAtBottom

    let bottomRightX = baseLengthAtMiddleRight
    let bottomRightY = baseLengthAtBottom

    let rightBottomX = baseLengthAtRight
    let rightBottomY = baseLengthAtMiddleBottom

    ((leftTopX, leftTopY),(topLeftX, topLeftY),(topRightX, topRightY),(rightTopX, rightTopY),(leftBottomX, leftBottomY),(bottomLeftX, bottomLeftY),(bottomRightX, bottomRightY),(rightBottomX, rightBottomY))

let private calculatePointsSquare (calculateLength, (octaSquareSideSize : float), (otherSideSize : float)) coordinate =
    let baseLengthAtLeft = calculateLength ((float)(coordinate.CIndex))
    let baseLengthAtTop = calculateLength ((float)(coordinate.RIndex))

    let baseLengthAtMiddleLeft = baseLengthAtLeft + otherSideSize
    let baseLengthAtMiddleRight = baseLengthAtMiddleLeft + octaSquareSideSize

    let baseLengthAtMiddleTop = baseLengthAtTop + otherSideSize
    let baseLengthAtMiddleBottom = baseLengthAtMiddleTop + octaSquareSideSize

    let leftTopX = baseLengthAtMiddleLeft
    let leftTopY = baseLengthAtMiddleTop

    let rightTopX = baseLengthAtMiddleRight
    let rightTopY = baseLengthAtMiddleTop

    let leftBottomX = baseLengthAtMiddleLeft
    let leftBottomY = baseLengthAtMiddleBottom

    let rightBottomX = baseLengthAtMiddleRight
    let rightBottomY = baseLengthAtMiddleBottom

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY))

let private appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) (grid : OctaSquareGrid) appendWall coordinate (sBuilder : StringBuilder) =

    let cell = grid.Cell coordinate

    if (isOctagon coordinate) then
        let ((leftTopX, leftTopY),(topLeftX, topLeftY),(topRightX, topRightY),(rightTopX, rightTopY),(leftBottomX, leftBottomY),(bottomLeftX, bottomLeftY),(bottomRightX, bottomRightY),(rightBottomX, rightBottomY)) =
            calculatePointsOctagon (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate do
            let lines =
                match position with
                | Left -> $"M {round leftBottomX} {round leftBottomY} L {round leftTopX} {round leftTopY}"
                | TopLeft -> $"M {round leftTopX} {round leftTopY} L {round topLeftX} {round topLeftY}"
                | Top -> $"M {round topLeftX} {round topLeftY} L {round topRightX} {round topRightY}"
                | TopRight -> $"M {round topRightX} {round topRightY} L {round rightTopX} {round rightTopY}"
                | Right -> $"M {round rightTopX} {round rightTopY} L {round rightBottomX} {round rightBottomY}"
                | BottomLeft -> $"M {round leftBottomX} {round leftBottomY} L {round bottomLeftX} {round bottomLeftY}"
                | Bottom -> $"M {round bottomLeftX} {round bottomLeftY} L {round bottomRightX} {round bottomRightY}"
                | BottomRight -> $"M {round bottomRightX} {round bottomRightY} L {round rightBottomX} {round rightBottomY}"

            appendWall sBuilder lines (cell.WallTypeAtPosition position) |> ignore
    else
        let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
            calculatePointsSquare (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate do
            let lines =
                match position with
                | Left -> $"M {round leftBottomX} {round leftBottomY} L {round leftTopX} {round leftTopY}"
                | Top -> $"M {round leftTopX} {round leftTopY} L {round rightTopX} {round rightTopY}"
                | Right -> $"M {round rightBottomX} {round rightBottomY} L {round rightTopX} {round rightTopY}"
                | Bottom -> $"M {round leftBottomX} {round leftBottomY} L {round rightBottomX} {round rightBottomY}"
                | _ -> failwith $"Could not match position {coordinate} at {position}"

            appendWall sBuilder lines (cell.WallTypeAtPosition position) |> ignore

let private wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) coordinate =
    
    if (isOctagon coordinate) then
        let ((leftTopX, leftTopY),(topLeftX, topLeftY),(topRightX, topRightY),(rightTopX, rightTopY),(leftBottomX, leftBottomY),(bottomLeftX, bottomLeftY),(bottomRightX, bottomRightY),(rightBottomX, rightBottomY)) =
            calculatePointsOctagon (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        $"M {round leftBottomX} {round leftBottomY} " +
        $"L {round leftTopX} {round leftTopY} " +
        $"L {round topLeftX} {round topLeftY} " +
        $"L {round topRightX} {round topRightY} " +
        $"L {round rightTopX} {round rightTopY} " +
        $"L {round rightBottomX} {round rightBottomY} " +
        $"L {round bottomRightX} {round bottomRightY} " +
        $"L {round bottomLeftX} {round bottomLeftY}"
    else
        let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
            calculatePointsSquare (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        $"M {round leftBottomX} {round leftBottomY} " +
        $"L {round leftTopX} {round leftTopY} " +
        $"L {round rightTopX} {round rightTopY} " +
        $"L {round rightBottomX} {round rightBottomY}"

let render (grid : OctaSquareGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    let octaSquareSideSize = 30.0 // also hypotenuse of the triangle 
    let otherSideSize = Math.Sqrt((octaSquareSideSize ** 2.0) / 2.0) // by good old Pythagoras's theorem

    let calculateLength numberOf =
        marginWidth + numberOf * (otherSideSize + octaSquareSideSize) + otherSideSize

    let isOctagon = OctaSquarePositionHandler.IsOctagon

    let appendWallsType = appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) grid
    let wholeCellLines = wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize)

    let appendSimpleWalls sBuilder =
        appendSimpleWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder

    let width = calculateLength ((float)grid.NumberOfColumns) + marginWidth + 20.0 // because of the size of the border
    let height = calculateLength ((float)grid.NumberOfRows) + marginHeight + 20.0 // because of the size of the border

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeDistanceColoration map wholeCellLines
    |> appendPathWithAnimation path wholeCellLines
    //|> appendSimpleWalls
    |> appendWallsWithInset
    |> appendFooter
    |> ignore

    sBuilder.ToString()