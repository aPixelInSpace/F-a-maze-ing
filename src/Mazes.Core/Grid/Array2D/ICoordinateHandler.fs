﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core

type ICoordinateHandler<'Position> =

    abstract member NeighborCoordinateAt : coordinate:Coordinate -> position:'Position -> Coordinate
    abstract member NeighborPositionAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> 'Position