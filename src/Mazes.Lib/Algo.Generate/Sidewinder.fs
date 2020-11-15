﻿module Mazes.Lib.Algo.Generate.Sidewinder

open System
open Mazes.Lib
open Mazes.Lib.Extensions
open Mazes.Lib.Cell
open Mazes.Lib.Grid.Grid
open Mazes.Lib.Grid.Grid.Wall

let private carveRow (rng : Random) grid rowIndex row =
    let mutable runStartIndex = 0
    let mutable lastColumnIndexWithLeftWall = 0
    
    row
    |> Array.iteri(
        fun columnIndex _ ->
            // if the cell is not part of the maze, we only update the run start index
            if not (Cell.isPartOfMaze rowIndex columnIndex grid) then
                runStartIndex <- runStartIndex + 1
            else

            let isTopALimit = (Cell.isTopALimit rowIndex columnIndex grid)
            let isRightALimit = (Cell.isRightALimit rowIndex columnIndex grid)
    
            // if we are in a top right corner, we check which of the previous cells have a wall at the top that can be removed            
            if isTopALimit && isRightALimit then
                let eligibleCellsWithRemovableTopWall = ResizeArray<int>()
                for i in runStartIndex .. (columnIndex - 1) do
                    if not (Cell.isTopALimit rowIndex i grid) then
                        eligibleCellsWithRemovableTopWall.Add(i)

                // if there is something
                if eligibleCellsWithRemovableTopWall.Count > 0 then
                    // randomly remove the top wall of one
                    updateWallAtPosition Top Empty rowIndex (eligibleCellsWithRemovableTopWall.[rng.Next(0, eligibleCellsWithRemovableTopWall.Count - 1)]) grid
                else
                    // we absolutely have to ensure that the last wall on the left is empty if possible
                    let isLastLeftWallALimit = (Cell.isLeftALimit rowIndex lastColumnIndexWithLeftWall grid)
                    if not isLastLeftWallALimit then
                        updateWallAtPosition Left Empty rowIndex lastColumnIndexWithLeftWall grid
            else

            // if the top is a limit then we always choose remove right
            if isTopALimit then                
                updateWallAtPosition Right Empty rowIndex columnIndex grid
                runStartIndex <- columnIndex + 1
            else

            // if the right is a limit then we always choose to randomly remove one of the top wall of the run
            if isRightALimit then
                updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                runStartIndex <- columnIndex + 1
            else

            // if top and right are both not a limit we flip a coin to decide which one we remove                            
            match rng.Next(2) with
            // we continue carving to the right
            | 0 -> updateWallAtPosition Right Empty rowIndex columnIndex grid
            // or we open to the top by choosing randomly one of the top wall
            | 1 -> updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                   lastColumnIndexWithLeftWall <- columnIndex + 1
                   runStartIndex <- columnIndex + 1
            | _ -> raise(Exception("Random number generation problem"))
    )

let transformIntoMaze rng grid =    
    
    grid.Cells
    |> Array2D.extractRows
    |> Seq.iteri(fun rowIndex row -> carveRow rng grid rowIndex row)
    
    grid