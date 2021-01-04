// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.OctaSquare

open Mazes.Core
open Mazes.Core.Grid.Array2D

type OctaSquareCoordinateHandler private () =

    static let instance = OctaSquareCoordinateHandler()

    interface ICoordinateHandler<OctaSquarePosition> with

        member this.NeighborCoordinateAt coordinate position =
            let isOctagon = OctaSquarePositionHandler.IsOctagon coordinate

            match (position, isOctagon) with
            | (OctaSquarePosition.Left, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.Top, _) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | (OctaSquarePosition.Right, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | (OctaSquarePosition.Bottom, _) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | (OctaSquarePosition.TopLeft, true) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.TopRight, true) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex + 1 }
            | (OctaSquarePosition.BottomLeft, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.BottomRight, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }
            | _ -> None

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            OctaSquarePositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

    member this.ToInterface =
        this :> ICoordinateHandler<OctaSquarePosition>

    static member Instance =
        instance.ToInterface