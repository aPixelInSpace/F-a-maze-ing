// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.TriGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Tri
open Mazes.Render.SVG.Base

let private calculatePoints (calculateWidth, calculateHeight, isUpright, triWidth, triHalfWidth, triHeight) coordinate =
    let baseLengthAtRight = calculateWidth ((float)(coordinate.CIndex + 1))
    let baseLengthAtBottom = calculateHeight ((float)(coordinate.RIndex + 1))
    let baseLengthAtLeft = baseLengthAtRight - triWidth
    let baseLengthAtTop = baseLengthAtBottom - triHeight

    let isUpright = isUpright coordinate 

    let leftX = baseLengthAtLeft
    let middleX = baseLengthAtLeft + triHalfWidth
    let rightX = baseLengthAtRight

    let (leftY, middleY, rightY) =
        match isUpright with
        | true -> (baseLengthAtBottom, baseLengthAtTop, baseLengthAtBottom)
        | false -> (baseLengthAtTop, baseLengthAtBottom, baseLengthAtTop)

    ((leftX, leftY), (middleX, middleY), (rightX, rightY))

let private appendWallsType calculatePoints (grid : TriGrid) appendWall coordinate (sBuilder : StringBuilder) =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    let cell = grid.Cell coordinate

    for position in TriPositionHandler.Instance.Values coordinate do
        let lines =
            match position with
            | Left -> $"M {round leftX} {round leftY} L {round middleX} {round middleY}"
            | Right -> $"M {round rightX} {round rightY} L {round middleX} {round middleY}"
            | Top | Bottom -> $"M {round leftX} {round leftY} L {round rightX} {round rightY}"

        appendWall sBuilder lines (cell.WallTypeAtPosition position) |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    $"M {round leftX} {round leftY} " +
    $"L {round middleX} {round middleY} " +
    $"L {round rightX} {round rightY} "

let render (grid : TriGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    let triWidth = 30.0
    let triHalfWidth = triWidth / 2.0
    let triHeight = (triWidth * Math.Sqrt(3.0)) / 2.0

    let isNumberOfColumnsEven = grid.NumberOfColumns % 2 = 0
    let calculateWidth numberOfColumns =
        if isNumberOfColumnsEven then
            marginWidth + ((numberOfColumns - 1.0) / 2.0) * triWidth + triHalfWidth
        else
            marginWidth + ((numberOfColumns + 1.0) / 2.0) * triWidth

    let calculateHeight numberOfRows = (numberOfRows * triHeight) + marginHeight

    let isUpright = TriPositionHandler.IsUpright

    let calculatePoints = calculatePoints (calculateWidth, calculateHeight, isUpright, triWidth, triHalfWidth, triHeight)

    let appendWallsType = appendWallsType calculatePoints grid
    let wholeCellLines = wholeCellLines calculatePoints

    let appendSimpleWalls sBuilder =
        appendSimpleWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder

    let width = calculateWidth ((float)grid.NumberOfColumns) + marginWidth
    let height = calculateHeight ((float)grid.NumberOfRows) + marginHeight

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeDistanceColoration map wholeCellLines
    |> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves (wholeCellLines calculatePoints grid)
    //|> appendSimpleWalls
    |> appendWallsWithInset
    |> appendFooter
    |> ignore

    sBuilder.ToString()