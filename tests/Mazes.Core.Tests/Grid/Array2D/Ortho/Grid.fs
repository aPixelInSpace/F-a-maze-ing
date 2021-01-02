// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Array2D.Ortho.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid.Array2D.Ortho

[<Fact>]
let ``Given an empty canvas, when creating a grid, then the grid should also be empty`` () =

    // arrange
    let emptyStringCanvas =
        Convert.startLineTag +  "\n" +
        Convert.endLineTag

    let emptyCanvas = Convert.fromString emptyStringCanvas

    // act
    let grid = emptyCanvas.Value |> OrthoGrid.Create

    // assert
    grid.Cells.Length |> should equal 0

[<Fact>]  
let ``Given a canvas with a single zone part of the maze, when creating a grid, then the grid should contain a single cell with only borders`` () =

    // arrange
    let singleZoneStringCanvas =
        Convert.startLineTag + "\n" +
        "*\n" +
        Convert.endLineTag

    let singleZoneCanvas = Convert.fromString singleZoneStringCanvas

    // act
    let grid = singleZoneCanvas.Value |> OrthoGrid.Create
    
    // assert
    grid.Cells.Length |> should equal 1
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal Border

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side horizontally, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Convert.startLineTag + "\n" +
        "**\n" +
        Convert.endLineTag

    let twoZonesCanvas = Convert.fromString twoZonesStringCanvas

    // act
    let grid = twoZonesCanvas.Value |> OrthoGrid.Create
    
    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[0, *].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal Border
    
    grid.Cells.[0, 1].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 1].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[0, 1].WallTypeAtPosition Left |> should equal Normal

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side vertically, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Convert.startLineTag + "\n" +
        "*\n" +
        "*\n" +
        Convert.endLineTag

    let twoZonesCanvas = Convert.fromString twoZonesStringCanvas

    // act
    let grid = twoZonesCanvas.Value |> OrthoGrid.Create

    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[*, 0].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal Border
    
    grid.Cells.[1, 0].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[1, 0].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[1, 0].WallTypeAtPosition Left |> should equal Border
    
[<Fact>]  
let ``Given a 3x3 canvas, when creating a grid, then it should have 3x3 cells with border walls on the edge and normal walls inside`` () =

    // arrange
    let threeByThreeStringCanvas =
        Convert.startLineTag + "\n" +
        "***\n" +
        "***\n" +
        "***\n" +
        Convert.endLineTag

    let threeByThreeCanvas = Convert.fromString threeByThreeStringCanvas

    // act
    let grid = threeByThreeCanvas.Value |> OrthoGrid.Create

    // assert
    grid.Cells.Length |> should equal 9
    grid.Cells.[0, *].Length |> should equal 3
    grid.Cells.[1, *].Length |> should equal 3
    grid.Cells.[2, *].Length |> should equal 3
    grid.Cells.[*, 0].Length |> should equal 3
    grid.Cells.[*, 1].Length |> should equal 3
    grid.Cells.[*, 2].Length |> should equal 3

    // 1
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal Border
    
    // 2
    grid.Cells.[1, 0].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[1, 0].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[1, 0].WallTypeAtPosition Left |> should equal Border
    
    // 3
    grid.Cells.[2, 0].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[2, 0].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[2, 0].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[2, 0].WallTypeAtPosition Left |> should equal Border
    
    // 4
    grid.Cells.[0, 1].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 1].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[0, 1].WallTypeAtPosition Left |> should equal Normal
    
    // 5
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Normal
    
    // 6
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[2, 1].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[2, 1].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[2, 1].WallTypeAtPosition Left |> should equal Normal
    
    // 7
    grid.Cells.[0, 2].WallTypeAtPosition Top |> should equal Border
    grid.Cells.[0, 2].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[0, 2].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[0, 2].WallTypeAtPosition Left |> should equal Normal
    
    // 8
    grid.Cells.[1, 2].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[1, 2].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[1, 2].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Normal
    
    // 9
    grid.Cells.[2, 2].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[2, 2].WallTypeAtPosition Right |> should equal Border
    grid.Cells.[2, 2].WallTypeAtPosition Bottom |> should equal Border
    grid.Cells.[2, 2].WallTypeAtPosition Left |> should equal Normal

[<Fact>]  
let ``Given a grid, when linking a cell, then the neighbors walls should be empty at the positions`` () =

    // arrange
    let canvas3x3 =
        Convert.startLineTag + "\n" +
        "***\n" +
        "***\n" +
        "***\n" +
        Convert.endLineTag

    let threeByThreeCanvas = Convert.fromString canvas3x3
    let grid = threeByThreeCanvas.Value |> OrthoGrid.Create

    let coordinate11 = { RIndex = 1; CIndex = 1 }

    // act + assert
    
    // act top
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Normal
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Normal
    
    grid.ToInterface.LinkCells coordinate11 (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Top)
    
    // assert top
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Empty
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Empty
    
    //
    
    // act right    
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Normal
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Normal
    
    grid.ToInterface.LinkCells coordinate11 (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Right)
    
    // assert right
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Empty
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Empty
    
    //
    
    // act bottom
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Normal
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Normal
    
    grid.ToInterface.LinkCells coordinate11 (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Bottom)
    
    // assert bottom
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Empty
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Empty
    
    //
    
    // act left    
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Normal
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Normal
    
    grid.ToInterface.LinkCells coordinate11 (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Left)
    
    // assert left
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Empty
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Empty