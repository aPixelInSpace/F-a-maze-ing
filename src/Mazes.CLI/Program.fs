// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

// Learn more about F# at http://fsharp.org

open System
open CommandLine
open Mazes.CLGenerate


[<EntryPoint>]
let main argv =
    
    let result = Parser.Default.ParseArguments<GenerateOptions>(argv)
    match result with
    | :? Parsed<GenerateOptions> as parsed -> parsed |> handleVerbGenerate
    | :? NotParsed<GenerateOptions> as notParsed -> printfn "%s" (String.Join(",", (notParsed.Errors |> Seq.map(fun e -> e.ToString()))))
    | _ -> printfn "Something went wrong !"
    
    0 // return an integer exit code
