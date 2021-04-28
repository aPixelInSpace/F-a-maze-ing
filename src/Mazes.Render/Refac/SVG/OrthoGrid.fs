// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.OrthoGrid

open Mazes.Core.Refac
open Mazes.Core.Refac.Structure

type Parameters =
    {
        Width : int
        Height : int
        BridgeWidth : float
        BridgeDistanceFromCenter : float
        MarginWidth : int
        MarginHeight : int
    }

let calculateVerticalOffset marginHeight height numberOfRows =
    marginHeight + (numberOfRows * height)

let calculateHorizontalOffset marginWidth width numberOfColumns =
    marginWidth + (numberOfColumns * width)

let calculatePoints calculateVerticalOffset calculateHorizontalOffset height width coordinate =
    let coordinate2D = coordinate.Coordinate2D
    let baseX, baseY = (calculateHorizontalOffset coordinate2D.CIndex, calculateVerticalOffset coordinate2D.RIndex)
    let leftTopX, leftTopY = (float baseX, float baseY)
    let rightTopX, rightTopY = (float (baseX + width), float baseY)
    let leftBottomX, leftBottomY = (float baseX , float (baseY + height))
    let rightBottomX, rightBottomY = (float (baseX + width), float (baseY + height))

    (leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY)

let cellPoints calculatePoints coordinate position =
    let (leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY) =
        calculatePoints coordinate

    let points =
        match position with
        | OrthogonalDisposition.Left -> (leftBottomX, leftBottomY), (leftTopX, leftTopY)
        | OrthogonalDisposition.Top -> (leftTopX, leftTopY), (rightTopX, rightTopY)
        | OrthogonalDisposition.Right -> (rightTopX, rightTopY), (rightBottomX, rightBottomY)
        | OrthogonalDisposition.Bottom -> (rightBottomX, rightBottomY), (leftBottomX, leftBottomY)

    points

let getParam parameters grid =

    let calculateVerticalOffset = calculateVerticalOffset parameters.MarginHeight parameters.Height
    let calculateHorizontalOffset = calculateHorizontalOffset parameters.MarginWidth parameters.Width

    let totalHeight = calculateVerticalOffset (GridArray2D.numberOfRows grid)
    let totalWidth = calculateHorizontalOffset (GridArray2D.numberOfColumns grid)

    let calculatePoints = calculatePoints calculateVerticalOffset calculateHorizontalOffset parameters.Height parameters.Width
    let cellPoints = cellPoints calculatePoints

    cellPoints, totalHeight, totalWidth