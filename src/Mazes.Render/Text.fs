﻿module Mazes.Render.Text

open System.Text
open Mazes.Core
open Mazes.Core.Extensions
open Mazes.Core.Grid

let private repetitionsMiddlePart = 1

let private getPieceOfWall wallTypeLeft wallTypeTop wallTypeRight wallTypeBottom =
    match wallTypeLeft, wallTypeTop, wallTypeRight, wallTypeBottom with
    // Empty
    | Empty, Empty, Empty, Empty -> ' '
    
    // Normal
    | Normal, Normal, Normal, Normal -> '┼'
    
    // Normal - Empty
    | Empty, Normal, Normal, Normal -> '├'
    | Normal, Normal, Empty, Normal -> '┤'
    | Normal, Empty, Normal, Normal -> '┬'
    | Normal, Normal, Normal, Empty -> '┴'
    
    | Empty, Empty, Normal, Normal -> '╭' // or '┌'
    | Empty, Normal, Normal, Empty -> '╰' // or '└'
    | Normal, Empty, Empty, Normal -> '╮' // or '┐'
    | Normal, Normal, Empty, Empty -> '╯' // or '┘' 
    
    | Empty, Normal, Empty, Normal -> '│'
    | Normal, Empty, Normal, Empty -> '─'
    
    | Normal, Empty, Empty, Empty -> '╴'
    | Empty, Normal, Empty, Empty -> '┴' // or '╵'
    | Empty, Empty, Normal, Empty -> '╶'
    | Empty, Empty, Empty, Normal -> '┬' // or '╷'
    
    // Border    
    | Border, Border, Border, Border -> '╋'
    
    // Border - Empty
    | Empty, Border, Border, Border -> '┣'
    | Border, Border, Empty, Border -> '┫'
    | Border, Empty, Border, Border -> '┳'
    | Border, Border, Border, Empty -> '┻'
    
    | Empty, Empty, Border, Border -> '┏'
    | Empty, Border, Empty, Border -> '┃'
    | Empty, Border, Border, Empty -> '┗'
    | Border, Empty, Empty, Border -> '┓'
    | Border, Empty, Border, Empty -> '━'
    | Border, Border, Empty, Empty -> '┛'
    
    | Border, Empty, Empty, Empty -> ' '
    | Empty, Border, Empty, Empty -> '┻'
    | Empty, Empty, Border, Empty -> ' '
    | Empty, Empty, Empty, Border -> '┳'
    
    // Normal - Border
    | Border, Normal, Normal, Normal -> '┽'
    | Normal, Normal, Border, Normal -> '┾'
    | Normal, Border, Normal, Normal -> '╀'
    | Normal, Normal, Normal, Border -> '╁'
    
    | Border, Border, Normal, Normal -> '╃'
    | Border, Normal, Border, Normal -> '┿'
    | Border, Normal, Normal, Border -> '╅'
    | Normal, Border, Border, Normal -> '╄'
    | Normal, Border, Normal, Border -> '╂'
    | Normal, Normal, Border, Border -> '╆'
    
    | Normal, Border, Border, Border -> '╊'
    | Border, Normal, Border, Border -> '╈'
    | Border, Border, Normal, Border -> '╉'
    | Border, Border, Border, Normal -> '╇'
    
    // Normal (2) - Empty - Border
    | Normal, Normal, Empty, Border -> '┧'
    | Normal, Normal, Border, Empty -> '┶'
    
    | Empty, Normal, Normal, Border -> '┟'
    | Border, Normal, Normal, Empty -> '┵'
    
    | Empty, Border, Normal, Normal -> '┞'
    | Border, Empty, Normal, Normal -> '┭'
    
    | Normal, Empty, Border, Normal -> '┮'
    | Normal, Border, Empty, Normal -> '┦'
    
    | Normal, Empty, Normal, Border -> '┰'
    | Normal, Border, Normal, Empty -> '┸'
    
    | Empty, Normal, Border, Normal -> '┝'
    | Border, Normal, Empty, Normal -> '┥'
    
    // Normal - Empty - Border (2)
    | Border, Border, Empty, Normal -> '┩'
    | Border, Border, Normal, Empty -> '┹'
    
    | Empty, Border, Border, Normal -> '┡'
    | Normal, Border, Border, Empty -> '┺'
    
    | Empty, Normal, Border, Border -> '┢'
    | Normal, Empty, Border, Border -> '┲'
    
    | Border, Empty, Normal, Border -> '┱'
    | Border, Normal, Empty, Border -> '┪'
    
    | Border, Empty, Border, Normal -> '┯'
    | Border, Normal, Border, Empty -> '┷'
    
    | Empty, Border, Normal, Border -> '┠'
    | Normal, Border, Empty, Border -> '┨'
    
    // Normal - Empty (2) - Border
    | Empty, Empty, Normal, Border -> '┎'
    | Empty, Empty, Border, Normal -> '┍'
    
    | Normal, Empty, Empty, Border -> '┒'
    | Border, Empty, Empty, Normal -> '┑'
    
    | Normal, Border, Empty, Empty -> '┚'
    | Border, Normal, Empty, Empty -> '┙'
    
    | Empty, Normal, Border, Empty -> '┖'
    | Empty, Border, Normal, Empty -> '┕'
    
    | Empty, Normal, Empty, Border -> '╽'
    | Empty, Border, Empty, Normal -> '╿'
    
    | Normal, Empty, Border, Empty -> '╼'
    | Border, Empty, Normal, Empty -> '╾'

let private append (sBuilder : StringBuilder) (grid : Grid) rowIndex columnIndex =

    let cell = getCell rowIndex columnIndex grid

    let startWallLeft =
        match existAt rowIndex (columnIndex - 1) grid with
        | true -> (getCell rowIndex (columnIndex - 1) grid).WallTop.WallType
        | false -> Empty

    let startWallTop =
        match existAt (rowIndex - 1) columnIndex grid with
        | true -> (getCell (rowIndex - 1) columnIndex grid).WallLeft.WallType
        | false -> Empty

    let startWallRight = cell.WallTop.WallType

    let startWallBottom = cell.WallLeft.WallType

    // starting part
    sBuilder.Append(getPieceOfWall
                        startWallLeft
                        startWallTop
                        startWallRight
                        startWallBottom) |> ignore

    // middle part
    [1 .. repetitionsMiddlePart ] |> List.iter(fun _ ->
        sBuilder.Append(getPieceOfWall
                            cell.WallTop.WallType
                            Empty
                            cell.WallTop.WallType
                            Empty) |> ignore)

    // last part only on the last column
    if (columnIndex = (maxColumnIndex grid)) then

        let endWallLeft = cell.WallTop.WallType

        let endWallTop =
            match existAt (rowIndex - 1) columnIndex grid with
            | true -> (getCell (rowIndex - 1) columnIndex grid).WallRight.WallType
            | false -> Empty

        let endWallRight = Empty

        let endWallBottom = cell.WallRight.WallType

        sBuilder.Append(getPieceOfWall
                            endWallLeft
                            endWallTop
                            endWallRight
                            endWallBottom) |> ignore

    ()

let private appendForLastRow (sBuilder : StringBuilder) (grid : Grid) rowIndex columnIndex =

    let cell = getCell rowIndex columnIndex grid

    let startWallLeft =
        match existAt rowIndex (columnIndex - 1) grid with
        | true -> (getCell rowIndex (columnIndex - 1) grid).WallBottom.WallType
        | false -> Empty

    let startWallTop = cell.WallLeft.WallType
    let startWallRight = cell.WallBottom.WallType
    let startWallBottom = Empty
    
    // starting part
    sBuilder.Append(getPieceOfWall
                        startWallLeft
                        startWallTop
                        startWallRight
                        startWallBottom) |> ignore

    // middle part
    [1 .. repetitionsMiddlePart ] |> List.iter(fun _ ->
        sBuilder.Append(getPieceOfWall
                            cell.WallBottom.WallType
                            Empty
                            cell.WallBottom.WallType
                            Empty) |> ignore)

    // last part only on the last column
    if (columnIndex = (maxColumnIndex grid)) then
        let endWallLeft = cell.WallBottom.WallType

        let endWallTop = cell.WallRight.WallType

        let endWallRight = Empty

        let endWallBottom = Empty

        sBuilder.Append(getPieceOfWall
                            endWallLeft
                            endWallTop
                            endWallRight
                            endWallBottom) |> ignore

let private appendRow (sBuilder : StringBuilder) grid rowIndex row =
    row |> Array.iteri(fun columnIndex _ -> append sBuilder grid rowIndex columnIndex |> ignore)
    sBuilder.Append("\n") |> ignore

let private appendRows sBuilder grid rows =
    // one line for each row
    rows
    |> List.iteri(fun rowIndex row -> appendRow sBuilder grid rowIndex row)

    if hasCells grid then
        // necessary to add the last line
        rows.[(maxRowIndex grid)] |> Array.iteri(fun columnIndex _ -> appendForLastRow sBuilder grid (maxRowIndex grid) columnIndex |> ignore)

let printGrid grid =
    let sBuilder = StringBuilder()

    grid.Cells
        |> Array2D.extractByRows
        |> Seq.toList
        |> appendRows sBuilder grid
    
    sBuilder.ToString()