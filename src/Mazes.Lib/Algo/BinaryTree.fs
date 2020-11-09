module Mazes.Lib.Algo.BinaryTree

open System
open Mazes.Lib
open Mazes.Lib.SimpleTypes
open Mazes.Lib.Grid.Wall

let private removeOneWall (rng : Random) grid rowIndex columnIndex cell =
    // if we are in a top right corner, we can't remove one of these walls
    // so we do nothing
    if cell.WallTop.WallType = Border && cell.WallRight.WallType = Border then ()
    else
    
    // if the top is a border then we always choose remove right
    if cell.WallTop.WallType = Border then updateWallAtPosition Right Empty rowIndex columnIndex grid
    else
    
    // if the right is a border then we always choose remove top
    if cell.WallRight.WallType = Border then updateWallAtPosition Top Empty rowIndex columnIndex grid
    else
    
    // if top and right are both not a border we flip a coin to decide which one we remove
    match rng.Next(2) with
    | 0 ->
        updateWallAtPosition Top Empty rowIndex columnIndex grid
    | 1 ->
        updateWallAtPosition Right Empty rowIndex columnIndex grid
    | _ -> raise(Exception("Random number generation problem"))
    
let transformIntoMaze seed grid =
    let rng = Random(seed)
    grid.Cells |> Array2D.iteri(removeOneWall rng grid)
    
    grid