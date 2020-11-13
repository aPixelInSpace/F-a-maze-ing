module Mazes.CLGenerate

open System
open System.IO
open System.Text
open CommandLine
open CLSimpleTypes
open Mazes.Lib
open Mazes.Lib.Algo.Generate
open Mazes.Render.Text
open Mazes.Output.Html

let private defaultNameOfFile = "The F Amazing Maze"

[<Verb("generate", isDefault = true, HelpText = "Generate a new maze")>]
type GenerateOptions = {
    [<Option('n', "name", Required = false, HelpText = "A name for the maze. If empty, a default one is provided.")>] name : string option
    [<Option('r', "rows", Required = false, Default = 50, HelpText = "The number of rows of the maze." )>] rows : int
    [<Option('c', "columns", Required = false, Default = 80, HelpText = "The number of columns of the maze." )>] columns : int
    [<Option('a', "algo", Required = false, HelpText = "The algorithm to use to generate the maze. If empty, a random one is chosen." )>] algo : AlgoEnum option
    [<Option('s', "seed", Required = false, HelpText = "The seed to use for the random number generator. If empty, a random seed is picked." )>] seed : int option
    [<Option('d', "directory", Required = false, HelpText = "The directory where to output the maze. If empty, the directory is the directory of this program." )>] directory : string option
    [<Option('q', "quiet", Required = false, Default = false, HelpText = "Automatically exit the program when finished")>] quiet : bool
}

let matchAlgoEnumWithFunction algoEnum =
    match algoEnum with
       | AlgoEnum.BinaryTree -> BinaryTree.transformIntoMaze
       | AlgoEnum.Sidewinder -> Sidewinder.transformIntoMaze
       | _ -> raise(Exception("Generating algorithm unknown"))

let handleVerbGenerate (options : Parsed<GenerateOptions>) =
    let nameOfMaze =
        match options.Value.name with
        | Some name -> name
        | None -> defaultNameOfFile
    
    let directory =
        match options.Value.directory with
        | Some directory -> directory
        | None -> Directory.GetCurrentDirectory()
     
    let filePath = Path.Combine(directory, nameOfMaze + ".html")
     
    let grid = (Grid.create options.Value.rows options.Value.columns)
    
    let rng =
        match options.Value.seed with
        | Some seed -> Random(seed)
        | None -> Random()
    
    let algo =
        match options.Value.algo with
        | Some algo -> matchAlgoEnumWithFunction algo
        | None ->
            let rngAlgo = Random()
            let enumAlgoUpperBound = ((AlgoEnum.GetValues(typeof<AlgoEnum>)).GetUpperBound(0)) + 1            
            matchAlgoEnumWithFunction (enum<AlgoEnum> (rngAlgo.Next(enumAlgoUpperBound)))
    
    let transformedGrid = (algo rng grid)
    
    let maze =
        { Name = nameOfMaze
          Grid = transformedGrid }
    
    let htmlOutput = outputHtml maze (printGrid transformedGrid)
    
    File.WriteAllText(filePath, htmlOutput, Encoding.UTF8)
        
    printfn "Mazes creation finished !"
    printfn "File location is %s" filePath
    
    if not options.Value.quiet then    
        printfn "Press any key to exit"
        |> Console.ReadKey
        |> ignore
    else
        ()