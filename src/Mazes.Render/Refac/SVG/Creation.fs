// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.Creation

open System.Text
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Render.Refac.SVG.GlobalOptions
open Mazes.Render.Refac.SVG.Base

let cellPoints g parameters coordinate p =
    match g, p with
    | GridArray2DChoice g, DispositionArray2D p ->
        match p with
        | DispositionArray2D.OrthogonalDisposition p ->
            let cellPoints,_,_ = OrthoGrid.getParam parameters g
            cellPoints coordinate p

let heightWidth g parameters =
    match g with
    | GridArray2DChoice g ->
        match GridArray2D.gridStructure g with
        | GridArray2DOrthogonal _ ->
            let _,height,width = OrthoGrid.getParam parameters g
            height, width

let wholeCellLines g parameters line coordinate =

    let straightLine t ((x,y), _) = $"{t} {round x} {round y} "

    let sweepFlag = if (coordinate.Coordinate2D.RIndex + coordinate.Coordinate2D.CIndex) % 2 = 0 then "0" else "1"

    let arcLineHead (rx, ry) ((x1,y1), x2, y2) =
        $"M {round x1} {round y1} A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "
    let arcLineTail (rx, ry) ((x1,y1), x2, y2) =
        $"A {round rx} {round ry}, 0, 0, {sweepFlag}, {round x2} {round y2} "

    let drawLineHead, drawLineTail =
        match line with
        | Straight -> straightLine "M", straightLine "L"

    let dispositions =
        Grid.dispositions g coordinate.Coordinate2D
        |> Seq.map(cellPoints g parameters coordinate)

    let head =
        dispositions |> Seq.head
    let tail =
        dispositions |> Seq.tail

    ((drawLineHead head), tail)||> Seq.fold(fun s d -> s + (drawLineTail d))

let render globalOptions parameters ndStruct =

    let sBuilder = StringBuilder()

    let dimension, grid = NDimensionalStructure.firstSlice2D ndStruct

    let height, width = heightWidth grid parameters

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle globalOptions
    |> appendBackground "transparent"

    |> appendFooter
    |> ignore

    sBuilder.ToString()