// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Output.File

open CommandLine

[<Literal>]
let verb = "o-file"

[<Verb(verb, isDefault = false, HelpText = "Output to the file system")>]
type Options = {
    [<Option('p', "path", Required = true, HelpText = "The full path of the file")>] path : string
}

let handleVerb data (options : Parsed<Options>) =
    System.IO.File.WriteAllText(options.Value.path, data)
    
    // todo : delete this, it is for testing purpose only
    let grid =
        (Mazes.Core.Refac.Canvas.Array2D.Shape.Rectangle.create 7 10)
        |> Mazes.Core.Refac.Structure.Grid.createBaseGrid (Mazes.Core.Refac.Structure.GridStructure.createArray2DOrthogonal())
        |> Mazes.Core.Refac.Structure.NDimensionalStructure.create2D

    let maze = grid |> Mazes.Core.Refac.Maze.Generate.AldousBroder.createMaze 1
    
    let globalParams =
        {
            Mazes.Render.Refac.SVG.GlobalOptions.Parameters.Default with
                WallRenderType = Mazes.Render.Refac.SVG.GlobalOptions.Inset
                LineType = Mazes.Render.Refac.SVG.GlobalOptions.Circle |> Mazes.Render.Refac.SVG.GlobalOptions.Arc
                BackgroundColoration = Mazes.Render.Refac.SVG.GlobalOptions.Distance 
        }
    let gridParams = Mazes.Render.Refac.SVG.OrthoGrid.Parameters.CreateDefaultSquare |> Mazes.Render.Refac.SVG.Creation.OrthoParameters
    
    let map = Mazes.Core.Refac.Maze.Maze.createMap (Mazes.Core.Refac.Structure.NDimensionalStructure.firstCellPartOfMaze maze.NDStruct) maze
    
    let svg = Mazes.Render.Refac.SVG.Creation.render globalParams gridParams maze.NDStruct (Some map)
    
    System.IO.File.WriteAllText(options.Value.path.Replace(".svg", "-refac.svg"), svg)