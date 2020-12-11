// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.Shape.Rectangle

open Mazes.Core
open Mazes.Core.Canvas

let create numberOfRows numberOfColumns =
    Canvas.create numberOfRows numberOfColumns (fun _ _ -> true)