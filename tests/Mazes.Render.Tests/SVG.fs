// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Tests.SVG

open System
open System.Text
open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate
open Mazes.Render
open Mazes.Render.SVG.GlobalOptions

let removeLineReturn (s : string) =
    s.Replace("\n","").Replace("\r", "")

[<Fact>]
let ``Given a maze with an ortho grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside
        |> Grid2D.Type.Ortho.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    
    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    let globalOptionsParam = { Parameters.Default with BackgroundColoration = Distance }

    // act
    let renderedMaze =
        SVG.OrthoGrid.render
            globalOptionsParam
            SVG.OrthoGrid.Parameters.CreateDefaultSquare
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/ortho.svg", Encoding.UTF8) |> removeLineReturn
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a polar grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Disk.create 9 1.0 2
        |> Grid2D.Type.Polar.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.PolarGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/theta.svg", Encoding.UTF8) |> removeLineReturn

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a hex grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Hexagon.create 5.0
        |> Grid2D.Type.Hex.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.HexGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/sigma.svg", Encoding.UTF8) |> removeLineReturn
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a tri grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.TriangleIsosceles.create 9 Shape.TriangleIsosceles.BaseAt.Bottom 1 1
        |> Grid2D.Type.Tri.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.TriGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/delta.svg", Encoding.UTF8) |> removeLineReturn
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a octa-square grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Rectangle.create 5 7
        |> Grid2D.Type.OctaSquare.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0


    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.OctaSquareGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/upsilon.svg", Encoding.UTF8) |> removeLineReturn
        
    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a Cairo pentagonal grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Pentagon.create 5.0
        |> Grid2D.Type.PentaCairo.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.PentaCairoGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/pentacairo.svg", Encoding.UTF8) |> removeLineReturn

    renderedMaze |> should equal expectedRenderedMaze

[<Fact>]
let ``Given a maze with a brick grid, a path and a map, when creating an SVG, then it should match the expected result`` () =
    // arrange
    let grid =
        Shape.Rectangle.create 5 7
        |> Grid2D.Type.Brick.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    grid.Weave (Random(1)) 1.0

    let maze =
        grid
        |> HuntAndKill.createMaze 1

    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze

    maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)

    // act
    let renderedMaze =
        SVG.BrickGrid.render
            maze.NDStruct
            (Some (map.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
            (Some map)
            (Some maze.NDStruct.GetFirstCellPartOfMaze)
            (Some maze.NDStruct.GetLastCellPartOfMaze)
        |> removeLineReturn

    // assert
    let expectedRenderedMaze = IO.File.ReadAllText("Resources/brick.svg", Encoding.UTF8) |> removeLineReturn
        
    renderedMaze |> should equal expectedRenderedMaze