// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLGenerate

open System
open System.IO
open System.Text
open CommandLine
open Mazes.Core
open Mazes.Core.Canvas
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate
open Mazes.Render
open Mazes.Render.Text
open Mazes.Output.Html

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
           | AlgoEnum.BinaryTree -> BinaryTree.createMaze BinaryTree.Direction.Left BinaryTree.Direction.Bottom rngSeed 1 1
           | AlgoEnum.Sidewinder -> Sidewinder.createMaze Sidewinder.Direction.Top Sidewinder.Direction.Right rngSeed 1 1
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

    let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> OrthoGrid.create)
    //let grid = (Shape.TriangleIsosceles.create 150 Shape.TriangleIsosceles.BaseAt.Bottom 3 2 |> Grid.create)
    //let grid = (Shape.Ellipse.create 15 19 0.0 0.0 0 0 None Shape.Ellipse.Side.Inside |> Grid.create)
    //let grid = (Shape.Ellipse.create 20 15 -10.0 0.0 0 8 (Some 2.5) Shape.Ellipse.Side.Outside |> Grid.create)
    //let grid = (Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside |> Grid.create)
    //let grid = (Mazes.Utility.Canvas.Convert.fromImage 0.0f "d:\\temp\\Microchip.png" |> Grid.create)
    //let grid = Shape.Ellipse.create 6 7 0.0 0.0 0 0 (Some 0.05) Shape.Ellipse.Side.Inside |> OrthoGrid.create

    //let canvasSave = (Shape.Rectangle.create 15 15 |> Canvas.save)
    //File.WriteAllText(filePath.Replace(".html", ".canvas.mazes"), canvasSave, Encoding.UTF8)
    //let save = File.ReadAllText(filePath.Replace(".html", ".canvas.mazes"))     
    //let canvas =
    //   match Canvas.Convert.fromString save with
    //    | Some canvas -> canvas
    //    | None -> failwith "A problem occured while loading the saved canvas"
    //let grid = (canvas |> Grid.create)
    
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

    // Async
//    let maze1 =
//        async {
//                let gridSliced1 = GridView.sliceGrid grid { RowIndex = 0; ColumnIndex = 0 } { RowIndex = 9; ColumnIndex = 9 }
//                let maze = (algo (rngSeed) gridSliced1)
//                GridView.mergeGrid maze.Grid grid { RowIndex = 0; ColumnIndex = 0 }
//            }
//    let maze2 =
//        async {
//                let gridSliced2 = GridView.sliceGrid grid { RowIndex = 0; ColumnIndex = 10 } { RowIndex = 8; ColumnIndex = 19 }
//                let maze = (algo (rngSeed + 1) gridSliced2)
//                GridView.mergeGrid maze.Grid grid { RowIndex = 0; ColumnIndex = 10 }
//            }
//    let maze3 =
//        async {
//                let gridSliced3 = GridView.sliceGrid grid { RowIndex = 10; ColumnIndex = 0 } { RowIndex = 19; ColumnIndex = 9 }
//                let maze = (algo (rngSeed + 2) gridSliced3)
//                GridView.mergeGrid maze.Grid grid { RowIndex = 10; ColumnIndex = 0 }
//            }
//    let maze4 =
//        async {
//                let gridSliced4 = GridView.sliceGrid grid { RowIndex = 9; ColumnIndex = 10 } { RowIndex = 19; ColumnIndex = 19 }
//                let maze = (algo (rngSeed + 3) gridSliced4)
//                GridView.mergeGrid maze.Grid grid { RowIndex = 9; ColumnIndex = 10 }
//            }
//    
//    [maze1; maze2; maze3; maze4] |> Async.Parallel |> Async.RunSynchronously |> ignore
//    
//    let maze = { Grid = grid }

    let maze = (algo rngSeed grid)

    let map = maze.createDijkstraMap maze.Grid.GetFirstTopLeftPartOfMazeZone

    let renderedGrid = renderGrid  (maze.Grid.ToSpecializedGrid)
    
    let htmlOutput = outputHtml maze { Name = nameOfMaze } renderedGrid
    File.WriteAllText(filePath, htmlOutput, Encoding.UTF8)
    
    //let rawTestOutput = Output.RawForTest.outputRawForTest maze renderedGrid
    //File.WriteAllText(filePath.Replace(".html", ".txt"), rawTestOutput, Encoding.UTF8)

    let renderedGridSvg = SVG.renderGrid (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetFirstBottomRightPartOfMazeZone) map
    //let renderedGridSvg = SVG.renderGrid maze.Grid (map.Graph.PathFromRootTo { RowIndex = 0; ColumnIndex = 3 }) map
    //let renderedGridSvg = SVG.renderGrid maze.Grid.ToSpecializedGrid (map.LongestPaths |> Seq.head) map
    File.WriteAllText(filePath.Replace(".html", ".svg"), renderedGridSvg, Encoding.UTF8)

    printfn "Mazes creation finished !"
    printfn "File location is %s" filePath

    if not options.Value.quiet then    
        printfn "Press any key to exit"
        |> Console.ReadKey
        |> ignore