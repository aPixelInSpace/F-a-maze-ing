// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

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