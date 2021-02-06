// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.ArrayOfA.Polar

open Mazes.Core
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid
open Mazes.Core.Grid.ArrayOfA.Polar.PolarArrayOfA

[<Struct>]
type PolarCell =
    {
        Walls : Connection<PolarPosition> array
    }

    member this.WallTypeAtPosition position =
        (this.Walls |> Array.find(fun w -> w.ConnectionPosition = position)).ConnectionType

    static member IsALink wallType =
        wallType = Open

    /// Returns true if the cell has at least one link
    member this.IsLinked (cells : PolarCell[][]) coordinate =
        let wallCondition =
            (this.Walls
            |> Array.where(fun wall -> PolarCell.IsALink wall.ConnectionType)).Length > 0

        let outwardCondition =
            let outwardNeighbors = PolarCoordinate.neighborsCoordinateAt cells coordinate Outward
            if not (outwardNeighbors |> Seq.isEmpty) then
                outwardNeighbors
                |> Seq.filter(fun n ->
                    cells.[n.RIndex].[n.CIndex].Walls
                    |> Array.where(fun w -> w.ConnectionPosition = Inward && PolarCell.IsALink w.ConnectionType)
                    |> Array.length > 0)
                |> Seq.length > 0
            else
                false

        wallCondition || outwardCondition

    member this.IsLinkedAt pos =
        PolarCell.IsALink (this.WallTypeAtPosition pos)

    member this.AreLinked (cells : PolarCell[][]) (coordinate : Coordinate) (otherCoordinate : Coordinate) =
        let neighborPosition = PolarCoordinate.neighborPositionAt cells coordinate otherCoordinate
        if neighborPosition <> Outward then
            this.IsLinkedAt neighborPosition
        else
            let neighborCell = getCell cells otherCoordinate
            neighborCell.IsLinkedAt Inward

module PolarCell =

    let create canvas internalWallType (coordinate : Coordinate) isCellPartOfMaze =

        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate
        let neighborsCoordinateAt = PolarCoordinate.neighborsCoordinateAt canvas.Zones coordinate

        let walls = ResizeArray<Connection<PolarPosition>>()

        if not (isFirstRing coordinate.RIndex) then
            let isInwardNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Inward) |> Seq.head)
            walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isInwardNeighborPartOfMaze); ConnectionPosition = Inward })

        let isCcwNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Ccw) |> Seq.head)
        walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isCcwNeighborPartOfMaze); ConnectionPosition = Ccw })

        let isCwNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Cw) |> Seq.head)
        walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isCwNeighborPartOfMaze); ConnectionPosition = Cw })
 
        if isLastRing coordinate.RIndex canvas.NumberOfRings then
            walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze); ConnectionPosition = Outward })

        {
            Walls = walls.ToArray()
        }