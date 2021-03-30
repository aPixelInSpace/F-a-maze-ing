// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.Ortho

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG
open Mazes.Render.SVG.GlobalOptions
open Mazes.Render.SVG.OrthoGrid

type Lines =
    | Straight = 0
    | Circle = 1
    | Curved = 2
    | Random = 3

type WallRenderTypeEnum =
    | Line = 0
    | Inset = 1

let mapWallRenderType wallRenderTypeEnum =
    match wallRenderTypeEnum with
    | WallRenderTypeEnum.Line -> Line
    | WallRenderTypeEnum.Inset -> Inset
    | _ -> failwith "wallRenderTypeEnum not supported"

type BackgroundColorationEnum =
    | NoColoration = 0
    | Plain = 1
    | Distance = 2
    | GradientV = 3

let mapBackgroundColoration backColorEnum =
    match backColorEnum with
    | BackgroundColorationEnum.NoColoration -> NoColoration
    | BackgroundColorationEnum.Plain -> Plain
    | BackgroundColorationEnum.Distance -> Distance
    | BackgroundColorationEnum.GradientV -> GradientV
    | _ -> failwith "backColorEnum not supported"

[<Literal>]
let verb = "rs-ortho"

[<Verb(verb, isDefault = false, HelpText = "Orthogonal SVG render")>]
type Options = {
    [<Option('s', "solution", Required = false, Default = false, HelpText = "Show solution ?")>] solution : bool
    [<Option('e', "entranceExit", Required = false, Default = true, HelpText = "Add an entrance and an exit ?")>] entranceExit : bool
    [<Option('l', "lines", Required = false, Default = Lines.Straight, HelpText = "Type of lines (*Straight, Circle, Curved or Random). In circle mode only the Width value is considered; in curved mode you can change the curve option to obtain various effects")>] lines : Lines
    [<Option('b', "backgroundColor", Required = false, Default = BackgroundColorationEnum.Plain, HelpText = "Background coloration (*Plain, Distance, GradientV)")>] backgroundColor : BackgroundColorationEnum
    [<Option(Default = WallRenderTypeEnum.Line, HelpText = "Type of rendering for the walls (*Line or Inset)")>] wallRenderType : WallRenderTypeEnum
    [<Option(HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option(Default = 5.0, HelpText = "Curve multiplication factor")>] curveMultFact : float
    [<Option(Default = 30, HelpText = "Width of a single cell")>] width : int
    [<Option(Default = 30, HelpText = "Height of a single cell")>] height : int
    [<Option(Default = 10.0, HelpText = "Width of the bridge")>] bridgeWidth : float
    [<Option(Default = 12.0, HelpText = "Distance of the bridge from the center of a cell")>] bridgeDistanceFromCenter : float
    [<Option(Default = 20, HelpText = "Margin for the entire maze")>] margin : int
    [<Option(Default = 0, HelpText = "Change the curve value when drawing a line; only applicable in Curved mode on the lines option")>] curve : int
    [<Option(Default = "#FFFFFF", HelpText = "Color choice 1")>] color1 : string
    [<Option(Default = "#12A4B5", HelpText = "Color choice 2")>] color2 : string
}

let handleVerb (maze : Maze<_,_>) (options : Parsed<Options>) =
    let map = lazy (maze.createMap maze.NDStruct.GetFirstCellPartOfMaze)
    
    let colorMap =
        match options.Value.backgroundColor with
        | BackgroundColorationEnum.Distance -> Some map.Value
        | _ -> None
    
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

    let globalOptionsParam =
        {
            WallRenderType = mapWallRenderType options.Value.wallRenderType
            BackgroundColoration = mapBackgroundColoration options.Value.backgroundColor
            Color1 = options.Value.color1
            Color2 = options.Value.color2
        }

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
    
    OrthoGrid.render globalOptionsParam param maze.NDStruct path colorMap entrance exit