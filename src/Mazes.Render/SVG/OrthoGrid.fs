// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Render.SVG.Base

let private cellWidth = 30
let private cellHeight = 30
let private bridgeHalfWidth = 7.0
let private bridgeDistanceFromCenter = 9.0
let private marginWidth = 20
let private marginHeight = 20

let private calculatePoints (calculateHeight, calculateWidth) coordinate =
    let (baseX, baseY) = (calculateWidth coordinate.CIndex, calculateHeight coordinate.RIndex)
    let (leftTopX, leftTopY) = (baseX, baseY)
    let (rightTopX, rightTopY) = (baseX + cellWidth, baseY )
    let (leftBottomX, leftBottomY) = (baseX , baseY + cellHeight)
    let (rightBottomX, rightBottomY) = (baseX + cellWidth, baseY + cellHeight)

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY))

let center calculatePoints coordinate =
    let ((leftTopX, leftTopY),_,_,_) = calculatePoints coordinate
    ((float)leftTopX, (float)leftTopY) |> translatePoint ((float)(cellWidth / 2), (float)(cellHeight / 2))

let private appendWallsType calculatePoints (grid : OrthoGrid) appendWall coordinate (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let cell = grid.Cell coordinate
    for position in OrthoPositionHandler.Instance.Values coordinate do
        let wallType = (cell.WallTypeAtPosition position)
        let lines =
            match position with
            | Left -> $"M {leftBottomX} {leftBottomY} L {leftTopX} {leftTopY}"
            | Top -> $"M {leftTopX} {leftTopY} L {rightTopX} {rightTopY}"
            | Right -> $"M {rightBottomX} {rightBottomY} L {rightTopX} {rightTopY}"
            | Bottom -> $"M {leftBottomX} {leftBottomY} L {rightBottomX} {rightBottomY}"

        appendWall sBuilder lines wallType coordinate |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    $"M {leftBottomX} {leftBottomY} " +
    $"L {rightBottomX} {rightBottomY} " +
    $"L {rightTopX} {rightTopY} " +
    $"L {leftTopX} {leftTopY} "

let render (grid : OrthoGrid) (path : Coordinate seq) (map : Map) =
    let sBuilder = StringBuilder()

    let calculateHeight numberOfRows =
        marginHeight + (numberOfRows * cellHeight)

    let calculateWidth numberOfColumns =
        marginWidth + (numberOfColumns * cellWidth)

    let width = calculateWidth grid.Canvas.NumberOfColumns + marginWidth
    let height = calculateHeight grid.Canvas.NumberOfRows + marginHeight

    let calculatePoints = calculatePoints (calculateHeight, calculateWidth)

    let calculatePointsBridge = calculatePointsBridge (center calculatePoints) bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge grid.NonAdjacentNeighbors.All
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge grid.NonAdjacentNeighbors.All
    
    let appendWallsType = appendWallsType calculatePoints grid
    let wholeCellLines = wholeCellLines calculatePoints
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge
    
    let appendSimpleWalls sBuilder =
        appendSimpleWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder

    let appendMazeDistanceBridgeColoration =
        appendMazeDistanceBridgeColoration grid.NonAdjacentNeighbors.All wholeBridgeLines (map.ShortestPathGraph.NodeDistanceFromRoot) (map.FarthestFromRoot.Distance)

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacentNeighbors.ExistNeighbor wholeBridgeLines

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle
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
    |> appendMazeBridgeColoration grid.NonAdjacentNeighbors.All wholeBridgeLines
    |> appendMazeDistanceBridgeColoration
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> appendFooter
    |> ignore
 
    sBuilder.ToString()