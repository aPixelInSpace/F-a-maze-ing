// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open Mazes.Core.Position

[<Struct>]
type Coordinate =
    {
        RIndex : int
        CIndex : int
    }

    member this.NeighborCoordinateAtPosition position =    
        match position with
        | Top ->  { RIndex = this.RIndex - 1; CIndex = this.CIndex }
        | Right -> { RIndex = this.RIndex; CIndex = this.CIndex + 1 }
        | Bottom -> { RIndex = this.RIndex + 1; CIndex = this.CIndex }
        | Left -> { RIndex = this.RIndex; CIndex = this.CIndex - 1 }

    member this.NeighborPositionAtCoordinate coordinate =
        match coordinate with
        | c when c = this.NeighborCoordinateAtPosition Top -> Top
        | c when c = this.NeighborCoordinateAtPosition Right -> Right
        | c when c = this.NeighborCoordinateAtPosition Bottom -> Bottom
        | c when c = this.NeighborCoordinateAtPosition Left -> Left
        | _ -> failwith "Unable to match the coordinate with a position"

    override this.ToString() =
        $"{this.RIndex};{this.CIndex}"