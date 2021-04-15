// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

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
        |> Seq.sumBy(fun kv -> Grid.totalOfMazeCells kv.Value)

    let existAt n (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.Dimension
        if n.Structure.ContainsKey(dimension) then
            Grid.existAt (slice2D n dimension) nCoordinate.Coordinate2D
        else
            false

    let coordinatesPartOfMaze n =
        let cells dimension grid =
            Grid.coordinatesPartOfMaze grid
            |> Seq.map(NCoordinate.create dimension)

        n.Structure
        |> Seq.collect(fun kv -> cells kv.Key kv.Value)

    let randomCoordinatePartOfMazeAndNotConnected n (rng : Random) =
        let unconnectedPartOfMazeCells =
            coordinatesPartOfMaze n
            |> Seq.filter(fun c ->
                    let slice2d = slice2D n c.Dimension
                    not (Grid.isCellConnected slice2d c.Coordinate2D || CoordinateConnections.isCellConnected n.CoordinateConnections c))
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]

    let neighbors n (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.Dimension
        let slice2d = slice2D n dimension

        let neighbors2d =
            Grid.neighbors slice2d nCoordinate.Coordinate2D
            |> Seq.map(NCoordinate.create dimension)

        let neighbors =
            CoordinateConnections.neighbors n.CoordinateConnections nCoordinate
            |> Seq.map(fst)

        neighbors2d |> Seq.append neighbors

    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    let isCellConnected n nCoordinate =
        CoordinateConnections.isCellConnected n.CoordinateConnections nCoordinate ||
        Grid.isCellConnected (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D

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
            Grid.areConnected (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D

        nonAdjacent2DCondition || adjacent2DCondition

    /// Returns the neighbors coordinates that are or not connected NOT NECESSARILY WITH the coordinate
    let connectedNeighbors n isConnected (nCoordinate : NCoordinate) =
        neighbors n nCoordinate
        |> Seq.filter(fun neighbor ->
            let isConnectedAdjacent2d = Grid.isCellConnected (slice2D n neighbor.Dimension) neighbor.Coordinate2D
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

    let updateConnection n connectionType nCoordinate otherNCoordinate =
        if CoordinateConnections.existNeighbor n.CoordinateConnections nCoordinate otherNCoordinate then
            CoordinateConnections.updateConnectionState n.CoordinateConnections connectionType nCoordinate otherNCoordinate
        elif NCoordinate.isSame2DDimension nCoordinate otherNCoordinate then
            Grid.updateConnectionState (slice2D n nCoordinate.Dimension) connectionType nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D

    let ifNotAtLimitUpdateConnection n connectionType nCoordinate otherNCoordinate =
        if (CoordinateConnections.existNeighbor n.CoordinateConnections nCoordinate otherNCoordinate) ||
           ((NCoordinate.isSame2DDimension nCoordinate otherNCoordinate) &&
            not (Grid.isLimitAtCoordinate (slice2D n nCoordinate.Dimension) nCoordinate.Coordinate2D otherNCoordinate.Coordinate2D)) then

            updateConnection n connectionType nCoordinate otherNCoordinate