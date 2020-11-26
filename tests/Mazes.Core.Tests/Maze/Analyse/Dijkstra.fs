// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Analyse.Dijkstra

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas
open Mazes.Core.Grid
open Mazes.Core.Maze.Generate
open Mazes.Core.Maze.Analyse

// fixture
let maze =
    let canvas10By10 =
        let stringCanvas =
            Canvas.Convert.startLineTag +
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
    |> Sidewinder.createMaze Top Right (Random(1)) 1 1

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

    // act
    let map = maze |> Dijkstra.createMap { RowIndex = 0; ColumnIndex = 0  }  

    // assert
    let get = get map.DistancesFromRoot
    map.TotalZonesAccessibleFromRoot |> should equal 79

    let topLeftZone = get { RowIndex = 0; ColumnIndex = 9 }
    topLeftZone.DistanceFromRoot |> should equal (Some 13)

    let bottomLeftZone = get { RowIndex = 9; ColumnIndex = 0 }
    bottomLeftZone.DistanceFromRoot |> should equal (Some 17)

    let bottomRightZone = get { RowIndex = 9; ColumnIndex = 8 }
    bottomRightZone.DistanceFromRoot |> should equal (Some 19)

    let centerZone = get { RowIndex = 5; ColumnIndex = 4 }
    centerZone.DistanceFromRoot |> should equal (Some 15)

    let outsideOfTheMazeZone = get { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeZone.DistanceFromRoot |> should equal None

[<Fact>]
let ``Given a root outside the maze, when creating a map, then the root is the only zone of the map`` () =

    // act
    let map = maze |> Dijkstra.createMap { RowIndex = 0; ColumnIndex = 1  }  

    // assert
    let get = get map.DistancesFromRoot
    map.TotalZonesAccessibleFromRoot |> should equal 1

    let outsideOfTheMazeZone = get { RowIndex = 0; ColumnIndex = 1 }
    outsideOfTheMazeZone.DistanceFromRoot |> should equal (Some 0)

    let topLeftZone = get { RowIndex = 0; ColumnIndex = 9 }
    topLeftZone.DistanceFromRoot |> should equal None

    let bottomLeftZone = get { RowIndex = 9; ColumnIndex = 0 }
    bottomLeftZone.DistanceFromRoot |> should equal None

    let bottomRightZone = get { RowIndex = 9; ColumnIndex = 8 }
    bottomRightZone.DistanceFromRoot |> should equal None

    let centerZone = get { RowIndex = 5; ColumnIndex = 4 }
    centerZone.DistanceFromRoot |> should equal None