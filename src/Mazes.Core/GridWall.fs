module Mazes.Core.GridWall

open Mazes.Core.Grid

let updateWallAtPosition wallPosition wallType rowIndex columnIndex grid =
    match wallPosition with
    | Top ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallTop = { WallType = wallType; WallPosition = Position.Top } }
        grid.Cells.[rowIndex - 1, columnIndex] <- { grid.Cells.[rowIndex - 1, columnIndex] with WallBottom = { WallType = wallType; WallPosition = Position.Bottom } }
    | Right ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallRight = { WallType = wallType; WallPosition = Position.Right } }
        grid.Cells.[rowIndex, columnIndex + 1] <- { grid.Cells.[rowIndex, columnIndex + 1] with WallLeft = { WallType = wallType; WallPosition = Position.Left } }
    | Bottom ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallBottom = { WallType = wallType; WallPosition = Position.Bottom } }
        grid.Cells.[rowIndex + 1, columnIndex] <- { grid.Cells.[rowIndex + 1, columnIndex] with WallTop = { WallType = wallType; WallPosition = Position.Top } }
    | Left ->
        grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallLeft = { WallType = wallType; WallPosition = Position.Left } }
        grid.Cells.[rowIndex, columnIndex - 1] <- { grid.Cells.[rowIndex, columnIndex - 1] with WallRight = { WallType = wallType; WallPosition = Position.Right } }