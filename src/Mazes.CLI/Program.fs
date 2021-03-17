// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

// Learn more about F# at http://fsharp.org

open System
open CommandLine
open Mazes.CLI.Render.SVG
open Mazes.Core.Structure
open Mazes.CLI.Canvas
open Mazes.CLI.Structure
open Mazes.CLI.Maze
open Mazes.CLI.Render
open Mazes.CLI.Output
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Brick
open Mazes.Core.Structure.Grid2D.Type.Hex
open Mazes.Core.Structure.Grid2D.Type.OctaSquare
open Mazes.Core.Structure.Grid2D.Type.Ortho
open Mazes.Core.Structure.Grid2D.Type.PentaCairo
open Mazes.Core.Structure.Grid2D.Type.Tri

[<Literal>]
let pipe = ":"

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
                match handler with
                | Some handler -> Some (parsed |> handler)
                | None -> None
            | :? NotParsed<'Verb> as notParsed ->
                printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
                None
            | _ ->
                printfn "Something went wrong !"
                failwith "Something went wrong !"

let pick array = array |> Array.tryFind(Option.isSome) |> Option.flatten

type CanvasChoice =
    | Array2DCanvas of Mazes.Core.Canvas.Array2D.Canvas
    | ArrayOfACanvas of Mazes.Core.Canvas.ArrayOfA.Canvas

let initCanvas argv =
    [|        
        handleVerb<Array2D.Rectangle.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.Rectangle.handleVerb) Array2D.Rectangle.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.TriangleIsosceles.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.TriangleIsosceles.handleVerb) Array2D.TriangleIsosceles.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.Ellipse.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.Ellipse.handleVerb) Array2D.Ellipse.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.Hexagon.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.Hexagon.handleVerb) Array2D.Hexagon.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.Pentagon.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.Pentagon.handleVerb) Array2D.Pentagon.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.PentagonStar.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.PentagonStar.handleVerb) Array2D.PentagonStar.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<Array2D.Image.Options, Mazes.Core.Canvas.Array2D.Canvas> (Some Array2D.Image.handleVerb) Array2D.Image.verb argv
        |> Option.map CanvasChoice.Array2DCanvas

        handleVerb<ArrayOfA.Disk.Options, Mazes.Core.Canvas.ArrayOfA.Canvas> (Some ArrayOfA.Disk.handleVerb) ArrayOfA.Disk.verb argv
        |> Option.map CanvasChoice.ArrayOfACanvas
    |] |> pick

type NdStructChoice =
    | Array2DOrtho of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<OrthoPosition>, OrthoPosition>
    | Array2DHex of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<HexPosition>, HexPosition>
    | Array2DTri of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<TriPosition>, TriPosition>
    | Array2DOctaSquare of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<OctaSquarePosition>, OctaSquarePosition>
    | Array2DPentaCairo of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<PentaCairoPosition>, PentaCairoPosition>
    | Array2DBrick of Mazes.Core.Structure.NDimensionalStructure<GridArray2D<BrickPosition>, BrickPosition>
    | ArrayOfAPolar of Mazes.Core.Structure.NDimensionalStructure<GridArrayOfA, PolarPosition>

let canvasArray2DToNdStruct argv (canvas : CanvasChoice) =
    match canvas with
    | Array2DCanvas canvas ->
        [|
            handleVerb<Grid2D.Ortho.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.Ortho.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.Ortho.verb argv
            |> Option.map Array2DOrtho
            
            handleVerb<Grid2D.Hex.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.Hex.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.Hex.verb argv
            |> Option.map Array2DHex

            handleVerb<Grid2D.Tri.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.Tri.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.Tri.verb argv
            |> Option.map Array2DTri

            handleVerb<Grid2D.OctaSquare.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.OctaSquare.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.OctaSquare.verb argv
            |> Option.map Array2DOctaSquare

            handleVerb<Grid2D.PentaCairo.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.PentaCairo.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.PentaCairo.verb argv
            |> Option.map Array2DPentaCairo

            handleVerb<Grid2D.Brick.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.Brick.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.Brick.verb argv
            |> Option.map Array2DBrick
        |] |> pick
    | ArrayOfACanvas canvas ->
        handleVerb<Grid2D.Polar.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> (Some (fun parsed -> parsed |> Grid2D.Polar.handleVerb canvas |> NDimensionalStructure.create2D)) Grid2D.Polar.verb argv
        |> Option.map ArrayOfAPolar

type MazeChoice =
    | Array2DOrtho of Mazes.Core.Maze.Maze<GridArray2D<OrthoPosition>, OrthoPosition>
    | Array2DHex of Mazes.Core.Maze.Maze<GridArray2D<HexPosition>, HexPosition>
    | Array2DTri of Mazes.Core.Maze.Maze<GridArray2D<TriPosition>, TriPosition>
    | Array2DOctaSquare of Mazes.Core.Maze.Maze<GridArray2D<OctaSquarePosition>, OctaSquarePosition>
    | Array2DPentaCairo of Mazes.Core.Maze.Maze<GridArray2D<PentaCairoPosition>, PentaCairoPosition>
    | Array2DBrick of Mazes.Core.Maze.Maze<GridArray2D<BrickPosition>, BrickPosition>
    | ArrayOfAPolar of Mazes.Core.Maze.Maze<GridArrayOfA, PolarPosition>

let ndStructToMaze argv (ndStruct : NdStructChoice) =
    let algorithms ndStruct =
        [|
           handleVerb<NoMaze.Options, Mazes.Core.Maze.Maze<_,_>> (Some (NoMaze.handleVerb ndStruct)) NoMaze.verb argv
           handleVerb<BinaryTree.Options, Mazes.Core.Maze.Maze<_,_>> (Some (BinaryTree.handleVerb ndStruct)) BinaryTree.verb argv
           handleVerb<Sidewinder.Options, Mazes.Core.Maze.Maze<_,_>> (Some (Sidewinder.handleVerb ndStruct)) Sidewinder.verb argv
           handleVerb<AldousBroder.Options, Mazes.Core.Maze.Maze<_,_>> (Some (AldousBroder.handleVerb ndStruct)) AldousBroder.verb argv
           handleVerb<Wilson.Options, Mazes.Core.Maze.Maze<_,_>> (Some (Wilson.handleVerb ndStruct)) Wilson.verb argv
           handleVerb<HuntAndKill.Options, Mazes.Core.Maze.Maze<_,_>> (Some (HuntAndKill.handleVerb ndStruct)) HuntAndKill.verb argv
           handleVerb<RecursiveBacktracker.Options, Mazes.Core.Maze.Maze<_,_>> (Some (RecursiveBacktracker.handleVerb ndStruct)) RecursiveBacktracker.verb argv
           handleVerb<Kruskal.Options, Mazes.Core.Maze.Maze<_,_>> (Some (Kruskal.handleVerb ndStruct)) Kruskal.verb argv
           handleVerb<PrimSimple.Options, Mazes.Core.Maze.Maze<_,_>> (Some (PrimSimple.handleVerb ndStruct)) PrimSimple.verb argv
           handleVerb<PrimSimpleModified.Options, Mazes.Core.Maze.Maze<_,_>> (Some (PrimSimpleModified.handleVerb ndStruct)) PrimSimpleModified.verb argv
           handleVerb<PrimWeighted.Options, Mazes.Core.Maze.Maze<_,_>> (Some (PrimWeighted.handleVerb ndStruct)) PrimWeighted.verb argv
           handleVerb<Eller.Options, Mazes.Core.Maze.Maze<_,_>> (Some (Eller.handleVerb ndStruct)) Eller.verb argv
           handleVerb<RecursiveDivision.Options, Mazes.Core.Maze.Maze<_,_>> (Some (RecursiveDivision.handleVerb ndStruct)) RecursiveDivision.verb argv
           handleVerb<GrowingTreeMixRandomAndLast.Options, Mazes.Core.Maze.Maze<_,_>> (Some (GrowingTreeMixRandomAndLast.handleVerb ndStruct)) GrowingTreeMixRandomAndLast.verb argv
           handleVerb<GrowingTreeMixChosenRandomAndLast.Options, Mazes.Core.Maze.Maze<_,_>> (Some (GrowingTreeMixChosenRandomAndLast.handleVerb ndStruct)) GrowingTreeMixChosenRandomAndLast.verb argv
           handleVerb<GrowingTreeMixOldestAndLast.Options, Mazes.Core.Maze.Maze<_,_>> (Some (GrowingTreeMixOldestAndLast.handleVerb ndStruct)) GrowingTreeMixOldestAndLast.verb argv
           handleVerb<GrowingTreeDirection.Options, Mazes.Core.Maze.Maze<_,_>> (Some (GrowingTreeDirection.handleVerb ndStruct)) GrowingTreeDirection.verb argv
           handleVerb<GrowingTreeSpiral.Options, Mazes.Core.Maze.Maze<_,_>> (Some (GrowingTreeSpiral.handleVerb ndStruct)) GrowingTreeSpiral.verb argv
        |] |> pick

    match ndStruct with
    | NdStructChoice.Array2DOrtho ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DOrtho
    | NdStructChoice.Array2DHex ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DHex
    | NdStructChoice.Array2DTri ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DTri
    | NdStructChoice.Array2DOctaSquare ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DOctaSquare
    | NdStructChoice.Array2DPentaCairo ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DPentaCairo
    | NdStructChoice.Array2DBrick ndStruct -> algorithms ndStruct |> Option.map MazeChoice.Array2DBrick
    | NdStructChoice.ArrayOfAPolar ndStruct -> algorithms ndStruct  |> Option.map MazeChoice.ArrayOfAPolar

let mazeToRender argv (maze : MazeChoice) =
    match maze with
    | Array2DOrtho maze ->
        [|
           handleVerb<SVG.Ortho.Options, string> (Some (SVG.Ortho.handleVerb maze)) SVG.Ortho.verb argv
           handleVerb<Text.Ortho.Options, string> (Some (Text.Ortho.handleVerb maze)) Text.Ortho.verb argv
        |] |> pick
    | Array2DHex maze -> handleVerb<SVG.Hex.Options, string> (Some (SVG.Hex.handleVerb maze)) SVG.Hex.verb argv
    | Array2DTri maze -> handleVerb<SVG.Tri.Options, string> (Some (SVG.Tri.handleVerb maze)) SVG.Tri.verb argv
    | Array2DOctaSquare maze -> handleVerb<SVG.OctaSquare.Options, string> (Some (SVG.OctaSquare.handleVerb maze)) SVG.OctaSquare.verb argv
    | Array2DPentaCairo maze -> handleVerb<SVG.PentaCairo.Options, string> (Some (SVG.PentaCairo.handleVerb maze)) SVG.PentaCairo.verb argv
    | Array2DBrick maze -> handleVerb<SVG.Brick.Options, string> (Some (SVG.Brick.handleVerb maze)) SVG.Brick.verb argv
    | ArrayOfAPolar maze -> handleVerb<SVG.Polar.Options, string> (Some (SVG.Polar.handleVerb maze)) SVG.Polar.verb argv

let renderToOutput argv render =
    [|
        handleVerb<File.Options, unit> (Some (File.handleVerb render)) File.verb argv
        handleVerb<Console.Options, unit> (Some (Console.handleVerb render)) Console.verb argv
    |] |> pick    

let checkAllHandlers argv =    
    handleVerb<Array2D.Rectangle.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.Rectangle.verb argv |> ignore
    handleVerb<Array2D.TriangleIsosceles.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.TriangleIsosceles.verb argv |> ignore
    handleVerb<Array2D.Ellipse.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.Ellipse.verb argv |> ignore
    handleVerb<Array2D.Hexagon.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.Hexagon.verb argv |> ignore
    handleVerb<Array2D.Pentagon.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.Pentagon.verb argv |> ignore
    handleVerb<Array2D.PentagonStar.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.PentagonStar.verb argv |> ignore
    handleVerb<Array2D.Image.Options, Mazes.Core.Canvas.Array2D.Canvas> None Array2D.Image.verb argv |> ignore
    handleVerb<ArrayOfA.Disk.Options, Mazes.Core.Canvas.ArrayOfA.Canvas> None ArrayOfA.Disk.verb argv |> ignore

    handleVerb<Grid2D.Ortho.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.Ortho.verb argv |> ignore    
    handleVerb<Grid2D.Hex.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.Hex.verb argv |> ignore
    handleVerb<Grid2D.Tri.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.Tri.verb argv |> ignore
    handleVerb<Grid2D.OctaSquare.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.OctaSquare.verb argv |> ignore
    handleVerb<Grid2D.PentaCairo.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.PentaCairo.verb argv |> ignore
    handleVerb<Grid2D.Brick.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.Brick.verb argv |> ignore
    handleVerb<Grid2D.Polar.Options, Mazes.Core.Structure.NDimensionalStructure<_,_>> None Grid2D.Polar.verb argv |> ignore

    handleVerb<NoMaze.Options, Mazes.Core.Maze.Maze<_,_>> None NoMaze.verb argv |> ignore
    handleVerb<BinaryTree.Options, Mazes.Core.Maze.Maze<_,_>> None BinaryTree.verb argv |> ignore
    handleVerb<Sidewinder.Options, Mazes.Core.Maze.Maze<_,_>> None Sidewinder.verb argv |> ignore
    handleVerb<AldousBroder.Options, Mazes.Core.Maze.Maze<_,_>> None AldousBroder.verb argv |> ignore
    handleVerb<Wilson.Options, Mazes.Core.Maze.Maze<_,_>> None Wilson.verb argv |> ignore
    handleVerb<HuntAndKill.Options, Mazes.Core.Maze.Maze<_,_>> None HuntAndKill.verb argv |> ignore
    handleVerb<RecursiveBacktracker.Options, Mazes.Core.Maze.Maze<_,_>> None RecursiveBacktracker.verb argv |> ignore
    handleVerb<Kruskal.Options, Mazes.Core.Maze.Maze<_,_>> None Kruskal.verb argv |> ignore
    handleVerb<PrimSimple.Options, Mazes.Core.Maze.Maze<_,_>> None PrimSimple.verb argv |> ignore
    handleVerb<PrimSimpleModified.Options, Mazes.Core.Maze.Maze<_,_>> None PrimSimpleModified.verb argv |> ignore
    handleVerb<PrimWeighted.Options, Mazes.Core.Maze.Maze<_,_>> None PrimWeighted.verb argv |> ignore
    handleVerb<Eller.Options, Mazes.Core.Maze.Maze<_,_>> None Eller.verb argv |> ignore
    handleVerb<RecursiveDivision.Options, Mazes.Core.Maze.Maze<_,_>> None RecursiveDivision.verb argv |> ignore
    handleVerb<GrowingTreeMixRandomAndLast.Options, Mazes.Core.Maze.Maze<_,_>> None GrowingTreeMixRandomAndLast.verb argv |> ignore
    handleVerb<GrowingTreeMixChosenRandomAndLast.Options, Mazes.Core.Maze.Maze<_,_>> None GrowingTreeMixChosenRandomAndLast.verb argv |> ignore
    handleVerb<GrowingTreeMixOldestAndLast.Options, Mazes.Core.Maze.Maze<_,_>> None GrowingTreeMixOldestAndLast.verb argv |> ignore
    handleVerb<GrowingTreeDirection.Options, Mazes.Core.Maze.Maze<_,_>> None GrowingTreeDirection.verb argv |> ignore
    handleVerb<GrowingTreeSpiral.Options, Mazes.Core.Maze.Maze<_,_>> None GrowingTreeSpiral.verb argv |> ignore

    handleVerb<SVG.Ortho.Options, string> None SVG.Ortho.verb argv |> ignore
    handleVerb<SVG.Hex.Options, string> None SVG.Hex.verb argv |> ignore
    handleVerb<SVG.Tri.Options, string> None SVG.Tri.verb argv |> ignore
    handleVerb<SVG.OctaSquare.Options, string> None SVG.OctaSquare.verb argv |> ignore
    handleVerb<SVG.PentaCairo.Options, string> None SVG.PentaCairo.verb argv |> ignore
    handleVerb<SVG.Brick.Options, string> None SVG.Brick.verb argv |> ignore
    handleVerb<SVG.Polar.Options, string> None SVG.Polar.verb argv |> ignore
    handleVerb<Text.Ortho.Options, string> None Text.Ortho.verb argv |> ignore

    handleVerb<File.Options, unit> None File.verb argv |> ignore
    handleVerb<Console.Options, unit> None Console.verb argv |> ignore

[<EntryPoint>]
let main argv =

    let verbs = argv |> verbs ((=)pipe)

    match verbs.Length with
    | 1 ->
        checkAllHandlers verbs.[0]
    | 5 ->
        let result =
            initCanvas verbs.[0]
            |> Option.bind(canvasArray2DToNdStruct verbs.[1])
            |> Option.bind(ndStructToMaze verbs.[2])
            |> Option.bind(mazeToRender verbs.[3])
            |> Option.bind(renderToOutput verbs.[4])

        match result with
        | Some _ -> printfn "\nMaze generated !"
        | None -> printfn "Something went wrong, some chosen actions are not compatible."

    | _ -> failwith "Number of verbs not supported"
    
    0 // return an integer exit code