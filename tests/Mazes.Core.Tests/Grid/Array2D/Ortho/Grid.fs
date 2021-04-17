// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Array2D.Ortho.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D.Type.Ortho

[<Fact>]
let ``Given a empty canvas, when creating a 2d structure, then the 2d structure should also be empty`` () =

    // arrange
    let emptyStringCanvas =
        Convert.startLineTag +  "\n" +
        Convert.endLineTag

    let emptyCanvas = Convert.fromString emptyStringCanvas

    // act
    let grid = emptyCanvas.Value
               |> Grid.createBaseGrid
               |> NDimensionalStructure.create2D

    // assert
    grid.TotalOfMazeCells |> should equal 0

[<Fact>]  
let ``Given a canvas with a single zone part of the maze, when creating a 2d structure, then the 2d structure should contain a single cell with only borders`` () =

    // arrange
    let singleZoneStringCanvas =
        Convert.startLineTag + "\n" +
        "*\n" +
        Convert.endLineTag

    let singleZoneCanvas = Convert.fromString singleZoneStringCanvas

    // act
    let grid = singleZoneCanvas.Value
               |> Grid.createBaseGrid
               |> NDimensionalStructure.create2D
    
    // assert
    grid.TotalOfMazeCells |> should equal 1
    
    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side horizontally, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Convert.startLineTag + "\n" +
        "**\n" +
        Convert.endLineTag

    let twoZonesCanvas = Convert.fromString twoZonesStringCanvas

    // act
    let grid = twoZonesCanvas.Value
               |> Grid.createBaseGrid
               |> NDimensionalStructure.create2D
    
    // assert
    grid.TotalOfMazeCells |> should equal 2

    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.[0, *].Length |> should equal 2
    
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent

    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Left |> should equal Close

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
    let grid =
        twoZonesCanvas.Value
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    // assert
    grid.TotalOfMazeCells |> should equal 2

    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.[*, 0].Length |> should equal 2
    
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent
    
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent
    
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
    let grid =
        threeByThreeCanvas.Value
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    // assert
    grid.TotalOfMazeCells |> should equal 9

    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    spGrid.Cells.[0, *].Length |> should equal 3
    spGrid.Cells.[1, *].Length |> should equal 3
    spGrid.Cells.[2, *].Length |> should equal 3
    spGrid.Cells.[*, 0].Length |> should equal 3
    spGrid.Cells.[*, 1].Length |> should equal 3
    spGrid.Cells.[*, 2].Length |> should equal 3

    // 1
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[0, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent
    
    // 2
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent
    
    // 3
    spGrid.Cells.[2, 0].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[2, 0].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[2, 0].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[2, 0].ConnectionTypeAtPosition Left |> should equal ClosePersistent
    
    // 4
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Left |> should equal Close
    
    // 5
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Left |> should equal Close
    
    // 6
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Left |> should equal Close
    
    // 7
    spGrid.Cells.[0, 2].ConnectionTypeAtPosition Top |> should equal ClosePersistent
    spGrid.Cells.[0, 2].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[0, 2].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[0, 2].ConnectionTypeAtPosition Left |> should equal Close
    
    // 8
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Left |> should equal Close
    
    // 9
    spGrid.Cells.[2, 2].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[2, 2].ConnectionTypeAtPosition Right |> should equal ClosePersistent
    spGrid.Cells.[2, 2].ConnectionTypeAtPosition Bottom |> should equal ClosePersistent
    spGrid.Cells.[2, 2].ConnectionTypeAtPosition Left |> should equal Close

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
    let grid =
        threeByThreeCanvas.Value
        |> Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let spGrid = (snd grid.FirstSlice2D).ToSpecializedStructure
    let coordinate11 = { RIndex = 1; CIndex = 1 }
    let nCoordinate11 = NCoordinate.createFrom2D coordinate11

    // act + assert
    
    // act top
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Top |> should equal Close
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Bottom |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Top) with
    | Some n -> grid.UpdateConnection Open nCoordinate11 (NCoordinate.createFrom2D n)
    | None -> failwith "Fail"
    
    // assert top
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Top |> should equal Open
    spGrid.Cells.[0, 1].ConnectionTypeAtPosition Bottom |> should equal Open
    
    //
    
    // act right    
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Right |> should equal Close
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Left |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Right) with
    | Some n -> grid.UpdateConnection Open nCoordinate11 (NCoordinate.createFrom2D n)
    | None -> failwith "Fail"
    
    // assert right
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Right |> should equal Open
    spGrid.Cells.[1, 2].ConnectionTypeAtPosition Left |> should equal Open
    
    //
    
    // act bottom
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Bottom |> should equal Close
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Top |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Bottom) with
    | Some n -> grid.UpdateConnection Open nCoordinate11 (NCoordinate.createFrom2D n)
    | None -> failwith "Fail"
    
    // assert bottom
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Bottom |> should equal Open
    spGrid.Cells.[2, 1].ConnectionTypeAtPosition Top |> should equal Open
    
    //
    
    // act left    
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Left |> should equal Close
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Right |> should equal Close
    
    match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate11 Left) with
    | Some n -> grid.UpdateConnection Open nCoordinate11 (NCoordinate.createFrom2D n)
    | None -> failwith "Fail"
    
    // assert left 
    spGrid.Cells.[1, 1].ConnectionTypeAtPosition Left |> should equal Open
    spGrid.Cells.[1, 0].ConnectionTypeAtPosition Right |> should equal Open