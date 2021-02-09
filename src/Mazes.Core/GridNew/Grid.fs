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
    abstract member RIndexes : int seq
    abstract member CIndexes : int seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AdjustedCoordinate : coordinate:Coordinate -> Coordinate
    abstract member ExistAt : coordinate:Coordinate -> bool
    abstract member AdjustedExistAt : coordinate:Coordinate -> bool
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
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
    /// Returns the coordinate of the neighbor at the position, if there are multiple neighbors then returns the last one
    abstract member Neighbor : Coordinate -> Position -> Coordinate option
    abstract member VirtualNeighbor : Coordinate -> Position -> Coordinate option
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

        member this.RIndexes =
            this.Cells |> Array2D.getRIndexes

        member this.CIndexes =
            this.Cells |> Array2D.getCIndexes

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.AdjustedCoordinate coordinate =
            coordinate

        member this.ExistAt coordinate =
            Array2D.existAt this.Cells coordinate

        member this.AdjustedExistAt coordinate =
            Array2D.existAt this.Cells coordinate

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)

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

        member this.Neighbor coordinate position =
            (this.CoordinateHandler.NeighborCoordinateAt coordinate (this.PositionHandler.Map coordinate position))

        member this.VirtualNeighbor coordinate position =
            this.ToInterface.Neighbor coordinate position

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

type PolarPosition =
    | Inward
    | Outward
    /// Counter-Clockwise
    | Ccw
    /// Clockwise
    | Cw

type GridArrayOfA =
    {
        Canvas : ArrayOfA.Canvas
        Cells : ICell<PolarPosition>[][]
        PositionHandler : IPositionHandler<PolarPosition>
    }

    interface IAdjacentStructure<GridArrayOfA, PolarPosition> with

        member this.TotalOfCells =
            this.Cells.Length

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.RIndexes =
            this.Cells |> ArrayOfA.getRIndexes

        member this.CIndexes =
            this.Cells |> ArrayOfA.getCIndexes

        member this.Dimension1Boundaries cellIndex =
            let maxCellsInLastRing = this.Cells.[ArrayOfA.maxD1Index this.Cells].Length

            let startIndex =
                this.Cells
                |> Array.findIndex(fun ring ->
                    let steps = maxCellsInLastRing / ring.Length
                    cellIndex % steps = 0)

            let length = this.Cells.Length - startIndex

            (startIndex, length)

        member this.Dimension2Boundaries ringIndex =
            (0, this.Cells.[ringIndex].Length)

        member this.AdjustedCoordinate coordinate =
            let maxCellsInLastRing = this.Cells.[ArrayOfA.maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length

            let ratio = maxCellsInLastRing / ringLength

            { coordinate with CIndex = coordinate.CIndex / ratio }

        member this.ExistAt coordinate =
            ArrayOfA.existAt this.Cells coordinate

        member this.AdjustedExistAt coordinate =
            let maxCellsInLastRing = this.Cells.[ArrayOfA.maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length
            let ratio = maxCellsInLastRing / ringLength
            coordinate.CIndex % ratio = 0

        member this.IsLimitAt coordinate otherCoordinate =
            let zone = this.Canvas.Zone coordinate

            let neighborCondition =
                let neighborPosition = this.NeighborPositionAt this.Cells coordinate otherCoordinate
                let neighborCell = this.ToInterface.Cell otherCoordinate

                if neighborPosition <> Inward then
                    not (this.Canvas.ExistAt otherCoordinate) ||
                    not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                    neighborCell.ConnectionTypeAtPosition (this.PositionHandler.Opposite otherCoordinate neighborPosition) = ClosePersistent                
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
                let outwardNeighbors = this.NeighborsCoordinateAt this.Cells coordinate Outward
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

            not (this.ToInterface.IsLimitAt coordinate otherCoordinate) && connectionCondition

        member this.Neighbors coordinate =
            let listOfNeighborCoordinate =
                let neighborsCoordinateAt = this.NeighborsCoordinateAt this.Cells coordinate
                seq {
                    for position in this.PositionHandler.Values coordinate do
                        for coordinate in neighborsCoordinateAt position do
                            yield (coordinate, position)
                }

            this.Canvas.NeighborsPartOfMazeOf listOfNeighborCoordinate
            |> Seq.filter(fun (nCoordinate, _) -> not (this.ToInterface.IsLimitAt coordinate nCoordinate))
            |> Seq.map(fst)

        member this.Neighbor coordinate position =
            let neighbors = this.NeighborsCoordinateAt this.Cells coordinate (this.PositionHandler.Map coordinate position)
            if neighbors |> Seq.isEmpty then
                None
            else
                Some (neighbors |> Seq.last)

        member this.VirtualNeighbor coordinate position =
            Some (this.NeighborBaseCoordinateAt coordinate (this.PositionHandler.Map coordinate position))

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
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell (this.PositionHandler.Opposite otherCoordinate neighborPosition))

                if (this.NeighborsCoordinateAt this.Cells coordinate (this.PositionHandler.Opposite coordinate neighborPosition)) |> Seq.head = otherCoordinate then
                    this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell (this.PositionHandler.Opposite coordinate neighborPosition))
                    this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell neighborPosition)            
            | Inward ->
                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition)
            | Outward ->
                let otherCell = (this.ToInterface.Cell otherCoordinate)
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell (this.PositionHandler.Opposite otherCoordinate neighborPosition))

        member this.GetFirstCellPartOfMaze =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastCellPartOfMaze =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToSpecializedStructure =
            this

    member this.ToInterface =
        this :> IAdjacentStructure<GridArrayOfA, PolarPosition>

    member this.ConnectionTypeAtPosition (cell : ICell<PolarPosition>) position =
        cell.ConnectionTypeAtPosition position

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

    member private this.NeighborBaseCoordinateAt coordinate position =
        match position with
        | Outward ->  { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
        | Cw -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        | Inward -> { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
        | Ccw -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
type IGrid<'Grid> =
    abstract member TotalOfMazeCells : int
    abstract member RIndexes : int seq
    abstract member CIndexes : int seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AdjustedCoordinate : coordinate:Coordinate -> Coordinate
    abstract member ExistAt : coordinate:Coordinate -> bool
    abstract member AdjustedExistAt : coordinate:Coordinate -> bool
    abstract member CoordinatesPartOfMaze : Coordinate seq
    abstract member RandomCoordinatePartOfMazeAndNotConnected : rng : Random -> Coordinate
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Returns all the neighbors adjacent and non adjacent
    abstract member Neighbors : Coordinate -> Coordinate seq
    /// Returns all the adjacent neighbors
    abstract member AdjacentNeighbors : Coordinate -> Coordinate seq
    /// Returns the coordinates of the adjacent neighbors at the position
    abstract member AdjacentNeighbor : Coordinate -> Position -> Coordinate option
    abstract member AdjacentVirtualNeighbor : Coordinate -> Position -> Coordinate option
    /// Given a coordinate, returns true if the cell is part of the maze, false otherwise
    abstract member IsCellPartOfMaze : Coordinate -> bool
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : Coordinate -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Returns the neighbors coordinates that are or not connected NOT NECESSARILY WITH the coordinate
    abstract member ConnectedNeighbors : isConnected:bool -> coordinate:Coordinate -> Coordinate seq
    /// Returns the neighbors coordinates that are or not connected WITH the coordinate
    abstract member ConnectedWithNeighbors : isConnected:bool -> coordinate:Coordinate -> Coordinate seq
    abstract member UpdateConnection : ConnectionType -> Coordinate -> Coordinate -> unit
    abstract member IfNotAtLimitUpdateConnection : ConnectionType -> Coordinate -> Coordinate -> unit
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

        member this.RIndexes =
            this.BaseGrid.RIndexes

        member this.CIndexes =
            this.BaseGrid.CIndexes

        member this.Dimension1Boundaries dimension2Index =
            this.BaseGrid.Dimension1Boundaries dimension2Index

        member this.Dimension2Boundaries dimension1Index =
            this.BaseGrid.Dimension2Boundaries dimension1Index

        member this.AdjustedCoordinate coordinate =
            this.BaseGrid.AdjustedCoordinate coordinate

        member this.ExistAt coordinate =
            this.BaseGrid.ExistAt coordinate

        member this.AdjustedExistAt coordinate =
            this.BaseGrid.AdjustedExistAt coordinate

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

        member this.IsLimitAt coordinate otherCoordinate =
            this.BaseGrid.IsLimitAt coordinate otherCoordinate

        member this.Neighbors coordinate =
            (this.ToInterface.AdjacentNeighbors coordinate)
            |> Seq.append (
                this.NonAdjacentNeighbors.NonAdjacentNeighbors(coordinate)
                |> Seq.map(fst))

        member this.AdjacentNeighbors coordinate =
            this.BaseGrid.Neighbors coordinate

        member this.AdjacentNeighbor coordinate position =
            this.BaseGrid.Neighbor coordinate position

        member this.AdjacentVirtualNeighbor coordinate position =
            this.BaseGrid.VirtualNeighbor coordinate position

        member this.IsCellPartOfMaze coordinate =
            this.BaseGrid.IsCellPartOfMaze coordinate

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
            this.ToInterface.Neighbors coordinate
            |> Seq.filter(fun nCoordinate ->
                if isConnected then
                    (this.BaseGrid.IsCellConnected nCoordinate) = isConnected ||
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isConnected
                else
                    (this.BaseGrid.IsCellConnected nCoordinate) = isConnected &&
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isConnected)
            |> Seq.distinct

        member this.ConnectedWithNeighbors connected coordinate =
            let neighborsCoordinates = this.ToInterface.Neighbors coordinate

            seq {
                for neighborCoordinate in neighborsCoordinates do
                    if connected then
                        if this.NonAdjacentNeighbors.ExistNeighbor coordinate neighborCoordinate then
                            if this.NonAdjacentNeighbors.AreConnected coordinate neighborCoordinate then
                                yield neighborCoordinate
                        elif (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
                    else
                        if this.NonAdjacentNeighbors.ExistNeighbor coordinate neighborCoordinate then
                            if not (this.NonAdjacentNeighbors.AreConnected coordinate neighborCoordinate) then
                                yield neighborCoordinate
                        elif not (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
            }

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
                this.NonAdjacentNeighbors.UpdateConnection connectionType coordinate otherCoordinate
            else
                this.BaseGrid.UpdateConnection connectionType coordinate otherCoordinate

        member this.IfNotAtLimitUpdateConnection connectionType coordinate otherCoordinate =
            if (this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate) || not (this.BaseGrid.IsLimitAt coordinate otherCoordinate) then
                this.ToInterface.UpdateConnection connectionType coordinate otherCoordinate

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