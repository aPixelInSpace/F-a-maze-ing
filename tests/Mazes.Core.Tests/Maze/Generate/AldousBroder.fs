// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.AldousBroder

open FsUnit
open Xunit
open Mazes.Core.Grid.Ortho.Canvas.Shape
open Mazes.Core.Grid.Ortho
open Mazes.Core.Grid.Polar
open Mazes.Core.Grid.Polar.Canvas.Shape
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Creating a orthogonal rectangular 5 by 10 maze generated with Aldous-Broder algorithm (rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> OrthoGrid.createGridFunction
    
    // act
    let maze = grid |> AldousBroder.createMaze 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|_  |      _|_ _    |\n" +
        "| |_ _|_|_  |     | |\n" +
        "| | |  _ _ _| |_|_| |\n" +
        "| |      _ _ _ _| | |\n" +
        "|_ _|_|_ _ _ _|_ _ _|\n"
        
    maze.Grid.ToString |> should equal expectedMaze

//[<Fact>]
//let ``Creating a polar disc 5 rings maze generated with Aldous-Broder algorithm (rng 1) should be like the expected output`` () =
//    // arrange
//    let grid =
//        (Disc.create 5 1.0 3)
//        |> PolarGrid.createGridFunction
//    
//    // act
//    let maze = grid |> AldousBroder.createMaze 1
//        
//    // assert
//    let expectedMaze =
//        "| |___  _\n" +
//        "|||| |\n" +
//        " __  _  _  _\n" +
//        "  ||  || ||\n" + 
//        "_ __ _ ___   ______ _ __\n" +
//        "|     ||   ||      |\n" +    
//        "___  _ _ _  __ _ __ _  _\n" +
//        "|   | | || |   | |  | |\n" + 
//        "________________________"
//        
//    maze.Grid.ToString |> should equal expectedMaze
