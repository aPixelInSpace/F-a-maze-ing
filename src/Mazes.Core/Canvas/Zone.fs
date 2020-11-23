namespace Mazes.Core.Canvas

type Zone =
    | PartOfMaze
    | NotPartOfMaze

module Zone =

    let create isPartOfMaze =
        match isPartOfMaze with
        | true -> PartOfMaze
        | false -> NotPartOfMaze