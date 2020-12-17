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

    // arrange
    let outsideOfTheMazeNode = { RowIndex = 0; ColumnIndex = 1 }

    // act
    let map = maze.createDijkstraMap outsideOfTheMazeNode

    // assert
    let hasNode = map.ShortestPathGraph.ContainsNode
    map.ConnectedNodes |> should equal 1

    hasNode outsideOfTheMazeNode |> should equal true
    map.ShortestPathGraph.NodeDistanceFromRoot outsideOfTheMazeNode |> should equal (Some 0)

    let topLeftNode = { RowIndex = 0; ColumnIndex = 9 }
    hasNode topLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot topLeftNode |> should equal None

    let bottomLeftNode = { RowIndex = 9; ColumnIndex = 0 }
    hasNode bottomLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomLeftNode |> should equal None

    let bottomRightNode = { RowIndex = 9; ColumnIndex = 8 }
    hasNode bottomRightNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomRightNode |> should equal None

    let centerNode = { RowIndex = 5; ColumnIndex = 4 }
    hasNode centerNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot centerNode |> should equal None

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
    let distances = map.ShortestPathGraph.ToString
    
    let expectedDistances =
        "(0;0)->0->(0;1) (0;0)->0->(1;0) (0;1)->1->(0;2) (0;1)->1->(1;1) (0;1)->1->(0;0)\n\
         (1;0)->1->(0;0) (1;0)->1->(1;1) (1;0)->1->(2;0) (0;2)->2->(0;3) (0;2)->2->(1;2)\n\
         (0;2)->2->(0;1) (1;1)->2->(0;1) (1;1)->2->(1;2) (1;1)->2->(2;1) (1;1)->2->(1;0)\n\
         (2;0)->2->(1;0) (2;0)->2->(2;1) (2;0)->2->(3;0) (0;3)->3->(0;4) (0;3)->3->(1;3)\n\
         (0;3)->3->(0;2) (1;2)->3->(0;2) (1;2)->3->(1;3) (1;2)->3->(2;2) (1;2)->3->(1;1)\n\
         (2;1)->3->(1;1) (2;1)->3->(2;2) (2;1)->3->(3;1) (2;1)->3->(2;0) (3;0)->3->(2;0)\n\
         (3;0)->3->(3;1) (0;4)->4->(1;4) (0;4)->4->(0;3) (1;3)->4->(0;3) (1;3)->4->(1;4)\n\
         (1;3)->4->(2;3) (1;3)->4->(1;2) (2;2)->4->(1;2) (2;2)->4->(2;3) (2;2)->4->(3;2)\n\
         (2;2)->4->(2;1) (3;1)->4->(2;1) (3;1)->4->(3;2) (3;1)->4->(3;0) (1;4)->5->(0;4)\n\
         (1;4)->5->(2;4) (1;4)->5->(1;3) (2;3)->5->(1;3) (2;3)->5->(2;4) (2;3)->5->(3;3)\n\
         (2;3)->5->(2;2) (3;2)->5->(2;2) (3;2)->5->(3;3) (3;2)->5->(3;1) (2;4)->6->(1;4)\n\
         (2;4)->6->(3;4) (2;4)->6->(2;3) (3;3)->6->(2;3) (3;3)->6->(3;4) (3;3)->6->(3;2)\n\
         (3;4)->7->(2;4) (3;4)->7->(3;3) "

    distances |> should equal expectedDistances

[<Fact>]
let ``Given a map, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze.createDijkstraMap rootCoordinate

    // assert
    let outsideOfTheMazeNode = { RowIndex = 0; ColumnIndex = 1 }
    map.ShortestPathGraph.ContainsNode outsideOfTheMazeNode |> should equal false
    
    let distances = map.ShortestPathGraph.ToString
    
    let expectedDistances =
        "(0;0)->0->(1;0) (1;0)->1->(0;0) (1;0)->1->(2;0) (2;0)->2->(1;0) (2;0)->2->(2;1)\n\
         (2;1)->3->(2;2) (2;1)->3->(2;0) (2;2)->4->(1;2) (2;2)->4->(2;1) (1;2)->5->(0;2)\n\
         (1;2)->5->(1;3) (1;2)->5->(2;2) (0;2)->6->(0;3) (0;2)->6->(1;2) (1;3)->6->(2;3)\n\
         (1;3)->6->(1;2) (0;3)->7->(0;4) (0;3)->7->(0;2) (2;3)->7->(1;3) (2;3)->7->(2;4)\n\
         (2;3)->7->(3;3) (0;4)->8->(0;5) (0;4)->8->(0;3) (2;4)->8->(2;5) (2;4)->8->(3;4)\n\
         (2;4)->8->(2;3) (3;3)->8->(2;3) (3;3)->8->(4;3) (0;5)->9->(0;6) (0;5)->9->(1;5)\n\
         (0;5)->9->(0;4) (2;5)->9->(1;5) (2;5)->9->(2;4) (3;4)->9->(2;4) (3;4)->9->(3;5)\n\
         (3;4)->9->(4;4) (4;3)->9->(3;3) (4;3)->9->(5;3) (0;6)->10->(0;7) (0;6)->10->(1;6)\n\
         (0;6)->10->(0;5) (1;5)->10->(0;5) (1;5)->10->(2;5) (3;5)->10->(3;6) (3;5)->10->(3;4)\n\
         (4;4)->10->(3;4) (5;3)->10->(4;3) (5;3)->10->(6;3) (0;7)->11->(0;8) (0;7)->11->(0;6)\n\
         (1;6)->11->(0;6) (1;6)->11->(1;7) (1;6)->11->(2;6) (3;6)->11->(3;7) (3;6)->11->(4;6)\n\
         (3;6)->11->(3;5) (6;3)->11->(5;3) (6;3)->11->(7;3) (6;3)->11->(6;2) (0;8)->12->(0;9)\n\
         (0;8)->12->(0;7) (1;7)->12->(2;7) (1;7)->12->(1;6) (2;6)->12->(1;6) (3;7)->12->(3;6)\n\
         (4;6)->12->(3;6) (4;6)->12->(5;6) (4;6)->12->(4;5) (7;3)->12->(6;3) (7;3)->12->(7;4)\n\
         (7;3)->12->(7;2) (6;2)->12->(6;3) (6;2)->12->(6;1) (0;9)->13->(0;8) (2;7)->13->(1;7)\n\
         (2;7)->13->(2;8) (5;6)->13->(4;6) (5;6)->13->(6;6) (5;6)->13->(5;5) (4;5)->13->(4;6)\n\
         (7;4)->13->(7;5) (7;4)->13->(7;3) (7;2)->13->(7;3) (6;1)->13->(6;2) (6;1)->13->(7;1)\n\
         (6;1)->13->(6;0) (2;8)->14->(2;9) (2;8)->14->(3;8) (2;8)->14->(2;7) (6;6)->14->(5;6)\n\
         (6;6)->14->(6;7) (5;5)->14->(5;6) (5;5)->14->(5;4) (7;5)->14->(7;6) (7;5)->14->(8;5)\n\
         (7;5)->14->(7;4) (7;1)->14->(6;1) (6;0)->14->(5;0) (6;0)->14->(6;1) (6;0)->14->(7;0)\n\
         (2;9)->15->(3;9) (2;9)->15->(2;8) (3;8)->15->(2;8) (3;8)->15->(4;8) (6;7)->15->(7;7)\n\
         (6;7)->15->(6;6) (5;4)->15->(5;5) (5;4)->15->(6;4) (7;6)->15->(7;5) (8;5)->15->(7;5)\n\
         (8;5)->15->(8;6) (8;5)->15->(8;4) (5;0)->15->(4;0) (5;0)->15->(6;0) (7;0)->15->(6;0)\n\
         (7;0)->15->(8;0) (3;9)->16->(2;9) (3;9)->16->(4;9) (4;8)->16->(3;8) (7;7)->16->(6;7)\n\
         (7;7)->16->(7;8) (6;4)->16->(5;4) (6;4)->16->(6;5) (8;6)->16->(8;7) (8;6)->16->(8;5)\n\
         (8;4)->16->(8;5) (8;4)->16->(9;4) (4;0)->16->(4;1) (4;0)->16->(5;0) (8;0)->16->(7;0)\n\
         (8;0)->16->(9;0) (4;9)->17->(3;9) (4;9)->17->(5;9) (7;8)->17->(7;9) (7;8)->17->(8;8)\n\
         (7;8)->17->(7;7) (6;5)->17->(6;4) (8;7)->17->(8;6) (9;4)->17->(8;4) (9;4)->17->(9;5)\n\
         (9;4)->17->(9;3) (4;1)->17->(5;1) (4;1)->17->(4;0) (9;0)->17->(8;0) (5;9)->18->(4;9)\n\
         (5;9)->18->(6;9) (7;9)->18->(6;9) (7;9)->18->(7;8) (8;8)->18->(7;8) (8;8)->18->(9;8)\n\
         (9;5)->18->(9;6) (9;5)->18->(9;4) (9;3)->18->(9;4) (9;3)->18->(9;2) (5;1)->18->(4;1)\n\
         (6;9)->19->(5;9) (6;9)->19->(7;9) (9;8)->19->(8;8) (9;6)->19->(9;5) (9;2)->19->(9;3)\n\
         "

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
    let path = map.ShortestPathGraph.PathFromRootTo { RowIndex = 9; ColumnIndex = 8 }

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
    
    map.ShortestPathGraph.MinAdjacentOutNode { RowIndex = 2; ColumnIndex = 0 } |> should equal (Some 2)
    map.ShortestPathGraph.MinAdjacentOutNode { RowIndex = 0; ColumnIndex = 2 } |> should equal (Some 2)

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