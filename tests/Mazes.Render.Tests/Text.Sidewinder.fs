module Mazes.Render.Tests.Text.Sidewinder

open System
open FsUnit
open Xunit
open Mazes.Lib.Cell
open Mazes.Lib.Grid
open Mazes.Lib.Algo.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a 3 by 3 maze generated with the sidewinder algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 3 3)
        |> Sidewinder.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━┓\n" +
        "┠─╴ ╶─┨\n" +
        "┃ ┬ ╶─┨\n" +
        "┗━┷━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 5 maze generated with the sidewinder algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 5 5)
        |> Sidewinder.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
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
let ``Rendering a 5 by 10 maze generated with the sidewinder algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Shape.Rectangle.create 5 10)
        |> Sidewinder.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━┓\n" +
        "┠───╴ ╶─╮ ╶───╮ ┬ ╶─┨\n" +
        "┃ ┬ ┬ ┬ │ ╶───┴─┤ ┬ ┃\n" +
        "┃ │ ├─╯ │ ┬ ╭─╴ │ ╰─┨\n" +
        "┃ │ ╰───┴─┤ ╰─╮ ╰─╮ ┃\n" +
        "┗━┷━━━━━━━┷━━━┷━━━┷━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 3 by 5 maze which has carved corner with the sidewinder algorithm (and a rng seed of 2) should be like the expected output`` () =
    // arrange
    let carvedCornerGrid =        
        
        let grid = (Shape.Rectangle.create 3 5)
        
        grid.Cells.[0, 0] <-
            {
                CellType = NotPartOfMaze
                WallTop = { WallType = Empty; WallPosition = WallPosition.Top }
                WallRight = { WallType = Empty; WallPosition = WallPosition.Right }
                WallBottom = { WallType = Empty; WallPosition = WallPosition.Bottom }
                WallLeft = { WallType = Empty; WallPosition = WallPosition.Left }
            }
            
        grid.Cells.[0, 1] <-
            {
                CellType = NotPartOfMaze
                WallTop = { WallType = Empty; WallPosition = WallPosition.Top }
                WallRight = { WallType = Border; WallPosition = WallPosition.Right }
                WallBottom = { WallType = Border; WallPosition = WallPosition.Bottom }
                WallLeft = { WallType = Empty; WallPosition = WallPosition.Left }
            }

        Grid.Wall.updateWallAtPosition Right Border 0 1 grid
        Grid.Wall.updateWallAtPosition Bottom Border 0 1 grid
        
        grid.Cells.[1, 0] <-
            {
                CellType = NotPartOfMaze
                WallTop = { WallType = Empty; WallPosition = WallPosition.Top }
                WallRight = { WallType = Border; WallPosition = WallPosition.Right }
                WallBottom = { WallType = Border; WallPosition = WallPosition.Bottom }
                WallLeft = { WallType = Empty; WallPosition = WallPosition.Left }
            }
        
        Grid.Wall.updateWallAtPosition Right Border 1 0 grid
        Grid.Wall.updateWallAtPosition Bottom Border 1 0 grid
        
        grid.Cells.[0, 4] <-
            {
                CellType = NotPartOfMaze
                WallTop = { WallType = Empty; WallPosition = WallPosition.Top }
                WallRight = { WallType = Empty; WallPosition = WallPosition.Right }
                WallBottom = { WallType = Empty; WallPosition = WallPosition.Bottom }
                WallLeft = { WallType = Empty; WallPosition = WallPosition.Left }
            }
        Grid.Wall.updateWallAtPosition Left Border 0 4 grid
        Grid.Wall.updateWallAtPosition Bottom Border 0 4 grid
        
        grid
      
    let maze =
        carvedCornerGrid
        |> Sidewinder.transformIntoMaze (Random(2))

    // act
    let renderedMaze = maze |> Text.printGrid

    // assert
    let expectedRenderedMaze =
        "    ┏━━━┓  \n" +
        "  ┏━┛ ┬ ┗━┓\n" +
        "┏━┹─╴ ╰─╮ ┃\n" +
        "┗━━━━━━━┷━┛"
        
    renderedMaze |> should equal expectedRenderedMaze