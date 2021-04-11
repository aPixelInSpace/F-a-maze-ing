// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac
open Mazes.Core.Refac.Utils
open Mazes.Core.Refac.Array2D

[<Struct>]
type OrthoCell = OrthoCell of Connection<OrthogonalDisposition> array

module OrthoCell =

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

    let create numberOfRows numberOfColumns internalConnectionState (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getConnectionState isOnEdge position =
            if isOnEdge then
                ConnectionState.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (neighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionState.getConnectionTypeForInternal internalConnectionState isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a connection type for the neighbor {coordinate} at {position}"

        let connectionState pos =
            match pos with
            | OrthogonalDisposition.Top -> getConnectionState (isFirstRow coordinate.RIndex) (Orthogonal OrthogonalDisposition.Top)
            | OrthogonalDisposition.Right -> getConnectionState (isLastColumn coordinate.CIndex numberOfColumns) (Orthogonal OrthogonalDisposition.Right)
            | OrthogonalDisposition.Bottom -> getConnectionState (isLastRow coordinate.RIndex numberOfRows) (Orthogonal OrthogonalDisposition.Bottom)
            | OrthogonalDisposition.Left -> getConnectionState (isFirstColumn coordinate.CIndex) (Orthogonal OrthogonalDisposition.Left)

        [| for pos in seqOfUnionCases<OrthogonalDisposition>() do
               { State = (connectionState pos); Position = pos } |]
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