// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core.Position

type WallType =
    | Normal
    | Border
    | Empty

[<Struct>]
type Wall = {
    WallType : WallType
    WallPosition : Position
}