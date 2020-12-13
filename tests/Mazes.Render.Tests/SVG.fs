﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a maze, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let maze =
        Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
        |> OrthoGrid.create
        |> RecursiveBacktracker.createMaze 1

    let map = maze.createDijkstraMap (snd maze.Grid.Canvas.GetFirstTopLeftPartOfMazeZone)

    // act
    let renderedMaze = SVG.renderGrid maze.Grid (map.Graph.PathFromRootTo (snd maze.Grid.Canvas.GetFirstBottomRightPartOfMazeZone)) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/Example.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze