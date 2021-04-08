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