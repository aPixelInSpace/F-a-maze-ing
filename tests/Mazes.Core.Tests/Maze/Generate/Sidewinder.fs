// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Sidewinder

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Tests.Helpers
open Mazes.Core.Canvas.Shape
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Creating a rectangular 5 by 10 maze generated with the sidewinder algorithm (Top, Right, rng 1) should be like the expected output`` () =
    // arrange
    let grid =
        (Rectangle.create 5 10)
        |> OrthoGrid.create
    
    // act
    let maze = grid.ToGrid |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
        
    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|_ _   _   _ _     _|\n" +
        "|       |  _ _|_|   |\n" +
        "| | |_| |    _  | |_|\n" +
        "| | |_ _|_| |_  |_  |\n" +
        "|_|_ _ _ _|_ _|_ _|_|\n"
        
    maze.Grid.ToGrid.ToString |> should equal expectedMaze

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
        |> OrthoGrid.create

    let direction1 = mapSidewinderDirectionEnumToSidewinderDirection direction1
    let direction2 = mapSidewinderDirectionEnumToSidewinderDirection direction2

    // act
    let maze = Sidewinder.createMaze direction1 direction2 rngSeed direction1Weight direction2Weight gridRectangle.ToGrid

    // we use the map to ensure that the total nodes accessible in the maze is equal to the total number of maze nodes of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let rootCoordinate = snd gridRectangle.Canvas.GetFirstTopLeftPartOfMazeZone
    let map = maze.createDijkstraMap rootCoordinate

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
let ``Given a triangular canvas, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (baseLength, baseAt, baseDecrement, heightIncrement,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    let baseAt = mapBaseAtEnumToBaseAt baseAt

    // arrange
    let gridTriangle =
        TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement
        |> OrthoGrid.create

    let direction1 = mapSidewinderDirectionEnumToSidewinderDirection direction1
    let direction2 = mapSidewinderDirectionEnumToSidewinderDirection direction2

    // act
    let maze = Sidewinder.createMaze direction1 direction2 rngSeed direction1Weight direction2Weight gridTriangle.ToGrid

    // we use the map to ensure that the total nodes accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells