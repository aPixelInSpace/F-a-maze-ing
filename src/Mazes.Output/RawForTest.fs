// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Output.RawForTest

open System.Text

let outputRawForTest maze (textRenderedMaze : string) =
    let sbMaze = StringBuilder()
    
    textRenderedMaze.Split("\n")
    |> Array.iter(fun mazeTextRow ->
                    sbMaze
                        .Append("\"")
                        .Append(mazeTextRow)
                        .Append("\\n\" +\n")
                    |> ignore)

    sbMaze.ToString()