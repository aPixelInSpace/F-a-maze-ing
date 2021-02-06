// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Render.SVG.Base

open System
open System.Text
open Mazes.Core
open Mazes.Core.Trigonometry
open Mazes.Core.Analysis.Dijkstra

[<Literal>]
let debugShowCoordinate = false

[<Literal>]
let showClosedBridge = false

// #4287f5 : blue
// #63a873 : green
// #e6da00 : yellow
// #fffaba : pale yellow
// #fffef0 : very pale yellow
// #f74525 : orange
// #4a74e8 : blue path
// #2d195e : mauve

[<Literal>]
let elementIdPrefix = "eip"
[<Literal>]
let animationIdPrefix = "aip"
[<Literal>]
let normalWallClass = "nwc"
[<Literal>]
let borderWallClass = "bwc"
[<Literal>]
let normalWallInsetBackClass = "nwibc"
[<Literal>]
let normalWallInsetForeClass = "nwifc"
[<Literal>]
let borderWallInsetBackClass = "bwibc"
[<Literal>]
let borderWallInsetForeClass = "bwifc"
[<Literal>]
let normalWallBridgeClass = "nwbc"
[<Literal>]
let pathClass = "pc"
[<Literal>]
let pathAnimatedClass = "pac"
[<Literal>]
let leaveClass = "lc"
[<Literal>]
let colorClass = "cc"
[<Literal>]
let colorDistanceClass = "cdc"
[<Literal>]
let pathOpacity = "0.4"

[<Literal>]
let svgStyle =
        "<defs>
                <style>
                    ." + normalWallClass + " {
                        stroke: #333;
                        fill: transparent;
                        stroke-width: 1;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + borderWallClass + " {
                        stroke: #333;
                        fill: transparent;
                        stroke-width: 2;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + normalWallInsetBackClass + " {
                        fill: transparent;
                        stroke: #333;
                        stroke-width: 10;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + normalWallInsetForeClass + " {
                        stroke: white;
                        stroke-width: 8;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + borderWallInsetBackClass + " {
                        stroke: #333;
                        fill: transparent;
                        stroke-width: 10;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + borderWallInsetForeClass + " {
                        stroke: white;
                        fill: transparent;
                        stroke-width: 4;
                        stroke-linecap: round;
                        stroke-linejoin: round;
                    }
                    ." + normalWallBridgeClass + " {
                        stroke: #333;
                        fill: transparent;
                        stroke-width: 2;
                        //stroke-linecap: round;
                        //stroke-linejoin: round;
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
                    ." + colorDistanceClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: #4287f5;
                    }
                    ." + colorClass + " {
                        stroke: transparent;
                        stroke-width: 0;
                        fill: white;
                    }
                </style>
            </defs>"

let round (f : float) =
    Math.Round(f, 2).ToString().Replace(",", ".")

let appendHeader width height (sBuilder : StringBuilder) =
    sBuilder.Append("<?xml version=\"1.0\" standalone=\"no\"?>\n")
            .Append("<!-- Copyright 2020-2021 Patrizio Amella. All rights reserved. See License at https://github.com/aPixelInSpace/F-a-maze-ing/blob/main/LICENSE for more information. -->\n")
            .Append("<svg width=\"" + width + "\" height=\"" + height + "\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">")

let appendStyle (sBuilder : StringBuilder) =
    sBuilder.Append(svgStyle)

let appendBackground (color : string) (sBuilder : StringBuilder) =
    sBuilder.Append($"<rect width=\"100%%\" height=\"100%%\" fill=\"{color}\"/>")

let appendPathElement (sBuilder : StringBuilder) id styleClass lines coordinate =
    sBuilder.Append("<path ") |> ignore

    match id with
    | Some id -> sBuilder.Append($"id=\"{elementIdPrefix}{id}\" ") |> ignore
    | None -> ()

    sBuilder.Append($"d=\"{lines}\" class=\"{styleClass}\"") |> ignore

    if debugShowCoordinate then
        sBuilder.Append($"><title>RIndex {coordinate.RIndex}; CIndex {coordinate.CIndex}</title></path>") |> ignore
    else
        sBuilder.Append($"/>") |> ignore

    sBuilder.Append($"\n")

let appendPathBridge (sBuilder : StringBuilder) id styleClass lines =
    sBuilder.Append("<path ") |> ignore

    match id with
    | Some id -> sBuilder.Append($"id=\"{elementIdPrefix}{id}\" ") |> ignore
    | None -> ()

    sBuilder.Append($"d=\"{lines}\" class=\"{styleClass}\"/>") |> ignore

    sBuilder.Append($"\n")

let appendWall (sBuilder : StringBuilder) lines (wallType : ConnectionType) coordinate =
    match wallType with
    | Close -> appendPathElement sBuilder None normalWallClass lines coordinate
    | ClosePersistent -> appendPathElement sBuilder None borderWallClass lines coordinate
    | Open -> sBuilder

let appendNormalWallBackInset (sBuilder : StringBuilder) lines (wallType : ConnectionType) coordinate =
    match wallType with
    | Close -> appendPathElement sBuilder None normalWallInsetBackClass lines coordinate
    | _ -> sBuilder

let appendNormalWallForeInset (sBuilder : StringBuilder) lines (wallType : ConnectionType) coordinate =
    match wallType with
    | Close -> appendPathElement sBuilder None normalWallInsetForeClass lines coordinate
    | _ -> sBuilder

let appendBorderWallBackInset (sBuilder : StringBuilder) lines (wallType : ConnectionType) coordinate =
    match wallType with
    | ClosePersistent -> appendPathElement sBuilder None borderWallInsetBackClass lines coordinate
    | _ -> sBuilder

let appendBorderWallForeInset (sBuilder : StringBuilder) lines (wallType : ConnectionType) coordinate =
    match wallType with
    | ClosePersistent -> appendPathElement sBuilder None borderWallInsetForeClass lines coordinate
    | _ -> sBuilder

let appendPathElementColor (sBuilder : StringBuilder) styleClass opacity (lines : string) coordinate =
    sBuilder.Append($"<path d=\"{lines}\" class=\"{styleClass}\" fill-opacity=\"{opacity}\"") |> ignore
    if debugShowCoordinate then
        match coordinate with
        | Some coordinate -> sBuilder.Append($"><title>RIndex {coordinate.RIndex}; CIndex {coordinate.CIndex}</title></path>") |> ignore
        | None -> sBuilder.Append($"/>") |> ignore
     else
        sBuilder.Append($"/>") |> ignore

    sBuilder.Append($"\n")

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

let appendCellColorWithDynamicOpacity lines distanceFromRoot maxDistance coordinate (sBuilder : StringBuilder) =
    let opacity = round (1.0 - (float (maxDistance - distanceFromRoot) / float maxDistance))
    let sOpacity = opacity.ToString().Replace(",", ".")

    appendPathElementColor sBuilder colorDistanceClass sOpacity lines coordinate

let appendMazeDistanceColoration map wholeCellLines (sBuilder : StringBuilder) =
    let distanceFromRoot coordinate =
        match (map.ShortestPathGraph.NodeDistanceFromRoot coordinate) with
        | Some distance when distance = 0 -> 0
        | Some distance -> distance - 1
        | None -> 0
    map.ShortestPathGraph.Graph.Vertices
    |> Seq.iter(fun coordinate -> sBuilder |> appendCellColorWithDynamicOpacity (wholeCellLines coordinate) (distanceFromRoot coordinate) (map.FarthestFromRoot.Distance - 1) (Some coordinate) |> ignore)

    sBuilder

let appendMazeDistanceBridgeColoration sequence wholeBridgeLines distanceFromRoot maxDistanceFromRoot (sBuilder : StringBuilder) =
    let distanceFromRoot coordinate =
        match (distanceFromRoot coordinate) with
        | Some distance when distance = 0 -> 0
        | Some distance -> distance - 1
        | None -> 0

    sequence
    |> Seq.iter(fun (fromCoordinate, toCoordinate, wallType) ->
        match wallType, showClosedBridge with
        | Open, _ | Close, true ->
            sBuilder |> appendCellColorWithDynamicOpacity (wholeBridgeLines fromCoordinate toCoordinate) (distanceFromRoot fromCoordinate) (maxDistanceFromRoot - 1) None |> ignore
        | _ -> ())

    sBuilder

let appendMazeColoration sequence wholeCellLines (sBuilder : StringBuilder) =
    sequence
    |> Seq.iter(fun coordinate -> appendPathElementColor sBuilder colorClass "1" (wholeCellLines coordinate) (Some coordinate) |> ignore)

    sBuilder

let appendMazeBridgeColoration sequence wholeBridgeLines (sBuilder : StringBuilder) =
    sequence
    |> Seq.iter(fun (fromCoordinate, toCoordinate, wallType) ->
        match wallType, showClosedBridge with
        | Open, _ | Close, true ->
            appendPathElementColor sBuilder colorClass "1" (wholeBridgeLines fromCoordinate toCoordinate) None |> ignore
        | _ -> ())

    sBuilder

let appendPath path wholeCellLines (sBuilder : StringBuilder) =
    path
    |> Seq.iter(fun coordinate -> appendPathElement sBuilder None pathClass (wholeCellLines coordinate) coordinate |> ignore)

    sBuilder

let appendPathWithAnimation path wholeCellLines (sBuilder : StringBuilder) =
    path
    |> Seq.iteri(
        fun i coordinate ->
            appendPathElement sBuilder (Some i) pathAnimatedClass (wholeCellLines coordinate) coordinate |> ignore
            let related = if i > 0 then Some ((i - 1).ToString()) else None 
            appendAnimationElement sBuilder (i.ToString()) related |> ignore)

    sBuilder

let appendPathAndBridgesWithAnimation path wholeCellLines existBridge wholeBridgeLines (sBuilder : StringBuilder) =
    let mutable previous = None
    path
    |> Seq.iteri(
        fun i coordinate ->
            let i = i * 2
            let related = if i > 0 then Some ((i - 2).ToString()) else None
            
            appendPathElement sBuilder (Some i) pathAnimatedClass (wholeCellLines coordinate) coordinate |> ignore
            appendAnimationElement sBuilder (i.ToString()) related |> ignore
            
            match previous with
            | Some previousCoordinate ->
                if existBridge previousCoordinate coordinate then
                    appendPathElement sBuilder (Some (i - 1)) pathAnimatedClass (wholeBridgeLines previousCoordinate coordinate) coordinate |> ignore
                    appendAnimationElement sBuilder ((i - 1).ToString()) related |> ignore
            | None -> ()

            previous <- Some coordinate)

    sBuilder

let appendLeaves leaves wholeCellLines (sBuilder : StringBuilder) =
    leaves
    |> Seq.iter(fun coordinate -> appendPathElement sBuilder None leaveClass (wholeCellLines coordinate) coordinate |> ignore)

    sBuilder

let appendBaseWalls sequence walls (sBuilder : StringBuilder) =
    sequence
    |> Seq.iter(fun item -> sBuilder |> walls item |> ignore)

    sBuilder

let appendSimpleWalls sequence cellsWithWall sBuilder =
    let appendSimpleWalls = cellsWithWall appendWall

    sBuilder
    |> appendBaseWalls sequence appendSimpleWalls

let appendWallsWithInset sequence cellsWithWall sBuilder =
    let appendWallsTypeNormalBack = cellsWithWall appendNormalWallBackInset
    let appendWallsTypeNormalFore = cellsWithWall appendNormalWallForeInset
    let appendWallsTypeBorderBack = cellsWithWall appendBorderWallBackInset
    let appendWallsTypeBorderFore = cellsWithWall appendBorderWallForeInset

    sBuilder
    |> appendBaseWalls sequence appendWallsTypeNormalBack
    |> appendBaseWalls sequence appendWallsTypeNormalFore
    |> appendBaseWalls sequence appendWallsTypeBorderBack
    |> appendBaseWalls sequence appendWallsTypeBorderFore

let calculatePointsBridge center bridgeHalfWidth bridgeDistanceFromCenter fromCoordinate toCoordinate =
    let centerFrom = center fromCoordinate
    let centerTo = center toCoordinate
    let angle = calculateAngle centerFrom centerTo
    let angle180 = convertToRadian 180.0

    let centerFrom = calculatePoint centerFrom (-angle) bridgeDistanceFromCenter
    
    let angleCenterTo = angle180 - angle
    let centerTo = calculatePoint centerTo (angleCenterTo) bridgeDistanceFromCenter

    let angle90 = convertToRadian 90.0
    let angleMin = -(angle - angle90)
    let angleMax = -(angle + angle90)

    let leftFromBridge = calculatePoint centerFrom angleMin bridgeHalfWidth
    let rightFromBridge = calculatePoint centerFrom angleMax bridgeHalfWidth
    let leftToBridge = calculatePoint centerTo angleMin bridgeHalfWidth
    let rightToBridge = calculatePoint centerTo angleMax bridgeHalfWidth

    (leftFromBridge, rightFromBridge, leftToBridge, rightToBridge)

let wholeBridgeLines calculatePointsBridge fromCoordinate toCoordinate =
    let ((leftFromX, leftFromY), (rightFromX, rightFromY), (leftToX, leftToY), (rightToX, rightToY)) =
        calculatePointsBridge fromCoordinate toCoordinate

    $"M {round leftFromX} {round leftFromY} " +
    $"L {round rightFromX} {round rightFromY} " +
    $"L {round rightToX} {round rightToY} " +
    $"L {round leftToX} {round leftToY} "

let appendSimpleBridges calculatePointsBridge (bridges : (Coordinate * Coordinate * ConnectionType) seq) (sBuilder : StringBuilder) =
    bridges
    |> Seq.iter(fun (fromCoordinate, toCoordinate, wallType) ->
            match wallType, showClosedBridge with
            | Open, _ | Close, true ->
                let ((leftFromX, leftFromY), (rightFromX, rightFromY), (leftToX, leftToY), (rightToX, rightToY)) = calculatePointsBridge fromCoordinate toCoordinate
                appendPathBridge sBuilder None normalWallBridgeClass $"M {round leftFromX} {round leftFromY} L {round leftToX} {round leftToY}" |> ignore
                appendPathBridge sBuilder None normalWallBridgeClass $"M {round rightFromX} {round rightFromY} L {round rightToX} {round rightToY}" |> ignore
            | _ -> ())
            
    sBuilder

let appendSimpleWallsBridges calculatePointsBridge (bridges : (Coordinate * Coordinate * ConnectionType) seq) (sBuilder : StringBuilder) =
    bridges
    |> Seq.iter(fun (fromCoordinate, toCoordinate, wallType) ->
            let ((leftFromX, leftFromY), (rightFromX, rightFromY), (leftToX, leftToY), _) = calculatePointsBridge fromCoordinate toCoordinate
            match wallType, showClosedBridge with
            | Close, true ->
                let distance = calculateDistance (leftFromX, leftFromY) (leftToX, leftToY)
                let angle = -(calculateAngle (leftFromX, leftFromY) (leftToX, leftToY))
                let (leftPointX, leftPointY) = calculatePoint (leftFromX, leftFromY) angle (distance / 2.0)
                let (rightPointX, rightPointY) = calculatePoint (rightFromX, rightFromY) angle (distance / 2.0)
                appendWall sBuilder $"M {round leftPointX} {round leftPointY} L {round rightPointX} {round rightPointY}" wallType fromCoordinate |> ignore
            | _ -> ())

    sBuilder