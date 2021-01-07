// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

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

        member this.Opposite _ position =
            match position with
            | TopLeft -> BottomRight
            | Top -> Bottom
            | TopRight -> BottomLeft
            | BottomLeft -> TopRight
            | Bottom -> Top
            | BottomRight -> TopLeft

        member this.Values _ =
            [| TopLeft; Top; TopRight; BottomLeft; Bottom; BottomRight |]

        member this.Map coordinate position =
            let cIndexEven = (HexPositionHandler.IsEven coordinate)
            match position with
            | Position.Top -> Top
            | Position.Left -> if cIndexEven then TopLeft else BottomLeft
            | Position.Bottom -> Bottom
            | Position.Right -> if cIndexEven then TopRight else BottomRight

    member this.ToInterface =
        this :> IPositionHandler<HexPosition>

    static member Instance =
        instance.ToInterface

    static member IsEven coordinate =
        coordinate.CIndex % 2 = 0