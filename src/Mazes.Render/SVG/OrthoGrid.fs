// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Ortho
open Mazes.Render
open Mazes.Render.SVG.Base

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
        | Straight -> (0.0, 0.0)
        | Circle ->
            let perfectRadius = (pythagorasHypotenuse ((float)this.Width) ((float)this.Height) / 2.0)
            (perfectRadius, perfectRadius)
        | Curve (rx, ry) -> ((float)rx, (float)ry)

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
    let (leftTopX, leftTopY) = ((float)baseX, (float)baseY)
    let (rightTopX, rightTopY) = ((float)(baseX + parameters.Width), (float)baseY)
    let (leftBottomX, leftBottomY) = ((float)baseX , (float)(baseY + parameters.Height))
    let (rightBottomX, rightBottomY) = ((float)(baseX + parameters.Width), (float)(baseY + parameters.Height))

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY))

let private center (parameters : Parameters) calculatePoints coordinate =
    let ((leftTopX, leftTopY),_,_,_) = calculatePoints coordinate
    (leftTopX, leftTopY) |> translatePoint ((float)(parameters.Width / 2), (float)(parameters.Height / 2))

let private appendWallsType (parameters : Parameters) calculatePoints getLine (grid : IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let cell = grid.Cell coordinate2D

    let orientation = if (coordinate2D.RIndex + coordinate2D.CIndex) % 2 = 0 then "0" else "1"

    let (rx, ry) = parameters.Radius

//    let (rx, ry) =
//        let (rx, ry) = parameters.Radius
//        let rnd = rng.NextDouble()
//        if rnd < 0.5 then
//            (rx, ry)
//        elif rnd < 0.8 then
//            (rx - 20.0, ry - 20.0)
//        else
//            (0.0, 0.0)
    
    for position in OrthoPositionHandler.Instance.Values coordinate2D do
        let wallType = (cell.ConnectionTypeAtPosition position)
        let line =
            match parameters.Line with
            | Straight ->
                match position with
                | Left -> getLine straightLine (leftBottomX, leftBottomY) (leftTopX, leftTopY)
                | Top -> getLine straightLine (leftTopX, leftTopY) (rightTopX, rightTopY)
                | Right -> getLine straightLine (rightBottomX, rightBottomY) (rightTopX, rightTopY)
                | Bottom -> getLine straightLine (leftBottomX, leftBottomY) (rightBottomX, rightBottomY)
            | Circle | Curve _ ->
                let arcLine = arcLine orientation (rx, ry)
                match position with
                | Left -> getLine arcLine (leftTopX, leftTopY) (leftBottomX, leftBottomY)
                | Top -> getLine arcLine (rightTopX, rightTopY) (leftTopX, leftTopY)
                | Right -> getLine arcLine (rightBottomX, rightBottomY) (rightTopX, rightTopY)
                | Bottom -> getLine arcLine (leftBottomX, leftBottomY) (rightBottomX, rightBottomY)

        match line with
        | Some line ->
            appendWall sBuilder line wallType coordinate |> ignore
        | None -> ()

let private wholeCellLines (parameters : Parameters) calculatePoints (coordinate : NCoordinate) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)) =
        calculatePoints coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let orientation = if (coordinate2D.RIndex + coordinate2D.CIndex) % 2 = 0 then "0" else "1"
    let (rx, ry) = parameters.Radius

    match parameters.Line with
    | Straight ->
        $"M {round leftBottomX} {round leftBottomY} " +
        $"L {round rightBottomX} {round rightBottomY} " +
        $"L {round rightTopX} {round rightTopY} " +
        $"L {round leftTopX} {round leftTopY} "
    | Circle | Curve _ ->
        $"M {round leftTopX} {round leftTopY} A {round rx} {round ry}, 0, 0, {orientation}, {round leftBottomX} {round leftBottomY} " +
        $"A {round rx} {round ry}, 0, 0, {orientation}, {round rightBottomX} {round rightBottomY} " +
        $"A {round rx} {round ry}, 0, 0, {orientation}, {round rightTopX} {round rightTopY} " +
        $"A {round rx} {round ry}, 0, 0, {orientation}, {round leftTopX} {round leftTopY} "

let render (parameters : Parameters) (grid : NDimensionalStructure<GridArray2D<OrthoPosition>, OrthoPosition>) path map entrance exit =
    let sBuilder = StringBuilder()
    let lineTracker = LineTracker<LinePointsFloat>.createEmpty

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
    
    let getLine = getLine lineTracker.ContainsLine lineTracker.Add

    let calculatePointsBridge = calculatePointsBridge (center parameters calculatePoints) (parameters.BridgeWidth / 2.0) parameters.BridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors
    
    let appendWallsTypeNoDoubleLine = appendWallsType parameters calculatePoints (getLine true) slice2D
    let appendWallsType = appendWallsType parameters calculatePoints (getLine false) slice2D    

    let wholeCellLines = wholeCellLines parameters calculatePoints
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge
    
    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsTypeNoDoubleLine sBuilder
    
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