// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D.Type.OrthoD

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D

type OrthoDPosition =
    | TopLeft
    | TopRight
    | BottomLeft
    | BottomRight

type OrthoDPositionHandler private () =

    static let instance = OrthoDPositionHandler()

    interface IPositionHandler<OrthoDPosition> with

        member this.Opposite _ position =
            match position with
            | TopLeft -> BottomRight
            | TopRight -> BottomLeft
            | BottomLeft -> TopRight
            | BottomRight -> TopLeft

        member this.Values _ =
            [| TopLeft; TopRight; BottomLeft; BottomRight |]

        member this.Map _ position =
            match position with
            | Position.Left -> TopLeft
            | Position.Top -> TopRight
            | Position.Right -> BottomRight
            | Position.Bottom -> BottomLeft

    member this.ToInterface =
        this :> IPositionHandler<OrthoDPosition>

    static member Instance =
        instance.ToInterface

type OrthoDCoordinateHandler private () =

    static let instance = OrthoDCoordinateHandler()

    interface ICoordinateHandlerArray2D<OrthoDPosition> with

        member this.NeighborCoordinateAt coordinate position =
            let isEven = coordinate.CIndex % 2 = 0
            match position with
            | OrthoDPosition.TopLeft ->  Some { RIndex = (if isEven then coordinate.RIndex - 1 else coordinate.RIndex); CIndex = coordinate.CIndex - 1 }
            | OrthoDPosition.TopRight -> Some { RIndex = (if isEven then coordinate.RIndex - 1 else coordinate.RIndex); CIndex = coordinate.CIndex + 1 }
            | OrthoDPosition.BottomLeft -> Some { RIndex = (if isEven then coordinate.RIndex else coordinate.RIndex + 1); CIndex = coordinate.CIndex - 1 }
            | OrthoDPosition.BottomRight -> Some  { RIndex = (if isEven then coordinate.RIndex else coordinate.RIndex + 1); CIndex = coordinate.CIndex + 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            OrthoDPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

        member this.WeaveCoordinates coordinates =
            let filtered =
                coordinates
                |> Seq.filter(fun c -> c.CIndex % 2 = 0)

            let diagonal =
                filtered
                |> Seq.map(fun c -> (c, { RIndex = c.RIndex + 1; CIndex = c.CIndex + 2 }))
            
            let horizontal =
                filtered
                |> Seq.map(fun c -> (c, { RIndex = c.RIndex; CIndex = c.CIndex + 2 }))

            diagonal
            |> Seq.append horizontal

    member this.ToInterface =
        this :> ICoordinateHandlerArray2D<OrthoDPosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type OrthoDCell =
    private
        { Connections : Connection<OrthoDPosition> array }

    interface ICell<OrthoDPosition> with
        member this.Create connections =
            ({ Connections = connections } :> ICell<OrthoDPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[OrthoDCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<OrthoDPosition>

    static member ConnectionPositionIndex position =
        match position with
        | TopLeft -> 0
        | TopRight -> 1
        | BottomLeft -> 2
        | BottomRight -> 3

    static member Create numberOfRows numberOfColumns internalConnectionType (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getConnectionType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (OrthoDCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalConnectionType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a connection type for the neighbor {coordinate} at {position}"

        let isEven = coordinate.CIndex % 2 = 0

        let connectionType pos =
            match pos with
            | TopLeft -> getConnectionType ((isFirstRow coordinate.RIndex && isEven) || isFirstColumn coordinate.CIndex) TopLeft
            | TopRight -> getConnectionType ((isFirstRow coordinate.RIndex && isEven) || isLastColumn coordinate.CIndex numberOfColumns) TopRight
            | BottomLeft -> getConnectionType ((isLastRow coordinate.RIndex numberOfRows && not isEven) || isFirstColumn coordinate.CIndex) BottomLeft
            | BottomRight -> getConnectionType ((isLastRow coordinate.RIndex numberOfRows && not isEven) || isLastColumn coordinate.CIndex numberOfColumns) BottomRight

        {
            Connections =
                [| for pos in OrthoDPositionHandler.Instance.Values coordinate do
                       { ConnectionType = (connectionType pos); ConnectionPosition = pos } |]                
        }.ToInterface

module Grid =

    let toString (maze : IAdjacentStructure<GridArray2D<OrthoDPosition>, OrthoDPosition>) =
        let sBuilder = StringBuilder()

        sBuilder.ToString()

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            OrthoDCell.Create
            OrthoDPositionHandler.Instance
            OrthoDCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            OrthoDCell.Create
            OrthoDPositionHandler.Instance
            OrthoDCoordinateHandler.Instance
            canvas