// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

module PolarCellM =

    let value (PolarCell c) = c
    
    let listOfPossiblePositionsCoordinates coordinate =
        [|
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }, PolarDisposition PolarDisposition.Ccw
            { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }, PolarDisposition PolarDisposition.Inward
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }, PolarDisposition PolarDisposition.Cw
            { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }, PolarDisposition PolarDisposition.Outward
        |]