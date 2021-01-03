// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.ArrayOfA.Polar.PolarCoordinate

open Mazes.Core
open Mazes.Core.Grid.ArrayOfA.Polar.PolarArrayOfA

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
            if not (isLastRing coordinate.RIndex (numberOfRings arrayOfA)) then
                let currentRingNumberOfCells = getNumberOfCellsAt arrayOfA coordinate.RIndex
                let outwardRingNumberOfCells = getNumberOfCellsAt arrayOfA (coordinate.RIndex + 1)
                let ratio = outwardRingNumberOfCells / currentRingNumberOfCells
                for ratioIndex in 0 .. ratio - 1 do
                    yield { RIndex = coordinate.RIndex + 1; CIndex = (coordinate.CIndex * ratio) + ratioIndex }
        | Ccw ->
            if isFirstCellOfRing coordinate.CIndex then
                yield { RIndex = coordinate.RIndex; CIndex = maxCellsIndex arrayOfA coordinate.RIndex }
            else
                yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
        | Cw ->
            if isLastCellOfRing arrayOfA coordinate.RIndex coordinate.CIndex then
                yield { RIndex = coordinate.RIndex; CIndex = minCellIndex }
            else
                yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
    }

let neighborBaseCoordinateAt coordinate position =    
        match position with
        | Outward ->  { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
        | Cw -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        | Inward -> { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
        | Ccw -> { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }

let neighborPositionAt (arrayOfA : 'A[][]) coordinate otherCoordinate =
        let neighborCoordinateAt = neighborsCoordinateAt arrayOfA coordinate
        match otherCoordinate with
        | c when c = (neighborCoordinateAt Ccw |> Seq.head) -> Ccw
        | c when c = (neighborCoordinateAt Cw |> Seq.head) -> Cw
        | c when (neighborCoordinateAt Outward |> Seq.tryFind(fun n -> c = n)).IsSome -> Outward
        | c when (neighborCoordinateAt Inward |> Seq.tryFind(fun n -> c = n)).IsSome -> Inward
        | _ -> failwith "Unable to match the polar coordinates with a position"