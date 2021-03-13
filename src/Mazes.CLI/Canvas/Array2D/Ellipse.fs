// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.Ellipse

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.Array2D.Shape.Ellipse

type SideEnum =
    | Inside = 0
    | Outside = 1

let mapToSide sideEnum =
    match sideEnum with
    | SideEnum.Inside -> Inside
    | SideEnum.Outside -> Outside
    | _ -> failwith "Unknown side"

[<Literal>]
let verb = "s-ellipse"

[<Verb(verb, isDefault = false, HelpText = "Ellipse shape")>]
type Options = {
    [<Option('r', "rowRadiusLength", Required = true, HelpText = "The length for the horizontal radius.")>] rowRadiusLength : int
    [<Option('c', "columnRadiusLength", Required = true, HelpText = "The length for the vertical radius.")>] columnRadiusLength : int
    [<Option(Default = 0.0, HelpText = "Zoom factor on the horizontal axis.")>] rowEnlargingFactor : float
    [<Option(Default = 0.0, HelpText = "Zoom factor on the vertical axis.")>] columnEnlargingFactor : float
    [<Option(Default = 0, HelpText = "Translation factor on the horizontal axis.")>] rowTranslationFactor : int
    [<Option(Default = 0, HelpText = "Translation factor on the vertical axis.")>] columnTranslationFactor : int
    [<Option(Default = 0.0, HelpText = "Inside ellipse factor.")>] ellipseFactor : float    
    [<Option(Default = SideEnum.Inside, HelpText = "Indicate where the ellipse is Inside or Outside." )>] side : SideEnum
}

let handleVerb (options : Parsed<Options>) =
    Ellipse.create
        options.Value.rowRadiusLength
        options.Value.columnRadiusLength
        options.Value.rowEnlargingFactor
        options.Value.columnEnlargingFactor
        options.Value.rowTranslationFactor
        options.Value.columnTranslationFactor
        (Some options.Value.ellipseFactor)
        (mapToSide options.Value.side)