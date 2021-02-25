﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.HuntAndKill

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Maze.Generate

let createCoordinate2D = NCoordinate.create [| 0 |]

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let orthoGrid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D
    
    // act
    let maze = orthoGrid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|  _|    _|  _|     |\n" +
        "|_  |_|_ _  |  _| |_|\n" +
        "|  _ _ _|  _| |_ _  |\n" +
        "| |  _ _ _ _|_ _  | |\n" +
        "|_ _ _ _ _ _ _ _ _|_|\n"

    snd maze.NDimensionalStructure.FirstSlice2D |> Mazes.Core.Grid.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a ortho grid 5 by 10 with non adjacent neighbors, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output and every cell should be accessible`` () =
    // arrange
    let orthoGrid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    orthoGrid.NonAdjacent2DConnections.UpdateConnection ConnectionType.Close (createCoordinate2D { RIndex = 2; CIndex = 4 }) (createCoordinate2D { RIndex = 4; CIndex = 4 })
    orthoGrid.NonAdjacent2DConnections.UpdateConnection ConnectionType.Close (createCoordinate2D { RIndex = 3; CIndex = 2 }) (createCoordinate2D { RIndex = 4; CIndex = 4 })
    orthoGrid.NonAdjacent2DConnections.UpdateConnection ConnectionType.Close (createCoordinate2D { RIndex = 3; CIndex = 2 }) (createCoordinate2D { RIndex = 1; CIndex = 1 })
    orthoGrid.NonAdjacent2DConnections.UpdateConnection ConnectionType.Close (createCoordinate2D { RIndex = 4; CIndex = 8 }) (createCoordinate2D { RIndex = 2; CIndex = 7 })
    
    // act
    let maze = orthoGrid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" + 
        "|   |   |  _ _  |   |\n" +
        "|_|_ _|_ _  | |_ _| |\n" +
        "|  _ _ _ _ _|_ _ _ _|\n" +
        "| |     |  _    |   |\n" +
        "|_ _|_|_ _|_ _|_ _|_|\n"

    snd maze.NDimensionalStructure.FirstSlice2D |> Mazes.Core.Grid.Type.Ortho.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Mazes.Core.Grid.Type.Polar.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "¦¨¦‾|‾¦‾|‾|‾¦\n" +
        "¦‾¦‾|¨|‾¦¨|‾¦‾|¨|¨|¨|¨|‾¦\n" +
        "¦‾¦‾|‾¦¨|¨|‾¦‾|¨|¨|‾¦‾|¨¦‾|¨|‾|¨|‾¦‾|¨¦‾¦‾¦‾|‾|¨¦\n" +
        "¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨¦‾¦¨|¨¦¨|¨¦¨¦‾¦‾¦‾¦¨|¨¦¨¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    snd maze.NDimensionalStructure.FirstSlice2D |> Mazes.Core.Grid.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a triangular grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Tri.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    //let expectedMaze = ""
        
    //maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a octagon-square grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.OctaSquare.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    //let expectedMaze = ""

    //maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a Cairo pentagonal grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.PentaCairo.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    //let expectedMaze = ""
        
    //maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells

[<Fact>]
let ``Given a brick grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Brick.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    //let expectedMaze = ""

    //maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.NDimensionalStructure.TotalOfMazeCells