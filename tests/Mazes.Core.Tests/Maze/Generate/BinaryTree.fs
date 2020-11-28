// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.BinaryTree

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
let ``Given a rectangular canvas, when a creating a maze with the binary tree algorithm, then the maze should have every cell accessible``
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
    let maze = BinaryTree.createMaze direction1 direction2 (Random(rngSeed)) direction1Weight direction2Weight gridRectangle

    // we use the map to ensure that the total zones accessible in the maze is equal to the total number of maze zones of the canvas
    // thus ensuring that the every cell in the maze is accessible after creating the maze
    let (_, rootCoordinate) = gridRectangle.Canvas.GetFirstTopLeftPartOfMazeZone
    let map = Dijkstra.createMap rootCoordinate maze

    // assert
    map.TotalZonesAccessibleFromRoot |> should equal (maze.Grid.Canvas).TotalOfMazeZones