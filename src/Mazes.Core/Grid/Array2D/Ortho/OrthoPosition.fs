// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open Mazes.Core
open Mazes.Core.Grid.Array2D

type OrthoPosition =
    | Left
    | Top
    | Right
    | Bottom

type OrthoPositionHandler private () =

    static let instance = OrthoPositionHandler()

    interface IPositionHandler<OrthoPosition> with

        member this.Opposite position =
            match position with
            | Left -> Right
            | Top -> Bottom
            | Right -> Left
            | Bottom -> Top

        member this.Values =
            [| Left; Top; Right; Bottom |]

        member this.Map position =
            match position with
            | Position.Left -> Left
            | Position.Top -> Top
            | Position.Right -> Right
            | Position.Bottom -> Bottom

    member this.ToInterface =
        this :> IPositionHandler<OrthoPosition>

    static member Instance =
        instance.ToInterface