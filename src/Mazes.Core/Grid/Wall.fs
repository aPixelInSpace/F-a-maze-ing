// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

type Position =
    | Top
    | Right
    | Bottom
    | Left

    member this.Opposite =
        match this with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right

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