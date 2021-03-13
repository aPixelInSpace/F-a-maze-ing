// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Pentagon

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.Array2D.Shape.Ellipse

[<Literal>]
let verb = "s-pentagon"

[<Verb(verb, isDefault = false, HelpText = "Pentagon shape")>]
type Options = {
    [<Option('s', "edgeSize", Required = true, HelpText = "The length of one side of the pentagon.")>] edgeSize : float
}

let handleVerb (options : Parsed<Options>) =
    Pentagon.create
        options.Value.edgeSize