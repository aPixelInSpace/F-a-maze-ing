﻿// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Render.Refac.SVG.GlobalOptions

open Mazes.Core.Refac
open Mazes.Render.Refac.SVG

type WallRenderType =
    | Line
    | Inset

type LineType =
    | Straight
    | Arc of ArcType
and ArcType =
    | Circle
    | FixedCurve of int * int
    | RandomCurve of (System.Random * float)

type BackgroundColoration =
    | NoColoration
    | Plain
    | Distance
    | GradientV
    | GradientH
    | GradientC
    | RandomColor of (System.Random * Color.RGB * Color.RGB)

type Parameters =
    {
        WallRenderType : WallRenderType
        LineType : LineType
        BackgroundColoration : BackgroundColoration
        Color1 : string
        Color2 : string
        SolutionColor : string
        NormalWallColor : string
    }
    
    static member Default =
        {
            WallRenderType = Line
            LineType = Straight
            BackgroundColoration = NoColoration
            Color1 = "#FFFFFF"
            Color2 = "#12A4B5"
            SolutionColor = "purple"
            NormalWallColor = "#333333"
        }

type SvgGrid<'Disposition> =
    {
        LinePoints : NCoordinate -> 'Disposition -> (float * float) * (float * float)
        BridgePoints : NCoordinate -> NCoordinate -> ((float * float) * (float * float)) * ((float * float) * (float * float))
        Height : int
        Width : int
    }