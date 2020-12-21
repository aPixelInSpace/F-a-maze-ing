﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open Mazes.Core
open Mazes.Core.Grid.Ortho

[<Struct>]
type PolarWall = {
    WallType : WallType
    WallPosition : PolarPosition
}