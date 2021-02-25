// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open System.Collections.Generic
open Mazes.Core

type NDimensionalStructure<'Grid, 'Position> =
    {
        Structure : Dictionary<Dimension, IAdjacentStructure<'Grid, 'Position>>
        NonAdjacent2DConnections : NonAdjacent2DConnections
        Obstacles : N_Obstacles
    }

    member this.Slice2D dimension =
        this.Structure.Item(dimension)

    member this.FirstSlice2D =
        let firstDimension =
            this.Structure.Keys
            |> Seq.sort
            |> Seq.head

        (firstDimension, this.Slice2D firstDimension)

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
        let cells (dimension : Dimension) (adjStruct : IAdjacentStructure<_,_>) =
            adjStruct.CoordinatesPartOfMaze
            |> Seq.map(NCoordinate.create dimension)

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
        let existNonAdjacentNeighbor = this.NonAdjacent2DConnections.ExistNeighbor nCoordinate otherNCoordinate

        let nonAdjacent2DCondition =
            if existNonAdjacentNeighbor then
                this.NonAdjacent2DConnections.AreConnected nCoordinate otherNCoordinate
            else
                false
        
        let adjacent2DCondition =
            not existNonAdjacentNeighbor &&
            (nCoordinate.IsSame2DDimension otherNCoordinate) &&
            (this.Slice2D nCoordinate.ToDimension).AreConnected nCoordinate.ToCoordinate2D otherNCoordinate.ToCoordinate2D

        nonAdjacent2DCondition || adjacent2DCondition

    member this.ConnectedNeighbors isConnected (nCoordinate : NCoordinate) =
        this.Neighbors nCoordinate
        |> Seq.filter(fun neighbor ->
            let isConnectedAdjacent2d = (this.Slice2D neighbor.ToDimension).IsCellConnected neighbor.ToCoordinate2D
            let isConnectedNonAdjacent2d = this.NonAdjacent2DConnections.IsCellConnected neighbor

            if isConnected then isConnectedAdjacent2d || isConnectedNonAdjacent2d
            else not (isConnectedAdjacent2d) && not (isConnectedNonAdjacent2d))
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

    member this.Weave (rng : Random) weight =
        for slice2D in this.Structure do
            let dimension = slice2D.Key
            let adjStruct = slice2D.Value

            let weaveCoordinates = adjStruct.WeaveCoordinates adjStruct.CoordinatesPartOfMaze
            for (fromCoordinate, toCoordinate) in weaveCoordinates do
                if (toCoordinate.RIndex >= fst (adjStruct.Dimension1Boundaries toCoordinate.CIndex)) &&
                   (toCoordinate.RIndex < snd (adjStruct.Dimension1Boundaries toCoordinate.CIndex)) &&
                   (toCoordinate.CIndex >= fst (adjStruct.Dimension2Boundaries toCoordinate.RIndex)) &&
                   (toCoordinate.CIndex < snd (adjStruct.Dimension2Boundaries toCoordinate.RIndex)) &&
                   adjStruct.IsCellPartOfMaze toCoordinate &&
                   rng.NextDouble() < weight
                   then

                    this.NonAdjacent2DConnections.UpdateConnection
                        Close (NCoordinate.create dimension fromCoordinate) (NCoordinate.create dimension toCoordinate)

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

type ArrayEqualityComparer() =
    interface IEqualityComparer<int[]> with
        member this.Equals (a,b) = (Array.forall2 (=) a b)
        member this.GetHashCode (a) = hash (a |> Array.map hash)

module NDimensionalStructure =

    let create (dimensions : Dimension) (newAdjacentStructureInstance : (unit -> IAdjacentStructure<_, _>)) =
        let baseAdjStruct = newAdjacentStructureInstance()
        let coordinates2D = baseAdjStruct.CoordinatesPartOfMaze

        let dimensionsSeq =
            let nextDimension (dimension : Dimension) =
                let newDimension = dimension |> Array.copy
                let mutable isFound = false 
                for d in 0 .. newDimension.Length - 1 do
                    if not isFound && newDimension.[d] < dimensions.[d] then
                        newDimension.[d] <- newDimension.[d] + 1
                        isFound <- true
                    elif not isFound then
                        newDimension.[d] <- 0

                newDimension

            let countSlices2D = (1, dimensions) ||> Array.fold(fun count d -> (d + 1) * count)

            let mutable currentDimension = Array.create dimensions.Length 0
            seq {
                yield currentDimension
                for _ in 1 .. countSlices2D - 1 do
                    currentDimension <- nextDimension currentDimension
                    yield currentDimension
            }

        let structure = Dictionary<Dimension, IAdjacentStructure<_, _>>(ArrayEqualityComparer())
        for dimension in dimensionsSeq do
            structure.Add(dimension, newAdjacentStructureInstance())

        // orthogonally connect every slice 2d to the next one
        let nonAdjacent2DConnections = NonAdjacent2DConnections.CreateEmpty
        for d in 0 .. dimensions.Length - 1 do
            let startDimensions = dimensionsSeq |> Seq.filter(fun dimension -> dimension.[d] = 0)
            for startDimension in startDimensions do

                let currentDimensionIndex = ref 0
                let mutable currentDimension = startDimension
                while currentDimensionIndex.Value < dimensions.[d] do                    
                    incr currentDimensionIndex
                    let newDimension = currentDimension |> Array.copy
                    newDimension.[d] <- newDimension.[d] + 1

                    for coordinate2D in coordinates2D do
                        let nCoordinate = NCoordinate.create currentDimension coordinate2D
                        let nOtherCoordinate = NCoordinate.create newDimension coordinate2D
                        nonAdjacent2DConnections.UpdateConnection Close nCoordinate nOtherCoordinate

                    currentDimension <- newDimension
        {
            Structure = structure
            NonAdjacent2DConnections = nonAdjacent2DConnections
            Obstacles = N_Obstacles.CreateEmpty
        }

    let create2D adjStruct =
        create [| 0 |] (fun () -> adjStruct)