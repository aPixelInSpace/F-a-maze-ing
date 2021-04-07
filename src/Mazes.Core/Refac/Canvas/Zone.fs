// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Canvas

type Zone =
    | PartOfMaze
    | Empty

    member this.IsAPartOfMaze =
        this = PartOfMaze

module Zone =

    let create isPartOfMaze =
        if isPartOfMaze then PartOfMaze else Empty