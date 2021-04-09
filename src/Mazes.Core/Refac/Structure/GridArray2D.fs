// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArray2D =
    private
        {
            Canvas : Canvas.CanvasArray2D
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

    let dimension1Boundaries g _ =
        (0, Canvas.CanvasArray2D.numberOfRows g.Canvas)

    let dimension2Boundaries g _ =
        (0, Canvas.CanvasArray2D.numberOfColumns g.Canvas)

    let adjustedCoordinate _ coordinate =
        coordinate

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
        CellArray2D.connectionsState (cell g coordinate)
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

    let isLimitAt g coordinate position =
        let zone = Canvas.CanvasArray2D.zone g.Canvas coordinate
        let cell = cell g coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = CellArray2D.neighborCoordinateAt coordinate position

                match neighborCoordinate with
                | Some neighborCoordinate ->
                    not ((Canvas.CanvasArray2D.existAt g.Canvas neighborCoordinate) &&
                        (Canvas.CanvasArray2D.zone g.Canvas neighborCoordinate).IsAPartOfMaze)
                | None -> true

        not zone.IsAPartOfMaze ||
        (CellArray2D.connectionStateAtPosition cell position) = ClosePersistent ||
        neighborCondition()

    let areConnected g coordinate otherCoordinate =
        let cell = (cell g coordinate)
        let neighborPos = CellArray2D.neighborPositionAt cell coordinate otherCoordinate

        not (isLimitAt g coordinate neighborPos) &&        
        (CellArray2D.connectionStateAtPosition cell neighborPos) = Open

    let neighbors g coordinate =
        let cell = cell g coordinate

        Canvas.CanvasArray2D.neighborsPartOfMazeOf g.Canvas (CellArray2D.listOfPossibleCoordinate cell coordinate) 
        |> Seq.filter(fun (_, nPosition) -> not (isLimitAt g coordinate nPosition))
        |> Seq.map(fst)

    let neighbor coordinate position =
        (CellArray2D.neighborCoordinateAt coordinate position)

    let updateConnectionState g connectionState coordinate otherCoordinate =
        let currentCell = cell g coordinate
        let otherCell = cell g otherCoordinate
        
        g.Cells.[coordinate.RIndex, coordinate.CIndex] <-
            CellArray2D.cellWithStateAtPosition currentCell connectionState (CellArray2D.neighborPositionAt currentCell coordinate otherCoordinate)
        
        g.Cells.[otherCoordinate.RIndex, otherCoordinate.CIndex] <-
            CellArray2D.cellWithStateAtPosition otherCell connectionState (CellArray2D.neighborPositionAt otherCell otherCoordinate coordinate)