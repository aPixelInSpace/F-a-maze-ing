// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PolarGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Trigonometry
open Mazes.Render.SVG.Base

let private calculatePoints (grid : IAdjacentStructure<GridArrayOfA, PolarPosition>) (centerX, centerY, ringHeight) (coordinate : NCoordinate) =
    let coordinate2D = coordinate.ToCoordinate2D
    let ringIndex = coordinate2D.RIndex
    let cellIndex = coordinate2D.CIndex

    // these depends on the ring index but are recalculated for every cell
    // maybe find a way to pass them once for a given ring
    let theta = (2.0 * Math.PI) / (float)(grid.ToSpecializedStructure.Cells.[ringIndex].Length)
    let innerRadius = (float)(ringIndex * ringHeight)
    let outerRadius = (float)((ringIndex + 1) * ringHeight)

    // these depends on the cell index, so it's fine
    let thetaCcw = (float)cellIndex * theta
    let thetaCw = ((float)(cellIndex + 1)) * theta

    let bottomLeftX = centerX + innerRadius * Math.Cos(thetaCcw)
    let bottomLeftY = centerY + innerRadius * Math.Sin(thetaCcw)
    let topLeftX = centerX + outerRadius * Math.Cos(thetaCcw)
    let topLeftY = centerY + outerRadius * Math.Sin(thetaCcw)
    let bottomRightX = centerX + innerRadius * Math.Cos(thetaCw)
    let bottomRightY = centerY + innerRadius * Math.Sin(thetaCw)
    let topRightX = centerX + outerRadius * Math.Cos(thetaCw)
    let topRightY = centerY + outerRadius * Math.Sin(thetaCw)

    ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY))

let center calculatePoints coordinate =
    let (_, _, topLeft, bottomRight, _) = calculatePoints coordinate

    middlePoint topLeft bottomRight // this is a good enough approximation

let private appendWallsType (grid : IAdjacentStructure<GridArrayOfA, PolarPosition>) (centerX, centerY, ringHeight) appendWall coordinate (sBuilder : StringBuilder) =
    let ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY)) =
        calculatePoints grid (centerX, centerY, ringHeight) coordinate

    let coordinate2D = coordinate.ToCoordinate2D
    let connections = (grid.Cell coordinate2D).Connections
    let wallInward = connections |> Array.tryFind(fun w -> w.ConnectionPosition = Inward)
    match wallInward with
    | Some wallInward ->
        appendWall sBuilder $"M {round bottomLeftX} {round bottomLeftY} A {round innerRadius} {round innerRadius}, 0, 0, 1, {round bottomRightX} {round bottomRightY}" wallInward.ConnectionType coordinate |> ignore
    | None -> ()

    let wallLeft = connections |> Array.tryFind(fun w -> w.ConnectionPosition = Ccw)
    match wallLeft with
    | Some wallLeft ->
        appendWall sBuilder $"M {round bottomLeftX} {round bottomLeftY} L {round topLeftX} {round topLeftY}" wallLeft.ConnectionType coordinate |> ignore
    | None -> ()

    let wallOutward = connections |> Array.tryFind(fun w -> w.ConnectionPosition = Outward)
    match wallOutward with
    | Some wallOutward ->
        appendWall sBuilder $"M {round topLeftX} {round topLeftY} A {round outerRadius} {round outerRadius}, 0, 0, 1, {round topRightX} {round topRightY}" wallOutward.ConnectionType coordinate |> ignore
    | None -> ()

let private wholeCellLines grid (centerX, centerY, ringHeight) coordinate =
    let ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY)) =
        calculatePoints grid (centerX, centerY, ringHeight) coordinate

    $"M {round bottomLeftX} {round bottomLeftY} " +
    $"L {round topLeftX} {round topLeftY} " +
    $"A {round outerRadius} {round outerRadius}, 0, 0, 1, {round topRightX} {round topRightY} " +
    $"L {round bottomRightX} {round bottomRightY} " +
    $"A {round innerRadius} {round innerRadius}, 0, 0, 0, {round bottomLeftX} {round bottomLeftY}"

let render (grid : NDimensionalStructure<GridArrayOfA, PolarPosition>) path map entrance exit =
    
    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let ringHeight = 30
    let bridgeHalfWidth = 5.0
    let bridgeDistanceFromCenter = 7.0

    let (dimension, slice2D) = grid.FirstSlice2D
    let totalOfCells = slice2D.TotalOfCells

    let centerX = (float)((totalOfCells * ringHeight) + marginWidth)
    let centerY = (float)((totalOfCells * ringHeight) + marginHeight)

    let width = (totalOfCells * ringHeight * 2) + (marginWidth * 2)
    let height = (totalOfCells * ringHeight * 2) + (marginHeight * 2)

    let coordinatesPartOfMaze = grid.CoordinatesPartOfMaze
    let nonAdjacentNeighbors = (grid.NonAdjacent2DConnections.All (Some dimension))

    let calculatePoints = calculatePoints slice2D (centerX, centerY, ringHeight)

    let calculatePointsBridge = calculatePointsBridge (center calculatePoints) bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge nonAdjacentNeighbors
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge nonAdjacentNeighbors

    let appendWallsType = (appendWallsType slice2D (centerX, centerY, ringHeight))
    let wholeCellLines = wholeCellLines slice2D (centerX, centerY, ringHeight)
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge

    let appendSimpleWalls sBuilder =
        appendSimpleWalls coordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset coordinatesPartOfMaze appendWallsType sBuilder

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacent2DConnections.ExistNeighbor wholeBridgeLines

    let blankColor _ = Some "white"
    let colorPicker coordinate = None

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle GlobalOptions.Parameters.Default
    |> appendBackground "transparent"

    //|> appendMazeColoration (coordinatesPartOfMaze |> Seq.map(snd)) wholeCellLines
    |> appendMazeDistanceColoration map wholeCellLines

    //|> appendPath path wholeCellLines
    //|> appendPathWithAnimation path wholeCellLines
    //|> appendLeaves map wholeCellLines

    |> appendSimpleWalls

    //|> appendPathAndBridgesWithAnimation
    //|> appendWallsWithInset

    |> appendSimpleBridges
    |> appendMazeBridgeColoration nonAdjacentNeighbors wholeBridgeLines blankColor
    |> appendMazeDistanceBridgeColoration nonAdjacentNeighbors map wholeBridgeLines
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> textCell (center calculatePoints) entrance "start"
    |> textCell (center calculatePoints) exit "exit"

    |> appendFooter
    |> ignore

    sBuilder.ToString()