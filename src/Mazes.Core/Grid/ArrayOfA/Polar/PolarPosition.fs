// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.ArrayOfA.Polar

open Mazes.Core

type PolarPosition =
    | Inward
    | Outward
    /// Counter-Clockwise
    | Ccw
    /// Clockwise
    | Cw

    member this.Opposite =
        match this with
        | Inward -> Outward
        | Outward -> Inward
        | Ccw -> Cw
        | Cw -> Ccw

module PolarPosition =

    let values =
        [| Ccw; Cw; Inward; Outward |]

    let map position =
        match position with
        | Position.Top -> Inward
        | Position.Right -> Cw
        | Position.Bottom -> Outward
        | Position.Left -> Ccw