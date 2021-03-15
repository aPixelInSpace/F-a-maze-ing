// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.GrowingTreeDirection

open System
open CommandLine
open Mazes.Core.Maze.Generate

[<Literal>]
let verb = "a-gtd"

[<Verb(verb, isDefault = false, HelpText = "Growing Tree mix oldest and last algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option(Default = 0.5, HelpText = "Probability between 0.0 and 1.0 to choose the neighbor on the right (all the probability cannot be > 1.0)")>] toRightWeight : float
    [<Option(Default = 0.3, HelpText = "Probability between 0.0 and 1.0 to choose the neighbor on the bottom (all the probability cannot be > 1.0)")>] toBottomWeight : float
    [<Option(Default = 0.1, HelpText = "Probability between 0.0 and 1.0 to choose the neighbor on the left (all the probability cannot be > 1.0)")>] toLeftWeight : float
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> GrowingTreeDirection.createMaze seed options.Value.toRightWeight options.Value.toBottomWeight options.Value.toLeftWeight