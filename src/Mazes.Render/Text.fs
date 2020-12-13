// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Text

open System.Text
open Mazes.Core
open Mazes.Core.Position
open Mazes.Core.Array2D
open Mazes.Core.Grid.Ortho

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

let private ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid (coordinate : Coordinate) pos1 pos2 =
    let neighborAtPos1 = coordinate.NeighborCoordinateAtPosition pos1
    match grid.Canvas.ExistAt neighborAtPos1 with
    | true ->
        (grid.Cell neighborAtPos1).WallTypeAtPosition pos2
    | false -> Empty

let private append
    (sBuilder : StringBuilder) (grid : OrthoGrid) coordinate
    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom,
     middleWall,
     (lastIntersectionWallLeft : WallType Lazy), (lastIntersectionWallTop : WallType Lazy), lastIntersectionWallRight, lastIntersectionWallBottom) =

    // intersection
    sBuilder.Append(getPieceOfWall
                        intersectionWallLeft
                        intersectionWallTop
                        intersectionWallRight
                        intersectionWallBottom) |> ignore

    // middle part
    [1 .. repetitionsMiddlePart ] |> List.iter(fun _ ->
        sBuilder.Append(getPieceOfWall
                            middleWall
                            Empty
                            middleWall
                            Empty) |> ignore)

    // last part only on the last column
    if (coordinate.ColumnIndex = grid.Canvas.MaxColumnIndex) then
        sBuilder.Append(getPieceOfWall
                            lastIntersectionWallLeft.Value
                            lastIntersectionWallTop.Value
                            lastIntersectionWallRight
                            lastIntersectionWallBottom) |> ignore
    ()

let private wallTypes (grid : OrthoGrid) coordinate =
    let cell = grid.Cell coordinate
    
    let ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate
    
    let intersectionWallLeft = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Left Top        

    let intersectionWallTop = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Top Left
    
    let intersectionWallRight = cell.WallTypeAtPosition Top

    let intersectionWallBottom = cell.WallTypeAtPosition Left
    
    let middleWall = cell.WallTypeAtPosition Top

    let lastIntersectionWallLeft = (lazy (cell.WallTypeAtPosition Top))

    let lastIntersectionWallTop = (lazy (ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Top Right))

    let lastIntersectionWallRight = Empty

    let lastIntersectionWallBottom = cell.WallTypeAtPosition Right

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private wallTypesLastRow (grid : OrthoGrid) coordinate =
    let cell = grid.Cell coordinate    
    let ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate
    
    let intersectionWallLeft = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Left Bottom

    let intersectionWallTop = cell.WallTypeAtPosition Left

    let intersectionWallRight = cell.WallTypeAtPosition Bottom

    let intersectionWallBottom = Empty
    
    let middleWall = cell.WallTypeAtPosition Bottom

    let lastIntersectionWallLeft = (lazy (cell.WallTypeAtPosition Bottom))

    let lastIntersectionWallTop = (lazy (cell.WallTypeAtPosition Right))    

    let lastIntersectionWallRight = Empty

    let lastIntersectionWallBottom = Empty

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private appendColumns rowIndex lastColumnIndex append wallTypes =
    for columnIndex in 0 .. lastColumnIndex do
        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
        append coordinate (wallTypes coordinate)

let private appendRows sBuilder grid =
    let append = append sBuilder grid

    let lastRowIndex = grid.Cells |> maxRowIndex
    let lastColumnIndex = grid.Cells |> maxColumnIndex

    for rowIndex in 0 .. lastRowIndex do
        let appendColumns = appendColumns rowIndex lastColumnIndex append

        appendColumns (wallTypes grid)
        sBuilder.Append("\n") |> ignore

        if rowIndex = lastRowIndex then
            appendColumns (wallTypesLastRow grid)

let renderGrid grid =
    let sBuilder = StringBuilder()

    appendRows sBuilder grid

    sBuilder.ToString()