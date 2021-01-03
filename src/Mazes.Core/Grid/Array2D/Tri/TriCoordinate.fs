// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Tri

open Mazes.Core
open Mazes.Core.Grid.Array2D

type TriCoordinateHandler private () =

    static let instance = TriCoordinateHandler()

    interface ICoordinateHandler<TriPosition> with

        member this.NeighborCoordinateAt coordinate position =
            let isUpright = TriPositionHandler.IsUpright coordinate
            match (position, isUpright) with
            | (TriPosition.Top, false) ->  Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | (TriPosition.Right, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | (TriPosition.Bottom, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | (TriPosition.Left, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | _ -> None

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            TriPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

    member this.ToInterface =
        this :> ICoordinateHandler<TriPosition>

    static member Instance =
        instance.ToInterface