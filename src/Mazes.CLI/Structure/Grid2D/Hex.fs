// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.Hex

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-hex"

[<Verb(verb, isDefault = false, HelpText = "Hexagonal grid")>]
type GridHex = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<GridHex>) =
    canvas
    |> match options.Value.empty with
       | true -> Hex.Grid.createEmptyBaseGrid
       | false -> Hex.Grid.createBaseGrid