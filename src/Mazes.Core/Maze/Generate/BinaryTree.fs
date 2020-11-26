// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

module Mazes.Core.Maze.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Canvas
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze

let private carveRow
    // params
    direction1
    direction2
    (rng : Random)
    rngTotalWeight    
    rngDirection1Weight
    grid
    rowIndex
    startColumnIndex
    increment
    endColumnIndex =    
    
    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }
        
        // if the cell is not part of the maze, we do nothing
        if not (isPartOfMaze grid.Canvas coordinate) then ()
        else

        let isDir1ALimit = (isALimitAt grid coordinate direction1)
        let isDir2ALimit = (isALimitAt grid coordinate direction2)
        
        let ifNotAtLimitUpdateWallAtPosition = ifNotAtLimitUpdateWallAtPosition grid coordinate

        // if we are in a corner 
        if isDir1ALimit &&  isDir2ALimit then
            ifNotAtLimitUpdateWallAtPosition direction1.Opposite Empty
            ifNotAtLimitUpdateWallAtPosition direction2.Opposite Empty
        else

        let updateWallAtPosition = updateWallAtPosition grid coordinate

        // if the dir 1 is a limit then we always choose remove dir 2 (and the opposite dir 2 if possible)
        if isDir1ALimit then
            updateWallAtPosition direction2 Empty
            ifNotAtLimitUpdateWallAtPosition direction2.Opposite Empty
        else

        // if the dir 2 is a limit then we always choose remove dir 1 (and the opposite dir 1 if possible)
        if isDir2ALimit then
            updateWallAtPosition direction1 Empty
            ifNotAtLimitUpdateWallAtPosition direction1.Opposite Empty
        else

        // if dir 1 and dir 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngDirection1Weight ->
            updateWallAtPosition direction1 Empty
        | _ ->
            updateWallAtPosition direction2 Empty

let createMaze direction1 direction2 rng rngDirection1Weight rngDirection2Weight grid =
    
    let (startColumnIndex, increment, endColumnIndex) =
        match direction1, direction2 with
        | _, Left | Left, _ -> (getIndex grid.Canvas.NumberOfColumns, -1, 0)
        | _ -> (0, 1, getIndex grid.Canvas.NumberOfColumns)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.Cells
    |> extractByRows
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection1Weight
            grid
            rowIndex
            startColumnIndex
            increment
            endColumnIndex)    

    { Grid = grid }