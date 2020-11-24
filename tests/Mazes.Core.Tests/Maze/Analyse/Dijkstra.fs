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

let canvas10By10 =
    let savedCanvas =
        "Type=Canvas\n" +
        "*-********\n" +
        "*-**-***--\n" +
        "**********\n" +
        "---*******\n" +
        "**-****-**\n" +
        "**-****--*\n" +
        "********-*\n" +
        "**********\n" +
        "*---*****-\n" +
        "*-*****-*-\n" +    
        "end"
    
    match Canvas.load savedCanvas with
    | Some canvas -> canvas
    | None -> raise(Exception "The saved canvas is not correct")

[<Fact>]
let ``Creating a map with root (0,0) on a maze generated with the sidewinder algorithm (Top, Right, rng 1) on the canvas 10x10 should contain the following zones and distances`` () =
    // arrange
    let maze =
        canvas10By10
        |> Grid.create
        |> Sidewinder.createMaze Top Right (Random(1)) 1 1

    (*
        the maze looks like this
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

    // act
    let map = maze |> Dijkstra.createMap { RowIndex = 0; ColumnIndex = 0  }

    // assert
    let get = get map.DistancesFromRoot
    map.ZonesAccessibleFromRoot |> should equal 79

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