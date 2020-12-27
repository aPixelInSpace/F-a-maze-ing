// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Position

type Position =
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