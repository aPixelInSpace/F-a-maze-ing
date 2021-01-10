// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.Base

open System
open System.Text
open Mazes.Core
open Mazes.Core.Analysis.Dijkstra

// #4287f5 : blue
// #63a873 : green
// #e6da00 : yellow
// #fffaba : pale yellow
// #fffef0 : very pale yellow
// #f74525 : orange
// #4a74e8 : blue path
// #2d195e : mauve

[<Literal>]
let elementIdPrefix = "p"
[<Literal>]
let animationIdPrefix = "a"
[<Literal>]
let normalWallClass = "n"
[<Literal>]
let borderWallClass = "b"
[<Literal>]
let pathClass = "p"
[<Literal>]
let pathAnimatedClass = "pa"
[<Literal>]
let leaveClass = "l"
[<Literal>]
let colorClass = "c"
[<Literal>]
let pathOpacity = "0.4"

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
                        fill-opacity: " + pathOpacity + ";
                    }
                    ." + pathAnimatedClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: purple;
                        fill-opacity: 0.0;
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

let convertToRadian angleInDegree =
    (angleInDegree * Math.PI) / 180.0

/// Returns the point situated at the matching angle and distance from the base point
let calculatePoint (baseX, baseY) angleInRadian distance =
    (distance * Math.Cos(angleInRadian) + baseX, distance * Math.Sin(angleInRadian) + baseY)

let appendHeader width height (sBuilder : StringBuilder) =
    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>\n")
            .Append("<!-- Copyright 2020-2021 Patrizio Amella. All rights reserved. See License at https://github.com/aPixelInSpace/F-a-maze-ing/blob/main/LICENSE for more information. -->\n")
            .Append("<svg width=\"" + width + "\" height=\"" + height + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">")

let appendStyle (sBuilder : StringBuilder) =
    sBuilder.Append(svgStyle)

let appendBackground (color : string) (sBuilder : StringBuilder) =
    sBuilder.Append($"<rect width=\"100%%\" height=\"100%%\" fill=\"{color}\"/>")

let appendPathElement (sBuilder : StringBuilder) id styleClass lines =
    sBuilder.Append("<path ") |> ignore

    match id with
    | Some id -> sBuilder.Append($"id=\"{elementIdPrefix}{id}\" ") |> ignore
    | None -> ()

    sBuilder.Append($"d=\"{lines}\" class=\"{styleClass}\"/>\n")

let appendWall (sBuilder : StringBuilder) lines (wallType : WallType) =
    match wallType with
    | Normal -> appendPathElement sBuilder None normalWallClass lines
    | Border -> appendPathElement sBuilder None borderWallClass lines
    | Empty -> sBuilder

let appendPathElementColor (sBuilder : StringBuilder) styleClass opacity (lines : string) =
    sBuilder.Append($"<path d=\"{lines}\" class=\"{styleClass}\" fill-opacity=\"{opacity}\"/>\n")

let appendAnimationElement (sBuilder : StringBuilder) id relatedId =
    let elementId = elementIdPrefix + id
    let animationId = animationIdPrefix + elementId

    sBuilder.Append($"<animate id=\"{animationId}\" xlink:href=\"#{elementId}\" attributeName=\"fill-opacity\" fill=\"freeze\" from=\"0\" to=\"{pathOpacity}\" dur=\"0.02s\" ") |> ignore

    match relatedId with
    | Some relatedId ->
        let relatedAnimationId = animationIdPrefix + elementIdPrefix + relatedId
        sBuilder.Append($"begin=\"{relatedAnimationId}.end\"") |> ignore
    | None -> ()

    sBuilder.Append($"/>\n")

let appendFooter (sBuilder : StringBuilder) =
    sBuilder.Append("</svg>")

let appendCellColorWithDynamicOpacity lines distanceFromRoot maxDistance (sBuilder : StringBuilder) =
    let opacity = round (1.0 - (float (maxDistance - distanceFromRoot) / float maxDistance))
    let sOpacity = opacity.ToString().Replace(",", ".")

    appendPathElementColor sBuilder colorClass sOpacity lines

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
    |> Seq.iter(fun coordinate -> appendPathElement sBuilder None pathClass (wholeCellLines coordinate) |> ignore)

    sBuilder

let appendPathWithAnimation  path wholeCellLines (sBuilder : StringBuilder) =
    path
    |> Seq.iteri(
        fun i coordinate ->
            appendPathElement sBuilder (Some i) pathAnimatedClass (wholeCellLines coordinate) |> ignore
            let related = if i > 0 then Some ((i - 1).ToString()) else None 
            appendAnimationElement sBuilder (i.ToString()) related |> ignore)

    sBuilder

let appendLeaves leaves wholeCellLines (sBuilder : StringBuilder) =
    leaves
    |> Seq.iter(fun coordinate -> appendPathElement sBuilder None leaveClass (wholeCellLines coordinate) |> ignore)

    sBuilder

let appendWalls sequence walls (sBuilder : StringBuilder) =
    sequence
    |> Seq.iter(fun item -> sBuilder |> walls item |> ignore)

    sBuilder