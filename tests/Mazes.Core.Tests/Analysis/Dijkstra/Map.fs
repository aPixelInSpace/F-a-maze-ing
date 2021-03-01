// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Analysis.Dijkstra.Map

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Analysis.Dijkstra
open Mazes.Core.Structure
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate

// fixture
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

let maze2D =
    (Convert.fromString stringCanvas).Value    
    |> Grid2D.Type.Ortho.Grid.createBaseGrid
    |> NDimensionalStructure.create2D
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

let newAdjStructInstance () =
    (Convert.fromString stringCanvas).Value    
    |> Grid2D.Type.Ortho.Grid.createBaseGrid
    
let maze3D =
    newAdjStructInstance
    |> NDimensionalStructure.create3D 3
    |> HuntAndKill.createMaze 1

let maze7D =
    newAdjStructInstance
    |> NDimensionalStructure.create [| 1; 1; 1; 1; 1 |]
    |> HuntAndKill.createMaze 1

let canvas5x5 =
    let stringCanvas =
        Convert.startLineTag + "\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +
        "*****\n" +    
        Convert.endLineTag

    (Convert.fromString stringCanvas).Value

[<Fact>]
let ``Given a root inside the maze 2D, when creating a map, then it should give all the count of the connected nodes`` () =

    // arrange
    let rootCoordinate2D = maze2D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map2D = maze2D.createMap rootCoordinate2D

    // assert
    map2D.ConnectedNodes |> should equal 79

[<Fact>]
let ``Given a root inside the maze 3D, when creating a map, then it should give all the count of the connected nodes`` () =

    // arrange
    let rootCoordinate3D = maze3D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map3D = maze3D.createMap rootCoordinate3D

    // assert
    map3D.ConnectedNodes |> should equal 237

[<Fact>]
let ``Given a root inside the maze 7D, when creating a map, then it should give all the count of the connected nodes`` () =

    // arrange
    let rootCoordinate7D = maze7D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map7D = maze7D.createMap rootCoordinate7D

    // assert
    map7D.ConnectedNodes |> should equal 2528

[<Fact>]
let ``Given a root outside the maze, when creating a map, then the root is the only node of the map`` () =

    // arrange
    let outsideOfTheMazeNode = NCoordinate.createFrom2D { RIndex = 0; CIndex = 1 }

    // act
    let map = maze2D.createMap outsideOfTheMazeNode

    // assert
    let hasNode = map.ShortestPathGraph.ContainsNode
    map.ConnectedNodes |> should equal 1

    hasNode outsideOfTheMazeNode |> should equal true
    map.ShortestPathGraph.NodeDistanceFromRoot outsideOfTheMazeNode |> should equal (Some 0)

    let topLeftNode = NCoordinate.createFrom2D { RIndex = 0; CIndex = 9 }
    hasNode topLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot topLeftNode |> should equal None

    let bottomLeftNode = NCoordinate.createFrom2D { RIndex = 9; CIndex = 0 }
    hasNode bottomLeftNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomLeftNode |> should equal None

    let bottomRightNode = NCoordinate.createFrom2D { RIndex = 9; CIndex = 8 }
    hasNode bottomRightNode |> should equal false
    map.ShortestPathGraph.NodeDistanceFromRoot bottomRightNode |> should equal None

    let centerNode = NCoordinate.createFrom2D { RIndex = 5; CIndex = 4 }
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
        |> Grid2D.Type.Ortho.Grid.createEmptyBaseGrid
        |> NDimensionalStructure.create2D
        |> Maze.toMaze

    // act
    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    // assert
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0;0;)->1->(0;1;0;) (0;0;0;)->1->(1;0;0;) (0;1;0;)->2->(0;2;0;) (0;1;0;)->2->(1;1;0;) (0;2;0;)->3->(0;3;0;)\n" +
        "(0;2;0;)->3->(1;2;0;) (0;3;0;)->4->(0;4;0;) (0;3;0;)->4->(1;3;0;) (0;4;0;)->5->(1;4;0;) (1;0;0;)->2->(1;1;0;)\n" +
        "(1;0;0;)->2->(2;0;0;) (1;1;0;)->3->(1;2;0;) (1;1;0;)->3->(2;1;0;) (1;2;0;)->4->(1;3;0;) (1;2;0;)->4->(2;2;0;)\n" +
        "(1;3;0;)->5->(1;4;0;) (1;3;0;)->5->(2;3;0;) (1;4;0;)->6->(2;4;0;) (2;0;0;)->3->(2;1;0;) (2;0;0;)->3->(3;0;0;)\n" +
        "(2;1;0;)->4->(2;2;0;) (2;1;0;)->4->(3;1;0;) (2;2;0;)->5->(2;3;0;) (2;2;0;)->5->(3;2;0;) (2;3;0;)->6->(2;4;0;)\n" +
        "(2;3;0;)->6->(3;3;0;) (2;4;0;)->7->(3;4;0;) (3;0;0;)->4->(3;1;0;) (3;1;0;)->5->(3;2;0;) (3;2;0;)->6->(3;3;0;)\n" +
        "(3;3;0;)->7->(3;4;0;) "

    graph |> should equal expectedGraph

[<Fact>]
let ``Given a maze, when creating a map and getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let rootCoordinate = maze2D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze2D.createMap rootCoordinate
    let mapNotUsingAPriorityQueue = Mazes.Core.Analysis.Dijkstra.Map.create (maze2D.NDStruct.ConnectedWithNeighbors true) maze2D.NDStruct.CostOfCoordinate Tracker.SimpleTracker.createEmpty rootCoordinate

    // assert
    let outsideOfTheMazeNode = NCoordinate.createFrom2D { RIndex = 0; CIndex = 1 }

    map.ShortestPathGraph.ContainsNode outsideOfTheMazeNode |> should equal false
    mapNotUsingAPriorityQueue.ShortestPathGraph.ContainsNode outsideOfTheMazeNode |> should equal false
    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    let graphNotUsingAPriorityQueue = mapNotUsingAPriorityQueue.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0;0;)->1->(1;0;0;) (0;2;0;)->7->(0;3;0;) (0;3;0;)->8->(0;4;0;) (0;4;0;)->9->(0;5;0;) (0;5;0;)->10->(0;6;0;)\n" +
        "(0;5;0;)->10->(1;5;0;) (0;6;0;)->11->(0;7;0;) (0;6;0;)->11->(1;6;0;) (0;7;0;)->12->(0;8;0;) (0;8;0;)->13->(0;9;0;)\n" +
        "(1;0;0;)->2->(2;0;0;) (1;2;0;)->6->(0;2;0;) (1;2;0;)->6->(1;3;0;) (1;3;0;)->7->(2;3;0;) (1;6;0;)->12->(1;7;0;)\n" +
        "(1;6;0;)->12->(2;6;0;) (1;7;0;)->13->(2;7;0;) (2;0;0;)->3->(2;1;0;) (2;1;0;)->4->(2;2;0;) (2;2;0;)->5->(1;2;0;)\n" +
        "(2;3;0;)->8->(2;4;0;) (2;3;0;)->8->(3;3;0;) (2;4;0;)->9->(2;5;0;) (2;4;0;)->9->(3;4;0;) (2;5;0;)->10->(1;5;0;)\n" +
        "(2;7;0;)->14->(2;8;0;) (2;8;0;)->15->(2;9;0;) (2;8;0;)->15->(3;8;0;) (2;9;0;)->16->(3;9;0;) (3;3;0;)->9->(4;3;0;)\n" +
        "(3;4;0;)->10->(3;5;0;) (3;4;0;)->10->(4;4;0;) (3;5;0;)->11->(3;6;0;) (3;6;0;)->12->(3;7;0;) (3;6;0;)->12->(4;6;0;)\n" +
        "(3;8;0;)->16->(4;8;0;) (3;9;0;)->17->(4;9;0;) (4;0;0;)->17->(4;1;0;) (4;1;0;)->18->(5;1;0;) (4;3;0;)->10->(5;3;0;)\n" +
        "(4;6;0;)->13->(4;5;0;) (4;6;0;)->13->(5;6;0;) (4;9;0;)->18->(5;9;0;) (5;0;0;)->16->(4;0;0;) (5;3;0;)->11->(6;3;0;)\n" +
        "(5;4;0;)->16->(6;4;0;) (5;5;0;)->15->(5;4;0;) (5;6;0;)->14->(5;5;0;) (5;6;0;)->14->(6;6;0;) (5;9;0;)->19->(6;9;0;)\n" +
        "(6;0;0;)->15->(5;0;0;) (6;0;0;)->15->(7;0;0;) (6;1;0;)->14->(6;0;0;) (6;1;0;)->14->(7;1;0;) (6;2;0;)->13->(6;1;0;)\n" +
        "(6;3;0;)->12->(6;2;0;) (6;3;0;)->12->(7;3;0;) (6;4;0;)->17->(6;5;0;) (6;6;0;)->15->(6;7;0;) (6;7;0;)->16->(7;7;0;)\n" +
        "(7;0;0;)->16->(8;0;0;) (7;3;0;)->13->(7;2;0;) (7;3;0;)->13->(7;4;0;) (7;4;0;)->14->(7;5;0;) (7;5;0;)->15->(7;6;0;)\n" +
        "(7;5;0;)->15->(8;5;0;) (7;7;0;)->17->(7;8;0;) (7;8;0;)->18->(7;9;0;) (7;8;0;)->18->(8;8;0;) (7;9;0;)->19->(6;9;0;)\n" +
        "(8;0;0;)->17->(9;0;0;) (8;4;0;)->17->(9;4;0;) (8;5;0;)->16->(8;4;0;) (8;5;0;)->16->(8;6;0;) (8;6;0;)->17->(8;7;0;)\n" +
        "(8;8;0;)->19->(9;8;0;) (9;3;0;)->19->(9;2;0;) (9;4;0;)->18->(9;3;0;) (9;4;0;)->18->(9;5;0;) (9;5;0;)->19->(9;6;0;)\n"

    graph |> should equal expectedGraph
    graphNotUsingAPriorityQueue |> should equal expectedGraph

[<Fact>]
let ``Given a root inside the maze, when creating a map, then it should give all the dead ends (leaves) of the maze`` () =

    // arrange
    let rootCoordinate = maze2D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze2D.createMap rootCoordinate

    // assert
    let expectedDeadEnds =
            [| { RIndex = 0; CIndex = 0 }; { RIndex = 4; CIndex = 4 }; { RIndex = 2; CIndex = 6 }; { RIndex = 3; CIndex = 7 }
               { RIndex = 0; CIndex = 9 }; { RIndex = 4; CIndex = 5 }; { RIndex = 7; CIndex = 2 }; { RIndex = 7; CIndex = 1 }
               { RIndex = 7; CIndex = 6 }; { RIndex = 4; CIndex = 8 }; { RIndex = 6; CIndex = 5 }; { RIndex = 8; CIndex = 7 }
               { RIndex = 9; CIndex = 0 }; { RIndex = 5; CIndex = 1 }; { RIndex = 9; CIndex = 8 }; { RIndex = 9; CIndex = 6 }
               { RIndex = 9; CIndex = 2 } |]
            |> Array.map(NCoordinate.createFrom2D)

    map.Leaves.Length |> should equal expectedDeadEnds.Length
    (map.Leaves |> Array.sort) |> should equal (expectedDeadEnds |> Array.sort)

[<Fact>]
let ``Given a map and a goal coordinate, when searching the shortest path between the root and the goal, then it should return the list of coordinates that forms that path`` () =

    // arrange
    let rootCoordinate = maze2D.NDStruct.GetFirstCellPartOfMaze
    let map = maze2D.createMap rootCoordinate

    // act
    let path = map.ShortestPathGraph.PathFromRootTo (NCoordinate.createFrom2D { RIndex = 9; CIndex = 8 })

    // assert
    let expectedPath =
            [| { RIndex = 0; CIndex = 0 }; { RIndex = 1; CIndex = 0 }; { RIndex = 2; CIndex = 0 }; { RIndex = 2; CIndex = 1 }
               { RIndex = 2; CIndex = 2 }; { RIndex = 1; CIndex = 2 }; { RIndex = 1; CIndex = 3 }; { RIndex = 2; CIndex = 3 }
               { RIndex = 2; CIndex = 4 }; { RIndex = 3; CIndex = 4 }; { RIndex = 3; CIndex = 5 }; { RIndex = 3; CIndex = 6 }
               { RIndex = 4; CIndex = 6 }; { RIndex = 5; CIndex = 6 }; { RIndex = 6; CIndex = 6 }; { RIndex = 6; CIndex = 7 }
               { RIndex = 7; CIndex = 7 }; { RIndex = 7; CIndex = 8 }; { RIndex = 8; CIndex = 8 }; { RIndex = 9; CIndex = 8 } |]
            |> Array.map(NCoordinate.createFrom2D)

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
        |> Grid2D.Type.Ortho.Grid.createEmptyBaseGrid
        |> NDimensionalStructure.create2D
        |> Maze.toMaze

    let rootCoordinate = maze.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze.createMap rootCoordinate

    // assert
    map.ConnectedNodes |> should equal 8
    map.FarthestFromRoot.Distance |> should equal 4
    
    map.ShortestPathGraph.NodeDistanceFromRoot (NCoordinate.createFrom2D { RIndex = 0; CIndex = 0 }) |> should equal (Some 0)
    map.ShortestPathGraph.NodeDistanceFromRoot (NCoordinate.createFrom2D { RIndex = 2; CIndex = 0 }) |> should equal (Some 2)
    map.ShortestPathGraph.NodeDistanceFromRoot (NCoordinate.createFrom2D { RIndex = 0; CIndex = 2 }) |> should equal (Some 2)

[<Fact>]
let ``Given a map, when getting the farthest coordinates, then it should return the infos of the farthest coordinates from the root`` () =

    // arrange
    let rootCoordinate = maze2D.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze2D.createMap rootCoordinate

    // assert
    map.FarthestFromRoot.Distance |> should equal 19    
    
    let expectedFarthestCoordinates = [| { RIndex = 6; CIndex = 9 }; { RIndex = 9; CIndex = 2 }; { RIndex = 9; CIndex = 6 }; { RIndex = 9; CIndex = 8 } |]

    map.FarthestFromRoot.Coordinates
    |> Array.map(fun c -> c.ToCoordinate2D)
    |> Array.sortBy(fun c -> c.RIndex, c.CIndex)
    |> should equal expectedFarthestCoordinates

[<Fact>]
let ``Given a map, when getting the longest paths in the map, then it should return the coordinates that forms the longest paths`` () =

    // arrange
    let maze =
        canvas5x5
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
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

    let rootCoordinate = maze.NDStruct.GetFirstCellPartOfMaze
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
        |> Array.map(NCoordinate.createFrom2D)

    longestPath |> Seq.toArray |> should equal expectedLongestPath

[<Fact>]
let ``Given a maze with a non adjacent neighbor, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let maze =
        canvas5x5
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
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

    maze.NDStruct.NonAdjacent2DConnections.UpdateConnection Open (NCoordinate.createFrom2D { RIndex = 0; CIndex = 0 }) (NCoordinate.createFrom2D { RIndex = 3; CIndex = 1 })

    let rootCoordinate = maze.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze.createMap rootCoordinate

    // assert    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0;0;)->1->(0;1;0;) (0;0;0;)->1->(3;1;0;) (0;1;0;)->2->(0;2;0;) (0;2;0;)->3->(0;3;0;) (0;2;0;)->3->(1;2;0;)\n" +
        "(0;3;0;)->4->(0;4;0;) (0;4;0;)->5->(1;4;0;) (1;0;0;)->5->(1;1;0;) (1;2;0;)->4->(1;1;0;) (1;2;0;)->4->(1;3;0;)\n" +
        "(1;2;0;)->4->(2;2;0;) (1;3;0;)->5->(2;3;0;) (2;0;0;)->4->(1;0;0;) (2;0;0;)->4->(3;0;0;) (2;1;0;)->3->(2;0;0;)\n" +
        "(2;2;0;)->5->(3;2;0;) (2;3;0;)->6->(2;4;0;) (2;3;0;)->6->(3;3;0;) (2;4;0;)->7->(3;4;0;) (3;0;0;)->5->(4;0;0;)\n" +
        "(3;1;0;)->2->(2;1;0;) (3;3;0;)->7->(4;3;0;) (3;4;0;)->8->(4;4;0;) (4;0;0;)->6->(4;1;0;) (4;1;0;)->7->(4;2;0;)\n"

    graph |> should equal expectedGraph

[<Fact>]
let ``Given a maze with an obstacle, when getting all the distances from the root, then it should match the expected distances`` () =

    // arrange
    let maze =
        canvas5x5
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D
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

    maze.NDStruct.Obstacles.AddUpdateCost 50 (NCoordinate.createFrom2D { RIndex = 1; CIndex = 2 })

    let rootCoordinate = maze.NDStruct.GetFirstCellPartOfMaze

    // act
    let map = maze.createMap rootCoordinate

    // assert    
    let graph = map.ShortestPathGraph.ToString (fun e -> e.Source, e.Tag, e.Target)
    
    let expectedGraph =
        "(0;0;0;)->1->(0;1;0;) (0;1;0;)->2->(0;2;0;) (0;2;0;)->3->(0;3;0;) (0;2;0;)->3->(1;2;0;) (0;3;0;)->4->(0;4;0;)\n" +
        "(0;4;0;)->5->(1;4;0;) (1;0;0;)->56->(2;0;0;) (1;1;0;)->55->(1;0;0;) (1;2;0;)->54->(1;1;0;) (1;2;0;)->54->(1;3;0;)\n" +
        "(1;2;0;)->54->(2;2;0;) (1;3;0;)->55->(2;3;0;) (2;0;0;)->57->(2;1;0;) (2;0;0;)->57->(3;0;0;) (2;1;0;)->58->(3;1;0;)\n" +
        "(2;2;0;)->55->(3;2;0;) (2;3;0;)->56->(2;4;0;) (2;3;0;)->56->(3;3;0;) (2;4;0;)->57->(3;4;0;) (3;0;0;)->58->(4;0;0;)\n" +
        "(3;3;0;)->57->(4;3;0;) (3;4;0;)->58->(4;4;0;) (4;0;0;)->59->(4;1;0;) (4;1;0;)->60->(4;2;0;) "

    graph |> should equal expectedGraph