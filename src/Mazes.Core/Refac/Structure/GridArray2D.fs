// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArray2D =
    private
        {
            CanvasArray2D : Canvas.CanvasArray2D
            GridStructureArray2D : GridStructureArray2D
        }

module GridArray2D =

    let cellsArray2D g =
        match g.GridStructureArray2D with
        | GridArray2DOrthogonal g -> g

    let gridStructureArray2D g c =
        match g with
        | GridArray2DOrthogonal g -> c |> GridArray2DOrthogonal
    
    let gridStructure g =
        g.GridStructureArray2D

    let totalOfCells g =
        (cellsArray2D g).Length

    let totalOfMazeCells g =
        Canvas.CanvasArray2D.totalOfMazeZones g.CanvasArray2D

    let rIndexes g =
        cellsArray2D g |> Array2D.getRIndexes

    let cIndexes g =
        cellsArray2D g |> Array2D.getCIndexes

    let dimension1Boundaries g =
        (0, Canvas.CanvasArray2D.numberOfRows g.CanvasArray2D)

    let dimension2Boundaries g =
        (0, Canvas.CanvasArray2D.numberOfColumns g.CanvasArray2D)

    let numberOfRows g =
        Canvas.CanvasArray2D.numberOfRows g.CanvasArray2D

    let numberOfColumns g =
        Canvas.CanvasArray2D.numberOfColumns g.CanvasArray2D

    let existAt g coordinate =
        Array2D.existAt (cellsArray2D g) coordinate

    let cell g coordinate =
        Array2D.get (cellsArray2D g) coordinate

    let dispositions g coordinate =
        CellArray2DM.dispositions (cell g coordinate) coordinate
        |> Seq.map(DispositionArray2D)

    let cellsSeq g =
        cellsArray2D g |> Array2D.getItemByItem Array2D.RowsAscendingColumnsAscending (fun _ _ -> true)

    let isCellPartOfMaze g coordinate =
        Canvas.CanvasArray2D.isZonePartOfMaze g.CanvasArray2D coordinate

    let isCellConnected g coordinate =
        CellArray2DM.connectionsStates (cell g coordinate)
        |> Array.filter(fun c -> c = Open)
        |> Array.length > 0

    let coordinatesPartOfMaze g =
        cellsSeq g
        |> Seq.filter(fun (_, c) -> isCellPartOfMaze g c)
        |> Seq.map(snd)

    let randomCoordinatePartOfMazeAndNotConnected g (rng : Random) =
        let unconnectedPartOfMazeCells =
            coordinatesPartOfMaze g
            |> Seq.filter(fun c -> not <| isCellConnected g c)
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

    let isLimitAtPosition g coordinate position =
        let zone = Canvas.CanvasArray2D.zone g.CanvasArray2D coordinate
        let cell = cell g coordinate

        let neighborCondition =
            fun () ->
                let neighborCoordinate = CellArray2DM.neighborCoordinateAt g.GridStructureArray2D coordinate position

                match neighborCoordinate with
                | Some neighborCoordinate ->
                    not ((Canvas.CanvasArray2D.existAt g.CanvasArray2D neighborCoordinate) &&
                        (Canvas.CanvasArray2D.zone g.CanvasArray2D neighborCoordinate).IsAPartOfMaze)
                | None -> true

        not zone.IsAPartOfMaze ||
        (CellArray2DM.connectionStateAtPosition cell position) = ClosePersistent ||
        neighborCondition()

    let isLimitAtCoordinate g coordinate otherCoordinate =
        isLimitAtPosition g coordinate (CellArray2DM.neighborPositionAt g.GridStructureArray2D coordinate otherCoordinate)

    let areConnected g coordinate otherCoordinate =
        let cell = (cell g coordinate)
        let neighborPos = CellArray2DM.neighborPositionAt g.GridStructureArray2D coordinate otherCoordinate

        not (isLimitAtPosition g coordinate neighborPos) &&        
        (CellArray2DM.connectionStateAtPosition cell neighborPos) = Open

    let neighbors g coordinate =
        Canvas.CanvasArray2D.neighborsPartOfMazeOf g.CanvasArray2D (CellArray2DM.listOfPossibleCoordinate g.GridStructureArray2D coordinate) 
        |> Seq.filter(fun (_, nPosition) -> not (isLimitAtPosition g coordinate nPosition))
        |> Seq.map(fst)

    let neighbor g coordinate position =
        (CellArray2DM.neighborCoordinateAt g.GridStructureArray2D coordinate position)

    let updateConnectionState g connectionState coordinate otherCoordinate =
        let currentCell = cell g coordinate
        let otherCell = cell g otherCoordinate
        
        (cellsArray2D g).[coordinate.RIndex, coordinate.CIndex] <-
            CellArray2DM.newCellWithStateAtPosition currentCell connectionState (CellArray2DM.neighborPositionAt g.GridStructureArray2D coordinate otherCoordinate)
        
        (cellsArray2D g).[otherCoordinate.RIndex, otherCoordinate.CIndex] <-
            CellArray2DM.newCellWithStateAtPosition otherCell connectionState (CellArray2DM.neighborPositionAt g.GridStructureArray2D otherCoordinate coordinate)

    let ifNotAtLimitUpdateConnectionState g connectionState coordinate otherCoordinate =
        if not (isLimitAtCoordinate g coordinate otherCoordinate) then
            updateConnectionState g connectionState coordinate otherCoordinate

    let openCell g coordinate =
        let cell = cell g coordinate
        let candidatePosition =
            (CellArray2DM.listOfPossibleCoordinate g.GridStructureArray2D coordinate)
            |> Seq.tryFind(fun (c, _) -> (not (existAt g c)) || (not (isCellPartOfMaze g c)))

        match candidatePosition with
        | Some (_, position) ->
            (cellsArray2D g).[coordinate.RIndex, coordinate.CIndex] <-
                CellArray2DM.newCellWithStateAtPosition cell Open position
        | None -> ()

    let weaveCoordinates g coordinates =
        match g.GridStructureArray2D with
        | GridArray2DOrthogonal _ -> OrthogonalM.weaveCoordinates coordinates

    let firstCellPartOfMaze g =
        Canvas.CanvasArray2D.firstPartOfMazeZone g.CanvasArray2D

    let lastCellPartOfMaze g =
        Canvas.CanvasArray2D.lastPartOfMazeZone g.CanvasArray2D

    let private createInternal internalConnectionType gridStructure (canvas : Canvas.CanvasArray2D) =
        let cells =
            canvas.Zones |>
            Array2D.mapi(fun rowIndex columnIndex _ ->
                CellArray2DM.initialize
                    (Canvas.CanvasArray2D.isZonePartOfMaze canvas)
                    gridStructure
                    (Canvas.CanvasArray2D.numberOfRows canvas)
                    (Canvas.CanvasArray2D.numberOfColumns canvas)
                    internalConnectionType
                    { RIndex = rowIndex; CIndex = columnIndex })

        {
          CanvasArray2D = canvas
          GridStructureArray2D = (gridStructureArray2D gridStructure cells)
        }

    let createBaseGrid disposition canvas =
        createInternal Close disposition canvas

    let createEmptyBaseGrid disposition canvas =
        createInternal Open disposition canvas

    let toString g =
        match g.GridStructureArray2D with
        | GridArray2DOrthogonal _ -> OrthogonalM.toString CellArray2DM.connectionStateAtPositionGeneric (cellsArray2D g)