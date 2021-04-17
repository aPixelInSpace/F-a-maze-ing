// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Sidewinder.Rectangle

open System
open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a 3 by 3 maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Rectangle.create 3 3)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "┏━━━━━┓\n" +
        "┠─╴ ╶─┨\n" +
        "┃ ┬ ╶─┨\n" +
        "┗━┷━━━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 5 maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Rectangle.create 5 5)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━┓\n" +
        "┠───╴ ╶─╮ ┃\n" +
        "┃ ╶─╮ ┬ ╰─┨\n" +
        "┃ ┬ │ │ ┬ ┃\n" +
        "┃ ╰─┴─┤ │ ┃\n" +
        "┗━━━━━┷━┷━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 10 maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━┓\n" +
        "┠───╴ ╶─╮ ╶───╮ ┬ ╶─┨\n" +
        "┃ ┬ ┬ ┬ │ ╶───┴─┤ ┬ ┃\n" +
        "┃ │ ├─╯ │ ┬ ╭─╴ │ ╰─┨\n" +
        "┃ │ ╰───┴─┤ ╰─╮ ╰─╮ ┃\n" +
        "┗━┷━━━━━━━┷━━━┷━━━┷━┛"
        
    renderedMaze |> should equal expectedRenderedMaze