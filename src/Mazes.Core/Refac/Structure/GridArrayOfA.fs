// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArrayOfA =
    private
        {
            CanvasArrayOfA : Canvas.CanvasArrayOfA
            TypeArrayOfA : GridArrayOfAType
            CellsArrayOfA : CellArrayOfA[][]
        }

module GridArrayOfA =

    let ratio g coordinate =
        let maxCellsInLastRing = g.CellsArrayOfA.[ArrayOfA.maxD1Index g.CellsArrayOfA].Length
        let ringLength = g.CellsArrayOfA.[coordinate.RIndex].Length

        maxCellsInLastRing / ringLength

    let adjustedCoordinate g coordinate =
        { coordinate with CIndex = coordinate.CIndex / (ratio g coordinate) }

    let adjustedExistAt g coordinate =
        coordinate.CIndex % (ratio g coordinate) = 0

    let virtualNeighbor g coordinate disposition =
        Some
            (CellArrayOfAM.listOfPossibleCoordinate g.TypeArrayOfA coordinate
            |> Array.find(fun (_,d) -> d = disposition)
            |> fst)