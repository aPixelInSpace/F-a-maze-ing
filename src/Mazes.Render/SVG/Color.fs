// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.Color

open System
open Mazes.Core

type RGB = (int * int * int)

let toHtmlHexColor (rgb : RGB) =
    let (r, g, b) = rgb
    let rs = r.ToString("X")
    let gs = g.ToString("X")
    let bs = b.ToString("X")
    Some $"#{rs}{gs}{bs}"

let random (rng : Random) (color1 : RGB) (color2 : RGB) _ =
    let (r1, g1, b1, r2, g2, b2) =
        let (r1, g1, b1) = color1
        let (r2, g2, b2) = color2
        (Math.Min(r1, r2), Math.Min(g1, g2), Math.Min(b1, b2), Math.Max(r1, r2), Math.Max(g1, g2), Math.Max(b1, b2))

    (rng.Next(r1, r2 + 1), rng.Next(g1, g2 + 1), rng.Next(b1, b2 + 1))

let linearGradient (color1 : RGB) (color2 : RGB) (percent : float) =
    let (r1, g1, b1) = color1
    let (r2, g2, b2) = color2
    
    let compute v1 v2 = (int)((float)v1 + percent * ((float)v2 - (float)v1))
    ((compute r1 r2), (compute g1 g2), (compute b1 b2))

let columnDistance maxColumnIndex (coordinate : NCoordinate) =
    (float)coordinate.ToCoordinate2D.CIndex / (float)maxColumnIndex

let rowDistance maxRowIndex (coordinate : NCoordinate) =
    (float)(coordinate.ToCoordinate2D.RIndex) / (float)maxRowIndex