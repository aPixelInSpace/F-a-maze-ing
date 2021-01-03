// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Tri

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Tri

[<Struct>]
type TriCell =
    private
        { Walls : Wall<TriPosition> array }

    interface ICell<TriPosition> with
        member this.Create walls =
            ({ Walls = walls } :> ICell<TriPosition>)

        member this.Walls =
            this.WallsArray

        member this.WallIndex position =
            TriCell.WallIndex position

        member this.WallTypeAtPosition position =
            this.Walls.[TriCell.WallIndex position].WallType

        member this.IsALink wallType =
            TriCell.IsALink wallType

        member this.IsLinkedAt position =
            this.ToInterface.IsALink (this.ToInterface.WallTypeAtPosition position)

        member this.AreLinked coordinate otherCoordinate =
            this.ToInterface.IsLinkedAt (TriCoordinateHandler.Instance.NeighborPositionAt coordinate otherCoordinate)

        member this.IsLinked =
            this.IsLinked

    member this.WallsArray =
        this.Walls

    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> TriCell.IsALink wall.WallType)).Length > 0

    member this.ToInterface =
        this :> ICell<TriPosition>

    static member IsALink wallType =
        wallType = Empty

    static member WallIndex position =
        match position with
        | Left -> 0
        | Right -> 1
        | Top -> 2
        | Bottom -> 2

    static member Create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                match (TriCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    WallType.getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        {
            Walls =
                [|
                   { WallType = (getWallType (isFirstColumn coordinate.CIndex) Left); WallPosition = Left }
                   { WallType = (getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right); WallPosition = Right }

                   if TriPositionHandler.IsUpright coordinate then
                       { WallType = (getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom); WallPosition = Bottom }
                   else
                       { WallType = (getWallType (isFirstRow coordinate.RIndex) Top); WallPosition = Top } |]                
        }.ToInterface