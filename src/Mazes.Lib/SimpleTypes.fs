namespace Mazes.Lib.SimpleTypes

type WallType =
    | Normal
    | Border
    | Empty

type WallPosition =
    | Top
    | Right
    | Bottom
    | Left

/// CellWall
type Wall = {
    WallType : WallType
    WallPosition : WallPosition
}

type CellType =
    | PartOfMaze
    | NotPartOfMaze

/// CellQuadrilateral
type Cell = {
    CellType : CellType
    WallTop : Wall
    WallRight : Wall
    WallBottom : Wall
    WallLeft : Wall
}
// Todo : add some helper functions for a cell