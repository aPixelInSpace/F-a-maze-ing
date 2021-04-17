// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Canvas.Array2D.Shape.Pentagon

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.Array2D.Shape

[<Fact>]
let ``Given an edge length of 10, when creating a pentagon, then it should give a pentagon canvas that is like the representation`` () =

    // arrange
    let edgeSize = 10.0

    // act
    let sut = Pentagon.create edgeSize

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Convert.startLineTag + "\n" +
            "................\n" +
            ".......***......\n" +
            "......*****.....\n" +
            "....*********...\n" +
            "...***********..\n" +
            "..*************.\n" +
            ".***************\n" +
            "..*************.\n" +
            "..*************.\n" +
            "..*************.\n" +
            "...***********..\n" +
            "...***********..\n" +
            "...***********..\n" +
            "....*********...\n" +
            "................\n" +
            Convert.endLineTag

        (Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas

[<Fact>]
let ``Given an edge length of 10 and 6, when creating a pentagon star, then it should give a pentagon star canvas that is like the representation`` () =

    // arrange
    let greatEdgeSize = 10.0
    let smallEdgeSize = 6.0

    // act
    let sut = PentagonStar.create greatEdgeSize smallEdgeSize

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Convert.startLineTag + "\n" +
            "................\n" +
            "........*.......\n" +
            ".......***......\n" +
            ".......****.....\n" +
            "......*****.....\n" +
            "..*************.\n" +
            "..*************.\n" +
            "...************.\n" +
            "...***********..\n" +
            "....*********...\n" +
            "....*********...\n" +
            "....*********...\n" +
            "....*********...\n" +
            "....***....**...\n" +
            "................\n" +
            Convert.endLineTag

        (Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas