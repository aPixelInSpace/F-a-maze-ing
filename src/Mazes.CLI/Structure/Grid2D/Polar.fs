// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.Polar

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-polar"

[<Verb(verb, isDefault = false, HelpText = "Polar grid")>]
type GridPolar = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<GridPolar>) =
    canvas
    |> match options.Value.empty with
       | true -> Polar.Grid.createEmptyBaseGrid
       | false -> Polar.Grid.createBaseGrid