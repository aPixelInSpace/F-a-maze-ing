// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Ortho

[<Struct>]
type OrthoCell =
    private
        { Walls : Wall<OrthoPosition> array }

    interface ICell<OrthoPosition> with
        member this.Create walls =
            ({ Walls = walls } :> ICell<OrthoPosition>)

        member this.Walls =
            this.WallsArray

        member this.WallIndex position =
            OrthoCell.WallIndex position

        member this.WallTypeAtPosition position =
            this.Walls.[OrthoCell.WallIndex position].WallType

        member this.IsALink wallType =
            OrthoCell.IsALink wallType

        member this.IsLinkedAt position =
            this.ToInterface.IsALink (this.ToInterface.WallTypeAtPosition position)

        member this.AreLinked coordinate otherCoordinate =
            this.ToInterface.IsLinkedAt (OrthoCoordinateHandler.Instance.NeighborPositionAt coordinate otherCoordinate)

        member this.IsLinked =
            this.IsLinked

    member this.WallsArray =
        this.Walls

    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> OrthoCell.IsALink wall.WallType)).Length > 0

    member this.ToInterface =
        this :> ICell<OrthoPosition>

    static member IsALink wallType =
        wallType = Empty

    static member WallIndex position =
        match position with
        | Left -> 0
        | Top -> 1
        | Right -> 2
        | Bottom -> 3

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    WallType.getWallTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let wallType pos =
            match pos with
            | Top -> getWallType (isFirstRow coordinate.RIndex) Top
            | Right -> getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right
            | Bottom -> getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom
            | Left -> getWallType (isFirstColumn coordinate.CIndex) Left

        {
            Walls =
                [| for pos in OrthoPositionHandler.Instance.Values coordinate do
                       { WallType = (wallType pos); WallPosition = pos } |]                
        }.ToInterface