module Mazes.Core.Algo.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Extensions
open Mazes.Core.Position
open Mazes.Core.GridWall

type private mode =
    | ByRows
    | ByColumns

let private updateWallAtPosition mode wallPosition wallType rowIndex columnIndex grid =
    match mode with
    | ByRows ->
        updateWallAtPosition wallPosition wallType rowIndex columnIndex grid
    | ByColumns ->
        updateWallAtPosition wallPosition wallType columnIndex rowIndex grid

let private ifNotAtLimitUpdateWallAtPosition mode wallPosition wallType rowIndex columnIndex grid =
    match mode with
    | ByRows ->
        ifNotAtLimitUpdateWallAtPosition wallPosition wallType rowIndex columnIndex grid
    | ByColumns ->
        ifNotAtLimitUpdateWallAtPosition wallPosition wallType columnIndex rowIndex grid

let private isALimitAt mode position rowIndex columnIndex grid =
    match mode with
    | ByRows ->
        GridCell.isALimitAt position rowIndex columnIndex grid
    | ByColumns ->
        GridCell.isALimitAt position columnIndex rowIndex grid

let private isPartOfMaze mode rowIndex columnIndex grid =
    match mode with
    | ByRows ->
        GridCell.isPartOfMaze rowIndex columnIndex grid
    | ByColumns ->
       GridCell.isPartOfMaze columnIndex rowIndex grid

let private getRandomColumnIndexForDir1FromRange isALimitAt dir (rng : Random) grid rowIndex increment startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableWallAtDir = ResizeArray<int>()

    for i in startColumnIndex .. increment .. endColumnIndex do
        if not (isALimitAt dir rowIndex i grid) then
            eligibleCellsWithRemovableWallAtDir.Add(i)
    
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
    getRandomColumnIndexForDir1FromRange
    // params
    direction1
    direction2
    (rng : Random)
    rngTotalWeight    
    rngDirection2Weight
    grid
    rowIndex
    startColumnIndex
    increment
    endColumnIndex =
        
    let mutable runStartIndex = startColumnIndex
    let mutable lastColumnIndexWithDir2Wall = startColumnIndex

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        // if the cell is not part of the maze, we only update the run start index
        if not (isPartOfMaze rowIndex columnIndex grid) then
            runStartIndex <- runStartIndex + increment
        else

        let isDir1ALimit = (isALimitAt direction1 rowIndex columnIndex grid)
        let isDir2ALimit = (isALimitAt direction2 rowIndex columnIndex grid)

        // if we are in a corner
        if isDir1ALimit && isDir2ALimit then
            // we check which of the previous cells have a wall at the dir 1 that can be removed
            let randomColumnIndex = getRandomColumnIndexForDir1FromRange runStartIndex (columnIndex - increment)

            match randomColumnIndex with
            | Some randomColumnIndex ->
                // if there is some we remove it
                updateWallAtPosition direction1 Empty rowIndex randomColumnIndex grid
            | None ->
                // we absolutely have to ensure that the last wall on the dir 2 is empty if possible
                ifNotAtLimitUpdateWallAtPosition direction2 Empty rowIndex lastColumnIndexWithDir2Wall grid                
        else

        // if the dir 1 is a limit then we always choose remove dir 2
        if isDir1ALimit then                
            updateWallAtPosition direction2 Empty rowIndex columnIndex grid
            
            // we have to check whether if there was some prior dir 1 wall to remove 
            let randomColumnIndex = getRandomColumnIndexForDir1FromRange runStartIndex (columnIndex - increment)
            match randomColumnIndex with
            | Some columnIndexForDir1Removal ->                
                updateWallAtPosition direction1 Empty rowIndex columnIndexForDir1Removal grid
                lastColumnIndexWithDir2Wall <- columnIndex
            | None -> ()

            runStartIndex <- columnIndex + increment
        else

        // if the dir 2 is a limit then we always choose to randomly remove one of the dir 1 of the run
        if isDir2ALimit then
            updateWallAtPosition direction1 Empty rowIndex (rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)) grid            
            runStartIndex <- columnIndex + increment
        else

        // if dir 1 and dir 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        // we continue carving to the dir 2
        | rng when rng < rngDirection2Weight -> updateWallAtPosition direction2 Empty rowIndex columnIndex grid
        // or we open to the dir 1 by choosing randomly one of the dir 1 wall
        | _ -> 
               let randomColumnIndex = rng.Next(Math.Min(runStartIndex, columnIndex), Math.Max(runStartIndex, columnIndex) + 1)
               updateWallAtPosition direction1 Empty rowIndex randomColumnIndex grid
               
               lastColumnIndexWithDir2Wall <- columnIndex
               runStartIndex <- columnIndex + increment        

let transformIntoMaze direction1 direction2 rng rngDirection1Weight rngDirection2Weight grid =    
    
    let (extractBy, startColumnIndex, increment, endColumnIndex, mode) =
        match direction1, direction2 with
        | _, Right -> (Array2D.extractByRows, 0, 1, Grid.getIndex grid.NumberOfColumns, ByRows)
        | _, Left -> (Array2D.extractByRows, Grid.getIndex grid.NumberOfColumns, -1, 0, ByRows)
        | _, Top -> (Array2D.extractByColumns, Grid.getIndex grid.NumberOfRows, -1, 0, ByColumns)
        | _, Bottom -> (Array2D.extractByColumns, 0, 1, Grid.getIndex grid.NumberOfRows, ByColumns)
    
    let isALimitAt = isALimitAt mode
    
    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight
    
    grid.Cells
    |> extractBy
    |> Seq.iteri(fun rowIndex _ ->
        carveRow
            // dependencies
            (isPartOfMaze mode)
            isALimitAt
            (updateWallAtPosition mode)
            (ifNotAtLimitUpdateWallAtPosition mode)
            (getRandomColumnIndexForDir1FromRange isALimitAt direction1 rng grid rowIndex increment)

            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection2Weight
            grid
            rowIndex
            startColumnIndex
            increment
            endColumnIndex)
    
    grid