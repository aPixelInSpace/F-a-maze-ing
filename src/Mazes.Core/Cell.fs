namespace Mazes.Core

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