module Mazes.Output.RawForTest

open System.Text
open System.IO

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