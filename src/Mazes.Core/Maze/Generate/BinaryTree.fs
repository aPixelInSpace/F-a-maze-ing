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
        
        let ifNotAtLimitLinkCellAtPosition = grid.IfNotAtLimitLinkCellAtPosition coordinate

        // if we are in a corner 
        if isPos1ALimit &&  isPos2ALimit then
            ifNotAtLimitLinkCellAtPosition position1.Opposite
            ifNotAtLimitLinkCellAtPosition position2.Opposite
        else

        let linkCellAtPosition = grid.LinkCellAtPosition coordinate

        // if the pos 1 is a limit then we always choose remove pos 2 (and the opposite pos 2 if possible)
        if isPos1ALimit then
            linkCellAtPosition position2
            ifNotAtLimitLinkCellAtPosition position2.Opposite
        else

        // if the pos 2 is a limit then we always choose remove pos 1 (and the opposite pos 1 if possible)
        if isPos2ALimit then
            linkCellAtPosition position1
            ifNotAtLimitLinkCellAtPosition position1.Opposite
        else

        // if pos 1 and pos 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngPosition1Weight ->
            linkCellAtPosition position1
        | _ ->
            linkCellAtPosition position2

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