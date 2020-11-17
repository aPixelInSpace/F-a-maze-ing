module Mazes.Core.Algo.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Extensions
open Mazes.Core.GridWall

let private carveRow (rng : Random) grid rowIndex row =
    let mutable lastColumnIndexWithTopWall = 0
    let mutable lastColumnIndexWithLeftWall = 0
    
    let mutable isLastRemovedTop = false
    
    row
    |> Array.iteri(
        fun columnIndex _ ->
            // if the cell is not part of the maze, we do nothing
            if not (GridCell.isPartOfMaze rowIndex columnIndex grid) then ()
            else

            let isTopALimit = (GridCell.isALimitAt Top rowIndex columnIndex grid)
            let isRightALimit = (GridCell.isALimitAt Right rowIndex columnIndex grid)

            // if we are in a top right corner 
            if isTopALimit &&  isRightALimit then                
                if isLastRemovedTop then                    
                    // we absolutely have to ensure that the wall on the left is empty if possible
                    let isLastLeftALimit = (GridCell.isALimitAt Left rowIndex lastColumnIndexWithLeftWall grid)
                    if not isLastLeftALimit then
                        updateWallAtPosition Left Empty rowIndex lastColumnIndexWithLeftWall grid
                else                    
                    // or with top
                    let isLastTopALimit = (GridCell.isALimitAt Top rowIndex lastColumnIndexWithTopWall grid)
                    if not isLastTopALimit then
                        updateWallAtPosition Top Empty rowIndex lastColumnIndexWithTopWall grid            
            else

            // if the top is a limit then we always choose remove right
            if isTopALimit then updateWallAtPosition Right Empty rowIndex columnIndex grid
            else

            // if the right is a limit then we always choose remove top
            if isRightALimit then updateWallAtPosition Top Empty rowIndex columnIndex grid
            else

            // if top and right are both not a limit we flip a coin to decide which one we remove
            match rng.Next(2) with
            | 0 ->
                updateWallAtPosition Top Empty rowIndex columnIndex grid
                isLastRemovedTop <- true
                lastColumnIndexWithLeftWall <- columnIndex + 1
            | 1 ->
                updateWallAtPosition Right Empty rowIndex columnIndex grid
                isLastRemovedTop <- false
                lastColumnIndexWithTopWall <- columnIndex
            | _ -> raise(Exception("Random number generation problem"))
       )

let transformIntoMaze rng grid =
    grid.Cells
    |> Array2D.extractByRows
    |> Seq.iteri(fun rowIndex row -> carveRow rng grid rowIndex row)    

    grid