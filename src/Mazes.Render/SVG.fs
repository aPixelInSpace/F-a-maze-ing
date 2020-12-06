// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid

let private cellWidth = 25
let private cellHeight = 25
let private marginWidth = 20
let private marginHeight = 20

let private startLine (sBuilder : StringBuilder) rowIndex columnIndex wallType =
    let append =
        match wallType with
        | Border -> true
        | Normal -> true
        | Empty -> false

    if append then
        sBuilder.Append("<polyline points=\"" + ((columnIndex * cellWidth) + marginWidth).ToString() + "," + ((rowIndex * cellHeight) + marginHeight).ToString() + " ") |> ignore

let private endLine (sBuilder : StringBuilder) rowIndex columnIndex wallType =
    let (append, styleClass) =
        match wallType with
        | Border -> true, "b"
        | Normal -> true, "n"
        | Empty -> false, "e"

    if append then
        sBuilder.Append(((columnIndex * cellWidth) + marginWidth).ToString() + "," + ((rowIndex * cellHeight) + marginHeight).ToString() + "\" class=\"" + styleClass + "\"/>") |> ignore

// todo : eliminate the duplicate code
let private appendRows sBuilder (grid : Grid) (rows : _[] List) =
    let lastRowIndex = (rows |> Seq.length) - 1
    for rowIndex in 0 .. lastRowIndex do
        let mutable currentWallType = None

        let lastColumnIndex = (rows.[rowIndex] |> Seq.length) - 1
        for columnIndex in 0 .. lastColumnIndex do
            let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
            let wallTypeTop = (grid.Cell coordinate).WallTop.WallType
            match currentWallType with
            | Some currentWallTypeValue ->
                if currentWallTypeValue <> wallTypeTop then
                    endLine sBuilder coordinate.RowIndex coordinate.ColumnIndex currentWallTypeValue
                    startLine sBuilder coordinate.RowIndex coordinate.ColumnIndex wallTypeTop
                    currentWallType <- (Some wallTypeTop)
            | None ->
                startLine sBuilder coordinate.RowIndex coordinate.ColumnIndex wallTypeTop
                currentWallType <- (Some wallTypeTop)

            if columnIndex = lastColumnIndex then
                endLine sBuilder coordinate.RowIndex (coordinate.ColumnIndex + 1) wallTypeTop

        if rowIndex = lastRowIndex then
            currentWallType <- None

            for columnIndex in 0 .. lastColumnIndex do
                let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
                let wallTypeBottom = (grid.Cell coordinate).WallBottom.WallType
                match currentWallType with
                | Some currentWallTypeValue ->
                    if currentWallTypeValue <> wallTypeBottom then
                        endLine sBuilder (coordinate.RowIndex + 1) (coordinate.ColumnIndex) currentWallTypeValue
                        startLine sBuilder (coordinate.RowIndex + 1) (coordinate.ColumnIndex) wallTypeBottom
                        currentWallType <- (Some wallTypeBottom)
                | None ->
                    startLine sBuilder (coordinate.RowIndex + 1) (coordinate.ColumnIndex) wallTypeBottom
                    currentWallType <- (Some wallTypeBottom)

                if columnIndex = lastColumnIndex then
                    endLine sBuilder (coordinate.RowIndex + 1) (coordinate.ColumnIndex + 1) wallTypeBottom

let private appendColumns sBuilder (grid : Grid) (columns : _[] List) =
    let lastColumnIndex = (columns |> Seq.length) - 1
    for columnIndex in 0 .. lastColumnIndex do
        let mutable currentWallType = None

        let lastRowIndex = (columns.[columnIndex] |> Seq.length) - 1
        for rowIndex in 0 .. lastRowIndex do
            let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
            let wallTypeLeft = (grid.Cell coordinate).WallLeft.WallType
            match currentWallType with
            | Some currentWallTypeValue ->
                if currentWallTypeValue <> wallTypeLeft then
                    endLine sBuilder coordinate.RowIndex coordinate.ColumnIndex currentWallTypeValue
                    startLine sBuilder coordinate.RowIndex coordinate.ColumnIndex wallTypeLeft
                    currentWallType <- (Some wallTypeLeft)
            | None ->
                startLine sBuilder coordinate.RowIndex coordinate.ColumnIndex wallTypeLeft
                currentWallType <- (Some wallTypeLeft)

            if rowIndex = lastRowIndex then
                endLine sBuilder (coordinate.RowIndex + 1) coordinate.ColumnIndex wallTypeLeft

        if columnIndex = lastColumnIndex then
            currentWallType <- None

            for rowIndex in 0 .. lastRowIndex do
                let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
                let wallTypeRight = (grid.Cell coordinate).WallRight.WallType
                match currentWallType with
                | Some currentWallTypeValue ->
                    if currentWallTypeValue <> wallTypeRight then
                        endLine sBuilder (coordinate.RowIndex) (coordinate.ColumnIndex + 1) currentWallTypeValue
                        startLine sBuilder (coordinate.RowIndex) (coordinate.ColumnIndex + 1) wallTypeRight
                        currentWallType <- (Some wallTypeRight)
                | None ->
                    startLine sBuilder (coordinate.RowIndex) (coordinate.ColumnIndex + 1) wallTypeRight
                    currentWallType <- (Some wallTypeRight)

                if rowIndex = lastRowIndex then
                    endLine sBuilder (coordinate.RowIndex + 1) (coordinate.ColumnIndex + 1) wallTypeRight

let renderGrid grid =
    let sBuilder = StringBuilder()

    let svgStyle =
        """<defs>
                <style>
                    .n {
                        stroke: #000;
                        fill=transparent;
                        stroke-width: 1;
                    }
                    .b {
                        stroke: #333;
                        fill=transparent;
                        stroke-width: 3;
                    }
                </style>
            </defs>"""
    

    let width = ((grid.Canvas.NumberOfColumns) * cellWidth) + (marginWidth * 2)
    let height = ((grid.Canvas.NumberOfRows) * cellHeight) + (marginHeight * 2)

    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>") |> ignore
    sBuilder.Append("<svg width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">") |> ignore
    sBuilder.Append(svgStyle) |> ignore
    sBuilder.Append("<rect width=\"100%\" height=\"100%\" fill=\"transparent\"/>") |> ignore
    // #4287f5

    grid.Cells
        |> extractByRows
        |> Seq.toList
        |> appendRows sBuilder grid

    grid.Cells
        |> extractByColumns
        |> Seq.toList
        |> appendColumns sBuilder grid

    sBuilder.Append("</svg>") |> ignore
    sBuilder.ToString()