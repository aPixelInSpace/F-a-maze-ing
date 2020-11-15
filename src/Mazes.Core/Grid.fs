namespace Mazes.Core.Grid

open Mazes.Core.Cell

module Grid =
    
    type Grid = {
        Cells : Cell[,]
        NumberOfRows : int
        NumberOfColumns : int
    }

    let hasCells grid =
        grid.Cells.Length > 0

    let minRowIndex =
        0

    let maxRowIndex grid =
        grid.NumberOfRows - 1

    let minColumnIndex =
        0

    let maxColumnIndex grid =
        grid.NumberOfColumns - 1

    module Cell =
        let existAt rowIndex columnIndex grid =
            minRowIndex <= rowIndex &&
            rowIndex <= (maxRowIndex grid) &&
            minColumnIndex <= columnIndex &&
            columnIndex <= (maxColumnIndex grid)

        let getCell rowIndex columnIndex grid =
            grid.Cells.[rowIndex, columnIndex]

        let isPartOfMaze rowIndex columnIndex grid =
            (getCell rowIndex columnIndex grid).CellType = PartOfMaze
        
        let isTopALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid

            if cell.CellType = NotPartOfMaze || cell.WallTop.WallType = Border then
                true
            else
                // the top cell is not part of the maze
                (existAt (rowIndex - 1) columnIndex grid) && (getCell (rowIndex - 1) columnIndex grid).CellType = NotPartOfMaze
            
        let isRightALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid

            if cell.CellType = NotPartOfMaze || cell.WallRight.WallType = Border then
                true
            else
                // the right cell is not part of the maze
                (existAt rowIndex (columnIndex + 1) grid) && (getCell rowIndex (columnIndex + 1) grid).CellType = NotPartOfMaze

        let isLeftALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid

            if cell.CellType = NotPartOfMaze || cell.WallLeft.WallType = Border then
                true
            else
                // the left cell is not part of the maze
                (existAt rowIndex (columnIndex - 1) grid) && (getCell rowIndex (columnIndex - 1) grid).CellType = NotPartOfMaze

        let isBottomALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid

            if cell.CellType = NotPartOfMaze || cell.WallBottom.WallType = Border then
                true
            else
                // the bottom cell is not part of the maze
                (existAt (rowIndex + 1) columnIndex grid) && (getCell (rowIndex + 1) columnIndex grid).CellType = NotPartOfMaze

    module Wall =
        let updateWallAtPosition wallPosition wallType rowIndex columnIndex grid =
            match wallPosition with
            | Top ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallTop = { WallType = wallType; WallPosition = WallPosition.Top } }
                grid.Cells.[rowIndex - 1, columnIndex] <- { grid.Cells.[rowIndex - 1, columnIndex] with WallBottom = { WallType = wallType; WallPosition = WallPosition.Bottom } }
            | Right ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallRight = { WallType = wallType; WallPosition = WallPosition.Right } }
                grid.Cells.[rowIndex, columnIndex + 1] <- { grid.Cells.[rowIndex, columnIndex + 1] with WallLeft = { WallType = wallType; WallPosition = WallPosition.Left } }
            | Bottom ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallBottom = { WallType = wallType; WallPosition = WallPosition.Bottom } }
                grid.Cells.[rowIndex + 1, columnIndex] <- { grid.Cells.[rowIndex + 1, columnIndex] with WallTop = { WallType = wallType; WallPosition = WallPosition.Top } }
            | Left ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallLeft = { WallType = wallType; WallPosition = WallPosition.Left } }
                grid.Cells.[rowIndex, columnIndex - 1] <- { grid.Cells.[rowIndex, columnIndex - 1] with WallRight = { WallType = wallType; WallPosition = WallPosition.Right } }