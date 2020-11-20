module Mazes.Core.Algo.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Extensions
open Mazes.Core.Position
open Mazes.Core.GridWall

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
    let mutable lastColumnIndexWithDir1Wall = 0
    let mutable lastColumnIndexWithDir2Wall = 0
    
    let mutable isLastRemovedDir1 = false

    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        // if the cell is not part of the maze, we do nothing
        if not (GridCell.isPartOfMaze rowIndex columnIndex grid) then ()
        else

        let isDir1ALimit = (GridCell.isALimitAt direction1 rowIndex columnIndex grid)
        let isDir2ALimit = (GridCell.isALimitAt direction2 rowIndex columnIndex grid)

        // if we are in a corner 
        if isDir1ALimit &&  isDir2ALimit then                
            if isLastRemovedDir1 then
                ifNotAtLimitUpdateWallAtPosition direction2 Empty rowIndex lastColumnIndexWithDir2Wall grid
                ifNotAtLimitUpdateWallAtPosition (Position.getOpposite direction1) Empty rowIndex columnIndex grid
            else
                ifNotAtLimitUpdateWallAtPosition direction1 Empty rowIndex lastColumnIndexWithDir1Wall grid
                ifNotAtLimitUpdateWallAtPosition (Position.getOpposite direction2) Empty rowIndex columnIndex grid
        else

        // if the dir 1 is a limit then we always choose remove dir 2 (and the opposite dir 2 if possible)
        if isDir1ALimit then
            updateWallAtPosition direction2 Empty rowIndex columnIndex grid
            ifNotAtLimitUpdateWallAtPosition (Position.getOpposite direction2) Empty rowIndex columnIndex grid
        else

        // if the dir 2 is a limit then we always choose remove dir 1 (and the opposite dir 1 if possible)
        if isDir2ALimit then
            updateWallAtPosition direction1 Empty rowIndex columnIndex grid
            ifNotAtLimitUpdateWallAtPosition (Position.getOpposite direction1) Empty rowIndex columnIndex grid
        else

        // if dir 1 and dir 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngDirection1Weight ->
            updateWallAtPosition direction1 Empty rowIndex columnIndex grid
            isLastRemovedDir1 <- true
            lastColumnIndexWithDir2Wall <- columnIndex
        | _ ->
            updateWallAtPosition direction2 Empty rowIndex columnIndex grid
            isLastRemovedDir1 <- false
            lastColumnIndexWithDir1Wall <- columnIndex        

let transformIntoMaze direction1 direction2 rng rngDirection1Weight rngDirection2Weight grid =
    
    let (startColumnIndex, increment, endColumnIndex) =
        match direction1, direction2 with
        | _, Left | Left, _ -> (Grid.getIndex grid.NumberOfColumns, -1, 0)
        | _ -> (0, 1, Grid.getIndex grid.NumberOfColumns)

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.Cells
    |> Array2D.extractByRows
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

    grid