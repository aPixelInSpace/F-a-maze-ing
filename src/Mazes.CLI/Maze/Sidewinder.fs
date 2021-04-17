// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Maze.Sidewinder

open System
open CommandLine
open Mazes.Core.Maze.Generate
open Mazes.Core.Maze.Generate.Sidewinder

type DirectionEnum =
    | Top = 0
    | Right = 1
    | Bottom = 2
    | Left = 3

let mapToDirection baseAtEnum =
    match baseAtEnum with
    | DirectionEnum.Bottom -> Direction.Bottom
    | DirectionEnum.Top -> Direction.Top
    | DirectionEnum.Left -> Direction.Left
    | DirectionEnum.Right -> Direction.Right
    | _ -> failwith "Unknown direction"

[<Literal>]
let verb = "a-sw"

[<Verb(verb, isDefault = false, HelpText = "Sidewinder algorithm")>]
type Options = {
    [<Option('s', "seed", Required = false, HelpText = "RNG seed, if none is provided a random one is chosen")>] seed : int option
    [<Option(Default = DirectionEnum.Top, HelpText = "First direction (Top)")>] direction1 : DirectionEnum
    [<Option(Default = DirectionEnum.Right, HelpText = "Second direction (Right)")>] direction2 : DirectionEnum
    [<Option(Default = 1, HelpText = "Weight for the direction 1")>] direction1Weight : int
    [<Option(Default = 1, HelpText = "Weight for the direction 2")>] direction2Weight : int
}

let handleVerb ndStruct (options : Parsed<Options>) =
    let seed =
        match options.Value.seed with
        | Some seed -> seed
        | None -> (Random()).Next()

    ndStruct |> Sidewinder.createMaze (mapToDirection options.Value.direction1) (mapToDirection options.Value.direction2) seed options.Value.direction1Weight options.Value.direction2Weight