// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.Base

open System
open System.Text
open Mazes.Core.Analysis.Dijkstra

// #4287f5 : blue
// #63a873 : green
// #fffaba : pale yellow
// #fffef0 : very pale yellow

[<Literal>]
let normalWallClass = "n"
[<Literal>]
let borderWallClass = "b"
[<Literal>]
let pathClass = "p"
[<Literal>]
let leaveClass = "l"
[<Literal>]
let colorClass = "c"

[<Literal>]
let svgStyle =
        "<defs>
                <style>
                    ." + normalWallClass + " {
                        stroke: #333;
                        fill:transparent;
                        stroke-width: 1;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                        //stroke-dasharray: 5;
                    }
                    ." + borderWallClass + " {
                        stroke: #333;
                        fill:transparent;
                        stroke-width: 2;
                        stroke-linecap:round;
                        stroke-linejoin: round;
                        //stroke-dasharray: 5;
                    }
                    ." + pathClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: purple;
                        fill-opacity: 0.4;
                    }
                    ." + leaveClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: #70361f;
                        fill-opacity: 0.2;
                    }
                    ." + colorClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: #4287f5;
                    }
                </style>
            </defs>"

let round (f : float) =
    Math.Round(f, 2).ToString().Replace(",", ".")

let appendHeader width height (sBuilder : StringBuilder) =
    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>\n")
            .Append("<!-- Copyright 2020 Patrizio Amella. All rights reserved. See License at https://github.com/aPixelInSpace/F-a-maze-ing/blob/main/LICENSE for more information. -->\n")
            .Append("<svg width=\"" + width + "\" height=\"" + height + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\">")

let appendStyle (sBuilder : StringBuilder) =
    sBuilder.Append(svgStyle)

let appendBackground (color : string) (sBuilder : StringBuilder) =
    sBuilder.Append($"<rect width=\"100%%\" height=\"100%%\" fill=\"{color}\"/>")

let addPath (sBuilder : StringBuilder) styleClass (lines : string) =
    sBuilder.Append($"<path d=\"{lines}\" class=\"{styleClass}\"/>\n")

let addPathColor (sBuilder : StringBuilder) styleClass opacity (lines : string) =
    sBuilder.Append($"<path d=\"{lines}\" class=\"{styleClass}\" fill-opacity=\"{opacity}\"/>\n")

let appendFooter (sBuilder : StringBuilder) =
    sBuilder.Append("</svg>")

let appendCellColorWithDynamicOpacity lines distanceFromRoot maxDistance (sBuilder : StringBuilder) =
    let opacity = round (1.0 - (float (maxDistance - distanceFromRoot) / float maxDistance))
    let sOpacity = opacity.ToString().Replace(",", ".")

    addPathColor sBuilder colorClass sOpacity lines

let appendMazeColoration map wholeCellLines (sBuilder : StringBuilder) =
    let distanceFromRoot coordinate =
        match (map.ShortestPathGraph.NodeDistanceFromRoot coordinate) with
        | Some distance when distance = 0 -> 0
        | Some distance -> distance - 1
        | None -> 0
    map.ShortestPathGraph.Graph.Vertices
    |> Seq.iter(fun coordinate -> sBuilder |> appendCellColorWithDynamicOpacity (wholeCellLines coordinate) (distanceFromRoot coordinate) (map.FarthestFromRoot.Distance - 1) |> ignore)

    sBuilder

let appendPath path wholeCellLines (sBuilder : StringBuilder) =
    path
    |> Seq.iter(fun coordinate -> addPath sBuilder pathClass (wholeCellLines coordinate) |> ignore)

    sBuilder

let appendLeaves leaves wholeCellLines (sBuilder : StringBuilder) =
    leaves
    |> Seq.iter(fun coordinate -> addPath sBuilder leaveClass (wholeCellLines coordinate) |> ignore)

    sBuilder

let appendWalls cells appendWalls (sBuilder : StringBuilder) =
    cells
    |> Seq.iter(fun coordinate -> sBuilder |> appendWalls coordinate |> ignore)

    sBuilder