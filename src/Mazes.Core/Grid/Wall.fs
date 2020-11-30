// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core.Position

type WallType =
    | Normal
    | Border
    | Empty

type Wall = {
    WallType : WallType
    WallPosition : Position
}

module Wall =
    let wallIndex position =
        match position with
        | Top -> 0
        | Right -> 1
        | Bottom -> 2
        | Left -> 3