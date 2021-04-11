// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArray2D =
    private
        {
            Canvas : Canvas.CanvasArray2D
            Disposition : DispositionArray2D
            Cells : CellArray2D[,]
        }

module GridArray2D =

    let totalOfCells g =
        g.Cells.Length

    let totalOfMazeCells g =
        Canvas.CanvasArray2D.totalOfMazeZones g.Canvas

    let rIndexes g =
        g.Cells |> Array2D.getRIndexes

    let cIndexes g =
        g.Cells |> Array2D.getCIndexes

    let dimension1Boundaries g =
        (0, Canvas.CanvasArray2D.numberOfRows g.Canvas)

    let dimension2Boundaries g =
        (0, Canvas.CanvasArray2D.numberOfColumns g.Canvas)

    let existAt g coordinate =
        Array2D.existAt g.Cells coordinate

    let adjustedExistAt g coordinate =
        Array2D.existAt g.Cells coordinate

    let cell g coordinate =
        Array2D.get g.Cells coordinate

    let cellsSeq g =
        g.Cells |> Array2D.getItemByItem Array2D.RowsAscendingColumnsAscending (fun _ _ -> true)

    let isCellPartOfMaze g coordinate =
        Canvas.CanvasArray2D.isZonePartOfMaze g.Canvas coordinate

    let isCellConnected g coordinate =
        CellArray2D.connectionsStates (cell g coordinate)
        |> Array.filter(fun c -> c = Open)
        |> Array.length > 0

    let coordinatesPartOfMaze g =
        cellsSeq g
        |> Seq.filter(fun (_, c) -> isCellPartOfMaze g c)
        |> Seq.map(snd)

    let randomCoordinatePartOfMazeAndNotConnected g (rng : Random) =
        let unconnectedPartOfMazeCells =
            coordinatesPartOfMaze g
            |> Seq.filter(fun c -> not (isCellConnected g c))
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

    let isLimitAtPosition g coordinate position =
        let zone = Canvas.CanvasArray2D.zone g.Canvas coordinate
        let cell = cell g coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = CellArray2D.neighborCoordinateAt cell coordinate position

                match neighborCoordinate with
                | Some neighborCoordinate ->
                    not ((Canvas.CanvasArray2D.existAt g.Canvas neighborCoordinate) &&
                        (Canvas.CanvasArray2D.zone g.Canvas neighborCoordinate).IsAPartOfMaze)
                | None -> true

        not zone.IsAPartOfMaze ||
        (CellArray2D.connectionStateAtPosition cell position) = ClosePersistent ||
        neighborCondition()

    let isLimitAtCoordinate g coordinate otherCoordinate =
        isLimitAtPosition g coordinate (CellArray2D.neighborPositionAt (cell g coordinate) coordinate otherCoordinate)

    let areConnected g coordinate otherCoordinate =
        let cell = (cell g coordinate)
        let neighborPos = CellArray2D.neighborPositionAt cell coordinate otherCoordinate

        not (isLimitAtPosition g coordinate neighborPos) &&        
        (CellArray2D.connectionStateAtPosition cell neighborPos) = Open

    let neighbors g coordinate =
        let cell = cell g coordinate

        Canvas.CanvasArray2D.neighborsPartOfMazeOf g.Canvas (CellArray2D.listOfPossibleCoordinate cell coordinate) 
        |> Seq.filter(fun (_, nPosition) -> not (isLimitAtPosition g coordinate nPosition))
        |> Seq.map(fst)

    let neighbor g coordinate position =
        let cell = cell g coordinate
        (CellArray2D.neighborCoordinateAt cell coordinate position)

    let updateConnectionState g connectionState coordinate otherCoordinate =
        let currentCell = cell g coordinate
        let otherCell = cell g otherCoordinate
        
        g.Cells.[coordinate.RIndex, coordinate.CIndex] <-
            CellArray2D.newCellWithStateAtPosition currentCell connectionState (CellArray2D.neighborPositionAt currentCell coordinate otherCoordinate)
        
        g.Cells.[otherCoordinate.RIndex, otherCoordinate.CIndex] <-
            CellArray2D.newCellWithStateAtPosition otherCell connectionState (CellArray2D.neighborPositionAt otherCell otherCoordinate coordinate)

    let ifNotAtLimitUpdateConnectionState g connectionState coordinate otherCoordinate =
        if not (isLimitAtCoordinate g coordinate otherCoordinate) then
            updateConnectionState g connectionState coordinate otherCoordinate

    let openCell g coordinate =
        let cell = cell g coordinate
        let candidatePosition =
            (CellArray2D.listOfPossibleCoordinate cell coordinate)
            |> Seq.tryFind(fun (c, _) -> (not (existAt g c)) || (not (isCellPartOfMaze g c)))

        match candidatePosition with
        | Some (_, position) ->
            g.Cells.[coordinate.RIndex, coordinate.CIndex] <-
                CellArray2D.newCellWithStateAtPosition cell Open position
        | None -> ()

    let weaveCoordinates g coordinates =
        match g.Disposition with
        | Orthogonal _ -> OrthoCell.weaveCoordinates coordinates

    let firstCellPartOfMaze g =
        Canvas.CanvasArray2D.firstPartOfMazeZone g.Canvas

    let lastCellPartOfMaze g =
        Canvas.CanvasArray2D.lastPartOfMazeZone g.Canvas

    let private createInternal cellCreation internalConnectionType (canvas : Canvas.CanvasArray2D) disposition =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                cellCreation
                    (Canvas.CanvasArray2D.numberOfRows canvas)
                    (Canvas.CanvasArray2D.numberOfColumns canvas)
                    internalConnectionType
                    { RIndex = rowIndex; CIndex = columnIndex }
                    (Canvas.CanvasArray2D.isZonePartOfMaze canvas))

        {
          Canvas = canvas
          Disposition = disposition
          Cells = cells
        }

    let createBaseGrid cellCreation canvas disposition =
        createInternal cellCreation Close canvas disposition

    let createEmptyBaseGrid cellCreation canvas disposition =
        createInternal cellCreation Open canvas disposition