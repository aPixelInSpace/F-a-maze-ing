// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.ArrayOfA.Disk

open CommandLine
open Mazes.Core.Canvas.ArrayOfA.Shape

[<Literal>]
let verb = "s-disk"

[<Verb(verb, isDefault = false, HelpText = "Disk shape")>]
type Options = {
    [<Option('r', "rings", Required = true, HelpText = "The number of rings.")>] rings : int
    [<Option('w', "ratio", Required = false, Default = 1.0, HelpText = "Width/height ratio." )>] ratio : float
    [<Option('c', "center", Required = false, Default = 3, HelpText = "Number of cells for the central ring." )>] center : int
}

let handleVerb (options : Parsed<Options>) =
    Disk.create options.Value.rings options.Value.ratio options.Value.center