// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.NoMaze

open System
open CommandLine
open Mazes.Core.Maze
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-nm"

[<Verb(verb, isDefault = false, HelpText = "Does not run a maze algorithm")>]
type Options = {
    [<Option()>] noOption : int
}

let handleVerb ndStruct (options : Parsed<Options>) =

    ndStruct |> Maze.toMaze