// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.Tri

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-tri"

[<Verb(verb, isDefault = false, HelpText = "Triangular grid")>]
type Options = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<Options>) =
    canvas
    |> match options.Value.empty with
       | true -> Tri.Grid.createEmptyBaseGrid
       | false -> Tri.Grid.createBaseGrid