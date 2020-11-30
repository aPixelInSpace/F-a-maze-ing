// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Grid.GridView

open FsUnit
open Xunit
open Mazes.Core.Canvas.Shape
open Mazes.Core.Grid


[<Fact>]
let ``Given a grid, when slicing the grid, then the sliced grid should contain a subset of the source grid`` () =

    // arrange
    let gridRectangle =
        Rectangle.create 10 10
        |> Grid.create

    // act
    let slicedGrid = GridView.sliceGrid gridRectangle { RowIndex = 1; ColumnIndex = 1 } { RowIndex = 4; ColumnIndex = 4 }

    // assert
    slicedGrid.Cells.Length |> should equal 16