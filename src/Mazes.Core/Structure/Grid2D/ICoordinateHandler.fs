// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D

open Mazes.Core

type ICoordinateHandlerArray2D<'Position> =

    abstract member NeighborCoordinateAt : coordinate:Coordinate2D -> position:'Position -> Coordinate2D option
    abstract member NeighborPositionAt : coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> 'Position
    abstract member WeaveCoordinates : Coordinate2D seq -> (Coordinate2D * Coordinate2D) seq

type ICoordinateHandlerArrayOfA<'Position> =

    abstract member NeighborsCoordinateAt : arrayOfA:'A[][] -> coordinate:Coordinate2D -> position:'Position -> Coordinate2D seq
    abstract member NeighborPositionAt : arrayOfA:'A[][] -> coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> 'Position
    abstract member WeaveCoordinates : (Coordinate2D -> bool) -> (int -> (int * int)) -> Coordinate2D seq -> (Coordinate2D * Coordinate2D) seq