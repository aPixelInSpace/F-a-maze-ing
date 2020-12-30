// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PolarGrid

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Polar
open Mazes.Render.SVG.Base

let private appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

let private calculatePoints (grid : PolarGrid) (centerX, centerY, ringHeight) coordinate =
    let ringIndex = coordinate.RIndex
    let cellIndex = coordinate.CIndex

    // these depends on the ring index but are recalculated for every cell
    // maybe find a way to pass them once for a given ring
    let ring = grid.Cells.[ringIndex]
    let theta = (2.0 * Math.PI) / (float)ring.Length
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

let private appendWallsType (grid : PolarGrid) (centerX, centerY, ringHeight) (cell, coordinate) (sBuilder : StringBuilder) =
    let ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY)) =
        calculatePoints grid (centerX, centerY, ringHeight) coordinate

    let appendWall = appendWall sBuilder

    // todo : put a parameter for cobweb style : 0, 0, 1, -> 0, 0, 0,
    let wallInward = cell.Walls |> Array.tryFind(fun w -> w.WallPosition = Inward)
    match wallInward with
    | Some wallInward ->
        appendWall $"M {round bottomLeftX} {round bottomLeftY} A {round innerRadius} {round innerRadius}, 0, 0, 1, {round bottomRightX} {round bottomRightY}" wallInward.WallType |> ignore
    | None -> ()

    let wallLeft = cell.Walls |> Array.tryFind(fun w -> w.WallPosition = Ccw)
    match wallLeft with
    | Some wallLeft ->
        appendWall $"M {round bottomLeftX} {round bottomLeftY} L {round topLeftX} {round topLeftY}" wallLeft.WallType |> ignore
    | None -> ()

    let wallOutward = cell.Walls |> Array.tryFind(fun w -> w.WallPosition = Outward)
    match wallOutward with
    | Some wallOutward ->
        appendWall $"M {round topLeftX} {round topLeftY} A {round outerRadius} {round outerRadius}, 0, 0, 1, {round topRightX} {round topRightY}" wallOutward.WallType |> ignore
    | None -> ()

let private wholeCellLines grid (centerX, centerY, ringHeight) coordinate =
    let ((innerRadius, outerRadius), (bottomLeftX, bottomLeftY), (topLeftX, topLeftY), (bottomRightX, bottomRightY), (topRightX, topRightY)) =
        calculatePoints grid (centerX, centerY, ringHeight) coordinate
    $"M {round bottomLeftX} {round bottomLeftY} " +
    $"L {round topLeftX} {round topLeftY} " +
    $"A {round outerRadius} {round outerRadius}, 0, 0, 1, {round topRightX} {round topRightY} " +
    $"L {round bottomRightX} {round bottomRightY} " +
    $"A {round innerRadius} {round innerRadius}, 0, 0, 0, {round bottomLeftX} {round bottomLeftY}"

let render (grid : PolarGrid) (path : Coordinate seq) (map : Map) =
    
    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let ringHeight = 15
    let centerX = (float)((grid.Cells.Length * ringHeight) + marginWidth)
    let centerY = (float)((grid.Cells.Length * ringHeight) + marginHeight)

    let width = (grid.Cells.Length * ringHeight * 2) + (marginWidth * 2)
    let height = (grid.Cells.Length * ringHeight * 2) + (marginHeight * 2)

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle
    |> appendBackground "transparent"
    |> appendMazeColoration map (wholeCellLines grid (centerX, centerY, ringHeight))
    //|> appendPath path (wholeCellLines grid (centerX, centerY, ringHeight))
    |> appendPathWithAnimation path (wholeCellLines grid (centerX, centerY, ringHeight))
    //|> appendLeaves map.Leaves (wholeCellLines grid (centerX, centerY, ringHeight))
    |> appendWalls (grid.GetCellByCell (fun _ _ -> true)) (appendWallsType grid (centerX, centerY, ringHeight))
    |> appendFooter
    |> ignore

    sBuilder.ToString()