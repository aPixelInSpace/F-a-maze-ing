// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core

type IPositionHandler<'Position> =

    abstract member Opposite : sourceCoordinate:Coordinate -> position:'Position -> 'Position
    abstract member Values : Coordinate -> 'Position array
    abstract member Map : Coordinate -> Position -> 'Position