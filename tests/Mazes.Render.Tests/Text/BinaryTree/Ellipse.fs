// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.BinaryTree.Ellipse

open System
open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Top BinaryTree.Direction.Right 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┛ ┬ ╭───╴ ┬ ┬ ┗━┓\n" +
        "┗━┓ ├─╯ ┬ ┬ ├─╯ ┏━┛\n" +
        "  ┗━┷━━━┪ ┢━┷━━━┛  \n" +
        "        ┗━┛        "

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Top, Left, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Top BinaryTree.Direction.Left 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┛ ┬ ┬ ╶───╮ ┬ ┗━┓\n" +
        "┗━┓ ╰─┤ ┬ ┬ ╰─┤ ┏━┛\n" +
        "  ┗━━━┷━┪ ┢━━━┷━┛  \n" +
        "        ┗━┛        "

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Bottom, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Bottom BinaryTree.Direction.Right 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━┯━┩ ┗━━━┯━┓  \n" +
        "┏━┛ │ ┴ ├───╮ │ ┗━┓\n" +
        "┗━┓ ╰─╴ ╰─╴ ┴ ┴ ┏━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Bottom, Left, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Bottom BinaryTree.Direction.Left 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━━━┛ ┡━┯━┯━┓  \n" +
        "┏━┛ │ ╭───┤ ┴ │ ┗━┓\n" +
        "┗━┓ ┴ ┴ ╶─╯ ╶─╯ ┏━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Right, Top, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Right BinaryTree.Direction.Top 1 1 1
    
    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┹───╴ ┬ ╭───╴ ┗━┓\n" +
        "┗━┱─╴ ╭─┴─┴─╴ ┬ ┏━┛\n" +
        "  ┗━━━┷━┓ ┏━━━┷━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Left, Top, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Top 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┛ ╶───╮ ┬ ╶───┺━┓\n" +
        "┗━┓ ┬ ╶─┴─┴─╮ ╶─┲━┛\n" +
        "  ┗━┷━━━┓ ┏━┷━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Right, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Right BinaryTree.Direction.Bottom 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┡━┯━━━┓  \n" +
        "┏━┹───┬─╴ │ ╰─╴ ┗━┓\n" +
        "┗━┱─╴ ╰─╴ ╰───╴ ┏━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the binary tree algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Bottom 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━┯━┩ ┗━━━━━┓  \n" +
        "┏━┛ ╶─╯ │ ╶─┬───┺━┓\n" +
        "┗━┓ ╶───╯ ╶─╯ ╶─┲━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 15, column radius 25, row translation factor 14, in outside mode ellipse maze generated with the binary tree algorithm (Bottom, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 15 25 0.0 0.0 14 0 None Shape.Ellipse.Side.Outside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Bottom BinaryTree.Direction.Right 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━┯━┯━┯━━━━━┯━┯━━━┯━━━┯━┯━┯━━━━━━━┯━━━━━━━━━┯━┯━┯━━━┯━━━┯━━━━━━━━━━━┯━━━━━━━┯━━━━━┯━━━━━┯━┯━━━━━┯━┓\n" +
        "┃ │ │ ╰─┬─╮ ┴ ├─╮ ╰─╮ ┴ │ ╰─┬─┬─╮ ├─┬───┬─╴ ┴ ┴ ├─╮ ├─╮ ╰─┬─────┬─╮ ├─┬─┬─╮ ├─┬─╮ ╰───╴ ┴ ├─┬─╴ │ ┃\n" +
        "┃ ┴ ├─╮ │ ├─╮ │ ├─╴ ├─╮ ├─╮ │ ┴ │ │ ╰─╴ ├─────╴ ┴ ┴ ┴ ╰─╮ ├─┬─╮ │ │ │ │ ┴ │ ┴ ┴ ├───────╮ │ ├─╴ │ ┃\n" +
        "┠─╮ ┴ ┴ │ │ ┴ │ ├─╴ ┴ │ ┴ ┴ ├─╮ ┴ ├───╴ ├─┬─────┬─────╮ │ ┴ ┴ │ ┴ ┴ ┴ ├─╮ ├───╮ ╰─────╮ ┴ ┴ ├─╮ ┴ ┃\n" +
        "┃ ├─┬─╴ │ ├─╴ ┴ ├───╮ ├─┬─╮ ┴ ├─╴ ├─┬─╴ │ ├─┬─╮ ├───╮ │ ├───╴ ╰─┬─┬─╴ ┴ │ ╰─╮ ├─┬───╴ ├─┬─╴ ┴ ╰─╮ ┃\n" +
        "┃ │ ├─╴ │ ╰─┬─╮ ├─╮ ┴ │ │ ╰─╮ ╰─╮ ┴ ╰─╴ │ │ │ ┴ ├─╮ ┴ │ ├───┬─╴ │ ╰─┬─╮ ├─╮ │ ┴ ├───╴ │ ├───┬─╴ │ ┃\n" +
        "┃ │ ├─╮ ╰─╮ │ │ │ ╰─╮ │ ╰─╴ ╰─╴ ├───┬─╴ │ │ ╰─╴ │ ├─╴ ┴ ├─╴ ╰─╮ ╰─╮ │ │ │ ┴ ├─╴ ├─┬─╴ │ ╰─╮ ╰─╮ ┴ ┃\n" +
        "┃ ┴ ┴ ├─╮ ┴ │ │ ├─╮ │ ├───────╮ ├─╮ ├─╴ ┴ ╰─┬─╮ ┴ ├───╮ ├─┬─╮ ╰─╮ │ ┴ │ ╰─╮ ├─╮ │ ╰─╴ ├─╮ ╰─╮ ├─╮ ┃\n" +
        "┠───╮ ┴ ╰─╮ ┴ │ ┴ ┴ ┴ ╰─────╴ │ ┴ │ ├─┬─┬─╮ │ ╰─╴ ├─╴ ┴ ┴ │ ╰─╴ │ ╰─╴ ╰─╴ ┴ ┴ │ ├───╴ │ ├─╮ │ ┴ │ ┃\n" +
        "┠─╴ ├─┬─╴ ╰─╴ ├─┬───────┬─┬─╴ ╰─╴ ┴ ┴ │ ┴ │ ├─┬─╮ ├─┬───╮ ╰───╴ ╰─┬─┬───────╮ │ ╰───╮ │ ┴ │ ╰─╮ ┴ ┃\n" +
        "┠─╮ ┴ ├─┬───╴ │ ├─────╮ ┴ ├───────┬─╴ ├─╴ ┴ ┴ ┴ ┴ │ ╰─╮ ╰─┬───┬─╮ ┴ ╰─┬───╮ │ ├─┬─╮ │ ╰─╮ ╰─╴ ├─╴ ┃\n" +
        "┃ ╰─╮ ┴ ├─┬─╴ │ ├─┬─╴ ╰─╴ ╰───┬─╮ ╰─╮ ╰─┬─────┬─╴ ╰─╴ ├─╮ ├─╴ ┴ ╰─┬─╮ ╰─╴ ┴ ┴ ┴ │ ┴ ├─╴ ╰─┬─╮ ╰─╴ ┃\n" +
        "┠─╮ ├─╮ ┴ ╰─╴ ┴ │ ├─┬───┬─┬─╴ │ ╰─╮ ├─╴ ├───╴ ╰─┬─┬─╴ ┴ ┴ ╰───┬─╴ │ ╰─┬───┬───╮ ├─╴ ├───╴ │ ├───╴ ┃\n" +
        "┃ ┴ │ ╰─┬───┬─╴ ┴ ┴ ├─╴ ┴ ╰─╮ ├─╮ ┴ ├─╮ ╰─────╮ ┴ ╰─┬─┬─────╴ ╰─╮ ╰─╮ ├─╴ ╰─╴ │ ╰─╮ ╰───╴ │ ╰─┬─╴ ┃\n" +
        "┠─╮ ╰─╮ ├─╴ ├─┬───╮ ╰─┬─┬─╮ │ │ ╰─╴ ┴ ╰─────╴ ┴ ┏━┓ ┴ ╰───────╴ ╰─╴ │ ├─┬───╴ ├─╮ ├───┬─╴ ╰─╮ ├─╴ ┃\n" +
        "┃ ├─╴ ┴ ╰─╴ ┴ ╰─╮ ├─╴ ┴ ┴ ┴ ┴ ┴ ┏━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━━━━━━━┓ ┴ ┴ ╰───╴ ┴ ┴ ╰─╴ ╰───╮ ┴ ╰─╴ ┃\n" +
        "┃ ╰───┬───┬───╮ │ ╰───╴ ┏━━━━━━━┛                                 ┗━━━━━━━┱─────────┬───╴ ├─┬─┬─╮ ┃\n" +
        "┠───╴ ├─╮ ╰─╴ ┴ ╰─╴ ┏━━━┛                                                 ┗━━━┱───╴ ├───╮ ┴ │ ┴ │ ┃\n" +
        "┠───╴ │ ╰─┬───╴ ┏━━━┛                                                         ┗━━━┓ ╰─╴ ╰─╴ ╰─╴ ┴ ┃\n" +
        "┠───╮ ╰─╴ ┴ ┏━━━┛                                                                 ┗━━━┱─────┬───╴ ┃\n" +
        "┠─╴ ╰─┬─╴ ┏━┛                                                                         ┗━┱─╴ ├───╮ ┃\n" +
        "┠─┬─╴ ┴ ┏━┛                                                                             ┗━┓ ╰─╮ ┴ ┃\n" +
        "┃ ├─╴ ┏━┛                                                                                 ┗━┓ ╰─╮ ┃\n" +
        "┃ │ ┏━┛                                                                                     ┗━┓ │ ┃\n" +
        "┃ ┴ ┃                                                                                         ┃ ┴ ┃\n" +
        "┃ ┏━┛                                                                                         ┗━┓ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┗━┛                                                                                             ┗━┛\n" +
        "                                                                                                   "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 15, column radius 25, row translation factor 14, in outside mode ellipse maze generated with the binary tree algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 15 25 0.0 0.0 14 0 None Shape.Ellipse.Side.Outside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Bottom 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━┯━┯━━━━━┯━┯━━━┯━┯━━━┯━┯━┯━━━┯━┯━┯━┯━┯━━━┯━━━┯━━━━━━━┯━┯━┯━┯━━━┯━┯━┯━━━━━━━┯━━━┯━━━━━┯━┯━━━━━━━┓\n" +
        "┃ ╶─┤ ┴ ╶─┬─┤ │ ╭─╯ ┴ ╶─╯ ┴ ┴ ╶─╯ ┴ │ │ ┴ ╭─╯ ╶─╯ ╶─┬─┬─┤ ┴ │ ┴ ╶─╯ ┴ ┴ ╭───┬─╯ ╭─╯ ╶─┬─╯ ┴ ╭─────┨\n" +
        "┃ ╶─┤ ╶───╯ │ │ │ ╶─┬─┬───┬─────────╯ ┴ ╶─╯ ╭─┬─┬─┬─┤ │ │ ╶─┤ ╭─────┬───╯ ╶─╯ ╶─┤ ╶───╯ ╶───╯ ╶─┬─┨\n" +
        "┃ ╭─╯ ╶─┬─┬─╯ │ │ ╭─╯ │ ╶─╯ ╶─┬─┬─┬───┬─┬───╯ │ │ ┴ │ │ ┴ ╶─┤ │ ╶─┬─╯ ╶─┬─┬───┬─┤ ╶───┬─────┬─┬─╯ ┃\n" +
        "┃ ┴ ╭─┬─┤ ┴ ╶─┤ │ ┴ ╶─╯ ╭───┬─┤ ┴ ┴ ╭─┤ │ ╶───╯ │ ╶─╯ ┴ ╶───┤ ┴ ╶─┤ ╶─┬─╯ ┴ ╶─╯ │ ╶─┬─┤ ╶───┤ ┴ ╶─┨\n" +
        "┃ ╶─┤ ┴ │ ╶───┤ │ ╶─┬───╯ ╶─╯ ┴ ╭───┤ ┴ │ ╶───┬─╯ ╶─┬───────┤ ╭─┬─╯ ╭─╯ ╭─────┬─╯ ╶─╯ ┴ ╭───┤ ╶───┨\n" +
        "┃ ╭─╯ ╭─╯ ╭───┤ ┴ ╶─┤ ╶─┬───────╯ ╭─╯ ╭─┤ ╶─┬─┤ ╶───┤ ╭─────┤ ┴ │ ╶─┤ ╭─┤ ╭───╯ ╭───────╯ ╭─╯ ╶───┨\n" +
        "┃ ┴ ╶─╯ ╭─╯ ╶─┤ ╭───╯ ╶─╯ ╭───┬───╯ ╭─╯ ┴ ╶─╯ │ ╶─┬─╯ ┴ ╭─┬─┤ ╶─╯ ╶─╯ │ │ │ ╶───╯ ╶─────┬─╯ ╶─┬─┬─┨\n" +
        "┃ ╶─┬───╯ ╶───┤ │ ╶───┬─┬─┤ ╭─┤ ╭───┤ ╭───┬─┬─┤ ╶─┤ ╭───╯ ┴ ┴ ╶───┬───┤ │ │ ╭─┬─┬─┬───┬─╯ ╭─┬─╯ │ ┃\n" +
        "┃ ╭─╯ ╭───┬───╯ │ ╭───╯ │ │ │ ┴ ┴ ╭─┤ │ ╭─╯ │ ┴ ╶─╯ ┴ ╶───┬───┬─┬─┤ ╭─┤ ┴ ┴ │ │ │ ┴ ╶─┤ ╭─┤ ┴ ╶─┤ ┃\n" +
        "┃ │ ╶─┤ ╭─╯ ╭───╯ ┴ ╶───╯ │ ┴ ╭─┬─╯ ┴ │ ┴ ╭─╯ ╭───┬─┬─┬─┬─┤ ╶─┤ ┴ │ │ │ ╶─┬─╯ │ │ ╶───┤ │ ┴ ╶─┬─╯ ┃\n" +
        "┃ │ ╭─╯ ┴ ╭─┤ ╶─┬───┬─┬─┬─┤ ╭─╯ ┴ ╭─┬─┤ ╶─╯ ╶─┤ ╭─┤ ┴ │ │ ┴ ╭─╯ ╭─╯ ┴ │ ╭─┤ ╭─┤ ┴ ╶───┤ ┴ ╶─┬─╯ ╭─┨\n" +
        "┃ │ │ ╶───┤ │ ╶─┤ ╶─╯ │ ┴ │ ┴ ╭───┤ ┴ │ ╭─┬─┬─┤ ┴ ┴ ╭─┤ │ ╶─┤ ╶─╯ ╭───┤ ┴ ┴ │ ┴ ╶───┬─┤ ╭─┬─╯ ╶─╯ ┃\n" +
        "┃ │ ┴ ╭───┤ │ ╭─╯ ╭───┤ ╭─┤ ╶─╯ ╭─╯ ╭─┤ │ │ ┴ ┴ ╶───╯ │ │ ╭─╯ ╶─┬─╯ ╶─╯ ╭─┬─┤ ╶─┬─┬─┤ ┴ │ ┴ ╭───┬─┨\n" +
        "┃ │ ╶─╯ ╭─┤ ┴ │ ╶─╯ ╶─┤ │ ┴ ╶───╯ ╶─╯ ┴ ┴ ┴ ╶───┲━┓ ╶─╯ ┴ ┴ ╶───╯ ╶─────╯ ┴ ┴ ╭─╯ │ ┴ ╶─┤ ╶─╯ ╭─╯ ┃\n" +
        "┃ │ ╭─┬─╯ │ ╭─┤ ╭─┬─┬─┤ ┴ ╶─────┲━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━━━━━━━┓ ╶───────┬─┤ ╶─╯ ╭─┬─┤ ╭─┬─┤ ╶─┨\n" +
        "┃ ┴ ┴ ┴ ╶─┤ │ ┴ │ │ ┴ ┴ ┏━━━━━━━┛                                 ┗━━━━━━━┓ ┴ ┴ ╭───╯ │ ┴ │ ┴ │ ╭─┨\n" +
        "┃ ╶─┬───┬─╯ │ ╶─╯ ┴ ┏━━━┛                                                 ┗━━━┓ ┴ ╶─┬─┤ ╭─╯ ╶─┤ │ ┃\n" +
        "┃ ╭─┤ ╭─┤ ╭─╯ ╶─┲━━━┛                                                         ┗━━━┓ ┴ ┴ ┴ ╭───┤ │ ┃\n" +
        "┃ │ │ ┴ │ ┴ ┏━━━┛                                                                 ┗━━━┓ ╶─┤ ╭─╯ │ ┃\n" +
        "┃ ┴ │ ╶─╯ ┏━┛                                                                         ┗━┓ ┴ ┴ ╭─┤ ┃\n" +
        "┃ ╭─╯ ╶─┲━┛                                                                             ┗━┓ ╶─┤ ┴ ┃\n" +
        "┃ ┴ ╶─┲━┛                                                                                 ┗━┓ ┴ ╶─┨\n" +
        "┃ ╶─┲━┛                                                                                     ┗━┓ ╶─┨\n" +
        "┃ ╶─┨                                                                                         ┃ ╶─┨\n" +
        "┃ ┏━┛                                                                                         ┗━┓ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┗━┛                                                                                             ┗━┛\n" +
        "                                                                                                   "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 25, column radius 15, column translation factor 14, in outside mode ellipse maze generated with the binary tree algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 25 15 0.0 0.0 0 14 None Shape.Ellipse.Side.Outside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Top BinaryTree.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓  \n" +
        "┃ ┬ ┬ ╭───╴ ┬ ╭─╴ ╭─╴ ┬ ┬ ╭─────╴ ╭───────╴ ┬ ┬ ┬ ┏━━━━━┛  \n" +
        "┠─╯ ├─╯ ╭───┴─┴───╯ ╭─┴─┴─╯ ╭───╴ ├───╴ ┬ ╭─╯ ┢━┷━┛        \n" +
        "┠─╴ │ ┬ ├─╴ ┬ ╭─╴ ┬ ├─╴ ╭─╴ ├─╴ ┬ │ ┬ ┬ ├─╯ ┏━┛            \n" +
        "┃ ╭─┴─┴─╯ ┬ │ │ ╭─╯ ├───╯ ┬ │ ┬ │ │ │ │ │ ┏━┛              \n" +
        "┃ ├───────╯ │ ├─╯ ╭─╯ ┬ ┬ │ │ │ │ ├─╯ │ ┢━┛                \n" +
        "┃ │ ┬ ┬ ╭─╴ │ ├───╯ ╭─┴─┴─┴─┴─┴─┴─╯ ┬ ┢━┛                  \n" +
        "┃ │ │ │ │ ┬ │ ├─╴ ╭─┴─╴ ╭─────╴ ┬ ┬ │ ┃                    \n" +
        "┠─╯ │ ├─┴─╯ │ ├─╴ │ ╭───╯ ╭───╴ │ │ ┢━┛                    \n" +
        "┠─╴ ├─┴─╴ ┬ ├─┴─╴ ├─┴─╴ ┬ ├───╴ ├─╯ ┃                      \n" +
        "┠───╯ ┬ ┬ ├─╯ ╭───┴─╴ ╭─┴─╯ ┬ ╭─╯ ┏━┛                      \n" +
        "┃ ┬ ╭─╯ │ ├───╯ ╭─╴ ┬ │ ┬ ╭─╯ ├─╴ ┃                        \n" +
        "┃ │ ├─╴ │ │ ┬ ┬ ├─╴ │ │ ├─┴───╯ ┏━┛                        \n" +
        "┃ │ ├───╯ ├─╯ │ │ ╭─┴─╯ │ ╭───╴ ┃                          \n" +
        "┠─╯ │ ┬ ╭─╯ ╭─╯ │ │ ┬ ╭─╯ │ ╭─╴ ┃                          \n" +
        "┃ ╭─╯ ├─┴───╯ ┬ │ ├─╯ │ ╭─╯ │ ┬ ┃                          \n" +
        "┠─╯ ╭─╯ ╭─╴ ┬ │ │ │ ╭─╯ ├───╯ ┢━┛                          \n" +
        "┃ ┬ ├─╴ ├─╴ │ │ │ ├─╯ ┬ │ ┬ ┬ ┃                            \n" +
        "┠─╯ │ ╭─┴───┴─╯ ├─╯ ╭─╯ │ ├─╯ ┃                            \n" +
        "┠─╴ │ ├───╴ ╭───╯ ╭─╯ ┬ │ │ ┬ ┃                            \n" +
        "┠─╴ ├─╯ ┬ ╭─╯ ╭─╴ ├─╴ ├─┴─┴─╯ ┃                            \n" +
        "┃ ┬ ├─╴ │ │ ┬ │ ┬ ├───┴─╴ ┬ ┬ ┃                            \n" +
        "┃ │ ├───┴─╯ │ ├─╯ ├─╴ ┬ ┬ │ │ ┃                            \n" +
        "┠─╯ │ ╭─╴ ╭─╯ │ ┬ │ ╭─┴─╯ │ │ ┃                            \n" +
        "┠─╴ │ │ ╭─╯ ╭─┴─╯ ├─╯ ╭───╯ ┢━┛                            \n" +
        "┠───┴─┴─┴─╴ ├─╴ ┬ │ ┬ │ ┬ ┬ ┗━┓                            \n" +
        "┠─╴ ╭─────╴ ├───╯ ├─┴─┴─┴─┴─╴ ┃                            \n" +
        "┠─╴ │ ╭───╴ │ ┬ ┬ ├─╴ ╭─╴ ┬ ┬ ┃                            \n" +
        "┠───┴─╯ ┬ ╭─┴─┴─╯ │ ╭─┴───┴─╯ ┃                            \n" +
        "┠─╴ ╭─╴ │ │ ┬ ┬ ┬ ├─╯ ╭─────╴ ┃                            \n" +
        "┠─╴ │ ╭─┴─┴─╯ │ ├─┴─╴ │ ╭─╴ ┬ ┃                            \n" +
        "┠─╴ ├─╯ ╭─╴ ┬ ├─┴─╴ ┬ ├─┴─╴ │ ┃                            \n" +
        "┠─╴ ├───┴─╴ ├─╯ ╭───┴─┴───╴ │ ┃                            \n" +
        "┠─╴ ├─╴ ╭─╴ │ ╭─┴─╴ ╭─╴ ┬ ┬ │ ┗━┓                          \n" +
        "┃ ┬ ├─╴ ├───╯ ├───╴ ├─╴ │ ├─╯ ┬ ┃                          \n" +
        "┃ │ ├───┴─────╯ ┬ ╭─╯ ╭─╯ ├───╯ ┃                          \n" +
        "┃ ├─┴───╴ ┬ ┬ ╭─┴─┴─╴ │ ╭─┴───╴ ┃                          \n" +
        "┠─┴─╴ ╭─╴ ├─┴─╯ ┬ ╭───╯ │ ┬ ╭─╴ ┗━┓                        \n" +
        "┠───╴ │ ┬ ├─╴ ┬ ├─╯ ╭─╴ │ ├─╯ ╭─╴ ┃                        \n" +
        "┠───╴ │ ├─┴───┴─┴─╴ ├─╴ ├─╯ ╭─╯ ┬ ┗━┓                      \n" +
        "┃ ┬ ╭─╯ ├───╴ ┬ ╭───┴─╴ ├─╴ ├─╴ ├─╴ ┃                      \n" +
        "┠─┴─╯ ╭─┴───╴ │ │ ╭─╴ ┬ ├───┴─╴ │ ┬ ┗━┓                    \n" +
        "┃ ╭───┴───╴ ╭─╯ │ ├───┴─╯ ╭─╴ ╭─┴─┴─╴ ┃                    \n" +
        "┃ ├─╴ ╭─╴ ╭─╯ ┬ ├─╯ ┬ ╭─╴ ├─╴ │ ┬ ┬ ┬ ┗━┓                  \n" +
        "┠─╯ ┬ │ ╭─┴─╴ │ │ ╭─╯ ├───╯ ┬ ├─╯ ├─┴─╴ ┗━┓                \n" +
        "┠───┴─╯ │ ╭───┴─┴─┴───┴─────┴─╯ ╭─┴─────╴ ┗━┓              \n" +
        "┃ ╭─╴ ╭─╯ │ ╭───────╴ ╭───╴ ┬ ┬ │ ╭───╴ ┬ ┬ ┗━┓            \n" +
        "┠─┴───╯ ╭─╯ ├─╴ ╭─╴ ╭─┴─╴ ╭─╯ │ ├─┴─────┴─┴─╴ ┗━━━┓        \n" +
        "┠───────╯ ╭─┴───┴─╴ ├─╴ ╭─╯ ┬ ├─╯ ┬ ╭─╴ ┬ ┬ ┬ ┬ ┬ ┗━━━━━┓  \n" +
        "┗━━━━━━━━━┷━━━━━━━━━┷━━━┷━━━┷━┷━━━┷━┷━━━┷━┷━┷━┷━┷━━━━━━━┛  "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 25, column radius 15, column translation factor 14, in outside mode ellipse maze generated with the binary tree algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Ellipse.create 25 15 0.0 0.0 0 14 None Shape.Ellipse.Side.Outside)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid

    let maze =
        grid
        |> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Bottom 1 1 1
    
    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━┯━┯━┯━┯━━━┯━┯━┯━━━━━━━┯━━━┯━━━━━┯━┯━━━━━━━━━━━━━┓  \n" +
        "┃ ╭─────┤ │ ┴ │ ╭─╯ │ │ ╭───┬─┤ ╭─┤ ╭───┤ ┴ ╭─────┲━━━━━┛  \n" +
        "┃ │ ╶───╯ ┴ ╶─┤ ┴ ╭─╯ │ ┴ ╶─┤ ┴ ┴ │ ┴ ╶─╯ ╭─╯ ┏━━━┛        \n" +
        "┃ ┴ ╶─────────╯ ╶─┤ ╭─╯ ╭───╯ ╶───┤ ╭─┬───╯ ┏━┛            \n" +
        "┃ ╶─┬─────────────╯ │ ╶─┤ ╶───┬─┬─┤ │ ┴ ╶─┲━┛              \n" +
        "┃ ╶─┤ ╭─┬─┬─┬─┬─┬───┤ ╭─╯ ╶─┬─╯ ┴ ┴ ┴ ╶─┲━┛                \n" +
        "┃ ╶─╯ ┴ │ │ │ ┴ │ ╭─╯ │ ╶───╯ ╶───────┲━┛                  \n" +
        "┃ ╶───┬─┤ ┴ │ ╭─╯ ┴ ╭─╯ ╶─┬─┬─────┬───┨                    \n" +
        "┃ ╭───┤ │ ╶─╯ │ ╭───┤ ╭───╯ │ ╭───┤ ┏━┛                    \n" +
        "┃ │ ╶─╯ │ ╭───┤ │ ╭─╯ │ ╶───╯ │ ╭─╯ ┃                      \n" +
        "┃ │ ╶─┬─╯ ┴ ╶─╯ │ ┴ ╭─┤ ╶───┬─╯ ┴ ┏━┛                      \n" +
        "┃ │ ╭─┤ ╶─────┬─╯ ╶─╯ ┴ ╶─┬─╯ ╶───┨                        \n" +
        "┃ │ │ ┴ ╶─┬─┬─╯ ╶───┬───┬─┤ ╶───┲━┛                        \n" +
        "┃ │ ┴ ╶─┬─╯ ┴ ╶───┬─╯ ╭─╯ ┴ ╶─┬─┨                          \n" +
        "┃ ┴ ╶─┬─╯ ╶─┬─────╯ ╭─┤ ╭───┬─╯ ┃                          \n" +
        "┃ ╭─┬─╯ ╭───╯ ╶─────┤ ┴ │ ╶─┤ ╶─┨                          \n" +
        "┃ ┴ ┴ ╶─╯ ╭─────────┤ ╶─┤ ╶─╯ ┏━┛                          \n" +
        "┃ ╭─────┬─╯ ╭───┬─┬─┤ ╭─╯ ╶─┬─┨                            \n" +
        "┃ ┴ ╶───╯ ╭─╯ ╭─┤ ┴ │ │ ╶───┤ ┃                            \n" +
        "┃ ╭─┬─┬───┤ ╶─┤ ┴ ╭─╯ ┴ ╭───┤ ┃                            \n" +
        "┃ ┴ ┴ │ ╭─┤ ╶─╯ ╶─╯ ╶───┤ ╶─╯ ┃                            \n" +
        "┃ ╶───╯ ┴ │ ╶─┬─────┬─┬─┤ ╶───┨                            \n" +
        "┃ ╶───┬─┬─╯ ╶─╯ ╶─┬─╯ │ ┴ ╶─┬─┨                            \n" +
        "┃ ╭─┬─╯ │ ╶─┬─┬───┤ ╶─╯ ╶─┬─╯ ┃                            \n" +
        "┃ ┴ ┴ ╶─╯ ╶─╯ │ ╶─┤ ╭─┬─┬─┤ ┏━┛                            \n" +
        "┃ ╭─┬─┬─┬───┬─┤ ╶─┤ │ │ ┴ │ ┡━┓                            \n" +
        "┃ ┴ │ ┴ │ ╶─╯ ┴ ╶─┤ │ ┴ ╶─┤ │ ┃                            \n" +
        "┃ ╭─┤ ╭─╯ ╶─┬─┬─┬─╯ ┴ ╭─┬─┤ ┴ ┃                            \n" +
        "┃ │ │ ┴ ╭───╯ ┴ ┴ ╶───┤ ┴ │ ╭─┨                            \n" +
        "┃ │ ┴ ╶─┤ ╭─────┬─┬─┬─╯ ╶─┤ │ ┃                            \n" +
        "┃ │ ╭───╯ │ ╭───╯ │ ┴ ╭───┤ ┴ ┃                            \n" +
        "┃ │ │ ╭─┬─┤ ┴ ╭───┤ ╭─┤ ╶─┤ ╶─┨                            \n" +
        "┃ ┴ ┴ │ ┴ │ ╭─╯ ╶─┤ ┴ │ ╶─┤ ╶─┨                            \n" +
        "┃ ╶───┤ ╶─┤ │ ╶─┬─┤ ╶─┤ ╶─╯ ╶─┺━┓                          \n" +
        "┃ ╶─┬─╯ ╭─╯ ┴ ╭─┤ │ ╭─┤ ╶─────┬─┨                          \n" +
        "┃ ╭─╯ ╶─┤ ╭─┬─╯ ┴ ┴ │ │ ╭───┬─┤ ┃                          \n" +
        "┃ ┴ ╶─┬─┤ ┴ ┴ ╭─┬───┤ ┴ │ ╭─┤ │ ┃                          \n" +
        "┃ ╶───┤ ┴ ╭───╯ │ ╶─╯ ╶─┤ │ │ │ ┗━┓                        \n" +
        "┃ ╭───┤ ╶─┤ ╭─┬─┤ ╭─────┤ │ │ ┴ ╭─┨                        \n" +
        "┃ │ ╶─┤ ╭─┤ ┴ ┴ │ │ ╶─┬─╯ ┴ │ ╶─┤ ┗━┓                      \n" +
        "┃ │ ╶─╯ │ ┴ ╶───┤ │ ╭─╯ ╭─┬─┤ ╶─┤ ╶─┨                      \n" +
        "┃ ┴ ╭─┬─┤ ╶───┬─╯ │ │ ╭─┤ ┴ ┴ ╶─╯ ╭─╄━┓                    \n" +
        "┃ ╶─┤ ┴ ┴ ╭───╯ ╭─╯ │ ┴ │ ╶─┬─┬─┬─╯ │ ┃                    \n" +
        "┃ ╭─┤ ╶─┬─╯ ╶───┤ ╭─╯ ╶─╯ ╭─╯ ┴ ┴ ╶─╯ ┡━┓                  \n" +
        "┃ │ │ ╭─┤ ╭─┬─┬─┤ ┴ ╶─┬─┬─┤ ╭─┬─┬───┬─╯ ┗━┓                \n" +
        "┃ │ │ ┴ │ │ │ │ ┴ ╶─┬─╯ │ ┴ │ │ │ ╭─┤ ╶─┬─╄━┓              \n" +
        "┃ ┴ │ ╭─╯ │ ┴ │ ╶─┬─╯ ╭─┤ ╭─┤ ┴ ┴ │ │ ╶─╯ ┴ ┗━┓            \n" +
        "┃ ╶─┤ ┴ ╭─╯ ╭─┤ ╭─┤ ╶─┤ │ │ │ ╶─┬─┤ │ ╭─┬─┬───┺━┯━┓        \n" +
        "┃ ╶─╯ ╶─╯ ╶─╯ ┴ ┴ ┴ ╶─╯ ┴ ┴ ┴ ╶─╯ ┴ ┴ ┴ ┴ ┴ ╶───╯ ┗━━━━━┓  \n" +
        "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛  "
        
    renderedMaze |> should equal expectedRenderedMaze