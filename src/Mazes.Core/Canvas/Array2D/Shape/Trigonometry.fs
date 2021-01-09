// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.Array2D.Shape.Trigonometry

open System

let convertToRadian angleInDegree =
    (angleInDegree * Math.PI) / 180.0

/// Returns the point situated at the matching angle and distance from the base point
let calculatePoint (baseX, baseY) angleInRadian distance =
    (distance * Math.Cos(angleInRadian) + baseX, distance * Math.Sin(angleInRadian) + baseY)

let pythagorasSide side hypotenuse =
    Math.Sqrt(hypotenuse ** 2.0 - side ** 2.0)

let pythagorasHypotenuse sideA sideB =
    Math.Sqrt(sideA ** 2.0 + sideB ** 2.0)

/// Returns the length of the third side of a triangle formed by sideA, sideB and the angle between them
let sideAngleSide sideA angleInRadian sideB =
    Math.Sqrt(sideA ** 2.0 + sideB ** 2.0 - 2.0 * sideA * sideB * Math.Cos(angleInRadian))

let checkSide (px:float, py:float) (s1x:float, s1y:float) (s2x:float, s2y:float) =
    (py - s1y) * (s2x - s1x) - (px - s1x) * (s2y - s1y) >= 0.0