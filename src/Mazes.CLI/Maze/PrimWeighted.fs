// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.PrimWeighted

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-pw"

[<Verb(verb, isDefault = false, HelpText = "Prim's weighted algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option('w', "weighted", Required = false, HelpText = "Weight, if none is provided a random one is chosen")>] weight : int option
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()
        

    let weight =
        match options.Value.weight with
        | Some weight -> weight
        | None -> (Random()).Next()

    ndStruct |> PrimWeighted.createMaze seed weight