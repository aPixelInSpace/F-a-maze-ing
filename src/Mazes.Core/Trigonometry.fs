// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Trigonometry

open System

let convertToRadian angleInDegree =
    (angleInDegree * Math.PI) / 180.0

let convertToDegree angleInRadian =
    (180.0 / Math.PI) * angleInRadian

/// Returns the point situated at the matching angle and distance from the base point
let calculatePoint (baseX, baseY) angleInRadian distance =
    (distance * Math.Cos(angleInRadian) + baseX, distance * Math.Sin(angleInRadian) + baseY)

/// Returns the angle in radians between two points
let calculateAngle (pointAx, pointAy) (pointBx, pointBy) =
    let deltaX = pointBx - pointAx
    let deltaY = pointAy - pointBy
    Math.Atan2(deltaY, deltaX)

/// Returns the distance between two points
let calculateDistance (pointAx, pointAy) (pointBx, pointBy) =
    Math.Sqrt(((pointBx - pointAx) ** 2.0) + ((pointBy - pointAy) ** 2.0))

let translatePoint (translation : (float * float)) (point : (float * float)) =
    (fst point + fst translation, snd point + snd translation)

let middlePoint (pointAx, pointAy) (pointBx, pointBy) =
    ((pointAx + pointBx) / 2.0, (pointAy + pointBy) / 2.0)

let pythagorasSide side hypotenuse =
    Math.Sqrt(hypotenuse ** 2.0 - side ** 2.0)

let pythagorasHypotenuse sideA sideB =
    Math.Sqrt(sideA ** 2.0 + sideB ** 2.0)

/// Returns the length of the third side of a triangle formed by sideA, sideB and the angle between them
let sideAngleSide sideA angleInRadian sideB =
    Math.Sqrt(sideA ** 2.0 + sideB ** 2.0 - 2.0 * sideA * sideB * Math.Cos(angleInRadian))

let sideAngleAngle sideA angleAInRadian angleBInRadian =
    (sideA * Math.Sin(angleAInRadian)) / Math.Sin(angleBInRadian)

let checkSide (px:float, py:float) (s1x:float, s1y:float) (s2x:float, s2y:float) =
    (py - s1y) * (s2x - s1x) - (px - s1x) * (s2y - s1y) >= 0.0

/// Checks if the given x,y are inside the polygon formed by the given points (the points must be consecutive)
let isInsideConvexPolygon (points : (float * float) array) x y =
    let checkSide = checkSide (x, y)

    (points
    |> Array.pairwise
    |> Array.fold(fun check (pointA, pointB) -> check && checkSide pointA pointB) true)

    &&

    checkSide (points |> Array.last) (points |> Array.head)

/// Checks if the given x,y are inside at least one of the polygons
let isInsideMultipleConvexPolygons (polygonsPoints : (float * float) array seq) x y =
    polygonsPoints
    |> Seq.fold(fun check polygonPoints -> check || isInsideConvexPolygon polygonPoints x y) false