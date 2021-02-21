// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open System.Collections.Generic
open Mazes.Core

type NDimensionalStructure<'Grid, 'Position> =
    private
        {
            //Dimensions : Dimension
            Structure : Dictionary<Dimension, IAdjacentStructure<'Grid, 'Position>>
            NonAdjacent2DConnections : NonAdjacent2DConnections
            Obstacles : N_Obstacles
        }

    member this.Slice2D dimension =
        this.Structure.Item(dimension)

    member this.TotalOfMazeCells =
        this.Structure
        |> Seq.sumBy(fun kv -> kv.Value.TotalOfMazeCells)

    member this.ExistAt (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.ToDimension
        if this.Structure.ContainsKey(dimension) then
            (this.Slice2D dimension).ExistAt nCoordinate.ToCoordinate2D
        else
            false

    member this.CoordinatesPartOfMaze =
        let cells (dimension : Dimension) (adjStruct : IAdjacentStructure<'Grid, 'Position>) =
            adjStruct.Cells
            |> Seq.filter(fun (_, coordinate) -> adjStruct.IsCellPartOfMaze coordinate)
            |> Seq.map(fun (_, coordinate) -> NCoordinate.create dimension coordinate)

        this.Structure
        |> Seq.collect(fun kv -> cells kv.Key kv.Value)

    member this.RandomCoordinatePartOfMazeAndNotConnected (rng : Random) =
        let unconnectedPartOfMazeCells =
            this.CoordinatesPartOfMaze
            |> Seq.filter(fun c ->
                    let slice2d = this.Slice2D c.ToDimension
                    not (slice2d.IsCellConnected c.ToCoordinate2D || this.NonAdjacent2DConnections.IsCellConnected c))
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

    member this.Neighbors (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.ToDimension
        let slice2d = this.Slice2D dimension

        let neighbors2d =
            slice2d.Neighbors nCoordinate.ToCoordinate2D
            |> Seq.map(NCoordinate.create dimension)

        let neighbors =
            this.NonAdjacent2DConnections.Neighbors nCoordinate
            |> Seq.map(fst)

        neighbors2d |> Seq.append neighbors

    member this.IsCellConnected nCoordinate =
        this.NonAdjacent2DConnections.IsCellConnected nCoordinate ||
        (this.Slice2D nCoordinate.ToDimension).IsCellConnected nCoordinate.ToCoordinate2D

    member this.AreConnected nCoordinate otherNCoordinate =
        let nonAdjacent2DCondition =
            if this.NonAdjacent2DConnections.ExistNeighbor nCoordinate otherNCoordinate then
                this.NonAdjacent2DConnections.AreConnected nCoordinate otherNCoordinate
            else
                false
        
        let adjacent2DCondition =
            (nCoordinate.IsSame2DDimension otherNCoordinate) &&
            (this.Slice2D nCoordinate.ToDimension).AreConnected nCoordinate.ToCoordinate2D otherNCoordinate.ToCoordinate2D

        nonAdjacent2DCondition || adjacent2DCondition

    member this.ConnectedNeighbors isConnected (nCoordinate : NCoordinate) =
        this.Neighbors nCoordinate
        |> Seq.filter(fun c ->
            let isConnectedAdjacent2d = (this.Slice2D nCoordinate.ToDimension).IsCellConnected nCoordinate.ToCoordinate2D
            let isConnectedNonAdjacent2d = this.NonAdjacent2DConnections.IsCellConnected nCoordinate

            if isConnected then isConnectedAdjacent2d || isConnectedNonAdjacent2d
            else isConnectedAdjacent2d && isConnectedNonAdjacent2d)
        |> Seq.distinct

    member this.ConnectedWithNeighbors connected nCoordinate =
        seq {
            for neighborCoordinate in (this.Neighbors nCoordinate) do
                let areConnected = this.AreConnected nCoordinate neighborCoordinate
                if (connected && areConnected) || (not connected && not areConnected) then
                    yield neighborCoordinate
        }

    member this.UpdateConnection connectionType nCoordinate otherNCoordinate =
        if this.NonAdjacent2DConnections.ExistNeighbor nCoordinate otherNCoordinate then
            this.NonAdjacent2DConnections.UpdateConnection connectionType nCoordinate otherNCoordinate
        elif nCoordinate.IsSame2DDimension otherNCoordinate then
            (this.Slice2D nCoordinate.ToDimension).UpdateConnection connectionType nCoordinate.ToCoordinate2D otherNCoordinate.ToCoordinate2D

    member this.IfNotAtLimitUpdateConnection connectionType nCoordinate otherNCoordinate =
        if (this.NonAdjacent2DConnections.ExistNeighbor nCoordinate otherNCoordinate) ||
           ((nCoordinate.IsSame2DDimension otherNCoordinate) &&
            not ((this.Slice2D nCoordinate.ToDimension).IsLimitAt nCoordinate.ToCoordinate2D otherNCoordinate.ToCoordinate2D)) then

            this.UpdateConnection connectionType nCoordinate otherNCoordinate

    member this.UpdateConnectionNonAdjacent2DNeighbor connectionType nCoordinate otherNCoordinate =
         this.NonAdjacent2DConnections.UpdateConnection connectionType nCoordinate otherNCoordinate

    member this.OpenCell (nCoordinate : NCoordinate) =
        (this.Slice2D nCoordinate.ToDimension).OpenCell nCoordinate.ToCoordinate2D

    member this.CostOfCoordinate nCoordinate =
        1 + (this.Obstacles.Cost nCoordinate)

    member this.GetFirstCellPartOfMaze =
        let dimension =
            this.Structure.Keys
            |> Seq.sort
            |> Seq.find(fun d -> (this.Slice2D d).TotalOfMazeCells > 0)

        NCoordinate.create dimension (this.Slice2D dimension).GetFirstCellPartOfMaze

    member this.GetLastCellPartOfMaze =
        let dimension =
            this.Structure.Keys
            |> Seq.sortDescending
            |> Seq.find(fun d -> (this.Slice2D d).TotalOfMazeCells > 0)

        NCoordinate.create dimension (this.Slice2D dimension).GetLastCellPartOfMaze

module NDimensionalStructure =

    let create (dimensions : Dimension) (baseGrid : (unit -> IAdjacentStructure<_, _>)) =
//        let dimensionsSeq (dimensionBase : Dimension) =
//            let next dimension position =
//                let newDimension = dimension |> Array.copy
//                if newDimension.[position] < dimensions.[position] then
//                    newDimension.[position] <- newDimension.[position] + 1
//                else
//                    
//            
//            let mutable dimension = dimensionBase
//            seq {
//                while dimension <> dimensions do
//                    
//            }
                
        ()