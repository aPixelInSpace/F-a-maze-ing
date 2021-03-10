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
type ShapeEllipse = {
    [<Option('r', "rowRadiusLength", Required = true, HelpText = "The length for the horizontal radius.")>] rowRadiusLength : int
    [<Option('c', "columnRadiusLength", Required = true, HelpText = "The length for the vertical radius.")>] columnRadiusLength : int
    [<Option('l', "rowEnlargingFactor", Required = false, Default = 0.0, HelpText = "Zoom factor on the horizontal axis.")>] rowEnlargingFactor : float
    [<Option('f', "columnEnlargingFactor", Required = false, Default = 0.0, HelpText = "Zoom factor on the vertical axis.")>] columnEnlargingFactor : float
    [<Option('h', "rowTranslationFactor", Required = false, Default = 0, HelpText = "Translation factor on the horizontal axis.")>] rowTranslationFactor : int
    [<Option('v', "columnTranslationFactor", Required = false, Default = 0, HelpText = "Translation factor on the vertical axis.")>] columnTranslationFactor : int
    [<Option('e', "ellipseFactor", Required = false, Default = 0.0, HelpText = "Ellipse factor.")>] ellipseFactor : float    
    [<Option('s', "side", Required = false, Default = SideEnum.Inside, HelpText = "Indicate if the grid is inside the ellipse 0; or outside 1." )>] sideEnum : SideEnum
}

let handleVerb (options : Parsed<ShapeEllipse>) =
    Ellipse.create
        options.Value.rowRadiusLength
        options.Value.columnRadiusLength
        options.Value.rowEnlargingFactor
        options.Value.columnEnlargingFactor
        options.Value.rowTranslationFactor
        options.Value.columnTranslationFactor
        (Some options.Value.ellipseFactor)
        (mapToSide options.Value.sideEnum)