// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Render.Text.Ortho

open CommandLine
open Mazes.Core.Maze
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Ortho
open Mazes.Render.Text

[<Literal>]
let verb = "rt-ortho"

[<Verb(verb, isDefault = false, HelpText = "Unicode render")>]
type Options = {
    [<Option('e', "entranceExit", Required = false, Default = true, HelpText = "Add an entrance and an exit ?")>] entranceExit : bool
}

let handleVerb (maze : Maze<GridArray2D<OrthoPosition>, OrthoPosition>) (options : Parsed<Options>) =
    
    match options.Value.entranceExit with
    | true ->
        let (entrance, exit) = (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)
        maze.OpenMaze (entrance, exit)
    | false -> ()

    renderGrid (snd maze.NDStruct.FirstSlice2D)