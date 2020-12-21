// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.PolarGrid

open System
open System.Text
open Mazes.Core.Grid.Polar

let render (grid : PolarGrid) =
    /// round
    let r (f : float) =
        Math.Round(f, 2).ToString().Replace(",", ".")

    let sBuilder = StringBuilder()

    let marginWidth = 20
    let marginHeight = 20

    let ringHeight = 50
    let centerX = (float)((grid.Cells.Length * ringHeight) + marginWidth)
    let centerY = (float)((grid.Cells.Length * ringHeight) + marginHeight)

    let width = (grid.Cells.Length * ringHeight * 2) + (marginWidth * 2)
    let height = (grid.Cells.Length * ringHeight * 2) + (marginHeight * 2)

    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>\n") |> ignore
    sBuilder.Append("<!-- Copyright 2020 Patrizio Amella. All rights reserved. See License at https://github.com/aPixelInSpace/F-a-maze-ing/blob/main/LICENSE for more information. -->\n") |> ignore
    sBuilder.Append("<svg width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">\n") |> ignore

    let lastRingIndex = grid.Cells.Length - 1
    for ringIndex in 0 .. lastRingIndex do
        let ring = grid.Cells.[ringIndex]
        let theta = (2.0 * Math.PI) / (float)ring.Length
        let innerRadius = (float)(ringIndex * ringHeight)
        let outerRadius = (float)((ringIndex + 1) * ringHeight)
        for cellIndex in 0 .. ring.Length - 1 do
            let thetaCcw = (float)cellIndex * theta
            let thetaCw = ((float)(cellIndex + 1)) * theta

            let bottomLeftX = centerX + innerRadius * Math.Cos(thetaCcw)
            let bottomLeftY = centerY + innerRadius * Math.Sin(thetaCcw)
            let topLeftX = centerX + outerRadius * Math.Cos(thetaCcw)
            let topLeftY = centerY + outerRadius * Math.Sin(thetaCcw)
            let bottomRightX = centerX + innerRadius * Math.Cos(thetaCw)
            let bottomRightY = centerY + innerRadius * Math.Sin(thetaCw)
            let topRightX = centerX + outerRadius * Math.Cos(thetaCw)
            let topRightY = centerY + outerRadius * Math.Sin(thetaCw)

            sBuilder.Append($"<path d=\"M {r bottomLeftX} {r bottomLeftY} A {r innerRadius} {r innerRadius}, 0, 0, 1, {r bottomRightX} {r bottomRightY}\" fill=\"transparent\" stroke=\"#333\" stroke-width=\"1\"/>\n")
                    .Append($"<path d=\"M {r bottomLeftX} {r bottomLeftY} L {r topLeftX} {r topLeftY} 0\" fill=\"transparent\" stroke=\"#333\" stroke-width=\"1\"/>") |> ignore

            if ringIndex = lastRingIndex then
                sBuilder.Append($"<path d=\"M {r topLeftX} {r topLeftY} A {r outerRadius} {r outerRadius}, 0, 0, 1, {r topRightX} {r topRightY}\" fill=\"transparent\" stroke=\"#333\" stroke-width=\"1\"/>\n") |> ignore

    sBuilder.Append("</svg>") |> ignore
    sBuilder.ToString()