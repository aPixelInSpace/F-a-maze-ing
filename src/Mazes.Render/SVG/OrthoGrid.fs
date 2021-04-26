// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.OrthoGrid

open System
open System.Text
open FSharpPlus.Memoization
open Mazes.Core
open Mazes.Core.Utils
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
    | Random of (System.Random * float)

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
    
    member this.PerfectRadius =
        let perfectRadius = (pythagorasHypotenuse ((float)this.Width) ((float)this.Height) / 2.0)
        (perfectRadius, perfectRadius)
    
    member this.Radius() =
        match this.Line with
        | Straight -> (0.0, 0.0)
        | Circle -> this.PerfectRadius
        | Curve (rx, ry) -> ((float)rx, (float)ry)
        | Random (rng, mult) ->
            let min = fst this.PerfectRadius
            let max = min * mult
            let nextFloat =
                rng.NextDouble() * (max - min) + min            
            
            (nextFloat, nextFloat)

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
    let coordinate2D = coordinate.Coordinate2D
    let (baseX, baseY) = (calculateWidth coordinate2D.CIndex, calculateHeight coordinate2D.RIndex)
    let (leftTopX, leftTopY) = ((float)baseX, (float)baseY)
    let (rightTopX, rightTopY) = ((float)(baseX + parameters.Width), (float)baseY)
    let (leftBottomX, leftBottomY) = ((float)baseX , (float)(baseY + parameters.Height))
    let (rightBottomX, rightBottomY) = ((float)(baseX + parameters.Width), (float)(baseY + parameters.Height))
    
    let orientation = if (coordinate2D.RIndex + coordinate2D.CIndex) % 2 = 0 then "0" else "1"

    ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY), orientation)

let private center (parameters : Parameters) calculatePoints coordinate =
    let ((leftTopX, leftTopY),_,_,_,_) = calculatePoints coordinate
    (leftTopX, leftTopY) |> translatePoint ((float)(parameters.Width / 2), (float)(parameters.Height / 2))

let private appendWallsType (parameters : Parameters) calculatePoints getRadius getLine (grid : IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>) appendWall (coordinate : NCoordinate) (sBuilder : StringBuilder) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY), orientation) =
        calculatePoints coordinate

    let coordinate2D = coordinate.Coordinate2D
    let cell = grid.Cell coordinate2D
    
    let inverseOrientationL =
        let coordinate2D = coordinate.Coordinate2D
        match coordinate2D.RIndex, coordinate2D.CIndex with
        | _, 0 -> orientation
        | _ -> if orientation = "0" then "1" else "0"
        
    let inverseOrientation = if orientation = "0" then "1" else "0"

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
            | Circle | Curve _ | Random _ ->
                let arcLine = arcLine
                match position with
                | Left ->
                    let arcLine = arcLine inverseOrientationL (getRadius (leftTopX, leftTopY) (leftBottomX, leftBottomY))
                    getLine arcLine (leftTopX, leftTopY) (leftBottomX, leftBottomY)
                | Top ->
                    let arcLine = arcLine inverseOrientation (getRadius (leftTopX, leftTopY) (rightTopX, rightTopY))
                    getLine arcLine (leftTopX, leftTopY) (rightTopX, rightTopY)
                | Right ->
                    let arcLine = arcLine orientation (getRadius (rightTopX, rightTopY) (rightBottomX, rightBottomY))
                    getLine arcLine (rightTopX, rightTopY) (rightBottomX, rightBottomY)
                | Bottom ->
                    let arcLine = arcLine orientation (getRadius (leftBottomX, leftBottomY) (rightBottomX, rightBottomY))
                    getLine arcLine (leftBottomX, leftBottomY) (rightBottomX, rightBottomY)

        match line with
        | Some line ->
            appendWall sBuilder line wallType coordinate |> ignore
        | None -> ()

let private wholeCellLines (parameters : Parameters) calculatePoints getRadius (coordinate : NCoordinate) =
    let ((leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY), orientation) =
        calculatePoints coordinate

    match parameters.Line with
    | Straight ->
        $"M {round leftBottomX} {round leftBottomY} " +
        $"L {round rightBottomX} {round rightBottomY} " +
        $"L {round rightTopX} {round rightTopY} " +
        $"L {round leftTopX} {round leftTopY} "
    | Circle | Curve _ | Random _ ->
        let (ltLbRx, ltLbRy) = getRadius (leftTopX, leftTopY) (leftBottomX, leftBottomY)
        let (lbRbRx, lbRbRy) = getRadius (leftBottomX, leftBottomY) (rightBottomX, rightBottomY)
        let (rtRbRx, rtRbRy) = getRadius (rightTopX, rightTopY) (rightBottomX, rightBottomY)
        let (ltRtRx, ltRtRy) = getRadius (leftTopX, leftTopY) (rightTopX, rightTopY)

        let inverseOrientationL =
            let coordinate2D = coordinate.Coordinate2D
            match coordinate2D.RIndex, coordinate2D.CIndex with
            | _, 0 -> orientation
            | _ -> if orientation = "0" then "1" else "0"
            
        let inverseOrientation = if orientation = "0" then "1" else "0"

        $"M {round leftTopX} {round leftTopY} A {round ltLbRx} {round ltLbRy}, 0, 0, {inverseOrientationL}, {round leftBottomX} {round leftBottomY} " +
        $"A {round lbRbRx} {round lbRbRy}, 0, 0, {orientation}, {round rightBottomX} {round rightBottomY} " +
        $"A {round rtRbRx} {round rtRbRy}, 0, 0, {inverseOrientation}, {round rightTopX} {round rightTopY} " +
        $"A {round ltRtRx} {round ltRtRy}, 0, 0, {orientation}, {round leftTopX} {round leftTopY} "

let render (globalOptionsParameters : SVG.GlobalOptions.Parameters) (parameters : Parameters) (grid : NDimensionalStructure<GridArray2D<OrthoPosition>, OrthoPosition>) path map entrance exit =
    let sBuilder = StringBuilder()
    let lineTracker = LineTracker<LinePointsFloat>.createEmpty

    let calculateHeight numberOfRows =
        parameters.MarginHeight + (numberOfRows * parameters.Height)

    let calculateWidth numberOfColumns =
        parameters.MarginWidth + (numberOfColumns * parameters.Width)

    let dimension, slice2D = grid.FirstSlice2D

    let width = calculateWidth slice2D.ToSpecializedStructure.NumberOfColumns + parameters.MarginWidth
    let height = calculateHeight slice2D.ToSpecializedStructure.NumberOfRows + parameters.MarginHeight

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePoints = memoizeN (calculatePoints parameters (calculateHeight, calculateWidth))
    let getRadius =
        let getRadius = (fun _ _ -> parameters.Radius())
        memoizeN getRadius
    
    let getLine = getLine lineTracker.ContainsLine lineTracker.Add

    let calculatePointsBridge = calculatePointsBridge (center parameters calculatePoints) (parameters.BridgeWidth / 2.0) parameters.BridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors
    
    let appendWallsTypeNoDoubleLine = appendWallsType parameters calculatePoints getRadius (getLine true) slice2D
    let appendWallsType = appendWallsType parameters calculatePoints getRadius (getLine false) slice2D    

    let wholeCellLines = wholeCellLines parameters calculatePoints getRadius
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge
    
    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsTypeNoDoubleLine sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendMazeDistanceBridgeColoration =
        appendMazeDistanceBridgeColoration  nonAdjacentNeighbors map wholeBridgeLines

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    let color1 _ = Some globalOptionsParameters.Color1

    let colorPicker distancePicker coordinate =
        Color.linearGradient (Color.toRGB globalOptionsParameters.Color1) (Color.toRGB globalOptionsParameters.Color2) (distancePicker coordinate)        
        |> Color.toHtmlHexColor

    let renderWalls sBuilder =
        match globalOptionsParameters.WallRenderType with
        | SVG.GlobalOptions.Line ->
            sBuilder
            |> appendSimpleWalls
        | SVG.GlobalOptions.Inset ->
            sBuilder
            |> appendPathAndBridgesWithAnimation
            |> appendWallsWithInset
    
    let renderPathAndBridgesWithAnimation sBuilder =
        match globalOptionsParameters.WallRenderType with
        | SVG.GlobalOptions.Line ->
            sBuilder
            |> appendPathAndBridgesWithAnimation
        | SVG.GlobalOptions.Inset ->
            sBuilder

    let renderBackgroundColoration sBuilder =
        match globalOptionsParameters.BackgroundColoration with
        | SVG.GlobalOptions.NoColoration ->
            sBuilder
        | SVG.GlobalOptions.Plain ->
            sBuilder
            |> appendMazeColoration coordinatesPartOfMaze wholeCellLines color1
        | SVG.GlobalOptions.Distance ->
            sBuilder
            |> appendMazeDistanceColoration map wholeCellLines
        | SVG.GlobalOptions.GradientV ->
            let rowDistance = Color.rowDistance (slice2D.ToSpecializedStructure.NumberOfRows - 1)
            sBuilder
            |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker rowDistance)
        | SVG.GlobalOptions.GradientH ->
            let columnDistance = Color.columnDistance (slice2D.ToSpecializedStructure.NumberOfColumns - 1)
            sBuilder
            |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker columnDistance)
        | SVG.GlobalOptions.GradientC ->
            let center = ((float)((slice2D.ToSpecializedStructure.NumberOfRows - 1) / 2), (float)((slice2D.ToSpecializedStructure.NumberOfColumns - 1) / 2))
            let maxDistance = (calculateDistance (0.0, 0.0) center) + 1.5
            let centerDistance = Color.centerDistance center maxDistance
            sBuilder
            |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker centerDistance)
        | SVG.GlobalOptions.RandomColor (rng, color1, color2) ->
            let randomColor coordinate = Color.toHtmlHexColor (Color.random rng color1 color2 coordinate)
            sBuilder
            |> appendMazeColoration coordinatesPartOfMaze wholeCellLines randomColor
            

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle globalOptionsParameters
    |> appendBackground "transparent"
    
    |> renderBackgroundColoration

    //|> appendPath path wholeCellLines
    //|> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map.Leaves wholeCellLines
    //|> appendPathAndBridgesWithAnimation
    
    |> renderWalls
    
    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines color1 
    |> appendMazeDistanceBridgeColoration
    |> renderPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell (center parameters calculatePoints) entrance "start"
    |> textCell (center parameters calculatePoints) exit "exit"

    |> appendFooter
    |> ignore
 
    sBuilder.ToString()