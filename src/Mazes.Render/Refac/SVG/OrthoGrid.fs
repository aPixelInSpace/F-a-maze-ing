// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.Refac.SVG.OrthoGrid

open Mazes.Core.Refac
open Mazes.Core.Refac.Trigonometry
open Mazes.Core.Refac.Structure
open Mazes.Render.Refac.SVG.GlobalOptions

type Parameters =
    {
        Width : int
        Height : int
        BridgeWidth : float
        BridgeDistanceFromCenter : float
        MarginWidth : int
        MarginHeight : int
    }
    
    static member CreateDefaultSquare =
        {
            Width = 30
            Height = 30
            BridgeWidth = 10.0
            BridgeDistanceFromCenter = 12.0
            MarginWidth = 20
            MarginHeight = 20
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

let linePoints calculatePoints coordinate position =
    let (leftTopX, leftTopY), (rightTopX, rightTopY), (leftBottomX, leftBottomY), (rightBottomX, rightBottomY) =
        calculatePoints coordinate

    let points =
        match position with
        | OrthogonalDisposition.Left -> (leftBottomX, leftBottomY), (leftTopX, leftTopY)
        | OrthogonalDisposition.Top -> (leftTopX, leftTopY), (rightTopX, rightTopY)
        | OrthogonalDisposition.Right -> (rightTopX, rightTopY), (rightBottomX, rightBottomY)
        | OrthogonalDisposition.Bottom -> (rightBottomX, rightBottomY), (leftBottomX, leftBottomY)

    points

let centerPoint calculatePoints height width coordinate =
    let leftTopPoint = match calculatePoints coordinate with | f, _, _, _ -> f
    leftTopPoint |> translatePoint (float (width / 2), float (height / 2))

let getConfig parameters grid =

    let calculateVerticalOffset = calculateVerticalOffset parameters.MarginHeight parameters.Height
    let calculateHorizontalOffset = calculateHorizontalOffset parameters.MarginWidth parameters.Width

    let totalHeight = (calculateVerticalOffset (GridArray2D.numberOfRows grid)) + parameters.MarginHeight
    let totalWidth = (calculateHorizontalOffset (GridArray2D.numberOfColumns grid)) + parameters.MarginWidth

    let calculatePoints = calculatePoints calculateVerticalOffset calculateHorizontalOffset parameters.Height parameters.Width
    let linePoints = linePoints calculatePoints

    let centerPoint = centerPoint calculatePoints parameters.Height parameters.Width
    let calculatePointsBridge = Base.calculatePointsBridge centerPoint (parameters.BridgeWidth / 2.0) parameters.BridgeDistanceFromCenter

    {
        LinePoints = linePoints
        BridgePoints = calculatePointsBridge
        Height = totalHeight
        Width = totalWidth        
    }