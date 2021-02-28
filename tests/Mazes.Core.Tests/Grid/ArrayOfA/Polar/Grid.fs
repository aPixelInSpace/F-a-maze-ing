// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.ArrayOfA.Polar.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Polar

[<Fact>]
let ``Given a canvas, when creating a grid, then the grid should be empty`` () =

    // arrange
    let emptyCanvas = Canvas.createPolar 0 1.0 1 (fun _ _ -> true)

    // act
    let grid =
        emptyCanvas
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    // assert
    grid.TotalOfMazeCells |> should equal 0

[<Fact>]  
let ``Given a canvas with one ring and a single zone part of the maze, when creating a grid, then the grid should contain a single cell with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 1 ring with 1 cell
    let emptyCanvas = Canvas.createPolar 1 1.0 1 (fun _ _ -> true)

    // act
    let grid =
        emptyCanvas
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // assert
    grid.TotalOfMazeCells |> should equal 1

    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.[0].Length |> should equal 1

    spGrid.Cells.[0].[0].Connections.Length |> should equal 3

    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Outward |> should equal ClosePersistent
    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Ccw |> should equal Close
    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a canvas with one ring and two zone part of the maze, when creating a grid, then the grid should contain a two cells with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 1 ring with 2 cells
    let emptyCanvas = Canvas.createPolar 1 1.0 2 (fun _ _ -> true)

    // act
    let grid =
        emptyCanvas
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // assert
    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.Length |> should equal 1

    spGrid.Cells.[0].Length |> should equal 2

    spGrid.Cells.[0].[0].Connections.Length |> should equal 3

    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Outward |> should equal ClosePersistent
    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Ccw |> should equal Close
    spGrid.Cells.[0].[0].ConnectionTypeAtPosition Cw |> should equal Close

    spGrid.Cells.[0].[1].Connections.Length |> should equal 3
    spGrid.Cells.[0].[1].ConnectionTypeAtPosition Outward |> should equal ClosePersistent
    spGrid.Cells.[0].[1].ConnectionTypeAtPosition Ccw |> should equal Close
    spGrid.Cells.[0].[1].ConnectionTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a canvas with three rings and three zone part of the maze for the first ring, when creating a grid, then the grid should contain a  cells with a border outward and normal walls on ccw and cw`` () =

    // arrange
    // 3 rings with 3 cells in the first ring
    let emptyCanvas = Canvas.createPolar 3 1.0 3 (fun _ _ -> true)

    // act
    let grid =
        emptyCanvas
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D
    
    // assert
    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.Length |> should equal 3

    spGrid.Cells.[0].Length |> should equal 3
    spGrid.Cells.[1].Length |> should equal 6
    spGrid.Cells.[2].Length |> should equal 12

    for cIndex in 0 .. spGrid.Cells.[0].Length - 1 do
        spGrid.Cells.[0].[cIndex].Connections.Length |> should equal 2
        spGrid.Cells.[0].[cIndex].ConnectionTypeAtPosition Ccw |> should equal Close
        spGrid.Cells.[0].[cIndex].ConnectionTypeAtPosition Cw |> should equal Close

    for cIndex in 0 .. spGrid.Cells.[1].Length - 1 do
        spGrid.Cells.[1].[cIndex].Connections.Length |> should equal 3
        spGrid.Cells.[1].[cIndex].ConnectionTypeAtPosition Inward |> should equal Close
        spGrid.Cells.[1].[cIndex].ConnectionTypeAtPosition Ccw |> should equal Close
        spGrid.Cells.[1].[cIndex].ConnectionTypeAtPosition Cw |> should equal Close

    for cIndex in 0 .. spGrid.Cells.[2].Length - 1 do
        spGrid.Cells.[2].[cIndex].Connections.Length |> should equal 4
        spGrid.Cells.[2].[cIndex].ConnectionTypeAtPosition Outward |> should equal ClosePersistent
        spGrid.Cells.[2].[cIndex].ConnectionTypeAtPosition Inward |> should equal Close
        spGrid.Cells.[2].[cIndex].ConnectionTypeAtPosition Ccw |> should equal Close
        spGrid.Cells.[2].[cIndex].ConnectionTypeAtPosition Cw |> should equal Close

[<Fact>]  
let ``Given a grid, when linking a cell, then the neighbors walls should be linked`` () =

    // arrange
    // 3 rings with 3 cells in the first ring
    let emptyCanvas = Canvas.createPolar 3 1.0 3 (fun _ _ -> true)
    let grid =
        emptyCanvas
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    // act
    let coordinate00 = NCoordinate.createFrom2D { RIndex = 0; CIndex = 0 }
    let coordinate10 = NCoordinate.createFrom2D { RIndex = 1; CIndex = 0 }

    grid.IsCellConnected coordinate00 |> should equal false
    grid.AreConnected coordinate00 coordinate10 |> should equal false

    grid.UpdateConnection Open (NCoordinate.createFrom2D { RIndex = 0; CIndex = 0 }) (NCoordinate.createFrom2D { RIndex = 1; CIndex = 0 })

    // assert
    grid.AreConnected coordinate00 coordinate10 |> should equal true
    grid.IsCellConnected coordinate00 |> should equal true
    grid.IsCellConnected coordinate10 |> should equal true