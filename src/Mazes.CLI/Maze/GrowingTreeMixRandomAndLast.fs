// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.GrowingTreeMixRandomAndLast

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-gtmrl"

[<Verb(verb, isDefault = false, HelpText = "Growing Tree mix random and last algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option('l', "longPassages", Required = false, Default = 0.5, HelpText = "Probability to generate long passages from 0.0 always choose a random neighbor to 1.0 always choose the last (same as recursive backtracker)")>] longPassages : float
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> GrowingTreeMixRandomAndLast.createMaze seed options.Value.longPassages