// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.OrthoGrid

open Mazes.Core.Refac

type LineType =
    | Straight
    | Circle
    | FixedCurve of int * int
    | RandomCurve of float

type Parameters =
    {
        Width : int
        Height : int
        BridgeWidth : float
        BridgeDistanceFromCenter : float
        MarginWidth : int
        MarginHeight : int
        Line : LineType
    }

let calculateHeight marginHeight height numberOfRows =
    marginHeight + (numberOfRows * height)

let calculateWidth marginWidth width numberOfColumns =
    marginWidth + (numberOfColumns * width)

let orientation coordinate = if (coordinate.Coordinate2D.RIndex + coordinate.Coordinate2D.CIndex) % 2 = 0 then "0" else "1"

let calculatePoints calculateVerticalOffset calculateHorizontalOffset height width coordinate =
    let coordinate2D = coordinate.Coordinate2D
    let baseX, baseY = (calculateHorizontalOffset coordinate2D.CIndex, calculateVerticalOffset coordinate2D.RIndex)
    let leftTopX, leftTopY = (float baseX, float baseY)
    let rightTopX, rightTopY = (float (baseX + width), float baseY)
    let leftBottomX, leftBottomY = (float baseX , float (baseY + height))
    let rightBottomX, rightBottomY = (float (baseX + width), float (baseY + height))

    (leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)

let lineParam calculatePoints coordinate position lineType =
    let (leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY) =
        calculatePoints coordinate

    let points =
        match position with
        | OrthogonalDisposition.Left -> leftBottomX, leftBottomY, leftTopX, leftTopY
        | OrthogonalDisposition.Top -> leftTopX, leftTopY, rightTopX, rightTopY
        | OrthogonalDisposition.Right -> rightTopX, rightTopY, rightBottomX, rightBottomY
        | OrthogonalDisposition.Bottom -> rightBottomX, rightBottomY, leftBottomX, leftBottomY

    points 