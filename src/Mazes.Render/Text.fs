// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Text

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid.Array2D.Ortho

let private repetitionsMiddlePart = 1

let private getPieceOfWall wallTypeLeft wallTypeTop wallTypeRight wallTypeBottom =
    match wallTypeLeft, wallTypeTop, wallTypeRight, wallTypeBottom with
    // Empty
    | Open, Open, Open, Open -> ' '
    
    // Normal
    | Close, Close, Close, Close -> '┼'
    
    // Normal - Empty
    | Open, Close, Close, Close -> '├'
    | Close, Close, Open, Close -> '┤'
    | Close, Open, Close, Close -> '┬'
    | Close, Close, Close, Open -> '┴'
    
    | Open, Open, Close, Close -> '╭' // or '┌'
    | Open, Close, Close, Open -> '╰' // or '└'
    | Close, Open, Open, Close -> '╮' // or '┐'
    | Close, Close, Open, Open -> '╯' // or '┘' 
    
    | Open, Close, Open, Close -> '│'
    | Close, Open, Close, Open -> '─'
    
    | Close, Open, Open, Open -> '╴'
    | Open, Close, Open, Open -> '┴' // or '╵'
    | Open, Open, Close, Open -> '╶'
    | Open, Open, Open, Close -> '┬' // or '╷'
    
    // Border    
    | ClosePersistent, ClosePersistent, ClosePersistent, ClosePersistent -> '╋'
    
    // Border - Empty
    | Open, ClosePersistent, ClosePersistent, ClosePersistent -> '┣'
    | ClosePersistent, ClosePersistent, Open, ClosePersistent -> '┫'
    | ClosePersistent, Open, ClosePersistent, ClosePersistent -> '┳'
    | ClosePersistent, ClosePersistent, ClosePersistent, Open -> '┻'
    
    | Open, Open, ClosePersistent, ClosePersistent -> '┏'
    | Open, ClosePersistent, Open, ClosePersistent -> '┃'
    | Open, ClosePersistent, ClosePersistent, Open -> '┗'
    | ClosePersistent, Open, Open, ClosePersistent -> '┓'
    | ClosePersistent, Open, ClosePersistent, Open -> '━'
    | ClosePersistent, ClosePersistent, Open, Open -> '┛'
    
    | ClosePersistent, Open, Open, Open -> ' '
    | Open, ClosePersistent, Open, Open -> '┻'
    | Open, Open, ClosePersistent, Open -> ' '
    | Open, Open, Open, ClosePersistent -> '┳'
    
    // Normal - Border
    | ClosePersistent, Close, Close, Close -> '┽'
    | Close, Close, ClosePersistent, Close -> '┾'
    | Close, ClosePersistent, Close, Close -> '╀'
    | Close, Close, Close, ClosePersistent -> '╁'
    
    | ClosePersistent, ClosePersistent, Close, Close -> '╃'
    | ClosePersistent, Close, ClosePersistent, Close -> '┿'
    | ClosePersistent, Close, Close, ClosePersistent -> '╅'
    | Close, ClosePersistent, ClosePersistent, Close -> '╄'
    | Close, ClosePersistent, Close, ClosePersistent -> '╂'
    | Close, Close, ClosePersistent, ClosePersistent -> '╆'
    
    | Close, ClosePersistent, ClosePersistent, ClosePersistent -> '╊'
    | ClosePersistent, Close, ClosePersistent, ClosePersistent -> '╈'
    | ClosePersistent, ClosePersistent, Close, ClosePersistent -> '╉'
    | ClosePersistent, ClosePersistent, ClosePersistent, Close -> '╇'
    
    // Normal (2) - Empty - Border
    | Close, Close, Open, ClosePersistent -> '┧'
    | Close, Close, ClosePersistent, Open -> '┶'
    
    | Open, Close, Close, ClosePersistent -> '┟'
    | ClosePersistent, Close, Close, Open -> '┵'
    
    | Open, ClosePersistent, Close, Close -> '┞'
    | ClosePersistent, Open, Close, Close -> '┭'
    
    | Close, Open, ClosePersistent, Close -> '┮'
    | Close, ClosePersistent, Open, Close -> '┦'
    
    | Close, Open, Close, ClosePersistent -> '┰'
    | Close, ClosePersistent, Close, Open -> '┸'
    
    | Open, Close, ClosePersistent, Close -> '┝'
    | ClosePersistent, Close, Open, Close -> '┥'
    
    // Normal - Empty - Border (2)
    | ClosePersistent, ClosePersistent, Open, Close -> '┩'
    | ClosePersistent, ClosePersistent, Close, Open -> '┹'
    
    | Open, ClosePersistent, ClosePersistent, Close -> '┡'
    | Close, ClosePersistent, ClosePersistent, Open -> '┺'
    
    | Open, Close, ClosePersistent, ClosePersistent -> '┢'
    | Close, Open, ClosePersistent, ClosePersistent -> '┲'
    
    | ClosePersistent, Open, Close, ClosePersistent -> '┱'
    | ClosePersistent, Close, Open, ClosePersistent -> '┪'
    
    | ClosePersistent, Open, ClosePersistent, Close -> '┯'
    | ClosePersistent, Close, ClosePersistent, Open -> '┷'
    
    | Open, ClosePersistent, Close, ClosePersistent -> '┠'
    | Close, ClosePersistent, Open, ClosePersistent -> '┨'
    
    // Normal - Empty (2) - Border
    | Open, Open, Close, ClosePersistent -> '┎'
    | Open, Open, ClosePersistent, Close -> '┍'
    
    | Close, Open, Open, ClosePersistent -> '┒'
    | ClosePersistent, Open, Open, Close -> '┑'
    
    | Close, ClosePersistent, Open, Open -> '┚'
    | ClosePersistent, Close, Open, Open -> '┙'
    
    | Open, Close, ClosePersistent, Open -> '┖'
    | Open, ClosePersistent, Close, Open -> '┕'
    
    | Open, Close, Open, ClosePersistent -> '╽'
    | Open, ClosePersistent, Open, Close -> '╿'
    
    | Close, Open, ClosePersistent, Open -> '╼'
    | ClosePersistent, Open, Close, Open -> '╾'

let private ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty (grid : OrthoGrid) (coordinate : Coordinate) pos1 pos2 =
    let neighborAtPos1 = OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate pos1
    match neighborAtPos1 with
    | Some neighborAtPos1 ->
        match grid.Canvas.ExistAt neighborAtPos1 with
        | true ->
            (grid.Cell neighborAtPos1).WallTypeAtPosition pos2
        | false -> Open
    | None -> failwith $"Could not find a neighbor {coordinate} at {pos1}"

let private append
    (sBuilder : StringBuilder) (grid : OrthoGrid) coordinate
    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom,
     middleWall,
     (lastIntersectionWallLeft : ConnectionType Lazy), (lastIntersectionWallTop : ConnectionType Lazy), lastIntersectionWallRight, lastIntersectionWallBottom) =

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
                            Open
                            middleWall
                            Open) |> ignore)

    // last part only on the last column
    if (coordinate.CIndex = grid.Canvas.MaxColumnIndex) then
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

    let lastIntersectionWallRight = Open

    let lastIntersectionWallBottom = cell.WallTypeAtPosition Right

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private wallTypesLastRow (grid : OrthoGrid) coordinate =
    let cell = grid.Cell coordinate    
    let ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate
    
    let intersectionWallLeft = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Left Bottom

    let intersectionWallTop = cell.WallTypeAtPosition Left

    let intersectionWallRight = cell.WallTypeAtPosition Bottom

    let intersectionWallBottom = Open
    
    let middleWall = cell.WallTypeAtPosition Bottom

    let lastIntersectionWallLeft = (lazy (cell.WallTypeAtPosition Bottom))

    let lastIntersectionWallTop = (lazy (cell.WallTypeAtPosition Right))    

    let lastIntersectionWallRight = Open

    let lastIntersectionWallBottom = Open

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private appendColumns rowIndex lastColumnIndex append wallTypes =
    for columnIndex in 0 .. lastColumnIndex do
        let coordinate = { RIndex = rowIndex; CIndex = columnIndex }
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