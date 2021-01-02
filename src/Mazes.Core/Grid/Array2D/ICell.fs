// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core
open Mazes.Core.Grid

type ICell<'Position> =

    abstract member Create : Wall<'Position> array -> ICell<'Position> 
    abstract member Walls : Wall<'Position> array
    abstract member WallIndex : 'Position -> int
    abstract member WallTypeAtPosition : 'Position -> WallType
    abstract member IsALink : WallType -> bool
    abstract member IsLinkedAt : 'Position -> bool
    abstract member AreLinked : Coordinate -> Coordinate -> bool
    /// Returns true if the cell has at least one link
    abstract member IsLinked : bool