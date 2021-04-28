// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

module CellArray2DM =
    
    let listOfPossibleCoordinate disposition coordinate =
        match disposition with
        | GridArray2DOrthogonal _ ->
            OrthoCellM.listOfPossiblePositionsCoordinates coordinate

    let dispositions c coordinate =
        match c with
        | OrthoCellChoice _ -> OrthoCellM.dispositions |> Seq.map(OrthogonalDisposition)

    let connectionsStates c =
        match c with
        | OrthoCellChoice c ->
            OrthoCellM.value c
            |> Array.map(fun c -> c.State)

    let neighborCoordinateAt disposition coordinate position =
        listOfPossibleCoordinate disposition coordinate
        |> Array.tryFind(fun pc -> (snd pc) = position)
        |> Option.map(fst)

    let initialize isCellPartOfMaze disposition numberOfRows numberOfColumns internalConnectionState coordinate =
        let param = (isCellPartOfMaze, (neighborCoordinateAt disposition), numberOfRows, numberOfColumns, internalConnectionState, coordinate)

        match disposition with
        | GridArray2DOrthogonal _ ->
            OrthoCellM.initialize param |> OrthoCellChoice

    let connectionStateAtPositionGeneric<'Position when 'Position : equality> connections position =
        (connections
        |> Seq.find(fun c ->  c.Position = position)).State

    let connectionStateAtPosition c position =
        match c, position with
        | OrthoCellChoice c, DispositionArray2D.OrthogonalDisposition p -> connectionStateAtPositionGeneric<OrthogonalDisposition> (OrthoCellM.value c) p

    let neighborPositionAt c coordinate otherCoordinate =
        snd
            ((listOfPossibleCoordinate c coordinate)
            |> Array.find(fun pc -> (fst pc) = otherCoordinate))

    let newCellWithStateAtPositionGeneric<'Position when 'Position : equality> connections connectionState position =
         connections
         |> Seq.map(fun c -> if c.Position = position then { State = connectionState; Position = position } else c)

    let newCellWithStateAtPosition cell connectionState position =
        match cell, position with
        | OrthoCellChoice cell, DispositionArray2D.OrthogonalDisposition p ->
            newCellWithStateAtPositionGeneric<OrthogonalDisposition> (OrthoCellM.value cell) connectionState p
            |> Seq.toArray |> OrthoCell |> OrthoCellChoice