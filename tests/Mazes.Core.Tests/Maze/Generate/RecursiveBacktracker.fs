// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.RecursiveBacktracker

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Shape
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Creating a rectangular 5 by 10 maze generated with the Recursive Backtracker algorithm (rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid.create
    
    // act
    let maze = grid |> RecursiveBacktracker.createMaze 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "| |  _ _ _  |_    | |\n" +
        "| |_ _| |  _|  _| | |\n" +
        "| |  _ _ _|  _| | | |\n" +
        "| | |   |_  |   | | |\n" +
        "|_ _ _|_ _ _|_|_ _ _|\n"

    maze.Grid.ToString |> should equal expectedMaze
