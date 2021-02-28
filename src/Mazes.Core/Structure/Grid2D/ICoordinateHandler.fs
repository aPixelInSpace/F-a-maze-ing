// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D

open Mazes.Core

type ICoordinateHandler<'Position> =

    abstract member NeighborCoordinateAt : coordinate:Coordinate2D -> position:'Position -> Coordinate2D option
    abstract member NeighborPositionAt : coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> 'Position
    abstract member WeaveCoordinates : Coordinate2D seq -> (Coordinate2D * Coordinate2D) seq