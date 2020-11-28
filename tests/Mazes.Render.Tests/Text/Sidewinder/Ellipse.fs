﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Sidewinder.Ellipse

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┹─╴ ┬ ╶───╮ ┬ ┗━┓\n" +
        "┗━┓ ┬ │ ┬ ╭─╯ ╰─┲━┛\n" +
        "  ┗━┷━┷━┪ ┢━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Top, Left, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Left (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┛ ┗━━━━━┓  \n" +
        "┏━┛ ┬ ┬ ╶───┬─╴ ┗━┓\n" +
        "┗━┱─╯ ╰─╮ ┬ │ ┬ ┏━┛\n" +
        "  ┗━━━━━┪ ┢━┷━┷━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Bottom, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Bottom Sidewinder.Direction.Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┩ ┗━━━┯━┓  \n" +
        "┏━┹─╴ ╶─╯ ╭─┬─┤ ┗━┓\n" +
        "┗━┱───╴ ╶─╯ ┴ ┴ ┏━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Bottom, Left, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Bottom Sidewinder.Direction.Left (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━━━┛ ┡━━━━━┓  \n" +
        "┏━┛ │ ╭─┬─┴─╴ ╶─┺━┓\n" +
        "┗━┓ ┴ ┴ ╰───╴ ╶─┲━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Right, Top, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Right Sidewinder.Direction.Top (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━┯━┛ ┗━━━┯━┓  \n" +
        "┏━┛ │ ╰───┬─╴ │ ┗━┓\n" +
        "┗━┓ ╰─╴ ┬ ╰─╴ ┴ ┏━┛\n" +
        "  ┗━━━━━┪ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Left, Top, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Top (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━━━━━┩ ┡━━━━━┓  \n" +
        "┏━┛ ╭───╯ ┴ ╶───┺━┓\n" +
        "┗━┓ ┴ ┬ ┬ ╶───╮ ┏━┛\n" +
        "  ┗━━━┷━┪ ┏━━━┷━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━━━┩ ┗━━━━━┓  \n" +
        "┏━┛ ┴ ┬ ┴ ╭───╮ ┗━┓\n" +
        "┗━┓ ╶─┴─╮ ┴ ╶─┴─┲━┛\n" +
        "  ┗━━━━━┪ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Right, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Right Sidewinder.Direction.Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━┓        \n" +
        "  ┏━┯━━━┛ ┡━━━┯━┓  \n" +
        "┏━┛ ├─╮ ┬ ╰─╴ │ ┗━┓\n" +
        "┗━┓ ┴ ╰─┴───╴ ┴ ┏━┛\n" +
        "  ┗━━━━━┓ ┏━━━━━┛  \n" +
        "        ┗━┛        "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 15, column radius 25, row translation factor 14, in outside mode ellipse maze generated with the sidewinder algorithm (Bottom, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 15 25 0.0 0.0 14 0 Shape.Ellipse.Side.Outside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Bottom Sidewinder.Direction.Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━┯━━━━━┯━┯━━━━━┯━┯━┯━┯━━━━━━━┯━┯━┯━┯━┯━┯━┯━┯━━━┯━━━━━┯━━━━━━━┯━━━┯━━━┯━┯━┯━━━━━━━━━┯━┯━┯━━━┓\n" +
        "┠───╮ ╭─╯ ╶───╯ ╰───╴ ┴ ┴ │ │ ╶───┬─┤ ┴ ┴ ┴ ┴ ┴ ┴ ├─╴ ╰───╴ │ ╶─┬───┤ ╭─┤ ╭─╯ ┴ ╰─╴ ╶─────┤ │ ┴ ╭─┨\n" +
        "┃ ╶─╯ ╰─┬───┬─────┬───╮ ╶─┤ ┴ ╶─┬─┤ ╰───╮ ╶─┬─────┤ ╶─┬─┬───┴─╮ ├─╮ │ ┴ ┴ ├───┬─╴ ╭───┬───╯ ╰─╮ │ ┃\n" +
        "┃ ╭─────┴─╴ │ ╶───┼─╮ ╰─╮ ┴ ╭───╯ ├───╴ ├─╮ ╰───╮ ├─╴ ┴ ┴ ╭───┤ │ │ ├─╴ ╶─┤ ╶─┼─╴ │ ╭─┼─╴ ╶───╯ │ ┃\n" +
        "┃ │ ╶─┬─┬───┴───╮ ┴ ╰─╴ │ ╶─┼─┬─╴ ├─┬─╴ │ ┴ ╶─┬─╯ ├─╴ ╭───╯ ╶─┤ │ ┴ │ ╭─┬─┤ ╭─╯ ╭─╯ ┴ ┴ ╶───┬─┬─╯ ┃\n" +
        "┃ ┴ ╭─┤ ┴ ╭─┬───┴─╴ ╭───┼─╮ ┴ ┴ ╶─┤ ╰─╴ ├─╴ ╶─┼─╮ ┴ ╭─┤ ╭─┬─┬─┤ ┴ ╶─╯ ┴ ┴ │ ┴ ╭─┼─┬───┬─┬─╮ ┴ ┴ ╭─┨\n" +
        "┠─╴ ┴ ┴ ╭─┤ ┴ ╭─┬───┤ ╭─╯ ├─┬─┬─╮ ┴ ╶───┴─╴ ╭─┤ │ ╶─╯ │ │ ┴ │ ╰───┬─┬─┬─╮ ╰─╮ ┴ ┴ ├─╴ │ │ ├─┬─╮ │ ┃\n" +
        "┃ ╶─┬─┬─┤ ╰─╮ ┴ ├─╴ ┴ ╰─╴ ┴ │ │ ╰─┬─╴ ╭───┬─╯ ┴ ├─┬─╮ ┴ ├─╮ │ ╶─┬─┤ ┴ ┴ ├─╮ ╰─┬─╮ │ ╭─┤ ┴ ┴ │ ┴ ┴ ┃\n" +
        "┃ ╭─┤ ┴ ╰─╴ │ ╶─┤ ╭───┬─┬─┬─╯ ├─╮ │ ╭─┤ ╭─┼───╮ │ ┴ ┴ ╭─┤ │ ├─╮ │ ├─╮ ╶─╯ ├─╴ │ │ ┴ ┴ ╰─┬─╮ ┴ ╭───┨\n" +
        "┃ │ ╰─╮ ╭─┬─┤ ╭─┤ ┴ ╭─╯ │ ├─╴ ┴ ┴ ┴ │ ┴ ┴ │ ╶─╯ ╰─╮ ╭─╯ ┴ │ │ │ │ ┴ ┴ ╭─┬─┼─╮ │ ╰─┬─╮ ╭─┤ │ ╶─┼─╮ ┃\n" +
        "┃ │ ╶─╯ ┴ ┴ │ ┴ ┴ ╭─┤ ╶─┤ │ ╭─┬───┬─┴─╮ ╭─┤ ╶─┬─┬─╯ ├─╮ ╶─┤ │ ┴ ├─╴ ╭─╯ ┴ ┴ ┴ ├─╮ ┴ ┴ │ ┴ ╰─╮ ┴ │ ┃\n" +
        "┃ ├─┬───╮ ╶─┼───╮ │ │ ╭─┤ │ │ ┴ ╶─┴─╴ │ │ ├─╮ ┴ ├─╮ ┴ ┴ ╭─┤ ┴ ╶─╯ ╭─┼─╴ ╭─────╯ ╰─╮ ╭─┼─╮ ╶─┼─╴ ┴ ┃\n" +
        "┃ ┴ ╰─╮ │ ╭─╯ ╭─┤ │ ┴ ┴ ┴ │ ╰─╴ ╭─────┤ │ │ ├─╴ │ ┴ ╭───╯ ├─┬─╮ ╶─╯ │ ╭─┤ ╭─┬───┬─┤ │ ┴ ├─╮ ├─┬─╮ ┃\n" +
        "┠───╮ │ │ │ ╭─┤ │ ├─╮ ╭───┴───╴ ┴ ╶─┬─┤ ┴ ┴ │ ╶─┴─╴ ├───╮ ┴ │ ╰─╮ ╶─╯ ┴ │ ┴ ┴ ╶─┤ ┴ │ ╶─┤ │ │ ┴ │ ┃\n" +
        "┠─╮ ┴ ┴ │ ┴ ┴ ┴ │ ┴ │ ╰─┬─┬─┬───╴ ╶─╯ ╰─╴ ╶─╯ ╶─┲━┓ ╰─╴ ┴ ╶─┴─╴ ┴ ╶─────┼─╮ ╭───┤ ╶─╯ ╭─╯ ┴ ╰─╴ │ ┃\n" +
        "┃ ├─╮ ╭─┴─╴ ╭───┴─╮ │ ╶─╯ ┴ ╰─╴ ┏━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━━━━━━━┱─╴ ╶─╯ ┴ ├─╮ ├─┬─╮ ╰───┬─╴ ╭─┤ ┃\n" +
        "┃ ┴ ┴ ┴ ╶─┬─┼─╴ ╭─┤ ┴ ╶─┲━━━━━━━┛                                 ┗━━━━━━━┓ ┴ ┴ ┴ │ ┴ ╭─┬─╯ ╭─╯ ┴ ┃\n" +
        "┠─╮ ╶─┬─┬─┤ ╰─╴ ┴ ┴ ┏━━━┛                                                 ┗━━━┱─╴ ┴ ╭─╯ ┴ ╶─┴───╴ ┃\n" +
        "┃ │ ╶─╯ │ ╰───╴ ┏━━━┛                                                         ┗━━━┓ ╰─────┬─╴ ╭───┨\n" +
        "┃ ├─╴ ╶─┤ ╶─┲━━━┛                                                                 ┗━━━┓ ╶─┼─╮ │ ╭─┨\n" +
        "┃ ┴ ╭───╯ ┏━┛                                                                         ┗━┓ ┴ │ │ ┴ ┃\n" +
        "┃ ╶─┴─╴ ┏━┛                                                                             ┗━┓ ┴ ╰─╴ ┃\n" +
        "┠─╮ ╶─┲━┛                                                                                 ┗━┱───╴ ┃\n" +
        "┃ │ ┏━┛                                                                                     ┗━┱─╴ ┃\n" +
        "┃ ┴ ┃                                                                                         ┃ ╶─┨\n" +
        "┃ ┏━┛                                                                                         ┗━┓ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┗━┛                                                                                             ┗━┛\n" +
        "                                                                                                   "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 15, column radius 25, row translation factor 14, in outside mode ellipse maze generated with the sidewinder algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 15 25 0.0 0.0 14 0 Shape.Ellipse.Side.Outside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━┯━━━┯━┯━┯━━━┯━━━━━┯━━━━━┯━━━━━┯━━━━━┯━━━━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━┯━━━┯━━━━━┯━┯━┓\n" +
        "┃ │ ╶─╯ ┴ ┴ ┬ ┴ ┬ ╶─╯ ┬ ╭─╯ ╭───┤ ╶───╯ ╶─╮ ╶───╮ ┬ ╶─┬─╮ ┬ ╶─╯ ╶─╮ ╶───────╮ ╶───┬─╯ ╶─╯ ┬ ╶─╯ ┴ ┃\n" +
        "┃ ┴ ╭─╮ ┬ ┬ ╰─┬─┤ ┬ ┬ │ ┴ ╶─╯ ┬ │ ┬ ┬ ┬ ╶─┤ ╶───┴─┴───╯ │ ╰─╮ ╶─┬─┴───╮ ┬ ╶─┴─────┤ ┬ ┬ ╶─┤ ┬ ╶───┨\n" +
        "┃ ┬ │ ╰─┤ ╰─┬─╯ │ │ ╰─┤ ╭───╮ ├─╯ │ ├─┴───┤ ┬ ╶─╮ ╶─────┤ ╶─┴───╯ ╭─╮ ├─┤ ┬ ╶─────╯ ╰─┴─╮ ╰─┼─╮ ╶─┨\n" +
        "┃ ╰─╯ ╭─┴─┬─┤ ┬ ├─┤ ┬ │ ┴ ┬ ╰─╯ ╭─┤ │ ┬ ╶─┤ │ ╶─┴───╮ ╶─┤ ╶─┬─╮ ╶─╯ ╰─╯ │ │ ╶─────┬─╮ ╶─┴─╮ │ │ ╶─┨\n" +
        "┃ ┬ ╶─┤ ┬ ┴ ┴ ╰─┤ ╰─┴─┼───┼───╮ ┴ ╰─╯ │ ╶─┴─┴─┬───╮ │ ╶─┤ ╭─╯ ╰───╮ ╶───┤ ╰─╮ ╶───┤ ╰─┬───┴─╯ │ ┬ ┃\n" +
        "┃ │ ┬ │ │ ╶─┬─╮ ┴ ╭───╯ ┬ │ ┬ ╰───┬─╮ ╰───┬───┤ ┬ ╰─┼───┼─╯ ╶───╮ ├─────┴───┼─────╯ ╶─╯ ╶─┬───┤ ╰─┨\n" +
        "┃ ╰─┤ │ │ ╶─╯ ├───╯ ╶─╮ ╰─┤ │ ╶───╯ │ ╶───╯ ╭─╯ ╰─┬─╯ ╶─╯ ┬ ╭───┴─╯ ╶─╮ ╶─┬─╯ ╶─────┬─┬─╮ │ ╶─┼─╮ ┃\n" +
        "┃ ╭─┤ │ │ ┬ ╭─╯ ╭─╮ ┬ │ ┬ │ │ ┬ ╭───┤ ╶─────╯ ╭───╯ ╶───╮ ╰─╯ ╶───╮ ╶─┤ ╶─╯ ╭─╮ ╭─╮ ┴ ┴ │ ┴ ╶─╯ │ ┃\n" +
        "┃ │ ╰─┤ ├─┤ │ ╶─╯ │ │ ├─┤ │ ╰─┴─╯ ╭─┴─┬───╮ ╶─┤ ╭─╮ ╭───┴───┬─╮ ┬ ├───┴─┬─╮ │ ╰─┤ ├─┬─╮ ├─╮ ╶─┬─┼─┨\n" +
        "┃ ┴ ┬ │ ┴ ├─╯ ┬ ╶─┴─┤ ┴ │ ┴ ┬ ╶───╯ ╶─╯ ╭─┴─┬─╯ ┴ ╰─┤ ╶─╮ ╶─┤ ├─┴─╯ ╭─┬─╯ │ ┴ ╶─┤ ┴ ┴ ╰─╯ │ ╭─╯ ┴ ┃\n" +
        "┃ ╶─┴─┤ ╭─╯ ╭─┼─╮ ┬ │ ╶─┴───┴───────┬─┬─╯ ┬ ┴ ┬ ╶───╯ ┬ │ ╶─┤ ┴ ┬ ╶─╯ │ ╶─┼─────┤ ╶─┬───╮ │ │ ┬ ╭─┨\n" +
        "┃ ╶─╮ ┴ ┴ ╶─╯ ┴ ├─┴─┴─────┬─┬───┬─┬─┤ │ ╶─┼─╮ ╰─────╮ ╰─┤ ╭─╯ ╶─┼─╮ ┬ ┴ ┬ │ ╶───┤ ┬ ┴ ╭─┴─┴─╯ ╰─┤ ┃\n" +
        "┃ ╶─┴─╮ ╶─╮ ╶─╮ │ ╶───╮ ┬ │ │ ╶─╯ ┴ ┴ ┴ ┬ │ ╰───────┴─╮ ╰─╯ ┬ ╶─╯ ╰─┤ ┬ │ ┴ ╭─┬─╯ │ ┬ ┴ ╶─┬───╮ │ ┃\n" +
        "┃ ╶───┴───┤ ╶─┤ ┴ ╶─╮ ╰─┴─┤ │ ╶─╮ ╶─────┴─╯ ╶─╮ ┏━┓ ╶─┴─────┴───╮ ╶─┤ ╰─┼───╯ │ ┬ │ ├─┬─╮ ┴ ╭─┤ │ ┃\n" +
        "┃ ┬ ╭─────┼─╮ ╰─┬─┬─┤ ╭───╯ ┴ ┬ ┢━━━━━━━━━━━━━┷━┛ ┗━━━━━━━━━━━━━┷━┓ ╰───╯ ┬ ┬ ┴ ╰─┼─╯ │ ╰───┤ ╰─╯ ┃\n" +
        "┃ │ ┴ ┬ ╭─╯ │ ╶─╯ │ ╰─╯ ┏━━━━━┷━┛                                 ┗━━━━━━━┪ ╰─────╯ ┬ ┴ ╶───┤ ╶─┬─┨\n" +
        "┃ │ ┬ │ ┴ ╭─┴─╮ ┬ ┴ ┏━━━┛                                                 ┗━━━┓ ┬ ┬ │ ╶───┬─╯ ╶─┤ ┃\n" +
        "┃ ╰─┤ ╰─╮ ┴ ╶─┴─╆━━━┛                                                         ┗━┷━┪ ╰─╮ ╶─╯ ┬ ╶─┤ ┃\n" +
        "┃ ╶─┤ ╶─┤ ╶─┲━━━┛                                                                 ┗━━━┪ ╶───┤ ╶─╯ ┃\n" +
        "┃ ╶─┴─╮ │ ┏━┛                                                                         ┗━┓ ┬ │ ╭─╮ ┃\n" +
        "┃ ╶───┤ ┢━┛                                                                             ┗━┪ ╰─╯ │ ┃\n" +
        "┃ ╶───╆━┛                                                                                 ┗━┓ ┬ ╰─┨\n" +
        "┃ ╶─┲━┛                                                                                     ┗━┪ ┬ ┃\n" +
        "┃ ╶─┨                                                                                         ┃ ╰─┨\n" +
        "┃ ┏━┛                                                                                         ┗━┓ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┃ ┃                                                                                             ┃ ┃\n" +
        "┗━┛                                                                                             ┗━┛\n" +
        "                                                                                                   "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 25, column radius 15, column translation factor 14, in outside mode ellipse maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 25 15 0.0 0.0 0 14 Shape.Ellipse.Side.Outside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓  \n" +
        "┠───╴ ╶─╮ ╶───╮ ╭───╴ ┬ ┬ ┬ ┬ ╶─────╮ ┬ ┬ ┬ ┬ ┬ ┬ ┏━━━━━┛  \n" +
        "┃ ┬ ╭───╯ ┬ ╶─┴─┴─╮ ╶─┤ ╰─┤ │ ╭─╴ ╶─┴─┴─┤ │ │ ┢━┷━┛        \n" +
        "┠─╯ ╰─╮ ╭─┴───────┴───┴─╴ ╰─┤ │ ╶───╮ ┬ ╰─┴─╆━┛            \n" +
        "┠─╴ ╶─┴─┤ ╶───────┬─╴ ╭─╴ ┬ │ │ ╭─╴ ╰─┴───┲━┛              \n" +
        "┠─╴ ╶─╮ ├─╴ ┬ ┬ ╶─┴─┬─╯ ┬ ╰─┴─┼─╯ ╭─╴ ┬ ┏━┛                \n" +
        "┃ ╶─╮ ├─┴─╴ ├─╯ ╭───╯ ╭─╯ ┬ ┬ ╰───┤ ┬ ┢━┛                  \n" +
        "┠─╴ ├─╯ ╶─╮ ╰─┬─╯ ┬ ╶─┼─╴ ╰─┴─╮ ┬ ╰─┤ ┃                    \n" +
        "┃ ╶─┴───┬─┴─╴ │ ╭─╯ ┬ ╰─┬───╴ ├─╯ ╶─╆━┛                    \n" +
        "┃ ┬ ╶───┴─╮ ╭─╯ ╰───┤ ╶─┤ ┬ ┬ ├───╴ ┃                      \n" +
        "┠─╯ ╶─╮ ╶─┤ │ ┬ ╶───┴───┤ │ ╰─┴─╮ ┏━┛                      \n" +
        "┃ ╶───┴───┼─╯ ╰───┬─╴ ┬ │ ╰─╮ ╭─╯ ┃                        \n" +
        "┃ ╭─╴ ╭─╴ │ ╶─╮ ╶─┴───┤ │ ╶─┤ │ ┏━┛                        \n" +
        "┠─╯ ┬ │ ╶─┼───┴─────╴ │ ├───┴─╯ ┃                          \n" +
        "┃ ╭─╯ ╰─╮ │ ╶─────╮ ╶─┤ ├─────╴ ┃                          \n" +
        "┃ │ ╶─┬─╯ ╰─╮ ┬ ╶─┤ ┬ │ │ ┬ ┬ ╶─┨                          \n" +
        "┠─┴───╯ ╭─╴ │ │ ╭─╯ │ │ ├─╯ ╰─┲━┛                          \n" +
        "┃ ┬ ╶───┴─╮ ├─╯ │ ╭─╯ │ ├─╴ ┬ ┃                            \n" +
        "┃ │ ╶─┬───╯ ╰───┴─┤ ┬ ├─┴─╴ │ ┃                            \n" +
        "┠─╯ ╶─┤ ╶───╮ ┬ ┬ ├─╯ ├───╴ │ ┃                            \n" +
        "┃ ┬ ┬ │ ┬ ┬ │ │ │ │ ╭─╯ ┬ ╶─┤ ┃                            \n" +
        "┃ ╰─┴─┴─┴─┤ ├─╯ │ ╰─┤ ╶─┼───╯ ┃                            \n" +
        "┃ ┬ ┬ ╶───┤ │ ╭─╯ ┬ ├─╴ ╰─╮ ┬ ┃                            \n" +
        "┃ │ │ ┬ ┬ ├─┴─╯ ╭─┴─╯ ╶─╮ │ ╰─┨                            \n" +
        "┃ ╰─┤ ╰─┤ │ ╶─╮ │ ╭─╴ ┬ │ │ ┏━┛                            \n" +
        "┃ ╶─┤ ┬ │ ╰─╮ ├─╯ ╰─╮ │ │ │ ┗━┓                            \n" +
        "┃ ┬ │ │ ╰───┼─╯ ┬ ╭─┴─╯ ╰─┤ ┬ ┃                            \n" +
        "┃ ├─╯ │ ┬ ┬ │ ┬ │ │ ┬ ╶─╮ ╰─┤ ┃                            \n" +
        "┃ ╰───┴─┴─┴─┼─╯ ╰─┤ ╰───┴─╮ │ ┃                            \n" +
        "┃ ╶─╮ ┬ ┬ ╭─╯ ╶─╮ │ ┬ ┬ ╭─╯ │ ┃                            \n" +
        "┃ ┬ ╰─┤ ├─╯ ┬ ╭─╯ ├─┴─┴─╯ ╶─┤ ┃                            \n" +
        "┠─╯ ┬ │ ╰─╮ │ │ ┬ ╰─┬─╴ ┬ ┬ │ ┃                            \n" +
        "┃ ┬ ├─╯ ┬ │ ╰─┤ │ ╶─┤ ╶─┼─╯ ╰─┨                            \n" +
        "┃ ╰─┤ ╭─╯ ╰─┬─╯ ╰─┬─╯ ┬ ╰─╮ ┬ ┗━┓                          \n" +
        "┃ ┬ │ ╰─╮ ┬ │ ┬ ┬ │ ╭─╯ ╶─┴─┴─╮ ┃                          \n" +
        "┃ │ ╰─┬─╯ │ │ ╰─┴─┤ ├───╴ ╶─╮ │ ┃                          \n" +
        "┃ │ ╶─┴───┴─┤ ┬ ┬ ├─╯ ┬ ╶───┼─╯ ┃                          \n" +
        "┃ │ ┬ ╶───╮ │ ├─╯ ╰───┼───╴ │ ╶─┺━┓                        \n" +
        "┃ ╰─┤ ┬ ┬ ╰─┼─╯ ╭───╴ │ ┬ ╭─╯ ╶─╮ ┃                        \n" +
        "┃ ┬ │ │ │ ┬ │ ┬ ╰─╮ ┬ │ │ ├───╴ │ ┗━┓                      \n" +
        "┃ │ │ │ │ │ │ │ ╭─┴─┴─┴─┴─╯ ╶─╮ │ ╶─┨                      \n" +
        "┃ ├─┴─╯ ├─┴─╯ │ ╰─┬─╴ ┬ ╶─────┼─╯ ╶─┺━┓                    \n" +
        "┠─╯ ╶─╮ ╰─╮ ┬ ├─╴ ├─╴ ├─╴ ╶─┬─╯ ╶───╮ ┃                    \n" +
        "┃ ┬ ╶─┤ ┬ ├─╯ ├─╴ ╰─╮ │ ╭─╴ ├───╴ ┬ ╰─┺━┓                  \n" +
        "┠─╯ ┬ ╰─┤ │ ┬ ╰───┬─╯ ╰─┤ ┬ ╰─╮ ┬ │ ┬ ┬ ┗━┓                \n" +
        "┃ ╶─┴─╮ ╰─┤ ├───╴ ╰─────┤ ├─╴ │ │ ├─╯ │ ╶─┺━┓              \n" +
        "┃ ┬ ╶─┼───╯ │ ┬ ┬ ┬ ╭───╯ │ ╭─┴─┴─╯ ╶─┴───╮ ┗━┓            \n" +
        "┠─╯ ┬ ╰─────┴─┼─╯ ├─╯ ╶─╮ ╰─┴─╮ ┬ ┬ ┬ ┬ ╭─╯ ╶─┺━━━┓        \n" +
        "┠─╴ │ ┬ ┬ ╶─╮ ╰───┼─╴ ╭─┴─────╯ ├─┴─┴─╯ ╰───╮ ╶─╮ ┗━━━━━┓  \n" +
        "┗━━━┷━┷━┷━━━┷━━━━━┷━━━┷━━━━━━━━━┷━━━━━━━━━━━┷━━━┷━━━━━━━┛  "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a row radius 25, column radius 15, column translation factor 14, in outside mode ellipse maze generated with the sidewinder algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 25 15 0.0 0.0 0 14 Shape.Ellipse.Side.Outside)
        |> Grid.create
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze.Grid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━┯━━━━━━━━━┯━━━━━━━━━━━━━┯━┯━━━━━━━━━━━━━━━┯━━━━━━━━━━━┓  \n" +
        "┃ │ ┬ ┬ ╶───╯ ┬ ┬ ╶───┬───┤ ┴ ╶───┬───┬─────╯ ╶─╮ ┏━━━━━┛  \n" +
        "┃ ┴ ╰─┤ ┬ ┬ ╶─┤ ╰─┬─╮ │ ╭─╯ ╶─┬───╯ ╶─┤ ┬ ╭───┲━┷━┛        \n" +
        "┃ ┬ ╭─┤ │ ╰───┤ ╶─╯ ╰─┤ ┴ ╶───╯ ╶─╮ ╶─╯ ╰─╯ ┏━┛            \n" +
        "┃ ╰─┤ ├─┤ ╶─╮ ╰─┬─╮ ╶─╯ ╶─────╮ ╶─┤ ╶─╮ ┬ ┏━┛              \n" +
        "┃ ┬ │ ┴ │ ┬ ╰─┬─╯ │ ╶─╮ ┬ ╶───┼─╮ │ ╭─┴─╆━┛                \n" +
        "┃ │ │ ╶─┼─┤ ╶─╯ ╶─┴───┼─┴─╮ ╶─╯ │ ╰─┤ ┏━┛                  \n" +
        "┃ ╰─┤ ┬ │ │ ┬ ╶─╮ ┬ ╶─┤ ┬ ╰───╮ ├─┬─╯ ┃                    \n" +
        "┃ ╭─┤ │ ┴ ├─┤ ╭─┴─┴───╯ ╰─────┤ ┴ ┴ ┏━┛                    \n" +
        "┃ │ │ ├───╯ │ ┴ ┬ ╶─╮ ╶───┬───┼─╮ ╶─┨                      \n" +
        "┃ ┴ │ ┴ ╭─╮ ╰───┤ ┬ ╰─────╯ ╶─╯ ╰─┲━┛                      \n" +
        "┃ ╶─╯ ╭─╯ │ ┬ ╭─┤ ╰─╮ ┬ ╶─╮ ╭─────┨                        \n" +
        "┃ ╶─╮ ┴ ╶─┼─┴─╯ │ ╶─┴─┴───┤ │ ┬ ┏━┛                        \n" +
        "┃ ╶─┴───╮ ┴ ╭───┤ ╭───────┼─┤ ╰─┨                          \n" +
        "┃ ╶───╮ ├───┤ ╶─┴─╯ ┬ ╶─┬─┤ │ ╶─┨                          \n" +
        "┃ ┬ ┬ │ │ ╶─┤ ╶─┬───┤ ╶─╯ ┴ │ ╶─┨                          \n" +
        "┃ │ │ ╰─╯ ┬ ┴ ╭─╯ ╶─┤ ┬ ┬ ╶─╯ ┏━┛                          \n" +
        "┃ │ ╰─┬─┬─┴───┤ ╶───┤ ├─┤ ┬ ┬ ┃                            \n" +
        "┃ ╰─┬─┤ │ ╭─╮ ┴ ┬ ╶─┼─╯ │ │ ╰─┨                            \n" +
        "┃ ╶─┤ ┴ ┴ ┴ │ ┬ ╰───╯ ╶─┴─┴─┬─┨                            \n" +
        "┃ ╶─╯ ╭───┬─┼─┤ ┬ ╶─╮ ╶─────╯ ┃                            \n" +
        "┃ ╶─╮ ┴ ╶─╯ ┴ │ ├───┴─┬─────╮ ┃                            \n" +
        "┃ ╶─┤ ╭─╮ ┬ ┬ ╰─┤ ┬ ┬ ┴ ╭───┴─┨                            \n" +
        "┃ ╶─┤ │ │ ├─┴───╯ ╰─┤ ╶─╯ ┬ ┬ ┃                            \n" +
        "┃ ╶─┤ ┴ ╰─╯ ╶─┬───┬─┤ ╭───┼─╆━┛                            \n" +
        "┃ ╭─┴─┬─┬───╮ │ ╶─╯ ╰─╯ ╶─╯ ┗━┓                            \n" +
        "┃ ┴ ┬ ┴ ┴ ┬ ╰─╯ ╶─╮ ╭───╮ ╭─┬─┨                            \n" +
        "┃ ╭─┤ ╶─╮ ╰─────╮ ╰─╯ ╶─┤ │ │ ┃                            \n" +
        "┃ │ │ ╶─┤ ┬ ╶─┬─┴───╮ ┬ ╰─╯ ┴ ┃                            \n" +
        "┃ ┴ │ ┬ ╰─┤ ╶─╯ ╶───┴─┴─┬─────┨                            \n" +
        "┃ ╶─┼─┤ ┬ │ ╶───┬───────┤ ╶─╮ ┃                            \n" +
        "┃ ┬ ┴ ╰─┴─┴─┬─╮ ┴ ╶───╮ ┴ ╭─┼─┨                            \n" +
        "┃ │ ╭───────┤ │ ╶───┬─┴─╮ ┴ ┴ ┃                            \n" +
        "┃ │ ┴ ╶───╮ │ ╰─┬───╯ ┬ ╰─╮ ╶─┺━┓                          \n" +
        "┃ ╰───┬───┴─┤ ╶─╯ ╶─╮ ├───┴─╮ ╭─┨                          \n" +
        "┃ ┬ ╶─╯ ┬ ╶─┤ ╶─╮ ┬ ╰─╯ ┬ ╶─┤ ┴ ┃                          \n" +
        "┃ ╰───╮ │ ╶─╯ ╭─┴─┤ ╶─╮ ╰───┤ ┬ ┃                          \n" +
        "┃ ┬ ╭─┴─┴───┬─╯ ╭─┼───┤ ┬ ╶─┼─┴─┺━┓                        \n" +
        "┃ ╰─┤ ┬ ┬ ╶─╯ ╭─╯ ┴ ╶─┤ │ ╶─╯ ╶───┨                        \n" +
        "┃ ╶─┤ ├─┴─╮ ╶─┤ ╶───┬─┴─┤ ┬ ┬ ╭───┺━┓                      \n" +
        "┃ ╭─╯ ┴ ┬ ├───╯ ╶─┬─╯ ╭─┤ ╰─┤ ┴ ╶───┨                      \n" +
        "┃ ┴ ┬ ╶─┴─┤ ╭─────┤ ╶─╯ ╰───┴─┬─────┺━┓                    \n" +
        "┃ ┬ │ ┬ ╶─┤ ┴ ┬ ╶─╯ ╶─╮ ╶───╮ │ ╶─────┨                    \n" +
        "┃ │ │ ├───┤ ╶─┴─┬─╮ ╶─┼───╮ ╰─╯ ╶─╮ ┬ ┗━┓                  \n" +
        "┃ │ │ ┴ ┬ │ ╶───┤ ╰───╯ ╭─┴─╮ ╭─╮ ╰─┤ ┬ ┗━┓                \n" +
        "┃ ╰─┴─╮ │ ┴ ╭───╯ ╶─┬─╮ ┴ ╶─┴─┤ │ ╶─┤ │ ╶─╄━┓              \n" +
        "┃ ╶─┬─┤ │ ╶─┤ ╶───╮ ┴ ├─┬─────┤ ╰─╮ │ │ ╶─┤ ┗━┓            \n" +
        "┃ ╶─╯ ╰─┤ ╶─╯ ╶─╮ ├───╯ │ ╶─┬─╯ ┬ ╰─┤ ├───╯ ╶─┺━┯━┓        \n" +
        "┃ ┬ ╶───┴─╮ ╶───┤ ┴ ╶───╯ ╶─╯ ┬ ╰─╮ │ ┴ ╶───╮ ╶─╯ ┗━━━━━┓  \n" +
        "┗━┷━━━━━━━┷━━━━━┷━━━━━━━━━━━━━┷━━━┷━┷━━━━━━━┷━━━━━━━━━━━┛  "
        
    renderedMaze |> should equal expectedRenderedMaze