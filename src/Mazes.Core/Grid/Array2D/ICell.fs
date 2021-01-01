// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core
open Mazes.Core.Grid

type ICell<'Position> =

    abstract member Create : Wall<'Position> array -> ICell<'Position> 
    abstract member Walls : Wall<'Position> array
    abstract member WallIndex : 'Position -> int
    abstract member WallTypeAtPosition : IPosition<'Position> -> WallType
    abstract member IsALink : WallType -> bool
    abstract member IsLinkedAt : IPosition<'Position> -> bool
    abstract member AreLinked : Coordinate -> Coordinate -> bool
    abstract member IsLinked : bool