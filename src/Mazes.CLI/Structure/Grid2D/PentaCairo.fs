// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.PentaCairo

open System
open CommandLine
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-pentac"

[<Verb(verb, isDefault = false, HelpText = "Pentagonal 'Cairo' grid")>]
type Options = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option('w', "weave", Required = false, Default = 0.0, HelpText = "Value between 0.0 and 1.0 to generate 'bridges' / weave the maze")>] weave : float
}

let handleVerb canvas (options : Parsed<Options>) =
    let nStruct =
        canvas
        |> match options.Value.empty with
           | true -> PentaCairo.Grid.createEmptyBaseGrid
           | false -> PentaCairo.Grid.createBaseGrid
        |> NDimensionalStructure.create2D

    let rng =
        match options.Value.seed with
        | Some seed -> Random(seed)
        | None -> Random()

    nStruct.Weave rng options.Value.weave

    nStruct