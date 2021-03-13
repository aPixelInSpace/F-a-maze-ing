// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.PentagonStar

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.Array2D.Shape.Ellipse

[<Literal>]
let verb = "s-pentagonStar"

[<Verb(verb, isDefault = false, HelpText = "Pentagon Star shape")>]
type Options = {
    [<Option('g', "greatEdgeSize", Required = true, HelpText = "The length of the great side of the pentagon star.")>] greatEdgeSize : float
    [<Option('s', "smallEdgeSize", Required = true, HelpText = "The length of the small side of the pentagon star.")>] smallEdgeSize : float
}

let handleVerb (options : Parsed<Options>) =
    PentagonStar.create
        options.Value.greatEdgeSize
        options.Value.smallEdgeSize