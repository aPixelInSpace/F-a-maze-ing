// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.RecursiveDivision

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the recursive division algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createEmptyBaseGrid

    // act
    let maze = grid |> RecursiveDivision.createMaze 1 0.2 2 2
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|_   _ _ _  |  _|  _|\n" +
        "| |_  |      _  |_  |\n" +
        "|_ _  |_ _|_|  _|   |\n" +
        "|  _ _| |_  |       |\n" +
        "|_ _ _|_ _ _ _|_|_ _|\n"
        
    maze.Grid |> Mazes.Core.Grid.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells