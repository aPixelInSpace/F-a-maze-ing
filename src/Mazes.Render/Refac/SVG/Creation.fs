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

let linePoints g gridParameters coordinate p =
    match g, gridParameters, p with
    | GridArray2DChoice g, OrthoParameters parameters, DispositionArray2D p ->
        match p with
        | DispositionArray2D.OrthogonalDisposition p ->
            let orthoParam = OrthoGrid.getConfig parameters g
            orthoParam.LinePoints coordinate p

let frameSize g gridParameters =
    match g, gridParameters with
    | GridArray2DChoice g, OrthoParameters parameters ->
        match GridArray2D.gridStructure g with
        | GridArray2DOrthogonal _ ->
            let orthoParam = OrthoGrid.getConfig parameters g
            orthoParam.Height, orthoParam.Width

let bridgePoints g gridParameters =
    match g, gridParameters with
    | GridArray2DChoice g, OrthoParameters parameters ->
        match GridArray2D.gridStructure g with
        | GridArray2DOrthogonal _ ->
            let orthoParam = OrthoGrid.getConfig parameters g
            orthoParam.BridgePoints

let drawLine gridParameters line coordinate =
    let getRadius gridParameters arcLine =
        match gridParameters with
        | OrthoParameters parameters ->
            let perfectRadius = lazy (pythagorasHypotenuse (float parameters.Width) (float parameters.Height) / 2.0)
            match arcLine with
            | Circle -> (perfectRadius.Value, perfectRadius.Value)
            | FixedCurve (rx, ry) -> (float rx, float ry)
            | RandomCurve (rng, mult) ->
                let min = perfectRadius.Value
                let max = min * mult
                let nextFloat =
                    rng.NextDouble() * (max - min) + min
                (nextFloat, nextFloat)

    let straightLine t ((x,y), (_,_)) = $"{t} {round x} {round y} "

    let sweepFlag coordinate = if (coordinate.Coordinate2D.RIndex + coordinate.Coordinate2D.CIndex) % 2 = 0 then "0" else "1"

    let arcLineHead sweepFlag (rx, ry) ((x1,y1), (x2, y2)) =
        $"M {round x1} {round y1} A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "
    let arcLineTail sweepFlag (rx, ry) ((_,_), (x2, y2)) =
        $"A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "

    match line with
    | Straight -> straightLine "M", straightLine "L"
    | Arc a ->
        let radius = getRadius gridParameters a
        let sweepFlag = sweepFlag coordinate
        arcLineHead sweepFlag radius, arcLineTail sweepFlag radius

let wholeCellLines g gridParameters line coordinate =
    let drawLineHead, drawLineTail = drawLine gridParameters line coordinate

    let dispositions =
        Grid.dispositions g coordinate.Coordinate2D
        |> Seq.map(linePoints g gridParameters coordinate)

    let head =
        dispositions |> Seq.head
    let tail =
        dispositions |> Seq.tail

    ((drawLineHead head), tail)||> Seq.fold(fun s d -> s + (drawLineTail d))

let appendWall (closeClass, closePersistentClass) g gridParameters line coordinate p (sBuilder : StringBuilder) =
    let connectionState = Grid.connectionStateAtPosition g coordinate.Coordinate2D p

    let appendPathElement wallClass =
        let drawLine, _ = drawLine gridParameters line coordinate
        let points = linePoints g gridParameters coordinate
        let line = drawLine (points p)
        appendPathElement sBuilder None wallClass line coordinate

    match connectionState with
    | ClosePersistent -> appendPathElement closePersistentClass 
    | Close -> appendPathElement closeClass
    | _ -> sBuilder

let appendWallLine = appendWall (normalWallClass, borderWallClass)
let appendWallInsetBackground = appendWall (normalWallInsetBackClass, borderWallInsetBackClass)
let appendWallInsetForeground = appendWall (normalWallInsetForeClass, borderWallInsetForeClass)

let renderBackgroundColoration g globalOptions gridParameters coord map sBuilder =
    let wholeCellLines = wholeCellLines g gridParameters globalOptions.LineType
    let colorPicker distancePicker coordinate =
        Color.linearGradient (Color.toRGB globalOptions.Color1) (Color.toRGB globalOptions.Color2) (distancePicker coordinate)        
        |> Color.toHtmlHexColor
    
    match globalOptions.BackgroundColoration with
    | NoColoration ->
        sBuilder
    | Plain ->
        sBuilder
        |> appendMazeColoration coord wholeCellLines (fun _ -> Some globalOptions.Color1)
    | Distance ->
        sBuilder
        |> appendMazeDistanceColoration map wholeCellLines
    | GradientV ->
        let rowDistance = Color.rowDistance (Grid.numberOfRows g - 1)
        sBuilder
        |> appendMazeColoration coord wholeCellLines (colorPicker rowDistance)
    | GradientH ->
        let columnDistance = Color.columnDistance (Grid.numberOfColumns g - 1)
        sBuilder
        |> appendMazeColoration coord wholeCellLines (colorPicker columnDistance)
    | GradientC ->
        let center = (float ((Grid.numberOfRows g - 1) / 2), float ((Grid.numberOfColumns g - 1) / 2))
        let maxDistance = (calculateDistance (0.0, 0.0) center) + 1.5
        let centerDistance = Color.centerDistance center maxDistance
        sBuilder
        |> appendMazeColoration coord wholeCellLines (colorPicker centerDistance)
    | RandomColor (rng, color1, color2) ->
        let randomColor coordinate = Color.toHtmlHexColor (Color.random rng color1 color2 coordinate)
        sBuilder
        |> appendMazeColoration coord wholeCellLines randomColor

let renderWalls g globalOptions gridParameters coord sBuilder =
    let appendsWall =
        match globalOptions.WallRenderType with
        | Line -> [ appendWallLine ]
        | Inset -> [ appendWallInsetBackground; appendWallInsetForeground ]

    let render appendWall =
        coord
        |> Seq.iter(fun c ->
            let dispositions = Grid.dispositions g c.Coordinate2D
            dispositions |> Seq.iter(fun d -> appendWall c d sBuilder |> ignore))

    appendsWall
    |> List.iter(fun appendWall -> render (appendWall g gridParameters globalOptions.LineType))

    sBuilder

let render globalOptions gridParameters ndStruct map =

    let sBuilder = StringBuilder()

    let dimension, grid = NDimensionalStructure.firstSlice2D ndStruct

    let height, width = frameSize grid gridParameters

    let coord = NDimensionalStructure.coordinatesPartOfMazeOfDimension dimension ndStruct

    let withParams f = f grid globalOptions gridParameters

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle globalOptions
    |> appendBackground "transparent"
    
    |> withParams renderBackgroundColoration coord map

    |> withParams renderWalls coord

    |> appendFooter
    |> ignore

    sBuilder.ToString()