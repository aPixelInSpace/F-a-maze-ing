// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.Array2D.TriangleIsosceles

open CommandLine
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.Array2D.Shape.TriangleIsosceles

type BaseAtEnum =
    | Bottom = 0
    | Top = 1
    | Left = 2
    | Right = 3

let mapToBaseAt baseAtEnum =
    match baseAtEnum with
    | BaseAtEnum.Bottom -> BaseAt.Bottom
    | BaseAtEnum.Top -> BaseAt.Top
    | BaseAtEnum.Left -> BaseAt.Left
    | BaseAtEnum.Right -> BaseAt.Right
    | _ -> failwith "Unknown base at"

[<Literal>]
let verb = "s-triangle"

[<Verb(verb, isDefault = false, HelpText = "Triangle isosceles shape")>]
type Options = {
    [<Option('b', "base", Required = true, HelpText = "The length of the base.")>] baseLength : int
    [<Option(Default = BaseAtEnum.Bottom, HelpText = "The position of the base (*Bottom, Top, Left or Right)." )>] baseAt : BaseAtEnum
    [<Option(Default = 1, HelpText = "The decrement value for the base.")>] baseDecrement : int
    [<Option(Default = 1, HelpText = "The increment value for the height.")>] heightIncrement : int
}

let handleVerb (options : Parsed<Options>) =
    TriangleIsosceles.create options.Value.baseLength (mapToBaseAt options.Value.baseAt) options.Value.baseDecrement options.Value.heightIncrement