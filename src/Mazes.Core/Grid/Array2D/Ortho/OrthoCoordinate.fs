// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open Mazes.Core
open Mazes.Core.Grid.Array2D

type OrthoCoordinateHandler private () =

    static let instance = OrthoCoordinateHandler()

    interface ICoordinateHandler<OrthoPosition> with

        member this.NeighborCoordinateAt coordinate position =
            match position with
            | OrthoPosition.Top ->  { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Right -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | OrthoPosition.Bottom -> { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Left -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate
            match otherCoordinate with
            | c when c = neighborCoordinateAt OrthoPosition.Top -> OrthoPosition.Top
            | c when c = neighborCoordinateAt OrthoPosition.Right -> OrthoPosition.Right
            | c when c = neighborCoordinateAt OrthoPosition.Bottom -> OrthoPosition.Bottom
            | c when c = neighborCoordinateAt OrthoPosition.Left -> OrthoPosition.Left
            | _ -> failwith "Unable to match the ortho coordinates with a position"

    member this.ToInterface =
        this :> ICoordinateHandler<OrthoPosition>

    static member Instance =
        instance.ToInterface