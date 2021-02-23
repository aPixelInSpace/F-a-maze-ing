// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Array2D

type Direction =
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

    member this.Position =
        match this with
        | Top -> Position.Top
        | Right -> Position.Right
        | Bottom -> Position.Bottom
        | Left -> Position.Left

    

let private carveRow
    // params
    (direction1 : Direction)
    (direction2 : Direction)
    (rng : Random)
    rngTotalWeight    
    rngPosition1Weight
    (grid : Grid.IAdjacentStructure<_,_>)
    rIndex
    getRowInfo =
    
    let (startColumnIndex, increment, endColumnIndex) = getRowInfo
    
    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RIndex = rIndex; CIndex = columnIndex }

        let neighborCoordinate position =
            match (grid.Neighbor coordinate position) with
            | Some neighbor -> neighbor
            | None -> failwith "Binary Tree, unable to find the neighbor coordinate"

        let isPosALimit position = ((grid.Neighbor coordinate position).IsNone) || (grid.IsLimitAt coordinate (neighborCoordinate position))
        let ifNotAtLimitLinkCells position =
            if not (isPosALimit position) then
                grid.UpdateConnection Open coordinate (neighborCoordinate position)
        
        // if the cell is not part of the maze, we do nothing
        if not (grid.IsCellPartOfMaze coordinate) then ()
        else

        let isPos1ALimit = isPosALimit direction1.Position
        let isPos2ALimit = isPosALimit direction2.Position

        // if we are in a corner 
        if isPos1ALimit &&  isPos2ALimit then
            ifNotAtLimitLinkCells direction1.Opposite.Position
            ifNotAtLimitLinkCells direction2.Opposite.Position
        else

        // if the pos 1 is a limit then we always choose remove pos 2 (and the opposite pos 2 if possible)
        if isPos1ALimit then
            grid.UpdateConnection Open coordinate (neighborCoordinate direction2.Position)
            ifNotAtLimitLinkCells direction2.Opposite.Position
        else

        // if the pos 2 is a limit then we always choose remove pos 1 (and the opposite pos 1 if possible)
        if isPos2ALimit then
            grid.UpdateConnection Open coordinate (neighborCoordinate direction1.Position)
            ifNotAtLimitLinkCells direction1.Opposite.Position
        else

        // if pos 1 and pos 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngPosition1Weight ->
            grid.UpdateConnection Open coordinate (neighborCoordinate direction1.Position)
        | _ ->
            grid.UpdateConnection Open coordinate (neighborCoordinate direction2.Position)

let createMaze direction1 direction2 rngSeed rngDirection1Weight rngDirection2Weight (grid : Grid.IAdjacentStructure<_,_>) : Maze.Maze<_> =

    let rng = Random(rngSeed)

    let getRowInfo rowIndex =
        let (startIndex, length) = grid.Dimension2Boundaries rowIndex
        match direction1, direction2 with
        | _, Left | Left, _ -> (getIndex length , -1, startIndex)
        | _ -> (startIndex, 1, getIndex length)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.RIndexes
    |> Seq.iter(fun rIndex ->
        carveRow
            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection1Weight
            grid
            rIndex
            (getRowInfo rIndex))    

    { Grid = (Grid.Grid.create grid) }