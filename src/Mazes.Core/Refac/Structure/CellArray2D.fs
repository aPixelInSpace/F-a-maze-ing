// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

[<Struct>]
type CellArray2D =
    | OrthoCellChoice of OrthoCell

module CellArray2D =
    
    let listOfPossibleCoordinate c coordinate =
        match c with
        | OrthoCellChoice _ ->
            OrthoCellM.listOfPossiblePositionsCoordinates coordinate
    
    let connectionsStates c =
        match c with
        | OrthoCellChoice c ->
            OrthoCellM.value c
            |> Array.map(fun c -> c.State)

    let neighborCoordinateAt c coordinate position =
        match c with
        | OrthoCellChoice _ ->
            OrthoCellM.neighborCoordinateAt coordinate position

    let create isCellPartOfMaze disposition numberOfRows numberOfColumns internalConnectionState coordinate =
        match disposition with
        | GridArray2DType.Orthogonal ->
            OrthoCellM.create isCellPartOfMaze numberOfRows numberOfColumns internalConnectionState coordinate |> OrthoCellChoice

    let connectionStateAtPosition c position =
        match c, position with
        | OrthoCellChoice c, DispositionArray2D.Orthogonal p -> OrthoCellM.connectionStateAtPosition c p

    let neighborPositionAt c coordinate otherCoordinate =
        snd
            ((listOfPossibleCoordinate c coordinate)
            |> Array.find(fun pc -> (fst pc) = otherCoordinate))

    let newCellWithStateAtPosition cell connectionState position =
        match cell, position with
        | OrthoCellChoice cell, DispositionArray2D.Orthogonal p ->
            OrthoCellM.newCellWithStateAtPosition cell connectionState p
            |> OrthoCellChoice