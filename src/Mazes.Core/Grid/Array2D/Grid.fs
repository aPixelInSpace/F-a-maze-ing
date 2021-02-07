// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid
open Mazes.Core.Array2D

[<AbstractClass>]
type Grid<'Grid, 'Position, 'PH, 'CH
    when 'PH :> IPositionHandler<'Position> and
         'CH :> ICoordinateHandler<'Position>>
         (canvas : Canvas, cells : ICell<'Position>[,], nonAdjacentNeighbors : NonAdjacentNeighbors, obstacles : Obstacles,
          positionHandler : 'PH, coordinateHandler : 'CH) =

    member this.Canvas = canvas
    member this.Cells = cells
    member this.NonAdjacentNeighbors = nonAdjacentNeighbors
    member this.Obstacles = obstacles
    member this.PositionHandler = positionHandler
    member this.CoordinateHandler = coordinateHandler    

    interface IGrid<'Grid> with

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.AddCostForCoordinate cost coordinate =
            this.Obstacles.AddUpdateCost cost coordinate

        member this.CostOfCoordinate coordinate =
            1 + (this.Obstacles.Cost coordinate)

        member this.VirtualAdjacentNeighborCoordinate coordinate position =
            (this.CoordinateHandler.NeighborCoordinateAt coordinate (this.PositionHandler.Map coordinate position))

        member this.IsCellConnected coordinate =
            this.NonAdjacentNeighbors.IsCellConnected coordinate ||
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
            this.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
            |> Seq.map(snd)

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            this.UpdateWallAtCoordinates coordinate otherCoordinate connectionType

        member this.AdjacentNeighbor coordinate position =
            (this.CoordinateHandler.NeighborCoordinateAt coordinate (this.PositionHandler.Map coordinate position))

        member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
            if (this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate) || not (this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) then
                this.ToInterface.UpdateConnection Open coordinate otherCoordinate

        member this.Neighbors coordinate =
            this.Neighbors coordinate

        member this.NeighborsThatAreLinked isLinked coordinate =
            this.Neighbors coordinate
            |> Seq.filter(fun nCoordinate ->
                if isLinked then
                    (this.Cell nCoordinate).IsLinked = isLinked ||
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isLinked
                else
                    (this.Cell nCoordinate).IsLinked = isLinked &&
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isLinked)

        member this.AddUpdateNonAdjacentNeighbor fromCoordinate toCoordinate wallType =
            this.NonAdjacentNeighbors.UpdateConnection wallType fromCoordinate toCoordinate

        member this.LinkedNeighbors coordinate =
            let neighborsCoordinates = this.Neighbors coordinate

            seq {
                for neighborCoordinate in neighborsCoordinates do   
                    if (this.AreLinked coordinate neighborCoordinate) then
                        yield neighborCoordinate
            }

        member this.NotLinkedNeighbors coordinate =
            let neighborsCoordinates = this.Neighbors coordinate

            seq {
                for neighborCoordinate in neighborsCoordinates do   
                    if not (this.AreLinked coordinate neighborCoordinate) then
                        yield neighborCoordinate
            }

        member this.RandomCoordinatePartOfMazeAndNotLinked rng =
            let unlinkedPartOfMazeCells =
                this.Cells
                |> getItemByItem RowsAscendingColumnsAscending
                    (fun cell coordinate ->
                        (this.Canvas.Zone coordinate).IsAPartOfMaze &&
                        not cell.IsLinked
                    ) |> Seq.toArray

            snd (unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)])

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

    member this.IsLimitAt coordinate position =
        let zone = this.Canvas.Zone coordinate
        let cell = this.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = this.CoordinateHandler.NeighborCoordinateAt coordinate position

                match neighborCoordinate with
                | Some neighborCoordinate ->
                    not ((this.Canvas.ExistAt neighborCoordinate) &&
                        (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)
                | None -> true

        not zone.IsAPartOfMaze ||
        cell.WallTypeAtPosition position = ClosePersistent ||
        neighborCondition()

    member private this.UpdateWallAtPosition coordinate position wallType =
        let getWalls coordinate position =
            let cell = this.Cells.[coordinate.RIndex, coordinate.CIndex]
            cell.Walls
            |> Array.mapi(fun index wall ->
                if index = (cell.WallIndex position) then
                    { ConnectionType = wallType; ConnectionPosition = position }
                else
                    wall
                )

        let cell = this.Cells.[coordinate.RIndex, coordinate.CIndex]
        this.Cells.[coordinate.RIndex, coordinate.CIndex] <- cell.Create (getWalls coordinate position)

        let neighbor = this.CoordinateHandler.NeighborCoordinateAt coordinate position
        match neighbor with
        | Some neighbor ->
            let neighborCell = this.Cells.[neighbor.RIndex, neighbor.CIndex]
            this.Cells.[neighbor.RIndex, neighbor.CIndex] <- neighborCell.Create (getWalls neighbor (this.PositionHandler.Opposite coordinate position))
        | None -> ()

    member private this.UpdateWallAtCoordinates (coordinate : Coordinate) otherCoordinate wallType =
        if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
            this.NonAdjacentNeighbors.UpdateConnection wallType coordinate otherCoordinate
        else
            let neighborCoordinateAt = this.CoordinateHandler.NeighborCoordinateAt coordinate

            for position in this.PositionHandler.Values coordinate do
                if Some otherCoordinate = (neighborCoordinateAt position) then
                    this.UpdateWallAtPosition coordinate position wallType

    member this.AdjacentNeighbors coordinate =
        let listOfAdjacentNeighborCoordinate =
            seq {
                for position in this.PositionHandler.Values coordinate do
                    let coordinate = this.CoordinateHandler.NeighborCoordinateAt coordinate position
                    match coordinate with
                    | Some coordinate -> yield (coordinate, position)
                    | None -> ()
            }

        this.Canvas.NeighborsPartOfMazeOf listOfAdjacentNeighborCoordinate
        |> Seq.filter(fun (_, nPosition) -> not (this.IsLimitAt coordinate nPosition))
        |> Seq.map(fst)

    member this.Neighbors coordinate =
        let listOfAdjacentNeighborCoordinate = this.AdjacentNeighbors coordinate

        let listOfNonAdjacentNeighborCoordinate =
            this.NonAdjacentNeighbors.NonAdjacentNeighbors(coordinate)
            |> Seq.map(fst)

        listOfAdjacentNeighborCoordinate
        |> Seq.append listOfNonAdjacentNeighborCoordinate

    member this.AreLinked coordinate otherCoordinate =
        if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
            this.NonAdjacentNeighbors.AreConnected coordinate otherCoordinate
        else
            not (this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) &&        
            (this.Cell coordinate).AreLinked coordinate otherCoordinate

    member this.ToInterface =
        this :> IGrid<'Grid>

    abstract member ToString : string
    abstract member ToSpecializedGrid : 'Grid