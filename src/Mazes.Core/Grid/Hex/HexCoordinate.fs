// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Hex

open Mazes.Core

module HexCoordinate =

    let neighborCoordinateAt coordinate position =    
        match position with
        | TopLeft ->  { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
        | Top -> { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
        | TopRight -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        | BottomLeft -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex - 1 }
        | Bottom -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
        | BottomRight -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }

    let neighborPositionAt coordinate otherCoordinate =
        let neighborCoordinateAt = neighborCoordinateAt coordinate
        match otherCoordinate with
        | c when c = neighborCoordinateAt TopLeft -> TopLeft
        | c when c = neighborCoordinateAt Top -> Top
        | c when c = neighborCoordinateAt TopRight -> TopRight
        | c when c = neighborCoordinateAt BottomLeft -> BottomLeft
        | c when c = neighborCoordinateAt Bottom -> Bottom
        | c when c = neighborCoordinateAt BottomRight -> BottomRight
        | _ -> failwith "Unable to match the hex coordinates with a position"