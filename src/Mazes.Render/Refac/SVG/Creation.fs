// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.Creation

open System.Text
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Render.Refac.SVG.Base

let cellPoints g parameters coordinate p =
    match g, p with
    | GridArray2DChoice g, DispositionArray2DChoice p ->
        match p with
        | DispositionArray2D.Orthogonal p ->
            let cellPoints,_,_ = OrthoGrid.getParam parameters g
            cellPoints coordinate p

let heightWidth g parameters =
    match g with
    | GridArray2DChoice g ->
        match GridArray2D.gridType g with
        | GridArray2DType.Orthogonal ->
            let _,height,width = OrthoGrid.getParam parameters g
            height, width

let render globalOptionsParameters parameters ndStruct =

    let sBuilder = StringBuilder()

    let dimension, grid = NDimensionalStructure.firstSlice2D ndStruct

    let height, width = heightWidth grid parameters

    sBuilder
    |> appendHeader (width.ToString()) (height.ToString())
    |> appendStyle globalOptionsParameters
    |> appendBackground "transparent"

    |> appendFooter
    |> ignore

    sBuilder.ToString()