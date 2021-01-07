// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.PentaCairo

open Mazes.Core
open Mazes.Core.Grid.Array2D

type PentaCairoCoordinateHandler private () =

    static let instance = PentaCairoCoordinateHandler()

    interface ICoordinateHandler<PentaCairoPosition> with

        member this.NeighborCoordinateAt coordinate position =
            match PentaCairoPositionHandler.Quadrant coordinate with
            | One ->
                match position with
                | S -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex - 1 }
                | A -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
                | B -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
                | C -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
                | D -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | Two ->
                match position with
                | S -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex - 1 }
                | A -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
                | B -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
                | C -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
                | D -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | Three ->
                match position with
                | S -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex + 1 }
                | A -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
                | B -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
                | C -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
                | D -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | Four ->
                match position with
                | S -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }
                | A -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
                | B -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
                | C -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
                | D -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            PentaCairoPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

    member this.ToInterface =
        this :> ICoordinateHandler<PentaCairoPosition>

    static member Instance =
        instance.ToInterface