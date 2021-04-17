// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.Array2D.Shape.Rectangle

open Mazes.Core
open Mazes.Core.Canvas.Array2D

let create numberOfRows numberOfColumns =
    Canvas.create numberOfRows numberOfColumns (fun _ _ -> true)