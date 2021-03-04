// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

// Learn more about F# at http://fsharp.org

open System
open CommandLine
open Mazes.Core.Structure
open Mazes.CLI.Canvas
open Mazes.CLI.Structure
open Mazes.CLI.Maze
open Mazes.CLI.Render
open Mazes.CLI.Output

[<Literal>]
let pipe = "|"

let verbs f argv =
    let i = ref 0
    argv
    |> Seq.groupBy (fun x ->
      if f x then incr i
      !i)
    |> Seq.map(fun s -> (snd s) |> Seq.filter(fun s -> s <> pipe))
    |> Seq.toArray

let handleVerb<'Verb, 'Result> handler verb (argv : string seq) : 'Result option =
    if verb <> (argv |> Seq.head) then None
    else    
        let parserResult = Parser.Default.ParseArguments<'Verb>(argv)
        match parserResult with
            | :? Parsed<'Verb> as parsed ->
                Some (parsed |> handler)
            | :? NotParsed<'Verb> as notParsed ->
                printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
                None
            | _ ->
                printfn "Something went wrong !"
                failwith "Something went wrong !"

let initCanvas argv =
    handleVerb<Array2D.Rectangle.ShapeRectangle, Mazes.Core.Canvas.Array2D.Canvas> Array2D.Rectangle.handleVerb Array2D.Rectangle.verb argv

let canvasArray2DToNdStruct argv canvas =        
    handleVerb<Grid2D.Ortho.GridOrtho, Mazes.Core.Structure.NDimensionalStructure<_,_>> (fun parsed -> parsed |> Grid2D.Ortho.handleVerb canvas |> NDimensionalStructure.create2D) Grid2D.Ortho.verb argv

let ndStructToMaze argv ndStruct =
    [|
       handleVerb<HuntAndKill.Options, Mazes.Core.Maze.Maze<_,_>> (HuntAndKill.handleVerb ndStruct) HuntAndKill.verb argv
       handleVerb<RecursiveBacktracker.Options, Mazes.Core.Maze.Maze<_,_>> (RecursiveBacktracker.handleVerb ndStruct) RecursiveBacktracker.verb argv
       handleVerb<Kruskal.Options, Mazes.Core.Maze.Maze<_,_>> (Kruskal.handleVerb ndStruct) Kruskal.verb argv
       handleVerb<PrimSimple.Options, Mazes.Core.Maze.Maze<_,_>> (PrimSimple.handleVerb ndStruct) PrimSimple.verb argv
       handleVerb<PrimSimpleModified.Options, Mazes.Core.Maze.Maze<_,_>> (PrimSimpleModified.handleVerb ndStruct) PrimSimpleModified.verb argv
       handleVerb<PrimWeighted.Options, Mazes.Core.Maze.Maze<_,_>> (PrimWeighted.handleVerb ndStruct) PrimWeighted.verb argv
    |]
    |> Array.tryFind(Option.isSome) |> Option.flatten

let mazeToRender argv maze =
    handleVerb<SVG.Ortho.RenderSVGOrtho, string> (SVG.Ortho.handleVerb maze) SVG.Ortho.verb argv

let renderToOutput argv render =
    handleVerb<File.OutputFile, unit> (File.handleVerb render) File.verb argv

[<EntryPoint>]
let main argv =
    
    let verbs = argv |> verbs ((=)pipe)
    
    let canvasArray2D = initCanvas verbs.[0]
    
    let ndStruct = canvasArray2D |> Option.bind(canvasArray2DToNdStruct verbs.[1])

    let maze = ndStruct |> Option.bind(ndStructToMaze verbs.[2])
    
    let render = maze |> Option.bind(mazeToRender verbs.[3])
    
    let output = render |> Option.bind(renderToOutput verbs.[4])

    
    0 // return an integer exit code
