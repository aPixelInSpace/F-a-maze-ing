// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.GrowingTreeMixChosenRandomAndLast

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-gtmcrl"

[<Verb(verb, isDefault = false, HelpText = "Growing Tree mix chosen random and last algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option('l', "longPassages", Required = false, Default = 0.5, HelpText = "Probability to generate long passages from 0.0 always choose a random neighbor (stick to it until it has no neighbor) to 1.0 always choose the last (same as recursive backtracker)")>] longPassages : float
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> GrowingTreeMixChosenRandomAndLast.createMaze seed options.Value.longPassages