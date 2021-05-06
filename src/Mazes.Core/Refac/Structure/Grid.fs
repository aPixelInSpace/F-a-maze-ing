// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type Grid =
    | GridArray2DChoice of GridArray2D
    | GridArrayOfAChoice of GridArrayOfA

module Grid =

    let gridStructure g =
        match g with
        | GridArray2DChoice g -> GridArray2D.gridStructure g |> GridStructureArray2D

    let rIndexes g =
        match g with
        | GridArray2DChoice g -> GridArray2D.rIndexes g

    let cIndexes g =
        match g with
        | GridArray2DChoice g -> GridArray2D.cIndexes g

    let totalOfMazeCells g =
        match g with
        | GridArray2DChoice g -> GridArray2D.totalOfMazeCells g

    let existAt g =
        match g with
        | GridArray2DChoice g -> GridArray2D.existAt g

    let adjustedExistAt g =
        match g with
        | GridArray2DChoice g -> GridArray2D.existAt g
        | GridArrayOfAChoice g -> GridArrayOfA.adjustedExistAt g

    let coordinatesPartOfMaze g =
        match g with
        | GridArray2DChoice g -> GridArray2D.coordinatesPartOfMaze g

    let isCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> GridArray2D.isCellPartOfMaze g

    let isCellConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2D.isCellConnected g

    let dispositions g coordinate =
        match g with
        | GridArray2DChoice g -> GridArray2D.dispositions g coordinate

    let neighbor g coordinate pos =
        match g, pos with
        | GridArray2DChoice g, DispositionArray2D pos -> GridArray2D.neighbor g coordinate pos

    let neighbors g =
        match g with
        | GridArray2DChoice g -> GridArray2D.neighbors g

    let areConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2D.areConnected g

    let updateConnectionState g =
        match g with
        | GridArray2DChoice g -> GridArray2D.updateConnectionState g

    let ifNotAtLimitUpdateConnectionState g =
        match g with
        | GridArray2DChoice g -> GridArray2D.ifNotAtLimitUpdateConnectionState g

    let connectionStateAtPosition g coordinate position =
        match g, position with
        | GridArray2DChoice g, DispositionArray2D position -> GridArray2D.connectionStateAtPosition g coordinate position

    let isLimitAtCoordinate g =
        match g with
        | GridArray2DChoice g -> GridArray2D.isLimitAtCoordinate g

    let openCell g =
        match g with
        | GridArray2DChoice g -> GridArray2D.openCell g

    let weaveCoordinates g =
        match g with
        | GridArray2DChoice g -> GridArray2D.weaveCoordinates g

    let adjustedCoordinate g coordinate =
        match g with
        | GridArray2DChoice _ -> coordinate
        | GridArrayOfAChoice g -> GridArrayOfA.adjustedCoordinate g coordinate

    let rawNeighborCoordinate g coordinate disposition =
        match g, disposition with
        | GridArray2DChoice g, DispositionArray2D d -> GridArray2D.neighbor g coordinate d
        | GridArrayOfAChoice g, DispositionArrayOfA d -> GridArrayOfA.virtualNeighbor g coordinate d

    let dimension1Boundaries g _ =
        match g with
        | GridArray2DChoice g -> GridArray2D.dimension1Boundaries g

    let dimension2Boundaries g _ =
        match g with
        | GridArray2DChoice g -> GridArray2D.dimension2Boundaries g

    let numberOfRows g =
        match g with
        | GridArray2DChoice g -> GridArray2D.numberOfRows g

    let numberOfColumns g =
        match g with
        | GridArray2DChoice g -> GridArray2D.numberOfColumns g

    let firstCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> snd (GridArray2D.firstCellPartOfMaze g)

    let lastCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> snd (GridArray2D.lastCellPartOfMaze g)

    let randomCoordinatePartOfMazeAndNotConnected g (rng : Random) =
        let unconnectedPartOfMazeCells =
            coordinatesPartOfMaze g
            |> Seq.filter(fun c ->
                not (isCellConnected g c))
            |> Seq.toArray

        unconnectedPartOfMazeCells.[rng.Next(unconnectedPartOfMazeCells.Length)]        

    let createBaseGrid gridType canvas =
        match gridType with
        | GridStructureArray2D g -> GridArray2D.createBaseGrid g canvas |> GridArray2DChoice

    let createEmptyBaseGrid gridType canvas =
        match gridType with
        | GridStructureArray2D g -> GridArray2D.createEmptyBaseGrid g canvas |> GridArray2DChoice

    let toString g =
        match g with
        | GridArray2DChoice g -> GridArray2D.toString g