// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Tests.Helpers
open Mazes.Core.Grid

[<Fact>]
let ``Given an empty canvas, when creating a grid, then the grid should also be empty`` () =

    // arrange
    let emptyStringCanvas =
        Canvas.Convert.startLineTag +    
        Canvas.Convert.endLineTag

    let emptyCanvas = Canvas.Convert.fromString emptyStringCanvas

    // act
    let sut = getValue emptyCanvas |> Grid.create

    // assert
    sut.Cells.Length |> should equal 0

[<Fact>]  
let ``Given a canvas with a single zone part of the maze, when creating a grid, then the grid should contain a single cell with only borders`` () =

    // arrange
    let singleZoneStringCanvas =
        Canvas.Convert.startLineTag +
        "*\n" +
        Canvas.Convert.endLineTag

    let singleZoneCanvas = Canvas.Convert.fromString singleZoneStringCanvas

    // act
    let sut = getValue singleZoneCanvas |> Grid.create
    
    // assert
    sut.Cells.Length |> should equal 1
    
    sut.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side horizontally, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Canvas.Convert.startLineTag +
        "**\n" +
        Canvas.Convert.endLineTag

    let twoZonesCanvas = Canvas.Convert.fromString twoZonesStringCanvas

    // act
    let sut = getValue twoZonesCanvas |> Grid.create
    
    // assert
    sut.Cells.Length |> should equal 2
    sut.Cells.[0, *].Length |> should equal 2
    
    sut.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    sut.Cells.[0, 1].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 1].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[0, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }

[<Fact>]  
let ``Given a canvas with two zones part of the maze side by side vertically, when creating a grid, then the grid should contain two cells with normal wall in the middle`` () =

    // arrange
    let twoZonesStringCanvas =
        Canvas.Convert.startLineTag +
        "*\n" +
        "*\n" +
        Canvas.Convert.endLineTag

    let twoZonesCanvas = Canvas.Convert.fromString twoZonesStringCanvas

    // act
    let sut = getValue twoZonesCanvas |> Grid.create

    // assert
    sut.Cells.Length |> should equal 2
    sut.Cells.[*, 0].Length |> should equal 2
    
    sut.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    sut.Cells.[1, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[1, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[1, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
[<Fact>]  
let ``Given a 3x3 canvas, when creating a grid, then it should have 3x3 cells with border walls on the edge and normal walls inside`` () =

    // arrange
    let threeByThreeStringCanvas =
        Canvas.Convert.startLineTag +
        "***\n" +
        "***\n" +
        "***\n" +
        Canvas.Convert.endLineTag

    let threeByThreeCanvas = Canvas.Convert.fromString threeByThreeStringCanvas

    // act
    let sut = getValue threeByThreeCanvas |> Grid.create

    // assert
    sut.Cells.Length |> should equal 9
    sut.Cells.[0, *].Length |> should equal 3
    sut.Cells.[1, *].Length |> should equal 3
    sut.Cells.[2, *].Length |> should equal 3
    sut.Cells.[*, 0].Length |> should equal 3
    sut.Cells.[*, 1].Length |> should equal 3
    sut.Cells.[*, 2].Length |> should equal 3
    
    // 1
    sut.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 2
    sut.Cells.[1, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[1, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[1, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 3
    sut.Cells.[2, 0].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[2, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[2, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[2, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }
    
    // 4
    sut.Cells.[0, 1].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[0, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 5
    sut.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 6
    sut.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[2, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal  }
    sut.Cells.[2, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[2, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 7
    sut.Cells.[0, 2].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    sut.Cells.[0, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[0, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[0, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 8
    sut.Cells.[1, 2].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[1, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[1, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal  }
    sut.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
    // 9
    sut.Cells.[2, 2].WallTop |> should equal { WallPosition = Top; WallType = Normal  }
    sut.Cells.[2, 2].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    sut.Cells.[2, 2].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    sut.Cells.[2, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal  }
    
[<Fact>]  
let ``Given a grid, when updating a wall, then the neighbors walls should also be updated`` () =

    // arrange
    let threeByThreeStringCanvas =
        Canvas.Convert.startLineTag +
        "***\n" +
        "***\n" +
        "***\n" +
        Canvas.Convert.endLineTag

    let threeByThreeCanvas = Canvas.Convert.fromString threeByThreeStringCanvas
    let grid = getValue threeByThreeCanvas |> Grid.create

    let coordinate11 = { RowIndex = 1; ColumnIndex = 1 }

    // act + assert
    
    // act top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    
    Grid.updateWallAtPosition grid coordinate11 Top Empty
    
    // assert top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    
    //
    
    // act right    
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    
    Grid.updateWallAtPosition grid coordinate11 Right Empty
    
    // assert right
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Empty }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    
    //
    
    // act bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    
    Grid.updateWallAtPosition grid coordinate11 Bottom Empty
    
    // assert bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    
    //
    
    // act left    
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    
    Grid.updateWallAtPosition grid coordinate11 Left Empty
    
    // assert left
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Empty }