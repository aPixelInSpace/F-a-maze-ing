module Mazes.Core.GridWall

open Mazes.Core.Position

let updateWallAtPosition wallPosition wallType rowIndex columnIndex grid =
    match wallPosition with
    | Top ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallTop = { WallType = wallType; WallPosition = Top } }
        grid.Cells.[rowIndex - 1, columnIndex] <- { grid.Cells.[rowIndex - 1, columnIndex] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
    | Right ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallRight = { WallType = wallType; WallPosition = Right } }
        grid.Cells.[rowIndex, columnIndex + 1] <- { grid.Cells.[rowIndex, columnIndex + 1] with WallLeft = { WallType = wallType; WallPosition = Left } }
    | Bottom ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
        grid.Cells.[rowIndex + 1, columnIndex] <- { grid.Cells.[rowIndex + 1, columnIndex] with WallTop = { WallType = wallType; WallPosition = Top } }
    | Left ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallLeft = { WallType = wallType; WallPosition = Left } }
        grid.Cells.[rowIndex, columnIndex - 1] <- { grid.Cells.[rowIndex, columnIndex - 1] with WallRight = { WallType = wallType; WallPosition = Right } }

let ifNotAtLimitUpdateWallAtPosition wallPosition wallType rowIndex columnIndex grid =
    let isPosAtLimit = (GridCell.isALimitAt wallPosition rowIndex columnIndex grid)
    if not isPosAtLimit then
        updateWallAtPosition wallPosition wallType rowIndex columnIndex grid