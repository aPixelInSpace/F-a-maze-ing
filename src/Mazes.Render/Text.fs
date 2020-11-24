module Mazes.Render.Text

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas
open Mazes.Core.Canvas.Canvas
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid

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

let private ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate pos1 pos2 =
    match existAt grid.Canvas (Cell.getNeighborCoordinateAtPosition coordinate pos1) with
    | true -> getWallTypeAtPosition (get grid.Cells (Cell.getNeighborCoordinateAtPosition coordinate pos1)) pos2
    | false -> Empty

let private append
    (sBuilder : StringBuilder) (grid : Grid) coordinate
    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom,
     middleWall,
     lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom) =

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
    if (coordinate.ColumnIndex = (maxColumnIndex grid.Canvas)) then
        sBuilder.Append(getPieceOfWall
                            (lastIntersectionWallLeft ())
                            (lastIntersectionWallTop ())
                            lastIntersectionWallRight
                            lastIntersectionWallBottom) |> ignore
    ()

let private wallTypes (grid : Grid) coordinate =
    let cell = get grid.Cells coordinate
    let getWallTypeAtPosition = getWallTypeAtPosition cell
    let ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate
    
    let intersectionWallLeft = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Left Top        

    let intersectionWallTop = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Top Left
    
    let intersectionWallRight = getWallTypeAtPosition Top

    let intersectionWallBottom = getWallTypeAtPosition Left
    
    let middleWall = getWallTypeAtPosition Top

    let lastIntersectionWallLeft = (fun () -> getWallTypeAtPosition Top)

    let lastIntersectionWallTop = (fun () -> ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Top Right)

    let lastIntersectionWallRight = Empty

    let lastIntersectionWallBottom = getWallTypeAtPosition Right

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private wallTypesLastRow (grid : Grid) coordinate =
    let cell = get grid.Cells coordinate
    let getWallTypeAtPosition = getWallTypeAtPosition cell
    let ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty grid coordinate
    
    let intersectionWallLeft = ifExistAtPos1ThenGetWallTypeAtPos2ElseEmpty Left Bottom

    let intersectionWallTop = getWallTypeAtPosition Left

    let intersectionWallRight = getWallTypeAtPosition Bottom

    let intersectionWallBottom = Empty
    
    let middleWall = getWallTypeAtPosition Bottom

    let lastIntersectionWallLeft = (fun () -> getWallTypeAtPosition Bottom)

    let lastIntersectionWallTop = (fun () -> getWallTypeAtPosition Right)

    let lastIntersectionWallRight = Empty

    let lastIntersectionWallBottom = Empty

    (intersectionWallLeft, intersectionWallTop, intersectionWallRight, intersectionWallBottom, middleWall, lastIntersectionWallLeft, lastIntersectionWallTop, lastIntersectionWallRight, lastIntersectionWallBottom)

let private appendRows sBuilder grid rows =
    let append = append sBuilder grid

    // one line for each row
    rows
    |> List.iteri(fun rowIndex row ->
        row
        |> Array.iteri(fun columnIndex _ ->
        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
        let wallTypes = wallTypes grid coordinate
        append coordinate wallTypes)

        sBuilder.Append("\n") |> ignore)

    if hasCells grid then
        // necessary to add the last char line on the last row
        let lastRowIndex = (maxRowIndex grid.Canvas)
        rows.[lastRowIndex]
        |> Array.iteri(fun columnIndex _ ->
            let coordinate = { RowIndex = lastRowIndex; ColumnIndex = columnIndex }
            let wallTypesLastRow = wallTypesLastRow grid coordinate
            append coordinate wallTypesLastRow)

let renderGrid grid =
    let sBuilder = StringBuilder()

    grid.Cells
        |> extractByRows
        |> Seq.toList
        |> appendRows sBuilder grid
    
    sBuilder.ToString()

let renderCanvas canvas =

    let appendZone (sBuilder : StringBuilder) zone =
        let char =
            match zone with
            | NotPartOfMaze -> "░░"
            | PartOfMaze -> "▓▓"

        sBuilder.Append(char) |> ignore

    let appendRow (sBuilder : StringBuilder) rowZones =
        rowZones
        |> Array.iter(fun zone -> appendZone sBuilder zone)

        sBuilder.Append('\n') |> ignore

    let sBuilder = StringBuilder()
    canvas.Zones
        |> extractByRows
        |> Seq.iter(fun rowZones -> appendRow sBuilder rowZones)
    
    sBuilder.ToString()