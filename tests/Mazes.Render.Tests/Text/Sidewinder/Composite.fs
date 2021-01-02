﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Sidewinder.Composite

open System
open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Maze.Generate
open Mazes.Render

let savedCanvas =
    "Type=Canvas\n" +
    "...........*..*...............*************.........*******..............\n" +
    "...........**.*..............***************....***********..............\n" +
    "..*******...***..............***************..******......*.....*........\n" +
    ".....*........*.*.*.*........***************..***.........*.....*........\n" +
    ".....*******************.....******..*******.....*******************.....\n" +
    "..****.*.*.*******.*.*.****..******..*******..****.*.*.*******.*.*.****..\n" +
    ".***********.....***********.***************.***********.....***********.\n" +
    "**************.*******************************************.**************\n" +
    ".***********.....***********.******..*******.***********.....***********.\n" +
    "..****.*.*.*******.*.*.****..******..*******..****.*.*.*******.*.*.****..\n" +
    ".....*******************.....***************.....*******************.....\n" +
    "......**..**..*.*.*.*........***************........*.....*.**...........\n" +
    "......**..**..*.....*........***************........*.....*.**...........\n" +
    "......******..*.....********************************......*.**...........\n" +
    "......******..*...............*************...............*..............\n" +
    "end"

let canvas =
    match Convert.fromString savedCanvas with
    | Some canvas -> canvas
    | None -> failwith "The saved canvas is not correct"

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━━━━━━━━━━━━━━━━━━━━━━━┓                 ┏━━━━━━━━━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┛ ┬ ╶───╮ ╭───╴ ┬ ┬ ┬ ┬ ╶─┺━┓       ┏━━━━━━━┛ ┬ ┬ ┬ ┬ ┬ ┬ ┃                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┠─╴ ├───╴ │ ╰─────┤ ╰─┤ ╰─╮ ┬ ┃   ┏━━━┛ ╶─────┲━┷━┷━┷━┷━┷━┪ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ ┬ ├─╴ ╶─┴─╮ ╭───╯ ╶─┴───┴─┴─┨   ┠─╴ ┬ ┏━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━━━┛ ┗━┛ ┗━┛ ┗━┛ ┗━━━━━┓         ┃ ╰─┼─╴ ╶───╆━┷━┓ ╭─╴ ╭─╴ ┬ ┬ ┃   ┗━━━┷━╋━━━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┱─╴ ╶───╮ ╭─╴ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓   ┃ ╶─┼─╴ ┬ ╶─┨   ┃ ├─╴ ├─╴ │ ╰─┨   ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┓ ╭─╴ ╭─╴ ┬ ╶─┲━┓ ┏━┓ ┏━┓ ┗━━━━━┓    \n" +
        "  ┏━┛ ╭─╴ ╶─┺━┛ ┗━┛ ┗━┛ ┏━━━━━┷━┷━┓ ┗━┛ ┗━┛ ┗━┛ ╶─┬─╴ ┗━┓ ┃ ╶─┼─╴ ╰───┺━━━┛ │ ╶─┼─╴ ╰───┨ ┏━┛ ╭───╴ ┗━┛ ┗━┛ ┗━┛ ┢━━━┷━━━┷━┓ ┗━┛ ┗━┛ ┗━┛ ╭─╴ ┬ ┗━┓  \n" +
        "┏━┛ ╶─┴─╮ ╭─╴ ╶───╮ ╶─╮ ┗━━━┓ ┏━━━┛ ┬ ┬ ╶───╮ ╶─╮ ╰─╮ ┬ ┗━┛ ╶─┴─────╮ ┬ ╶───┤ ┬ ╰─────╮ ┗━┛ ╶─┼─╴ ┬ ┬ ╶─╮ ╭─╴ ┬ ┗━━━┓ ┏━━━┛ ╭─╴ ┬ ╶─╮ ╶─┴───┤ ┬ ┗━┓\n" +
        "┗━┓ ╶─╮ │ │ ┬ ╶─╮ ╰───┴─┲━━━┛ ┗━━━┱─╯ │ ╭───╯ ╶─┤ ┬ ╰─┤ ┏━┱─╴ ╶─────┤ ┢━━━┓ │ ├─────╴ │ ┏━┓ ╶─┼─╴ ╰─┤ ┬ ╰─┤ ┬ │ ┏━━━┛ ┗━━━┓ │ ╶─┼───┴─────╴ ├─╯ ┏━┛\n" +
        "  ┗━┓ ├─╯ │ ┢━┓ ┢━┓ ┏━┓ ┗━━━━━━━━━┛ ┏━┪ ┢━┓ ┏━┱─╯ │ ╶─╆━┛ ┃ ╶───╮ ╭─╯ ┃   ┃ ├─┴─╴ ┬ ╭─╯ ┃ ┗━┓ │ ╭─╴ ┢━┪ ┏━┪ ┢━┪ ┗━━━━━━━━━┛ ┢━┓ ┢━┓ ┏━┱─╴ ┬ │ ┏━┛  \n" +
        "    ┗━┷━━━┪ ┗━┛ ┗━┛ ┗━┛ ┬ ╶───╮ ┬ ┬ ┗━┛ ┗━┛ ┗━┛ ┏━┷━━━┛   ┃ ┬ ╶─┤ │ ┬ ┗━━━┛ │ ┬ ┬ │ │ ┬ ┃   ┗━┷━┷━┓ ┗━┛ ┗━┛ ┗━┛ ╶───────╮ ┬ ┗━┛ ┗━┛ ┗━┛ ┏━┷━┷━┛    \n" +
        "          ┗━┓ ┬ ┏━━━┱─╴ ┢━━━┓ ┢━┪ ┢━┓ ┏━┓ ┏━━━━━┛         ┃ ╰───┤ │ ├─╴ ┬ ╭─╯ ╰─┤ ├─╯ │ ┃         ┗━━━━━┓ ┏━━━━━━━━━┓ ┏━╅─╯ ┏━━━━━━━━━━━┛          \n" +
        "            ┠─╯ ┃   ┃ ┬ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┠───╴ ╰─┤ ├─╴ ╰─┴─╮ ╶─┤ │ ╶─┤ ┃               ┃ ┃         ┃ ┃ ┃ ╶─┨                      \n" +
        "            ┃ ┬ ┗━━━┛ │ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ┬ ┬ ╶─┤ ├─╴ ╶─╮ │ ┬ │ │ ┬ │ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┠─╴ ┃                      \n" +
        "            ┠─┴─╴ ┬ ┬ ╰─┨   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┪ │ ┬ ╰─┼─╴ ┬ │ │ │ │ │ │ ┢━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━━━┛                      \n" +
        "            ┗━━━━━┷━┷━━━┛   ┗━┛                             ┗━┷━┷━━━┷━━━┷━┷━┷━┷━┷━┷━┷━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Top, Left, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Left 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━━━━━━━━━━━━━━━━━━━━━━━┓                 ┏━━━━━━━━━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┛ ┬ ┬ ╶─────╮ ┬ ┬ ╭───╴ ┬ ┗━┓       ┏━━━━━━━┛ ╶───┬───╴ ╶─┨                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┃ ┬ │ ╰─╮ ╶─╮ ╰─┴─┴─┼───╴ ├─╴ ┃   ┏━━━┛ ┬ ╭─╴ ┏━━━━━┷━━━━━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ ╰─┴───┴───┴─────╮ ├───╴ ╰───┨   ┃ ┬ ┬ ┢━┷━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━━━┛ ┗━┛ ┗━┛ ┗━┛ ┗━━━━━┓         ┠─╴ ╶───╮ ╶─┲━━━┱─┴─╯ ╶───╮ ┬ ┃   ┗━┷━┷━╋━━━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┱───╴ ┬ ┬ ╶─╮ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓   ┠─╴ ┬ ╶─┴─╮ ┃   ┃ ┬ ╶───╮ │ │ ┃   ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┱─╴ ╶───────╮ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓    \n" +
        "  ┏━┹─────╴ ┗━┛ ┗━┛ ┗━┛ ┏━━━┷━┷━━━┪ ┗━┛ ┗━┛ ┗━┛ ╶─────┺━┓ ┃ ┬ ╰─╮ ┬ ╰─┺━━━┛ │ ╶─┬─╯ │ ╰─┨ ┏━┹─╴ ╶─╮ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━━━━━┪ ┗━┛ ┗━┛ ┗━┛ ╭───╴ ┗━┓  \n" +
        "┏━┛ ┬ ╶─┬─╴ ╭─╴ ╶─┬─╴ ┬ ┗━━━┓ ┏━━━┛ ╶─╮ ╭─╴ ╭─╴ ╶───╮ ╶─┺━┛ │ ┬ ╰─┴─╮ ┬ ╶───┴───┤ ┬ │ ╶─┺━┛ ╶─╮ ╶─┴─╮ ┬ ┬ ┬ ╶───┺━━━┓ ┏━━━┹─╴ ╶─╮ ┬ ╶───┤ ╭───╴ ┗━┓\n" +
        "┗━┱─╯ ╶─┴───┤ ┬ ┬ │ ┬ │ ┏━━━┛ ┗━━━┓ ┬ ├─╯ ╶─┤ ╶───┬─┴─╴ ┏━┓ │ │ ╶─╮ ╰─╆━━━┓ ╶─╮ │ ╰─┤ ┬ ┏━┱─╴ ╰─╮ ╭─┴─┴─┴─┴───╴ ┏━━━┛ ┗━━━┱─╴ ┬ │ │ ┬ ┬ ╰─┤ ┬ ╶─┲━┛\n" +
        "  ┗━┓ ┬ ╭─╴ ┢━┪ ┢━┪ ┢━┪ ┗━━━━━━━━━┛ ┢━┪ ┏━┓ ┢━┓ ┬ │ ┬ ┏━┛ ┃ │ ├─╴ │ ┬ ┃   ┃ ┬ │ ╰───┴─┤ ┃ ┗━┱─╴ │ │ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━━━━━┛ ┏━┪ ┢━┪ ┢━┪ ╭─╯ │ ┏━┛  \n" +
        "    ┗━┷━┷━┓ ┗━┛ ┗━┛ ┗━┛ ┬ ┬ ╶───────┺━┛ ┗━┛ ┗━┛ ┢━┷━┷━┛   ┃ │ │ ┬ │ │ ┗━━━┛ │ │ ┬ ┬ ╶─┤ ┃   ┗━━━┷━┪ ┗━┛ ┗━┛ ┗━┛ ┬ ┬ ┬ ╶───╮ ┗━┛ ┗━┛ ┗━┛ ┢━━━┷━┛    \n" +
        "          ┗━┱─╴ ┏━━━┓ ┬ ┢━┷━┓ ┏━┓ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ │ ├─╯ ╰─┤ ╭─╴ ┬ │ │ ╰─┴─╮ │ ┃         ┗━━━━━┓ ┏━━━━━┷━┷━┪ ┏━┓ │ ┏━━━━━━━━━━━┛          \n" +
        "            ┃ ┬ ┃   ┃ ╰─┨   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┃ │ ╰─╮ ┬ ╰─┼─╴ ╰─┴─┤ ╭───╯ ╰─┨               ┃ ┃         ┃ ┃ ┃ │ ┃                      \n" +
        "            ┃ │ ┗━━━┛ ┬ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ │ ┬ │ │ ╭─╯ ╶─╮ ┬ ╰─┤ ┬ ┬ ╶─┺━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┃ │ ┃                      \n" +
        "            ┃ │ ┬ ┬ ┬ │ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┪ │ ├─╯ │ ╶─╮ ├─┴───╯ ╰─┤ ┏━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━┷━┛                      \n" +
        "            ┗━┷━┷━┷━┷━┷━┛   ┗━┛                             ┗━┷━┷━━━┷━━━┷━┷━━━━━━━━━┷━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Bottom, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Bottom Sidewinder.Direction.Right 1 1 1
    
    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━┯━━━━━┯━┯━━━━━┯━┯━┯━┯━┓                 ┏━━━━━┯━┯━┯━┯━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━╃─╴ │ ╶─┬─╯ ╰─┬─╴ ┴ ┴ │ ┴ ┡━┓       ┏━┯━┯━┯━┛ ╶───╯ ┴ ┴ ┴ ┃                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┃ ├─╮ ╰─╴ ╰─┬─╮ ┴ ╶─────╯ ╶─╯ ┃   ┏━━━┩ ┴ ┴ ┴ ┏━━━━━━━━━━━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ ┴ ╰─╴ ╶───╯ ╰───╴ ╶─────┬───┨   ┠─╴ ┴ ┏━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━┯━━━┛ ┗━┛ ┗━┛ ┗━┛ ┗━━━━━┓         ┠─╮ ╭─────┬─┲━━━┓ ╭─────┬─┴─╮ ┃   ┗━━━━━╋━━━━━━━━━━━━━┯━━━┛ ┡━━━┯━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━┯━━━┛ ┏━┓ ┏━┓ ┏━┓ ┴ ╶─────────┲━┓ ┏━┓ ┏━┓ ┗━━━┯━┓   ┃ │ ┴ ╭───┤ ┃   ┃ ┴ ╶─┬─┼─╴ │ ┃   ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┓ ╰───╴ ╰─╴ ┴ ┏━┓ ┏━┓ ┏━┓ ┗━┯━━━┓    \n" +
        "  ┏━┛ ╰───╮ ┗━┛ ┡━┩ ┗━┛ ┏━━━━━━━━━┓ ┗━┩ ┗━┛ ┗━╃─╴ ╭─┤ ┡━┓ ┃ ╰─╴ ┴ ╭─╯ ┡━━━╃─╮ ╭─╯ ┴ ╶─╯ ┃ ┏━┹─┬───╴ ┡━┩ ┗━┛ ┗━┩ ┏━━━━━━━━━┓ ┗━┛ ┡━┛ ┡━╃─╴ ├─╮ ┗━┓  \n" +
        "┏━┹─┬───╮ ╰─┬─╴ │ ╰─╴ ╶─┺━━━┓ ┏━━━┛ ╭─┴─╮ ╭─┬─┤ ╭─╯ │ ┴ ┗━┛ ╶───┬─┤ ╶─╯ ╶─╯ │ ├───┬─┬─╴ ┗━┹─╮ ┴ ╭─┬─╯ ┴ ╶─┬───╯ ┗━━━┓ ┏━━━┹─╴ ╶─┼─╮ ┴ │ ╭─┤ ├─╮ ┗━┓\n" +
        "┗━┓ ╰─╴ ╰─╴ ┴ ╶─╯ ╶─────┲━━━┛ ┗━━━┓ ┴ ╶─╯ ┴ ┴ ┴ │ ╭─┤ ╶─┲━┱─┬─╴ ┴ ┴ ╭─┲━━━┓ ┴ ┴ ╭─┤ ┴ ╭─┲━┓ ┴ ╶─╯ ├─────╴ ┴ ╶───┲━━━┛ ┗━━━┱─╴ ╶─╯ ┴ ╶─╯ │ │ ┴ ┴ ┏━┛\n" +
        "  ┗━┓ ╶─────┲━┓ ┏━┓ ┏━┓ ┗━┯━━━━━┯━┩ ┏━┓ ┏━┓ ┏━┓ ┴ ┴ ┴ ┏━┛ ┃ │ ╶───┬─╯ ┃   ┠───╮ │ ├─╮ │ ┃ ┗━┱─╴ ╶─╯ ┏━┓ ┏━┓ ┏━┓ ┡━┯━━━┯━━━┩ ┏━┓ ┏━┓ ┏━┓ ┴ ┴ ╶─┲━┛  \n" +
        "    ┗━━━━━┓ ┗━┩ ┗━┛ ┗━┹─╴ ┴ ╶───╯ ┴ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┛   ┃ ├─┬─╮ ╰─╮ ┡━┯━┹─╮ │ ┴ ┴ │ │ ┃   ┗━━━━━┓ ┗━┛ ┗━┛ ┗━┛ ┴ ╰─╴ ┴ ╶─┤ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┛    \n" +
        "          ┗━┓ │ ┏━━━┱─╴ ┏━━━┓ ┏━┓ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ ┴ ┴ │ ╭─╯ ┴ ╰─╮ ┴ ├─╮ ╶─┤ │ ┃         ┗━━━━━┓ ┏━━━━━━━━━┓ ┏━┓ │ ┏━━━━━━━━━━━┛          \n" +
        "            ┃ │ ┃   ┠─╮ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┠───╮ ┴ ├─╮ ╶───┤ ╭─┤ │ ╭─┤ │ ┃               ┃ ┃         ┃ ┃ ┃ ┴ ┃                      \n" +
        "            ┃ │ ┗━┯━┩ ┴ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ╶─┤ ╭─╯ ├─╴ ╭─┤ │ │ │ │ │ ┴ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┃ ╶─┨                      \n" +
        "            ┃ ╰─╴ ┴ ┴ ╶─┨   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┓ ┴ ┴ ╶─┴─╴ ┴ ┴ ┴ ┴ ┴ ┴ ┴ ┏━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━━━┛                      \n" +
        "            ┗━━━━━━━━━━━┛   ┗━┛                             ┗━━━━━━━━━━━━━━━━━━━━━━━━━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Bottom, Left, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Bottom Sidewinder.Direction.Left 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━┯━━━━━━━┯━┯━┯━┯━━━━━┯━━━┓                 ┏━━━━━┯━━━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┛ │ ╶─┬───╯ ┴ │ ╰───╮ ┴ ╭─┺━┓       ┏━┯━━━┯━┹───╴ ╰───╴ ╶─┨                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┠─╴ ┴ ╶─╯ ╶─┬─┬─┴───╴ ├─╮ ├─╴ ┃   ┏━━━┛ ╰─╴ ┴ ┏━━━━━━━━━━━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┠───╴ ╭─────╯ ╰─╴ ╶───┤ │ ┴ ╭─┨   ┃ ╶───┲━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━┯━┛ ┗━┩ ┡━┛ ┗━┛ ┗━━━━━┓         ┃ ╶─┬─╯ ╶─┬─┲━━━┱─┬─╴ ┴ │ ╭─╯ ┃   ┗━━━━━╋━━━━━━━━━━━━━━━━━┛ ┗━┯━┯━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━┯━┛ ┏━┓ ┏━┓ ┏━┱─╴ ╰───╴ ┴ ┴ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓   ┠─╮ ┴ ╭─┬─╯ ┃   ┃ ┴ ╭───┤ ╰─╮ ┃   ┏━┯━━━┩ ┏━┓ ┏━┓ ┏━┱───────╴ ┴ ┴ ┏━┓ ┏━┓ ┏━┓ ┗━┯━┯━┓    \n" +
        "  ┏━┹─╮ ├─╴ ┡━┩ ┗━┩ ┗━┛ ┏━━━━━━━━━┓ ┡━┛ ┗━┛ ┡━╃─────╮ ┗━┓ ┃ ┴ ╶─╯ ┴ ╭─╄━┯━┛ ╭─╯ ╭─┴─╴ ┴ ┃ ┏━┩ │ ╭─╯ ┡━┛ ┗━┛ ┡━┛ ┏━━━━━━━━━┓ ┗━┛ ┡━┩ ┗━┛ ╭─╯ ┴ ┗━┓  \n" +
        "┏━┹─╮ │ │ ╭─╯ ├─╮ ╰─╴ ╶─┺━━━┓ ┏━━━┛ ┴ ╭─────╯ │ ╭───┼─╴ ┗━┹─┬─┬───╴ │ ┴ ┴ ╶─╯ ╶─┤ ╭─────┺━┛ ┴ │ ┴ ╭─┼─╮ ╭─┬─╯ ╭─┺━━━┓ ┏━━━┹─╴ ╶─╯ ├───╮ ╰─┬───╮ ┗━┓\n" +
        "┗━┓ ┴ │ │ │ ╶─╯ ╰─╴ ╶───┲━━━┛ ┗━━━┱─╴ ╰─────╴ ┴ ┴ ╶─┤ ╶─┲━┓ ┴ ├─╮ ╶─╯ ┏━━━┱─╮ ╶─╯ ╰───╮ ┏━┱─╴ │ ╭─┤ ┴ ┴ ┴ ┴ ╶─╯ ┏━━━┛ ┗━━━┓ ╶─────╯ ╶─┴─╴ ╰─╮ ┴ ┏━┛\n" +
        "  ┗━┓ ┴ ┴ ┴ ┏━┓ ┏━┓ ┏━┓ ┗━┯━━━┯━┯━┩ ┏━┓ ┏━┓ ┏━┱─╴ ╶─╯ ┏━┛ ┠─╮ │ ├─┬─╮ ┃   ┃ │ ╶───┬───╯ ┃ ┗━┓ ┴ ┴ ┴ ┏━┓ ┏━┓ ┏━┓ ┡━━━━━┯━━━┛ ┏━┓ ┏━┓ ┏━┱───╴ ┴ ┏━┛  \n" +
        "    ┗━━━━━┓ ┗━┩ ┗━┛ ┗━╃─╴ ╰─╴ ┴ ┴ ┴ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┛   ┃ │ │ │ ┴ │ ┡━┯━┛ ╰─┬─╴ ├───╮ ┃   ┗━━━━━┓ ┗━┛ ┗━┛ ┗━┛ ┴ ╶───┴─╴ ╶─┺━┛ ┗━┛ ┗━┛ ┏━━━━━┛    \n" +
        "          ┗━┓ │ ┏━━━┓ │ ┏━━━┓ ┏━┓ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ │ ┴ │ ╶─╯ ┴ ├─┬─╴ ┴ ╶─┤ ╶─┤ ┃         ┗━━━━━┓ ┏━━━━━━━━━┓ ┏━┱─╴ ┏━━━━━━━━━━━┛          \n" +
        "            ┃ │ ┃   ┃ │ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┃ │ ╭─┼─╴ ╶─┬─┤ ╰─┬─╮ ╭─┼─╮ │ ┃               ┃ ┃         ┃ ┃ ┠─╴ ┃                      \n" +
        "            ┃ │ ┡━┯━┩ │ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ┴ ┴ ├─╮ ╶─╯ ┴ ╭─┤ ┴ │ ┴ ┴ ┴ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┠─╴ ┃                      \n" +
        "            ┃ ┴ ┴ ┴ ┴ ┴ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┓ ╶─╯ ╰───╴ ╶─╯ ╰─╴ ┴ ╶───┲━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━━━┛                      \n" +
        "            ┗━━━━━━━━━━━┛   ┗━┛                             ┗━━━━━━━━━━━━━━━━━━━━━━━━━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Right, Top, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Right Sidewinder.Direction.Top 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━┯━━━━━━━━━┯━━━┯━┯━━━┯━┓                 ┏━━━━━┯━┯━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┹─╴ ╰─┬───╴ ┬ ╰─╮ ┴ │ ┬ │ ┗━┓       ┏━┯━┯━━━┛ ┬ ┬ ┴ ┴ ╭─╴ ┃                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┠─┬─╴ ┬ ┴ ╭─╴ │ ┬ ╰─╮ ╰─╯ ╰─╴ ┃   ┏━━━┛ ┴ ╰─╴ ┏━┷━┷━━━━━┷━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ │ ┬ │ ╭─┼───╯ │ ┬ │ ╭───┬─╴ ┃   ┃ ╭─╴ ┏━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━┯━┛ ┡━┛ ┡━┛ ┗━┛ ┗━━━━━┓         ┃ ╰─┼─╯ │ ┴ ┏━━━╅─╯ │ ├─╴ ┴ ┬ ┃   ┗━┷━━━╋━━━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┓ ┬ ╰─╴ ┴ ┬ ┴ ┏━┓ ┏━┓ ┏━┓ ┡━━━┯━┓   ┠─╴ ╰───┴─╮ ┃   ┃ ┬ ├─┼─┬───┤ ┃   ┏━┯━━━┛ ┏━┓ ┏━┓ ┏━┱─────╴ ╭───╴ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓    \n" +
        "  ┏━╃─────╴ ┗━┛ ┡━┛ ┡━┩ ┢━━━━━━━┷━┓ ┗━┩ ┗━┩ ┗━┩ ╰─╮ │ ┗━┓ ┃ ┬ ╭─╴ ┬ ┴ ┗━┯━┩ │ │ │ ╰─╮ │ ┃ ┏━┛ ╰───╴ ┗━┛ ┗━┩ ┗━┛ ┏━━━━━┷━━━┓ ┗━┩ ┗━┛ ┗━┹─────╮ ┗━┓  \n" +
        "┏━┛ ├─────────╴ ┴ ┬ │ ┴ ┗━━━┓ ┏━━━┛ ┬ ╰─╮ ├─╮ ├─╮ ┴ │ ┬ ┗━┹─┤ │ ┬ │ ┬ ┬ ┴ ╰─┤ ┴ ╰─╴ │ ┴ ┗━┹───╮ ┬ ┬ ╭───╴ ╰───╴ ┗━━━┓ ┏━━━┹─╮ ├─────╴ ┬ ╭─╮ ╰─╴ ┗━┓\n" +
        "┗━┓ ┴ ╭─────╴ ╭───╯ ╰─╴ ┏━━━┛ ┗━━━┓ ├─╴ ┴ ┴ ┴ ┴ ╰─╴ ├─╯ ┏━┓ ╰─┴─╯ ├─┤ ┢━━━┓ ├─────╮ ╰─╮ ┏━┓ ┬ ┴ ├─┴─┴───────╴ ┬ ┏━━━┛ ┗━━━┓ ┴ ╰───╴ ╭─╯ │ ╰───╴ ┏━┛\n" +
        "  ┗━┱─┴─╴ ┬ ┏━┪ ┏━┓ ┏━┓ ┗━━━━━━━━━┛ ┢━┓ ┏━┓ ┏━┱─╴ ┬ ┴ ┏━┛ ┠───╴ ╭─╯ ┴ ┃   ┃ ├───╴ ╰─╴ ┴ ┃ ┗━╅─╴ │ ┬ ┏━┓ ┏━┓ ┏━┪ ┗━┯━━━┯━━━┛ ┏━┓ ┏━┓ ┢━┱─╯ ╭─╴ ┏━┛  \n" +
        "    ┗━━━━━┪ ┗━┛ ┗━┛ ┗━┹─╴ ╭───╴ ╭─╴ ┗━┛ ┗━┛ ┗━┛ ┏━┷━━━┛   ┠─╮ ╭─┴───╴ ┗━━━┩ ╰───┬─────╴ ┃   ┗━━━┷━┪ ┗━┛ ┗━┛ ┗━┹─╴ ╰─╴ ╰───╴ ┗━┛ ┗━┛ ┗━┛ ┏━┷━━━┛    \n" +
        "          ┗━┓ ┬ ┏━━━┱─╮ ┏━┷━┓ ┏━┪ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ │ │ ┬ ╭───╴ ┬ ╰───╴ ╰─┬─┬─╴ ┃         ┗━━━━━┓ ┏━━━━━━━━━┓ ┏━┓ ┬ ┏━━━━━━━━━━━┛          \n" +
        "            ┠─┤ ┃   ┃ │ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┃ ├─╯ │ │ ╭─╴ ├───────╮ ┴ ╰─╴ ┃               ┃ ┃         ┃ ┃ ┃ │ ┃                      \n" +
        "            ┃ │ ┗━┯━┛ │ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ╰─╮ ├─┴─╯ ┬ ├─╮ ╭─╴ ╰─╴ ┬ ┬ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┃ │ ┃                      \n" +
        "            ┃ ┴ ┬ ╰─╴ ┴ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┓ ╰─┴───╴ ├─╯ ╰─┴───╴ ┬ │ ┢━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━┷━┛                      \n" +
        "            ┗━━━┷━━━━━━━┛   ┗━┛                             ┗━━━━━━━━━┷━━━━━━━━━━━┷━┷━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Right, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Right Sidewinder.Direction.Bottom 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━━━┯━━━━━━━━━┯━━━━━━━┯━┓                 ┏━┯━━━━━┯━┯━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━╃─╴ ┬ ╰─╴ ╭───╴ ╰───╮ ┬ ┴ ┗━┓       ┏━┯━━━━━┛ ┴ ┬ ┬ ┴ ┴ ┬ ┃                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┃ ├─╴ ├───╴ │ ╭─╮ ╭─╴ ╰─┤ ┬ ┬ ┃   ┏━━━┛ ╰───╴ ┏━━━┷━┷━━━━━┪ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ ┴ ┬ │ ┬ ╭─╯ │ ╰─┴───╮ ┴ ├─╯ ┃   ┠─╴ ┬ ┏━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━━━┩ ┗━┛ ┗━┛ ┗━┛ ┗━━━━━┓         ┠─╴ ├─╯ ├─╯ ┏━┷━┱─┬─╴ ╰───┴─╴ ┃   ┗━━━┷━╋━━━━━━━━━━━━━━━┯━┛ ┗━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┱─╴ ┬ ╰─╴ ┬ ┬ ┏━┓ ┏━┓ ┏━┓ ┗━━━┯━┓   ┠─┬─╯ ╭─┴─╮ ┃   ┃ ├─────┬───╮ ┃   ┏━┯━━━┛ ┏━┓ ┏━┓ ┏━┱─╴ ╰───────╴ ┏━┓ ┏━┓ ┏━┓ ┗━┯━━━┓    \n" +
        "  ┏━┩ ╭─╴ ┬ ┗━┩ ┗━┩ ┡━┛ ┏━┷━━━━━┷━┪ ┗━┩ ┗━┛ ┗━┹─╴ ┬ │ ┗━┓ ┃ ╰─┬─┴─╴ ┴ ┗━━━┩ ├───╴ ╰─╮ ┴ ┃ ┏━┛ │ ┬ ┬ ┗━┛ ┗━┩ ┗━┛ ┏━━━━━━━━━┓ ┡━┛ ┗━┛ ┗━┹─╴ ╰─╴ ┗━┓  \n" +
        "┏━┛ │ ├───┴─╮ ├─╴ ┴ ┴ ┬ ┗━━━┓ ┏━━━┹─╴ ╰─┬───┬─────┤ ├─╴ ┗━┹─╴ ┴ ┬ ╭─╮ ┬ ┬ ┴ ╰─┬─┬─╴ ├─╮ ┗━┹─╮ ┴ ├─╯ ╭───╴ ╰─╴ ┬ ┗━━━┓ ┏━━━┛ ├─┬───────╴ ┬ ╭─┬─╴ ┗━┓\n" +
        "┗━┓ ╰─┴───╴ ┴ ╰─────╴ │ ┏━━━┛ ┗━━━┓ ╭─╴ ╰─╴ ┴ ╭─╴ ┴ │ ┬ ┏━┓ ┬ ╭─╯ │ ┴ ┢━┷━┱─╴ │ ├─╮ ┴ │ ┏━┓ ╰─╮ │ ┬ │ ╭─╴ ┬ ╭─╯ ┏━━━┛ ┗━━━┓ ┴ ╰─────╴ ┬ ├─╯ ╰─╴ ┏━┛\n" +
        "  ┗━┱─────╴ ┏━┓ ┏━┓ ┏━┪ ┗━┯━━━━━━━┩ ┢━┓ ┏━┓ ┏━┪ ╭─╴ ┴ ┢━┛ ┠─┤ ├─╴ │ ┬ ┃   ┃ ┬ │ ┴ ╰─╴ ┴ ┃ ┗━┓ ╰─┴─╯ ┢━┪ ┏━┪ ┢━┓ ┗━━━━━━━━━┛ ┏━┓ ┏━┓ ┏━╅─┴───╴ ┏━┛  \n" +
        "    ┗━━━━━┓ ┗━┛ ┗━┛ ┗━┛ ┬ ╰───╴ ┬ ┴ ┗━┛ ┗━┛ ┗━┛ ┢━━━━━┛   ┃ ├─┴─┬─┼─┤ ┗━━━┩ │ ├─┬───┬─╴ ┃   ┗━━━━━┓ ┗━┛ ┗━┛ ┗━┹─────╴ ╭─╴ ┬ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┛    \n" +
        "          ┗━┱─╴ ┏━━━┱─╴ ┢━━━┓ ┏━┪ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ ┴ ┬ │ ┴ ╰───╴ ├─┤ │ ├─╴ ┴ ┬ ┃         ┗━━━━━┓ ┏━━━━━━━━━┓ ┢━┱─┤ ┏━━━━━━━━━━━┛          \n" +
        "            ┃ ┬ ┃   ┠─╮ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┠───╯ │ ╭─────╮ │ ┴ ┴ ╰───┬─╯ ┃               ┃ ┃         ┃ ┃ ┃ ┴ ┃                      \n" +
        "            ┃ │ ┗━━━┛ │ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ╭─╴ │ │ ╭─╴ ┴ │ ╭─╮ ╭─╴ ├─╴ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┠─╴ ┃                      \n" +
        "            ┠─╯ ┬ ┬ ┬ ┴ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━╅─╴ ╰─┴─┴─╴ ┬ ╰─╯ ┴ │ ┬ ┴ ┏━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━━━┛                      \n" +
        "            ┗━━━┷━┷━┷━━━┛   ┗━┛                             ┗━━━━━━━━━━━┷━━━━━━━┷━┷━━━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Left, Top, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Top 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━┯━━━━━━━━━┯━━━┯━┯━━━┯━┓                 ┏━━━━━━━┯━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┛ ╶─┤ ╭───────┤ ╭─╯ │ ┬ │ ┗━┓       ┏━━━━━━━┛ ╶─╮ ╶─╯ ┬ ╶─┨                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┃ ╭───┤ ┴ ┬ ╶─┬─┤ ┴ ╭─╯ ╰─╯ ╶─┨   ┏━━━┛ ╶─────┲━━━┷━━━━━┷━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ │ ╶─┤ ┬ ├─╮ ┴ ┴ ┬ │ ┬ ╶─┬───┨   ┃ ╶───┲━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━━━━━┩ ┗━┩ ┗━┛ ┗━┛ ┗━━━━━┓         ┃ ┴ ┬ ┴ │ ┴ ┢━━━┓ ╰─┤ │ ╶─╯ ┬ ┃   ┗━━━━━╋━━━━━━━━━━━━━━━━━┛ ┡━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━┯━━━┛ ┏━┓ ┏━┓ ┏━┓ ┬ ┬ ┴ ╶─╯ ╶─┲━┓ ┏━┓ ┏━┓ ┗━┯━━━┓   ┃ ╶─┼───┴─┬─┨   ┃ ┬ │ ├─┬───┼─┨   ┏━━━━━┩ ┏━┓ ┏━┓ ┏━┓ ╶─────╯ ╶─╮ ┏━┓ ┏━┓ ┏━┓ ┗━━━━━┓    \n" +
        "  ┏━┛ │ ┬ ╶─┺━┛ ┗━┩ ┗━┩ ┢━┷━━━━━━━┓ ┗━┛ ┗━┩ ┡━┩ ┬ ┴ ┬ ┗━┓ ┃ ┬ ┴ ╶─╮ ┴ ┗━━━┛ │ │ │ ┴ ╭─┤ ┃ ┏━┛ ╭───╯ ┗━┩ ┗━┛ ┗━┛ ┏━━━━━━━━━┪ ┗━┛ ┗━┛ ┡━┩ ╶─────┺━┓  \n" +
        "┏━┛ ╭─╯ ╰─────╮ ┬ │ ┬ ┴ ┗━━━┓ ┏━━━┛ ┬ ┬ ╶─┤ │ ┴ ├───┤ ╶─┺━┛ ├─╮ ╶─┤ ┬ ╶─╮ ┬ ├─╯ ┴ ╶─┤ ┴ ┗━┛ ┬ ┴ ┬ ╶───╯ ╶─┬─╮ ┬ ┗━━━┓ ┏━━━┛ ╶─────╮ ┴ ┴ ╶───┬───┺━┓\n" +
        "┗━┓ ┴ ╶───────┴─┴─╯ │ ╶─┲━━━┛ ┗━━━┓ ╰─┴───╯ ┴ ╶─╯ ┬ ╰─╮ ┏━┓ ┴ │ ┬ │ ├─┲━┷━┪ │ ╶───┬─╯ ╭─┲━┓ ╰───┤ ┬ ┬ ┬ ╶─╯ │ ╰─┲━━━┛ ┗━━━┓ ╶─────┴─╮ ┬ ┬ ╶─╯ ╶─┲━┛\n" +
        "  ┗━┓ ╶─────┲━┓ ┏━┓ ┢━┓ ┗━┯━┯━━━━━┩ ┏━┓ ┏━┓ ┏━┓ ╶─┤ ┬ ┢━┛ ┃ ╶─┴─┴─┴─╯ ┃   ┃ │ ╶───╯ ╶─╯ ┃ ┗━┓ ┬ │ ╰─╆━┪ ┏━┓ ┢━┓ ┡━┯━━━━━━━┩ ┏━┓ ┏━┓ ┢━┪ │ ┬ ╶─┲━┛  \n" +
        "    ┗━━━━━┓ ┗━┩ ┗━┛ ┗━┩ ╶─╯ ┴ ╶───╯ ┗━┛ ┗━┛ ┗━┛ ┏━┷━┷━┛   ┃ ╭───╮ ╶───┺━━━┛ ┴ ╶─┬───────┨   ┗━┷━┷━┓ ┗━┛ ┗━┛ ┗━┛ ┴ ┴ ╶─────╯ ┗━┛ ┗━┛ ┗━┛ ┢━┷━━━┛    \n" +
        "          ┗━┓ ┴ ┏━━━┓ │ ┏━━━┓ ┏━┓ ┏━┓ ┏━┓ ┏━━━━━┛         ┃ │ ┬ ╰─╮ ╶───╮ ╶─────╯ ╭─┬───┨         ┗━━━━━┓ ┏━━━━━━━━━┓ ┏━┓ ╶─┲━━━━━━━━━━━┛          \n" +
        "            ┃ ╭─┨   ┃ │ ┃   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┃ ┴ │ ┬ │ ┬ ╶─┴─┬─┬───┬─╯ ┴ ╶─┨               ┃ ┃         ┃ ┃ ┃ ┬ ┃                      \n" +
        "            ┃ │ ┡━━━┩ │ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ╶─┴─┤ ├─┴─╮ ┬ ┴ ┴ ╶─╯ ╶─╮ ┬ ┗━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┃ │ ┃                      \n" +
        "            ┃ ┴ ┴ ╶─╯ ┴ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┓ ╶─┤ ┴ ╶─┤ │ ┬ ┬ ╶───╮ │ ┢━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━┷━┛                      \n" +
        "            ┗━━━━━━━━━━━┛   ┗━┛                             ┗━━━┷━━━━━┷━┷━┷━┷━━━━━┷━┷━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a customized composite canvas generated with the sidewinder algorithm (Left, Bottom, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        canvas
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Left Sidewinder.Direction.Bottom 1 1 1

    // act
    let renderedMaze =  maze.Grid.ToSpecializedGrid |> Text.renderGrid
        
    // assert
    let expectedRenderedMaze =
        "                      ┏━┓   ┏━┓                             ┏━━━━━━━━━━━━━━━┯━━━━━━━━━┓                 ┏━━━━━┯━━━━━━━┓                            \n" +
        "                      ┃ ┗━┓ ┃ ┃                           ┏━┩ ╶─╮ ┬ ╶─╮ ╶───┤ ╶───┬───╄━┓       ┏━━━━━┯━┛ ╶─╮ ┴ ╶─╮ ╶─┨                            \n" +
        "    ┏━━━━━━━━━━━━━┓   ┗━┓ ┗━┛ ┃                           ┃ ┴ ┬ ╰─┴─┬─┼─╮ ╶─┤ ╶───┤ ╶─┤ ┃   ┏━┯━┛ ┬ ╶─╯ ┏━━━┷━━━━━┷━┓ ┃         ┏━┓                \n" +
        "    ┗━━━━━┓ ┏━━━━━┛     ┗━━━┓ ┃ ┏━┓ ┏━┓ ┏━┓               ┃ ╶─┤ ┬ ╶─╯ ┴ │ ┬ │ ╶───╯ ╭─╯ ┃   ┃ ┴ ╶─╆━━━━━┛           ┃ ┃         ┃ ┃                \n" +
        "          ┃ ┗━━━━━━━━━━━┯━┯━┛ ┗━┛ ┗━┛ ┗━┛ ┗━━━━━┓         ┃ ╭─┴─┴─╮ ╭─┲━┷━┪ ┴ ┬ ╭───┤ ╭─┨   ┗━━━━━╋━━━━━━━━━━━━━━━━━┛ ┗━━━━━━━━━┛ ┗━━━━━┓          \n" +
        "    ┏━━━━━┛ ┏━┓ ┏━┓ ┏━┓ ┴ ┴ ╶─╮ ┬ ╶─┲━┓ ┏━┓ ┏━┓ ┗━┯━┯━┓   ┃ ┴ ╭─╮ │ ┴ ┃   ┃ ╶─┼─┤ ╭─╯ │ ┃   ┏━┯━━━┩ ┏━┓ ┏━┓ ┏━┓ ╶───────╮ ╶─┲━┓ ┏━┓ ┏━┓ ┗━━━━━┓    \n" +
        "  ┏━┩ ┬ ╶─╮ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┷━┷━┓ ┗━┩ ┗━┩ ┗━┛ ╭─┤ ┴ ┗━┓ ┃ ╶─╯ ╰─┤ ┬ ┡━━━┛ ┬ │ ┴ ┴ ╶─╯ ┃ ┏━┛ ┴ ╶─╯ ┡━┩ ┗━┛ ┗━┩ ┏━━━━━━━┷━┓ ┗━┛ ┗━┛ ┗━┛ ╶─────┺━┓  \n" +
        "┏━┛ ┴ ├───┴─────╮ ╶─╮ ╶─┺━━━┓ ┏━━━┛ ╭─╯ ╭─╯ ╭─╮ ┴ ┴ ┬ ┬ ┗━┛ ┬ ┬ ╶─┼─┼─╯ ┬ ╶─┤ │ ╶───┬───┺━┛ ╶─╮ ╶───╯ │ ╶───╮ │ ┗━━━┓ ┏━━━┛ ╭───╮ ┬ ┬ ┬ ┬ ╭─────┺━┓\n" +
        "┗━┓ ╶─┤ ╶─────╮ ╰───┤ ╶─┲━━━┛ ┗━━━┓ ┴ ┬ ┴ ╶─╯ ╰─╮ ╭─┤ ╰─┲━┓ ├─┤ ╶─╯ ┴ ┏━┷━┓ ├─┤ ┬ ╶─╯ ┬ ┏━┓ ╶─┤ ┬ ╶───╯ ╶───┴─╯ ┏━━━┛ ┗━━━┓ ┴ ╶─┴─┤ │ │ ╰─╯ ╶───┲━┛\n" +
        "  ┗━┓ ┴ ┬ ╶─┲━┪ ┏━┓ ┢━┓ ┗━┯━━━━━━━┛ ┏━┪ ┏━┓ ┏━┓ ╰─╯ ╰─┲━┛ ┃ │ ╰─╮ ┬ ╶─┨   ┃ ┴ │ │ ╶───┴─┨ ┗━┓ │ ╰───┲━┓ ┏━┓ ┏━┓ ┗━━━┯━━━━━┛ ┏━┓ ┏━┪ ┢━┪ ╶─────┲━┛  \n" +
        "    ┗━━━┷━┓ ┗━┩ ┗━┛ ┗━┛ ┬ ┴ ╶─╮ ┬ ┬ ┗━┛ ┗━┛ ┗━┛ ┏━━━━━┛   ┃ ┴ ┬ │ │ ┬ ┗━━━┛ ┬ ┴ │ ╶─╮ ╶─┨   ┗━┷━━━┓ ┗━┛ ┗━┛ ┗━┛ ╶─╮ ┴ ╶─╮ ╶─┺━┛ ┗━┛ ┗━┛ ┏━━━━━┛    \n" +
        "          ┗━┓ ┴ ┏━━━┓ ╶─╆━━━┓ ┢━┪ ┢━┓ ┏━┓ ┏━━━━━┛         ┃ ╭─┴─┤ ├─┼─╮ ╶─╮ ╰─┬─┤ ┬ │ ╶─┨         ┗━━━━━┓ ┏━━━━━━━┷━┓ ┏━┪ ╶─┲━━━━━━━━━━━┛          \n" +
        "            ┃ ┬ ┃   ┃ ╶─┨   ┃ ┃ ┗━┛ ┗━┛ ┃ ┃               ┃ │ ╶─┤ ┴ │ ╰─┬─┴───╯ ╰─┤ ╰───┨               ┃ ┃         ┃ ┃ ┃ ┬ ┃                      \n" +
        "            ┃ ├─┺━━━┛ ┬ ┃   ┃ ┃         ┃ ┗━━━━━━━━━━━━━━━┛ ┴ ╶─┴───╯ ╶─╯ ┬ ┬ ╶─┬─┤ ╶───┺━━━━━━━━━━━━━━━╋━┛         ┃ ┃ ┃ ╰─┨                      \n" +
        "            ┃ ┴ ┬ ╶─╮ │ ┃   ┃ ┃         ┗━━━━━━━━━━━━━━━━━━━┓ ╶─────╮ ╶─╮ ╰─┴───╯ │ ╶─┲━━━━━━━━━━━━━━━━━┛           ┃ ┃ ┗━━━┛                      \n" +
        "            ┗━━━┷━━━┷━┷━┛   ┗━┛                             ┗━━━━━━━┷━━━┷━━━━━━━━━┷━━━┛                             ┗━┛                            "
        
    renderedMaze |> should equal expectedRenderedMaze