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
            |> Seq.map(fun (c, p) -> (c, DispositionArray2D.Orthogonal p))
    
    let connectionsState c =
        match c with
        | OrthoCellChoice c ->
            OrthoCell.value c
            |> Array.map(fun c -> c.State)

    let neighborCoordinateAt coordinate position =
        match position with
        | DispositionArray2D.Orthogonal position -> OrthoCell.neighborCoordinateAt coordinate position
        | _ -> failwith "not implemented yet"

    let connectionStateAtPosition c position =
        match c, position with
        | OrthoCellChoice c, DispositionArray2D.Orthogonal p -> OrthoCell.connectionStateAtPosition c p
        | _ -> failwith "not implemented yet"

    let neighborPositionAt c coordinate otherCoordinate =        
        (match c with
        | OrthoCellChoice _ -> OrthoCell.neighborPositionAt coordinate otherCoordinate)
        
    let cellWithStateAtPosition cell connectionState position =
        match cell, position with
        | OrthoCellChoice cell, DispositionArray2D.Orthogonal p ->
            OrthoCell.cellWithStateAtPosition cell connectionState p
            |> OrthoCellChoice
        | _ -> failwith "not implemented yet"