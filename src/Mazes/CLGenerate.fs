// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLGenerate

open System
open System.Diagnostics
open System.IO
open System.Text
open CommandLine
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
    | Sidewinder = 1
    | AldousBroder = 2
    | Wilson = 3
    | HuntAndKill = 4
    | RecursiveBacktracker = 5

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

    let matchAlgoEnumWithFunction algoEnum rngSeed =
        match algoEnum with
           | AlgoEnum.BinaryTree -> BinaryTree.createMaze BinaryTree.Direction.Top BinaryTree.Direction.Right rngSeed 1 1
           | AlgoEnum.Sidewinder -> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Left rngSeed 1 1
           | AlgoEnum.AldousBroder -> AldousBroder.createMaze rngSeed
           | AlgoEnum.Wilson -> Wilson.createMaze rngSeed
           | AlgoEnum.HuntAndKill -> HuntAndKill.createMaze rngSeed
           | AlgoEnum.RecursiveBacktracker -> RecursiveBacktracker.createMaze rngSeed
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
    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> OrthoGrid.createGridFunction)
    //let grid = (Shape.TriangleIsosceles.create 35 Shape.TriangleIsosceles.BaseAt.Bottom 2 1 |> OrthoGrid.createGridFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> OrthoGrid.createGridFunction)
    //let grid = (Shape.Ellipse.create 20 15 -10.0 0.0 0 8 (Some 2.5) Shape.Ellipse.Side.Outside |> OrthoGrid.createGridFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> OrthoGrid.createGridFunction)
    //let grid = (Mazes.Utility.Canvas.Convert.fromImage 0.0f "d:\\temp\\Microchip.png" |> OrthoGrid.createGridFunction)
    //let grid = Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside |> OrthoGrid.createGridFunction
    //let grid = (Shape.Hexagon.create 15.0 |> Ortho.OrthoGrid.CreateFunction)

    //let grid = Shape.Disc.create options.Value.rows 1.0 2 |> PolarGrid.createGridFunction

    //let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.TriangleIsosceles.create 35 Shape.TriangleIsosceles.BaseAt.Bottom 2 1 |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Ellipse.create 15 17 0.0 0.0 0 0 (Some 0.1) Shape.Ellipse.Side.Inside |> Hex.HexGrid.CreateFunction)
    //let grid = (Shape.Hexagon.create options.Value.rows |> Hex.HexGrid.CreateFunction)
    let grid = (Shape.Hexagon.create 5.0 |> Hex.HexGrid.CreateFunction)

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

    let maze = (algo rngSeed grid)
    //maze.Grid.AddTwoWayTeleport { RIndex = 2; CIndex = 4 } { RIndex = 41; CIndex = 8 }

    stopWatch.Stop()
    printfn $"Created maze ({stopWatch.ElapsedMilliseconds} ms)"

    //

    stopWatch.Restart()

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone

    stopWatch.Stop()
    printfn $"Created map ({stopWatch.ElapsedMilliseconds} ms)"

    //

    stopWatch.Restart()

    //let renderedGrid = renderGrid  (maze.Grid.ToSpecializedGrid)

    //let htmlOutput = outputHtml maze { Name = nameOfMaze } renderedGrid
    //File.WriteAllText(filePath, htmlOutput, Encoding.UTF8)
    
    //let rawTestOutput = Output.RawForTest.outputRawForTest maze renderedGrid
    //File.WriteAllText(filePath.Replace(".html", ".txt"), rawTestOutput, Encoding.UTF8)

    //let renderedGridSvg = SVG.OrthoGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
    //let renderedGridSvg = SVG.renderGrid maze.Grid (map.Graph.PathFromRootTo { RIndex = 0; CIndex = 3 }) map
    //let renderedGridSvg = SVG.renderGrid maze.Grid.ToSpecializedGrid (map.LongestPaths |> Seq.head) map    

    //let renderedGridSvg = SVG.PolarGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map
    //let renderedGridSvg = SVG.PolarGrid.render maze.Grid.ToSpecializedGrid (map.LongestPaths |> Seq.head) map
    
    let renderedGridSvg = SVG.HexGrid.render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastPartOfMazeZone) map

    File.WriteAllText(filePath.Replace(".html", ".svg"), renderedGridSvg, Encoding.UTF8)

    stopWatch.Stop()
    printfn $"Render maze ({stopWatch.ElapsedMilliseconds} ms)"

    printfn "Mazes creation finished !"
    printfn "File location is %s" filePath

    if not options.Value.quiet then    
        printfn "Press any key to exit"
        |> Console.ReadKey
        |> ignore