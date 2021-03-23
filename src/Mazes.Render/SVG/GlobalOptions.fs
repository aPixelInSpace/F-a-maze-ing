// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Render.SVG.GlobalOptions

type WallRenderType =
    | Line
    | Inset

type Parameters =
    {
        WallRenderType : WallRenderType
    }