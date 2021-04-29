// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.Creation

open System.Text
open Mazes.Core.Refac
open Mazes.Core.Refac.Trigonometry
open Mazes.Core.Refac.Structure
open Mazes.Render.Refac.SVG.GlobalOptions
open Mazes.Render.Refac.SVG.Base

type GridParameters =
    | OrthoParameters of OrthoGrid.Parameters

let cellPoints g gridParameters coordinate p =
    match g, gridParameters, p with
    | GridArray2DChoice g, OrthoParameters parameters, DispositionArray2D p ->
        match p with
        | DispositionArray2D.OrthogonalDisposition p ->
            let cellPoints, _, _ = OrthoGrid.getParam parameters g
            cellPoints coordinate p

let frameSize g gridParameters =
    match g, gridParameters with
    | GridArray2DChoice g, OrthoParameters parameters ->
        match GridArray2D.gridStructure g with
        | GridArray2DOrthogonal _ ->
            let _, height, width = OrthoGrid.getParam parameters g
            height, width

let getRadius gridParameters arcLine =
    match gridParameters with
    | OrthoParameters parameters ->
        let perfectRadius = lazy (pythagorasHypotenuse ((float)parameters.Width) ((float)parameters.Height) / 2.0)
        match arcLine with
        | Circle -> (perfectRadius.Value, perfectRadius.Value)
        | FixedCurve (rx, ry) -> (float rx, float ry)
        | RandomCurve (rng, mult) ->
            let min = perfectRadius.Value
            let max = min * mult
            let nextFloat =
                rng.NextDouble() * (max - min) + min
            (nextFloat, nextFloat)

let wholeCellLines g gridParameters line coordinate =

    let straightLine t ((x,y), (_,_)) = $"{t} {round x} {round y} "

    let sweepFlag = if (coordinate.Coordinate2D.RIndex + coordinate.Coordinate2D.CIndex) % 2 = 0 then "0" else "1"

    let arcLineHead (rx, ry) ((x1,y1), (x2, y2)) =
        $"M {round x1} {round y1} A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "
    let arcLineTail (rx, ry) ((_,_), (x2, y2)) =
        $"A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "

    let drawLineHead, drawLineTail =
        match line with
        | Straight -> straightLine "M", straightLine "L"
        | Arc a ->
            let radius = getRadius gridParameters a
            arcLineHead radius, arcLineTail radius

    let dispositions =
        Grid.dispositions g coordinate.Coordinate2D
        |> Seq.map(cellPoints g gridParameters coordinate)

    let head =
        dispositions |> Seq.head
    let tail =
        dispositions |> Seq.tail

    ((drawLineHead head), tail)||> Seq.fold(fun s d -> s + (drawLineTail d))

//let renderBackgroundColoration n d g globalOptions gridParameters sBuilder =
//    let coord = lazy (NDimensionalStructure.coordinatesPartOfMazeOfDimension d n)
//    let wholeCellLines = wholeCellLines g gridParameters globalOptions.LineType
//    
//    match globalOptions.BackgroundColoration with
//    | NoColoration ->
//        sBuilder
//    | Plain ->
//        let color1 _ = Some globalOptions.Color1
//        sBuilder
//        |> appendMazeColoration coord.Value wholeCellLines color1
//    | Distance ->
//        sBuilder
//        |> appendMazeDistanceColoration map wholeCellLines
//    | GradientV ->
//        let rowDistance = Color.rowDistance (slice2D.ToSpecializedStructure.NumberOfRows - 1)
//        sBuilder
//        |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker rowDistance)
//    | GradientH ->
//        let columnDistance = Color.columnDistance (slice2D.ToSpecializedStructure.NumberOfColumns - 1)
//        sBuilder
//        |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker columnDistance)
//    | GradientC ->
//        let center = ((float)((slice2D.ToSpecializedStructure.NumberOfRows - 1) / 2), (float)((slice2D.ToSpecializedStructure.NumberOfColumns - 1) / 2))
//        let maxDistance = (calculateDistance (0.0, 0.0) center) + 1.5
//        let centerDistance = Color.centerDistance center maxDistance
//        sBuilder
//        |> appendMazeColoration coordinatesPartOfMaze wholeCellLines (colorPicker centerDistance)
//    | RandomColor (rng, color1, color2) ->
//        let randomColor coordinate = Color.toHtmlHexColor (Color.random rng color1 color2 coordinate)
//        sBuilder
//        |> appendMazeColoration coordinatesPartOfMaze wholeCellLines randomColor

let render globalOptions gridParameters ndStruct =

    let sBuilder = StringBuilder()

    let dimension, grid = NDimensionalStructure.firstSlice2D ndStruct

    let height, width = frameSize grid gridParameters

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle globalOptions
    |> appendBackground "transparent"

    |> appendFooter
    |> ignore

    sBuilder.ToString()