// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Output.Html

open System.IO
open System.Text
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D
open Mazes.Core.Structure.Grid2D.Type.Ortho
open Mazes.Core.Maze

let private columnsAxis (columnsAxisHtml : string) numberOfRows numberOfColumns =
    let numberWhiteSpacesPrefixForColumns =
        numberOfRows
            .ToString()
            // + 1 white space for the space between the number of the row and the maze
            // + 1 white space because the maze always start with an intersection and not directly a column 
            .Length + 2

    let totalOfDigitsForColumns = numberOfColumns.ToString().Length

    let sbColumnsRow = [| for _ in 1 .. totalOfDigitsForColumns -> StringBuilder()  |]

    for sb in sbColumnsRow do
        sb.Append(String.replicate numberWhiteSpacesPrefixForColumns " ") |> ignore

    for column in 0 .. numberOfColumns do
        let columnString = column.ToString()
        let digitLength = columnString.Length
        let offset = totalOfDigitsForColumns - digitLength

        for sbIndex in 0 .. sbColumnsRow.Length - 1 do
            match sbIndex with
            | sbIndex when (totalOfDigitsForColumns - sbIndex) > digitLength -> sbColumnsRow.[sbIndex].Append("  ") |> ignore
            | sbIndex ->
                sbColumnsRow.[sbIndex]
                    .Append(columnString.[sbIndex - offset])
                    .Append(" ")
                    |> ignore

    let sbColumns = StringBuilder()
    for sbColumnRow in sbColumnsRow do
        sbColumns.Append(columnsAxisHtml.Replace("{{ColumnsNumbers}}", sbColumnRow.ToString())) |> ignore

    sbColumns.ToString()

let private rowNumber numberOfRows rowIndex =
    let numberWhiteSpacesPrefixForRow numberOfRows rowNumber = numberOfRows.ToString().Length - rowNumber.ToString().Length

    if rowIndex < numberOfRows then  
        String.replicate (numberWhiteSpacesPrefixForRow numberOfRows rowIndex) " "
        + rowIndex.ToString()
        + " "
    else
        String.replicate (rowIndex.ToString().Length) " "
        + " "

let outputHtml (maze : IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>) mazeInfo (textRenderedMaze : string) =
    let resourcesDir = Path.Combine(Directory.GetCurrentDirectory(), "Output.Resources/")

    let mainHtml = File.ReadAllText(Path.Combine(resourcesDir, "Main.html-template"))
    let mazeLineHtml = File.ReadAllText(Path.Combine(resourcesDir, "MazeLine.html-template"))
    let columnsAxisHtml = File.ReadAllText(Path.Combine(resourcesDir, "ColumnsAxis.html-template"))

    let sbMaze = StringBuilder()

    let spGrid = maze.ToSpecializedStructure

    textRenderedMaze.Split("\n")
    |> Array.iteri(fun rowIndex mazeTextRow ->
                    let row = mazeLineHtml
                                  .Replace("{{MazeRow}}", mazeTextRow)
                                  .Replace("{{MazeRowNumber}}", rowNumber spGrid.NumberOfRows rowIndex)
                    sbMaze.Append(row) |> ignore)

    let columnsAxisHtml = columnsAxis columnsAxisHtml (getIndex spGrid.NumberOfRows) (getIndex spGrid.NumberOfColumns)

    let mainHtml = mainHtml
                       .Replace("{{Name}}", mazeInfo.Name)
                       .Replace("{{Maze}}", sbMaze.ToString())
                       .Replace("{{ColumnsAxis}}", columnsAxisHtml)

    mainHtml