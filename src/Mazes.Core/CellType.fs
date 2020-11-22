namespace Mazes.Core

type CellType =
    | PartOfMaze
    | NotPartOfMaze

module CellType =
    let create isPartOfMaze =
        match isPartOfMaze with
        | true -> PartOfMaze
        | false -> NotPartOfMaze