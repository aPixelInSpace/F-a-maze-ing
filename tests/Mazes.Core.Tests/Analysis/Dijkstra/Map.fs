// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Analysis.Dijkstra.Map

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate

// fixture
let maze =    
    let stringCanvas =
        Convert.startLineTag + "\n" +
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
        Convert.endLineTag

    let grid =
        (Convert.fromString stringCanvas).Value    
        |> OrthoGrid.CreateFunction

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

let grid5x5 =
    let stringCanvas =
        Convert.startLineTag + "\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +    
        Convert.endLineTag

    (Convert.fromString stringCanvas).Value
    |> OrthoGrid.CreateFunction

[<Fact>]
let ``Given a root inside the maze, when creating a map, then it should give all the count of the connected nodes`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal 79

[<Fact>]
let ``Given a root outside the maze, when creating a map, then the root is the only node of the map`` () =

    // arrange
    let outsideOfTheMazeNode = { RIndex = 0; CIndex = 1 }

    // act
    let map = maze.createMap outsideOfTheMazeNode

    // assert
    let hasNode = map.ShortestPathGraph.ContainsNode
    map.ConnectedNodes |> should equal 1

    hasNode outsideOfTheMazeNode |> should equal true
    map.ShortestPathGraph.NodeDistanceFromRoot outsideOfTheMazeNode |> should equal (Some 0)

    let topLeftNode = { RIndex = 0; CIndex = 9 }
    hasNode topLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot topLeftNode |> should equal None

    let bottomLeftNode = { RIndex = 9; CIndex = 0 }
    hasNode bottomLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomLeftNode |> should equal None

    let bottomRightNode = { RIndex = 9; CIndex = 8 }
    hasNode bottomRightNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomRightNode |> should equal None

    let centerNode = { RIndex = 5; CIndex = 4 }
    hasNode centerNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot centerNode |> should equal None

[<Fact>]
let ``Given a maze with no internal walls, when creating a map and getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let simpleCanvas =
        Convert.startLineTag + "\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        Convert.endLineTag

    let maze =
        (Convert.fromString simpleCanvas).Value
        |> OrthoGrid.Create
        |> Maze.createEmpty

    // act
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // assert
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0)->1->(0;1) (0;0)->1->(1;0) (0;1)->2->(0;2) (0;1)->2->(1;1) (0;2)->3->(0;3)\n" +
        "(0;2)->3->(1;2) (0;3)->4->(0;4) (0;3)->4->(1;3) (0;4)->5->(1;4) (1;0)->2->(1;1)\n" +
        "(1;0)->2->(2;0) (1;1)->3->(1;2) (1;1)->3->(2;1) (1;2)->4->(1;3) (1;2)->4->(2;2)\n" +
        "(1;3)->5->(1;4) (1;3)->5->(2;3) (1;4)->6->(2;4) (2;0)->3->(2;1) (2;0)->3->(3;0)\n" +
        "(2;1)->4->(2;2) (2;1)->4->(3;1) (2;2)->5->(2;3) (2;2)->5->(3;2) (2;3)->6->(2;4)\n" +
        "(2;3)->6->(3;3) (2;4)->7->(3;4) (3;0)->4->(3;1) (3;1)->5->(3;2) (3;2)->6->(3;3)\n" +
        "(3;3)->7->(3;4) "

    graph |> should equal expectedGraph

[<Fact>]
let ``Given a maze, when creating a map and getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate
    let mapNotUsingAPriorityQueue = Mazes.Core.Analysis.Dijkstra.Map.create maze.Grid.LinkedNeighbors maze.Grid.CostOfCoordinate Tracker.SimpleTracker.createEmpty rootCoordinate

    // assert
    let outsideOfTheMazeNode = { RIndex = 0; CIndex = 1 }

    map.ShortestPathGraph.ContainsNode outsideOfTheMazeNode |> should equal false
    mapNotUsingAPriorityQueue.ShortestPathGraph.ContainsNode outsideOfTheMazeNode |> should equal false
    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    let graphNotUsingAPriorityQueue = mapNotUsingAPriorityQueue.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0)->1->(1;0) (0;2)->7->(0;3) (0;3)->8->(0;4) (0;4)->9->(0;5) (0;5)->10->(0;6)\n" +
        "(0;5)->10->(1;5) (0;6)->11->(0;7) (0;6)->11->(1;6) (0;7)->12->(0;8) (0;8)->13->(0;9)\n" +
        "(1;0)->2->(2;0) (1;2)->6->(0;2) (1;2)->6->(1;3) (1;3)->7->(2;3) (1;6)->12->(1;7)\n" +
        "(1;6)->12->(2;6) (1;7)->13->(2;7) (2;0)->3->(2;1) (2;1)->4->(2;2) (2;2)->5->(1;2)\n" +
        "(2;3)->8->(2;4) (2;3)->8->(3;3) (2;4)->9->(2;5) (2;4)->9->(3;4) (2;5)->10->(1;5)\n" +
        "(2;7)->14->(2;8) (2;8)->15->(2;9) (2;8)->15->(3;8) (2;9)->16->(3;9) (3;3)->9->(4;3)\n" +
        "(3;4)->10->(3;5) (3;4)->10->(4;4) (3;5)->11->(3;6) (3;6)->12->(3;7) (3;6)->12->(4;6)\n" +
        "(3;8)->16->(4;8) (3;9)->17->(4;9) (4;0)->17->(4;1) (4;1)->18->(5;1) (4;3)->10->(5;3)\n" +
        "(4;6)->13->(4;5) (4;6)->13->(5;6) (4;9)->18->(5;9) (5;0)->16->(4;0) (5;3)->11->(6;3)\n" +
        "(5;4)->16->(6;4) (5;5)->15->(5;4) (5;6)->14->(5;5) (5;6)->14->(6;6) (5;9)->19->(6;9)\n" +
        "(6;0)->15->(5;0) (6;0)->15->(7;0) (6;1)->14->(6;0) (6;1)->14->(7;1) (6;2)->13->(6;1)\n" +
        "(6;3)->12->(6;2) (6;3)->12->(7;3) (6;4)->17->(6;5) (6;6)->15->(6;7) (6;7)->16->(7;7)\n" +
        "(7;0)->16->(8;0) (7;3)->13->(7;2) (7;3)->13->(7;4) (7;4)->14->(7;5) (7;5)->15->(7;6)\n" +
        "(7;5)->15->(8;5) (7;7)->17->(7;8) (7;8)->18->(7;9) (7;8)->18->(8;8) (7;9)->19->(6;9)\n" +
        "(8;0)->17->(9;0) (8;4)->17->(9;4) (8;5)->16->(8;4) (8;5)->16->(8;6) (8;6)->17->(8;7)\n" +
        "(8;8)->19->(9;8) (9;3)->19->(9;2) (9;4)->18->(9;3) (9;4)->18->(9;5) (9;5)->19->(9;6)\n"

    graph |> should equal expectedGraph
    graphNotUsingAPriorityQueue |> should equal expectedGraph

[<Fact>]
let ``Given a root inside the maze, when creating a map, then it should give all the dead ends (leaves) of the maze`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert
    let expectedDeadEnds =
            [| { RIndex = 0; CIndex = 0 }; { RIndex = 4; CIndex = 4 }; { RIndex = 2; CIndex = 6 }; { RIndex = 3; CIndex = 7 }
               { RIndex = 0; CIndex = 9 }; { RIndex = 4; CIndex = 5 }; { RIndex = 7; CIndex = 2 }; { RIndex = 7; CIndex = 1 }
               { RIndex = 7; CIndex = 6 }; { RIndex = 4; CIndex = 8 }; { RIndex = 6; CIndex = 5 }; { RIndex = 8; CIndex = 7 }
               { RIndex = 9; CIndex = 0 }; { RIndex = 5; CIndex = 1 }; { RIndex = 9; CIndex = 8 }; { RIndex = 9; CIndex = 6 }
               { RIndex = 9; CIndex = 2 } |]

    map.Leaves.Length |> should equal expectedDeadEnds.Length
    (map.Leaves |> Array.sort) |> should equal (expectedDeadEnds |> Array.sort)

[<Fact>]
let ``Given a map and a goal coordinate, when searching the shortest path between the root and the goal, then it should return the list of coordinates that forms that path`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone
    let map = maze.createMap rootCoordinate

    // act
    let path = map.ShortestPathGraph.PathFromRootTo { RIndex = 9; CIndex = 8 }

    // assert
    let expectedPath =
            [| { RIndex = 0; CIndex = 0 }; { RIndex = 1; CIndex = 0 }; { RIndex = 2; CIndex = 0 }; { RIndex = 2; CIndex = 1 }
               { RIndex = 2; CIndex = 2 }; { RIndex = 1; CIndex = 2 }; { RIndex = 1; CIndex = 3 }; { RIndex = 2; CIndex = 3 }
               { RIndex = 2; CIndex = 4 }; { RIndex = 3; CIndex = 4 }; { RIndex = 3; CIndex = 5 }; { RIndex = 3; CIndex = 6 }
               { RIndex = 4; CIndex = 6 }; { RIndex = 5; CIndex = 6 }; { RIndex = 6; CIndex = 6 }; { RIndex = 6; CIndex = 7 }
               { RIndex = 7; CIndex = 7 }; { RIndex = 7; CIndex = 8 }; { RIndex = 8; CIndex = 8 }; { RIndex = 9; CIndex = 8 } |]

    path |> Seq.toArray |> should equal expectedPath


[<Fact>]
let ``Given a grid with a hole, when getting the farthest coordinates, then it should return the information of the farthest coordinates from the root`` () =

    // arrange
    let simpleCanvas =
        Convert.startLineTag + "\n" +
        "***\n" +
        "*.*\n" +
        "***\n" +
        Convert.endLineTag

    let maze =
        (Convert.fromString simpleCanvas).Value    
        |> OrthoGrid.Create
        |> Maze.createEmpty

    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal 8
    map.FarthestFromRoot.Distance |> should equal 4
    
    map.ShortestPathGraph.NodeDistanceFromRoot { RIndex = 0; CIndex = 0 } |> should equal (Some 0)
    map.ShortestPathGraph.NodeDistanceFromRoot { RIndex = 2; CIndex = 0 } |> should equal (Some 2)
    map.ShortestPathGraph.NodeDistanceFromRoot { RIndex = 0; CIndex = 2 } |> should equal (Some 2)

[<Fact>]
let ``Given a map, when getting the farthest coordinates, then it should return the infos of the farthest coordinates from the root`` () =

    // arrange
    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert
    map.FarthestFromRoot.Distance |> should equal 19    
    
    let expectedFarthestCoordinates = [| { RIndex = 6; CIndex = 9 }; { RIndex = 9; CIndex = 2 }; { RIndex = 9; CIndex = 6 }; { RIndex = 9; CIndex = 8 } |]

    map.FarthestFromRoot.Coordinates
    |> Array.sortBy(fun c -> c.RIndex, c.CIndex)
    |> should equal expectedFarthestCoordinates

[<Fact>]
let ``Given a map, when getting the longest paths in the map, then it should return the coordinates that forms the longest paths`` () =

    // arrange
    let maze =
        grid5x5
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

    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone
    let map = maze.createMap rootCoordinate

    // act
    let longestPaths = map.LongestPaths

    // assert
    longestPaths
    |> Seq.length
    |> should equal 1

    let longestPath = longestPaths |> Seq.head
    longestPath |> Seq.length |> should equal 13

    let expectedLongestPath =
        [| { RIndex = 4; CIndex = 4 } ; { RIndex = 3; CIndex = 4 } ; { RIndex = 2; CIndex = 4 } ; { RIndex = 2; CIndex = 3 }
           { RIndex = 1; CIndex = 3 } ; { RIndex = 1; CIndex = 2 } ; { RIndex = 1; CIndex = 1 } ; { RIndex = 1; CIndex = 0 }
           { RIndex = 2; CIndex = 0 } ; { RIndex = 3; CIndex = 0 } ; { RIndex = 4; CIndex = 0 } ; { RIndex = 4; CIndex = 1 }
           { RIndex = 4; CIndex = 2 } |]

    longestPath |> Seq.toArray |> should equal expectedLongestPath

[<Fact>]
let ``Given a maze with a non adjacent neighbor, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let maze =
        grid5x5
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

    maze.Grid.AddUpdateNonAdjacentNeighbor { RIndex = 0; CIndex = 0 } { RIndex = 3; CIndex = 1 } Empty

    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0)->1->(0;1) (0;0)->1->(3;1) (0;1)->2->(0;2) (0;2)->3->(0;3) (0;2)->3->(1;2)\n" +
        "(0;3)->4->(0;4) (0;4)->5->(1;4) (1;0)->5->(1;1) (1;2)->4->(1;1) (1;2)->4->(1;3)\n" +
        "(1;2)->4->(2;2) (1;3)->5->(2;3) (2;0)->4->(1;0) (2;0)->4->(3;0) (2;1)->3->(2;0)\n" +
        "(2;2)->5->(3;2) (2;3)->6->(2;4) (2;3)->6->(3;3) (2;4)->7->(3;4) (3;0)->5->(4;0)\n" +
        "(3;1)->2->(2;1) (3;3)->7->(4;3) (3;4)->8->(4;4) (4;0)->6->(4;1) (4;1)->7->(4;2)\n"

    graph |> should equal expectedGraph

[<Fact>]
let ``Given a maze with an obstacle, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let maze =
        grid5x5
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

    maze.Grid.AddCostForCoordinate 50 { RIndex = 1; CIndex = 2 }

    let rootCoordinate = maze.Grid.GetFirstPartOfMazeZone

    // act
    let map = maze.createMap rootCoordinate

    // assert    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0)->1->(0;1) (0;1)->2->(0;2) (0;2)->3->(0;3) (0;2)->3->(1;2) (0;3)->4->(0;4)\n" +
        "(0;4)->5->(1;4) (1;0)->56->(2;0) (1;1)->55->(1;0) (1;2)->54->(1;1) (1;2)->54->(1;3)\n" +
        "(1;2)->54->(2;2) (1;3)->55->(2;3) (2;0)->57->(2;1) (2;0)->57->(3;0) (2;1)->58->(3;1)\n" +
        "(2;2)->55->(3;2) (2;3)->56->(2;4) (2;3)->56->(3;3) (2;4)->57->(3;4) (3;0)->58->(4;0)\n" +
        "(3;3)->57->(4;3) (3;4)->58->(4;4) (4;0)->59->(4;1) (4;1)->60->(4;2) "

    graph |> should equal expectedGraph