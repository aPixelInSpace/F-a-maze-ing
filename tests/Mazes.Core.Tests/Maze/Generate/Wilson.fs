// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Wilson

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Shape
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Creating a rectangular 5 by 10 maze generated with Wilson's algorithm (rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> OrthoGrid.createGridFunction
    
    // act
    let maze = grid |> Wilson.createMaze 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "| |   |  _|_  |_ _  |\n" +
        "|   |_|  _   _ _ _  |\n" +
        "| |  _|   |_ _ _| |_|\n" +
        "| |    _|  _ _ _  | |\n" +
        "|_|_|_|_ _ _|_ _ _ _|\n"

    maze.Grid.ToString |> should equal expectedMaze
