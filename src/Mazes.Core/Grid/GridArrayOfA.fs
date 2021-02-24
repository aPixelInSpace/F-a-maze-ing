// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid

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

        member this.CoordinatesPartOfMaze =
            this.ToInterface.Cells
            |> Seq.filter(fun (_, c) -> this.ToInterface.IsCellPartOfMaze c)
            |> Seq.map(snd)

        member this.RandomCoordinatePartOfMazeAndNotConnected (rng : Random) =
            let unconnectedPartOfMazeCells =
                this.ToInterface.CoordinatesPartOfMaze
                |> Seq.filter(fun c ->
                    not (this.ToInterface.IsCellConnected c || this.ToInterface.IsCellConnected c))
                |> Seq.toArray

            unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

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
            this.Canvas.NeighborsPartOfMazeOf (this.ListOfAdjacentNeighborCoordinate coordinate)
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
                let oppositePosition = this.PositionHandler.Opposite coordinate neighborPosition

                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition)

                let otherCell = (this.ToInterface.Cell otherCoordinate)
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell oppositePosition)

                if (this.NeighborsCoordinateAt this.Cells coordinate oppositePosition) |> Seq.head = otherCoordinate then
                    let cell = (this.ToInterface.Cell coordinate)
                    this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell oppositePosition)
                    let otherCell = (this.ToInterface.Cell otherCoordinate)
                    this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell neighborPosition)            
            | Inward ->
                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell neighborPosition)
            | Outward ->
                let otherCell = (this.ToInterface.Cell otherCoordinate)
                this.Cells.[otherCoordinate.RIndex].[otherCoordinate.CIndex] <- otherCell.Create (getNewConnections otherCell (this.PositionHandler.Opposite otherCoordinate neighborPosition))

        member this.IfNotAtLimitUpdateConnection connectionType coordinate otherCoordinate =
            if not (this.ToInterface.IsLimitAt coordinate otherCoordinate) then
                this.ToInterface.UpdateConnection connectionType coordinate otherCoordinate

        member this.WeaveCoordinates coordinates =
            coordinates
            |> Seq.filter(fun c ->
                let toCoordinate = { RIndex = c.RIndex + 2; CIndex = c.CIndex }
                this.ToInterface.ExistAt toCoordinate &&
                this.ToInterface.Dimension2Boundaries c.RIndex = (this.ToInterface.Dimension2Boundaries toCoordinate.RIndex) && 
                c.RIndex % 2 = 0)
            |> Seq.map(fun c -> (c, { RIndex = c.RIndex + 2; CIndex = c.CIndex }))

        member this.OpenCell coordinate =
            let candidate =
                (this.ListOfAdjacentNeighborCoordinate coordinate)
                |> Seq.tryFind(fun (c, _) -> (not (this.ToInterface.ExistAt c)) || (not (this.ToInterface.IsCellPartOfMaze c)))

            match candidate with
            | Some candidate -> this.ToInterface.UpdateConnection Open coordinate (fst candidate)
            | None ->
                if (PolarArrayOfA.isLastRing coordinate.RIndex (PolarArrayOfA.numberOfRings this.Cells)) then
                    let getNewConnections (cell : ICell<'Position>) position =
                        cell.Connections
                        |> Array.map(fun connection ->
                            if connection.ConnectionPosition = position then
                                { ConnectionType = Open; ConnectionPosition = position }
                            else
                                connection)

                    let cell = (this.ToInterface.Cell coordinate)
                    this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- cell.Create (getNewConnections cell Outward)

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

    member private this.ListOfAdjacentNeighborCoordinate coordinate =
        let neighborsCoordinateAt = this.NeighborsCoordinateAt this.Cells coordinate
        seq {
            for position in this.PositionHandler.Values coordinate do
                for coordinate in neighborsCoordinateAt position do
                    yield (coordinate, position)
        }