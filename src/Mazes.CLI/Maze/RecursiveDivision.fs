// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.RecursiveDivision

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-rd"

[<Verb(verb, isDefault = false, HelpText = "Recursive division algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option('r', "rooms", Required = false, Default = 0.0, HelpText = "Probability to generate a room (a space with no wall), range from 0.0 no room to 1.0 always generate a room. If this parameter is greater than 0.0 do not forget to create the grid empty with -e")>] rooms : float
    [<Option(Default = 3, HelpText = "Room height")>] roomsHeight : int
    [<Option(Default = 3, HelpText = "Room width")>] roomsWidth : int
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> RecursiveDivision.createMaze seed options.Value.rooms options.Value.roomsHeight options.Value.roomsWidth