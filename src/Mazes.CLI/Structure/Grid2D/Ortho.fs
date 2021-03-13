// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.Ortho

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-ortho"

[<Verb(verb, isDefault = false, HelpText = "Orthogonal grid")>]
type Options = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<Options>) =
    canvas
    |> match options.Value.empty with
       | true -> Ortho.Grid.createEmptyBaseGrid
       | false -> Ortho.Grid.createBaseGrid