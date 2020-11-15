namespace Mazes.Core.Cell

type WallType =
    | Normal
    | Border
    | Empty

type WallPosition =
    | Top
    | Right
    | Bottom
    | Left

type Wall = {
    WallType : WallType
    WallPosition : WallPosition
}

type CellType =
    | PartOfMaze
    | NotPartOfMaze

type Cell = {
    CellType : CellType
    WallTop : Wall
    WallRight : Wall
    WallBottom : Wall
    WallLeft : Wall
}