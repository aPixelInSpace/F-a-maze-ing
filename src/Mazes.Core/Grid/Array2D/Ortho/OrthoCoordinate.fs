// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open Mazes.Core
open Mazes.Core.Grid.Array2D

type OrthoCoordinateHandler private () =

    static let instance = OrthoCoordinateHandler()

    interface ICoordinateHandler<OrthoPosition> with

        member this.NeighborCoordinateAt coordinate position =
            match position with
            | OrthoPosition.Top ->  Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Right -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | OrthoPosition.Bottom -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Left -> Some  { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            OrthoPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

    member this.ToInterface =
        this :> ICoordinateHandler<OrthoPosition>

    static member Instance =
        instance.ToInterface