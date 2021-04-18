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
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }, Orthogonal OrthogonalDisposition.Left
            { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }, Orthogonal OrthogonalDisposition.Top
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }, Orthogonal OrthogonalDisposition.Right
            { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }, Orthogonal OrthogonalDisposition.Bottom
        |]

    let neighborCoordinateAt coordinate position =
        listOfPossiblePositionsCoordinates coordinate
        |> Array.tryFind(fun pc -> (snd pc) = position)
        |> Option.map(fst)

    let create isCellPartOfMaze numberOfRows numberOfColumns internalConnectionState (coordinate : Coordinate2D) =
        let getConnectionState =
            ConnectionState.getConnectionState isCellPartOfMaze neighborCoordinateAt internalConnectionState coordinate

        let connectionState pos =
            match pos with
            | OrthogonalDisposition.Top -> getConnectionState (isFirstRow coordinate.RIndex) (Orthogonal OrthogonalDisposition.Top)
            | OrthogonalDisposition.Right -> getConnectionState (isLastColumn coordinate.CIndex numberOfColumns) (Orthogonal OrthogonalDisposition.Right)
            | OrthogonalDisposition.Bottom -> getConnectionState (isLastRow coordinate.RIndex numberOfRows) (Orthogonal OrthogonalDisposition.Bottom)
            | OrthogonalDisposition.Left -> getConnectionState (isFirstColumn coordinate.CIndex) (Orthogonal OrthogonalDisposition.Left)

        [|
           for pos in seqOfUnionCases<OrthogonalDisposition>() do
               { State = (connectionState pos); Position = pos }
        |]
        |> OrthoCell

    let newCellWithStateAtPosition cell connectionState position =
         (value cell)
         |> Array.map(fun c -> if c.Position = position then { State = connectionState; Position = position } else c)
         |> OrthoCell

    let connectionStateAtPosition c position =
         (value c
         |> Array.find(fun c ->  c.Position = position)).State

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

module OrthogonalM =

    let toString (orthoCells : CellArray2D[,]) =
        let sBuilder = StringBuilder()
        let cells = orthoCells
        
        let get coordinate =
            match (get cells coordinate) with
            | CellArray2D.OrthoCellChoice c -> c
            | _ -> failwith "Incompatible cell"
        
        let connectionTypeAtPosition = OrthoCellM.connectionStateAtPosition

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
            let cell =  get { RIndex = 0; CIndex = columnIndex }
            appendHorizontalWall (connectionTypeAtPosition cell OrthogonalDisposition.Top)
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
        for rowIndex in 0 .. cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = get { RIndex = rowIndex; CIndex = columnIndex }
                appendVerticalWall (connectionTypeAtPosition cell OrthogonalDisposition.Left)
                appendHorizontalWall (connectionTypeAtPosition cell OrthogonalDisposition.Bottom)
                
                if columnIndex = lastColumnIndex then
                    appendVerticalWall (connectionTypeAtPosition cell OrthogonalDisposition.Right)

            sBuilder.Append("\n") |> ignore

        sBuilder.ToString()