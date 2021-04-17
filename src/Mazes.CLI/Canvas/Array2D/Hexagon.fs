// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Hexagon

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.Array2D.Shape.Ellipse

[<Literal>]
let verb = "s-hexagon"

[<Verb(verb, isDefault = false, HelpText = "Hexagon shape")>]
type Options = {
    [<Option('s', "edgeSize", Required = true, HelpText = "The length of one side of the hexagon.")>] edgeSize : float
}

let handleVerb (options : Parsed<Options>) =
    Hexagon.create
        options.Value.edgeSize