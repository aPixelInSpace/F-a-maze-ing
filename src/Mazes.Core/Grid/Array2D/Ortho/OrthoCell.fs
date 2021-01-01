// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Ortho

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Ortho

[<Struct>]
type OrthoCell =

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
        | Top -> 0
        | Right -> 1
        | Bottom -> 2
        | Left -> 3

    static member Create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isNeighborPartOfMaze = isCellPartOfMaze (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate position)
                WallType.getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze

        let wallTypeTop = getWallType (isFirstRow coordinate.RIndex) Top

        let wallTypeRight = getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right

        let wallTypeBottom = getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom

        let wallTypeLeft = getWallType (isFirstColumn coordinate.CIndex) Left                

        {
            Walls =
                [| { WallType = wallTypeTop; WallPosition = Top }
                   { WallType = wallTypeRight; WallPosition = Right }
                   { WallType = wallTypeBottom; WallPosition = Bottom }
                   { WallType = wallTypeLeft; WallPosition = Left } |]                
        }.ToInterface