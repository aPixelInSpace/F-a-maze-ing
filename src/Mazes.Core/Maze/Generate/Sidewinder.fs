// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Position
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze

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
    linkCellAtPosition
    ifNotAtLimitLinkCellAtPosition
    getRandomIndex2AtPos1ForFromRange
    // params
    (position1 : Position)
    (position2 : Position)
    (rng : Random)
    rngTotalWeight    
    rngPosition2Weight
    index1
    startIndex2
    increment
    endIndex2 =

    let mutable runStartIndex2 = startIndex2
    let mutable lastIndex2WithRemovablePos2Wall = startIndex2

    for index2 in startIndex2 .. increment .. endIndex2 do

        let coordinate = { RowIndex = index1; ColumnIndex = index2 }

        // if the cell is not part of the maze, we only update the run start index
        if not (isPartOfMaze coordinate) then
            runStartIndex2 <- runStartIndex2 + increment
            lastIndex2WithRemovablePos2Wall <- lastIndex2WithRemovablePos2Wall + increment
        else

        let isPos1ALimit = (isALimitAt coordinate position1)
        let isPos2ALimit = (isALimitAt coordinate position2)

        // if we are in a corner
        if isPos1ALimit && isPos2ALimit then
            // we check which of the previous cells have a wall at the pos 1 that can be removed
            let randomIndex2 = getRandomIndex2AtPos1ForFromRange runStartIndex2 (index2 - increment)

            match randomIndex2 with
            | Some randomIndex2 ->
                // if there is some we remove it
                linkCellAtPosition { coordinate with ColumnIndex = randomIndex2 } position1
            | None ->
                // we absolutely have to ensure that the last wall on the pos 2 is empty if possible
                ifNotAtLimitLinkCellAtPosition { coordinate with ColumnIndex = lastIndex2WithRemovablePos2Wall } position2

            runStartIndex2 <- index2 + increment
        else

        // if the pos 1 is a limit then we always choose remove pos 2
        if isPos1ALimit then                
            linkCellAtPosition coordinate position2
            ifNotAtLimitLinkCellAtPosition coordinate position2.Opposite

            // we have to check whether there was some prior pos 1 wall to remove 
            let randomIndex2 = getRandomIndex2AtPos1ForFromRange runStartIndex2 (index2 - increment)
            match randomIndex2 with
            | Some index2ForPos1Removal ->                
                linkCellAtPosition { coordinate with ColumnIndex = index2ForPos1Removal } position1
                lastIndex2WithRemovablePos2Wall <- index2
            | None -> ()
            
            runStartIndex2 <- index2 + increment
        else

        // if the pos 2 is a limit then we always choose to randomly remove one of the pos 1 of the run
        if isPos2ALimit then
            linkCellAtPosition { coordinate with ColumnIndex = (rng.Next(Math.Min(runStartIndex2, index2), Math.Max(runStartIndex2, index2) + 1)) } position1
            runStartIndex2 <- index2 + increment
        else

        // if pos 1 and pos 2 are both not a limit, we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with

        // we continue carving to the pos 2
        | rng when rng < rngPosition2Weight -> linkCellAtPosition coordinate position2

        // or we open to the pos 1 by choosing randomly one of the pos 1 wall
        | _ -> 
           let randomIndex2 = rng.Next(Math.Min(runStartIndex2, index2), Math.Max(runStartIndex2, index2) + 1)
           linkCellAtPosition { coordinate with ColumnIndex = randomIndex2 } position1
           lastIndex2WithRemovablePos2Wall <- index2
           runStartIndex2 <- index2 + increment        

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

let createMaze (direction1 : Direction) (direction2 : Direction) rngSeed rngDirection1Weight rngDirection2Weight (grid : Grid<OrthoGrid>) =    

    let rng = Random(rngSeed)

    let getCoordinate isByRows coordinate =
        match isByRows with
        | true ->
            coordinate
        | false ->
            { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  }

    let (extractBy, startIndex, increment, endIndex, getCoordinate) =
        match direction1, direction2 with
        | _, Right -> (grid.GetCellsByRows, 0, 1, getIndex grid.NumberOfColumns, getCoordinate true)
        | _, Left -> (grid.GetCellsByRows, getIndex grid.NumberOfColumns, -1, 0, getCoordinate true)
        | _, Top -> (grid.GetCellsByColumns, getIndex grid.NumberOfRows, -1, 0, getCoordinate false)
        | _, Bottom -> (grid.GetCellsByColumns, 0, 1, getIndex grid.NumberOfRows, getCoordinate false)

    let position1 = mapDirectionToPosition direction1
    let position2 = mapDirectionToPosition direction2

    let isPartOfMaze coordinate = (grid.IsCellPartOfMaze (getCoordinate coordinate))
    let isALimitAt coordinate = (grid.IsLimitAt (getCoordinate coordinate))
    let linkCellAtPosition coordinate = (grid.LinkCellAtPosition (getCoordinate coordinate))
    let ifNotAtLimitLinkCellAtPosition coordinate = (grid.IfNotAtLimitLinkCellAtPosition (getCoordinate coordinate))
    let getRandomIndex2FromRange = (getRandomColumnIndexFromRange isALimitAt rng increment position1)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    extractBy
    |> Seq.iteri(fun index1 _ ->
        carveRow
            // dependencies
            isPartOfMaze
            isALimitAt
            linkCellAtPosition
            ifNotAtLimitLinkCellAtPosition
            (getRandomIndex2FromRange index1)
            // params
            position1
            position2
            rng
            rngTotalWeight
            rngDirection2Weight
            index1
            startIndex
            increment
            endIndex)

    { Grid = grid }