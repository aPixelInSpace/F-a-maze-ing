// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Ortho
open Mazes.Render.SVG.Base

//let private cellWidth = 30
//let private cellHeight = 30
//let private bridgeHalfWidth = 5.0
//let private bridgeDistanceFromCenter = 12.0
//let private marginWidth = 20
//let private marginHeight = 20
//
//let private perfectRadius = pythagorasHypotenuse ((float)cellWidth) ((float)cellHeight) / 2.0
//let private radiusX = round perfectRadius
//let private radiusY = round perfectRadius

type Line =
    | Straight
    | Circle
    | Curve of (int * int)

type Parameters =
    {
        Width : int
        Height : int
        BridgeWidth : float
        BridgeDistanceFromCenter : float
        MarginWidth : int
        MarginHeight : int
        Line : Line
    }
    member this.Radius =
        match this.Line with
        | Straight -> ("0", "0")
        | Circle ->
            let perfectRadius = round (pythagorasHypotenuse ((float)this.Width) ((float)this.Height) / 2.0)
            (perfectRadius, perfectRadius)
        | Curve (rx, ry) -> (rx.ToString(), ry.ToString())

    static member CreateDefaultSquare =
        {
            Width = 30
            Height = 30
            BridgeWidth = 10.0
            BridgeDistanceFromCenter = 12.0
            MarginWidth = 20
            MarginHeight = 20
            Line = Straight
        }

    static member CreateDefaultCircle =
        {
            Width = 30
            Height = 30
            BridgeWidth = 10.0
            BridgeDistanceFromCenter = 12.0
            MarginWidth = 20
            MarginHeight = 20
            Line = Circle
        }

let private calculatePoints (parameters : Parameters) (calculateHeight, calculateWidth) (coordinate : NCoordinate) =
    let (baseX, baseY) = (calculateWidth coordinate.ToCoordinate2D.CIndex, calculateHeight coordinate.ToCoordinate2D.RIndex)
    let (leftTopX, leftTopY) = (baseX, baseY)
    let (rightTopX, rightTopY) = (baseX + parameters.Width, baseY )
    let (leftBottomX, leftBottomY) = (baseX , baseY + parameters.Height)
    let (rightBottomX, rightBottomY) = (baseX + parameters.Width, baseY + parameters.Height)

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY))

let private center (parameters : Parameters) calculatePoints coordinate =
    let ((leftTopX, leftTopY),_,_,_) = calculatePoints coordinate
    ((float)leftTopX, (float)leftTopY) |> translatePoint ((float)(parameters.Width / 2), (float)(parameters.Height / 2))

let private appendWallsType (parameters : Parameters) calculatePoints (grid : IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    let orientation = if (coordinate2D.RIndex + coordinate2D.CIndex) % 2 = 0 then "0" else "1"
    let (rx, ry) = parameters.Radius
    
    for position in OrthoPositionHandler.Instance.Values coordinate2D do
        let wallType = (cell.ConnectionTypeAtPosition position)
        let lines =
            match parameters.Line with
            | Straight ->
                match position with
                | Left -> $"M {leftBottomX} {leftBottomY} L {leftTopX} {leftTopY}"
                | Top -> $"M {leftTopX} {leftTopY} L {rightTopX} {rightTopY}"
                | Right -> $"M {rightBottomX} {rightBottomY} L {rightTopX} {rightTopY}"
                | Bottom -> $"M {leftBottomX} {leftBottomY} L {rightBottomX} {rightBottomY}"
            | Circle | Curve _ ->
                match position with
                | Left -> $"M {leftTopX} {leftTopY} A {rx} {ry}, 0, 0, {orientation}, {leftBottomX} {leftBottomY}"
                | Top -> $"M {rightTopX} {rightTopY} A {rx} {ry}, 0, 0, {orientation}, {leftTopX} {leftTopY}"
                | Right -> $"M {rightBottomX} {rightBottomY} A {rx} {ry}, 0, 0, {orientation}, {rightTopX} {rightTopY}"
                | Bottom -> $"M {leftBottomX} {leftBottomY} A {rx} {ry}, 0, 0, {orientation}, {rightBottomX} {rightBottomY}"

        appendWall sBuilder lines wallType coordinate |> ignore

let private wholeCellLines (parameters : Parameters) calculatePoints (coordinate : NCoordinate) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let orientation = if (coordinate2D.RIndex + coordinate2D.CIndex) % 2 = 0 then "0" else "1"
    let (rx, ry) = parameters.Radius

    match parameters.Line with
    | Straight ->
        $"M {leftBottomX} {leftBottomY} " +
        $"L {rightBottomX} {rightBottomY} " +
        $"L {rightTopX} {rightTopY} " +
        $"L {leftTopX} {leftTopY} "
    | Circle | Curve _ ->
        $"M {leftTopX} {leftTopY} A {rx} {ry}, 0, 0, {orientation}, {leftBottomX} {leftBottomY} " +
        $"A {rx} {ry}, 0, 0, {orientation}, {rightBottomX} {rightBottomY} " +
        $"A {rx} {ry}, 0, 0, {orientation}, {rightTopX} {rightTopY} " +
        $"A {rx} {ry}, 0, 0, {orientation}, {leftTopX} {leftTopY} "

let render (parameters : Parameters) (grid : NDimensionalStructure<GridArray2D<OrthoPosition>, OrthoPosition>) path map entrance exit =
    let sBuilder = StringBuilder()

    let calculateHeight numberOfRows =
        parameters.MarginHeight + (numberOfRows * parameters.Height)

    let calculateWidth numberOfColumns =
        parameters.MarginWidth + (numberOfColumns * parameters.Width)

    let (dimension, slice2D) = grid.FirstSlice2D

    let width = calculateWidth slice2D.ToSpecializedStructure.NumberOfColumns + parameters.MarginWidth
    let height = calculateHeight slice2D.ToSpecializedStructure.NumberOfRows + parameters.MarginHeight

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePoints = calculatePoints parameters (calculateHeight, calculateWidth)

    let calculatePointsBridge = calculatePointsBridge (center parameters calculatePoints) (parameters.BridgeWidth / 2.0) parameters.BridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors
    
    let appendWallsType = appendWallsType parameters calculatePoints slice2D
    let wholeCellLines = wholeCellLines parameters calculatePoints
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge
    
    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendMazeDistanceBridgeColoration =
        appendMazeDistanceBridgeColoration  nonAdjacentNeighbors map wholeBridgeLines

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

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
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines
    |> appendMazeDistanceBridgeColoration
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell (center parameters calculatePoints) entrance "start"
    |> textCell (center parameters calculatePoints) exit "exit"

    |> appendFooter
    |> ignore
 
    sBuilder.ToString()