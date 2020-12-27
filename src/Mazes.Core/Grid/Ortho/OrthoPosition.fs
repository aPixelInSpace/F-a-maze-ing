// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open Mazes.Core.Position

type OrthoPosition =
    | Top
    | Right
    | Bottom
    | Left

    member this.Opposite =
        match this with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right

module OrthoPosition =

    let map position =
        match position with
        | Position.Top -> Top
        | Position.Right -> Right
        | Position.Bottom -> Bottom
        | Position.Left -> Left