// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Structure.Grid2D.Brick

open CommandLine
open Mazes.Core.Structure.Grid2D.Type

[<Literal>]
let verb = "g-brick"

[<Verb(verb, isDefault = false, HelpText = "Brick grid")>]
type Options = {
    [<Option('e', "empty", Required = false, Default = false, HelpText = "If true, the grid will have no internal connections")>] empty : bool
}

let handleVerb canvas (options : Parsed<Options>) =
    canvas
    |> match options.Value.empty with
       | true -> Brick.Grid.createEmptyBaseGrid
       | false -> Brick.Grid.createBaseGrid