module Mazes.Render.Tests.Text.BinaryTree

open System
open FsUnit
open Xunit
open Mazes.Lib
open Mazes.Lib.Algo.Generate
open Mazes.Render

[<Fact>]
let ``Rendering a 3 by 3 maze generated with the binary tree algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Grid.create 3 3)
        |> BinaryTree.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━┓\n" +
        "┃ ┬ ┬ ┃\n" +
        "┃ ├─╯ ┃\n" +
        "┗━┷━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 5 maze generated with the binary tree algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Grid.create 5 5)
        |> BinaryTree.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━┓\n" +
        "┃ ┬ ┬ ╭─  ┃\n" +
        "┠─╯ │ ├─  ┃\n" +
        "┃ ╭─╯ │ ┬ ┃\n" +
        "┃ ├───┴─╯ ┃\n" +
        "┗━┷━━━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Rendering a 5 by 10 maze generated with the binary tree algorithm (and a rng seed of 1) should be like the expected output`` () =
    // arrange
    let maze =
        (Grid.create 5 10)
        |> BinaryTree.transformIntoMaze (Random(1))
    
    // act
    let renderedMaze = maze |> Text.printGrid
        
    // assert
    let expectedRenderedMaze =
        "┏━━━━━━━━━━━━━━━━━━━┓\n" +
        "┃ ┬ ┬ ╭───  ┬ ╭─  ┬ ┃\n" +
        "┠─╯ │ │ ╭───┴─╯ ╭─╯ ┃\n" +
        "┠───┴─╯ │ ┬ ╭─  ├─  ┃\n" +
        "┃ ╭─────┴─┴─╯ ╭─┴─  ┃\n" +
        "┗━┷━━━━━━━━━━━┷━━━━━┛"
        
    renderedMaze |> should equal expectedRenderedMaze