// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core
open Mazes.Core.Grid.Array2D

type ICoordinate<'Position when 'Position :> IPosition<'Position>> =

    abstract member NeighborCoordinateAt : coordinate:Coordinate -> position:'Position -> Coordinate
    abstract member NeighborPositionAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> 'Position