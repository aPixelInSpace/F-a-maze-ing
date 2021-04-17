// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.GrowingTree

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the growing tree mix random and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixRandomAndLast.createMaze 1 0.5
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "| |   | |   |  _ _  |\n" +
        "| | |_  | |_|_ _  |_|\n" +
        "|  _  |  _ _ _  |_  |\n" +
        "|_| |_|  _ _ _|  _ _|\n" +
        "|_ _ _ _|_ _ _ _ _ _|\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the growing tree mix random and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixRandomAndLast.createMaze 1 0.5

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "¦‾¦¨|‾¦‾|‾¦‾¦\n" +
        "|‾|‾|‾¦¨¦‾¦‾¦¨|‾¦¨|‾¦‾|¨|\n" +
        "¦¨¦‾¦¨|¨¦‾¦‾|‾|¨|¨¦‾|‾¦‾|¨|¨|‾|¨¦‾|¨|‾¦‾|¨|¨¦¨¦‾¦\n" +
        "|‾¦‾¦‾¦‾¦‾¦¨|¨¦‾¦¨|¨¦¨|‾¦‾¦‾¦¨¦¨¦‾¦‾|¨|¨¦¨|¨|‾¦¨|\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the growing tree mix oldest and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixOldestAndLast.createMaze 1 0.5
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|  _   _ _ _  |   | |\n" +
        "| |_        | | |_  |\n" +
        "| |_  |_| |_|_ _|  _|\n" +
        "|_| | | | |_ _  |_  |\n" +
        "|_ _ _ _|_ _ _|_ _ _|\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the growing tree mix oldest and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixOldestAndLast.createMaze 1 0.5

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "¦‾¦‾|‾¦‾¦‾¦¨¦\n" +
        "|‾|‾¦‾|¨|‾|¨|‾|‾¦¨¦‾¦‾|‾|\n" +
        "|‾¦¨|¨|‾¦‾|¨¦‾|¨¦‾¦¨¦‾|¨|¨|¨¦¨|‾¦¨¦‾¦‾|¨¦‾¦‾¦¨|¨|\n" +
        "¦¨|¨|¨¦¨|¨¦‾¦‾¦‾¦‾¦‾¦¨|¨|¨|‾¦‾¦¨|¨|¨¦‾¦‾¦‾¦‾¦‾¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the growing tree mix chosen random and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixChosenRandomAndLast.createMaze 1 0.5
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|_  |  _|_   _   _| |\n" +
        "|_     _| |  _|    _|\n" +
        "|_  |_       _|_|  _|\n" +
        "|    _|_|_|   |_    |\n" +
        "|_|_|_ _ _ _|_ _|_|_|\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the growing tree mix chosen random and last algorithm (rng 1, longPassages 0.5), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeMixChosenRandomAndLast.createMaze 1 0.5

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "|‾|‾¦‾¦‾¦¨¦‾|\n" +
        "¦¨¦‾|¨|¨|¨|‾¦‾¦‾¦¨¦‾¦‾¦‾¦\n" +
        "|¨|¨|‾¦‾¦¨|¨|¨|¨|¨|¨¦‾|‾|¨|¨|¨|¨|¨|¨¦‾|¨|¨|¨|¨|¨|\n" +
        "¦‾¦‾|¨|¨|¨|‾¦‾¦‾¦¨¦‾|‾¦¨¦‾¦¨|‾¦‾¦¨|¨|‾¦‾¦‾¦¨¦‾¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the growing tree direction algorithm (rng 1, toRightWeight 0.3, toBottomWeight 0.2, toLeftWeight 0.3), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeDirection.createMaze 1 0.3 0.2 0.3
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|_   _ _  |   |_    |\n" +
        "|  _|_  | | |_ _ _| |\n" +
        "| |  _ _| |_  | |  _|\n" +
        "| | | |  _|  _|  _| |\n" +
        "|_ _|_ _ _ _|_ _ _ _|\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the growing tree direction algorithm (rng 1, toRightWeight 0.3, toBottomWeight 0.2, toLeftWeight 0.3), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeDirection.createMaze 1 0.3 0.2 0.3

    // assert
    let expectedMaze =
        "¦ ¦ | ¦\n" +
        "¦‾¦‾|‾|¨|¨¦‾¦\n" +
        "¦‾¦‾¦‾|¨|‾¦¨|¨¦‾|‾|‾¦‾¦‾¦\n" +
        "|‾¦‾¦‾|‾|¨|‾¦‾|¨|¨¦‾|‾|¨¦‾|‾¦¨|¨¦‾|¨|¨¦‾|‾¦‾|‾¦‾|\n" +
        "¦‾¦¨|¨¦¨|¨¦¨|¨¦¨¦‾¦¨|¨¦‾¦¨|¨¦‾¦‾¦‾¦¨|‾¦¨¦¨|¨¦¨|¨¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the growing tree spiral algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeSpiral.createMaze 1 0.9 0.9 3 0.5
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|  _ _ _|   |  _ _  |\n" +
        "|   |_ _ _| |  _  | |\n" +
        "| |_ _ _  | | | | | |\n" +
        "| | |  _ _| |_ _| | |\n" +
        "|_ _|_ _ _ _ _ _ _|_|\n"

    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the growing tree spiral algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // act
    let maze = grid |> GrowingTreeSpiral.createMaze 1 0.9 0.9 3 0.5

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "|¨¦‾¦‾¦‾|‾¦¨|\n" +
        "¦‾|¨|‾¦‾¦‾¦‾|‾|¨|‾¦‾¦‾¦‾¦\n" +
        "¦‾|¨|‾|¨|‾¦¨|¨|‾¦‾¦‾¦‾|¨|¨|‾¦‾¦¨¦‾|‾¦‾¦‾¦‾¦‾¦‾¦‾¦\n" +
        "¦‾¦¨|¨¦¨¦‾¦‾¦¨|¨|¨¦‾¦‾¦¨¦¨|¨¦‾¦‾¦‾¦¨¦‾¦‾¦‾¦‾|¨¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"

    snd maze.NDStruct.FirstSlice2D |> Grid2D.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDStruct.TotalOfMazeCells