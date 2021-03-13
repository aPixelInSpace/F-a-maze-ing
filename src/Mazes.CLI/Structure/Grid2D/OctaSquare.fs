// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.OctaSquare

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-octas"

[<Verb(verb, isDefault = false, HelpText = "Octagonal and square grid")>]
type Options = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<Options>) =
    canvas
    |> match options.Value.empty with
       | true -> OctaSquare.Grid.createEmptyBaseGrid
       | false -> OctaSquare.Grid.createBaseGrid