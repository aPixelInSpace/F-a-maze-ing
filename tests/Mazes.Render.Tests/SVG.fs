// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Mazes.Core.Canvas
open Xunit
open Mazes.Core.Grid.Ortho.Canvas
open Mazes.Core.Grid.Ortho
open Mazes.Core.Grid.Polar
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a maze (with an ortho grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
        |> OrthoGrid.createGridFunction

    let maze =
        grid
        |> RecursiveBacktracker.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let renderedMaze = SVG.OrthoGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetFirstBottomRightPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/Orthogrid.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze (with a polar grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let canvas = Canvas.Shape.Disc.create 10 1.0 5
    //canvas.Zones.[1].[0] <- Zone.Empty
    //canvas.Zones.[1].[1] <- Zone.Empty
    //canvas.Zones.[2].[0] <- Zone.Empty
    //canvas.Zones.[3].[0] <- Zone.Empty

    let grid =
        canvas
        |> PolarGrid.createGridFunction

    //grid.LinkCells { RIndex = 1; CIndex = 0 } { RIndex = 2; CIndex = 0 }
    
    //let neighborsLinked = grid.NeighborsThatAreLinked false { RIndex = 2; CIndex = 1 }
    //neighborsLinked |> Seq.isEmpty |> should equal false

    let maze =
        grid
        |> RecursiveBacktracker.createMaze 1
    //let map = maze.createMap maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let renderedMaze = SVG.PolarGrid.render maze.Grid.ToSpecializedGrid
        
    // assert
    //
    //let expectedRenderedMaze = IO.File.ReadAllText("Resources/Polargrid.svg", Encoding.UTF8)
        
    //renderedMaze |> should equal expectedRenderedMaze

    ()