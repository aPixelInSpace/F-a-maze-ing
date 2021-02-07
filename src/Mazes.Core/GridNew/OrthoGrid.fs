﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.GridNew.Ortho

open System.Text
open Mazes.Core
open Mazes.Core.Array2D

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
            | Top -> getConnectionType (Array2D.isFirstRow coordinate.RIndex) Top
            | Right -> getConnectionType (Array2D.isLastColumn coordinate.CIndex numberOfColumns) Right
            | Bottom -> getConnectionType (Array2D.isLastRow coordinate.RIndex numberOfRows) Bottom
            | Left -> getConnectionType (Array2D.isFirstColumn coordinate.CIndex) Left

        {
            Connections =
                [| for pos in OrthoPositionHandler.Instance.Values coordinate do
                       { ConnectionType = (connectionType pos); ConnectionPosition = pos } |]                
        }.ToInterface

let toString connectionTypeAtPosition cells =
    let sBuilder = StringBuilder()

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

let private createInternal internalConnectionType (canvas : Canvas.Array2D.Canvas) =
    let cells =
        canvas.Zones |>
        Array2D.mapi(fun rowIndex columnIndex _ ->
            OrthoCell.Create
                canvas.NumberOfRows
                canvas.NumberOfColumns
                internalConnectionType
                { RIndex = rowIndex; CIndex = columnIndex }
                canvas.IsZonePartOfMaze)

    {
      Canvas = canvas
      Cells = cells
      PositionHandler = OrthoPositionHandler.Instance
      CoordinateHandler = OrthoCoordinateHandler.Instance
    }

let create canvas =
    createInternal Close canvas :> IAdjacentStructure<GridArray2D<OrthoPosition>, OrthoPosition>

let createEmpty canvas =
    createInternal Open canvas :> IAdjacentStructure<GridArray2D<OrthoPosition>,OrthoPosition>