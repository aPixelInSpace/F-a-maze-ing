// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

module GridM =

    let totalOfMazeCells g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.totalOfMazeCells g

    let existAt g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.existAt g

    let coordinatesPartOfMaze g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.coordinatesPartOfMaze g

    let isCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.isCellPartOfMaze g

    let isCellConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.isCellConnected g

    let neighbors g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.neighbors g

    let areConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.areConnected g

    let updateConnectionState g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.updateConnectionState g

    let isLimitAtCoordinate g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.isLimitAtCoordinate g

    let openCell g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.openCell g

    let weaveCoordinates g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.weaveCoordinates g

    let dimension1Boundaries g _ =
        match g with
        | GridArray2DChoice g -> GridArray2DM.dimension1Boundaries g

    let dimension2Boundaries g _ =
        match g with
        | GridArray2DChoice g -> GridArray2DM.dimension2Boundaries g

    let firstCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> snd (GridArray2DM.firstCellPartOfMaze g)

    let lastCellPartOfMaze g =
        match g with
        | GridArray2DChoice g -> snd (GridArray2DM.lastCellPartOfMaze g)

    let toString g =
        match g with
        | GridArray2DChoice g -> GridArray2DM.toString g