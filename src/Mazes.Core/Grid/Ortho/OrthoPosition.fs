// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open Mazes.Core

type OrthoPosition =
    | Left
    | Top
    | Right
    | Bottom

    member this.Opposite =
        match this with
        | Left -> Right
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top

module OrthoPosition =

    let values =
        [| Left; Top; Right; Bottom |]

    let map position =
        match position with
        | Position.Left -> Left
        | Position.Top -> Top
        | Position.Right -> Right
        | Position.Bottom -> Bottom