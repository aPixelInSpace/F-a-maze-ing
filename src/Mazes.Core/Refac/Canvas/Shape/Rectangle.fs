// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Canvas.Array2D.Shape.Rectangle

open Mazes.Core.Refac.Canvas

let create numberOfRows numberOfColumns =
    CanvasArray2D.create numberOfRows numberOfColumns (fun _ _ -> true)