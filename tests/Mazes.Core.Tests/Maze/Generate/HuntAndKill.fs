// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.HuntAndKill

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.Array2D.Tri
open Mazes.Core.Grid.Array2D.OctaSquare
open Mazes.Core.Grid.Array2D.PentaCairo
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let orthoGrid =
        (Rectangle.create 5 10)
        |> OrthoGrid.CreateFunction
    
    // act
    let maze = orthoGrid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|  _|    _|  _|     |\n" +
        "|_  |_|_ _  |  _| |_|\n" +
        "|  _ _ _|  _| |_ _  |\n" +
        "| |  _ _ _ _|_ _  | |\n" +
        "|_ _ _ _ _ _ _ _ _|_|\n"

    maze.Grid.ToString |> should equal expectedMaze

[<Fact>]
let ``Given a ortho grid 5 by 10 with non adjacent neighbors, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output and every cell should be accessible`` () =
    // arrange
    let orthoGrid =
        (Rectangle.create 5 10)
        |> OrthoGrid.CreateFunction

    let orthoGrid =
        let grid = orthoGrid()
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 2; CIndex = 4 } { RIndex = 4; CIndex = 4 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 2 } { RIndex = 4; CIndex = 4 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 2 } { RIndex = 1; CIndex = 1 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 4; CIndex = 8 } { RIndex = 2; CIndex = 7 } WallType.Normal
        (fun _ -> grid)

    // act
    let maze = orthoGrid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|   |   |  _ _  |   |\n" +
        "|_|_ _|_ _  | |_ _| |\n" +
        "|  _ _ _ _ _|_ _ _ _|\n" +
        "| |     |  _    |   |\n" +
        "|_ _|_|_ _|_ _|_ _|_|\n"

    maze.Grid.ToString |> should equal expectedMaze

    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone
    let map = maze.createMap rootCoordinate
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> PolarGrid.CreateFunction
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "¦¨¦‾|‾¦‾|‾|‾¦\n" +
        "¦‾¦‾|¨|‾¦¨|‾¦‾|¨|¨|¨|¨|‾¦\n" +
        "¦‾¦‾|‾¦¨|¨|‾¦‾|¨|¨|‾¦‾|¨¦‾|¨|‾|¨|‾¦‾|¨¦‾¦‾¦‾|‾|¨¦\n" +
        "¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨¦‾¦¨|¨¦¨|¨¦¨¦‾¦‾¦‾¦¨|¨¦¨¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a triangular grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> TriGrid.CreateFunction
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        ""
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a octagon-square grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> OctaSquareGrid.CreateFunction
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        ""
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a Cairo pentagonal grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> PentaCairoGrid.CreateFunction
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        ""
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells