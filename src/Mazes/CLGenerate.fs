// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLGenerate

open System
open System.Diagnostics
open System.IO
open System.Text
open CommandLine
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate
open Mazes.Render
open Mazes.Render.SVG

type AlgoEnum =
    | BinaryTree = 0
    | BT = 0
    | Sidewinder = 1
    | SW = 1
    | AldousBroder = 2
    | AB = 2
    | Wilson = 3
    | WS = 3
    | HuntAndKill = 4
    | HK = 4
    | RecursiveBacktracker = 5
    | RB = 5
    | Kruskal = 6
    | KR = 6
    | PrimSimple = 7
    | PS = 7
    | PrimSimpleModified = 8
    | PSM = 8
    | PrimWeighted = 9
    | PW = 9
    | GrowingTreeMixRandomAndLast = 10
    | GTMRL = 10
    | GrowingTreeMixChosenRandomAndLast = 11
    | GTMCRL = 11
    | GrowingTreeMixOldestAndLast = 12
    | GTMOL = 12
    | GrowingTreeDirection = 13
    | GTD = 13
    | GrowingTreeSpiral = 14
    | GTS = 14
    | Eller = 15
    | EL = 15
    | RecursiveDivision = 16
    | RD = 16

let private defaultNameOfFile = "The F Amazing Maze"

[<Verb("generate", isDefault = true, HelpText = "Generate a new maze")>]
type GenerateOptions = {
    [<Option('n', "name", Required = false, HelpText = "A name for the maze. If empty, a default one is provided.")>] name : string option
    [<Option('r', "rows", Required = false, Default = 50, HelpText = "The number of rows of the maze." )>] rows : int
    [<Option('c', "columns", Required = false, Default = 80, HelpText = "The number of columns of the maze." )>] columns : int
    [<Option('a', "algo", Required = false, HelpText = "The algorithm to use to generate the maze. If empty, a random one is chosen." )>] algo : AlgoEnum option
    [<Option('s', "seed", Required = false, HelpText = "The seed number to use for the random number generator. If empty, a random seed is picked." )>] seed : int option
    [<Option('d', "directory", Required = false, HelpText = "The directory where to output the maze. If empty, the directory is the directory of this program." )>] directory : string option
    [<Option('q', "quiet", Required = false, Default = false, HelpText = "Automatically exit the program when finished")>] quiet : bool
}

let handleVerbGenerate (options : Parsed<GenerateOptions>) =

//    let matchAlgoEnumWithFunction algoEnum rngSeed =
//        match algoEnum with
//           | AlgoEnum.BinaryTree -> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Bottom rngSeed 1 1
//           | AlgoEnum.Sidewinder -> Sidewinder.createMaze Sidewinder.Direction.Right Sidewinder.Direction.Top rngSeed 1 1
//           | AlgoEnum.AldousBroder -> AldousBroder.createMaze rngSeed
//           | AlgoEnum.Wilson -> Wilson.createMaze rngSeed
//           | AlgoEnum.HuntAndKill -> HuntAndKill.createMaze rngSeed
//           | AlgoEnum.RecursiveBacktracker -> RecursiveBacktracker.createMaze rngSeed
//           | AlgoEnum.Kruskal -> Kruskal.createMaze rngSeed
//           | AlgoEnum.PrimSimple -> PrimSimple.createMaze rngSeed
//           | AlgoEnum.PrimSimpleModified -> PrimSimpleModified.createMaze rngSeed
//           | AlgoEnum.PrimWeighted -> PrimWeighted.createMaze rngSeed 42
//           | AlgoEnum.GrowingTreeMixRandomAndLast -> GrowingTreeMixRandomAndLast.createMaze rngSeed 0.5
//           | AlgoEnum.GrowingTreeMixChosenRandomAndLast -> GrowingTreeMixChosenRandomAndLast.createMaze rngSeed 0.5
//           | AlgoEnum.GrowingTreeMixOldestAndLast -> GrowingTreeMixOldestAndLast.createMaze rngSeed 0.5
//           | AlgoEnum.GrowingTreeDirection -> GrowingTreeDirection.createMaze rngSeed 0.3 0.2 0.3
//           | AlgoEnum.GrowingTreeSpiral -> GrowingTreeSpiral.createMaze rngSeed 1.0 1.0 10 0.0
//           | AlgoEnum.Eller -> Eller.createMaze rngSeed
//           | AlgoEnum.RecursiveDivision -> RecursiveDivision.createMaze rngSeed 0.0 3 3
//           | _ -> failwith "Generating algorithm unknown"
    
    let matchAlgoEnumWithFunction algoEnum rngSeed =
        match algoEnum with
           | AlgoEnum.AldousBroder -> AldousBroder.createMazeNew rngSeed
           | AlgoEnum.Wilson -> Wilson.createMaze rngSeed
           | AlgoEnum.HuntAndKill -> HuntAndKill.createMazeNew rngSeed
           | AlgoEnum.RecursiveBacktracker -> RecursiveBacktracker.createMaze rngSeed
           | AlgoEnum.Kruskal -> Kruskal.createMazeNew rngSeed
           | AlgoEnum.PrimSimple -> PrimSimple.createMaze rngSeed
           | AlgoEnum.PrimSimpleModified -> PrimSimpleModified.createMaze rngSeed
           | AlgoEnum.PrimWeighted -> PrimWeighted.createMaze rngSeed 42
           | AlgoEnum.GrowingTreeMixRandomAndLast -> GrowingTreeMixRandomAndLast.createMaze rngSeed 0.5
           | AlgoEnum.GrowingTreeMixChosenRandomAndLast -> GrowingTreeMixChosenRandomAndLast.createMaze rngSeed 0.5
           | AlgoEnum.GrowingTreeMixOldestAndLast -> GrowingTreeMixOldestAndLast.createMaze rngSeed 0.5
           | AlgoEnum.GrowingTreeDirection -> GrowingTreeDirection.createMaze rngSeed 0.3 0.2 0.3
           | AlgoEnum.GrowingTreeSpiral -> GrowingTreeSpiral.createMaze rngSeed 0.1 0.9 3 0.2
           | AlgoEnum.Eller -> Eller.createMaze rngSeed
           | AlgoEnum.RecursiveDivision -> RecursiveDivision.createMaze rngSeed 0.0 3 3
           | _ -> failwith "Generating algorithm unknown"

    let nameOfMaze =
        match options.Value.name with
        | Some name -> name
        | None -> defaultNameOfFile

    let directory =
        match options.Value.directory with
        | Some directory -> directory
        | None -> Directory.GetCurrentDirectory()

    let filePath = Path.Combine(directory, nameOfMaze + ".html")

    let stopWatch = Stopwatch()

    stopWatch.Start()
    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> GridNew.Ortho.create |> GridNew.Grid.create)

    let grid = Shape.Disk.create options.Value.rows 1.0 2 |> GridNew.Polar.createBaseGrid |> GridNew.Grid.create
    
    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Ortho.OrthoGrid.CreateFunction)
    //let grid = (Shape.TriangleIsosceles.create 35 Shape.TriangleIsosceles.BaseAt.Bottom 2 1 |> OrthoGrid.createGridFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> Ortho.OrthoGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 20 15 -10.0 0.0 0 8 (Some 2.5) Shape.Ellipse.Side.Outside |> OrthoGrid.createGridFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> OrthoGrid.createGridFunction)
    //let grid = (Mazes.Utility.Canvas.Convert.fromImage 0.0f "d:\\temp\\Microchip.png" |> OrthoGrid.createGridFunction)
    //let grid = Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside |> OrthoGrid.createGridFunction
    //let grid = (Shape.Hexagon.create 15.0 |> Ortho.OrthoGrid.CreateFunction)
    //let grid = (Shape.PentagonStar.create 32.0 20.0 |> Ortho.OrthoGrid.CreateFunction)

    //let grid = Shape.Disk.create options.Value.rows 1.0 2 |> PolarGrid.CreateFunction
    //let grid = Shape.Disk.create options.Value.rows 1.0 2 |> PolarGrid.CreateEmptyFunction

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.TriangleIsosceles.create 35 Shape.TriangleIsosceles.BaseAt.Bottom 2 1 |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Hexagon.create options.Value.rows |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Hexagon.create 15.0 |> Hex.HexGrid.CreateFunction)

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Tri.TriGrid.CreateFunction)
    //let grid = (Shape.TriangleIsosceles.create 53 Shape.TriangleIsosceles.BaseAt.Bottom 1 1 |> Tri.TriGrid.CreateFunction)

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> OctaSquare.OctaSquareGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 30 35 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> OctaSquare.OctaSquareGrid.CreateFunction)

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.TriangleIsosceles.create 35 Shape.TriangleIsosceles.BaseAt.Bottom 2 1 |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.Hexagon.create 15.0 |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Mazes.Utility.Canvas.Convert.fromImage 0.0f "d:\\temp\\Microchip.png" |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.Pentagon.create 25.0 |> PentaCairo.PentaCairoGrid.CreateFunction)
    //let grid = (Shape.PentagonStar.create 20.0 12.0 |> PentaCairo.PentaCairoGrid.CreateFunction)

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Ortho.OrthoGrid.CreateEmptyFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> Ortho.OrthoGrid.CreateEmptyFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> Ortho.OrthoGrid.CreateEmptyFunction)
    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Hex.HexGrid.CreateEmptyFunction)
    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> PentaCairo.PentaCairoGrid.CreateEmptyFunction)

    stopWatch.Stop()
    printfn $"Created grid ({stopWatch.ElapsedMilliseconds} ms)"

    //let canvasSave = (Shape.Rectangle.create 15 15 |> Canvas.save)
    //File.WriteAllText(filePath.Replace(".html", ".canvas.mazes"), canvasSave, Encoding.UTF8)
    //let save = File.ReadAllText(filePath.Replace(".html", ".canvas.mazes"))     
    //let canvas =
    //   match Canvas.Convert.fromString save with
    //    | Some canvas -> canvas
    //    | None -> failwith "A problem occured while loading the saved canvas"
    //let grid = (canvas |> OrthoGrid.createGridFunction)
    
    let algo =
        match options.Value.algo with
        | Some algo -> matchAlgoEnumWithFunction algo
        | None ->
            let rngAlgo = Random()
            let enumAlgoUpperBound = ((AlgoEnum.GetValues(typeof<AlgoEnum>)).GetUpperBound(0)) + 1            
            matchAlgoEnumWithFunction (enum<AlgoEnum> (rngAlgo.Next(enumAlgoUpperBound)))

    let rngSeed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> 0

    //

    stopWatch.Restart()

    // Async
//    let maze1 =
//        async {
//                let gridSliced1 = GridView.sliceGrid grid { RIndex = 0; CIndex = 0 } { RIndex = 9; CIndex = 9 }
//                let maze = (algo (rngSeed) gridSliced1)
//                GridView.mergeGrid maze.Grid grid { RIndex = 0; CIndex = 0 }
//            }
//    let maze2 =
//        async {
//                let gridSliced2 = GridView.sliceGrid grid { RIndex = 0; CIndex = 10 } { RIndex = 8; CIndex = 19 }
//                let maze = (algo (rngSeed + 1) gridSliced2)
//                GridView.mergeGrid maze.Grid grid { RIndex = 0; CIndex = 10 }
//            }
//    let maze3 =
//        async {
//                let gridSliced3 = GridView.sliceGrid grid { RIndex = 10; CIndex = 0 } { RIndex = 19; CIndex = 9 }
//                let maze = (algo (rngSeed + 2) gridSliced3)
//                GridView.mergeGrid maze.Grid grid { RIndex = 10; CIndex = 0 }
//            }
//    let maze4 =
//        async {
//                let gridSliced4 = GridView.sliceGrid grid { RIndex = 9; CIndex = 10 } { RIndex = 19; CIndex = 19 }
//                let maze = (algo (rngSeed + 3) gridSliced4)
//                GridView.mergeGrid maze.Grid grid { RIndex = 9; CIndex = 10 }
//            }
//    
//    [maze1; maze2; maze3; maze4] |> Async.Parallel |> Async.RunSynchronously |> ignore
//    
//    let maze = { Grid = grid }

//    let grid =
//        let grid = grid()

//        let rng = Random(rngSeed)
//        for coordinate in grid.CoordinatesPartOfMaze do
//            let toCoordinate = 
//                if (coordinate.RIndex % 2 = 0 && coordinate.CIndex % 2 = 1) then
//                    Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }
//                elif (coordinate.RIndex % 2 = 1 && coordinate.CIndex % 2 = 0) then
//                    Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }
//                else
//                    None
//            
//            match toCoordinate with
//            | Some toCoordinate ->
//                if (toCoordinate.RIndex >= fst (grid.Dimension1Boundaries toCoordinate.CIndex)) &&
//                   (toCoordinate.RIndex < snd (grid.Dimension1Boundaries toCoordinate.CIndex)) &&
//                   (toCoordinate.CIndex >= fst (grid.Dimension2Boundaries toCoordinate.RIndex)) &&
//                   (toCoordinate.CIndex < snd (grid.Dimension2Boundaries toCoordinate.RIndex)) &&
//                   grid.IsCellPartOfMaze toCoordinate &&
//                   rng.NextDouble() < 0.5
//                   then
//                    grid.AddUpdateNonAdjacentNeighbor coordinate toCoordinate ConnectionType.Close
//            | None -> ()
        
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 6; CIndex = 6 } { RIndex = 5; CIndex = 5 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 7; CIndex = 14 } { RIndex = 9; CIndex = 14 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 8; CIndex = 5 } { RIndex = 10; CIndex = 5 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 9; CIndex = 7 } { RIndex = 10; CIndex = 8 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 5; CIndex = 22 } { RIndex = 7; CIndex = 22 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 9; CIndex = 21 } { RIndex = 11; CIndex = 21 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 6; CIndex = 6 } { RIndex = 5; CIndex = 2 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 6; CIndex = 6 } { RIndex = 7; CIndex = 5 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 19; CIndex = 0 } { RIndex = 15; CIndex = 5 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 16; CIndex = 19 } { RIndex = 18; CIndex = 19 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 18; CIndex = 19 } { RIndex = 18; CIndex = 21 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 12; CIndex = 25 } { RIndex = 17; CIndex = 27 } WallType.Normal
//        grid.AddUpdateNonAdjacentNeighbor { RIndex = 11; CIndex = 10 } { RIndex = 14; CIndex = 11 } WallType.Normal

        //(fun _ -> grid)

    let maze = (algo rngSeed grid)
    //maze.Grid.AddTwoWayTeleport { RIndex = 2; CIndex = 4 } { RIndex = 41; CIndex = 8 }
    //maze.Grid.AddCostForCoordinate 200 { RIndex = maze.Grid.GetFirstPartOfMazeZone.RIndex + 1; CIndex = maze.Grid.GetFirstPartOfMazeZone.CIndex}

    stopWatch.Stop()
    printfn $"Created maze ({stopWatch.ElapsedMilliseconds} ms)"

    //

    stopWatch.Restart()

    let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze

    stopWatch.Stop()
    printfn $"Created map ({stopWatch.ElapsedMilliseconds} ms)"

    //

    stopWatch.Restart()

    //
    
    //stopWatch.Restart()
    //
    //let maze = Maze.braid 1 0.3 map.Leaves maze
    //let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    //
    //stopWatch.Stop()
    //printfn $"Braided the maze and re-created the map ({stopWatch.ElapsedMilliseconds} ms)"

    //let renderedGrid = renderGrid  (maze.Grid.ToSpecializedGrid)

//    let htmlOutput = Mazes.Output.Html.outputHtml maze { Name = nameOfMaze } (Text.renderGrid maze.Grid.ToSpecializedGrid)
//    File.WriteAllText(filePath, htmlOutput, Encoding.UTF8)
//    
//    let rawTestOutput = Output.RawForTest.outputRawForTest maze (Text.renderGrid maze.Grid.ToSpecializedGrid)
//    File.WriteAllText(filePath.Replace(".html", ".txt"), rawTestOutput, Encoding.UTF8)

    //let renderedGridSvg = SVGNew.OrthoGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastCellPartOfMaze) map
    //let renderedGridSvg = SVG.renderGrid maze.Grid (map.Graph.PathFromRootTo { RIndex = 0; CIndex = 3 }) map
    //let renderedGridSvg = SVG.OrthoGrid.render maze.Grid.ToSpecializedGrid (map.LongestPaths |> Seq.head) map    
        
    //let renderedGridSvg = SVGNew.PolarGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastCellPartOfMaze) map
    let renderedGridSvg = SVGNew.PolarGrid.render maze.Grid.ToSpecializedGrid (map.LongestPaths |> Seq.head) map
    
    //let renderedGridSvg = SVG.HexGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map

    //let renderedGridSvg = SVG.TriGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map

    //let renderedGridSvg = SVG.OctaSquareGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map

    //let renderedGridSvg = SVG.PentaCairoGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
    //let renderedGridSvg = SVG.PentaCairoGrid.render (maze.Grid.ToSpecializedGrid) (map.LongestPaths |> Seq.head) map

    File.WriteAllText(filePath.Replace(".html", ".svg"), renderedGridSvg, Encoding.UTF8)

    stopWatch.Stop()
    printfn $"Render maze ({stopWatch.ElapsedMilliseconds} ms)"

    printfn "Mazes creation finished !"
    printfn "File location is %s" filePath

    if not options.Value.quiet then    
        printfn "Press any key to exit"
        |> Console.ReadKey
        |> ignore