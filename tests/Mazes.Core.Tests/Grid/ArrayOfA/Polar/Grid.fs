// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.ArrayOfA.Polar.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid.ArrayOfA.Polar

[<Fact>]
let ``Given a canvas, when creating a grid, then the grid should be empty`` () =

    // arrange
    let emptyCanvas = Canvas.createPolar 0 1.0 1 (fun _ _ -> true)

    // act
    let grid = emptyCanvas |> PolarGrid.Create Close

    // assert
    grid.Cells.Length |> should equal 0

[<Fact>]  
let ``Given a canvas with one ring and a single zone part of the maze, when creating a grid, then the grid should contain a single cell with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 1 ring with 1 cell
    let emptyCanvas = Canvas.createPolar 1 1.0 1 (fun _ _ -> true)

    // act
    let grid = emptyCanvas |> PolarGrid.Create Close
    
    // assert
    grid.Cells.Length |> should equal 1

    grid.Cells.[0].Length |> should equal 1

    grid.Cells.[0].[0].Walls.Length |> should equal 3

    grid.Cells.[0].[0].WallTypeAtPosition Outward |> should equal ClosePersistent
    grid.Cells.[0].[0].WallTypeAtPosition Ccw |> should equal Close
    grid.Cells.[0].[0].WallTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a canvas with one ring and two zone part of the maze, when creating a grid, then the grid should contain a two cells with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 1 ring with 2 cells
    let emptyCanvas = Canvas.createPolar 1 1.0 2 (fun _ _ -> true)

    // act
    let grid = emptyCanvas |> PolarGrid.Create Close
    
    // assert
    grid.Cells.Length |> should equal 1

    grid.Cells.[0].Length |> should equal 2

    grid.Cells.[0].[0].Walls.Length |> should equal 3

    grid.Cells.[0].[0].WallTypeAtPosition Outward |> should equal ClosePersistent
    grid.Cells.[0].[0].WallTypeAtPosition Ccw |> should equal Close
    grid.Cells.[0].[0].WallTypeAtPosition Cw |> should equal Close

    grid.Cells.[0].[1].Walls.Length |> should equal 3
    grid.Cells.[0].[1].WallTypeAtPosition Outward |> should equal ClosePersistent
    grid.Cells.[0].[1].WallTypeAtPosition Ccw |> should equal Close
    grid.Cells.[0].[1].WallTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a canvas with three rings and three zone part of the maze for the first ring, when creating a grid, then the grid should contain a  cells with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 3 rings with 3 cells in the first ring
    let emptyCanvas = Canvas.createPolar 3 1.0 3 (fun _ _ -> true)

    // act
    let grid = emptyCanvas |> PolarGrid.Create Close
    
    // assert
    grid.Cells.Length |> should equal 3

    grid.Cells.[0].Length |> should equal 3
    grid.Cells.[1].Length |> should equal 6
    grid.Cells.[2].Length |> should equal 12

    for cIndex in 0 .. grid.Cells.[0].Length - 1 do
        grid.Cells.[0].[cIndex].Walls.Length |> should equal 2
        grid.Cells.[0].[cIndex].WallTypeAtPosition Ccw |> should equal Close
        grid.Cells.[0].[cIndex].WallTypeAtPosition Cw |> should equal Close

    for cIndex in 0 .. grid.Cells.[1].Length - 1 do
        grid.Cells.[1].[cIndex].Walls.Length |> should equal 3
        grid.Cells.[1].[cIndex].WallTypeAtPosition Inward |> should equal Close
        grid.Cells.[1].[cIndex].WallTypeAtPosition Ccw |> should equal Close
        grid.Cells.[1].[cIndex].WallTypeAtPosition Cw |> should equal Close

    for cIndex in 0 .. grid.Cells.[2].Length - 1 do
        grid.Cells.[2].[cIndex].Walls.Length |> should equal 4
        grid.Cells.[2].[cIndex].WallTypeAtPosition Outward |> should equal ClosePersistent
        grid.Cells.[2].[cIndex].WallTypeAtPosition Inward |> should equal Close
        grid.Cells.[2].[cIndex].WallTypeAtPosition Ccw |> should equal Close
        grid.Cells.[2].[cIndex].WallTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a grid, when linking a cell, then the neighbors walls should be linked`` () =

    // arrange
    // 3 rings with 3 cells in the first ring
    let emptyCanvas = Canvas.createPolar 3 1.0 3 (fun _ _ -> true)
    let grid = emptyCanvas |> PolarGrid.Create Close

    // act
    let coordinate00 = { RIndex = 0; CIndex = 0 }
    let coordinate10 = { RIndex = 1; CIndex = 0 }

    (grid.Cell coordinate00).IsLinked grid.Cells coordinate00 |> should equal false
    (grid.Cell coordinate00).AreLinked grid.Cells coordinate00 coordinate10 |> should equal false

    grid.LinkCells Open { RIndex = 0; CIndex = 0 } { RIndex = 1; CIndex = 0 }

    // assert
    (grid.Cell coordinate00).AreLinked grid.Cells coordinate00 coordinate10 |> should equal true
    (grid.Cell coordinate00).IsLinked grid.Cells coordinate00 |> should equal true
    (grid.Cell coordinate10).IsLinked grid.Cells coordinate10 |> should equal true