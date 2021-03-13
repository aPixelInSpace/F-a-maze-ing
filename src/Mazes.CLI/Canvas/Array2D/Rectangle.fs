// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Rectangle

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape

[<Literal>]
let verb = "s-rectangle"

[<Verb(verb, isDefault = false, HelpText = "Rectangle shape")>]
type Options = {
    [<Option('r', "rows", Required = true, HelpText = "The number of rows.")>] rows : int
    [<Option('c', "columns", Required = true, HelpText = "The number of columns." )>] columns : int
}

let handleVerb (options : Parsed<Options>) =
    Rectangle.create options.Value.rows options.Value.columns