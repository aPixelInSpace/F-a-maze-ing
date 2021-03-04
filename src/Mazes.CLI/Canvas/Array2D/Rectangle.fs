// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Rectangle

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape

[<Literal>]
let verb = "s-rect"

[<Verb(verb, isDefault = false, HelpText = "Rectangle shape")>]
type ShapeRectangle = {
    [<Option('r', "rows", Required = true, HelpText = "The number of rows.")>] rows : int
    [<Option('c', "columns", Required = true, HelpText = "The number of columns." )>] columns : int
}

let handleVerb (options : Parsed<ShapeRectangle>) =
    Rectangle.create options.Value.rows options.Value.columns