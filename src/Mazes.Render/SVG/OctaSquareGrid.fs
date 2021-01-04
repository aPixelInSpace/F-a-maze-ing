// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OctaSquareGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.OctaSquare
open Mazes.Render.SVG.Base

let private appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

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

let private appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) (grid : OctaSquareGrid) coordinate (sBuilder : StringBuilder) =

    let cell = grid.Cell coordinate

    let appendWall s position =
        appendWall sBuilder s (cell.WallTypeAtPosition position) |> ignore

    if (isOctagon coordinate) then
        let ((leftTopX, leftTopY),(topLeftX, topLeftY),(topRightX, topRightY),(rightTopX, rightTopY),(leftBottomX, leftBottomY),(bottomLeftX, bottomLeftY),(bottomRightX, bottomRightY),(rightBottomX, rightBottomY)) =
            calculatePointsOctagon (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate do
            match position with
            | Left -> appendWall $"M {round leftBottomX} {round leftBottomY} L {round leftTopX} {round leftTopY}" position
            | TopLeft -> appendWall $"M {round leftTopX} {round leftTopY} L {round topLeftX} {round topLeftY}" position
            | Top -> appendWall $"M {round topLeftX} {round topLeftY} L {round topRightX} {round topRightY}" position
            | TopRight -> appendWall $"M {round topRightX} {round topRightY} L {round rightTopX} {round rightTopY}" position
            | Right -> appendWall $"M {round rightTopX} {round rightTopY} L {round rightBottomX} {round rightBottomY}" position
            | BottomLeft -> appendWall $"M {round leftBottomX} {round leftBottomY} L {round bottomLeftX} {round bottomLeftY}" position
            | Bottom -> appendWall $"M {round bottomLeftX} {round bottomLeftY} L {round bottomRightX} {round bottomRightY}" position
            | BottomRight -> appendWall $"M {round bottomRightX} {round bottomRightY} L {round rightBottomX} {round rightBottomY}" position
    else
        let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
            calculatePointsSquare (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate do
            match position with
            | Left -> appendWall $"M {round leftBottomX} {round leftBottomY} L {round leftTopX} {round leftTopY}" position
            | Top -> appendWall $"M {round leftTopX} {round leftTopY} L {round rightTopX} {round rightTopY}" position
            | Right -> appendWall $"M {round rightBottomX} {round rightBottomY} L {round rightTopX} {round rightTopY}" position
            | Bottom -> appendWall $"M {round leftBottomX} {round leftBottomY} L {round rightBottomX} {round rightBottomY}" position
            | _ -> failwith $"Could not match position {coordinate} at {position}"

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

    let octaSquareSideSize = 15.0 // also hypotenuse of the triangle 
    let otherSideSize = Math.Sqrt((octaSquareSideSize ** 2.0) / 2.0) // by good old Pythagoras's theorem

    let calculateLength numberOf =
        marginWidth + numberOf * (otherSideSize + octaSquareSideSize) + otherSideSize

    let isOctagon = OctaSquarePositionHandler.IsOctagon

    let width = calculateLength ((float)grid.NumberOfColumns) + marginWidth
    let height = calculateLength ((float)grid.NumberOfRows) + marginHeight

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map (wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize))
    |> appendPathWithAnimation path (wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize))
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze (appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) grid)
    |> appendFooter
    |> ignore

    sBuilder.ToString()