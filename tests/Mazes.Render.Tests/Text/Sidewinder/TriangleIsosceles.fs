﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Sidewinder.TriangleIsosceles

open System
open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a base 11, base at bottom, triangle with a base decrement value of 1 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 11 Shape.TriangleIsosceles.BaseAt.Bottom 1 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "          ┏━┓          \n" +
        "        ┏━┛ ┗━┓        \n" +
        "      ┏━┹─╴ ┬ ┗━┓      \n" +
        "    ┏━┛ ┬ ╭─┴─╴ ┗━┓    \n" +
        "  ┏━┛ ┬ │ │ ╶─────┺━┓  \n" +
        "┏━┛ ┬ │ │ │ ┬ ┬ ╭─╴ ┗━┓\n" +
        "┗━━━┷━┷━┷━┷━┷━┷━┷━━━━━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 11, base at top, triangle with a base decrement value of 1 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 11 Shape.TriangleIsosceles.BaseAt.Top 1 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━━━┓\n" +
        "┗━┱───╴ ╶─╮ ╶───╮ ┬ ┏━┛\n" +
        "  ┗━┱─╴ ┬ │ ┬ ┬ ╰─╆━┛  \n" +
        "    ┗━┓ ╰─┤ │ │ ┏━┛    \n" +
        "      ┗━┓ ├─╯ ┢━┛      \n" +
        "        ┗━┪ ┏━┛        \n" +
        "          ┗━┛          "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 11, base at left, triangle with a base decrement value of 1 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 11 Shape.TriangleIsosceles.BaseAt.Left 1 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━┓          \n" +
        "┃ ┗━┓        \n" +
        "┠─╴ ┗━┓      \n" +
        "┃ ╶───┺━┓    \n" +
        "┃ ╭───╴ ┗━┓  \n" +
        "┃ │ ┬ ┬ ╶─┺━┓\n" +
        "┃ │ │ │ ┬ ┏━┛\n" +
        "┠─╯ │ │ ┢━┛  \n" +
        "┃ ┬ │ ┢━┛    \n" +
        "┠─╯ ┢━┛      \n" +
        "┃ ┏━┛        \n" +
        "┗━┛          "

        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 11, base at right, triangle with a base decrement value of 1 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 11 Shape.TriangleIsosceles.BaseAt.Right 1 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "          ┏━┓\n" +
        "        ┏━┛ ┃\n" +
        "      ┏━┛ ╶─┨\n" +
        "    ┏━┛ ┬ ╶─┨\n" +
        "  ┏━┛ ┬ │ ╶─┨\n" +
        "┏━┛ ┬ │ │ ┬ ┃\n" +
        "┗━┓ ╰─┴─┤ │ ┃\n" +
        "  ┗━┓ ┬ ├─╯ ┃\n" +
        "    ┗━┪ ├─╴ ┃\n" +
        "      ┗━┪ ┬ ┃\n" +
        "        ┗━┪ ┃\n" +
        "          ┗━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 21, base at bottom, triangle with a base decrement value of 3 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 21 Shape.TriangleIsosceles.BaseAt.Bottom 3 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                  ┏━━━━━┓                  \n" +
        "            ┏━━━━━┹─╴ ╶─┺━━━━━┓            \n" +
        "      ┏━━━━━┛ ┬ ╶─╮ ╭───╴ ┬ ┬ ┗━━━━━┓      \n" +
        "┏━━━━━┛ ┬ ╶───┴─╮ │ │ ┬ ┬ │ │ ╭─╴ ┬ ┗━━━━━┓\n" +
        "┗━━━━━━━┷━━━━━━━┷━┷━┷━┷━┷━┷━┷━┷━━━┷━━━━━━━┛"

        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 21, base at top, triangle with a base decrement value of 3 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 21 Shape.TriangleIsosceles.BaseAt.Top 3 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓\n" +
        "┗━━━━━┱───╴ ╶─╮ ╶───╮ ╭───╴ ┬ ┬ ┬ ┬ ┏━━━━━┛\n" +
        "      ┗━━━━━┓ ╰───╮ │ │ ┬ ┬ │ ┢━┷━┷━┛      \n" +
        "            ┗━━━━━╅─╯ │ ┢━┷━┷━┛            \n" +
        "                  ┗━━━┷━┛                  "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 21, base at left, triangle with a base decrement value of 3 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 21 Shape.TriangleIsosceles.BaseAt.Left 3 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "┏━┓      \n" +
        "┃ ┃      \n" +
        "┃ ┃      \n" +
        "┃ ┗━┓    \n" +
        "┃ ┬ ┃    \n" +
        "┃ │ ┃    \n" +
        "┃ ╰─┺━┓  \n" +
        "┃ ┬ ┬ ┃  \n" +
        "┃ │ │ ┃  \n" +
        "┃ ╰─┤ ┗━┓\n" +
        "┃ ┬ │ ┬ ┃\n" +
        "┃ ├─╯ │ ┃\n" +
        "┃ │ ╶─╆━┛\n" +
        "┃ │ ╶─┨  \n" +
        "┃ ╰─╮ ┃  \n" +
        "┃ ┬ ┢━┛  \n" +
        "┃ │ ┃    \n" +
        "┠─╯ ┃    \n" +
        "┃ ┏━┛    \n" +
        "┃ ┃      \n" +
        "┃ ┃      \n" +
        "┗━┛      "

        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 21, base at right, triangle with a base decrement value of 3 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 21 Shape.TriangleIsosceles.BaseAt.Right 3 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "      ┏━┓\n" +
        "      ┃ ┃\n" +
        "      ┃ ┃\n" +
        "    ┏━┛ ┃\n" +
        "    ┃ ┬ ┃\n" +
        "    ┠─╯ ┃\n" +
        "  ┏━┹─╴ ┃\n" +
        "  ┃ ╶───┨\n" +
        "  ┃ ┬ ┬ ┃\n" +
        "┏━┛ │ │ ┃\n" +
        "┃ ╶─┤ │ ┃\n" +
        "┃ ┬ ├─╯ ┃\n" +
        "┗━┪ ├─╴ ┃\n" +
        "  ┃ │ ┬ ┃\n" +
        "  ┠─╯ │ ┃\n" +
        "  ┗━┓ ╰─┨\n" +
        "    ┃ ┬ ┃\n" +
        "    ┃ │ ┃\n" +
        "    ┗━┪ ┃\n" +
        "      ┃ ┃\n" +
        "      ┃ ┃\n" +
        "      ┗━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 51, base at bottom, triangle with a base decrement value of 1 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 51 Shape.TriangleIsosceles.BaseAt.Bottom 1 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =        
        "                                                  ┏━┓                                                  \n" +
        "                                                ┏━┛ ┗━┓                                                \n" +
        "                                              ┏━┹─╴ ┬ ┗━┓                                              \n" +
        "                                            ┏━┛ ┬ ╭─┴─╴ ┗━┓                                            \n" +
        "                                          ┏━┛ ┬ │ │ ╶─────┺━┓                                          \n" +
        "                                        ┏━┛ ┬ │ │ │ ┬ ┬ ╭─╴ ┗━┓                                        \n" +
        "                                      ┏━┹───╯ │ ╰─┴─┴─┤ ╰─╮ ╶─┺━┓                                      \n" +
        "                                    ┏━┛ ┬ ╭─╴ ╰─────╮ │ ╭─╯ ╶───┺━┓                                    \n" +
        "                                  ┏━┛ ╭─┴─┴─────────┴─╯ ╰─╮ ┬ ╶───┺━┓                                  \n" +
        "                                ┏━┛ ╭─┴─╴ ╶───────╮ ╶─────┴─┼─╴ ╭─╴ ┗━┓                                \n" +
        "                              ┏━┛ ┬ │ ╭─────╴ ╶───┴───╮ ╭─╴ │ ┬ ╰───╮ ┗━┓                              \n" +
        "                            ┏━┛ ┬ ╰─┼─╯ ╭─╴ ┬ ╶───╮ ╭─┴─╯ ╭─╯ ├───╴ ├─╴ ┗━┓                            \n" +
        "                          ┏━┛ ┬ ╰───┤ ┬ │ ╭─╯ ╶─╮ ╰─┼─╴ ┬ ╰─┬─╯ ╶───┤ ┬ ╶─┺━┓                          \n" +
        "                        ┏━┛ ╶─┴─────┼─┴─╯ │ ╭─╴ │ ╶─┼───╯ ╭─┴─╴ ┬ ┬ ╰─┴─╮ ┬ ┗━┓                        \n" +
        "                      ┏━┛ ╶─╮ ╶─╮ ┬ │ ┬ ╶─┴─┤ ╶─┤ ╶─┤ ┬ ┬ ╰─────┴─┤ ┬ ╶─┴─┤ ┬ ┗━┓                      \n" +
        "                    ┏━┛ ╶─┬─╯ ╶─┴─┼─╯ │ ┬ ╶─┤ ╭─╯ ╭─╯ ╰─┼─╴ ┬ ╶─╮ ╰─┴───╮ │ ╰─╮ ┗━┓                    \n" +
        "                  ┏━┛ ┬ ┬ │ ╶─┬───┴───┴─╯ ┬ ├─┴─╴ ╰─╮ ┬ ╰─╮ │ ╶─┴───╮ ╶─┤ ├───┴─╴ ┗━┓                  \n" +
        "                ┏━┛ ╶─┴─┼─╯ ╶─┤ ┬ ╶─╮ ┬ ┬ │ │ ╭─────┴─┴─╴ ├─╯ ┬ ┬ ╭─╯ ┬ │ ├───╴ ┬ ╶─┺━┓                \n" +
        "              ┏━┛ ╶─╮ ╭─╯ ┬ ╭─╯ │ ╭─╯ │ │ │ ├─┴─╴ ╶─────╮ │ ╭─┴─╯ │ ╭─╯ │ ╰───╮ │ ┬ ┬ ┗━┓              \n" +
        "            ┏━┛ ╭─╴ │ ╰─╮ │ │ ┬ │ │ ┬ │ │ ├─╯ ┬ ╶─╮ ╶───┴─┴─┴─╮ ╭─╯ │ ╶─┤ ╶─┬─┴─╯ │ │ ┬ ┗━┓            \n" +
        "          ┏━┛ ╶─┤ ╭─╯ ┬ ├─╯ ╰─┤ ├─╯ │ │ │ │ ╭─┴─╴ ├───╴ ╶─╮ ╭─╯ ╰───┤ ╶─┤ ┬ ╰─╮ ┬ ├─╯ │ ┬ ┗━┓          \n" +
        "        ┏━┛ ┬ ┬ │ │ ╶─┤ ├─╴ ╶─┤ │ ┬ │ │ │ │ │ ╶───┼─╴ ┬ ╭─┴─╯ ╶─╮ ┬ ╰─┬─╯ │ ┬ │ │ │ ┬ │ │ ╶─┺━┓        \n" +
        "      ┏━┛ ╶─┤ │ ╰─┴───┴─┼─╴ ╶─┤ ╰─┴─┴─┤ ├─╯ ╰─╮ ┬ │ ╭─╯ ╰─╮ ┬ ┬ │ ├─╴ │ ┬ │ │ ├─╯ │ ├─╯ │ ╶───┺━┓      \n" +
        "    ┏━┛ ╭───╯ │ ┬ ╶─╮ ┬ │ ┬ ╶─┼─╴ ┬ ┬ ├─╯ ┬ ╭─╯ │ │ ╰─╮ ┬ ╰─┤ ╰─┼─╯ ╶─┴─┴─┤ ├─╯ ╶─┼─╯ ╶─┼─╴ ┬ ╶─┺━┓    \n" +
        "  ┏━┛ ╭─╯ ┬ ╶─┤ ╰─╮ │ │ │ │ ┬ ├─╴ ╰─┴─┴─╮ │ │ ╭─╯ │ ┬ ╰─┴─╮ ├───╯ ╶─╮ ┬ ╶─┤ ╰─────┴─╮ ┬ │ ╭─╯ ┬ ╶─┺━┓  \n" +
        "┏━┹───╯ ┬ │ ┬ ╰─╮ │ ├─╯ ╰─┴─┼─┴─╴ ┬ ╶───┤ │ │ │ ╶─┼─╯ ╭───╯ │ ┬ ╭─╴ ╰─┤ ┬ │ ┬ ┬ ╶─╮ │ │ ╰─┤ ┬ │ ┬ ┬ ┗━┓\n" +
        "┗━━━━━━━┷━┷━┷━━━┷━┷━┷━━━━━━━┷━━━━━┷━━━━━┷━┷━┷━┷━━━┷━━━┷━━━━━┷━┷━┷━━━━━┷━┷━┷━┷━┷━━━┷━┷━┷━━━┷━┷━┷━┷━┷━━━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 51, base at bottom, triangle with a base decrement value of 5 and an height increment value of 1, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 51 Shape.TriangleIsosceles.BaseAt.Bottom 5 1)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =        
        "                                                  ┏━┓                                                  \n" +
        "                                        ┏━━━━━━━━━┛ ┗━━━━━━━━━┓                                        \n" +
        "                              ┏━━━━━━━━━┹─╴ ┬ ╶───╮ ╭───╴ ┬ ┬ ┗━━━━━━━━━┓                              \n" +
        "                    ┏━━━━━━━━━┛ ┬ ╶─────╮ ┬ │ ┬ ┬ │ │ ╭─╴ ├─┴─╴ ┬ ╶─────┺━━━━━━━━━┓                    \n" +
        "          ┏━━━━━━━━━┛ ╶─╮ ╶─╮ ┬ ├─╴ ╶───┴─┤ │ ├─╯ ╰─┴─┤ ╭─┴─────┴───────╴ ╶─╮ ┬ ╶─┺━━━━━━━━━┓          \n" +
        "┏━━━━━━━━━┛ ╶─┬───╴ ╶───┴───┤ ╰─┴─────┬─╴ ├─╯ │ ┬ ┬ ╭─┴─┴─╴ ╶───────╮ ╭─╴ ┬ │ ╰───┬─╴ ┬ ╶───┺━━━━━━━━━┓\n" +
        "┗━━━━━━━━━━━━━┷━━━━━━━━━━━━━┷━━━━━━━━━┷━━━┷━━━┷━┷━┷━┷━━━━━━━━━━━━━━━┷━┷━━━┷━┷━━━━━┷━━━┷━━━━━━━━━━━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 10, base at bottom, triangle with a base decrement value of 1 and an height increment value of 3, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 10 Shape.TriangleIsosceles.BaseAt.Bottom 1 3)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "        ┏━━━┓        \n" +
        "        ┃ ╶─┨        \n" +
        "        ┠─╴ ┃        \n" +
        "      ┏━┛ ┬ ┗━┓      \n" +
        "      ┃ ╶─┤ ╶─┨      \n" +
        "      ┃ ┬ │ ┬ ┃      \n" +
        "    ┏━┛ │ ╰─┴─┺━┓    \n" +
        "    ┃ ┬ │ ┬ ┬ ┬ ┃    \n" +
        "    ┠─╯ ├─╯ │ ╰─┨    \n" +
        "  ┏━┛ ┬ ╰───┴─╮ ┗━┓  \n" +
        "  ┃ ╶─┴─╮ ┬ ┬ ╰───┨  \n" +
        "  ┃ ╶─╮ │ ├─┴───╴ ┃  \n" +
        "┏━┹─╴ ├─╯ ╰───────┺━┓\n" +
        "┠───╴ │ ┬ ╶───╮ ┬ ╶─┨\n" +
        "┠─╴ ╶─┴─┴─┬───┴─╯ ╶─┨\n" +
        "┗━━━━━━━━━┷━━━━━━━━━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 10, base at top, triangle with a base decrement value of 1 and an height increment value of 3, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 10 Shape.TriangleIsosceles.BaseAt.Top 1 3)
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
        "┗━┪ │ ├─╯ ┬ ┬ ╭─╯ ┢━┛\n" +
        "  ┃ ├─╯ ┬ ╰─┴─┴─╮ ┃  \n" +
        "  ┃ │ ╶─┤ ┬ ┬ ╶─┴─┨  \n" +
        "  ┗━┪ ╶─┤ │ │ ╶─┲━┛  \n" +
        "    ┃ ╶─┤ │ ╰───┨    \n" +
        "    ┃ ╶─┴─┴─────┨    \n" +
        "    ┗━┓ ┬ ┬ ╶─┲━┛    \n" +
        "      ┃ │ │ ╶─┨      \n" +
        "      ┠─╯ ╰───┨      \n" +
        "      ┗━┓ ┬ ┏━┛      \n" +
        "        ┃ ╰─┨        \n" +
        "        ┠─╴ ┃        \n" +
        "        ┗━━━┛        "

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 10, base at left, triangle with a base decrement value of 1 and an height increment value of 3, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 10 Shape.TriangleIsosceles.BaseAt.Left 1 3)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━┓                        \n" +
        "┠─╴ ╶─┺━━━━━┓                  \n" +
        "┃ ┬ ╶─╮ ┬ ╶─┺━━━━━┓            \n" +
        "┃ ├─╴ │ │ ╶─────╮ ┗━━━━━┓      \n" +
        "┃ │ ┬ │ │ ┬ ╭─╴ ├───╴ ┬ ┗━━━━━┓\n" +
        "┃ ╰─┤ ╰─┤ ╰─┤ ┬ ├─╴ ╶─┴───╮ ┬ ┃\n" +
        "┠───╯ ╶─┴─╮ ├─┴─╯ ╶─────┲━┷━┷━┛\n" +
        "┠─────╴ ┬ │ ╰───╮ ┏━━━━━┛      \n" +
        "┠───╴ ╶─┴─┴─┲━━━┷━┛            \n" +
        "┃ ╶─╮ ┏━━━━━┛                  \n" +
        "┗━━━┷━┛                        "

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 10, base at right, triangle with a base decrement value of 1 and an height increment value of 3, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 10 Shape.TriangleIsosceles.BaseAt.Right 1 3)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "                        ┏━━━━━┓\n" +
        "                  ┏━━━━━┹─╴ ╶─┨\n" +
        "            ┏━━━━━┛ ┬ ╶───╮ ┬ ┃\n" +
        "      ┏━━━━━┹─╴ ┬ ┬ │ ╭───┴─╯ ┃\n" +
        "┏━━━━━┛ ╶─╮ ┬ ┬ │ │ │ ├─╴ ┬ ╶─┨\n" +
        "┃ ┬ ╶─────┤ ╰─┤ ╰─┤ │ ├─╴ ╰───┨\n" +
        "┗━┷━━━┓ ┬ │ ╭─╯ ╶─┴─┤ │ ╶─────┨\n" +
        "      ┗━┷━┷━╅───────┴─╯ ┬ ┬ ╶─┨\n" +
        "            ┗━━━━━┓ ┬ ╭─╯ ╰───┨\n" +
        "                  ┗━┷━┷━┱─╴ ╶─┨\n" +
        "                        ┗━━━━━┛"

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a base 30, base at bottom, triangle with a base decrement value of 2 and an height increment value of 5, maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Shape.TriangleIsosceles.create 30 Shape.TriangleIsosceles.BaseAt.Bottom 2 5)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  snd maze.NDStruct.FirstSlice2D |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "                            ┏━━━┓                            \n" +
        "                            ┃ ╶─┨                            \n" +
        "                            ┠─╴ ┃                            \n" +
        "                            ┃ ┬ ┃                            \n" +
        "                            ┃ │ ┃                            \n" +
        "                        ┏━━━┛ ╰─┺━━━┓                        \n" +
        "                        ┃ ┬ ┬ ┬ ┬ ╶─┨                        \n" +
        "                        ┃ ╰─┤ │ │ ┬ ┃                        \n" +
        "                        ┠─╴ │ │ ├─╯ ┃                        \n" +
        "                        ┃ ╭─╯ ├─╯ ╶─┨                        \n" +
        "                    ┏━━━┛ │ ╶─┤ ╶─╮ ┗━━━┓                    \n" +
        "                    ┃ ╭─╴ ╰───┴─╮ │ ┬ ╶─┨                    \n" +
        "                    ┃ ╰─╮ ╭───╴ ╰─┴─┴───┨                    \n" +
        "                    ┠───╯ │ ┬ ╶───╮ ┬ ╶─┨                    \n" +
        "                    ┠─╴ ╶─┴─┴─┬───┴─╯ ╶─┨                    \n" +
        "                ┏━━━┹───╴ ╭─╴ │ ┬ ┬ ┬ ╶─┺━━━┓                \n" +
        "                ┠───╴ ╶───┤ ╭─╯ │ │ ╰───┬─╴ ┃                \n" +
        "                ┃ ┬ ╶─┬─╴ ├─╯ ┬ ╰─┴─╮ ╭─┴─╴ ┃                \n" +
        "                ┃ │ ╭─┴─╴ ├─╴ │ ┬ ╶─┴─┤ ┬ ┬ ┃                \n" +
        "                ┃ │ ╰─╮ ╶─┼─╴ │ ╰─┬─╴ ╰─┴─┤ ┃                \n" +
        "            ┏━━━┛ ╰───┤ ╶─┴───┴─┬─┴─╴ ┬ ╭─╯ ┗━━━┓            \n" +
        "            ┃ ╶─┬───╴ ├───╴ ┬ ┬ ╰───╮ ├─╯ ╶───╮ ┃            \n" +
        "            ┃ ┬ │ ┬ ┬ ╰───╮ ╰─┤ ╶─╮ │ ├───────╯ ┃            \n" +
        "            ┠─╯ │ ╰─┴─╮ ┬ ╰───┴─┬─╯ ╰─┴─┬─╴ ┬ ┬ ┃            \n" +
        "            ┃ ┬ ├─╴ ╭─╯ ╰─┬─╴ ┬ ╰─╮ ╶───┴─╮ ├─╯ ┃            \n" +
        "        ┏━━━┛ ╰─┤ ┬ │ ┬ ╶─┼───┴───┴─╴ ┬ ╭─┴─╯ ╶─┺━━━┓        \n" +
        "        ┃ ┬ ╶─╮ │ ╰─┴─┴─╮ ╰─╮ ╭─────╴ │ ╰───┬─╴ ╶─╮ ┃        \n" +
        "        ┃ │ ┬ │ │ ┬ ┬ ╭─┴───┴─┴─╴ ╭─╴ │ ┬ ╭─╯ ┬ ┬ │ ┃        \n" +
        "        ┠─╯ │ ╰─┴─┴─┴─┤ ╭─╴ ┬ ╭─╴ │ ╭─╯ │ │ ┬ │ ╰─┴─┨        \n" +
        "        ┠─╴ ╰─╮ ┬ ╭───╯ │ ╭─╯ │ ╶─┴─┤ ┬ │ ├─╯ ├───╴ ┃        \n" +
        "    ┏━━━┛ ┬ ┬ │ │ │ ┬ ┬ │ │ ┬ ├─╴ ┬ ╰─┤ ╰─┴───┴───╮ ┗━━━┓    \n" +
        "    ┠─╴ ┬ ╰─┤ ╰─┼─┴─╯ │ │ │ ╰─┤ ┬ ├─╴ │ ╭─╴ ╶─╮ ╭─╯ ┬ ┬ ┃    \n" +
        "    ┃ ╶─┤ ╭─┴─╴ ├───╴ ╰─┤ ├─╴ ╰─┴─┤ ╶─┤ │ ╶─╮ │ ├─╴ │ │ ┃    \n" +
        "    ┃ ╶─┤ │ ┬ ┬ ╰─╮ ╭─╴ ╰─┤ ┬ ┬ ┬ │ ┬ │ │ ╶─┴─┼─╯ ┬ │ ╰─┨    \n" +
        "    ┠─╴ │ │ ╰─┼─╴ │ │ ┬ ┬ │ │ │ │ ╰─┤ ╰─┤ ┬ ╶─┴───┴─┤ ╶─┨    \n" +
        "┏━━━┛ ┬ ╰─┴───┤ ╭─╯ ╰─┤ │ │ ├─╯ ╰─╮ │ ┬ │ ├─╴ ┬ ┬ ┬ │ ┬ ┗━━━┓\n" +
        "┃ ┬ ┬ ├─────╴ ╰─┼───╴ │ │ ╰─┤ ┬ ┬ │ ╰─┼─╯ │ ┬ ├─╯ │ ├─╯ ┬ ┬ ┃\n" +
        "┃ │ │ ╰─╮ ╶─┬─╴ ╰─────┤ ├─╴ ╰─┼─╯ ╰─┬─╯ ┬ ╰─┤ ├─╴ │ ╰─╮ ╰─┤ ┃\n" +
        "┃ ╰─┤ ┬ │ ┬ ├─╴ ╶─────┤ │ ┬ ╭─╯ ┬ ┬ ╰───┤ ╭─┴─╯ ╶─┤ ┬ ╰─╮ ╰─┨\n" +
        "┃ ╶─┴─┤ │ │ ├─╴ ┬ ╶───┼─╯ │ │ ┬ ╰─┤ ┬ ╭─╯ ╰───┬───╯ │ ╶─┴─╮ ┃\n" +
        "┗━━━━━┷━┷━┷━┷━━━┷━━━━━┷━━━┷━┷━┷━━━┷━┷━┷━━━━━━━┷━━━━━┷━━━━━┷━┛"

    renderedMaze |> should equal expectedRenderedMaze