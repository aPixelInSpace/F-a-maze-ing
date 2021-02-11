// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Type.PentaCairo

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid

/// Typical left, top ... doesn't make really sense here (because the form has four rotated forms), so
/// S is the small side of the congruent convex pentagon
/// then clockwise : A, B, C and D
type PentaCairoPosition =
    | S
    | A
    | B
    | C
    | D

type Quadrant =
    /// Has the small side S outward in the upper left
    | One
    /// Has the small side S inward in the bottom left
    | Two
    /// Has the small side S inward in the top right
    | Three
    /// Has the small side S outward in the bottom right
    | Four

type PentaCairoPositionHandler private () =

    static let instance = PentaCairoPositionHandler()

    interface IPositionHandler<PentaCairoPosition> with

        member this.Opposite _ position =
            match position with
            | S -> S
            | A -> B
            | B -> A
            | C -> D
            | D -> C

        member this.Values _ =
            [| S; A; B; C; D; |]

        member this.Map coordinate position =
            match PentaCairoPositionHandler.Quadrant coordinate with
            | One ->
                match position with
                | Position.Top -> A // or S
                | Position.Left -> D
                | Position.Bottom -> C
                | Position.Right -> B
            | Two ->
                match position with
                | Position.Top -> B
                | Position.Left -> A
                | Position.Bottom -> D // or S
                | Position.Right -> C
            | Three ->
                match position with
                | Position.Top -> D // or S
                | Position.Left -> C
                | Position.Bottom -> B
                | Position.Right -> A
            | Four ->
                match position with
                | Position.Top -> C
                | Position.Left -> B
                | Position.Bottom -> A // or S
                | Position.Right -> D

    member this.ToInterface =
        this :> IPositionHandler<PentaCairoPosition>

    static member Instance =
        instance.ToInterface

    /// Four congruent convex pentagons form an hexagon.
    /// This function returns the arbitrarily assigned quadrant number for the pentagon
    /// that divide that hexagon
    static member Quadrant coordinate =
        match coordinate.RIndex % 2 = 0, coordinate.CIndex % 2 = 0 with
        | (true, true) -> Quadrant.One
        | (true, false) -> Quadrant.Two
        | (false, true) -> Quadrant.Three
        | (false, false) -> Quadrant.Four

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

[<Struct>]
type PentaCairoCell =
    private
        { Connections : Connection<PentaCairoPosition> array }

    interface ICell<PentaCairoPosition> with
        member this.Create walls =
            ({ Connections = walls } :> ICell<PentaCairoPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[PentaCairoCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<PentaCairoPosition>

    static member ConnectionPositionIndex position =
        match position with
        | S -> 0
        | A -> 1
        | B -> 2
        | C -> 3
        | D -> 4

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (PentaCairoCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let quadrant = PentaCairoPositionHandler.Quadrant coordinate

        let isFirstRow = isFirstRow coordinate.RIndex
        let isLastRow = isLastRow coordinate.RIndex numberOfRows
        let isFirstColumn = isFirstColumn coordinate.CIndex
        let isLastColumn = isLastColumn coordinate.CIndex numberOfColumns

        let wallType pos =
            match quadrant with
            | One ->
                match pos with
                | S -> getWallType (isFirstRow || isFirstColumn) S
                | A -> getWallType (isFirstRow) A
                | B -> getWallType (isLastColumn) B
                | C -> getWallType (isLastRow) C
                | D -> getWallType (isFirstColumn) D
            | Two ->
                match pos with
                | S -> getWallType (isLastRow || isFirstColumn) S
                | A -> getWallType (isFirstColumn) A
                | B -> getWallType (isFirstRow) B
                | C -> getWallType (isLastColumn) C
                | D -> getWallType (isLastRow) D
            | Three ->
                match pos with
                | S -> getWallType (isFirstRow || isLastColumn) S
                | A -> getWallType (isLastColumn) A
                | B -> getWallType (isLastRow) B
                | C -> getWallType (isFirstColumn) C
                | D -> getWallType (isFirstRow) D
            | Four ->
                match pos with
                | S -> getWallType (isLastRow || isLastColumn) S
                | A -> getWallType (isLastRow) A
                | B -> getWallType (isFirstColumn) B
                | C -> getWallType (isFirstRow) C
                | D -> getWallType (isLastColumn) D

        {
            Connections =
                [| for pos in PentaCairoPositionHandler.Instance.Values coordinate do
                       { ConnectionType = (wallType pos); ConnectionPosition = pos } |]
        }.ToInterface

module Grid =

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            PentaCairoCell.Create
            PentaCairoPositionHandler.Instance
            PentaCairoCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            PentaCairoCell.Create
            PentaCairoPositionHandler.Instance
            PentaCairoCoordinateHandler.Instance
            canvas