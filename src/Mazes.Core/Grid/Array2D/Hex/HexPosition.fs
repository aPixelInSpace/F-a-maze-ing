// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Hex

open Mazes.Core
open Mazes.Core.Grid.Array2D

type HexPosition =
    | TopLeft
    | Top
    | TopRight
    | BottomLeft
    | Bottom
    | BottomRight

type HexPositionHandler private () =

    static let instance = HexPositionHandler()

    interface IPositionHandler<HexPosition> with

        member this.Opposite position =
            match position with
            | TopLeft -> BottomRight
            | Top -> Bottom
            | TopRight -> BottomLeft
            | BottomLeft -> TopRight
            | Bottom -> Top
            | BottomRight -> TopLeft

        member this.Values =
            [| TopLeft; Top; TopRight; BottomLeft; Bottom; BottomRight |]

        member this.Map position =
            match position with
            | Position.Top -> Top
            | Position.Left -> TopLeft
            | Position.Bottom -> Bottom
            | Position.Right -> TopRight

    member this.ToInterface =
        this :> IPositionHandler<HexPosition>

    static member Instance =
        instance.ToInterface