// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Image

open CommandLine
open Mazes.Utility.Canvas

[<Literal>]
let verb = "s-image"

[<Verb(verb, isDefault = false, HelpText = "Shape from an image")>]
type Options = {
    [<Option('p', "path", Required = true, HelpText = "The full path of the image file")>] path : string
    [<Option('t', "tolerance", Required = false, Default = 0.0, HelpText = "The tolerance on the pixel color.")>] tolerance : float
}

let handleVerb (options : Parsed<Options>) =
    Convert.fromImage ((float32)options.Value.tolerance) options.Value.path