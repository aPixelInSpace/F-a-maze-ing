// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Helpers

open Mazes.Core.Grid
open Mazes.Core.Canvas.Shape

type SidewinderDirectionEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

let mapDirectionEnumToDirection dirEnum =
    match dirEnum with
    | SidewinderDirectionEnum.Top -> Position.Top
    | SidewinderDirectionEnum.Right -> Position.Right
    | SidewinderDirectionEnum.Bottom -> Position.Bottom
    | SidewinderDirectionEnum.Left -> Position.Left
    | _ -> failwith "Direction enumeration unknown"

type TriangleIsoscelesBaseAtEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

let mapBaseAtEnumToBaseAt dirEnum =
    match dirEnum with
    | SidewinderDirectionEnum.Top -> TriangleIsosceles.Top
    | SidewinderDirectionEnum.Right -> TriangleIsosceles.Right
    | SidewinderDirectionEnum.Bottom -> TriangleIsosceles.Bottom
    | SidewinderDirectionEnum.Left -> TriangleIsosceles.Left
    | _ -> failwith "Base at enumeration unknown"

let getValue anOption =
    match anOption with
    | Some anOption -> anOption
    | None -> failwith "There should be some option"