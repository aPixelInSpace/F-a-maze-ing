module Mazes.Core.Maze.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas.Canvas
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid

type private mode =
    | ByRows
    | ByColumns

let private updateWallAtPosition mode grid position wallType coordinate =
    match mode with
    | ByRows ->
        updateWallAtPosition position wallType coordinate grid
    | ByColumns ->
        updateWallAtPosition position wallType { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  } grid

let private ifNotAtLimitUpdateWallAtPosition mode grid position wallType coordinate =
    match mode with
    | ByRows ->
        ifNotAtLimitUpdateWallAtPosition position wallType coordinate grid
    | ByColumns ->
        ifNotAtLimitUpdateWallAtPosition position wallType { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  } grid

let private isALimitAt mode grid position coordinate =
    match mode with
    | ByRows ->
        isALimitAt position coordinate grid
    | ByColumns ->
        isALimitAt position { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  } grid

let private isPartOfMaze mode grid coordinate =
    match mode with
    | ByRows ->
        isPartOfMaze grid.Canvas coordinate
    | ByColumns ->
       isPartOfMaze grid.Canvas { RowIndex = coordinate.ColumnIndex; ColumnIndex = coordinate.RowIndex  }

let private getRandomColumnIndexForDir1FromRange isALimitAt (rng : Random) rowIndex increment dir startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableWallAtDir = ResizeArray<int>()

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do
        if not (isALimitAt dir { RowIndex = rowIndex; ColumnIndex = columnIndex }) then
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
    getRandomColumnIndexForFromRange
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
    let mutable lastColumnIndexWithDir2Wall = startColumnIndex

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RowIndex = rowIndex; ColumnIndex = columnIndex }

        // if the cell is not part of the maze, we only update the run start index
        if not (isPartOfMaze coordinate) then
            runStartIndex <- runStartIndex + increment
        else

        let isDir1ALimit = (isALimitAt direction1 coordinate)
        let isDir2ALimit = (isALimitAt direction2 coordinate)

        // if we are in a corner
        if isDir1ALimit && isDir2ALimit then
            // we check which of the previous cells have a wall at the dir 1 that can be removed
            let randomColumnIndex = getRandomColumnIndexForFromRange direction1 runStartIndex (columnIndex - increment)

            match randomColumnIndex with
            | Some randomColumnIndex ->
                // if there is some we remove it
                updateWallAtPosition direction1 Empty { coordinate with ColumnIndex = randomColumnIndex }
            | None ->
                // we absolutely have to ensure that the last wall on the dir 2 is empty if possible
                ifNotAtLimitUpdateWallAtPosition direction2 Empty { coordinate with ColumnIndex = lastColumnIndexWithDir2Wall }

            runStartIndex <- columnIndex + increment
        else

        // if the dir 1 is a limit then we always choose remove dir 2
        if isDir1ALimit then                
            updateWallAtPosition direction2 Empty coordinate
            ifNotAtLimitUpdateWallAtPosition (Position.getOpposite direction2) Empty coordinate
            
            // we have to check whether there was some prior dir 1 wall to remove 
            let randomColumnIndex = getRandomColumnIndexForFromRange direction1 runStartIndex (columnIndex - increment)
            match randomColumnIndex with
            | Some columnIndexForDir1Removal ->                
                updateWallAtPosition direction1 Empty { coordinate with ColumnIndex = columnIndexForDir1Removal }
                lastColumnIndexWithDir2Wall <- columnIndex
            | None -> ()
            
            runStartIndex <- columnIndex + increment
        else

        // if the dir 2 is a limit then we always choose to randomly remove one of the dir 1 of the run
        if isDir2ALimit then
            updateWallAtPosition direction1 Empty { coordinate with ColumnIndex = (rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)) }
            runStartIndex <- columnIndex + increment
        else

        // if dir 1 and dir 2 are both not a limit, we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        // we continue carving to the dir 2
        | rng when rng < rngDirection2Weight -> updateWallAtPosition direction2 Empty coordinate

        // or we open to the dir 1 by choosing randomly one of the dir 1 wall
        | _ -> 
               let randomColumnIndex = rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)
               updateWallAtPosition direction1 Empty { coordinate with ColumnIndex = randomColumnIndex }               
               lastColumnIndexWithDir2Wall <- columnIndex
               runStartIndex <- columnIndex + increment        

let transformIntoMaze direction1 direction2 rng rngDirection1Weight rngDirection2Weight grid =    
    
    let (extractBy, startColumnIndex, increment, endColumnIndex, mode) =
        match direction1, direction2 with
        | _, Right -> (extractByRows, 0, 1, getIndex grid.Canvas.NumberOfColumns, ByRows)
        | _, Left -> (extractByRows, getIndex grid.Canvas.NumberOfColumns, -1, 0, ByRows)
        | _, Top -> (extractByColumns, getIndex grid.Canvas.NumberOfRows, -1, 0, ByColumns)
        | _, Bottom -> (extractByColumns, 0, 1, getIndex grid.Canvas.NumberOfRows, ByColumns)
    
    let isALimitAt = isALimitAt mode grid
    
    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight
    
    grid.Cells
    |> extractBy
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // dependencies
            (isPartOfMaze mode grid)
            (isALimitAt)
            (updateWallAtPosition mode grid)
            (ifNotAtLimitUpdateWallAtPosition mode grid)
            (getRandomColumnIndexForDir1FromRange isALimitAt rng rowIndex increment)

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
    
    grid