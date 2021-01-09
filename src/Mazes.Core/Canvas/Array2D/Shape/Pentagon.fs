// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Canvas.Array2D.Shape.Pentagon

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.Array2D.Shape.Trigonometry

let isInsidePentagon (pointA, pointB, pointC, pointD, pointE) (rowIndex : int) (columnIndex : int) =
    let checkSide = checkSide ((float)columnIndex, (float)rowIndex) 

    checkSide pointA pointB && checkSide pointB pointC && checkSide pointC pointD && checkSide pointD pointE && checkSide pointE pointA

let create (edgeSize : float) =

    let distanceBetweenTwoNonAdjacentVertices =  sideAngleSide edgeSize (convertToRadian 108.0) edgeSize
    let height = pythagorasSide (edgeSize / 2.0) distanceBetweenTwoNonAdjacentVertices

    let numberOfRows = height
    let numberOfColumns = distanceBetweenTwoNonAdjacentVertices

    // the five point of the pentagon
    let pointA = (numberOfColumns / 2.0, 0.0)
    let pointB = calculatePoint pointA (convertToRadian 36.0) edgeSize
    let pointC = calculatePoint pointA (convertToRadian 72.0) distanceBetweenTwoNonAdjacentVertices
    let pointD = calculatePoint pointA (convertToRadian 108.0) distanceBetweenTwoNonAdjacentVertices
    let pointE = calculatePoint pointA (convertToRadian (108.0 + 36.0)) edgeSize

    Canvas.create ((int)numberOfRows) ((int)numberOfColumns) (isInsidePentagon (pointA, pointB, pointC, pointD, pointE))