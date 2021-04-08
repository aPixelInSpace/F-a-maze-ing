// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Canvas.ArrayOfA.Shape.Disk

open Mazes.Core.Refac.Canvas

let create numberOfRings widthHeightRatio numberOfCellsForCenterRing =
    CanvasArrayOfA.createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun _ _ -> true)