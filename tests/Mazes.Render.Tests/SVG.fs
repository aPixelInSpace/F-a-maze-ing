// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.Array2D.Hex
open Mazes.Core.Grid.Array2D.Tri
open Mazes.Core.Grid.Array2D.OctaSquare
open Mazes.Core.Grid.Array2D.PentaCairo
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a maze with an ortho grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
            |> OrthoGrid.Create
            :> IGrid<OrthoGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 6 } { RIndex = 3; CIndex = 6 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 3 } { RIndex = 4; CIndex = 4 } WallType.Normal

        (fun () -> grid)

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.OrthoGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/ortho.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a polar grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.Disk.create 5 1.0 2
            |> PolarGrid.Create
            :> IGrid<PolarGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 3 } { RIndex = 3; CIndex = 13 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 3 } { RIndex = 4; CIndex = 4 } WallType.Normal

        (fun () -> grid)

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
let ``Given a maze with a hex grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.Hexagon.create 5.0
            |> HexGrid.Create
            :> IGrid<HexGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 2 } { RIndex = 3; CIndex = 2 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 5; CIndex = 2 } { RIndex = 6; CIndex = 3 } WallType.Normal

        (fun () -> grid)

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.HexGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/sigma.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a tri grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.TriangleIsosceles.create 9 Shape.TriangleIsosceles.BaseAt.Bottom 1 1
            |> TriGrid.Create
            :> IGrid<TriGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 3 } { RIndex = 3; CIndex = 3 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 2 } { RIndex = 4; CIndex = 3 } WallType.Normal

        (fun _ -> grid)

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.TriGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/delta.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a octa-square grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.Rectangle.create 5 7
            |> OctaSquareGrid.Create
            :> IGrid<OctaSquareGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 2 } { RIndex = 3; CIndex = 2 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 3; CIndex = 3 } { RIndex = 4; CIndex = 4 } WallType.Normal

        (fun _ -> grid)

    let maze =
        grid
        |> HuntAndKill.createMaze 1
    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.OctaSquareGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/upsilon.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a Cairo pentagonal grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        let grid =
            Shape.Pentagon.create 5.0
            |> PentaCairoGrid.Create
            :> IGrid<PentaCairoGrid>

        grid.AddUpdateNonAdjacentNeighbor { RIndex = 1; CIndex = 3 } { RIndex = 3; CIndex = 3 } WallType.Normal
        grid.AddUpdateNonAdjacentNeighbor { RIndex = 5; CIndex = 3 } { RIndex = 6; CIndex = 4 } WallType.Normal

        (fun _ -> grid)

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    // act
    let renderedMaze = SVG.PentaCairoGrid.render  maze.Grid.ToSpecializedGrid (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/pentacairo.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze