// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Analysis.Dijkstra

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate

// fixture
let maze =    
    let stringCanvas =
        Canvas.Convert.startLineTag + "\n" +
        "*.********\n" +
        "*.**.***..\n" +
        "**********\n" +
        "...*******\n" +
        "**.****.**\n" +
        "**.****..*\n" +
        "********.*\n" +
        "**********\n" +
        "*...*****.\n" +
        "*.*****.*.\n" +    
        Canvas.Convert.endLineTag

    let grid =
        (Canvas.Convert.fromString stringCanvas).Value    
        |> OrthoGrid.create

    grid
    |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

    (*
        the above maze looks like this
        ┏━┓ ┏━━━━━━━━━━━━━━━┓
        ┃ ┃ ┃ ╶─┲━┓ ┬ ╶─┲━━━┛
        ┃ ┗━┛ ┬ ┗━┛ │ ┬ ┗━━━┓
        ┗━━━━━┪ ┬ ╶─┴─┴─╮ ┬ ┃
        ┏━━━┓ ┃ │ ╭─╴ ┏━┪ │ ┃
        ┃ ┬ ┃ ┃ ├─┴─╴ ┃ ┗━┪ ┃
        ┃ ╰─┺━┛ │ ╶─╮ ┗━┓ ┃ ┃
        ┃ ┬ ╭─╴ ╰───┴─╮ ┗━┛ ┃
        ┃ ┢━┷━━━┱─╴ ╶─┴─╮ ┏━┛
        ┃ ┃ ┏━━━┛ ╶───┲━┪ ┃  
        ┗━┛ ┗━━━━━━━━━┛ ┗━┛  
    *)

[<Fact>]
let ``Given a root inside the maze, when creating a map, then it should give all the count of the connected nodes`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal 79

[<Fact>]
let ``Given a root outside the maze, when creating a map, then the root is the only node of the map`` () =

    // act
    let map = maze.createDijkstraMap { RowIndex = 0; ColumnIndex = 1  }  

    // assert
    let node = map.Graph.Node
    map.ConnectedNodes |> should equal 1

    let outsideOfTheMazeNode = node { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeNode.IsSome |> should equal true
    outsideOfTheMazeNode.Value.DistanceFromRoot |> should equal 0

    let topLeftNode = node { RowIndex = 0; ColumnIndex = 9 }
    topLeftNode.IsNone |> should equal true

    let bottomLeftNode = node { RowIndex = 9; ColumnIndex = 0 }
    bottomLeftNode.IsNone |> should equal true

    let bottomRightNode = node { RowIndex = 9; ColumnIndex = 8 }
    bottomRightNode.IsNone |> should equal true

    let centerNode = node { RowIndex = 5; ColumnIndex = 4 }
    centerNode.IsNone |> should equal true

[<Fact>]
let ``Given a map with no internal walls, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let simpleCanvas =
        Canvas.Convert.startLineTag + "\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        Canvas.Convert.endLineTag

    let maze =
        (Canvas.Convert.fromString simpleCanvas).Value
        |> OrthoGrid.create
        |> Maze.createEmpty

    // act
    let map = maze.createDijkstraMap maze.Grid.GetFirstTopLeftPartOfMazeZone

    // assert
    let distances = map.Graph.ToString
    
    let expectedDistances =
        "(0)(1)(2)(3)(4)\n" +
        "(1)(2)(3)(4)(5)\n" +
        "(2)(3)(4)(5)(6)\n" +
        "(3)(4)(5)(6)(7)\n"

    distances |> should equal expectedDistances

[<Fact>]
let ``Given a map, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    let outsideOfTheMazeNode = map.Graph.Node { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeNode.IsNone |> should equal true
    
    let distances = map.Graph.ToString
    
    let expectedDistances =
        "(0)( )(6)(7)(8)(9)(10)(11)(12)(13)\n" +
        "(1)( )(5)(6)( )(10)(11)(12)( )( )\n" +
        "(2)(3)(4)(7)(8)(9)(12)(13)(14)(15)\n" +
        "( )( )( )(8)(9)(10)(11)(12)(15)(16)\n" +
        "(16)(17)( )(9)(10)(13)(12)( )(16)(17)\n" +
        "(15)(18)( )(10)(15)(14)(13)( )( )(18)\n" +
        "(14)(13)(12)(11)(16)(17)(14)(15)( )(19)\n" +
        "(15)(14)(13)(12)(13)(14)(15)(16)(17)(18)\n" +
        "(16)( )( )( )(16)(15)(16)(17)(18)( )\n" +
        "(17)( )(19)(18)(17)(18)(19)( )(19)( )\n"

    distances |> should equal expectedDistances

[<Fact>]
let ``Given a root inside the maze, when creating a map, then it should give all the dead ends (leaves) of the maze`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    let expectedDeadEnds =
            [| { RowIndex = 0; ColumnIndex = 0 }; { RowIndex = 4; ColumnIndex = 4 }; { RowIndex = 2; ColumnIndex = 6 }; { RowIndex = 3; ColumnIndex = 7 }
               { RowIndex = 0; ColumnIndex = 9 }; { RowIndex = 4; ColumnIndex = 5 }; { RowIndex = 7; ColumnIndex = 2 }; { RowIndex = 7; ColumnIndex = 1 }
               { RowIndex = 7; ColumnIndex = 6 }; { RowIndex = 4; ColumnIndex = 8 }; { RowIndex = 6; ColumnIndex = 5 }; { RowIndex = 8; ColumnIndex = 7 }
               { RowIndex = 9; ColumnIndex = 0 }; { RowIndex = 5; ColumnIndex = 1 }; { RowIndex = 9; ColumnIndex = 8 }; { RowIndex = 9; ColumnIndex = 6 }
               { RowIndex = 9; ColumnIndex = 2 } |]

    map.Leaves |> should equal expectedDeadEnds

[<Fact>]
let ``Given a map and a goal coordinate, when searching the shortest path between the root and the goal, then it should return the list of coordinates that forms that path`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone
    let map = maze.createDijkstraMap rootCoordinate

    // act
    let path = map.Graph.PathFromRootTo { RowIndex = 9; ColumnIndex = 8 }

    // assert
    let expectedPath =
            [| { RowIndex = 0; ColumnIndex = 0 }; { RowIndex = 1; ColumnIndex = 0 }; { RowIndex = 2; ColumnIndex = 0 }; { RowIndex = 2; ColumnIndex = 1 }
               { RowIndex = 2; ColumnIndex = 2 }; { RowIndex = 1; ColumnIndex = 2 }; { RowIndex = 1; ColumnIndex = 3 }; { RowIndex = 2; ColumnIndex = 3 }
               { RowIndex = 2; ColumnIndex = 4 }; { RowIndex = 3; ColumnIndex = 4 }; { RowIndex = 3; ColumnIndex = 5 }; { RowIndex = 3; ColumnIndex = 6 }
               { RowIndex = 4; ColumnIndex = 6 }; { RowIndex = 5; ColumnIndex = 6 }; { RowIndex = 6; ColumnIndex = 6 }; { RowIndex = 6; ColumnIndex = 7 }
               { RowIndex = 7; ColumnIndex = 7 }; { RowIndex = 7; ColumnIndex = 8 }; { RowIndex = 8; ColumnIndex = 8 }; { RowIndex = 9; ColumnIndex = 8 } |]

    path |> Seq.toArray |> should equal expectedPath


[<Fact>]
let ``Given a grid with a hole, when getting the farthest coordinates, then it should return the information of the farthest coordinates from the root`` () =

    // arrange
    let simpleCanvas =
        Canvas.Convert.startLineTag + "\n" +
        "***\n" +
        "*.*\n" +
        "***\n" +
        Canvas.Convert.endLineTag

    let maze =
        (Canvas.Convert.fromString simpleCanvas).Value    
        |> OrthoGrid.create
        |> Maze.createEmpty

    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal 8
    map.FarthestFromRoot.Distance |> should equal 4
    
    map.Graph.Nodes.[2, 0].Value.DistanceFromRoot |> should equal 2
    map.Graph.Nodes.[0, 2].Value.DistanceFromRoot |> should equal 2

[<Fact>]
let ``Given a map, when getting the farthest coordinates, then it should return the infos of the farthest coordinates from the root`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    map.FarthestFromRoot.Distance |> should equal 19    
    
    let expectedFarthestCoordinates = [| { RowIndex = 6; ColumnIndex = 9 }; { RowIndex = 9; ColumnIndex = 2 }; { RowIndex = 9; ColumnIndex = 6 }; { RowIndex = 9; ColumnIndex = 8 } |]

    map.FarthestFromRoot.Coordinates
    |> Array.sortBy(fun c -> c.RowIndex, c.ColumnIndex)
    |> should equal expectedFarthestCoordinates

[<Fact>]
let ``Given a map, when getting the longest paths in the map, then it should return the coordinates that forms the longest paths`` () =

    // arrange
    let grid =
        let stringCanvas =
            Canvas.Convert.startLineTag + "\n" +
            "*****\n" +
            "*****\n" +
            "*****\n" +
            "*****\n" +
            "*****\n" +    
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringCanvas).Value
        |> OrthoGrid.create

    let maze =
        grid
        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1

        (*
            the above maze looks like this
            ┏━━━━━━━━━┓
            ┠───╴ ╶─╮ ┃
            ┃ ╶─╮ ┬ ╰─┨
            ┃ ┬ │ │ ┬ ┃
            ┃ ╰─┴─┤ │ ┃
            ┗━━━━━┷━┷━┛
        *)

    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone
    let map = maze.createDijkstraMap rootCoordinate

    // act
    let longestPaths = map.LongestPaths

    // assert
    longestPaths
    |> Seq.length
    |> should equal 1

    let longestPath = longestPaths |> Seq.head
    longestPath |> Seq.length |> should equal 13

    let expectedLongestPath =
        [| { RowIndex = 4; ColumnIndex = 4 } ; { RowIndex = 3; ColumnIndex = 4 } ; { RowIndex = 2; ColumnIndex = 4 } ; { RowIndex = 2; ColumnIndex = 3 }
           { RowIndex = 1; ColumnIndex = 3 } ; { RowIndex = 1; ColumnIndex = 2 } ; { RowIndex = 1; ColumnIndex = 1 } ; { RowIndex = 1; ColumnIndex = 0 }
           { RowIndex = 2; ColumnIndex = 0 } ; { RowIndex = 3; ColumnIndex = 0 } ; { RowIndex = 4; ColumnIndex = 0 } ; { RowIndex = 4; ColumnIndex = 1 }
           { RowIndex = 4; ColumnIndex = 2 } |]

    longestPath |> Seq.toArray |> should equal expectedLongestPath

//[<Fact>]
//let ``Big maze`` () =
//    let maze =
//        Canvas.Shape.Rectangle.create 1000 1000
//        |> Grid.create
//        |> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right 1 1 1
//    
//    let map = maze.createDijkstraMap (snd maze.Grid.Canvas.GetFirstTopLeftPartOfMazeZone)
//
//    map.ConnectedNodes |> should equal 1000000
//
//    true |> should equal true