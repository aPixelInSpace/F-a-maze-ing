// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVGNew.PolarGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.GridNew
open Mazes.Core.Trigonometry
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Render.SVG.Base

let private calculatePoints (grid : Grid<GridArrayOfA, GridNew.PolarPosition>) (centerX, centerY, ringHeight) coordinate =
    let ringIndex = coordinate.RIndex
    let cellIndex = coordinate.CIndex

    // these depends on the ring index but are recalculated for every cell
    // maybe find a way to pass them once for a given ring
    let theta = (2.0 * Math.PI) / (float)(grid.BaseGrid.ToSpecializedStructure.Cells.[ringIndex].Length)
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

let private appendWallsType (grid : Grid<GridArrayOfA, GridNew.PolarPosition>) (centerX, centerY, ringHeight) appendWall coordinate (sBuilder : StringBuilder) =
    let ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY)) =
        calculatePoints grid (centerX, centerY, ringHeight) coordinate

    let connections = (grid.BaseGrid.Cell coordinate).Connections
    let wallInward = connections |> Array.tryFind(fun w -> w.ConnectionPosition = GridNew.Inward)
    match wallInward with
    | Some wallInward ->
        appendWall sBuilder $"M {round bottomLeftX} {round bottomLeftY} A {round innerRadius} {round innerRadius}, 0, 0, 1, {round bottomRightX} {round bottomRightY}" wallInward.ConnectionType coordinate |> ignore
    | None -> ()

    let wallLeft = connections |> Array.tryFind(fun w -> w.ConnectionPosition = GridNew.Ccw)
    match wallLeft with
    | Some wallLeft ->
        appendWall sBuilder $"M {round bottomLeftX} {round bottomLeftY} L {round topLeftX} {round topLeftY}" wallLeft.ConnectionType coordinate |> ignore
    | None -> ()

    let wallOutward = connections |> Array.tryFind(fun w -> w.ConnectionPosition = GridNew.Outward)
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

let render (grid : Grid<GridArrayOfA, GridNew.PolarPosition>) (path : Coordinate seq) (map : Map) =
    
    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let ringHeight = 30
    let bridgeHalfWidth = 5.0
    let bridgeDistanceFromCenter = 7.0

    let totalOfCells = grid.BaseGrid.TotalOfCells
    let centerX = (float)((totalOfCells * ringHeight) + marginWidth)
    let centerY = (float)((totalOfCells * ringHeight) + marginHeight)

    let width = (totalOfCells * ringHeight * 2) + (marginWidth * 2)
    let height = (totalOfCells * ringHeight * 2) + (marginHeight * 2)

    let calculatePoints = calculatePoints grid (centerX, centerY, ringHeight)

    let calculatePointsBridge = calculatePointsBridge (center calculatePoints) bridgeHalfWidth bridgeDistanceFromCenter
    let appendSimpleBridges = appendSimpleBridges calculatePointsBridge grid.NonAdjacentNeighbors.All
    let appendSimpleWallsBridges = appendSimpleWallsBridges calculatePointsBridge grid.NonAdjacentNeighbors.All

    let appendWallsType = (appendWallsType grid (centerX, centerY, ringHeight))
    let wholeCellLines = wholeCellLines grid (centerX, centerY, ringHeight)
    let wholeBridgeLines = wholeBridgeLines calculatePointsBridge

    let appendSimpleWalls sBuilder =
        appendSimpleWalls grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder
    
    let appendWallsWithInset sBuilder =
        appendWallsWithInset grid.ToInterface.CoordinatesPartOfMaze appendWallsType sBuilder

    let appendPathAndBridgesWithAnimation =
        appendPathAndBridgesWithAnimation path wholeCellLines grid.NonAdjacentNeighbors.ExistNeighbor wholeBridgeLines

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle
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
    |> appendMazeBridgeColoration grid.NonAdjacentNeighbors.All wholeBridgeLines
    |> appendMazeDistanceBridgeColoration grid.NonAdjacentNeighbors.All wholeBridgeLines map.ShortestPathGraph.NodeDistanceFromRoot map.FarthestFromRoot.Distance
    |> appendPathAndBridgesWithAnimation
    |> appendSimpleWallsBridges

    |> appendFooter
    |> ignore

    sBuilder.ToString()