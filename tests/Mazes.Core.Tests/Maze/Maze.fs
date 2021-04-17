// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Maze

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Structure
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a maze, when braiding the maze, then the number of dead ends should be reduced`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    let maze = grid |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.Leaves.Length |> should equal 9
    
    // act
    let maze = maze |> Maze.braid 1 0.5 map.Leaves
    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
        
    // assert
    map.Leaves.Length |> should equal 3
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|  _     _|  _      |\n" +
        "|_  |_ _ _  |  _|  _|\n" +
        "|  _ _  |  _| |  _  |\n" +
        "| |  _ _ _ _|_ _  | |\n" +
        "|_ _ _ _ _ _ _ _ _|_|\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze