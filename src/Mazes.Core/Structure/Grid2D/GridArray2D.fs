// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Structure

type GridArray2D<'Position when 'Position : equality> =
    {
        Canvas : Canvas
        Cells : ICell<'Position>[,]
        PositionHandler : IPositionHandler<'Position>
        CoordinateHandler : ICoordinateHandlerArray2D<'Position>
    }
    interface IAdjacentStructure<GridArray2D<'Position>, 'Position> with

        member this.TotalOfCells =
            this.Cells.Length

        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.RIndexes =
            this.Cells |> getRIndexes

        member this.CIndexes =
            this.Cells |> getCIndexes

        member this.Dimension1Boundaries _ =
            (0, this.Canvas.NumberOfRows)

        member this.Dimension2Boundaries _ =
            (0, this.Canvas.NumberOfColumns)

        member this.AdjustedCoordinate coordinate =
            coordinate

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.AdjustedExistAt coordinate =
            existAt this.Cells coordinate

        member this.CoordinatesPartOfMaze =
            this.ToInterface.Cells
            |> Seq.filter(fun (_, c) -> this.ToInterface.IsCellPartOfMaze c)
            |> Seq.map(snd)

        member this.RandomCoordinatePartOfMazeAndNotConnected (rng : Random) =
            let unconnectedPartOfMazeCells =
                this.ToInterface.CoordinatesPartOfMaze
                |> Seq.filter(fun c ->
                    not (this.ToInterface.IsCellConnected c))
                |> Seq.toArray

            unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate (this.CoordinateHandler.NeighborPositionAt coordinate otherCoordinate)

        member this.Cell coordinate =
            get this.Cells coordinate

        member this.Cells =
            this.Cells |> getItemByItem RowsAscendingColumnsAscending (fun _ _ -> true)

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
            this.Canvas.NeighborsPartOfMazeOf (this.ListOfAdjacentNeighborCoordinate coordinate)
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

        member this.IfNotAtLimitUpdateConnection connectionType coordinate otherCoordinate =
            if not (this.ToInterface.IsLimitAt coordinate otherCoordinate) then
                this.ToInterface.UpdateConnection connectionType coordinate otherCoordinate

        member this.WeaveCoordinates coordinates =
            this.CoordinateHandler.WeaveCoordinates coordinates

        member this.OpenCell coordinate =
            let candidatePosition =
                (this.ListOfAdjacentNeighborCoordinate coordinate)
                |> Seq.tryFind(fun (c, _) -> (not (this.ToInterface.ExistAt c)) || (not (this.ToInterface.IsCellPartOfMaze c)))

            match candidatePosition with
            | Some (_, position) ->
                let getNewConnections (cell : ICell<'Position>) position =
                    cell.Connections
                    |> Array.map(fun connection ->
                        if connection.ConnectionPosition = position then
                            { ConnectionType = Open; ConnectionPosition = position }
                        else
                            connection)

                let cell = (this.ToInterface.Cell coordinate)
                this.Cells.[coordinate.RIndex, coordinate.CIndex] <- cell.Create (getNewConnections cell position)
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

    member private this.ListOfAdjacentNeighborCoordinate coordinate =
        seq {
            for position in this.PositionHandler.Values coordinate do
                let coordinate = this.CoordinateHandler.NeighborCoordinateAt coordinate position
                match coordinate with
                | Some coordinate -> yield (coordinate, position)
                | None -> ()
            }

module GridArray2D =

    let private createInternal cellCreation positionHandler coordinateHandler internalConnectionType (canvas : Canvas.Array2D.Canvas) =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                cellCreation
                    canvas.NumberOfRows
                    canvas.NumberOfColumns
                    internalConnectionType
                    { RIndex = rowIndex; CIndex = columnIndex }
                    canvas.IsZonePartOfMaze)

        {
          Canvas = canvas
          Cells = cells
          PositionHandler = positionHandler
          CoordinateHandler = coordinateHandler
        }

    let createBaseGrid<'Position when 'Position : equality> cellCreation positionHandler coordinateHandler canvas =
        createInternal cellCreation positionHandler coordinateHandler Close canvas :> IAdjacentStructure<GridArray2D<'Position>, 'Position>

    let createEmptyBaseGrid<'Position when 'Position : equality> cellCreation positionHandler coordinateHandler canvas =
        createInternal cellCreation positionHandler coordinateHandler Open canvas :> IAdjacentStructure<GridArray2D<'Position>, 'Position>