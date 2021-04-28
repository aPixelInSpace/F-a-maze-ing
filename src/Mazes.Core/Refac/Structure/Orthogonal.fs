// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System.Text
open Mazes.Core.Refac
open Mazes.Core.Refac.Utils
open Mazes.Core.Refac.Array2D

module OrthoCellM =

    let value (OrthoCell c) = c
    
    let listOfPossiblePositionsCoordinates coordinate =
        [|
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }, OrthogonalDisposition OrthogonalDisposition.Left
            { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }, OrthogonalDisposition OrthogonalDisposition.Top
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }, OrthogonalDisposition OrthogonalDisposition.Right
            { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }, OrthogonalDisposition OrthogonalDisposition.Bottom
        |]

    let dispositions =
        seqOfUnionCases<OrthogonalDisposition>()

    let initialize (isCellPartOfMaze, neighborCoordinateAt, numberOfRows, numberOfColumns, internalConnectionState, coordinate) =
        let getConnectionState =
            ConnectionState.getConnectionState isCellPartOfMaze neighborCoordinateAt internalConnectionState coordinate

        let connectionState pos =
            match pos with
            | OrthogonalDisposition.Top -> getConnectionState (isFirstRow coordinate.RIndex) (OrthogonalDisposition OrthogonalDisposition.Top)
            | OrthogonalDisposition.Right -> getConnectionState (isLastColumn coordinate.CIndex numberOfColumns) (OrthogonalDisposition OrthogonalDisposition.Right)
            | OrthogonalDisposition.Bottom -> getConnectionState (isLastRow coordinate.RIndex numberOfRows) (OrthogonalDisposition OrthogonalDisposition.Bottom)
            | OrthogonalDisposition.Left -> getConnectionState (isFirstColumn coordinate.CIndex) (OrthogonalDisposition OrthogonalDisposition.Left)

        dispositions
        |> Seq.map(fun pos -> { State = (connectionState pos); Position = pos })
        |> Seq.toArray
        |> OrthoCell

module OrthogonalM =

    let weaveCoordinates coordinates =
        let filtered =
            coordinates
            |> Seq.filter(fun c -> c.RIndex % 2 = 0 && c.CIndex % 2 = 0)

        let vertical =
            filtered
            |> Seq.map(fun c -> (c, { RIndex = c.RIndex + 2; CIndex = c.CIndex }))
        
        let horizontal =
            filtered
            |> Seq.map(fun c -> (c, { RIndex = c.RIndex; CIndex = c.CIndex + 2 }))

        vertical
        |> Seq.append horizontal

    let toString connectionStateAtPosition (orthoCells : CellArray2D[,]) =
        let sBuilder = StringBuilder()
        let cells = orthoCells
        
        let get coordinate =
            match (get cells coordinate) with
            | CellArray2D.OrthoCellChoice c -> c
            | _ -> failwith "Incompatible cell"

        let appendHorizontalWall wallType =
            match wallType with
                | Close | ClosePersistent -> sBuilder.Append("_") |> ignore
                | Open -> sBuilder.Append(" ") |> ignore

        let appendVerticalWall wallType =
            match wallType with
                | Close | ClosePersistent -> sBuilder.Append("|") |> ignore
                | Open -> sBuilder.Append(" ") |> ignore

        // first row
        let lastColumnIndex = cells |> maxColumnIndex
        sBuilder.Append(" ") |> ignore
        for columnIndex in 0 .. lastColumnIndex do
            let cell = OrthoCellM.value (get { RIndex = 0; CIndex = columnIndex })
            appendHorizontalWall (connectionStateAtPosition cell OrthogonalDisposition.Top)
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
        for rowIndex in 0 .. cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = OrthoCellM.value (get { RIndex = rowIndex; CIndex = columnIndex })
                appendVerticalWall (connectionStateAtPosition cell OrthogonalDisposition.Left)
                appendHorizontalWall (connectionStateAtPosition cell OrthogonalDisposition.Bottom)
                
                if columnIndex = lastColumnIndex then
                    appendVerticalWall (connectionStateAtPosition cell OrthogonalDisposition.Right)

            sBuilder.Append("\n") |> ignore

        sBuilder.ToString()