// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Polar.PolarCoordinate

open Mazes.Core
open Mazes.Core.Grid.Polar.ArrayOfA

let neighborsCoordinateAt (arrayOfA : 'A[][]) coordinate position =
    seq {
        match position with
        | Inward ->
            if not (isFirstRing coordinate.RIndex) then
                let inwardRingNumberOfCells = getNumberOfCellsAt arrayOfA (coordinate.RIndex - 1)
                let currentRingNumberOfCells = getNumberOfCellsAt arrayOfA coordinate.RIndex
                let ratio = currentRingNumberOfCells / inwardRingNumberOfCells
                yield { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex / ratio }
        | Outward ->
            if not (isLastRing coordinate.RIndex (maxRingIndex arrayOfA)) then
                let currentRingNumberOfCells = getNumberOfCellsAt arrayOfA coordinate.RIndex
                let outwardRingNumberOfCells = getNumberOfCellsAt arrayOfA (coordinate.RIndex + 1)
                let ratio = outwardRingNumberOfCells / currentRingNumberOfCells
                for ratioIndex in 0 .. ratio - 1 do
                    yield { RIndex = coordinate.RIndex + 1; CIndex = (coordinate.CIndex * ratio) + ratioIndex }
        | Left ->
            if isFirstCellOfRing coordinate.CIndex then
                yield { RIndex = coordinate.RIndex; CIndex = maxCellsIndex arrayOfA coordinate.RIndex }
            else
                yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
        | Right ->
            if isLastCellOfRing arrayOfA coordinate.RIndex coordinate.CIndex then
                yield { RIndex = coordinate.RIndex; CIndex = minCellIndex }
            else
                yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }   
    }