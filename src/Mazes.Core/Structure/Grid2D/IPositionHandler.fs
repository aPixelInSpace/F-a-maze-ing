// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D

open Mazes.Core
open Mazes.Core.Structure

type IPositionHandler<'Position> =

    abstract member Opposite : sourceCoordinate:Coordinate2D -> position:'Position -> 'Position
    abstract member Values : Coordinate2D -> 'Position array
    abstract member Map : Coordinate2D -> Position -> 'Position