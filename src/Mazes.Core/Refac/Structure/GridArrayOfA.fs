// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArrayOfA =
    private
        {
            CanvasArrayOfA : Canvas.CanvasArrayOfA
            GridStructureArrayOfA : GridStructureArrayOfA
        }

module GridArrayOfA =

    let cellsArrayOfA g =
        match g.GridStructureArrayOfA with
        | GridArrayOfAPolar g -> g

    let ratio g coordinate =
        let c = (cellsArrayOfA g)
        let maxCellsInLastRing = c.[ArrayOfA.maxD1Index c].Length
        let ringLength = c.[coordinate.RIndex].Length

        maxCellsInLastRing / ringLength

    let adjustedCoordinate g coordinate =
        { coordinate with CIndex = coordinate.CIndex / (ratio g coordinate) }

    let adjustedExistAt g coordinate =
        coordinate.CIndex % (ratio g coordinate) = 0

    let virtualNeighbor g coordinate disposition =
        Some
            (CellArrayOfAM.listOfPossibleCoordinate g.GridStructureArrayOfA coordinate
            |> Array.find(fun (_,d) -> d = disposition)
            |> fst)