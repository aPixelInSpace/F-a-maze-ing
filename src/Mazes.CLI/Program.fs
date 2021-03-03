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

let nextVerb argv =
    let indexPipe =
        argv
        |> Seq.tryFindIndex(fun s -> s = "|")

    match indexPipe with
    | Some indexPipe -> argv |> Seq.skip(indexPipe + 1)
    | None -> Seq.empty

let verbs f argv =
    let i = ref 0
    argv
    |> Seq.groupBy (fun x ->
      if f x then incr i
      !i)
    |> Seq.map snd
    |> Seq.toArray

// todo : make generic these command line functions
let initCanvas argv =
    let shapeRectangleResult = Parser.Default.ParseArguments<Array2D.Rectangle.ShapeRectangle>(argv)
    let canvasArray2D =
        match shapeRectangleResult with
        | :? Parsed<Array2D.Rectangle.ShapeRectangle> as parsed ->
            Some (parsed |> Array2D.Rectangle.handleVerbShapeRectangle)
        | :? NotParsed<Array2D.Rectangle.ShapeRectangle> as notParsed ->
            printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
            None
        | _ ->
            printfn "Something went wrong !"
            failwith "Something went wrong !"

    canvasArray2D

let canvasArray2DToNdStruct argv canvas =
    let gridOrthoResult = Parser.Default.ParseArguments<Grid2D.Ortho.GridOrtho>(argv)
    let gridOrtho =
        match gridOrthoResult with
        | :? Parsed<Grid2D.Ortho.GridOrtho> as parsed ->
            Some (
                parsed
                |> Grid2D.Ortho.handleVerbGridOrtho canvas
                |> NDimensionalStructure.create2D)
        | :? NotParsed<Grid2D.Ortho.GridOrtho> as notParsed ->
            printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
            None
        | _ ->
            printfn "Something went wrong !"
            failwith "Something went wrong !"

    gridOrtho

let ndStructToMaze argv ndStruct =
    let parserResult = Parser.Default.ParseArguments<HuntAndKill.AlgoHk>(argv)
    let maze =
        match parserResult with
        | :? Parsed<HuntAndKill.AlgoHk> as parsed -> Some (parsed |> HuntAndKill.handleVerbAlgoHk ndStruct)
        | :? NotParsed<HuntAndKill.AlgoHk> as notParsed ->
            printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
            None
        | _ ->
            printfn "Something went wrong !"
            failwith "Something went wrong !"

    maze

let mazeToRender argv maze =
    let parserResult = Parser.Default.ParseArguments<SVG.Ortho.RenderSVGOrtho>(argv)
    let render =
        match parserResult with
        | :? Parsed<SVG.Ortho.RenderSVGOrtho> as parsed -> Some (parsed |> SVG.Ortho.handleVerbRenderSVGOrtho maze)
        | :? NotParsed<SVG.Ortho.RenderSVGOrtho> as notParsed ->
            printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
            None
        | _ ->
            printfn "Something went wrong !"
            failwith "Something went wrong !"

    render

let renderToOutput argv render =
    let parserResult = Parser.Default.ParseArguments<File.OutputFile>(argv)
    let render =
        match parserResult with
        | :? Parsed<File.OutputFile> as parsed -> Some (parsed |> File.handleVerbOutputFile render)
        | :? NotParsed<File.OutputFile> as notParsed ->
            printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
            None
        | _ ->
            printfn "Something went wrong !"
            failwith "Something went wrong !"

    render

[<EntryPoint>]
let main argv =
    
    let verbs = argv |> verbs ((=)"|")
    
    let canvasArray2D = initCanvas verbs.[0]
    
    let ndStruct = canvasArray2D |> Option.bind(canvasArray2DToNdStruct verbs.[1])

    let maze = ndStruct |> Option.bind(ndStructToMaze verbs.[2])
    
    let render = maze |> Option.bind(mazeToRender verbs.[3])
    
    let output = render |> Option.bind(renderToOutput verbs.[4])

    
    0 // return an integer exit code
