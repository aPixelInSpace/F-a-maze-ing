// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.RecursiveDivision

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the recursive division algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createEmptyBaseGrid
        |> NDimensionalStructure.create2D

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

    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells