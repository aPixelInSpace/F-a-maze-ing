// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Output.Console

open System
open CommandLine

[<Literal>]
let verb = "o-console"

[<Verb(verb, isDefault = false, HelpText = "Output to the console")>]
type Options = {
    [<Option()>] noOption : int
}

let handleVerb (data : string) _ =
    Console.Write(data)