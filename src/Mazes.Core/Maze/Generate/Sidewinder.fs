// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

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


let private getRandomIndex2AtPos1ForFromRange isALimitAt (rng : Random) increment position rIndex startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableWallAtPos = ResizeArray<int>()

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do
        if not (isALimitAt { RIndex = rIndex; CIndex = columnIndex } position) then
            eligibleCellsWithRemovableWallAtPos.Add(columnIndex)
    
    if eligibleCellsWithRemovableWallAtPos.Count > 0 then
        Some eligibleCellsWithRemovableWallAtPos.[rng.Next(0, eligibleCellsWithRemovableWallAtPos.Count - 1)]
    else
        None

// todo : refactor to make it simpler and more understandable
let private carveRow
    // dependencies
    getDimension1Limits    
    isPartOfMaze
    isALimitAt
    linkCellAtPosition
    ifNotAtLimitLinkCellAtPosition
    getRandomIndex2AtPos1ForFromRange
    // params
    (direction1 : Direction)
    (direction2 : Direction)
    (rng : Random)
    rngTotalWeight    
    rngPosition2Weight
    index1
    increment =

    let (startIndex2, endIndex2) = getDimension1Limits index1

    let mutable runStartIndex2 = startIndex2
    let mutable lastIndex2WithRemovablePos2Wall = startIndex2

    for index2 in startIndex2 .. increment .. endIndex2 do

        let coordinate = { RIndex = index1; CIndex = index2 }

        // if the cell is not part of the maze, we only update the run start index
        if not (isPartOfMaze coordinate) then
            runStartIndex2 <- runStartIndex2 + increment
            lastIndex2WithRemovablePos2Wall <- lastIndex2WithRemovablePos2Wall + increment
        else

        let isPos1ALimit = (isALimitAt coordinate direction1)
        let isPos2ALimit = (isALimitAt coordinate direction2)

        // if we are in a corner
        if isPos1ALimit && isPos2ALimit then
            // we check which of the previous cells have a wall at the pos 1 that can be removed
            let randomIndex2 = getRandomIndex2AtPos1ForFromRange runStartIndex2 (index2 - increment)

            match randomIndex2 with
            | Some randomIndex2 ->
                // if there is some we remove it
                linkCellAtPosition { coordinate with CIndex = randomIndex2 } direction1
            | None ->
                // we absolutely have to ensure that the last wall on the pos 2 is empty if possible
                ifNotAtLimitLinkCellAtPosition { coordinate with CIndex = lastIndex2WithRemovablePos2Wall } direction2

            runStartIndex2 <- index2 + increment
        else

        // if the pos 1 is a limit then we always choose remove pos 2
        if isPos1ALimit then                
            linkCellAtPosition coordinate direction2
            ifNotAtLimitLinkCellAtPosition coordinate direction2.Opposite

            // we have to check whether there was some prior pos 1 wall to remove 
            let randomIndex2 = getRandomIndex2AtPos1ForFromRange runStartIndex2 (index2 - increment)
            match randomIndex2 with
            | Some index2ForPos1Removal ->                
                linkCellAtPosition { coordinate with CIndex = index2ForPos1Removal } direction1
                lastIndex2WithRemovablePos2Wall <- index2
            | None -> ()
            
            runStartIndex2 <- index2 + increment
        else

        // if the pos 2 is a limit then we always choose to randomly remove one of the pos 1 of the run
        if isPos2ALimit then
            linkCellAtPosition { coordinate with CIndex = (rng.Next(Math.Min(runStartIndex2, index2), Math.Max(runStartIndex2, index2) + 1)) } direction1
            runStartIndex2 <- index2 + increment
        else

        // if pos 1 and pos 2 are both not a limit, we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with

        // we continue carving to the pos 2
        | rng when rng < rngPosition2Weight -> linkCellAtPosition coordinate direction2

        // or we open to the pos 1 by choosing randomly one of the pos 1 wall
        | _ -> 
           let randomIndex2 = rng.Next(Math.Min(runStartIndex2, index2), Math.Max(runStartIndex2, index2) + 1)
           linkCellAtPosition { coordinate with CIndex = randomIndex2 } direction1
           lastIndex2WithRemovablePos2Wall <- index2
           runStartIndex2 <- index2 + increment        

let createMaze (direction1 : Direction) (direction2 : Direction) rngSeed rngDirection1Weight rngDirection2Weight (grid : unit -> IGrid<'G>) =    

    let grid = grid()

    let rng = Random(rngSeed)

    let getTraverseCoordinate isByRows coordinate =
        (match isByRows with
        | true ->
            coordinate
        | false ->
            { RIndex = coordinate.CIndex; CIndex = coordinate.RIndex })

    let (extractBy, getCoordinate, getTraverseCoordinate, increment, isByRows) =
        let isByRows =
            match direction1, direction2 with
            | _, Right -> true
            | _, Left -> true
            | _, Top -> false
            | _, Bottom -> false
        
        let getCoordinate isByRows coordinate =
            match isByRows with
            | true -> getTraverseCoordinate isByRows coordinate
            | false -> grid.GetAdjustedCoordinate (getTraverseCoordinate isByRows coordinate)

        match direction1, direction2 with
         | _, Right -> (grid.GetRIndexes), (getCoordinate isByRows), (getTraverseCoordinate isByRows), 1, isByRows
         | _, Left -> (grid.GetRIndexes), (getCoordinate isByRows), (getTraverseCoordinate isByRows), -1, isByRows
         | _, Top -> (grid.GetCIndexes), (getCoordinate isByRows), (getTraverseCoordinate isByRows), -1, isByRows
         | _, Bottom -> (grid.GetCIndexes), (getCoordinate isByRows), (getTraverseCoordinate isByRows), 1, isByRows

    let getDimension1Limits dimensionIndex =
         match direction1, direction2 with
         | _, Right ->
             let (startIndex, length) = grid.Dimension2Boundaries dimensionIndex
             (startIndex, getIndex (length + startIndex))
         | _, Left ->
             let (startIndex, length) = grid.Dimension2Boundaries dimensionIndex
             (getIndex (length + startIndex), startIndex)
         | _, Top ->
             let (startIndex, length) = grid.Dimension1Boundaries dimensionIndex
             (getIndex (length + startIndex), startIndex)
         | _, Bottom ->
             let (startIndex, length) = grid.Dimension1Boundaries dimensionIndex
             (startIndex, getIndex (length + startIndex))

    let neighborCoordinate coordinate position =
            match (grid.Neighbor (getCoordinate coordinate) position) with
            | Some neighbor -> neighbor
            | None -> failwith "Sidewinder, unable to find the neighbor coordinate"

    let isPartOfMaze coordinate = (grid.IsCellPartOfMaze (getCoordinate coordinate))
    
    let isInsideGridBoundary coordinate (direction : Direction) =
        let coordinate = getTraverseCoordinate coordinate
        let existAt =
            match isByRows with
            | true -> grid.ExistAt
            | false -> grid.GetAdjustedExistAt
        match direction with
        | Left -> existAt { coordinate with CIndex = coordinate.CIndex - 1 }
        | Top -> existAt { coordinate with RIndex = coordinate.RIndex - 1 }
        | Right -> existAt { coordinate with CIndex = coordinate.CIndex + 1 }
        | Bottom -> existAt { coordinate with RIndex = coordinate.RIndex + 1 }
    
    let isALimitAt coordinate (direction : Direction) =
        (grid.Neighbor (getCoordinate coordinate) direction.Position).IsNone ||
        (grid.IsLimitAt (getCoordinate coordinate) (neighborCoordinate coordinate direction.Position)) ||
        (not (isInsideGridBoundary coordinate direction))

    let linkCellAtPosition coordinate (direction : Direction) = (grid.LinkCells (getCoordinate coordinate) (neighborCoordinate coordinate direction.Position))
    let ifNotAtLimitLinkCellAtPosition coordinate (direction : Direction) =
        if (grid.Neighbor (getCoordinate coordinate) direction.Position).IsSome then
            (grid.IfNotAtLimitLinkCells (getCoordinate coordinate) (neighborCoordinate coordinate direction.Position))

    let getRandomIndex2FromRange = (getRandomIndex2AtPos1ForFromRange isALimitAt rng increment direction1)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    extractBy
    |> Seq.iter(fun index1 ->
        carveRow
            // dependencies
            getDimension1Limits
            isPartOfMaze
            isALimitAt
            linkCellAtPosition
            ifNotAtLimitLinkCellAtPosition
            (getRandomIndex2FromRange index1)
            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection2Weight
            index1
            increment)

    { Grid = grid }