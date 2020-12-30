// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open Mazes.Core

type PolarPosition =
    | Inward
    | Outward
    | Ccw // counter-clockwise
    | Cw // clockwise

    member this.Opposite =
        match this with
        | Inward -> Outward
        | Outward -> Inward
        | Ccw -> Cw
        | Cw -> Ccw

module PolarPosition =

    let map position =
        match position with
        | Position.Top -> Inward
        | Position.Right -> Cw
        | Position.Bottom -> Outward
        | Position.Left -> Ccw