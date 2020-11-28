// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.Sidewinder

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Tests.Helpers
open Mazes.Core.Canvas.Shape
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate
open Mazes.Core.Maze.Analyse

let mapDirectionEnumToSidewinderDirection dirEnum =
    match dirEnum with
    | SidewinderDirectionEnum.Top -> Sidewinder.Direction.Top
    | SidewinderDirectionEnum.Right -> Sidewinder.Direction.Right
    | SidewinderDirectionEnum.Bottom -> Sidewinder.Direction.Bottom
    | SidewinderDirectionEnum.Left -> Sidewinder.Direction.Left
    | _ -> failwith "Direction enumeration unknown"

[<Theory>]
[<InlineData(1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 2, 1)>]
[<InlineData(1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 2)>]
[<InlineData(1, 2, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(2, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(1, 5, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(5, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]

[<InlineData(5, 5, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 2, 1, 1)>]
[<InlineData(5, 5, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 2, 1, 1)>]
[<InlineData(10, 25, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 3, 1, 1)>]
[<InlineData(25, 5, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 4, 2, 1)>]
[<InlineData(25, 25, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 5, 2, 3)>]
[<InlineData(25, 25, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 6, 1, 1)>]
[<InlineData(25, 35, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 7, 3, 2)>]
[<InlineData(40, 25, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 8, 1, 1)>]
let ``Given a rectangular canvas, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (numberOfRows, numberOfColumns,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    // arrange
    let gridRectangle =
        Rectangle.create numberOfRows numberOfColumns
        |> Grid.create

    let direction1 = mapDirectionEnumToSidewinderDirection direction1
    let direction2 = mapDirectionEnumToSidewinderDirection direction2

    // act
    let maze = Sidewinder.createMaze direction1 direction2 (Random(rngSeed)) direction1Weight direction2Weight gridRectangle

    // we use the map to ensure that the total zones accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let (_, rootCoordinate) = gridRectangle.Canvas.GetFirstTopLeftPartOfMazeZone
    let map = Dijkstra.createMap rootCoordinate maze

    // assert
    map.TotalZonesAccessibleFromRoot |> should equal (maze.Grid.Canvas).TotalOfMazeZones

[<Theory>]
[<InlineData(1, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(1, TriangleIsoscelesBaseAtEnum.Top, 2, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(2, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(2, TriangleIsoscelesBaseAtEnum.Top, 3, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(5, TriangleIsoscelesBaseAtEnum.Top, 1, 2, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]

[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]

[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]

[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]

[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 1, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]

[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 3, 1, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 1, 4, SidewinderDirectionEnum.Top, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 2, 2, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Right, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 4, 2, SidewinderDirectionEnum.Bottom, SidewinderDirectionEnum.Left, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Top, 2, 3, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Right, 2, 4, SidewinderDirectionEnum.Right, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Bottom, 1, 2, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Top, 1, 1, 1)>]
[<InlineData(10, TriangleIsoscelesBaseAtEnum.Left, 2, 1, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]

[<InlineData(30, TriangleIsoscelesBaseAtEnum.Left, 2, 3, SidewinderDirectionEnum.Left, SidewinderDirectionEnum.Bottom, 1, 1, 1)>]
let ``Given a triangular canvas, when creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (baseLength, baseAt, baseDecrement, heightIncrement,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    let baseAt = mapBaseAtEnumToBaseAt baseAt

    // arrange
    let gridTriangle =
        TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement
        |> Grid.create

    let direction1 = mapDirectionEnumToSidewinderDirection direction1
    let direction2 = mapDirectionEnumToSidewinderDirection direction2

    // act
    let maze = Sidewinder.createMaze direction1 direction2 (Random(rngSeed)) direction1Weight direction2Weight gridTriangle

    // we use the map to ensure that the total zones accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let (_, rootCoordinate) = gridTriangle.Canvas.GetFirstTopLeftPartOfMazeZone
    let map = Dijkstra.createMap rootCoordinate maze

    // assert
    map.TotalZonesAccessibleFromRoot |> should equal (maze.Grid.Canvas).TotalOfMazeZones