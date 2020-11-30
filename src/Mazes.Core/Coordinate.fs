// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open Mazes.Core.Position

type Coordinate =
    {
        RowIndex : int
        ColumnIndex : int
    }
    
    member this.NeighborCoordinateAtPosition position =    
        match position with
        | Top -> { this with RowIndex = this.RowIndex - 1; }
        | Right -> { this with ColumnIndex = this.ColumnIndex + 1 }
        | Bottom -> { this with RowIndex = this.RowIndex + 1; }
        | Left -> { this with ColumnIndex = this.ColumnIndex - 1 }