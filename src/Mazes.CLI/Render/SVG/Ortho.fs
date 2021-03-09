// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.Ortho

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG
open Mazes.Render.SVG.OrthoGrid

type Lines =
    | StraightLines = 0
    | CircleLines = 1
    | FixedLines = 2

[<Literal>]
let verb = "rs-ortho"

[<Verb(verb, isDefault = false, HelpText = "Orthogonal SVG render")>]
type RenderSVGOrtho = {
    [<Option('b', "bridgeWidth", Required = false, Default = 10.0, HelpText = "Width of the bridge")>] bridgeWidth : float
    [<Option('d', "distColor", Required = false, Default = false, HelpText = "Apply distance coloration ?")>] distColor : bool
    [<Option('e', "entranceExit", Required = false, Default = true, HelpText = "Add an entrance and an exit ?")>] entranceExit : bool
    [<Option('h', "height", Required = false, Default = 30, HelpText = "Height of a single cell")>] height : int
    [<Option('j', "bridgeDistance", Required = false, Default = 12.0, HelpText = "Distance of the bridge from the center of a cell")>] bridgeDistanceFromCenter : float
    [<Option('l', "lines", Required = false, Default = Lines.StraightLines, HelpText = "Type of lines (Straight = 0; Circle = 1; Fixed = 2). In circle mode only the Width value is considered; in fixed mode you can change the curve option to obtain various effects")>] lines : Lines
    [<Option('m', "margin", Required = false, Default = 20, HelpText = "Margin for the entire maze")>] margin : int
    [<Option('w', "width", Required = false, Default = 30, HelpText = "Width of a single cell")>] width : int
    [<Option('s', "solution", Required = false, Default = false, HelpText = "Show solution ?")>] solution : bool
    [<Option('u', "curve", Required = false, Default = 0, HelpText = "Change the curve value when drawing a line; only applicable in fixed mode on the lines option")>] curve : int
}

let handleVerb (maze : Maze<_,_>) (options : Parsed<RenderSVGOrtho>) =
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
                | Lines.StraightLines | Lines.FixedLines -> options.Value.height
                | _ -> options.Value.width
            BridgeWidth = options.Value.bridgeWidth
            BridgeDistanceFromCenter = options.Value.bridgeDistanceFromCenter
            MarginWidth = options.Value.margin
            MarginHeight = options.Value.margin
            Line =
                match options.Value.lines with
                | Lines.StraightLines -> Straight
                | Lines.CircleLines -> Circle
                | Lines.FixedLines -> (options.Value.curve, options.Value.curve) |> Curve
                | _ -> failwith "Unsupported Lines value"
        }
    
    OrthoGrid.render param maze.NDStruct path colorMap entrance exit