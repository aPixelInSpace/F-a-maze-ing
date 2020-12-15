// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open Mazes.Core.Position

[<Struct>]
type Coordinate =
    {
        RowIndex : int
        ColumnIndex : int
    }

    member this.NeighborCoordinateAtPosition position =    
        match position with
        | Top ->  { RowIndex = this.RowIndex - 1; ColumnIndex = this.ColumnIndex }
        | Right -> { RowIndex = this.RowIndex; ColumnIndex = this.ColumnIndex + 1 }
        | Bottom -> { RowIndex = this.RowIndex + 1; ColumnIndex = this.ColumnIndex }
        | Left -> { RowIndex = this.RowIndex; ColumnIndex = this.ColumnIndex - 1 }

    member this.NeighborPositionAtCoordinate coordinate =
        match coordinate with
        | c when c = this.NeighborCoordinateAtPosition Top -> Top
        | c when c = this.NeighborCoordinateAtPosition Right -> Right
        | c when c = this.NeighborCoordinateAtPosition Bottom -> Bottom
        | c when c = this.NeighborCoordinateAtPosition Left -> Left
        | _ -> failwith "Unable to match the coordinate with a position"

    member this.ToString =
        $"{this.RowIndex};{this.ColumnIndex}"