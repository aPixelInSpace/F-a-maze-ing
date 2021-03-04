// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.Ortho

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG

[<Literal>]
let verb = "rs-ortho"

[<Verb(verb, isDefault = false, HelpText = "Orthogonal SVG render")>]
type RenderSVGOrtho = {
    [<Option('d', "distColor", Required = false, Default = false, HelpText = "Distance coloration")>] distColor : bool
}

let handleVerb (maze : Maze<_,_>) (options : Parsed<RenderSVGOrtho>) =
    let map = maze.createMap maze.NDStruct.GetFirstCellPartOfMaze
    
    let colorMap =
        match options.Value.distColor with
        | true -> Some map
        | false -> None
    
    OrthoGrid.render maze.NDStruct None colorMap None None