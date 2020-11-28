// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze

type Direction =
    | Top
    | Right
    | Bottom
    | Left

let private getRandomColumnIndexFromRange isALimitAt (rng : Random) increment position rowIndex startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableWallAtPos = ResizeArray<int>()

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do
        if not (isALimitAt { RowIndex = rowIndex; ColumnIndex = columnIndex } position) then
            eligibleCellsWithRemovableWallAtPos.Add(columnIndex)
    
    if eligibleCellsWithRemovableWallAtPos.Count > 0 then
        Some eligibleCellsWithRemovableWallAtPos.[rng.Next(0, eligibleCellsWithRemovableWallAtPos.Count - 1)]
    else
        None

let private carveRow
    // dependencies    
    isPartOfMaze
    isALimitAt
    updateWallAtPosition
    ifNotAtLimitUpdateWallAtPosition
    getRandomColumnIndexAtPos1ForFromRange
    // params
    (position1 : Position)
    (position2 : Position)
    (rng : Random)
    rngTotalWeight    
    rngDirection2Weight
    rowIndex
    startColumnIndex
    increment
    endColumnIndex =

    let mutable runStartIndex = startColumnIndex
    let mutable lastColumnIndexWithRemovableDir2Wall = startColumnIndex

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }

        // if the cell is not part of the maze, we only update the run start index
        if not (isPartOfMaze coordinate) then
            runStartIndex <- runStartIndex + increment
            lastColumnIndexWithRemovableDir2Wall <- lastColumnIndexWithRemovableDir2Wall + increment
        else

        let isDir1ALimit = (isALimitAt coordinate position1)
        let isDir2ALimit = (isALimitAt coordinate position2)

        // if we are in a corner
        if isDir1ALimit && isDir2ALimit then
            // we check which of the previous cells have a wall at the dir 1 that can be removed
            let randomColumnIndex = getRandomColumnIndexAtPos1ForFromRange runStartIndex (columnIndex - increment)

            match randomColumnIndex with
            | Some randomColumnIndex ->
                // if there is some we remove it
                updateWallAtPosition { coordinate with ColumnIndex = randomColumnIndex } position1 Empty
            | None ->
                // we absolutely have to ensure that the last wall on the dir 2 is empty if possible
                ifNotAtLimitUpdateWallAtPosition { coordinate with ColumnIndex = lastColumnIndexWithRemovableDir2Wall } position2 Empty

            runStartIndex <- columnIndex + increment
        else

        // if the dir 1 is a limit then we always choose remove dir 2
        if isDir1ALimit then                
            updateWallAtPosition coordinate position2 Empty
            ifNotAtLimitUpdateWallAtPosition coordinate position2.Opposite Empty

            // we have to check whether there was some prior dir 1 wall to remove 
            let randomColumnIndex = getRandomColumnIndexAtPos1ForFromRange runStartIndex (columnIndex - increment)
            match randomColumnIndex with
            | Some columnIndexForDir1Removal ->                
                updateWallAtPosition { coordinate with ColumnIndex = columnIndexForDir1Removal } position1 Empty
                lastColumnIndexWithRemovableDir2Wall <- columnIndex
            | None -> ()
            
            runStartIndex <- columnIndex + increment
        else

        // if the dir 2 is a limit then we always choose to randomly remove one of the dir 1 of the run
        if isDir2ALimit then
            updateWallAtPosition { coordinate with ColumnIndex = (rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)) } position1 Empty
            runStartIndex <- columnIndex + increment
        else

        // if dir 1 and dir 2 are both not a limit, we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with

        // we continue carving to the dir 2
        | rng when rng < rngDirection2Weight -> updateWallAtPosition coordinate position2 Empty

        // or we open to the dir 1 by choosing randomly one of the dir 1 wall
        | _ -> 
           let randomColumnIndex = rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)
           updateWallAtPosition { coordinate with ColumnIndex = randomColumnIndex } position1 Empty
           lastColumnIndexWithRemovableDir2Wall <- columnIndex
           runStartIndex <- columnIndex + increment        

let mapDirectionToPosition direction =
    match direction with
    | Top -> Position.Top
    | Right -> Position.Right
    | Bottom -> Position.Bottom
    | Left -> Position.Left

type private mode =
    | ByRows
    | ByColumns

let createMaze (direction1 : Direction) (direction2 : Direction) rng rngDirection1Weight rngDirection2Weight grid =    

    let getCoordinate mode coordinate =
        match mode with
        | ByRows ->
            coordinate
        | ByColumns ->
            { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  }

    let (extractBy, startColumnIndex, increment, endColumnIndex, getCoordinate) =
        match direction1, direction2 with
        | _, Right -> (extractByRows, 0, 1, getIndex grid.Canvas.NumberOfColumns, getCoordinate ByRows)
        | _, Left -> (extractByRows, getIndex grid.Canvas.NumberOfColumns, -1, 0, getCoordinate ByRows)
        | _, Top -> (extractByColumns, getIndex grid.Canvas.NumberOfRows, -1, 0, getCoordinate ByColumns)
        | _, Bottom -> (extractByColumns, 0, 1, getIndex grid.Canvas.NumberOfRows, getCoordinate ByColumns)

    let position1 = mapDirectionToPosition direction1
    let position2 = mapDirectionToPosition direction2

    let isPartOfMaze coordinate = (grid.Canvas.IsZonePartOfMaze (getCoordinate coordinate))
    let isALimitAt coordinate = (isALimitAt grid (getCoordinate coordinate))
    let updateWallAtPosition coordinate = (updateWallAtPosition grid (getCoordinate coordinate))
    let ifNotAtLimitUpdateWallAtPosition coordinate = (ifNotAtLimitUpdateWallAtPosition grid (getCoordinate coordinate))
    let getRandomColumnIndexFromRange = (getRandomColumnIndexFromRange isALimitAt rng increment position1)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.Cells
    |> extractBy
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // dependencies
            isPartOfMaze
            isALimitAt
            updateWallAtPosition
            ifNotAtLimitUpdateWallAtPosition
            (getRandomColumnIndexFromRange rowIndex)
            // params
            position1
            position2
            rng
            rngTotalWeight
            rngDirection2Weight
            rowIndex
            startColumnIndex
            increment
            endColumnIndex)

    { Grid = grid}