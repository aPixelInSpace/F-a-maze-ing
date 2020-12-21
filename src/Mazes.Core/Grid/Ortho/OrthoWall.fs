// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open Mazes.Core
open Mazes.Core.Grid.Ortho

[<Struct>]
type OrthoWall = {
    WallType : WallType
    WallPosition : OrthoPosition
}