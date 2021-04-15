// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

type Grid =
    | GridArray2DChoice of GridArray2D
    | GridArrayOfAChoice of GridArrayOfA

module Grid =

    let totalOfMazeCells g =
        match g with
        | GridArray2DChoice g -> GridArray2D.totalOfMazeCells g

    let existAt g =
        match g with
        | GridArray2DChoice g -> GridArray2D.existAt g

    let coordinatesPartOfMaze g =
        match g with
        | GridArray2DChoice g -> GridArray2D.coordinatesPartOfMaze g

    let isCellConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2D.isCellConnected g

    let neighbors g =
        match g with
        | GridArray2DChoice g -> GridArray2D.neighbors g

    let areConnected g =
        match g with
        | GridArray2DChoice g -> GridArray2D.areConnected g

    let updateConnectionState g =
        match g with
        | GridArray2DChoice g -> GridArray2D.updateConnectionState g

    let isLimitAtCoordinate g =
        match g with
        | GridArray2DChoice g -> GridArray2D.isLimitAtCoordinate g