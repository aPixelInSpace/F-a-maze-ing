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
            OrthoCell.listOfPossiblePositionsCoordinates coordinate
    
    let connectionsStates c =
        match c with
        | OrthoCellChoice c ->
            OrthoCell.value c
            |> Array.map(fun c -> c.State)

    let neighborCoordinateAt c coordinate position =
        match c with
        | OrthoCellChoice _ ->
            OrthoCell.neighborCoordinateAt coordinate position

    let connectionStateAtPosition c position =
        match c, position with
        | OrthoCellChoice c, DispositionArray2D.Orthogonal p -> OrthoCell.connectionStateAtPosition c p

    let neighborPositionAt c coordinate otherCoordinate =
        snd
            ((listOfPossibleCoordinate c coordinate)
            |> Array.find(fun pc -> (fst pc) = otherCoordinate))

    let newCellWithStateAtPosition cell connectionState position =
        match cell, position with
        | OrthoCellChoice cell, DispositionArray2D.Orthogonal p ->
            OrthoCell.newCellWithStateAtPosition cell connectionState p
            |> OrthoCellChoice