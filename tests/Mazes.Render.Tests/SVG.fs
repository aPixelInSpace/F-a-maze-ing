// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Maze.Generate
open Mazes.Render

[<Fact>]
let ``Given a maze with an ortho grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
        |> Mazes.Core.Grid.Type.Ortho.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze
    
    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.OrthoGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/ortho.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a polar grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Disk.create 9 1.0 2
        |> Mazes.Core.Grid.Type.Polar.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.PolarGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/theta.svg", Encoding.UTF8)

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a hex grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Hexagon.create 5.0
        |> Mazes.Core.Grid.Type.Hex.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.HexGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/sigma.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a tri grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.TriangleIsosceles.create 9 Shape.TriangleIsosceles.BaseAt.Bottom 1 1
        |> Mazes.Core.Grid.Type.Tri.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.TriGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/delta.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a octa-square grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Rectangle.create 5 7
        |> Mazes.Core.Grid.Type.OctaSquare.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0


    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.OctaSquareGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/upsilon.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a Cairo pentagonal grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Pentagon.create 5.0
        |> Mazes.Core.Grid.Type.PentaCairo.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.PentaCairoGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/pentacairo.svg", Encoding.UTF8)

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a brick grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Rectangle.create 5 7
        |> Mazes.Core.Grid.Type.Brick.Grid.createBaseGrid
        |> Mazes.Core.Grid.NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDimensionalStructure.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDimensionalStructure.GetFirstCellPartOfMaze, maze.NDimensionalStructure.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.BrickGrid.render
            maze.NDimensionalStructure
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDimensionalStructure.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDimensionalStructure.GetFirstCellPartOfMaze)
            (Some maze.NDimensionalStructure.GetLastCellPartOfMaze)
        
    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/brick.svg", Encoding.UTF8)
        
    renderedMaze |> should equal expectedRenderedMaze