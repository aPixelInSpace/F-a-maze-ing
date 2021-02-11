// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Sidewinder

open FsUnit
open Xunit
open Mazes.Core.Tests.Helpers
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the Sidewinder algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.Grid.create
    
    // act
    let maze = grid |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|_ _   _   _ _     _|\n" +
        "|       |  _ _|_|   |\n" +
        "| | |_| |    _  | |_|\n" +
        "| | |_ _|_| |_  |_  |\n" +
        "|_|_ _ _ _|_ _|_ _|_|\n"
        
    maze.Grid |> Mazes.Core.Grid.Type.Ortho.Grid.toString |> should equal expectedMaze

type SidewinderDirectionEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

type SDE = SidewinderDirectionEnum

let mapSidewinderDirectionEnumToSidewinderDirection dirEnum =
    match dirEnum with
    | SDE.Top -> Sidewinder.Direction.Top
    | SDE.Right -> Sidewinder.Direction.Right
    | SDE.Bottom -> Sidewinder.Direction.Bottom
    | SDE.Left -> Sidewinder.Direction.Left
    | _ -> failwith "Sidewinder Direction enumeration unknown"

[<Theory>]
[<InlineData(1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(1, 1, SDE.Top, SDE.Right, 1, 2, 1)>]
[<InlineData(1, 1, SDE.Top, SDE.Right, 1, 1, 2)>]
[<InlineData(1, 2, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(2, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(1, 5, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(5, 1, SDE.Top, SDE.Right, 1, 1, 1)>]

[<InlineData(5, 5, SDE.Top, SDE.Right, 2, 1, 1)>]
[<InlineData(5, 5, SDE.Top, SDE.Left, 2, 1, 1)>]
[<InlineData(10, 25, SDE.Right, SDE.Top, 3, 1, 1)>]
[<InlineData(25, 5, SDE.Right, SDE.Bottom, 4, 2, 1)>]
[<InlineData(25, 25, SDE.Bottom, SDE.Left, 5, 2, 3)>]
[<InlineData(25, 25, SDE.Bottom, SDE.Right, 6, 1, 1)>]
[<InlineData(25, 35, SDE.Left, SDE.Top, 7, 3, 2)>]
[<InlineData(40, 25, SDE.Left, SDE.Bottom, 8, 1, 1)>]
let ``Given a rectangular canvas, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (numberOfRows, numberOfColumns,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    // arrange
    let gridRectangle =
        Rectangle.create numberOfRows numberOfColumns
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.Grid.create

    let map = mapSidewinderDirectionEnumToSidewinderDirection

    // act
    let maze = Sidewinder.createMaze (map direction1) (map direction2) rngSeed direction1Weight direction2Weight gridRectangle

    // we use the map to ensure that the total nodes accessible in the maze is equal to the total number of maze nodes of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let rootCoordinate = maze.Grid.GetFirstCellPartOfMaze
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Theory>]
[<InlineData(1, TBE.Top, 1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(1, TBE.Top, 2, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(2, TBE.Top, 1, 1, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(2, TBE.Top, 3, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(5, TBE.Top, 1, 2, SDE.Top, SDE.Right, 1, 1, 1)>]

[<InlineData(10, TBE.Top, 1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Right, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 1, 1, SDE.Left, SDE.Bottom, 1, 1, 1)>]

[<InlineData(10, TBE.Right, 1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Right, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 1, SDE.Left, SDE.Bottom, 1, 1, 1)>]

[<InlineData(10, TBE.Bottom, 1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Right, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 1, SDE.Left, SDE.Bottom, 1, 1, 1)>]

[<InlineData(10, TBE.Left, 1, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Right, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 1, 1, SDE.Left, SDE.Bottom, 1, 1, 1)>]

[<InlineData(10, TBE.Top, 3, 1, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 1, 4, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 2, 2, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 4, 2, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, TBE.Top, 2, 3, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Right, 2, 4, SDE.Right, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, TBE.Bottom, 1, 2, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, TBE.Left, 2, 1, SDE.Left, SDE.Bottom, 1, 1, 1)>]

[<InlineData(30, TBE.Left, 2, 3, SDE.Left, SDE.Bottom, 1, 1, 1)>]
let ``Given a triangular ortho grid, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (baseLength, baseAt, baseDecrement, heightIncrement,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    let baseAt = mapBaseAtEnumToBaseAt baseAt

    // arrange
    let gridTriangle =
        TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.Grid.create

    let map = mapSidewinderDirectionEnumToSidewinderDirection

    // act
    let maze = Sidewinder.createMaze (map direction1) (map direction2) rngSeed direction1Weight direction2Weight gridTriangle

    // we use the map to ensure that the total nodes accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let rootCoordinate = maze.Grid.GetFirstCellPartOfMaze
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Sidewinder algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disk.create 5 1.0 3)
        |> Mazes.Core.Grid.Type.Polar.Grid.createBaseGrid
        |> Mazes.Core.Grid.Grid.create
    
    // act
    let maze = grid |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    // assert
    let expectedMaze =
        "¦ ¦ ¦ ¦\n" +
        "|‾¦¨¦‾¦‾¦‾¦‾|\n" +
        "|¨|¨¦‾|¨|¨¦‾¦‾¦‾¦‾¦‾¦‾¦‾|\n" +
        "|¨|‾¦¨|¨|¨¦‾¦‾¦‾|¨|¨|¨|¨¦‾¦‾¦‾¦‾¦‾¦‾¦‾¦‾¦‾¦‾¦‾¦‾|\n" +
        "|¨|¨|¨|‾¦¨|‾¦‾¦¨|¨¦‾¦‾¦‾|¨¦‾|¨¦‾|¨|¨|‾¦¨¦‾¦‾¦‾|¨|\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid |> Mazes.Core.Grid.Type.Polar.Grid.toString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells

[<Theory>]
[<InlineData(1, 1.0, 1, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(3, 1.0, 1, SDE.Top, SDE.Left, 1, 1, 1)>]

[<InlineData(3, 1.0, 3, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Left, SDE.Bottom, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(3, 1.0, 3, SDE.Right, SDE.Bottom, 1, 1, 1)>]

[<InlineData(10, 1.0, 4, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Left, SDE.Bottom, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(10, 1.0, 4, SDE.Right, SDE.Bottom, 1, 1, 1)>]

[<InlineData(27, 1.0, 2, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(2, 1.0, 2, SDE.Top, SDE.Left, 1, 1, 1)>]
[<InlineData(15, 1.0, 4, SDE.Top, SDE.Right, 1, 1, 1)>]
[<InlineData(24, 1.0, 5, SDE.Bottom, SDE.Left, 1, 1, 1)>]
[<InlineData(12, 1.0, 3, SDE.Bottom, SDE.Right, 1, 1, 1)>]
[<InlineData(8, 1.0, 4, SDE.Left, SDE.Top, 1, 1, 1)>]
[<InlineData(23, 1.0, 3, SDE.Left, SDE.Bottom, 1, 1, 1)>]
[<InlineData(17, 1.0, 2, SDE.Right, SDE.Top, 1, 1, 1)>]
[<InlineData(11, 1.0, 1, SDE.Right, SDE.Bottom, 1, 1, 1)>]
let ``Given a disc polar grid, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (numberOfRings, widthHeightRatio, numberOfCellsForCenterRing,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    // arrange
    let map = mapSidewinderDirectionEnumToSidewinderDirection
    let grid =
        (Disk.create numberOfRings widthHeightRatio numberOfCellsForCenterRing)
        |> Mazes.Core.Grid.Type.Polar.Grid.createBaseGrid
        |> Mazes.Core.Grid.Grid.create
    
    // act
    let maze = grid |> Sidewinder.createMaze (map direction1) (map direction2) rngSeed direction1Weight direction2Weight

    // assert
    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells