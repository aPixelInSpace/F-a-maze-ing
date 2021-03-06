// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.Polar

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG

[<Literal>]
let verb = "rs-polar"

[<Verb(verb, isDefault = false, HelpText = "Polar SVG render")>]
type RenderSVGPolar = {
    [<Option('d', "distColor", Required = false, Default = false, HelpText = "Distance coloration")>] distColor : bool
}

let handleVerb (maze : Maze<_,_>) (options : Parsed<RenderSVGPolar>) =
    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    
    let colorMap =
        match options.Value.distColor with
        | true -> Some map
        | false -> None
    
    PolarGrid.render maze.NDStruct None colorMap None None