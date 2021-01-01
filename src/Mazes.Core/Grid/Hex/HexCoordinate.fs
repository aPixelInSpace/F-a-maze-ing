// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Hex

open Mazes.Core

module HexCoordinate =

    let neighborCoordinateAt coordinate position =    
        match position with
        | HexPosition.TopLeft ->  { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
        | HexPosition.Top -> { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
        | HexPosition.TopRight -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        | HexPosition.BottomLeft -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex - 1 }
        | HexPosition.Bottom -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
        | HexPosition.BottomRight -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }

    let neighborPositionAt coordinate otherCoordinate =
        let neighborCoordinateAt = neighborCoordinateAt coordinate
        match otherCoordinate with
        | c when c = neighborCoordinateAt HexPosition.TopLeft -> HexPosition.TopLeft
        | c when c = neighborCoordinateAt HexPosition.Top -> HexPosition.Top
        | c when c = neighborCoordinateAt HexPosition.TopRight -> HexPosition.TopRight
        | c when c = neighborCoordinateAt HexPosition.BottomLeft -> HexPosition.BottomLeft
        | c when c = neighborCoordinateAt HexPosition.Bottom -> HexPosition.Bottom
        | c when c = neighborCoordinateAt HexPosition.BottomRight -> HexPosition.BottomRight
        | _ -> failwith "Unable to match the hex coordinates with a position"