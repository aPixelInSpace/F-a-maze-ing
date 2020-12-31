// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Ortho.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Grid.Ortho

[<Fact>]
let ``Given an empty canvas, when creating a grid, then the grid should also be empty`` () =

    // arrange
    let emptyStringCanvas =
        Convert.startLineTag +  "\n" +
        Convert.endLineTag

    let emptyCanvas = Convert.fromString emptyStringCanvas

    // act
    let grid = emptyCanvas.Value |> OrthoGrid.create

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
    let grid = singleZoneCanvas.Value |> OrthoGrid.create
    
    // assert
    grid.Cells.Length |> should equal 1
    
    grid.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side horizontally, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Convert.startLineTag + "\n" +
        "**\n" +
        Convert.endLineTag

    let twoZonesCanvas = Convert.fromString twoZonesStringCanvas

    // act
    let grid = twoZonesCanvas.Value |> OrthoGrid.create
    
    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[0, *].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    grid.Cells.[0, 1].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 1].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[0, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }

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
    let grid = twoZonesCanvas.Value |> OrthoGrid.create

    // assert
    grid.Cells.Length |> should equal 2
    grid.Cells.[*, 0].Length |> should equal 2
    
    grid.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    grid.Cells.[1, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[1, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[1, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
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
    let grid = threeByThreeCanvas.Value |> OrthoGrid.create

    // assert
    grid.Cells.Length |> should equal 9
    grid.Cells.[0, *].Length |> should equal 3
    grid.Cells.[1, *].Length |> should equal 3
    grid.Cells.[2, *].Length |> should equal 3
    grid.Cells.[*, 0].Length |> should equal 3
    grid.Cells.[*, 1].Length |> should equal 3
    grid.Cells.[*, 2].Length |> should equal 3

    // 1
    grid.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 2
    grid.Cells.[1, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[1, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[1, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 3
    grid.Cells.[2, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[2, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[2, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[2, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 4
    grid.Cells.[0, 1].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[0, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 5
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 6
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[2, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    grid.Cells.[2, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[2, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 7
    grid.Cells.[0, 2].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[0, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[0, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 8
    grid.Cells.[1, 2].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[1, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[1, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 9
    grid.Cells.[2, 2].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    grid.Cells.[2, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[2, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[2, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }

[<Fact>]  
let ``Given a grid, when linking a cell, then the neighbors walls should be empty at the positions`` () =

    // arrange
    let threeByThreeStringCanvas =
        Convert.startLineTag + "\n" +
        "***\n" +
        "***\n" +
        "***\n" +
        Convert.endLineTag

    let threeByThreeCanvas = Convert.fromString threeByThreeStringCanvas
    let grid = threeByThreeCanvas.Value |> OrthoGrid.create

    let coordinate11 = { RIndex = 1; CIndex = 1 }

    // act + assert
    
    // act top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    
    grid.LinkCells coordinate11 (OrthoCoordinate.neighborCoordinateAt coordinate11 Top)
    
    // assert top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    
    //
    
    // act right    
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    
    grid.LinkCells coordinate11 (OrthoCoordinate.neighborCoordinateAt coordinate11 Right)
    
    // assert right
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Empty }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    
    //
    
    // act bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    
    grid.LinkCells coordinate11 (OrthoCoordinate.neighborCoordinateAt coordinate11 Bottom)
    
    // assert bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    
    //
    
    // act left    
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    
    grid.LinkCells coordinate11 (OrthoCoordinate.neighborCoordinateAt coordinate11 Left)
    
    // assert left
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Empty }