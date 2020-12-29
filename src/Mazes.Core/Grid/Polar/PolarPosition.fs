// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open Mazes.Core

type PolarPosition =
    | Inward
    | Outward
    | Left
    | Right

    member this.Opposite =
        match this with
        | Inward -> Outward
        | Outward -> Inward
        | Left -> Right
        | Right -> Left

module PolarPosition =

    let map position =
        match position with
        | Position.Top -> Inward
        | Position.Right -> Right
        | Position.Bottom -> Outward
        | Position.Left -> Left