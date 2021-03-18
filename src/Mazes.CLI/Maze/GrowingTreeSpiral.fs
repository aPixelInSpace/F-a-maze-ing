// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.GrowingTreeSpiral

open System
open CommandLine
open Mazes.Core.Maze.Generate

type SpiralRevolutionEnum =
    | Clockwise = 0
    | CounterClockwise = 1

[<Literal>]
let verb = "a-gts"

[<Verb(verb, isDefault = false, HelpText = "Growing Tree mix oldest and last algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option(Default = 1.0, HelpText = "Probability between 0.0 and 1.0 to choose the neighbor that will make a spiral")>] spiralWeight : float
    [<Option(Default = 1.0, HelpText = "Probability between 0.0 and 1.0 to make perfect spiral (as best as possible)")>] spiralUniformity : float
    [<Option(Default = 4, HelpText = "Max length for the spiral")>] spiralMaxLength : int
    [<Option(Default = 0.0, HelpText = "Probability between 0.0 (counter-clockwise) and 1.0 (clockwise) to choose the revolution of the spiral")>] spiralRevolution : float
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> GrowingTreeSpiral.createMaze seed options.Value.spiralWeight options.Value.spiralUniformity options.Value.spiralMaxLength options.Value.spiralRevolution