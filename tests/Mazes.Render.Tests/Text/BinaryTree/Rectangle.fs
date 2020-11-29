// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.BinaryTree.Rectangle

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a 3 by 3 maze generated with the binary tree algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 3 3)
        |> Grid.create
        |> BinaryTree.createMaze Top Right 1 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━┓\n" +
        "┃ ┬ ┬ ┃\n" +
        "┃ ├─╯ ┃\n" +
        "┗━┷━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 5 maze generated with the binary tree algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 5 5)
        |> Grid.create
        |> BinaryTree.createMaze Top Right 1 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━┓\n" +
        "┃ ┬ ┬ ╭─╴ ┃\n" +
        "┠─╯ │ ├─╴ ┃\n" +
        "┃ ╭─╯ │ ┬ ┃\n" +
        "┃ ├───┴─╯ ┃\n" +
        "┗━┷━━━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 10 maze generated with the binary tree algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 5 10)
        |> Grid.create
        |> BinaryTree.createMaze Top Right 1 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━┓\n" +
        "┃ ┬ ┬ ╭───╴ ┬ ╭─╴ ┬ ┃\n" +
        "┠─╯ │ │ ╭───┴─╯ ╭─╯ ┃\n" +
        "┠───┴─╯ │ ┬ ╭─╴ ├─╴ ┃\n" +
        "┃ ╭─────┴─┴─╯ ╭─┴─╴ ┃\n" +
        "┗━┷━━━━━━━━━━━┷━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze