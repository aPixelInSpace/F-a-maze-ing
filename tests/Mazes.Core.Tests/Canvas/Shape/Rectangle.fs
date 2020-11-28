﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Canvas.Shape.Rectangle

open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Tests.Helpers
open Mazes.Core.Canvas.Shape

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
            Canvas.Convert.startLineTag +
            "*****\n" +
            "*****\n" +
            "*****\n" +
            Canvas.Convert.endLineTag

        getValue (Canvas.Convert.fromString stringRepresentationCanvas)

    sut |> should equal expectedCanvas