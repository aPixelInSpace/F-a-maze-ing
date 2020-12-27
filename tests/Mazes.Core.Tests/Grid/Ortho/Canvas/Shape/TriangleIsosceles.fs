// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.Ortho.Canvas.Shape.TriangleIsosceles

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Grid.Ortho
open Mazes.Core.Grid.Ortho.Canvas.Shape

[<Fact>]
let ``Given a base length of 9, a base at bottom, a base decrement of 1 and a height increment of 1, when creating a triangle isosceles, then it should give a triangle canvas that is like the representation`` () =

    // arrange
    let baseLength = 9
    let baseAt = TriangleIsosceles.Bottom
    let baseDecrement = 1
    let heightIncrement = 1

    // act
    let sut = TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Canvas.Convert.startLineTag + "\n" +
            "....*....\n" +
            "...***...\n" +
            "..*****..\n" +
            ".*******.\n" +
            "*********\n" +
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas

[<Fact>]
let ``Given a base length of 7, a base at top, a base decrement of 2 and a height increment of 1, when creating a triangle isosceles, then it should give a triangle canvas that is like the representation`` () =

    // arrange
    let baseLength = 7
    let baseAt = TriangleIsosceles.Top
    let baseDecrement = 2
    let heightIncrement = 1

    // act
    let sut = TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Canvas.Convert.startLineTag + "\n" +
            "*******\n" +
            "..***..\n" +
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas

[<Fact>]
let ``Given a base length of 5, a base at left, a base decrement of 1 and a height increment of 2, when creating a triangle isosceles, then it should give a triangle canvas that is like the representation`` () =

    // arrange
    let baseLength = 5
    let baseAt = TriangleIsosceles.Left
    let baseDecrement = 1
    let heightIncrement = 2

    // act
    let sut = TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Canvas.Convert.startLineTag + "\n" +
            "**....\n" +
            "****..\n" +
            "******\n" +
            "****..\n" +
            "**....\n" +
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas

[<Fact>]
let ``Given a base length of 10, a base at right, a base decrement of 2 and a height increment of 2, when creating a triangle isosceles, then it should give a triangle canvas that is like the representation`` () =

    // arrange
    let baseLength = 10
    let baseAt = TriangleIsosceles.Right
    let baseDecrement = 2
    let heightIncrement = 2

    // act
    let sut = TriangleIsosceles.create baseLength baseAt baseDecrement heightIncrement

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Canvas.Convert.startLineTag + "\n" +
            "....**\n" +
            "....**\n" +
            "..****\n" +
            "..****\n" +
            "******\n" +
            "******\n" +
            "..****\n" +
            "..****\n" +
            "....**\n" +
            "....**\n" +
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas