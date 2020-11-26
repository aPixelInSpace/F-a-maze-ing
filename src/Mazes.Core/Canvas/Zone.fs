// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

namespace Mazes.Core.Canvas

type Zone =
    | PartOfMaze
    | NotPartOfMaze

    member this.IsAPartOfMaze =
        this = PartOfMaze

module Zone =

    let create isPartOfMaze =
        match isPartOfMaze with
        | true -> PartOfMaze
        | false -> NotPartOfMaze