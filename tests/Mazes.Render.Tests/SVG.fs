// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Xunit
open Mazes.Core.Canvas
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.Array2D.Hex
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a maze (with an ortho grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
        |> OrthoGrid.CreateFunction

    let maze =
        grid
        |> RecursiveBacktracker.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.OrthoGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/ortho.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze (with a polar grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Disk.create 5 1.0 2
        |> PolarGrid.createGridFunction

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.PolarGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/theta.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze (with a hex grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Hexagon.create 5.0
        |> HexGrid.CreateFunction

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.HexGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/sigma.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze