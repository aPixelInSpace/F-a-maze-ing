// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.BrickGrid

open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Brick
open Mazes.Render.SVG.Base

let private cellWidth = 48
let private cellHeight = 30
let private bridgeHalfWidth = 6.0
let private bridgeDistanceFromCenter = 12.0
let private marginWidth = 20
let private marginHeight = 20

let private calculatePoints (calculateHeight, calculateWidth) (coordinate : NCoordinate) =
    let coordinate2D = coordinate.ToCoordinate2D

    let widthToAdd =
        if not (BrickPositionHandler.IsEven coordinate2D) then
            cellWidth / 2
        else
            0

    let (baseX, baseY) = ((calculateWidth coordinate2D.CIndex) + widthToAdd, calculateHeight coordinate2D.RIndex)

    let (leftTopX, leftTopY) = (baseX, baseY)
    let (middleTopX, middleTopY) = (baseX + (cellWidth / 2), baseY)
    let (rightTopX, rightTopY) = (baseX + cellWidth, baseY)
    let (leftBottomX, leftBottomY) = (baseX , baseY + cellHeight)
    let (middleBottomX, middleBottomY) = (baseX + (cellWidth / 2), baseY + cellHeight)
    let (rightBottomX, rightBottomY) = (baseX + cellWidth, baseY + cellHeight)

    ((leftTopX, leftTopY), (middleTopX, middleTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (middleBottomX, middleBottomY), (rightBottomX, rightBottomY))

let private center calculatePoints coordinate =
    let ((leftTopX, leftTopY),_,_,_,_,_) = calculatePoints coordinate
    ((float)leftTopX, (float)leftTopY) |> translatePoint ((float)(cellWidth / 2), (float)(cellHeight / 2))

let private appendWallsType calculatePoints (grid : IAdjacentStructure<GridArray2D<BrickPosition>, BrickPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (middleTopX, middleTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (middleBottomX, middleBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    for position in BrickPositionHandler.Instance.Values coordinate2D do
        let wallType = (cell.ConnectionTypeAtPosition position)
        let lines =
            match position with
            | Left -> $"M {leftBottomX} {leftBottomY} L {leftTopX} {leftTopY}"
            | TopLeft -> $"M {leftTopX} {leftTopY} L {middleTopX} {middleTopY}"
            | TopRight -> $"M {middleTopX} {middleTopY} L {rightTopX} {rightTopY}"
            | Right -> $"M {rightTopX} {rightTopY} L {rightBottomX} {rightBottomY}"
            | BottomLeft -> $"M {leftBottomX} {leftBottomY} L {middleBottomX} {middleBottomY}"
            | BottomRight -> $"M {middleBottomX} {middleBottomY} L {rightBottomX} {rightBottomY}"

        appendWall sBuilder lines wallType coordinate |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftTopX, leftTopY), _, (rightTopX, rightTopY), (leftBottomX, leftBottomY), _, (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    $"M {leftBottomX} {leftBottomY} " +
    $"L {rightBottomX} {rightBottomY} " +
    $"L {rightTopX} {rightTopY} " +
    $"L {leftTopX} {leftTopY} "

let render (grid : NDimensionalStructure<GridArray2D<BrickPosition>, BrickPosition>) path map entrance exit =
    let sBuilder = StringBuilder()

    let calculateHeight numberOfRows =
        marginHeight + (numberOfRows * cellHeight)

    let calculateWidth numberOfColumns =
        marginWidth + (numberOfColumns * cellWidth)

    let (dimension, slice2D) = grid.FirstSlice2D

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let width = calculateWidth slice2D.ToSpecializedStructure.NumberOfColumns + marginWidth + (cellWidth / 2)
    let height = calculateHeight slice2D.ToSpecializedStructure.NumberOfRows + marginHeight

    let calculatePoints = calculatePoints (calculateHeight, calculateWidth)

    let center = (center calculatePoints)
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

    let appendMazeDistanceBridgeColoration =
        appendMazeDistanceBridgeColoration nonAdjacentNeighbors map wholeBridgeLines

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    let blankColor _ = Some "white"
    let colorPicker coordinate = None

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle "purple"
    |> appendBackground "transparent"
    
    //|> appendMazeColoration grid.ToInterface.CoordinatesPartOfMaze wholeCellLines
    |> appendMazeDistanceColoration map wholeCellLines

    //|> appendPath path wholeCellLines
    //|> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves wholeCellLines
    //|> appendPathAndBridgesWithAnimation

    |> appendSimpleWalls

    //|> appendPathAndBridgesWithAnimation
    //|> appendWallsWithInset

    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines blankColor
    |> appendMazeDistanceBridgeColoration
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell center entrance "start"
    |> textCell center exit "exit"

    |> appendFooter
    |> ignore
 
    sBuilder.ToString()