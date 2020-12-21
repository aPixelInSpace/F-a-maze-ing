// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar

open Mazes.Core
open Mazes.Core.Grid.Polar.ArrayOfA
open Mazes.Core.Grid.Polar.Canvas

[<Struct>]
type PolarCell =
    { Walls : PolarWall array }

module PolarCell =

    let create canvas (coordinate : Coordinate) isCellPartOfMaze =

        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate
        let neighborsCoordinateAt = PolarCoordinate.neighborsCoordinateAt canvas.Zones coordinate

        let walls = ResizeArray<PolarWall>()

        if not (isFirstRing coordinate.RIndex) then
            let isInwardNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Inward) |> Seq.head)
            walls.Add({ WallType = (WallType.getWallTypeForInternal isCurrentCellPartOfMaze isInwardNeighborPartOfMaze); WallPosition = Inward })

        let isLeftNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Left) |> Seq.head)
        walls.Add({ WallType = (WallType.getWallTypeForInternal isCurrentCellPartOfMaze isLeftNeighborPartOfMaze); WallPosition = Left })

        let isRightNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Right) |> Seq.head)
        walls.Add({ WallType = (WallType.getWallTypeForInternal isCurrentCellPartOfMaze isRightNeighborPartOfMaze); WallPosition = Right })
 
        if isLastRing coordinate.RIndex canvas.NumberOfRings then
            walls.Add({ WallType = (WallType.getWallTypeForEdge isCurrentCellPartOfMaze); WallPosition = Outward })

        {
            Walls = walls.ToArray()
        }