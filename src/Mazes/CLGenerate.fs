module Mazes.CLGenerate

open System
open System.IO
open System.Text
open CommandLine
open CLSimpleTypes
open Mazes.Core.Canvas
open Mazes.Core.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate
open Mazes.Render.Text
open Mazes.Output.Html
open Mazes.Output.RawForTest

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

    let rng =
        match options.Value.seed with
        | Some seed -> Random(seed)
        | None -> Random()

    let matchAlgoEnumWithFunction algoEnum =
        match algoEnum with
           | AlgoEnum.BinaryTree -> BinaryTree.transformIntoMaze Top Right rng 1 1
           | AlgoEnum.Sidewinder -> Sidewinder.transformIntoMaze Right Bottom rng 1 1
           | _ -> raise(Exception("Generating algorithm unknown"))


    let nameOfMaze =
        match options.Value.name with
        | Some name -> name
        | None -> defaultNameOfFile

    let directory =
        match options.Value.directory with
        | Some directory -> directory
        | None -> Directory.GetCurrentDirectory()

    let filePath = Path.Combine(directory, nameOfMaze + ".html")

    //printfn "%s" (Shape.Ellipse.createCanvas 5 15 0.0 0.0 0 0 Shape.Ellipse.Side.Outside |> renderCanvas)
    
    let grid = (Shape.Rectangle.create options.Value.rows options.Value.columns |> Grid.create)
    //let grid = (Shape.TriangleIsosceles.create 51 Shape.TriangleIsosceles.BaseAt.Bottom 3 2 |> Grid.create)
    //let grid = (Shape.Ellipse.create 20 30 0.0 0.0 0 0 Shape.Ellipse.Side.Inside |> Grid.create)

    let algo =
        match options.Value.algo with
        | Some algo -> matchAlgoEnumWithFunction algo
        | None ->
            let rngAlgo = Random()
            let enumAlgoUpperBound = ((AlgoEnum.GetValues(typeof<AlgoEnum>)).GetUpperBound(0)) + 1            
            matchAlgoEnumWithFunction (enum<AlgoEnum> (rngAlgo.Next(enumAlgoUpperBound)))

    let transformedGrid = (algo grid)

    let maze =
        { Name = nameOfMaze
          Grid = transformedGrid }

    let htmlOutput = outputHtml maze (printGrid transformedGrid)
    File.WriteAllText(filePath, htmlOutput, Encoding.UTF8)
    
    //let rawTestOutput = outputRawForTest maze (printGrid transformedGrid)
    //File.WriteAllText(filePath.Replace(".html", ".txt"), rawTestOutput, Encoding.UTF8)

    printfn "Mazes creation finished !"
    printfn "File location is %s" filePath

    if not options.Value.quiet then    
        printfn "Press any key to exit"
        |> Console.ReadKey
        |> ignore