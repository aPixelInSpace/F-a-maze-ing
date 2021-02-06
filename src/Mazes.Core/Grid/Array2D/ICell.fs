// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core
open Mazes.Core.Grid

type ICell<'Position> =

    abstract member Create : Connection<'Position> array -> ICell<'Position> 
    abstract member Walls : Connection<'Position> array
    abstract member WallIndex : 'Position -> int
    abstract member WallTypeAtPosition : 'Position -> ConnectionType
    abstract member IsALink : ConnectionType -> bool
    abstract member IsLinkedAt : 'Position -> bool
    abstract member AreLinked : Coordinate -> Coordinate -> bool
    /// Returns true if the cell has at least one link
    abstract member IsLinked : bool