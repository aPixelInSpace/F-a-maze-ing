// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.BinaryTree

open System
open Mazes.Core.Refac
open Mazes.Core.Refac.Array2D
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Structure.Grid
open Mazes.Core.Refac.Maze

type BinaryTreeDirection =
    | Top
    | Right
    | Bottom
    | Left

    member this.Opposite =
        match this with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right

let toDisposition g spiralDirection =
    match gridStructure g with
    | GridStructureArray2D gridStructure ->
        match gridStructure with
        | GridArray2DOrthogonal _ ->
            match spiralDirection with
            | Right -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Right)
            | Bottom -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Bottom)
            | Left -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Left)
            | Top -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Top)

let private carveRow
    // params
    direction1
    direction2
    (rng : Random)
    rngTotalWeight    
    rngPosition1Weight
    grid
    rIndex
    getRowInfo =
    
    let startColumnIndex, increment, endColumnIndex = getRowInfo
    
    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RIndex = rIndex; CIndex = columnIndex }

        let connect = updateConnectionState grid Open
        let disposition1 = lazy toDisposition grid direction1
        let disposition1Opposite = lazy toDisposition grid direction1.Opposite
        let disposition2 = lazy toDisposition grid direction2
        let disposition2Opposite = lazy toDisposition grid direction2.Opposite

        let neighborCoordinate position =
            match (neighbor grid coordinate position) with
            | Some neighbor -> neighbor
            | None -> failwith "Binary Tree, unable to find the neighbor coordinate"

        let isPosALimit position =
            (neighbor grid coordinate position).IsNone || (isLimitAtCoordinate grid coordinate (neighborCoordinate position))

        let ifNotAtLimitConnectCells position =
            if not (isPosALimit position) then
                connect coordinate (neighborCoordinate position)

        // if the cell is not part of the maze, we do nothing
        if not (isCellPartOfMaze grid coordinate) then ()
        else

        let isPos1ALimit = isPosALimit disposition1.Value
        let isPos2ALimit = isPosALimit disposition2.Value

        // if we are in a corner 
        if isPos1ALimit &&  isPos2ALimit then
            ifNotAtLimitConnectCells disposition1Opposite.Value
            ifNotAtLimitConnectCells disposition2Opposite.Value
        else

        // if the pos 1 is a limit then we always choose remove pos 2 (and the opposite pos 2 if possible)
        if isPos1ALimit then
            connect coordinate (neighborCoordinate disposition2.Value)
            ifNotAtLimitConnectCells disposition2Opposite.Value
        else

        // if the pos 2 is a limit then we always choose remove pos 1 (and the opposite pos 1 if possible)
        if isPos2ALimit then
            connect coordinate (neighborCoordinate disposition1.Value)
            ifNotAtLimitConnectCells disposition1Opposite.Value
        else

        // if pos 1 and pos 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngPosition1Weight ->
            connect coordinate (neighborCoordinate disposition1.Value)
        | _ ->
            connect coordinate (neighborCoordinate disposition2.Value)

let createMaze direction1 direction2 rngSeed rngDirection1Weight rngDirection2Weight ndStruct =

    let slice2D = snd (NDimensionalStructure.firstSlice2D ndStruct)

    let rng = Random(rngSeed)

    let getRowInfo rowIndex =
        let startIndex, length = dimension2Boundaries slice2D rowIndex
        match direction1, direction2 with
        | _, Left | Left, _ -> (getIndex length , -1, startIndex)
        | _ -> (startIndex, 1, getIndex length)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    rIndexes slice2D
    |> Seq.iter(fun rIndex ->
        carveRow
            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection1Weight
            slice2D
            rIndex
            (getRowInfo rIndex))    

    { NDStruct = ndStruct }