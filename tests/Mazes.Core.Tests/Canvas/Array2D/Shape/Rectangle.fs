// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Canvas.Array2D.Shape.Rectangle

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.Array2D.Shape

[<Fact>]
let ``Given a number of rows and columns, when creating a rectangle, then it should give a rectangular canvas`` () =

    // arrange
    let numberOfRows = 3
    let numberOfColumns = 5

    // act
    let sut = Rectangle.create numberOfRows numberOfColumns

    // assert
    let expectedCanvas =
        let stringRepresentationCanvas =
            Convert.startLineTag + "\n" +
            "*****\n" +
            "*****\n" +
            "*****\n" +
            Convert.endLineTag

        (Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas