// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.GridNew

open System
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid.ArrayOfA.Polar

type ICell<'Position> =
    abstract member Create : Connection<'Position> array -> ICell<'Position>
    abstract member Connections : Connection<'Position> array
    abstract member ConnectionTypeAtPosition : 'Position -> ConnectionType

type IAdjacentStructure<'Structure, 'Position> =
    abstract member TotalOfCells : int
    abstract member TotalOfMazeCells : int
    abstract member Cell : coordinate:Coordinate -> ICell<'Position>
    /// Returns every cells in the structure
    abstract member Cells : (ICell<'Position> * Coordinate) seq
    /// Given a coordinate, returns true if the cell is marked as part of the maze, false otherwise
    abstract member IsCellPartOfMaze : coordinate:Coordinate -> bool
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : coordinate:Coordinate -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Given a coordinate, returns the coordinates of the neighbors
    abstract member Neighbors : coordinate:Coordinate -> Coordinate seq
    /// Given two coordinates, updates the connection between them
    abstract member UpdateConnection : connectionType:ConnectionType -> coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstCellPartOfMaze : Coordinate
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastCellPartOfMaze : Coordinate
    abstract member ToSpecializedStructure : 'Structure

type GridArray2D<'Position when 'Position : equality> =
    {
        Canvas : Array2D.Canvas
        Cells : ICell<'Position>[,]
        PositionHandler : IPositionHandler<'Position>
        CoordinateHandler : ICoordinateHandler<'Position>
    }
    interface IAdjacentStructure<GridArray2D<'Position>, 'Position> with

        member this.TotalOfCells =
            this.Cells.Length

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Cell coordinate =
            Array2D.get this.Cells coordinate

        member this.Cells =
            this.Cells |> Array2D.getItemByItem Array2D.RowsAscendingColumnsAscending (fun _ _ -> true)

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.IsCellConnected coordinate =
            (this.ToInterface.Cell coordinate).Connections
            |> Array.filter(fun c -> c.ConnectionType = Open)
            |> Array.length > 0

        member this.AreConnected coordinate otherCoordinate =
            not (this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) &&        
            ((this.ToInterface.Cell coordinate).ConnectionTypeAtPosition (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)) = Open

        member this.Neighbors coordinate =
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

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            let getNewConnections (cell : ICell<'Position>) position =
                cell.Connections
                |> Array.map(fun connection ->
                    if connection.ConnectionPosition = position then
                        { ConnectionType = connectionType; ConnectionPosition = position }
                    else
                        connection)

            for position in this.PositionHandler.Values coordinate do
                if Some otherCoordinate = (this.CoordinateHandler.NeighborCoordinateAt coordinate position) then
                    let cell = this.ToInterface.Cell coordinate
                    this.Cells.[coordinate.RIndex, coordinate.CIndex] <- cell.Create (getNewConnections cell position)

                    let neighbor = this.CoordinateHandler.NeighborCoordinateAt coordinate position
                    match neighbor with
                    | Some neighbor ->
                        let neighborCell = this.ToInterface.Cell neighbor
                        this.Cells.[neighbor.RIndex, neighbor.CIndex] <- neighborCell.Create (getNewConnections neighborCell (this.PositionHandler.Opposite coordinate position))
                    | None -> ()

        member this.GetFirstCellPartOfMaze =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastCellPartOfMaze =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToSpecializedStructure =
            this

    member this.ToInterface =
        this :> IAdjacentStructure<GridArray2D<'Position>, 'Position>

    member this.NumberOfRows =
        this.Canvas.NumberOfRows

    member this.NumberOfColumns =
        this.Canvas.NumberOfColumns

    member this.ConnectionTypeAtPosition (cell : ICell<'Position>) position =
        cell.ConnectionTypeAtPosition position

    member private this.IsLimitAt coordinate position =
        let zone = this.Canvas.Zone coordinate
        let cell = this.ToInterface.Cell coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = this.CoordinateHandler.NeighborCoordinateAt coordinate position

                match neighborCoordinate with
                | Some neighborCoordinate ->
                    not ((this.Canvas.ExistAt neighborCoordinate) &&
                        (this.Canvas.Zone neighborCoordinate).IsAPartOfMaze)
                | None -> true

        not zone.IsAPartOfMaze ||
        cell.ConnectionTypeAtPosition position = ClosePersistent ||
        neighborCondition()

type GridArrayOfA =
    {
        Canvas : ArrayOfA.Canvas
        Cells : ICell<PolarPosition>[][]
    }

    interface IAdjacentStructure<GridArrayOfA, PolarPosition> with

        member this.TotalOfCells =
            this.Cells.Length

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.Cell coordinate =
            ArrayOfA.get this.Cells coordinate

        member this.Cells =
            this.Cells |> ArrayOfA.getItemByItem (fun _ _ -> true)

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.IsCellConnected coordinate =
            let localCondition =
                (this.ToInterface.Cell coordinate).Connections
                |> Array.filter(fun c -> c.ConnectionType = Open)
                |> Array.length > 0

            let outwardCondition =
                let outwardNeighbors = PolarCoordinate.neighborsCoordinateAt this.Cells coordinate Outward
                if not (outwardNeighbors |> Seq.isEmpty) then
                    outwardNeighbors
                    |> Seq.filter(fun n ->
                        (this.ToInterface.Cell n).Connections
                        |> Array.where(fun c -> c.ConnectionPosition = Inward && c.ConnectionType = Open)
                        |> Array.length > 0)
                    |> Seq.length > 0
                else
                    false

            localCondition || outwardCondition

        member this.AreConnected coordinate otherCoordinate =
            let neighborPosition = this.NeighborPositionAt this.Cells coordinate otherCoordinate
            let connectionCondition =
                if neighborPosition <> Outward then
                    ((this.ToInterface.Cell coordinate).ConnectionTypeAtPosition neighborPosition) = Open        
                else
                    ((this.ToInterface.Cell otherCoordinate).ConnectionTypeAtPosition Inward) = Open

            not (this.IsLimitAt coordinate otherCoordinate) && connectionCondition

        member this.Neighbors coordinate =
            let listOfNeighborCoordinate =
                let neighborsCoordinateAt = this.NeighborsCoordinateAt this.Cells coordinate
                seq {
                    for position in PolarPosition.values do
                        for coordinate in neighborsCoordinateAt position do
                            yield (coordinate, position)
                }

            this.Canvas.NeighborsPartOfMazeOf listOfNeighborCoordinate
            |> Seq.filter(fun (nCoordinate, _) -> not (this.IsLimitAt coordinate nCoordinate))
            |> Seq.map(fst)

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            let getNewConnections (cell : ICell<'Position>) position =
                cell.Connections
                |> Array.map(fun connection ->
                    if connection.ConnectionPosition = position then
                        { ConnectionType = connectionType; ConnectionPosition = position }
                    else
                        connection)

            let neighborPosition = this.NeighborPositionAt this.Cells coordinate otherCoordinate
            match neighborPosition with
            | Ccw | Cw ->
                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition)

                let otherCell = (this.ToInterface.Cell otherCoordinate)
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell neighborPosition.Opposite)

                if (this.NeighborsCoordinateAt this.Cells coordinate neighborPosition.Opposite) |> Seq.head = otherCoordinate then
                    this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition.Opposite)
                    this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell neighborPosition)            
            | Inward ->
                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition)
            | Outward ->
                let otherCell = (this.ToInterface.Cell otherCoordinate)
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell neighborPosition.Opposite)

        member this.GetFirstCellPartOfMaze =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastCellPartOfMaze =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToSpecializedStructure =
            this
    member this.ToInterface =
        this :> IAdjacentStructure<GridArrayOfA, PolarPosition>

    member this.IsLimitAt coordinate otherCoordinate =
        let zone = this.Canvas.Zone coordinate

        let neighborCondition =
            let neighborPosition = this.NeighborPositionAt this.Cells coordinate otherCoordinate
            let neighborCell = this.ToInterface.Cell otherCoordinate

            if neighborPosition <> Inward then
                not (this.Canvas.ExistAt otherCoordinate) ||
                not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                neighborCell.ConnectionTypeAtPosition neighborPosition.Opposite = ClosePersistent                
            else
                let cell = this.ToInterface.Cell coordinate
                if not (PolarArrayOfA.isFirstRing coordinate.RIndex) then
                    not (this.Canvas.ExistAt otherCoordinate) ||
                    not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                    cell.ConnectionTypeAtPosition neighborPosition = ClosePersistent
                else
                    true

        (not zone.IsAPartOfMaze) ||
        neighborCondition

    member private this.NeighborsCoordinateAt (arrayOfA : 'A[][]) coordinate position =
        seq {
            match position with
            | Inward ->
                if not (PolarArrayOfA.isFirstRing coordinate.RIndex) then
                    let inwardRingNumberOfCells = PolarArrayOfA.getNumberOfCellsAt arrayOfA (coordinate.RIndex - 1)
                    let currentRingNumberOfCells = PolarArrayOfA.getNumberOfCellsAt arrayOfA coordinate.RIndex
                    let ratio = currentRingNumberOfCells / inwardRingNumberOfCells
                    yield { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex / ratio }
            | Outward ->
                if not (PolarArrayOfA.isLastRing coordinate.RIndex (PolarArrayOfA.numberOfRings arrayOfA)) then
                    let currentRingNumberOfCells = PolarArrayOfA.getNumberOfCellsAt arrayOfA coordinate.RIndex
                    let outwardRingNumberOfCells = PolarArrayOfA.getNumberOfCellsAt arrayOfA (coordinate.RIndex + 1)
                    let ratio = outwardRingNumberOfCells / currentRingNumberOfCells
                    for ratioIndex in 0 .. ratio - 1 do
                        yield { RIndex = coordinate.RIndex + 1; CIndex = (coordinate.CIndex * ratio) + ratioIndex }
            | Ccw ->
                if PolarArrayOfA.isFirstCellOfRing coordinate.CIndex then
                    yield { RIndex = coordinate.RIndex; CIndex = PolarArrayOfA.maxCellsIndex arrayOfA coordinate.RIndex }
                else
                    yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | Cw ->
                if PolarArrayOfA.isLastCellOfRing arrayOfA coordinate.RIndex coordinate.CIndex then
                    yield { RIndex = coordinate.RIndex; CIndex = PolarArrayOfA.minCellIndex }
                else
                    yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        }

    member private this.NeighborPositionAt (arrayOfA : ICell<PolarPosition>[][]) coordinate otherCoordinate =
        let neighborCoordinateAt = this.NeighborsCoordinateAt arrayOfA coordinate
        match otherCoordinate with
        | c when c = (neighborCoordinateAt Ccw |> Seq.head) -> Ccw
        | c when c = (neighborCoordinateAt Cw |> Seq.head) -> Cw
        | c when (neighborCoordinateAt Outward |> Seq.tryFind(fun n -> c = n)).IsSome -> Outward
        | c when (neighborCoordinateAt Inward |> Seq.tryFind(fun n -> c = n)).IsSome -> Inward
        | _ -> failwith "Unable to match the polar coordinates with a position"

type IGrid<'Grid> =
    abstract member TotalOfMazeCells : int
    abstract member CoordinatesPartOfMaze : Coordinate seq
    abstract member RandomCoordinatePartOfMazeAndNotConnected : rng : Random -> Coordinate
    abstract member Neighbors : Coordinate -> Coordinate seq
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : Coordinate -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Returns the neighbors coordinates that are connected or not WITH the coordinate
    abstract member ConnectedNeighbors : isConnected:bool -> coordinate:Coordinate -> Coordinate seq
    abstract member UpdateConnection : ConnectionType -> Coordinate -> Coordinate -> unit
    abstract member CostOfCoordinate : coordinate:Coordinate -> Cost
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstCellPartOfMaze : Coordinate
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastCellPartOfMaze : Coordinate
    abstract member ToSpecializedGrid : 'Grid

type Grid<'Grid, 'Position> =
    {
        BaseGrid : IAdjacentStructure<'Grid, 'Position>
        NonAdjacentNeighbors : NonAdjacentNeighbors
        Obstacles : Obstacles
    }

    interface IGrid<Grid<'Grid, 'Position>> with

        member this.TotalOfMazeCells =
            this.BaseGrid.TotalOfMazeCells

        member this.CoordinatesPartOfMaze =
            this.BaseGrid.Cells
            |> Seq.filter(fun (_, coordinate) -> this.BaseGrid.IsCellPartOfMaze coordinate)
            |> Seq.map(snd)

        member this.RandomCoordinatePartOfMazeAndNotConnected (rng : Random) =
            let unconnectedPartOfMazeCells =
                this.BaseGrid.Cells
                |> Seq.filter(fun (_, c) ->
                    this.BaseGrid.IsCellPartOfMaze c &&
                    not (this.BaseGrid.IsCellConnected c || this.NonAdjacentNeighbors.IsCellConnected c))
                |> Seq.toArray

            snd (unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)])

        member this.Neighbors coordinate =
            let listOfAdjacentNeighborCoordinate = this.BaseGrid.Neighbors coordinate

            let listOfNonAdjacentNeighborCoordinate =
                this.NonAdjacentNeighbors.NonAdjacentNeighbors(coordinate)
                |> Seq.map(fst)

            listOfAdjacentNeighborCoordinate
            |> Seq.append listOfNonAdjacentNeighborCoordinate

        member this.IsCellConnected coordinate =
            this.NonAdjacentNeighbors.IsCellConnected coordinate ||
            this.BaseGrid.IsCellConnected coordinate

        member this.AreConnected coordinate otherCoordinate =
            let nonAdjacentCondition =
                if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
                    this.NonAdjacentNeighbors.AreConnected coordinate otherCoordinate
                else
                    false
            
            let adjacentCondition =
                this.BaseGrid.AreConnected coordinate otherCoordinate

            nonAdjacentCondition || adjacentCondition

        member this.ConnectedNeighbors isConnected coordinate =
            let neighborsCoordinates = this.ToInterface.Neighbors coordinate

            seq {
                for neighborCoordinate in neighborsCoordinates do
                    if isConnected then
                        if (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
                    else
                        if not (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
            }

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
                this.NonAdjacentNeighbors.UpdateConnection connectionType coordinate otherCoordinate
            else
                this.BaseGrid.UpdateConnection connectionType coordinate otherCoordinate

        member this.CostOfCoordinate coordinate =
            1 + (this.Obstacles.Cost coordinate)

        member this.GetFirstCellPartOfMaze =
            this.BaseGrid.GetFirstCellPartOfMaze

        member this.GetLastCellPartOfMaze =
            this.BaseGrid.GetLastCellPartOfMaze

        member this.ToSpecializedGrid =
            this

    member this.ToInterface =
        this :> IGrid<Grid<'Grid, 'Position>>

module Grid =
    
    let create (baseGrid : IAdjacentStructure<_, _>) =
        {
            BaseGrid = baseGrid
            NonAdjacentNeighbors = NonAdjacentNeighbors.CreateEmpty
            Obstacles = Obstacles.CreateEmpty
        } :> IGrid<_>