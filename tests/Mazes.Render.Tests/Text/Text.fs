// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Text

open System
open FsUnit
open Xunit
open Mazes.Core.Grid.Ortho.Canvas
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a grid, when rendering the grid in text, then the result should like the expected output`` () =
    // arrange
    let grid =
        (Shape.Rectangle.create 7 7)
        |> OrthoGrid.createGridFunction

    let gridWithInternalBorders =
        grid()

    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 2; CIndex = 2 } { RIndex = 2; CIndex = 3 }
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 2; CIndex = 2 } { RIndex = 1; CIndex = 2 }
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 2; CIndex = 3 } { RIndex = 1; CIndex = 3 }
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 1; CIndex = 2 } { RIndex = 1; CIndex = 3 }
    
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 5; CIndex = 2 } { RIndex = 5; CIndex = 3 }
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 5; CIndex = 2 } { RIndex = 4; CIndex = 2 }
    gridWithInternalBorders.PutBorderBetweenCells { RIndex = 5; CIndex = 3 } { RIndex = 4; CIndex = 3 }

    let maze =
        (fun () -> gridWithInternalBorders)
        |> RecursiveBacktracker.createMaze 1

    // act
    let renderedMaze = maze.Grid.ToSpecializedGrid |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━┯━━━━━┓\n" +
        "┃ ┬ ┬ ┳ │ ╶─╮ ┃\n" +
        "┃ │ ┖━╋━┵───┤ ┃\n" +
        "┃ ├─╴ ╿ ╭─╮ ┴ ┃\n" +
        "┠─╯ ┬ │ ┴ ╰───┨\n" +
        "┃ ╭─┶━╈━╾───╮ ┃\n" +
        "┃ ┴ ┬ ┻ ╶─╮ ┴ ┃\n" +
        "┗━━━┷━━━━━┷━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze