// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Type.Ortho

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid

type OrthoPosition =
    | Left
    | Top
    | Right
    | Bottom

type OrthoPositionHandler private () =

    static let instance = OrthoPositionHandler()

    interface IPositionHandler<OrthoPosition> with

        member this.Opposite _ position =
            match position with
            | Left -> Right
            | Top -> Bottom
            | Right -> Left
            | Bottom -> Top

        member this.Values _ =
            [| Left; Top; Right; Bottom |]

        member this.Map _ position =
            match position with
            | Position.Left -> Left
            | Position.Top -> Top
            | Position.Right -> Right
            | Position.Bottom -> Bottom

    member this.ToInterface =
        this :> IPositionHandler<OrthoPosition>

    static member Instance =
        instance.ToInterface

type OrthoCoordinateHandler private () =

    static let instance = OrthoCoordinateHandler()

    interface ICoordinateHandler<OrthoPosition> with

        member this.NeighborCoordinateAt coordinate position =
            match position with
            | OrthoPosition.Top ->  Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Right -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | OrthoPosition.Bottom -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | OrthoPosition.Left -> Some  { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            OrthoPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

        member this.WeaveCoordinates coordinates =
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

    member this.ToInterface =
        this :> ICoordinateHandler<OrthoPosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type OrthoCell =
    private
        { Connections : Connection<OrthoPosition> array }

    interface ICell<OrthoPosition> with
        member this.Create connections =
            ({ Connections = connections } :> ICell<OrthoPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[OrthoCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<OrthoPosition>

    static member ConnectionPositionIndex position =
        match position with
        | Left -> 0
        | Top -> 1
        | Right -> 2
        | Bottom -> 3

    static member Create numberOfRows numberOfColumns internalConnectionType (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getConnectionType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalConnectionType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a connection type for the neighbor {coordinate} at {position}"

        let connectionType pos =
            match pos with
            | Top -> getConnectionType (isFirstRow coordinate.RIndex) Top
            | Right -> getConnectionType (isLastColumn coordinate.CIndex numberOfColumns) Right
            | Bottom -> getConnectionType (isLastRow coordinate.RIndex numberOfRows) Bottom
            | Left -> getConnectionType (isFirstColumn coordinate.CIndex) Left

        {
            Connections =
                [| for pos in OrthoPositionHandler.Instance.Values coordinate do
                       { ConnectionType = (connectionType pos); ConnectionPosition = pos } |]                
        }.ToInterface

module Grid =

    let toString (maze : IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>) =
        let sBuilder = StringBuilder()
        let cells = maze.ToSpecializedStructure.Cells
        let connectionTypeAtPosition = maze.ToSpecializedStructure.ConnectionTypeAtPosition

        let appendHorizontalWall wallType =
            match wallType with
                | Close | ClosePersistent -> sBuilder.Append("_") |> ignore
                | ConnectionType.Open -> sBuilder.Append(" ") |> ignore

        let appendVerticalWall wallType =
            match wallType with
                | Close | ClosePersistent -> sBuilder.Append("|") |> ignore
                | ConnectionType.Open -> sBuilder.Append(" ") |> ignore

        // first row
        let lastColumnIndex = cells |> maxColumnIndex
        sBuilder.Append(" ") |> ignore
        for columnIndex in 0 .. lastColumnIndex do
            let cell =  get cells { RIndex = 0; CIndex = columnIndex }
            appendHorizontalWall (connectionTypeAtPosition cell Top)
            sBuilder.Append(" ") |> ignore
        sBuilder.Append("\n") |> ignore

        // every row
        for rowIndex in 0 .. cells |> maxRowIndex do
            for columnIndex in 0 .. lastColumnIndex do
                let cell = get cells { RIndex = rowIndex; CIndex = columnIndex }
                appendVerticalWall (connectionTypeAtPosition cell Left)
                appendHorizontalWall (connectionTypeAtPosition cell Bottom)
                
                if columnIndex = lastColumnIndex then
                    appendVerticalWall (connectionTypeAtPosition cell Right)

            sBuilder.Append("\n") |> ignore

        sBuilder.ToString()

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            OrthoCell.Create
            OrthoPositionHandler.Instance
            OrthoCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            OrthoCell.Create
            OrthoPositionHandler.Instance
            OrthoCoordinateHandler.Instance
            canvas