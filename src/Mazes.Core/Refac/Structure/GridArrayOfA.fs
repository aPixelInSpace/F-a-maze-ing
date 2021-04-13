// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System
open Mazes.Core.Refac

type GridArrayOfA =
    private
        {
            Canvas : Canvas.CanvasArrayOfA
            Type : GridArrayOfAType
            Cells : CellArrayOfA[][]
        }