// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.OctaSquare

open Mazes.Core
open Mazes.Core.Grid.Array2D

type OctaSquarePosition =
    | Left
    | TopLeft
    | Top
    | TopRight
    | Right
    | BottomLeft
    | Bottom
    | BottomRight

type OctaSquarePositionHandler private () =

    static let instance = OctaSquarePositionHandler()

    interface IPositionHandler<OctaSquarePosition> with

        member this.Opposite position =
            match position with
            | Left -> Right
            | TopLeft -> BottomRight
            | Top -> Bottom
            | TopRight -> BottomLeft
            | Right -> Left
            | BottomLeft -> TopRight
            | Bottom -> Top
            | BottomRight -> TopLeft

        member this.Values coordinate =
            if OctaSquarePositionHandler.IsOctagon coordinate then
                [| Left; TopLeft; Top; TopRight; Right; BottomLeft; Bottom; BottomRight; |]
            else
                [| Left; Top; Right; Bottom; |]

        member this.Map _ position =
            match position with
            | Position.Top -> Top
            | Position.Left -> Left
            | Position.Bottom -> Bottom
            | Position.Right -> Right

    member this.ToInterface =
        this :> IPositionHandler<OctaSquarePosition>

    static member Instance =
        instance.ToInterface

    static member IsOctagon coordinate =
        (coordinate.RIndex + coordinate.CIndex) % 2 = 0

    static member IsSquare coordinate =
        not (OctaSquarePositionHandler.IsOctagon coordinate)