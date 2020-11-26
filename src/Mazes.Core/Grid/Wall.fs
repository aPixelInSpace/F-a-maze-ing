// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

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