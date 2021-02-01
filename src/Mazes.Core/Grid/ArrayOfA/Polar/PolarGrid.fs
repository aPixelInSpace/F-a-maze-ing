// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.ArrayOfA.Polar

open System
open System.Text
open Mazes.Core
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid
open Mazes.Core.ArrayOfA
open Mazes.Core.Grid.ArrayOfA.Polar.PolarArrayOfA

type PolarGrid =
    {
        Canvas : Canvas
        Cells : PolarCell[][]
        NonAdjacentNeighbors : NonAdjacentNeighbors
        Obstacles : Obstacles
    }

    interface IGrid<PolarGrid> with
        member this.TotalOfMazeCells =
            this.Canvas.TotalOfMazeZones

        member this.EveryCoordinatesPartOfMaze =
            this.GetCellByCell (fun _ _ -> true)
            |> Seq.map(snd)

        member this.Dimension1Boundaries cellIndex =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length

            let startIndex =
                this.Cells
                |> Array.findIndex(fun ring ->
                    let steps = maxCellsInLastRing / ring.Length
                    cellIndex % steps = 0)

            let length = this.Cells.Length - startIndex

            (startIndex, length)

        member this.Dimension2Boundaries ringIndex =
            (0, this.Cells.[ringIndex].Length)

        member this.AddCostForCoordinate cost coordinate =
            this.Obstacles.AddUpdateCost cost coordinate

        member this.CostOfCoordinate coordinate =
            1 + (this.Obstacles.Cost coordinate)

        member this.AdjacentNeighborAbstractCoordinate coordinate position =
            Some (PolarCoordinate.neighborBaseCoordinateAt coordinate (PolarPosition.map position))

        member this.IsCellLinked coordinate =
            this.NonAdjacentNeighbors.IsLinked coordinate ||
            (this.Cell coordinate).IsLinked this.Cells coordinate

        member this.ExistAt coordinate =
            existAt this.Cells coordinate

        member this.GetAdjustedExistAt coordinate =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length
            let ratio = maxCellsInLastRing / ringLength
            coordinate.CIndex % ratio = 0

        member this.IsLimitAt coordinate otherCoordinate =
            this.IsLimitAt coordinate otherCoordinate

        member this.IsCellPartOfMaze coordinate =
            this.Canvas.IsZonePartOfMaze coordinate

        member this.GetRIndexes =
            this.Cells |> getRIndexes

        member this.GetCIndexes =
            this.Cells |> getCIndexes

        member this.GetAdjustedCoordinate coordinate =
            let maxCellsInLastRing = this.Cells.[maxD1Index this.Cells].Length
            let ringLength = this.Cells.[coordinate.RIndex].Length

            let ratio = maxCellsInLastRing / ringLength

            { coordinate with CIndex = coordinate.CIndex / ratio }

        member this.CoordinatesPartOfMaze =
            this.CoordinatesPartOfMaze

        member this.LinkCells coordinate otherCoordinate =
            this.LinkCells coordinate otherCoordinate

        member this.PutBorderBetweenCells coordinate otherCoordinate =
            let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
            this.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Border

        member this.AdjacentNeighbor coordinate position =
            this.AdjacentNeighbor coordinate (PolarPosition.map position)

        member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
            this.IfNotAtLimitLinkCells coordinate otherCoordinate

        member this.Neighbors coordinate =
            this.NeighborsFrom coordinate

        member this.NeighborsThatAreLinked isLinked coordinate =
            this.NeighborsThatAreLinked isLinked coordinate

        member this.AddUpdateNonAdjacentNeighbor fromCoordinate toCoordinate wallType =
            this.NonAdjacentNeighbors.AddUpdate fromCoordinate toCoordinate wallType

        member this.LinkedNeighbors coordinate =
            this.LinkedNeighbors coordinate

        member this.NotLinkedNeighbors coordinate =
            this.NotLinkedNeighbors coordinate

        member this.RandomNeighbor rng coordinate =
            this.RandomNeighbor rng coordinate

        member this.RandomCoordinatePartOfMazeAndNotLinked rng =
            this.RandomCoordinatePartOfMazeAndNotLinked rng

        member this.GetFirstPartOfMazeZone =
            snd this.Canvas.GetFirstPartOfMazeZone

        member this.GetLastPartOfMazeZone =
            snd this.Canvas.GetLastPartOfMazeZone

        member this.ToString =
            this.ToString

        member this.ToSpecializedGrid =
            this

    member this.Cell coordinate =
        get this.Cells coordinate

    member this.GetCellByCell filter =
        this.Cells |> getItemByItem filter

    member this.IsLimitAt coordinate otherCoordinate =
        let zone = this.Canvas.Zone coordinate

        let neighborCondition =
            let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
            let neighborCell = this.Cell otherCoordinate

            if neighborPosition <> Inward then
                not (this.Canvas.ExistAt otherCoordinate) ||
                not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                neighborCell.WallTypeAtPosition neighborPosition.Opposite = Border                
            else
                let cell = this.Cell coordinate
                if not (isFirstRing coordinate.RIndex) then
                    not (this.Canvas.ExistAt otherCoordinate) ||
                    not (this.Canvas.Zone otherCoordinate).IsAPartOfMaze ||
                    cell.WallTypeAtPosition neighborPosition = Border
                else
                    true

        (not zone.IsAPartOfMaze) ||
        neighborCondition

    member private this.UpdateWallAtPosition coordinate neighborCoordinate (neighborPosition : PolarPosition) wallType =
        let getWalls coordinate position =
            this.Cells.[coordinate.RIndex].[coordinate.CIndex].Walls
            |> Array.map(fun wall ->
                if wall.WallPosition = position then
                    { WallType = wallType; WallPosition = position }
                else
                    wall)

        match neighborPosition with
        | Ccw | Cw ->
            this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
            this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }
            if (PolarCoordinate.neighborsCoordinateAt this.Cells coordinate neighborPosition.Opposite) |> Seq.head = neighborCoordinate then
                this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition.Opposite) }
                this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition) }            
        | Inward ->
            this.Cells.[coordinate.RIndex].[coordinate.CIndex] <- { Walls = (getWalls coordinate neighborPosition) }
        | Outward ->
            this.Cells.[neighborCoordinate.RIndex].[neighborCoordinate.CIndex] <- { Walls = (getWalls neighborCoordinate neighborPosition.Opposite) }

    member this.LinkCells coordinate otherCoordinate =
        if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
            this.NonAdjacentNeighbors.AddUpdate coordinate otherCoordinate Empty
        else
            let neighborPosition = PolarCoordinate.neighborPositionAt this.Cells coordinate otherCoordinate
            this.UpdateWallAtPosition coordinate otherCoordinate neighborPosition Empty

    member this.IfNotAtLimitLinkCells coordinate otherCoordinate =
        if (this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate) || not (this.IsLimitAt coordinate otherCoordinate) then
            this.LinkCells coordinate otherCoordinate

    member this.AdjacentNeighbor coordinate position =
        let neighbors = PolarCoordinate.neighborsCoordinateAt this.Cells coordinate position
        if neighbors |> Seq.isEmpty then
            None
        else
            Some (neighbors |> Seq.last)

    member this.AdjacentNeighborsFrom coordinate =
        let listOfNeighborCoordinate =
            let neighborsCoordinateAt = PolarCoordinate.neighborsCoordinateAt this.Cells coordinate
            seq {
                for position in PolarPosition.values do
                    for coordinate in neighborsCoordinateAt position do
                        yield (coordinate, position)
            }

        this.Canvas.NeighborsPartOfMazeOf listOfNeighborCoordinate
        |> Seq.filter(fun (nCoordinate, _) -> not (this.IsLimitAt coordinate nCoordinate))
        |> Seq.map(fst)

    /// Returns the neighbors that are inside the bound of the grid
    member this.NeighborsFrom coordinate =
        let listOfNeighborCoordinate = this.AdjacentNeighborsFrom coordinate

        let listOfNonAdjacentNeighborCoordinate =
            this.NonAdjacentNeighbors.NonAdjacentNeighbors(coordinate)
            |> Seq.map(fst)

        listOfNeighborCoordinate
        |> Seq.append listOfNonAdjacentNeighborCoordinate

    member this.RandomNeighbor (rng : Random) coordinate =
        let neighbors = this.NeighborsFrom coordinate |> Seq.toArray
        neighbors.[rng.Next(neighbors.Length)]

    member this.NeighborsThatAreLinked isLinked coordinate =
        this.NeighborsFrom coordinate
        |> Seq.append (
            this.NonAdjacentNeighbors.NonAdjacentNeighbors coordinate
            |> Seq.map(fst))
        |> Seq.filter(fun nCoordinate ->
            if isLinked then
                (this.Cell nCoordinate).IsLinked this.Cells nCoordinate = isLinked ||
                (this.NonAdjacentNeighbors.IsLinked nCoordinate) = isLinked
            else
                (this.Cell nCoordinate).IsLinked this.Cells nCoordinate = isLinked &&
                (this.NonAdjacentNeighbors.IsLinked nCoordinate) = isLinked)

    member this.LinkedNeighbors coordinate =
        seq {
            let neighborsCoordinates = this.NeighborsFrom coordinate
            for neighborCoordinate in neighborsCoordinates do   
                if (this.AreLinked coordinate neighborCoordinate) then
                    yield neighborCoordinate
        }

    member this.NotLinkedNeighbors coordinate =
        let neighborsCoordinates = this.NeighborsFrom coordinate

        seq {
            for neighborCoordinate in neighborsCoordinates do   
                if not (this.AreLinked coordinate neighborCoordinate) then
                    yield neighborCoordinate
        }

    member this.RandomCoordinatePartOfMazeAndNotLinked (rng : Random) =
        let unlinkedPartOfMazeCells =
            this.GetCellByCell (fun cell coordinate -> (this.Canvas.Zone coordinate).IsAPartOfMaze && not (cell.IsLinked this.Cells coordinate))
            |> Seq.toArray
        snd unlinkedPartOfMazeCells.[rng.Next(unlinkedPartOfMazeCells.Length)]

    member this.CoordinatesPartOfMaze =
        this.Canvas.GetZoneByZone (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate)

    member this.ToString =
        let sBuilder = StringBuilder()

        let appendHorizontalWall wallType (sBuilder : StringBuilder) =
            match wallType with
                | Normal | Border -> sBuilder.Append("‾")
                | WallType.Empty -> sBuilder.Append("¨")

        let appendVerticalWall wallType (sBuilder : StringBuilder) =
            match wallType with
                | Normal | Border -> sBuilder.Append("|")
                | WallType.Empty -> sBuilder.Append("¦")

        let appendWhiteSpace (sBuilder : StringBuilder) =
            sBuilder.Append(" ")

        let appendRing appendCell appendLastCell (cellsRow : PolarCell array) =
            cellsRow
            |> Array.iter(appendCell)

            cellsRow
            |> Array.last
            |> appendLastCell

            sBuilder.Append("\n") |> ignore

        let lastCell (lastCell : PolarCell) =
            sBuilder
            |> appendVerticalWall (lastCell.WallTypeAtPosition Cw) |> ignore

        // first
        let firstRing (cell : PolarCell) =
            sBuilder
            |> appendVerticalWall (cell.WallTypeAtPosition Ccw)
            |> appendWhiteSpace
            |> ignore

        getRingByRing this.Cells
        |> Seq.head
        |> appendRing firstRing lastCell

        // others
        let everyOtherRing (cell : PolarCell) =
            sBuilder
            |> appendVerticalWall (cell.WallTypeAtPosition Ccw)
            |> appendHorizontalWall (cell.WallTypeAtPosition Inward)
            |> ignore

        getRingByRing this.Cells
        |> Seq.iteri(fun ringIndex cells ->
            if ringIndex > 0 then
                cells
                |> appendRing everyOtherRing lastCell
            else
                ())

        // last
        let lastRing (cell : PolarCell) =
            sBuilder
            |> appendWhiteSpace
            |> appendHorizontalWall (cell.WallTypeAtPosition Outward)
            |> ignore

        getRingByRing this.Cells
        |> Seq.last
        |> appendRing lastRing (fun _ -> ())

        sBuilder.ToString()

    member this.AreLinked coordinate otherCoordinate =
        if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
            this.NonAdjacentNeighbors.AreLinked coordinate otherCoordinate
        else
            not (this.IsLimitAt coordinate otherCoordinate) &&        
            (this.Cell coordinate).AreLinked this.Cells coordinate otherCoordinate

module PolarGrid =

    let Create (canvas : Canvas) =

        let cells =
            createPolar
                canvas.NumberOfRings
                canvas.WidthHeightRatio
                canvas.NumberOfCellsForCenterRing
                (fun rIndex cIndex -> PolarCell.create canvas { RIndex = rIndex; CIndex = cIndex } canvas.IsZonePartOfMaze)

        {
            Canvas = canvas
            Cells = cells
            NonAdjacentNeighbors = NonAdjacentNeighbors.CreateEmpty
            Obstacles = Obstacles.CreateEmpty
        }

    let CreateFunction canvas =
        fun () -> Create canvas :> IGrid<PolarGrid>