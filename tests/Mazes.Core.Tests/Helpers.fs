// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Helpers

open Mazes.Core.Canvas.Array2D.Shape

type TriangleIsoscelesBaseAtEnum =
    | Top = 1
    | Right = 2
    | Bottom = 3
    | Left = 4

type TBE = TriangleIsoscelesBaseAtEnum

let mapBaseAtEnumToBaseAt dirEnum =
    match dirEnum with
    | TBE.Top -> TriangleIsosceles.Top
    | TBE.Right -> TriangleIsosceles.Right
    | TBE.Bottom -> TriangleIsosceles.Bottom
    | TBE.Left -> TriangleIsosceles.Left
    | _ -> failwith "Triangle Isosceles Base at enumeration unknown"