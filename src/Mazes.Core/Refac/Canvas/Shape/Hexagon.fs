// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Canvas.Array2D.Shape.Hexagon

open System
open Mazes.Core.Refac.Canvas

let isInsideHexagon edgeSize vertical horizontal centerX centerY (rowIndex : int) (columnIndex : int) =
(*
    http://www.playchilla.com/how-to-check-if-a-point-is-inside-a-hexagon
   _____
  /q4.q1\  
  \q3.q2/
   -----
*)  
    let quadrant2x = Math.Abs(((float)rowIndex + edgeSize - centerX))
    let quadrant2y = Math.Abs(((float)columnIndex + edgeSize - centerY))

    if quadrant2x > horizontal || quadrant2y > horizontal * 2.0 then
        false
    else
        (2.0 * vertical * horizontal - vertical * quadrant2x - horizontal * quadrant2y) >= 0.0

let create (edgeSize : float) =

    let numberOfRows = (edgeSize * 2.0) + 1.0
    let numberOfColumns = (edgeSize * 2.0) + 1.0

    let vertical = edgeSize / 2.0
    let horizontal = edgeSize

    let centerX = numberOfRows - 1.0
    let centerY = numberOfColumns - 1.0

    CanvasArray2D.create ((int)numberOfRows) ((int)numberOfColumns) (isInsideHexagon edgeSize vertical horizontal centerX centerY)