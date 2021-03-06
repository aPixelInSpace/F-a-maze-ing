﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Canvas.Array2D.Canvas

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D

// fixture
let stringFixtureCanvas =
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

[<Fact>]
let ``Given a string representation of a canvas, when converting it to a canvas, then it should give a correct canvas`` () =

    // act
    let sut = Convert.fromString stringFixtureCanvas

    // assert
    sut.IsSome |> should be True

    let canvas = sut.Value

    canvas.NumberOfRows |> should equal 10
    canvas.NumberOfColumns |> should equal 10
    (canvas.Zone { RIndex = 0; CIndex = 0 }).IsAPartOfMaze |> should equal true
    (canvas.Zone { RIndex = 3; CIndex = 2 }).IsAPartOfMaze |> should equal false

[<Fact>]
let ``Given a wrong string representation of a canvas, when converting it to a canvas, then it should return no canvas`` () =

    // arrange
    let wrongStringCanvas = "foo"

    // act
    let sut = Convert.fromString wrongStringCanvas

    // assert
    sut.IsNone |> should be True

[<Fact>]
let ``Given an empty string representation of a canvas, when converting it to a canvas, then it should return an empty canvas`` () =

    // arrange
    let emptyStringCanvas =
        Convert.startLineTag + "\n" +
        Convert.endLineTag

    // act
    let sut = Convert.fromString emptyStringCanvas

    // assert
    sut.IsSome |> should be True
    
    let canvas = sut.Value

    canvas.NumberOfRows |> should equal 0
    canvas.NumberOfColumns |> should equal 0

[<Fact>]
let ``Given a canvas, when converting it to a string, then it should give a correct string representation`` () =

    // arrange
    let canvas = Convert.fromString stringFixtureCanvas

    // act
    let sut = Convert.toString canvas.Value

    // assert
    sut |> should equal stringFixtureCanvas

[<Fact>]
let ``Given a canvas, when counting the number of zones that are part of the maze, then it should give the correct count of zones of the maze`` () =

    // arrange
    let canvas = Convert.fromString stringFixtureCanvas

    // act
    let sut = canvas.Value.TotalOfMazeZones

    // assert
    sut |> should equal 79