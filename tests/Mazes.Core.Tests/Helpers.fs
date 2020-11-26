// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Helpers

open Mazes.Core.Grid

type DirectionEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

let mapDirectionEnumToDirection dirEnum =
    match dirEnum with
    | DirectionEnum.Top -> Position.Top
    | DirectionEnum.Right -> Position.Right
    | DirectionEnum.Bottom -> Position.Bottom
    | DirectionEnum.Left -> Position.Left
    | _ -> failwith "Direction enumeration unknown"

let getValue anOption =
    match anOption with
    | Some anOption -> anOption
    | None -> failwith "There should be some option"