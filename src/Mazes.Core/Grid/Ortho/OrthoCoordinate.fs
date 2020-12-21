// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open Mazes.Core

module OrthoCoordinate =

    let neighborCoordinateAt coordinate position =    
        match position with
        | Top ->  { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
        | Right -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        | Bottom -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
        | Left -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }

    let neighborPositionAt coordinate otherCoordinate =
        let neighborCoordinateAt = neighborCoordinateAt coordinate
        match otherCoordinate with
        | c when c = neighborCoordinateAt Top -> Top
        | c when c = neighborCoordinateAt Right -> Right
        | c when c = neighborCoordinateAt Bottom -> Bottom
        | c when c = neighborCoordinateAt Left -> Left
        | _ -> failwith "Unable to match the coordinate with a position"