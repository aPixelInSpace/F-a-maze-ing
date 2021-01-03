// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.TriGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Array2D.Tri
open Mazes.Render.SVG.Base

let private appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

let private calculatePoints (calculateWidth, calculateHeight, isNumberOfColumnsEven, isUpright, triWidth, triHalfWidth, triHeight, triHalfHeight, marginWidth, marginHeight) coordinate =
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

let private appendWallsType calculatePoints (grid : TriGrid) coordinate (sBuilder : StringBuilder) =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    let appendWall = appendWall sBuilder

    let cell = grid.Cell coordinate

    for position in TriPositionHandler.Instance.Values coordinate do
        match position with
        | Left -> appendWall $"M {round leftX} {round leftY} L {round middleX} {round middleY}" (cell.WallTypeAtPosition position) |> ignore
        | Right -> appendWall $"M {round rightX} {round rightY} L {round middleX} {round middleY}" (cell.WallTypeAtPosition position) |> ignore
        | Top | Bottom -> appendWall $"M {round leftX} {round leftY} L {round rightX} {round rightY}" (cell.WallTypeAtPosition position) |> ignore

let private wholeCellLines calculatePoints coordinate =
    let ((leftX, leftY), (middleX, middleY), (rightX, rightY)) = calculatePoints coordinate

    $"M {round leftX} {round leftY} " +
    $"L {round middleX} {round middleY} " +
    $"L {round rightX} {round rightY} "

let render (grid : TriGrid) (path : Coordinate seq) (map : Map) =

    let sBuilder = StringBuilder()

    let marginWidth = 20.0
    let marginHeight = 20.0

    let triWidth = 15.0
    let triHalfWidth = triWidth / 2.0
    let triHeight = (triWidth * Math.Sqrt(3.0)) / 2.0
    let triHalfHeight = triHeight / 2.0

    let isNumberOfColumnsEven = grid.NumberOfColumns % 2 = 0
    let calculateWidth numberOfColumns =
        if isNumberOfColumnsEven then
            ((numberOfColumns - 1.0) / 2.0) * triWidth + triHalfWidth + marginWidth
        else
            ((numberOfColumns + 1.0) / 2.0) * triWidth + marginWidth

    let calculateHeight numberOfRows = (numberOfRows * triHeight) + marginHeight

    let isUpright = TriPositionHandler.IsUpright

    let calculatePoints = calculatePoints (calculateWidth, calculateHeight, isNumberOfColumnsEven, isUpright, triWidth, triHalfWidth, triHeight, triHalfHeight, (float)marginWidth, (float)marginHeight)

    let width = calculateWidth ((float)grid.NumberOfColumns) + marginWidth
    let height = calculateHeight ((float)grid.NumberOfRows) + marginHeight

    sBuilder
    |> appendHeader ((round width).ToString()) ((round height).ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map (wholeCellLines calculatePoints)
    |> appendPathWithAnimation path (wholeCellLines calculatePoints)
    //|> appendLeaves map.Leaves (wholeCellLines calculatePoints grid)
    |> appendWalls grid.ToInterface.CoordinatesPartOfMaze (appendWallsType calculatePoints grid)
    |> appendFooter
    |> ignore

    sBuilder.ToString()