// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.BinaryTree

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate.BinaryTree

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the Binary Tree algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> OrthoGrid.CreateFunction
    
    // act
    let maze = grid |> createMaze Direction.Top Direction.Right 1 1 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|      _ _     _    |\n" +
        "|_| | |  _ _|_|  _| |\n" +
        "|_ _|_| |    _  |_  |\n" +
        "|  _ _ _|_|_|  _|_  |\n" +
        "|_|_ _ _ _ _ _|_ _ _|\n"
        
    maze.Grid.ToString |> should equal expectedMaze

type BinaryTreeDirectionEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

type BDE = BinaryTreeDirectionEnum

let mapBinaryTreeDirectionEnumToBinaryTreeDirection dirEnum =
    match dirEnum with
    | BDE.Top -> Direction.Top
    | BDE.Right -> Direction.Right
    | BDE.Bottom -> Direction.Bottom
    | BDE.Left -> Direction.Left
    | _ -> failwith "Binary Tree Direction enumeration unknown"

[<Theory>]
[<InlineData(1, 1, BDE.Top, BDE.Right, 1, 1, 1)>]
[<InlineData(1, 1, BDE.Top, BDE.Right, 1, 2, 1)>]
[<InlineData(1, 1, BDE.Top, BDE.Right, 1, 1, 2)>]
[<InlineData(1, 2, BDE.Top, BDE.Right, 1, 1, 1)>]
[<InlineData(2, 1, BDE.Top, BDE.Right, 1, 1, 1)>]
[<InlineData(1, 5, BDE.Top, BDE.Right, 1, 1, 1)>]
[<InlineData(5, 1, BDE.Top, BDE.Right, 1, 1, 1)>]

[<InlineData(5, 5, BDE.Top, BDE.Right, 2, 1, 1)>]
[<InlineData(5, 5, BDE.Top, BDE.Left, 2, 1, 1)>]
[<InlineData(10, 25, BDE.Right, BDE.Top, 3, 1, 1)>]
[<InlineData(25, 5, BDE.Right, BDE.Bottom, 4, 2, 1)>]
[<InlineData(25, 25, BDE.Bottom, BDE.Left, 5, 2, 3)>]
[<InlineData(25, 25, BDE.Bottom, BDE.Right, 6, 1, 1)>]
[<InlineData(25, 35, BDE.Left, BDE.Top, 7, 3, 2)>]
[<InlineData(40, 25, BDE.Left, BDE.Bottom, 8, 1, 1)>]
let ``Given a rectangular canvas, when a creating a maze with the binary tree algorithm, then the maze should have every cell accessible``
    (numberOfRows, numberOfColumns,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    // arrange
    let gridRectangle =
        Rectangle.create numberOfRows numberOfColumns
        |> OrthoGrid.CreateFunction

    let direction1 = mapBinaryTreeDirectionEnumToBinaryTreeDirection direction1
    let direction2 = mapBinaryTreeDirectionEnumToBinaryTreeDirection direction2

    // act
    let maze = createMaze direction1 direction2 rngSeed direction1Weight direction2Weight gridRectangle

    // we use the map to ensure that the total nodes accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Binary Tree algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> PolarGrid.createGridFunction
    
    // act
    let maze = grid |> createMaze Direction.Top Direction.Right 1 1 1

    // assert
    let expectedMaze =
        "¦ ¦ ¦ ¦\n" +
        "|¨|¨|¨|‾¦‾¦¨|\n" +
        "¦¨|‾¦¨|‾¦¨|¨|¨|‾¦‾¦‾¦¨|‾¦\n" +
        "¦‾¦‾¦‾¦¨|¨|¨|‾¦¨|‾¦¨|‾¦‾¦‾¦‾¦‾¦¨|‾¦‾¦‾¦¨|‾¦‾¦¨|‾¦\n" +
        "|‾¦¨|¨|‾¦‾¦¨|¨|¨|‾¦¨|¨|‾¦¨|¨|‾¦¨|‾¦¨|‾¦¨|¨|¨|¨|¨|\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells