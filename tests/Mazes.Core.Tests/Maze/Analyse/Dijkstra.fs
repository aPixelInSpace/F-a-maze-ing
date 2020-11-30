// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Analyse.Dijkstra

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate
open Mazes.Core.Maze.Analyse
open Mazes.Core.Tests.Helpers

// fixture
let maze =
    let canvas10By10 =
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

        match Canvas.Convert.fromString stringCanvas with
        | Some canvas -> canvas
        | None -> raise(Exception "The saved canvas is not correct")

    canvas10By10
    |> Grid.create
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
let ``Given a root inside the maze, when creating a map, then it should give all the distances from the root for every zone inside the maze`` () =

    // arrange
    let (_, rootCoordinate) = maze.Grid.Canvas.GetFirstTopLeftPartOfMazeZone

    // act
    let map = maze |> Dijkstra.createMap rootCoordinate

    // assert
    let zone = map.MapZone
    map.TotalZonesAccessibleFromRoot |> should equal 79

    let topLeftZone = zone { RowIndex = 0; ColumnIndex = 9 }
    topLeftZone.IsSome |> should equal true
    (getValue topLeftZone).DistanceFromRoot |> should equal 13

    let bottomLeftZone = zone { RowIndex = 9; ColumnIndex = 0 }
    bottomLeftZone.IsSome |> should equal true
    (getValue bottomLeftZone).DistanceFromRoot |> should equal 17

    let bottomRightZone = zone { RowIndex = 9; ColumnIndex = 8 }
    bottomRightZone.IsSome |> should equal true
    (getValue bottomRightZone).DistanceFromRoot |> should equal 19

    let centerZone = zone { RowIndex = 5; ColumnIndex = 4 }
    centerZone.IsSome |> should equal true
    (getValue centerZone).DistanceFromRoot |> should equal 15

    let outsideOfTheMazeZone = zone { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeZone.IsNone |> should equal true

[<Fact>]
let ``Given a root outside the maze, when creating a map, then the root is the only zone of the map`` () =

    // act
    let map = maze |> Dijkstra.createMap { RowIndex = 0; ColumnIndex = 1  }  

    // assert
    let zone = map.MapZone
    map.TotalZonesAccessibleFromRoot |> should equal 1

    let outsideOfTheMazeZone = zone { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeZone.IsSome |> should equal true
    (getValue outsideOfTheMazeZone).DistanceFromRoot |> should equal 0

    let topLeftZone = zone { RowIndex = 0; ColumnIndex = 9 }
    topLeftZone.IsNone |> should equal true

    let bottomLeftZone = zone { RowIndex = 9; ColumnIndex = 0 }
    bottomLeftZone.IsNone |> should equal true

    let bottomRightZone = zone { RowIndex = 9; ColumnIndex = 8 }
    bottomRightZone.IsNone |> should equal true

    let centerZone = zone { RowIndex = 5; ColumnIndex = 4 }
    centerZone.IsNone |> should equal true

[<Fact>]
let ``Given a root coordinate of a map and a goal coordinate, when searching the shortest path between the root and the goal, then it should return the list of coordinates that forms that path`` () =

    // arrange
    let (_, rootCoordinate) = maze.Grid.Canvas.GetFirstTopLeftPartOfMazeZone
    let map = maze |> Dijkstra.createMap rootCoordinate

    // act
    let path = map.PathFromRootTo (Some { RowIndex = 9; ColumnIndex = 8 })

    // assert
    let expectedPath =
            [ { RowIndex = 0; ColumnIndex = 0 }; { RowIndex = 1; ColumnIndex = 0 }; { RowIndex = 2; ColumnIndex = 0 }; { RowIndex = 2; ColumnIndex = 1 }
              { RowIndex = 2; ColumnIndex = 2 }; { RowIndex = 1; ColumnIndex = 2 }; { RowIndex = 1; ColumnIndex = 3 }; { RowIndex = 2; ColumnIndex = 3 }
              { RowIndex = 2; ColumnIndex = 4 }; { RowIndex = 3; ColumnIndex = 4 }; { RowIndex = 3; ColumnIndex = 5 }; { RowIndex = 3; ColumnIndex = 6 }
              { RowIndex = 4; ColumnIndex = 6 }; { RowIndex = 5; ColumnIndex = 6 }; { RowIndex = 6; ColumnIndex = 6 }; { RowIndex = 6; ColumnIndex = 7 }
              { RowIndex = 7; ColumnIndex = 7 }; { RowIndex = 7; ColumnIndex = 8 }; { RowIndex = 8; ColumnIndex = 8 }; { RowIndex = 9; ColumnIndex = 8 } ]

    path |> Seq.toList |> should equal expectedPath