// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Hex

open Mazes.Core
open Mazes.Core.Grid.Array2D

type HexCoordinateHandler private () =

    static let instance = HexCoordinateHandler()

    interface ICoordinateHandler<HexPosition> with

        member this.NeighborCoordinateAt coordinate position =

            let (rIndexTopLeftRight, rIndexBottomLeftRight) =
                match (HexPositionHandler.IsEven coordinate) with
                | true -> (coordinate.RIndex, coordinate.RIndex + 1)
                | false -> (coordinate.RIndex - 1, coordinate.RIndex)

            match position with
            | HexPosition.TopLeft ->  Some { RIndex = rIndexTopLeftRight; CIndex = coordinate.CIndex - 1 }
            | HexPosition.Top -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | HexPosition.TopRight -> Some { RIndex = rIndexTopLeftRight; CIndex = coordinate.CIndex + 1 }
            | HexPosition.BottomLeft -> Some { RIndex = rIndexBottomLeftRight; CIndex = coordinate.CIndex - 1 }
            | HexPosition.Bottom -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | HexPosition.BottomRight -> Some { RIndex = rIndexBottomLeftRight; CIndex = coordinate.CIndex + 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            HexPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

    member this.ToInterface =
        this :> ICoordinateHandler<HexPosition>

    static member Instance =
        instance.ToInterface