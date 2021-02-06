// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.PentaCairo

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.PentaCairo

[<Struct>]
type PentaCairoCell =
    private
        { Walls : Connection<PentaCairoPosition> array }

    interface ICell<PentaCairoPosition> with
        member this.Create walls =
            ({ Walls = walls } :> ICell<PentaCairoPosition>)

        member this.Walls =
            this.WallsArray

        member this.WallIndex position =
            PentaCairoCell.WallIndex position

        member this.WallTypeAtPosition position =
            this.Walls.[PentaCairoCell.WallIndex position].ConnectionType

        member this.IsALink wallType =
            PentaCairoCell.IsALink wallType

        member this.IsLinkedAt position =
            this.ToInterface.IsALink (this.ToInterface.WallTypeAtPosition position)

        member this.AreLinked coordinate otherCoordinate =
            this.ToInterface.IsLinkedAt (PentaCairoCoordinateHandler.Instance.NeighborPositionAt coordinate otherCoordinate)

        member this.IsLinked =
            this.IsLinked

    member this.WallsArray =
        this.Walls

    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> PentaCairoCell.IsALink wall.ConnectionType)).Length > 0

    member this.ToInterface =
        this :> ICell<PentaCairoPosition>

    static member IsALink wallType =
        wallType = Open

    static member WallIndex position =
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
            Walls =
                [| for pos in PentaCairoPositionHandler.Instance.Values coordinate do
                       { ConnectionType = (wallType pos); ConnectionPosition = pos } |]
        }.ToInterface