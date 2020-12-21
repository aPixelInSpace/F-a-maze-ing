// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Canvas.Shape.Rectangle

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Grid.Ortho
open Mazes.Core.Grid.Ortho.Canvas.Shape

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
            Canvas.Convert.startLineTag + "\n" +
            "*****\n" +
            "*****\n" +
            "*****\n" +
            Canvas.Convert.endLineTag

        (Canvas.Convert.fromString stringRepresentationCanvas).Value

    sut |> should equal expectedCanvas