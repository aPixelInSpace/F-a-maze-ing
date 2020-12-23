// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Polar.Canvas.Shape.Disc

open Mazes.Core.Grid.Polar.Canvas

let create numberOfRings widthHeightRatio numberOfCellsForCenterRing =
    Canvas.create numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun _ _ -> true)