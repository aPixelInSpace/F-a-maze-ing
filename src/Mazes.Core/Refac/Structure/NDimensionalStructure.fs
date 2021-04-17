// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open System.Collections.Generic
open Mazes.Core.Refac

type NDimensionalStructure =
    private
        {
            Structure : Dictionary<Dimension, Grid>
            CoordinateConnections : CoordinateConnections
            Obstacles : Obstacles
        }

module NDimensionalStructure =

    let slice2D n dimension =
        n.Structure.Item(dimension)

    let firstSlice2D n =
        let firstDimension =
            n.Structure.Keys
            |> Seq.sort
            |> Seq.head

        (firstDimension, slice2D n firstDimension)

    let totalOfMazeCells n =
        n.Structure
        |> Seq.sumBy(fun kv -> GridM.totalOfMazeCells kv.Value)

    let existAt n (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.Dimension
        if n.Structure.ContainsKey(dimension) then
            GridM.existAt (slice2D n dimension) nCoordinate.Coordinate2D
        else
            false

    let coordinatesPartOfMaze n =
        let cells dimension grid =
            GridM.coordinatesPartOfMaze grid
            |> Seq.map(NCoordinate.create dimension)

        n.Structure
        |> Seq.collect(fun kv -> cells kv.Key kv.Value)

    let randomCoordinatePartOfMazeAndNotConnected n (rng : Random) =
        let unconnectedPartOfMazeCells =
            coordinatesPartOfMaze n
            |> Seq.filter(fun c ->
                    let slice2d = slice2D n c.Dimension
                    not (GridM.isCellConnected slice2d c.Coordinate2D || CoordinateConnections.isCellConnected n.CoordinateConnections c))
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

    let neighbors n (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.Dimension
        let slice2d = slice2D n dimension

        let neighbors2d =
            GridM.neighbors slice2d nCoordinate.Coordinate2D
            |> Seq.map(NCoordinate.create dimension)

        let neighbors =
            CoordinateConnections.neighbors n.CoordinateConnections nCoordinate
            |> Seq.map(fst)

        neighbors2d |> Seq.append neighbors

    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    let isCellConnected n nCoordinate =
        CoordinateConnections.isCellConnected n.CoordinateConnections nCoordinate ||
        GridM.isCellConnected (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D

    /// Given two coordinates, returns true if they have their connection open, false otherwise
    let areConnected n nCoordinate otherNCoordinate =
        let existNonAdjacentNeighbor = CoordinateConnections.existNeighbor n.CoordinateConnections nCoordinate otherNCoordinate

        let nonAdjacent2DCondition =
            if existNonAdjacentNeighbor then
                CoordinateConnections.areConnected n.CoordinateConnections nCoordinate otherNCoordinate
            else
                false
        
        let adjacent2DCondition =
            not existNonAdjacentNeighbor &&
            (NCoordinate.isSame2DDimension nCoordinate otherNCoordinate) &&
            GridM.areConnected (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D

        nonAdjacent2DCondition || adjacent2DCondition

    /// Returns the neighbors coordinates that are or not connected NOT NECESSARILY WITH the coordinate
    let connectedNeighbors n isConnected (nCoordinate : NCoordinate) =
        neighbors n nCoordinate
        |> Seq.filter(fun neighbor ->
            let isConnectedAdjacent2d = GridM.isCellConnected (slice2D n neighbor.Dimension) neighbor.Coordinate2D
            let isConnectedNonAdjacent2d = CoordinateConnections.isCellConnected n.CoordinateConnections neighbor

            if isConnected then isConnectedAdjacent2d || isConnectedNonAdjacent2d
            else not isConnectedAdjacent2d && not isConnectedNonAdjacent2d)
        |> Seq.distinct

    /// Returns the neighbors coordinates that are or not connected WITH the coordinate
    let connectedWithNeighbors n connected nCoordinate =
        seq {
            for neighborCoordinate in (neighbors n nCoordinate) do
                let areConnected = areConnected n nCoordinate neighborCoordinate
                if (connected && areConnected) || (not connected && not areConnected) then
                    yield neighborCoordinate
        }

    let updateConnectionState n connectionState nCoordinate otherNCoordinate =
        if CoordinateConnections.existNeighbor n.CoordinateConnections nCoordinate otherNCoordinate then
            CoordinateConnections.updateConnectionState n.CoordinateConnections connectionState nCoordinate otherNCoordinate
        elif NCoordinate.isSame2DDimension nCoordinate otherNCoordinate then
            GridM.updateConnectionState (slice2D n nCoordinate.Dimension) connectionState nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D

    let ifNotAtLimitUpdateConnectionState n connectionState nCoordinate otherNCoordinate =
        if (CoordinateConnections.existNeighbor n.CoordinateConnections nCoordinate otherNCoordinate) ||
           ((NCoordinate.isSame2DDimension nCoordinate otherNCoordinate) &&
            not (GridM.isLimitAtCoordinate (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D)) then

            updateConnectionState n connectionState nCoordinate otherNCoordinate

    let weave n (rng : Random) weight =
        if weight > 0.0 then
            for slice2D in n.Structure do
                let dimension = slice2D.Key
                let adjStruct = slice2D.Value

                let weaveCoordinates = GridM.weaveCoordinates adjStruct (GridM.coordinatesPartOfMaze adjStruct)
                for fromCoordinate, toCoordinate in weaveCoordinates do
                    if (toCoordinate.RIndex >= fst (GridM.dimension1Boundaries adjStruct toCoordinate.CIndex)) &&
                       (toCoordinate.RIndex < snd (GridM.dimension1Boundaries adjStruct toCoordinate.CIndex)) &&
                       (toCoordinate.CIndex >= fst (GridM.dimension2Boundaries adjStruct toCoordinate.RIndex)) &&
                       (toCoordinate.CIndex < snd (GridM.dimension2Boundaries adjStruct toCoordinate.RIndex)) &&
                       GridM.isCellPartOfMaze adjStruct toCoordinate &&
                       rng.NextDouble() < weight
                       then

                        CoordinateConnections.updateConnectionState n.CoordinateConnections 
                            Close (NCoordinate.create dimension fromCoordinate) (NCoordinate.create dimension toCoordinate)

    let openCell n (nCoordinate : NCoordinate) =
        GridM.openCell (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D

    let costOfCoordinate n nCoordinate =
        (Obstacles.cost n.Obstacles nCoordinate) + (Cost 1)

    /// Returns the first (arbitrary) coordinate that is part of the maze
    let firstCellPartOfMaze n =
        let dimension =
            n.Structure.Keys
            |> Seq.sort
            |> Seq.find(fun d -> GridM.totalOfMazeCells (slice2D n d) > 0)

        NCoordinate.create dimension (GridM.firstCellPartOfMaze (slice2D n dimension))

    /// Returns the last (arbitrary) coordinate that is part of the maze
    let lastCellPartOfMaze n =
        let dimension =
            n.Structure.Keys
            |> Seq.sortDescending
            |> Seq.find(fun d -> GridM.totalOfMazeCells (slice2D n d) > 0)

        NCoordinate.create dimension (GridM.lastCellPartOfMaze (slice2D n dimension))

    let create (dimensions : Dimension) (newGridInstance : unit -> Grid) =
        let baseAdjStruct = newGridInstance()
        let coordinates2D = GridM.coordinatesPartOfMaze baseAdjStruct

        let dimensionsSeq =
            let nextDimension (dimension : Dimension) =
                let newDimension = Dimension.copy dimension
                let mutable isFound = false 
                for d in 0 .. Dimension.length newDimension - 1 do
                    if not isFound && (newDimension |> Dimension.valueAt d) < (dimensions |> Dimension.valueAt d) then
                        (Dimension.value newDimension).[d] <- (Dimension.value newDimension).[d] + 1
                        isFound <- true
                    elif not isFound then
                        (Dimension.value newDimension).[d] <- 0

                newDimension

            let countSlices2D = (1, Dimension.value dimensions) ||> Array.fold(fun count d -> (d + 1) * count)

            let mutable currentDimension = Dimension (Array.create (Dimension.value dimensions).Length 0)
            seq {
                yield currentDimension
                for _ in 1 .. countSlices2D - 1 do
                    currentDimension <- nextDimension currentDimension
                    yield currentDimension
            }

        let structure = Dictionary<Dimension, Grid>()
        for dimension in dimensionsSeq do
            structure.Add(dimension, newGridInstance())

        // orthogonally connect every slice 2d to the next one
        let coordinateConnections = CoordinateConnections.createEmpty()
        for d in 0 .. (Dimension.value dimensions).Length - 1 do
            let startDimensions = dimensionsSeq |> Seq.filter(fun dimension -> (Dimension.value dimension).[d] = 0)
            for startDimension in startDimensions do

                let currentDimensionIndex = ref 0
                let mutable currentDimension = startDimension
                while currentDimensionIndex.Value < (dimensions |> Dimension.valueAt d) do                    
                    incr currentDimensionIndex
                    let newDimension = Dimension.copy currentDimension
                    (Dimension.value newDimension).[d] <- (Dimension.value newDimension).[d] + 1

                    for coordinate2D in coordinates2D do
                        let nCoordinate = NCoordinate.create currentDimension coordinate2D
                        let nOtherCoordinate = NCoordinate.create newDimension coordinate2D
                        CoordinateConnections.updateConnectionState coordinateConnections Close nCoordinate nOtherCoordinate

                    currentDimension <- newDimension
        {
            Structure = structure
            CoordinateConnections = coordinateConnections
            Obstacles = Obstacles.createEmpty()
        }

    let create2D adjStruct =
        create (Dimension [| 0 |]) (fun () -> adjStruct)

    let create3D numberOfLevels adjStruct =
        create (Dimension [| numberOfLevels - 1 |]) adjStruct