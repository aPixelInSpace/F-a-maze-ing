module Mazes.Core.Maze.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Canvas
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze

type private mode =
    | ByRows
    | ByColumns

let private getCoordinate mode coordinate =
    match mode with
    | ByRows ->
        coordinate
    | ByColumns ->
        { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  }

let private getRandomColumnIndexFromRange isALimitAt (rng : Random) rowIndex increment dir startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableWallAtDir = ResizeArray<int>()

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do
        if not (isALimitAt { RowIndex = rowIndex; ColumnIndex = columnIndex } dir) then
            eligibleCellsWithRemovableWallAtDir.Add(columnIndex)
    
    if eligibleCellsWithRemovableWallAtDir.Count > 0 then
        Some eligibleCellsWithRemovableWallAtDir.[rng.Next(0, eligibleCellsWithRemovableWallAtDir.Count - 1)]
    else
        None

let private carveRow
    // dependencies    
    isPartOfMaze
    isALimitAt
    updateWallAtPosition
    ifNotAtLimitUpdateWallAtPosition
    getRandomColumnIndexAtDir1ForFromRange
    // params
    direction1
    direction2
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

        let isDir1ALimit = (isALimitAt coordinate direction1)
        let isDir2ALimit = (isALimitAt coordinate direction2)

        // if we are in a corner
        if isDir1ALimit && isDir2ALimit then
            // we check which of the previous cells have a wall at the dir 1 that can be removed
            let randomColumnIndex = getRandomColumnIndexAtDir1ForFromRange runStartIndex (columnIndex - increment)

            match randomColumnIndex with
            | Some randomColumnIndex ->
                // if there is some we remove it
                updateWallAtPosition { coordinate with ColumnIndex = randomColumnIndex } direction1 Empty
            | None ->
                // we absolutely have to ensure that the last wall on the dir 2 is empty if possible
                ifNotAtLimitUpdateWallAtPosition { coordinate with ColumnIndex = lastColumnIndexWithRemovableDir2Wall } direction2 Empty

            runStartIndex <- columnIndex + increment
        else

        // if the dir 1 is a limit then we always choose remove dir 2
        if isDir1ALimit then                
            updateWallAtPosition coordinate direction2 Empty
            ifNotAtLimitUpdateWallAtPosition coordinate (Position.getOpposite direction2) Empty

            // we have to check whether there was some prior dir 1 wall to remove 
            let randomColumnIndex = getRandomColumnIndexAtDir1ForFromRange runStartIndex (columnIndex - increment)
            match randomColumnIndex with
            | Some columnIndexForDir1Removal ->                
                updateWallAtPosition { coordinate with ColumnIndex = columnIndexForDir1Removal } direction1 Empty
                lastColumnIndexWithRemovableDir2Wall <- columnIndex
            | None -> ()
            
            runStartIndex <- columnIndex + increment
        else

        // if the dir 2 is a limit then we always choose to randomly remove one of the dir 1 of the run
        if isDir2ALimit then
            updateWallAtPosition { coordinate with ColumnIndex = (rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)) } direction1 Empty
            runStartIndex <- columnIndex + increment
        else

        // if dir 1 and dir 2 are both not a limit, we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        // we continue carving to the dir 2
        | rng when rng < rngDirection2Weight -> updateWallAtPosition coordinate direction2 Empty

        // or we open to the dir 1 by choosing randomly one of the dir 1 wall
        | _ -> 
           let randomColumnIndex = rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)
           updateWallAtPosition { coordinate with ColumnIndex = randomColumnIndex } direction1 Empty
           lastColumnIndexWithRemovableDir2Wall <- columnIndex
           runStartIndex <- columnIndex + increment        

let createMaze direction1 direction2 rng rngDirection1Weight rngDirection2Weight grid =    

    let (extractBy, startColumnIndex, increment, endColumnIndex, mode) =
        match direction1, direction2 with
        | _, Right -> (extractByRows, 0, 1, getIndex grid.Canvas.NumberOfColumns, ByRows)
        | _, Left -> (extractByRows, getIndex grid.Canvas.NumberOfColumns, -1, 0, ByRows)
        | _, Top -> (extractByColumns, getIndex grid.Canvas.NumberOfRows, -1, 0, ByColumns)
        | _, Bottom -> (extractByColumns, 0, 1, getIndex grid.Canvas.NumberOfRows, ByColumns)

    let getCoordinate = (getCoordinate mode)

    let isPartOfMaze coordinate = (isPartOfMaze grid.Canvas (getCoordinate coordinate))
    let isALimitAt coordinate = (isALimitAt grid (getCoordinate coordinate))
    let updateWallAtPosition coordinate = (updateWallAtPosition grid (getCoordinate coordinate))
    let ifNotAtLimitUpdateWallAtPosition coordinate = (ifNotAtLimitUpdateWallAtPosition grid (getCoordinate coordinate))

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.Cells
    |> extractBy
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // dependencies
            (isPartOfMaze)
            (isALimitAt)
            (updateWallAtPosition)
            (ifNotAtLimitUpdateWallAtPosition)
            (getRandomColumnIndexFromRange isALimitAt rng rowIndex increment direction1)

            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection2Weight
            rowIndex
            startColumnIndex
            increment
            endColumnIndex)

    { Grid = grid}