// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.Ortho

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG
open Mazes.Render.SVG.OrthoGrid

type Lines =
    | Straight = 0
    | Circle = 1
    | Curved = 2
    | Random = 3

[<Literal>]
let verb = "rs-ortho"

[<Verb(verb, isDefault = false, HelpText = "Orthogonal SVG render")>]
type Options = {
    [<Option('d', "distColor", Required = false, Default = false, HelpText = "Apply distance coloration ?")>] distColor : bool
    [<Option('s', "solution", Required = false, Default = false, HelpText = "Show solution ?")>] solution : bool
    [<Option('e', "entranceExit", Required = false, Default = true, HelpText = "Add an entrance and an exit ?")>] entranceExit : bool
    [<Option('l', "lines", Required = false, Default = Lines.Straight, HelpText = "Type of lines Straight, Circle or Curved). In circle mode only the Width value is considered; in curved mode you can change the curve option to obtain various effects")>] lines : Lines
    [<Option(HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option(Default = 5.0, HelpText = "Curve multiplication factor")>] curveMultFact : float
    [<Option(Default = 30, HelpText = "Width of a single cell")>] width : int
    [<Option(Default = 30, HelpText = "Height of a single cell")>] height : int
    [<Option(Default = 10.0, HelpText = "Width of the bridge")>] bridgeWidth : float
    [<Option(Default = 12.0, HelpText = "Distance of the bridge from the center of a cell")>] bridgeDistanceFromCenter : float
    [<Option(Default = 20, HelpText = "Margin for the entire maze")>] margin : int
    [<Option(Default = 0, HelpText = "Change the curve value when drawing a line; only applicable in fixed mode on the lines option")>] curve : int
}

let handleVerb (maze : Maze<_,_>) (options : Parsed<Options>) =
    let map = lazy (maze.createMap maze.NDStruct.GetFirstCellPartOfMaze)
    
    let colorMap =
        match options.Value.distColor with
        | true -> Some map.Value
        | false -> None
    
    let path =
        match options.Value.solution with
        | true -> (Some (map.Value.ShortestPathGraph.PathFromRootTo maze.NDStruct.GetLastCellPartOfMaze))
        | false -> None

    let (entrance, exit) =
        match options.Value.entranceExit with
        | true ->
            let (entrance, exit) = (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)
            maze.OpenMaze (entrance, exit)
            (Some entrance, Some exit)
        | false -> (None, None)

    let param =
        {
            Width = options.Value.width
            Height =
                match options.Value.lines with
                | Lines.Straight | Lines.Curved -> options.Value.height
                | _ -> options.Value.width
            BridgeWidth = options.Value.bridgeWidth
            BridgeDistanceFromCenter = options.Value.bridgeDistanceFromCenter
            MarginWidth = options.Value.margin
            MarginHeight = options.Value.margin
            Line =
                match options.Value.lines with
                | Lines.Straight -> Straight
                | Lines.Circle -> Circle
                | Lines.Curved -> (options.Value.curve, options.Value.curve) |> Curve
                | Lines.Random ->
                    match options.Value.seed with
                    | Some seed -> (System.Random(seed), options.Value.curveMultFact) |> Random
                    | None -> (System.Random(), options.Value.curveMultFact) |> Random
                | _ -> failwith "Unsupported Lines value"
        }
    
    OrthoGrid.render param maze.NDStruct path colorMap entrance exit