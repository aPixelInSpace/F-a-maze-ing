// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Render.SVG.Base

let private cellWidth = 30
let private cellHeight = 30
let private marginWidth = 20
let private marginHeight = 20
let private inset = 0

let private calculatePoints (calculateHeight, calculateWidth) inset coordinate =
    let (baseX, baseY) = (calculateWidth coordinate.CIndex, calculateHeight coordinate.RIndex)
    let (leftTopX, leftTopY) = (baseX + inset, baseY + inset)
    let (rightTopX, rightTopY) = (baseX + cellWidth - inset, baseY + inset)
    let (leftBottomX, leftBottomY) = (baseX + inset, baseY + cellHeight - inset)
    let (rightBottomX, rightBottomY) = (baseX + cellWidth - inset, baseY + cellHeight - inset)

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY))

let private appendWallsType calculatePoints (grid : OrthoGrid) coordinate (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints inset coordinate

    let cell = grid.Cell coordinate
    for position in OrthoPositionHandler.Instance.Values coordinate do
        let wallType = (cell.WallTypeAtPosition position)
        let lines =
            match position with
            | Left -> $"M {leftBottomX} {leftBottomY} L {leftTopX} {leftTopY}"
            | Top -> $"M {leftTopX} {leftTopY} L {rightTopX} {rightTopY}"
            | Right -> $"M {rightBottomX} {rightBottomY} L {rightTopX} {rightTopY}"
            | Bottom -> $"M {leftBottomX} {leftBottomY} L {rightBottomX} {rightBottomY}"

        appendWall sBuilder lines wallType |> ignore

let private appendWallsTypeInset calculatePoints (grid : OrthoGrid) coordinate (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints inset coordinate

    let appendWall = appendWall sBuilder
    let appendNormalWallInset = appendNormalWallInset sBuilder

    let cell = grid.Cell coordinate
    for position in OrthoPositionHandler.Instance.Values coordinate do
        let wallType = (cell.WallTypeAtPosition position)
        
        match position with
        | Left ->
            appendWall $"M {leftBottomX} {leftBottomY} L {leftTopX} {leftTopY}" wallType |> ignore
            if wallType = Normal then appendNormalWallInset $"M {leftBottomX} {leftBottomY + 1} L {leftTopX} {leftTopY  - 1}" |> ignore
        | Top ->
            appendWall $"M {leftTopX} {leftTopY} L {rightTopX} {rightTopY}" wallType |> ignore
            if wallType = Normal then appendNormalWallInset $"M {leftTopX - 1} {leftTopY} L {rightTopX + 1} {rightTopY}" |> ignore
        | Right ->
            appendWall $"M {rightBottomX} {rightBottomY} L {rightTopX} {rightTopY}" wallType |> ignore
            if wallType = Normal then  appendNormalWallInset $"M {rightBottomX} {rightBottomY + 1} L {rightTopX} {rightTopY - 1}" |> ignore
        | Bottom ->
            appendWall $"M {leftBottomX} {leftBottomY} L {rightBottomX} {rightBottomY}" wallType |> ignore
            if wallType = Normal then  appendNormalWallInset $"M {leftBottomX - 1} {leftBottomY} L {rightBottomX + 1} {rightBottomY}" |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints inset coordinate

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
    let wholeCellLines = wholeCellLines calculatePoints
    let appendWallsType = appendWallsType calculatePoints grid

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map wholeCellLines
    //|> appendPath path wholeCellLines
    |> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves wholeCellLines
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType
    |> appendFooter
    |> ignore
 
    sBuilder.ToString()