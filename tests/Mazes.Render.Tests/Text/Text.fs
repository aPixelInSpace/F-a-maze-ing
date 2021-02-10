// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.Text.Text

open System
open FsUnit
open Mazes.Core
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a grid, when rendering the grid in text, then the result should like the expected output`` () =
    // arrange
    let grid =
        (Shape.Rectangle.create 15 25)
        |> Mazes.Core.GridNew.Types.Ortho.Grid.createBaseGrid
        |> Mazes.Core.GridNew.Grid.create

    let closePersistent = grid.UpdateConnection ConnectionType.ClosePersistent

    closePersistent { RIndex = 2; CIndex = 2 } { RIndex = 2; CIndex = 3 }
    closePersistent { RIndex = 2; CIndex = 2 } { RIndex = 1; CIndex = 2 }
    closePersistent { RIndex = 2; CIndex = 3 } { RIndex = 1; CIndex = 3 }
    closePersistent { RIndex = 1; CIndex = 2 } { RIndex = 1; CIndex = 3 }
    
    closePersistent { RIndex = 4; CIndex = 2 } { RIndex = 4; CIndex = 3 }
    closePersistent { RIndex = 4; CIndex = 2 } { RIndex = 3; CIndex = 2 }
    closePersistent { RIndex = 4; CIndex = 3 } { RIndex = 3; CIndex = 3 }
    
    closePersistent { RIndex = 7; CIndex = 2 } { RIndex = 7; CIndex = 3 }
    closePersistent { RIndex = 7; CIndex = 2 } { RIndex = 6; CIndex = 2 }
    closePersistent { RIndex = 6; CIndex = 2 } { RIndex = 6; CIndex = 3 }
    
    closePersistent { RIndex = 10; CIndex = 2 } { RIndex = 10; CIndex = 3 }
    closePersistent { RIndex = 10; CIndex = 3 } { RIndex = 9; CIndex = 3 }
    closePersistent { RIndex = 9; CIndex = 2 } { RIndex = 9; CIndex = 3 }

    closePersistent { RIndex = 13; CIndex = 2 } { RIndex = 12; CIndex = 2 }
    closePersistent { RIndex = 13; CIndex = 3 } { RIndex = 12; CIndex = 3 }
    closePersistent { RIndex = 12; CIndex = 2 } { RIndex = 12; CIndex = 3 }
    
    closePersistent { RIndex = 2; CIndex = 6 } { RIndex = 2; CIndex = 7 }
    closePersistent { RIndex = 2; CIndex = 6 } { RIndex = 1; CIndex = 6 }
    closePersistent { RIndex = 2; CIndex = 7 } { RIndex = 1; CIndex = 7 }
    closePersistent { RIndex = 1; CIndex = 6 } { RIndex = 1; CIndex = 7 }
    
    closePersistent { RIndex = 4; CIndex = 6 } { RIndex = 4; CIndex = 7 }
    closePersistent { RIndex = 4; CIndex = 6 } { RIndex = 3; CIndex = 6 }
    closePersistent { RIndex = 4; CIndex = 7 } { RIndex = 3; CIndex = 7 }
    
    closePersistent { RIndex = 7; CIndex = 6 } { RIndex = 7; CIndex = 7 }
    closePersistent { RIndex = 7; CIndex = 6 } { RIndex = 6; CIndex = 6 }
    closePersistent { RIndex = 6; CIndex = 6 } { RIndex = 6; CIndex = 7 }
    
    closePersistent { RIndex = 10; CIndex = 6 } { RIndex = 10; CIndex = 7 }
    closePersistent { RIndex = 10; CIndex = 7 } { RIndex = 9; CIndex = 7 }
    closePersistent { RIndex = 9; CIndex = 6 } { RIndex = 9; CIndex = 7 }

    closePersistent { RIndex = 13; CIndex = 6 } { RIndex = 12; CIndex = 6 }
    closePersistent { RIndex = 13; CIndex = 7 } { RIndex = 12; CIndex = 7 }
    closePersistent { RIndex = 12; CIndex = 6 } { RIndex = 12; CIndex = 7 }
    
    closePersistent { RIndex = 2; CIndex = 10 } { RIndex = 2; CIndex = 11 }
    closePersistent { RIndex = 2; CIndex = 10 } { RIndex = 1; CIndex = 10 }
    closePersistent { RIndex = 2; CIndex = 11 } { RIndex = 1; CIndex = 11 }
    closePersistent { RIndex = 1; CIndex = 10 } { RIndex = 1; CIndex = 11 }
    
    closePersistent { RIndex = 4; CIndex = 10 } { RIndex = 4; CIndex = 11 }
    closePersistent { RIndex = 4; CIndex = 10 } { RIndex = 3; CIndex = 10 }
    closePersistent { RIndex = 4; CIndex = 11 } { RIndex = 3; CIndex = 11 }
    
    closePersistent { RIndex = 7; CIndex = 10 } { RIndex = 7; CIndex = 11 }
    closePersistent { RIndex = 7; CIndex = 10 } { RIndex = 6; CIndex = 10 }
    closePersistent { RIndex = 6; CIndex = 10 } { RIndex = 6; CIndex = 11 }
    
    closePersistent { RIndex = 10; CIndex = 10 } { RIndex = 10; CIndex = 11 }
    closePersistent { RIndex = 10; CIndex = 11 } { RIndex = 9; CIndex = 11 }
    closePersistent { RIndex = 9; CIndex = 10 } { RIndex = 9; CIndex = 11 }

    closePersistent { RIndex = 13; CIndex = 10 } { RIndex = 12; CIndex = 10 }
    closePersistent { RIndex = 13; CIndex = 11 } { RIndex = 12; CIndex = 11 }
    closePersistent { RIndex = 12; CIndex = 10 } { RIndex = 12; CIndex = 11 }
    
    closePersistent { RIndex = 4; CIndex = 13 } { RIndex = 4; CIndex = 14 }
    closePersistent { RIndex = 4; CIndex = 13 } { RIndex = 3; CIndex = 13 }
    
    closePersistent { RIndex = 6; CIndex = 13 } { RIndex = 6; CIndex = 14 }
    closePersistent { RIndex = 7; CIndex = 13 } { RIndex = 6; CIndex = 13 }
    
    closePersistent { RIndex = 10; CIndex = 13 } { RIndex = 10; CIndex = 14 }
    closePersistent { RIndex = 10; CIndex = 14 } { RIndex = 9; CIndex = 14 }

    closePersistent { RIndex = 12; CIndex = 13 } { RIndex = 12; CIndex = 14 }
    closePersistent { RIndex = 13; CIndex = 14 } { RIndex = 12; CIndex = 14 }
    
    closePersistent { RIndex = 4; CIndex = 15 } { RIndex = 4; CIndex = 16 }
    closePersistent { RIndex = 4; CIndex = 15 } { RIndex = 3; CIndex = 15 }
    
    closePersistent { RIndex = 6; CIndex = 15 } { RIndex = 6; CIndex = 16 }
    closePersistent { RIndex = 7; CIndex = 15 } { RIndex = 6; CIndex = 15 }
    
    closePersistent { RIndex = 10; CIndex = 15 } { RIndex = 10; CIndex = 16 }
    closePersistent { RIndex = 10; CIndex = 16 } { RIndex = 9; CIndex = 16 }

    closePersistent { RIndex = 12; CIndex = 15 } { RIndex = 12; CIndex = 16 }
    closePersistent { RIndex = 13; CIndex = 16 } { RIndex = 12; CIndex = 16 }
    
    closePersistent { RIndex = 4; CIndex = 17 } { RIndex = 4; CIndex = 18 }
    closePersistent { RIndex = 5; CIndex = 17 } { RIndex = 6; CIndex = 17 }
    
    closePersistent { RIndex = 7; CIndex = 17 } { RIndex = 7; CIndex = 18 }
    closePersistent { RIndex = 8; CIndex = 17 } { RIndex = 9; CIndex = 17 }
    
    closePersistent { RIndex = 10; CIndex = 17 } { RIndex = 10; CIndex = 18 }
    closePersistent { RIndex = 11; CIndex = 17 } { RIndex = 12; CIndex = 17 }

    let maze = grid |> HuntAndKill.createMazeNew 1

    // act
    let renderedMaze = maze.Grid.ToSpecializedGrid |> Text.renderGrid

    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━┯━━━┯━━━┯━━━┯━━━━━┯━┯━━━━━━━━━┯━━━━━┯━━━━━┓\n" +
        "┠─╴ ┬ ┳ ┴ ┬ ┴ ┳ ╰─╮ ┴ ┎─╴ ┴ ┴ ╭─────╴ │ ╶─╮ ╰───╮ ┃\n" +
        "┃ ╭─┶━╋━╾─┴─╼━╋━  ├─╼━╋━╾─────┴───╮ ╭─┴───┴───╮ │ ┃\n" +
        "┃ │ ┬ ┕───┬─╴ ┕───╯ ┬ ┻ ╭─╴ ╭─╴ ┬ ┴ │ ╭───┬─╴ │ │ ┃\n" +
        "┃ ┴ ┖━┳━  │  ━┳━╾───┶━┳━┵─┮━┪  ━┪ ┬ ┟─╯ ╶─╯ ╭─╯ │ ┃\n" +
        "┠─────┦ ╶─┴─╮ ╿ ╶───╮ ┻ ┬ │ ┻ ┬ ┞─╯ ╿ ╭─────┤ ╭─╯ ┃\n" +
        "┠───╴ ┟───╮ ┴ ╽ ╶───┴─┰─┤ │ ┎─╯ ╽  ━┥ ┴ ┬ ┬ │ │ ╭─┨\n" +
        "┃ ┬  ━┫ ┬ ╰─┮━╉───╴  ━┫ ┴ ┝━┛ ┍━┛ ┬ ┟───┤ │ │ ┴ │ ┃\n" +
        "┃ ├─╮ ┕─┤ ┬ │ ┻ ╭─┬───┦ ╶─┼───┤ ╶─┤ ╿ ╶─╯ ╰─╯ ╶─┤ ┃\n" +
        "┃ │ ├─┒ ┴ │ ┴ ┳ ┴ │ ┬ ╽ ┬ ┴ ┬ ╰─╮ ┖━┵─┬─────╮ ┬ ┴ ┃\n" +
        "┃ │ ┴ ┣━╾─┴─╴ ┣━╾─╯ │ ┣━┵───╆━  ┢━╾─┒ │ ╭───╯ ╰───┨\n" +
        "┃ ┴ ┬ ┞───┬───┦ ╶─┬─╯ ╿ ┬ ╭─┚ ╭─┚ ┬ ╿ │ ╰─╮ ╭───╮ ┃\n" +
        "┠───╯ ╽ ┬ ╰─╴ ┟─╴ │ ╶─┧ ╰─╯ ┳ │ ┳ ┖━┥ │ ┬ │ ╰─╴ │ ┃\n" +
        "┠─╴  ━┻━┵─╮  ━┻━┑ ╰─╼━┻━┭───┺━┙ ┡━  │ ┴ │ ┴ ┬ ╶─┤ ┃\n" +
        "┃ ╶─────╮ ╰───╴ ╰─╴ ┬ ┬ ┴ ╶───╮ ╰───┴───┴───┴───╯ ┃\n" +
        "┗━━━━━━━┷━━━━━━━━━━━┷━┷━━━━━━━┷━━━━━━━━━━━━━━━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze