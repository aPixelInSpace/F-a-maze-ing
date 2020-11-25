module Mazes.Core.Tests.Canvas

open System
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Array2D

// fixture
let stringCanvas =
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

let getValue anOption =
    match anOption with
    | Some anOption -> anOption
    | None -> failwith "There should be some option"

[<Fact>]
let ``Given a string representation of a canvas when converting it to a canvas then it should give a correct canvas`` () =

    // act
    let canvas = Canvas.Convert.fromString stringCanvas

    // assert
    canvas.IsSome |> should be True

    let canvas = getValue canvas

    canvas.NumberOfRows |> should equal 10
    canvas.NumberOfColumns |> should equal 10
    (get canvas.Zones { RowIndex = 0; ColumnIndex = 0 }) |> should equal PartOfMaze
    (get canvas.Zones { RowIndex = 3; ColumnIndex = 2 }) |> should equal NotPartOfMaze

[<Fact>]
let ``Given a canvas when converting it to a string then it should give a correct string representation`` () =

    // arrange
    let canvas = Canvas.Convert.fromString stringCanvas

    // act
    let canvas = Canvas.Convert.toString (getValue canvas)

    // assert
    canvas |> should equal stringCanvas

[<Fact>]
let ``Given a canvas when counting the number of zones that are part of the maze then it should give the correct count of zones of the maze`` () =

    // arrange
    let canvas = Canvas.Convert.fromString stringCanvas

    // act
    let canvas = Canvas.countPartOfMazeZones (getValue canvas)

    // assert
    canvas |> should equal 79