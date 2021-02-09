// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Kruskal

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.Array2D.Hex
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with Kruskal's algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.GridNew.Ortho.createBaseGrid
        |> Mazes.Core.GridNew.Grid.create
    
    // act
    let maze = grid |> Kruskal.createMazeNew 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "| |   | | |  _  |  _|\n" +
        "|   | |_ _  | |  _ _|\n" +
        "|_| |_|      _|_   _|\n" +
        "|    _ _| |_ _|    _|\n" +
        "|_|_ _|_ _|_ _ _|_ _|\n"
        
    maze.Grid |> Mazes.Core.GridNew.Ortho.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Kruskal's algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Mazes.Core.GridNew.Polar.createBaseGrid
        |> Mazes.Core.GridNew.Grid.create
    
    // act
    let maze = grid |> Kruskal.createMazeNew 1

    // assert
    let expectedMaze =
        "| ¦ | |\n" +
        "¦¨|‾¦‾¦‾¦¨¦‾¦\n" +
        "¦¨|‾¦‾¦‾¦¨|‾¦¨¦‾¦‾|¨|‾¦‾¦\n" +
        "¦¨|¨|¨|‾¦¨|¨|¨|¨|¨|¨¦‾¦‾|¨|¨|‾¦‾|‾|‾¦‾¦¨|‾¦‾¦‾¦‾¦\n" +
        "¦¨¦‾|¨|‾¦¨|¨|¨¦‾¦‾|¨|¨|¨¦‾|‾¦‾¦¨¦¨¦‾¦‾¦¨|¨|¨|¨|‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid |> Mazes.Core.GridNew.Polar.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells