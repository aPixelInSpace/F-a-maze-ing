// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core

type Grid<'Grid, 'Position> =
    {
        BaseGrid : IAdjacentStructure<'Grid, 'Position>
        NonAdjacentNeighbors : NonAdjacentNeighbors
        Obstacles : Obstacles
    }

    interface IGrid<Grid<'Grid, 'Position>> with

        member this.TotalOfMazeCells =
            this.BaseGrid.TotalOfMazeCells

        member this.RIndexes =
            this.BaseGrid.RIndexes

        member this.CIndexes =
            this.BaseGrid.CIndexes

        member this.Dimension1Boundaries dimension2Index =
            this.BaseGrid.Dimension1Boundaries dimension2Index

        member this.Dimension2Boundaries dimension1Index =
            this.BaseGrid.Dimension2Boundaries dimension1Index

        member this.AdjustedCoordinate coordinate =
            this.BaseGrid.AdjustedCoordinate coordinate

        member this.ExistAt coordinate =
            this.BaseGrid.ExistAt coordinate

        member this.AdjustedExistAt coordinate =
            this.BaseGrid.AdjustedExistAt coordinate

        member this.CoordinatesPartOfMaze =
            this.BaseGrid.Cells
            |> Seq.filter(fun (_, coordinate) -> this.BaseGrid.IsCellPartOfMaze coordinate)
            |> Seq.map(snd)

        member this.RandomCoordinatePartOfMazeAndNotConnected (rng : Random) =
            let unconnectedPartOfMazeCells =
                this.BaseGrid.Cells
                |> Seq.filter(fun (_, c) ->
                    this.BaseGrid.IsCellPartOfMaze c &&
                    not (this.BaseGrid.IsCellConnected c || this.NonAdjacentNeighbors.IsCellConnected c))
                |> Seq.toArray

            snd (unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)])

        member this.IsLimitAt coordinate otherCoordinate =
            this.BaseGrid.IsLimitAt coordinate otherCoordinate

        member this.Neighbors coordinate =
            (this.ToInterface.AdjacentNeighbors coordinate)
            |> Seq.append (
                this.NonAdjacentNeighbors.NonAdjacentNeighbors(coordinate)
                |> Seq.map(fst))

        member this.AdjacentNeighbors coordinate =
            this.BaseGrid.Neighbors coordinate

        member this.AdjacentNeighbor coordinate position =
            this.BaseGrid.Neighbor coordinate position

        member this.AdjacentVirtualNeighbor coordinate position =
            this.BaseGrid.VirtualNeighbor coordinate position

        member this.IsCellPartOfMaze coordinate =
            this.BaseGrid.IsCellPartOfMaze coordinate

        member this.IsCellConnected coordinate =
            this.NonAdjacentNeighbors.IsCellConnected coordinate ||
            this.BaseGrid.IsCellConnected coordinate

        member this.AreConnected coordinate otherCoordinate =
            let nonAdjacentCondition =
                if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
                    this.NonAdjacentNeighbors.AreConnected coordinate otherCoordinate
                else
                    false
            
            let adjacentCondition =
                this.BaseGrid.AreConnected coordinate otherCoordinate

            nonAdjacentCondition || adjacentCondition

        member this.ConnectedNeighbors isConnected coordinate =
            this.ToInterface.Neighbors coordinate
            |> Seq.filter(fun nCoordinate ->
                if isConnected then
                    (this.BaseGrid.IsCellConnected nCoordinate) = isConnected ||
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isConnected
                else
                    (this.BaseGrid.IsCellConnected nCoordinate) = isConnected &&
                    (this.NonAdjacentNeighbors.IsCellConnected nCoordinate) = isConnected)
            |> Seq.distinct

        member this.ConnectedWithNeighbors connected coordinate =
            let neighborsCoordinates = this.ToInterface.Neighbors coordinate

            seq {
                for neighborCoordinate in neighborsCoordinates do
                    if connected then
                        if this.NonAdjacentNeighbors.ExistNeighbor coordinate neighborCoordinate then
                            if this.NonAdjacentNeighbors.AreConnected coordinate neighborCoordinate then
                                yield neighborCoordinate
                        elif (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
                    else
                        if this.NonAdjacentNeighbors.ExistNeighbor coordinate neighborCoordinate then
                            if not (this.NonAdjacentNeighbors.AreConnected coordinate neighborCoordinate) then
                                yield neighborCoordinate
                        elif not (this.ToInterface.AreConnected coordinate neighborCoordinate) then
                            yield neighborCoordinate
            }

        member this.UpdateConnection connectionType coordinate otherCoordinate =
            if this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate then
                this.NonAdjacentNeighbors.UpdateConnection connectionType coordinate otherCoordinate
            else
                this.BaseGrid.UpdateConnection connectionType coordinate otherCoordinate

        member this.IfNotAtLimitUpdateConnection connectionType coordinate otherCoordinate =
            if (this.NonAdjacentNeighbors.ExistNeighbor coordinate otherCoordinate) || not (this.BaseGrid.IsLimitAt coordinate otherCoordinate) then
                this.ToInterface.UpdateConnection connectionType coordinate otherCoordinate

        member this.CostOfCoordinate coordinate =
            1 + (this.Obstacles.Cost coordinate)

        member this.GetFirstCellPartOfMaze =
            this.BaseGrid.GetFirstCellPartOfMaze

        member this.GetLastCellPartOfMaze =
            this.BaseGrid.GetLastCellPartOfMaze

        member this.ToSpecializedGrid =
            this

    member this.ToInterface =
        this :> IGrid<Grid<'Grid, 'Position>>

module Grid =
    
    let create (baseGrid : IAdjacentStructure<_, _>) =
        {
            BaseGrid = baseGrid
            NonAdjacentNeighbors = NonAdjacentNeighbors.CreateEmpty
            Obstacles = Obstacles.CreateEmpty
        } :> IGrid<_>