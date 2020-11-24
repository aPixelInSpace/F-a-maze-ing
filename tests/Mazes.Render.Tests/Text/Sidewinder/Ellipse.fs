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
        |> Sidewinder.createMaze Top Right (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Top Left (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Bottom Right (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Bottom Left (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Right Top (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Left Top (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Left Bottom (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Right Bottom (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Bottom Right (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Left Bottom (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Top Right (Random(1)) 1 1
    
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
        |> Sidewinder.createMaze Left Bottom (Random(1)) 1 1
    
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