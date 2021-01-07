// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.PentaCairo

open Mazes.Core
open Mazes.Core.Grid.Array2D

/// Typical left, top ... doesn't make really sense here (because the form has four rotated forms), so
/// S is the small side of the congruent convex pentagon
/// then clockwise : A, B, C and D
type PentaCairoPosition =
    | S
    | A
    | B
    | C
    | D

type Quadrant =
    /// Has the small side S outward in the upper left
    | One
    /// Has the small side S inward in the bottom left
    | Two
    /// Has the small side S inward in the top right
    | Three
    /// Has the small side S outward in the bottom right
    | Four

type PentaCairoPositionHandler private () =

    static let instance = PentaCairoPositionHandler()

    interface IPositionHandler<PentaCairoPosition> with

        member this.Opposite _ position =
            match position with
            | S -> S
            | A -> B
            | B -> A
            | C -> D
            | D -> C

        member this.Values _ =
            [| S; A; B; C; D; |]

        member this.Map coordinate position =
            match PentaCairoPositionHandler.Quadrant coordinate with
            | One ->
                match position with
                | Position.Top -> A // or S
                | Position.Left -> D
                | Position.Bottom -> C
                | Position.Right -> B
            | Two ->
                match position with
                | Position.Top -> B
                | Position.Left -> A
                | Position.Bottom -> D // or S
                | Position.Right -> C
            | Three ->
                match position with
                | Position.Top -> D // or S
                | Position.Left -> C
                | Position.Bottom -> B
                | Position.Right -> A
            | Four ->
                match position with
                | Position.Top -> C
                | Position.Left -> B
                | Position.Bottom -> A // or S
                | Position.Right -> D

    member this.ToInterface =
        this :> IPositionHandler<PentaCairoPosition>

    static member Instance =
        instance.ToInterface

    /// Four congruent convex pentagons form an hexagon.
    /// This function returns the arbitrarily assigned quadrant number for the pentagon
    /// that divide that hexagon
    static member Quadrant coordinate =
        match coordinate.RIndex % 2 = 0, coordinate.CIndex % 2 = 0 with
        | (true, true) -> Quadrant.One
        | (true, false) -> Quadrant.Two
        | (false, true) -> Quadrant.Three
        | (false, false) -> Quadrant.Four
        