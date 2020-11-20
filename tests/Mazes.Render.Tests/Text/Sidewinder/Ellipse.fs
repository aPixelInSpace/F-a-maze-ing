module Mazes.Render.Tests.Text.Sidewinder.Ellipse

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Position
open Mazes.Core.Algo.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a row radius 3, column radius 5, in inside mode ellipse maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Ellipse.create 3 5 0.0 0.0 0 0 Shape.Ellipse.Side.Inside)
        |> Sidewinder.transformIntoMaze Top Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Top Left (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Bottom Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Bottom Left (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Right Top (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Left Top (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Left Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Right Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Bottom Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Left Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Top Right (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
        |> Sidewinder.transformIntoMaze Left Bottom (Random(1)) 1 1
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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