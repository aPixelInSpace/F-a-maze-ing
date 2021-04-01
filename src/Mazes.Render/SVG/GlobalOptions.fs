// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Render.SVG.GlobalOptions

type WallRenderType =
    | Line
    | Inset

type BackgroundColoration =
    | NoColoration
    | Plain
    | Distance
    | GradientV
    | GradientH
    | GradientC

type Parameters =
    {
        WallRenderType : WallRenderType
        BackgroundColoration : BackgroundColoration
        Color1 : string
        Color2 : string
    }
    
    static member Default =
        {
            WallRenderType = Line
            BackgroundColoration = NoColoration
            Color1 = "#FFFFFF"
            Color2 = "#12A4B5"
        }