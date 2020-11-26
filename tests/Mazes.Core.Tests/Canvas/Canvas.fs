// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

module Mazes.Core.Tests.Canvas.Canvas

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Tests.Helpers
open Mazes.Core.Canvas
open Mazes.Core.Array2D

// fixture
let stringFixtureCanvas =
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

[<Fact>]
let ``Given a string representation of a canvas, when converting it to a canvas, then it should give a correct canvas`` () =

    // act
    let sut = Canvas.Convert.fromString stringFixtureCanvas

    // assert
    sut.IsSome |> should be True

    let canvas = getValue sut

    canvas.NumberOfRows |> should equal 10
    canvas.NumberOfColumns |> should equal 10
    (get canvas.Zones { RowIndex = 0; ColumnIndex = 0 }).IsAPartOfMaze |> should equal true
    (get canvas.Zones { RowIndex = 3; ColumnIndex = 2 }).IsAPartOfMaze |> should equal false

[<Fact>]
let ``Given a wrong string representation of a canvas, when converting it to a canvas, then it should return no canvas`` () =

    // arrange
    let wrongStringCanvas = "foo"

    // act
    let sut = Canvas.Convert.fromString wrongStringCanvas

    // assert
    sut.IsNone |> should be True

[<Fact>]
let ``Given an empty string representation of a canvas, when converting it to a canvas, then it should return an empty canvas`` () =

    // arrange
    let emptyStringCanvas =
        Canvas.Convert.startLineTag +    
        Canvas.Convert.endLineTag

    // act
    let sut = Canvas.Convert.fromString emptyStringCanvas

    // assert
    sut.IsSome |> should be True
    
    let canvas = getValue sut

    canvas.NumberOfRows |> should equal 0
    canvas.NumberOfColumns |> should equal 0

[<Fact>]
let ``Given a canvas, when converting it to a string, then it should give a correct string representation`` () =

    // arrange
    let canvas = Canvas.Convert.fromString stringFixtureCanvas

    // act
    let sut = Canvas.Convert.toString (getValue canvas)

    // assert
    sut |> should equal stringFixtureCanvas

[<Fact>]
let ``Given a canvas, when counting the number of zones that are part of the maze, then it should give the correct count of zones of the maze`` () =

    // arrange
    let canvas = Canvas.Convert.fromString stringFixtureCanvas

    // act
    let sut = (getValue canvas).TotalOfMazeZones

    // assert
    sut |> should equal 79