// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.HuntAndKill

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Verb("a-hk", isDefault = false, HelpText = "Hunt and kill algorithm")>]
type AlgoHk = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, If none is provided, a random one is chosen")>] seed : int option
}

let handleVerbAlgoHk ndStruct (options : Parsed<AlgoHk>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> HuntAndKill.createMaze seed