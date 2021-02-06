// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Array2D.Ortho.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid.Array2D.Ortho

[<Fact>]
let ``Given a canvas, when creating a grid, then the grid should also be empty`` () =

    // arrange
    let emptyStringCanvas =
        Convert.startLineTag +  "\n" +
        Convert.endLineTag

    let emptyCanvas = Convert.fromString emptyStringCanvas

    // act
    let grid = emptyCanvas.Value |> OrthoGrid.Create Close

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
    let grid = singleZoneCanvas.Value |> OrthoGrid.Create Close
    
    // assert
    grid.Cells.Length |> should equal 1
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal ClosePersistent

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side horizontally, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Convert.startLineTag + "\n" +
        "**\n" +
        Convert.endLineTag

    let twoZonesCanvas = Convert.fromString twoZonesStringCanvas

    // act
    let grid = twoZonesCanvas.Value |> OrthoGrid.Create Close
    
    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[0, *].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
    grid.Cells.[0, 1].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 1].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[0, 1].WallTypeAtPosition Left |> should equal Close

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
    let grid = twoZonesCanvas.Value |> OrthoGrid.Create Close

    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[*, 0].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
    grid.Cells.[1, 0].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[1, 0].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[1, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
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
    let grid = threeByThreeCanvas.Value |> OrthoGrid.Create Close

    // assert
    grid.Cells.Length |> should equal 9
    grid.Cells.[0, *].Length |> should equal 3
    grid.Cells.[1, *].Length |> should equal 3
    grid.Cells.[2, *].Length |> should equal 3
    grid.Cells.[*, 0].Length |> should equal 3
    grid.Cells.[*, 1].Length |> should equal 3
    grid.Cells.[*, 2].Length |> should equal 3

    // 1
    grid.Cells.[0, 0].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 0].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[0, 0].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[0, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
    // 2
    grid.Cells.[1, 0].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[1, 0].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[1, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
    // 3
    grid.Cells.[2, 0].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[2, 0].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[2, 0].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[2, 0].WallTypeAtPosition Left |> should equal ClosePersistent
    
    // 4
    grid.Cells.[0, 1].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 1].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[0, 1].WallTypeAtPosition Left |> should equal Close
    
    // 5
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Close
    
    // 6
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[2, 1].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[2, 1].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[2, 1].WallTypeAtPosition Left |> should equal Close
    
    // 7
    grid.Cells.[0, 2].WallTypeAtPosition Top |> should equal ClosePersistent
    grid.Cells.[0, 2].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[0, 2].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[0, 2].WallTypeAtPosition Left |> should equal Close
    
    // 8
    grid.Cells.[1, 2].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[1, 2].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[1, 2].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Close
    
    // 9
    grid.Cells.[2, 2].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[2, 2].WallTypeAtPosition Right |> should equal ClosePersistent
    grid.Cells.[2, 2].WallTypeAtPosition Bottom |> should equal ClosePersistent
    grid.Cells.[2, 2].WallTypeAtPosition Left |> should equal Close

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
    let grid = threeByThreeCanvas.Value |> OrthoGrid.Create Close

    let coordinate11 = { RIndex = 1; CIndex = 1 }

    // act + assert
    
    // act top
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Close
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Top) with
    | Some n -> grid.ToInterface.UpdateConnection Open coordinate11 n
    | None -> failwith "Fail"
    
    // assert top
    grid.Cells.[1, 1].WallTypeAtPosition Top |> should equal Open
    grid.Cells.[0, 1].WallTypeAtPosition Bottom |> should equal Open
    
    //
    
    // act right    
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Close
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Right) with
    | Some n -> grid.ToInterface.UpdateConnection Open coordinate11 n
    | None -> failwith "Fail"
    
    // assert right
    grid.Cells.[1, 1].WallTypeAtPosition Right |> should equal Open
    grid.Cells.[1, 2].WallTypeAtPosition Left |> should equal Open
    
    //
    
    // act bottom
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Close
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Bottom) with
    | Some n -> grid.ToInterface.UpdateConnection Open coordinate11 n
    | None -> failwith "Fail"
    
    // assert bottom
    grid.Cells.[1, 1].WallTypeAtPosition Bottom |> should equal Open
    grid.Cells.[2, 1].WallTypeAtPosition Top |> should equal Open
    
    //
    
    // act left    
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Close
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Left) with
    | Some n -> grid.ToInterface.UpdateConnection Open coordinate11 n
    | None -> failwith "Fail"
    
    // assert left
    grid.Cells.[1, 1].WallTypeAtPosition Left |> should equal Open
    grid.Cells.[1, 0].WallTypeAtPosition Right |> should equal Open