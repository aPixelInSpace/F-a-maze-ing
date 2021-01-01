// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open System
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Teleport
open Mazes.Core.Array2D

[<AbstractClass>]
type Grid<'Grid, 'Position, 'PH, 'CH
    when 'PH :> IPositionHandler<'Position> and
         'CH :> ICoordinateHandler<'Position>>
         (canvas : Canvas, cells : ICell<'Position>[,], teleports : Teleports,
          positionHandler : 'PH, coordinateHandler : 'CH) =

    member this.Canvas = canvas
    member this.Cells = cells
    member this.Teleports = teleports
    member this.PositionHandler = positionHandler
    member this.CoordinateHandler = coordinateHandler    


    interface IGrid<'Grid> with

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.NeighborAbstractCoordinate coordinate position =
            (this.CoordinateHandler.NeighborCoordinateAt coordinate (this.PositionHandler.Map position))

        member this.IsCellLinked coordinate =
            (this.Cell coordinate).IsLinked

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.GetAdjustedExistAt coordinate =
            existAt this.Cells coordinate

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.GetRIndexes =
            this.Cells |> getRIndexes

        member this.GetCIndexes =
            this.Cells |> getCIndexes

        member this.GetAdjustedCoordinate coordinate =
            coordinate

        member this.CoordinatesPartOfMaze =
            this.CoordinatesPartOfMaze

        member this.LinkCells coordinate otherCoordinate =
            this.LinkCells coordinate otherCoordinate

        member this.PutBorderBetweenCells coordinate otherCoordinate =
            this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Border

        member this.Neighbor coordinate position =
            Some (this.Neighbor coordinate (this.PositionHandler.Map position))

        member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
            this.IfNotAtLimitLinkCells coordinate otherCoordinate

        member this.NeighborsThatAreLinked isLinked coordinate =
            this.NeighborsThatAreLinked isLinked coordinate

        member this.AddTwoWayTeleport fromCoordinate toCoordinate =
            this.AddTwoWayTeleport fromCoordinate toCoordinate

        member this.LinkedNeighbors coordinate =
            this.LinkedNeighbors coordinate

        member this.RandomNeighbor rng coordinate =
            this.RandomNeighbor rng coordinate

        member this.RandomCoordinatePartOfMazeAndNotLinked rng =
            this.RandomCoordinatePartOfMazeAndNotLinked rng

        member this.GetFirstPartOfMazeZone =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastPartOfMazeZone =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToString =
            this.ToString

        member this.ToSpecializedGrid =
            this.ToSpecializedGrid
    
    member this.NumberOfRows =
        this.Canvas.NumberOfRows

    member this.NumberOfColumns =
        this.Canvas.NumberOfColumns

    member this.Cell coordinate =
        get this.Cells coordinate

    member this.GetCellByCell extractBy filter =
        getItemByItem this.Cells extractBy filter

    member this.IsLimitAt coordinate position =
        let zone = this.Canvas.Zone coordinate
        let cell = this.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = this.CoordinateHandler.NeighborCoordinateAt coordinate position

                not ((this.Canvas.ExistAt neighborCoordinate) &&
                    (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = Border ||
        neighborCondition()

    member private this.UpdateWallAtPosition coordinate position wallType =
        let getWalls coordinate position =
            let cell = this.Cells.[coordinate.RIndex, coordinate.CIndex]
            cell.Walls
            |> Array.mapi(fun index wall ->
                if index = (cell.WallIndex position) then
                    { WallType = wallType; WallPosition = position }
                else
                    wall
                )

        let cell = this.Cells.[coordinate.RIndex, coordinate.CIndex]
        this.Cells.[coordinate.RIndex, coordinate.CIndex] <- cell.Create (getWalls coordinate position)

        let neighbor = this.CoordinateHandler.NeighborCoordinateAt coordinate position
        let neighborCell = this.Cells.[neighbor.RIndex, neighbor.CIndex]
        this.Cells.[neighbor.RIndex, neighbor.CIndex] <- neighborCell.Create (getWalls neighbor (this.PositionHandler.Opposite position))

    member this.LinkCells coordinate otherCoordinate =
        this.UpdateWallAtCoordinates coordinate otherCoordinate WallType.Empty

    member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if not (this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) then
            this.LinkCells coordinate otherCoordinate

    member private this.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        let neighborCoordinateAt = this.CoordinateHandler.NeighborCoordinateAt coordinate

        for position in this.PositionHandler.Values do
            if otherCoordinate = (neighborCoordinateAt position) then
                this.UpdateWallAtPosition coordinate position wallType

    member this.Neighbor coordinate position =
        this.CoordinateHandler.NeighborCoordinateAt coordinate position

    /// Returns the neighbors that are inside the bound of the grid
    member this.Neighbors coordinate =
        let listOfNeighborCoordinate =
            seq {
                for position in this.PositionHandler.Values do
                    yield ((this.CoordinateHandler.NeighborCoordinateAt coordinate position), position)
            }

        this.Canvas.NeighborsPartOfMazeOf listOfNeighborCoordinate
            |> Seq.filter(fun (_, nPosition) -> not (this.IsLimitAt coordinate nPosition))
            |> Seq.map(fst)

    member this.RandomNeighbor (rng : Random) coordinate =
        let neighbors = this.Neighbors coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member this.NeighborsThatAreLinked isLinked coordinate =
        let neighbors = this.Neighbors coordinate
        neighbors |> Seq.filter(fun nCoordinate -> (this.Cell nCoordinate).IsLinked = isLinked)

    member this.AddTwoWayTeleport fromCoordinate toCoordinate =
        this.Teleports.AddTwoWayTeleport fromCoordinate toCoordinate

    member this.LinkedNeighbors coordinate =
        let isLinkedAt otherCoordinate =
            not (this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) &&        
            (this.Cell coordinate).AreLinked coordinate otherCoordinate

        let neighborsCoordinates = this.Neighbors coordinate

        seq {
            for neighborCoordinate in neighborsCoordinates do   
                if (isLinkedAt neighborCoordinate) then
                    yield neighborCoordinate

            for teleport in this.Teleports.Teleports coordinate do
                yield teleport
        }

    member this.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            this.GetCellByCell RowsAscendingColumnsAscending
                (fun cell coordinate ->
                    (this.Canvas.Zone coordinate).IsAPartOfMaze &&
                    not cell.IsLinked
                ) |> Seq.toArray

        snd (unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)])

    member this.CoordinatesPartOfMaze =
        this.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(snd)

    abstract member ToString : string
    abstract member ToSpecializedGrid : 'Grid