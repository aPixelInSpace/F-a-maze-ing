// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.Sidewinder

open System
open Mazes.Core.Refac
open Mazes.Core.Refac.Array2D
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Structure.Grid
open Mazes.Core.Refac.Maze

type SidewinderDirection =
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
    (direction1 : SidewinderDirection)
    (direction2 : SidewinderDirection)
    (rng : Random)
    rngTotalWeight    
    rngPosition2Weight
    index1
    increment =

    let startIndex2, endIndex2 = getDimension1Limits index1

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

let createMaze (direction1 : SidewinderDirection) (direction2 : SidewinderDirection) rngSeed rngDirection1Weight rngDirection2Weight ndStruct =    

    let slice2D = snd (NDimensionalStructure.firstSlice2D ndStruct)

    let rng = Random(rngSeed)

    let getTraverseCoordinate isByRows coordinate =
        (match isByRows with
        | true ->
            coordinate
        | false ->
            { RIndex = coordinate.CIndex; CIndex = coordinate.RIndex })

    let extractBy, getCoordinate, getTraverseCoordinate, increment, isByRows =
        let isByRows =
            match direction1, direction2 with
            | _, Right -> true
            | _, Left -> true
            | _, Top -> false
            | _, Bottom -> false
        
        let getCoordinate isByRows coordinate =
            match isByRows with
            | true -> getTraverseCoordinate isByRows coordinate
            | false -> adjustedCoordinate slice2D (getTraverseCoordinate isByRows coordinate)

        match direction1, direction2 with
         | _, Right -> rIndexes slice2D, (getCoordinate isByRows), (getTraverseCoordinate isByRows), 1, isByRows
         | _, Left -> rIndexes slice2D, (getCoordinate isByRows), (getTraverseCoordinate isByRows), -1, isByRows
         | _, Top -> cIndexes slice2D, (getCoordinate isByRows), (getTraverseCoordinate isByRows), -1, isByRows
         | _, Bottom -> cIndexes slice2D, (getCoordinate isByRows), (getTraverseCoordinate isByRows), 1, isByRows

    let getDimension1Limits dimensionIndex =
         match direction1, direction2 with
         | _, Right ->
             let startIndex, length = dimension2Boundaries slice2D dimensionIndex
             (startIndex, getIndex (length + startIndex))
         | _, Left ->
             let startIndex, length = dimension2Boundaries slice2D dimensionIndex
             (getIndex (length + startIndex), startIndex)
         | _, Top ->
             let startIndex, length = dimension1Boundaries slice2D dimensionIndex
             (getIndex (length + startIndex), startIndex)
         | _, Bottom ->
             let startIndex, length = dimension1Boundaries slice2D dimensionIndex
             (startIndex, getIndex (length + startIndex))

    let neighborCoordinate coordinate position =
            match (neighbor slice2D (getCoordinate coordinate) position) with
            | Some neighbor -> neighbor
            | None -> failwith "Sidewinder, unable to find the neighbor coordinate"

    let isPartOfMaze coordinate = (isCellPartOfMaze slice2D (getCoordinate coordinate))
    
    let isInsideGridBoundary coordinate (direction : SidewinderDirection) =
        let coordinate = getTraverseCoordinate coordinate
        let existAt =
            match isByRows with
            | true -> existAt slice2D
            | false -> adjustedExistAt slice2D

        match (virtualNeighbor slice2D coordinate (toDisposition slice2D direction)) with
        | Some neighbor -> existAt neighbor
        | None -> false

    let isALimitAt coordinate (direction : SidewinderDirection) =
        (neighbor slice2D (getCoordinate coordinate) (toDisposition slice2D direction)).IsNone ||
        (isLimitAtCoordinate slice2D (getCoordinate coordinate) (neighborCoordinate coordinate (toDisposition slice2D direction))) ||
        (not (isInsideGridBoundary coordinate direction))

    let linkCellAtPosition coordinate (direction : SidewinderDirection) = (updateConnectionState slice2D Open (getCoordinate coordinate) (neighborCoordinate coordinate (toDisposition slice2D direction)))
    let ifNotAtLimitLinkCellAtPosition coordinate (direction : SidewinderDirection) =
        if (neighbor slice2D (getCoordinate coordinate) (toDisposition slice2D direction)).IsSome then
            (ifNotAtLimitUpdateConnectionState slice2D Open (getCoordinate coordinate) (neighborCoordinate coordinate (toDisposition slice2D direction)))

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

    { NDStruct = ndStruct }