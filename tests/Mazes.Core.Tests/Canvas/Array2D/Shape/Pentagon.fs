// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

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

    let t = Convert.toString sut

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
            ".***************\n" +
            ".***************\n" +
            "..**************\n" +
            "..*************.\n" +
            "..*************.\n" +
            "..*************.\n" +
            "...***********..\n" +
            "...***********..\n" +
            Convert.endLineTag

        (Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas