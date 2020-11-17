module Mazes.Core.Tests.Grid

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.GridWall

[<Fact>]
let ``Creating a 0 by 0 grid should create a grid with an empty 2d array`` () =
    let grid = (Shape.Rectangle.create 0 0)
    
    grid.Cells.Length |> should equal 0

[<Fact>]  
let ``Creating a 1 by 1 grid should create a grid with 1 cell that has every wall as a border`` () =    
    let grid = (Shape.Rectangle.create 1 1)
        
    grid.Cells.Length |> should equal 1
    
    grid.Cells.[0, 0].WallTop |> should equal { WallPosition = Top; WallType = Border  }
    grid.Cells.[0, 0].WallRight |> should equal { WallPosition = Right; WallType = Border  }
    grid.Cells.[0, 0].WallBottom |> should equal { WallPosition = Bottom; WallType = Border  }
    grid.Cells.[0, 0].WallLeft |> should equal { WallPosition = Left; WallType = Border  }

[<Fact>]  
let ``Creating a 1 by 2 grid should create a grid with 2 horizontal cells that has border walls on the edge and the wall in the middle is normal`` () =
    let grid = (Shape.Rectangle.create 1 2)
    
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
let ``Creating a 2 by 1 grid should create a grid with 2 vertical cells that has border walls on the edge and the wall in the middle is normal`` () =
    let grid = (Shape.Rectangle.create 2 1)
    
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
let ``Creating a 3 by 3 grid should create a grid with 9 cells (3x3) that has border walls on the edge and the walls in the middle are normal`` () =
    let grid = (Shape.Rectangle.create 3 3)
    
    // | 1 | 4 | 7 | 
    // | 2 | 5 | 8 |
    // | 3 | 6 | 9 |
    
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
let ``Updating a wall should change it to the specified type : the wall of the cell itself and the wall of the corresponding neighbor`` () =
    // arrange
    let grid = (Shape.Rectangle.create 3 3)
        
    // act top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    
    grid |> updateWallAtPosition Top Empty 1 1
    
    // assert top
    grid.Cells.[1, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    grid.Cells.[0, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    
    //------------------------------------------------------------------------------//
    
    // act right    
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    
    grid |> updateWallAtPosition Right Empty 1 1
    
    // assert right
    grid.Cells.[1, 1].WallRight |> should equal { WallPosition = Right; WallType = Empty }
    grid.Cells.[1, 2].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    
    //------------------------------------------------------------------------------//
    
    // act bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Normal }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Normal }
    
    grid |> updateWallAtPosition Bottom Empty 1 1
    
    // assert bottom
    grid.Cells.[1, 1].WallBottom |> should equal { WallPosition = Bottom; WallType = Empty }
    grid.Cells.[2, 1].WallTop |> should equal { WallPosition = Top; WallType = Empty }
    
    //------------------------------------------------------------------------------//
    
    // act left    
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Normal }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Normal }
    
    grid |> updateWallAtPosition Left Empty 1 1
    
    // assert left
    grid.Cells.[1, 1].WallLeft |> should equal { WallPosition = Left; WallType = Empty }
    grid.Cells.[1, 0].WallRight |> should equal { WallPosition = Right; WallType = Empty }