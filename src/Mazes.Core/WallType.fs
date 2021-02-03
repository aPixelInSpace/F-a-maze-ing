// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

type WallType =
    | Normal
    | Border
    | Empty

module WallType =

    let getWallTypeForEdge isCurrentCellPartOfMaze =
        match isCurrentCellPartOfMaze with
        | true -> Border
        | false -> Empty

    let getWallTypeForInternal internalWallType isCurrentCellPartOfMaze isOtherCellPartOfMaze =
        match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
        | false, false -> Empty
        | true, true -> internalWallType
        | true, false | false, true -> Border

    let isALink wallType =
        wallType = Empty