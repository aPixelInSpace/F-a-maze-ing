// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.ArrayOfA.Shape.Disk

open Mazes.Core.Canvas.ArrayOfA

let create numberOfRings widthHeightRatio numberOfCellsForCenterRing =
    Canvas.createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun _ _ -> true)