// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Tri

open Mazes.Core
open Mazes.Core.Grid.Array2D

type TriPosition =
    | Left
    | Top
    | Right
    | Bottom

type TriPositionHandler private () =

    static let instance = TriPositionHandler()

    interface IPositionHandler<TriPosition> with

        member this.Opposite position =
            match position with
            | Left -> Right
            | Top -> Bottom
            | Right -> Left
            | Bottom -> Top

        member this.Values coordinate =
            let isUpright = TriPositionHandler.IsUpright coordinate
            [|
                Left
                Right
                if not isUpright then
                    Top
                else
                    Bottom
            |]

        member this.Map coordinate position =
            match position with
            | Position.Left -> Left
            | Position.Top -> Top
            | Position.Right -> Right
            | Position.Bottom -> Bottom

    member this.ToInterface =
        this :> IPositionHandler<TriPosition>

    static member Instance =
        instance.ToInterface

    static member IsUpright coordinate =
        (coordinate.RIndex + coordinate.CIndex) % 2 = 0