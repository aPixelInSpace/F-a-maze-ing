// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Eller

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with Eller's algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.GridNew.Types.Ortho.Grid.createBaseGrid
        |> Mazes.Core.GridNew.Grid.create
    
    // act
    let maze = grid |> Eller.createMaze 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|    _ _| |_   _|   |\n" +
        "| |_  |_  | | | | | |\n" +
        "| |_    |   |   | | |\n" +
        "|  _| |_ _|   |   | |\n" +
        "|_ _|_ _ _|_|_|_|_|_|\n"
        
    maze.Grid |> Mazes.Core.GridNew.Types.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with Eller's algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Mazes.Core.GridNew.Types.Polar.Grid.createBaseGrid
        |> Mazes.Core.GridNew.Grid.create
    
    // act
    let maze = grid |> Eller.createMaze 1

    // assert
    let expectedMaze =
        "| ¦ ¦ |\n" +
        "¦‾|‾¦‾|¨¦‾|¨¦\n" +
        "|‾¦‾¦‾¦‾|‾¦¨|‾¦¨|‾|‾|‾|‾|\n" +
        "|‾¦¨|‾¦‾¦‾|‾¦‾|¨¦‾|‾¦‾¦¨¦‾¦‾¦‾|¨¦‾|¨|‾|¨¦‾¦¨¦‾¦¨|\n" +
        "|‾¦¨¦¨|¨|¨¦¨|¨|¨|¨¦‾¦‾¦‾¦¨|¨¦‾¦¨|¨¦¨¦¨¦¨¦‾|¨|¨|¨|\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid |> Mazes.Core.GridNew.Types.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells