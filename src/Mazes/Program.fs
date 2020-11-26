// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

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
