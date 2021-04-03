// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OctaSquareGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.OctaSquare
open Mazes.Render.SVG.Base

let private calculatePointsOctagon (calculateLength, (octaSquareSideSize : float), (otherSideSize : float)) (coordinate : NCoordinate) =
    let baseLengthAtLeft = calculateLength ((float)(coordinate.ToCoordinate2D.CIndex))
    let baseLengthAtTop = calculateLength ((float)(coordinate.ToCoordinate2D.RIndex))

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

let private calculatePointsSquare (calculateLength, (octaSquareSideSize : float), (otherSideSize : float)) (coordinate : NCoordinate) =
    let baseLengthAtLeft = calculateLength ((float)(coordinate.ToCoordinate2D.CIndex))
    let baseLengthAtTop = calculateLength ((float)(coordinate.ToCoordinate2D.RIndex))

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

let center calculatePointsOctagon calculatePointsSquare isOctagon (coordinate : NCoordinate) =
    let (pointA, pointB) =
        if isOctagon coordinate.ToCoordinate2D then
            let (_, topLeft, _, _, _, _, bottomRight, _) = calculatePointsOctagon coordinate
            (topLeft, bottomRight)
        else
            let (leftTop, _, _, rightBottom) = calculatePointsSquare coordinate
            (leftTop, rightBottom)

    middlePoint pointA pointB

let private appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) (grid : IAdjacentStructure<GridArray2D<OctaSquarePosition>, OctaSquarePosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    if (isOctagon coordinate.ToCoordinate2D) then
        let ((leftTopX, leftTopY),(topLeftX, topLeftY),(topRightX, topRightY),(rightTopX, rightTopY),(leftBottomX, leftBottomY),(bottomLeftX, bottomLeftY),(bottomRightX, bottomRightY),(rightBottomX, rightBottomY)) =
            calculatePointsOctagon (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate2D do
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

            appendWall sBuilder lines (cell.ConnectionTypeAtPosition position) coordinate |> ignore
    else
        let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
            calculatePointsSquare (calculateLength, octaSquareSideSize, otherSideSize) coordinate

        for position in OctaSquarePositionHandler.Instance.Values coordinate2D do
            let lines =
                match position with
                | Left -> $"M {round leftBottomX} {round leftBottomY} L {round leftTopX} {round leftTopY}"
                | Top -> $"M {round leftTopX} {round leftTopY} L {round rightTopX} {round rightTopY}"
                | Right -> $"M {round rightBottomX} {round rightBottomY} L {round rightTopX} {round rightTopY}"
                | Bottom -> $"M {round leftBottomX} {round leftBottomY} L {round rightBottomX} {round rightBottomY}"
                | _ -> failwith $"Could not match position {coordinate} at {position}"

            appendWall sBuilder lines (cell.ConnectionTypeAtPosition position) coordinate |> ignore

let private wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) (coordinate : NCoordinate) =
    
    if (isOctagon coordinate.ToCoordinate2D) then
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

let render (grid : NDimensionalStructure<GridArray2D<OctaSquarePosition>, OctaSquarePosition>) path map entrance exit =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    let octaSquareSideSize = 30.0 // also hypotenuse of the triangle
    let otherSideSize = Math.Sqrt((octaSquareSideSize ** 2.0) / 2.0) // by good old Pythagoras's theorem

    let bridgeHalfWidth = 6.0
    let bridgeDistanceFromCenter = 12.0

    let calculateLength numberOf =
        marginWidth + numberOf * (otherSideSize + octaSquareSideSize) + otherSideSize

    let (dimension, slice2D) = grid.FirstSlice2D

    let isOctagon = OctaSquarePositionHandler.IsOctagon

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePointsOctagon = calculatePointsOctagon (calculateLength, octaSquareSideSize, otherSideSize)
    let calculatePointsSquare = calculatePointsSquare (calculateLength, octaSquareSideSize, otherSideSize)

    let center = (center calculatePointsOctagon calculatePointsSquare isOctagon)
    
    let calculatePointsBridge = calculatePointsBridge center bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors

    let appendWallsType = appendWallsType (calculateLength, isOctagon, octaSquareSideSize, otherSideSize) slice2D
    let wholeCellLines = wholeCellLines (calculateLength, isOctagon, octaSquareSideSize, otherSideSize)
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge

    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    let width = calculateLength ((float)slice2D.ToSpecializedStructure.NumberOfColumns) + marginWidth + 20.0 // because of the size of the border
    let height = calculateLength ((float)slice2D.ToSpecializedStructure.NumberOfRows) + marginHeight + 20.0 // because of the size of the border

    let blankColor _ = Some "white"
    let colorPicker coordinate = None

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle GlobalOptions.Parameters.Default
    |> appendBackground "transparent"

    |> appendMazeDistanceColoration map wholeCellLines

    //|> appendPathWithAnimation path wholeCellLines

    |> appendSimpleWalls
    //|> appendWallsWithInset

    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines blankColor
    |> appendMazeDistanceBridgeColoration nonAdjacentNeighbors map wholeBridgeLines
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell center entrance "start"
    |> textCell center exit "exit"

    |> appendFooter
    |> ignore

    sBuilder.ToString()