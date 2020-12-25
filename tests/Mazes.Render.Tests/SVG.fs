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
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.OrthoGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/Orthogrid.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze (with a polar grid), a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let canvas = Canvas.Shape.Disc.create 30 1.0 7
    //canvas.Zones.[1].[0] <- Zone.Empty
    //canvas.Zones.[1].[1] <- Zone.Empty
    //canvas.Zones.[2].[0] <- Zone.Empty
    //canvas.Zones.[3].[0] <- Zone.Empty
    //let head :: tail = canvas.Zones.[0]

//    for i in [5; 15] do
//        for c in 1 .. canvas.Zones.[i].Length - 1 do
//            canvas.Zones.[i].[c] <- Zone.Empty

    let emptyByModulo ringIndexes modulo =
        for i in ringIndexes do
            let maxC = canvas.Zones.[i].Length - 1
            for c in 0 .. maxC do
                if c % modulo = 0 then
                    canvas.Zones.[i].[c] <- Zone.Empty

    emptyByModulo [15] 2
    //emptyByModulo [5; 15] 3
    //emptyByModulo [5; 15] 4
    emptyByModulo [15] 5
    //emptyByModulo [29; 20] 2
    //emptyByModulo [27] 3
    //emptyByModulo [26; 25] 4

//    for c in 0 .. canvas.Zones.[0].Length - 1 do
//        canvas.Zones.[0].[c] <- Zone.Empty
//    for c in 0 .. canvas.Zones.[1].Length - 1 do
//        canvas.Zones.[1].[c] <- Zone.Empty
//    for c in 0 .. canvas.Zones.[2].Length - 1 do
//        canvas.Zones.[2].[c] <- Zone.Empty

    let grid =
        canvas
        |> PolarGrid.createGridFunction

    let changedGrid = grid()
//    changedGrid.PutBorderBetweenCells { RIndex = 7; CIndex = 0 } { RIndex = 8; CIndex = 0 }
//    changedGrid.PutBorderBetweenCells { RIndex = 8; CIndex = 0 } { RIndex = 9; CIndex = 0 }
//    changedGrid.PutBorderBetweenCells { RIndex = 8; CIndex = 0 } { RIndex = 8; CIndex = 1 }
//    changedGrid.PutBorderBetweenCells { RIndex = 7; CIndex = 0 } { RIndex = 7; CIndex = 1 }
//    changedGrid.PutBorderBetweenCells { RIndex = 7; CIndex = 1 } { RIndex = 8; CIndex = 1 }
    
    //let neighborsLinked = grid.NeighborsThatAreLinked false { RIndex = 2; CIndex = 1 }
    //neighborsLinked |> Seq.isEmpty |> should equal false

    let maze =
        (fun () -> changedGrid)
        |> HuntAndKill.createMaze 2
    let map = maze.createMap { RIndex = 0; CIndex = 0 } //maze.Grid.GetFirstTopLeftPartOfMazeZone

    // act
    let renderedMaze = SVG.PolarGrid.render maze.Grid.ToSpecializedGrid
                           (map.LongestPaths |> Seq.head) map
                           //(map.ShortestPathGraph.PathFromRootTo { RIndex = 29; CIndex = 2 }) map
        
    // assert
    //System.IO.File.WriteAllText("\\\\192.168.1.190\\Data\\Project\\F#\\Mazes - Doc\\maze.svg", renderedMaze)
    //let expectedRenderedMaze = IO.File.ReadAllText("Resources/Polargrid.svg", Encoding.UTF8)
    //renderedMaze |> should equal expectedRenderedMaze

    ()