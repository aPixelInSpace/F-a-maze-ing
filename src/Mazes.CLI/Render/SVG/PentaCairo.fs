﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.SVG.PentaCairo

open CommandLine
open Mazes.Core.Maze
open Mazes.Render.SVG

[<Literal>]
let verb = "rs-pentac"

[<Verb(verb, isDefault = false, HelpText = "Pentagonal 'Cairo' SVG render")>]
type Options = {
    [<Option('d', "distColor", Required = false, Default = false, HelpText = "Distance coloration")>] distColor : bool
    [<Option('s', "solution", Required = false, Default = false, HelpText = "Show solution ?")>] solution : bool
    [<Option('e', "entranceExit", Required = false, Default = true, HelpText = "Add an entrance and an exit ?")>] entranceExit : bool
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

    PentaCairoGrid.render maze.NDStruct path colorMap entrance exit