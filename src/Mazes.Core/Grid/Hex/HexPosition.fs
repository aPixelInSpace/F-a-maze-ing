// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Hex

open Mazes.Core

type HexPosition =
    | TopLeft
    | Top
    | TopRight
    | BottomLeft
    | Bottom
    | BottomRight

    member this.Opposite =
        match this with
        | TopLeft -> BottomRight
        | Top -> Bottom
        | TopRight -> BottomLeft
        | BottomLeft -> TopRight
        | Bottom -> Top
        | BottomRight -> TopLeft

module OrthoPosition =

    let map position =
        match position with
        | Position.Top -> Top
        | Position.Left -> TopLeft
        | Position.Bottom -> Bottom
        | Position.Right -> TopRight
