// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

let private carveRow
    // params
    position1
    position2
    (rng : Random)
    rngTotalWeight    
    rngPosition1Weight
    grid
    rowIndex
    startColumnIndex
    increment
    endColumnIndex =    
    
    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
        
        // if the cell is not part of the maze, we do nothing
        if not (grid.Canvas.IsZonePartOfMaze coordinate) then ()
        else

        let isPos1ALimit = (grid.IsLimitAt coordinate position1)
        let isPos2ALimit = (grid.IsLimitAt coordinate position2)
        
        let ifNotAtLimitUpdateWallAtPosition = grid.IfNotAtLimitUpdateWallAtPosition coordinate

        // if we are in a corner 
        if isPos1ALimit &&  isPos2ALimit then
            ifNotAtLimitUpdateWallAtPosition position1.Opposite Empty
            ifNotAtLimitUpdateWallAtPosition position2.Opposite Empty
        else

        let updateWallAtPosition = grid.UpdateWallAtPosition coordinate

        // if the pos 1 is a limit then we always choose remove pos 2 (and the opposite pos 2 if possible)
        if isPos1ALimit then
            updateWallAtPosition position2 Empty
            ifNotAtLimitUpdateWallAtPosition position2.Opposite Empty
        else

        // if the pos 2 is a limit then we always choose remove pos 1 (and the opposite pos 1 if possible)
        if isPos2ALimit then
            updateWallAtPosition position1 Empty
            ifNotAtLimitUpdateWallAtPosition position1.Opposite Empty
        else

        // if pos 1 and pos 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngPosition1Weight ->
            updateWallAtPosition position1 Empty
        | _ ->
            updateWallAtPosition position2 Empty

type Direction =
    | Top
    | Right
    | Bottom
    | Left

let mapDirectionToPosition direction =
    match direction with
    | Top -> Position.Top
    | Right -> Position.Right
    | Bottom -> Position.Bottom
    | Left -> Position.Left

let createMaze direction1 direction2 rngSeed rngDirection1Weight rngDirection2Weight grid =

    let rng = Random(rngSeed)

    let (startColumnIndex, increment, endColumnIndex) =
        match direction1, direction2 with
        | _, Left | Left, _ -> (getIndex grid.Canvas.NumberOfColumns, -1, 0)
        | _ -> (0, 1, getIndex grid.Canvas.NumberOfColumns)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    let position1 = mapDirectionToPosition direction1
    let position2 = mapDirectionToPosition direction2

    grid.Cells
    |> extractByRows
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // params
            position1
            position2
            rng
            rngTotalWeight
            rngDirection1Weight
            grid
            rowIndex
            startColumnIndex
            increment
            endColumnIndex)    

    { Grid = grid }