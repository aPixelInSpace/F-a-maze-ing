module Mazes.Lib.Algo.Generate.Sidewinder

open System
open Mazes.Lib
open Mazes.Lib.Extensions
open Mazes.Lib.Cell
open Mazes.Lib.Grid.Wall

let private carveRow (rng : Random) grid rowIndex row =
    let mutable runStartIndex = 0    
    
    row
    |> Array.iteri(
        fun columnIndex cell ->
            // if we are in a top right corner, we can't remove one of these walls
            // so we do nothing
            if cell.WallTop.WallType = Border && cell.WallRight.WallType = Border then ()
            else

            // if the top is a border then we always choose remove right
            if cell.WallTop.WallType = Border then updateWallAtPosition Right Empty rowIndex columnIndex grid
            else

            // if the right is a border then we always choose to randomly remove one of the top wall of the run
            if cell.WallRight.WallType = Border then
                updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                runStartIndex <- columnIndex + 1
            else

            // if top and right are both not a border we flip a coin to decide which one we remove                            
            match rng.Next(2) with
            // we continue carving to the right
            | 0 -> updateWallAtPosition Right Empty rowIndex columnIndex grid
            // or we open to the top by choosing randomly one of the top wall
            | 1 -> updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                   runStartIndex <- columnIndex + 1
            | _ -> raise(Exception("Random number generation problem"))
    )

    ()

let transformIntoMaze rng grid =    
    
    grid.Cells
    |> Array2D.extractRows
    |> Seq.iteri(fun rowIndex row -> carveRow rng grid rowIndex row)
    
    grid