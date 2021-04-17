// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Canvas.Array2D.Shape

open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Trigonometry

module Pentagon =

    let distanceBetweenTwoNonAdjacentVertices edgeSize =  sideAngleSide edgeSize (convertToRadian 108.0) edgeSize

    let height edgeSize = pythagorasSide (edgeSize / 2.0) (distanceBetweenTwoNonAdjacentVertices edgeSize)

    let calculatePointsPentagon centerPoint edgeSize rotationTheta translate =
        let distance = (height edgeSize) / 2.0
        let calculatePoint theta =
            calculatePoint centerPoint (convertToRadian (rotationTheta + theta)) distance
            |> translatePoint translate
        
        let pointA = calculatePoint -90.0
        let pointB = calculatePoint -18.0
        let pointC = calculatePoint 54.0
        let pointD = calculatePoint 126.0
        let pointE = calculatePoint 198.0

        [| pointA; pointB; pointC; pointD; pointE |]

    let create (edgeSize : float) =

        let height = height edgeSize
        let distanceBetweenTwoNonAdjacentVertices = distanceBetweenTwoNonAdjacentVertices edgeSize

        let numberOfRows = height
        let numberOfColumns = distanceBetweenTwoNonAdjacentVertices

        let pentagonPoints = calculatePointsPentagon (numberOfColumns / 2.0, numberOfRows / 2.0) edgeSize 0.0 (0.0, 0.0)

        Canvas.create ((int)numberOfRows) ((int)numberOfColumns) (fun rowIndex columnIndex ->  isInsideConvexPolygon pentagonPoints ((float)columnIndex) ((float)rowIndex))

module PentagonStar =

    let pentagonStarPolygons (greatPentagonPoints : (float * float) array) (smallPentagonPoints : (float * float) array) =
        seq {
            yield smallPentagonPoints
            yield [| greatPentagonPoints.[0]; smallPentagonPoints.[0]; smallPentagonPoints.[4] |]
            yield [| greatPentagonPoints.[1]; smallPentagonPoints.[1]; smallPentagonPoints.[0] |]
            yield [| greatPentagonPoints.[2]; smallPentagonPoints.[2]; smallPentagonPoints.[1] |]
            yield [| greatPentagonPoints.[3]; smallPentagonPoints.[3]; smallPentagonPoints.[2] |]
            yield [| greatPentagonPoints.[4]; smallPentagonPoints.[4]; smallPentagonPoints.[3] |]
        }

    let create (greatEdgeSize : float) (smallEdgeSize : float) =
        let greatPentagonHeight = Pentagon.height greatEdgeSize
        let greatPentagonDistanceBetweenTwoNonAdjacentVertices = Pentagon.distanceBetweenTwoNonAdjacentVertices greatEdgeSize

        let numberOfRows = greatPentagonHeight
        let numberOfColumns = greatPentagonDistanceBetweenTwoNonAdjacentVertices

        let center = (numberOfColumns / 2.0, numberOfRows / 2.0)
        let greatPentagonPoints = Pentagon.calculatePointsPentagon center greatEdgeSize 0.0 (0.0, 0.0)
        let smallPentagonPoints = Pentagon.calculatePointsPentagon center smallEdgeSize 36.0 (0.0, 0.0)

        let pentagonStarPolygons = pentagonStarPolygons greatPentagonPoints smallPentagonPoints

        Canvas.create ((int)numberOfRows) ((int)numberOfColumns) (fun rowIndex columnIndex ->
            isInsideMultipleConvexPolygons pentagonStarPolygons ((float)columnIndex) ((float)rowIndex))