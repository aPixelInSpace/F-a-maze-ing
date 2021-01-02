// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Canvas

type Zone =
    | PartOfMaze
    | Empty

    member this.IsAPartOfMaze =
        this = PartOfMaze

module Zone =

    let create isPartOfMaze =
        match isPartOfMaze with
        | true -> PartOfMaze
        | false -> Empty