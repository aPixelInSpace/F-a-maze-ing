module Mazes.Lib.Algo.Generate.BinaryTree

open System
open Mazes.Lib
open Mazes.Lib.Cell
open Mazes.Lib.Grid.Wall

let private removeOneWall (rng : Random) grid rowIndex columnIndex _ =
    // if we are in a top right corner, we can't remove one of these walls
    // so we do nothing
    
    let isTopALimit = (Grid.Cell.isTopALimit rowIndex columnIndex grid)
    let isRightALimit = (Grid.Cell.isRightALimit rowIndex columnIndex grid)
    
    if isTopALimit &&  isRightALimit then ()
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
    | 1 ->
        updateWallAtPosition Right Empty rowIndex columnIndex grid
    | _ -> raise(Exception("Random number generation problem"))
    
let transformIntoMaze rng grid =    
    grid.Cells |> Array2D.iteri(removeOneWall rng grid)
    
    grid