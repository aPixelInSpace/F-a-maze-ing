// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

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

[<Theory>]
[<InlineData(1, 1, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 1)>]
[<InlineData(1, 1, DirectionEnum.Top, DirectionEnum.Right, 1, 2, 1)>]
[<InlineData(1, 1, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 2)>]
[<InlineData(1, 2, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 1)>]
[<InlineData(2, 1, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 1)>]
[<InlineData(1, 5, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 1)>]
[<InlineData(5, 1, DirectionEnum.Top, DirectionEnum.Right, 1, 1, 1)>]

[<InlineData(5, 5, DirectionEnum.Top, DirectionEnum.Right, 2, 1, 1)>]
[<InlineData(5, 5, DirectionEnum.Top, DirectionEnum.Left, 2, 1, 1)>]
[<InlineData(10, 25, DirectionEnum.Right, DirectionEnum.Top, 3, 1, 1)>]
[<InlineData(25, 5, DirectionEnum.Right, DirectionEnum.Bottom, 4, 2, 1)>]
[<InlineData(25, 25, DirectionEnum.Bottom, DirectionEnum.Left, 5, 2, 3)>]
[<InlineData(25, 25, DirectionEnum.Bottom, DirectionEnum.Right, 6, 1, 1)>]
[<InlineData(25, 35, DirectionEnum.Left, DirectionEnum.Top, 7, 3, 2)>]
[<InlineData(40, 25, DirectionEnum.Left, DirectionEnum.Bottom, 8, 1, 1)>]
let ``Given a rectangular canvas, when a creating a maze with the sidewinder algorithm, then the maze should have every cell accessible``
    (numberOfRows, numberOfColumns,
     direction1, direction2,
     rngSeed,
     direction1Weight, direction2Weight) =

    // arrange
    let gridRectangle =
        Rectangle.create numberOfRows numberOfColumns
        |> Grid.create

    let direction1 = mapDirectionEnumToDirection direction1
    let direction2 = mapDirectionEnumToDirection direction2

    // act
    let maze = Sidewinder.createMaze direction1 direction2 (Random(rngSeed)) direction1Weight direction2Weight gridRectangle

    // we use the map to ensure that the total zones accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let root = { RowIndex = 0; ColumnIndex = 0  }
    let map = Dijkstra.createMap root maze

    // assert
    map.TotalZonesAccessibleFromRoot |> should equal (maze.Grid.Canvas).TotalOfMazeZones