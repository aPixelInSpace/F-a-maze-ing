// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D.Type.Brick

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D

type BrickPosition =
    | Left
    | TopLeft
    | TopRight
    | Right
    | BottomLeft
    | BottomRight

type BrickPositionHandler private () =

    static let instance = BrickPositionHandler()

    interface IPositionHandler<BrickPosition> with

        member this.Opposite _ position =
            match position with
            | Left -> Right
            | TopLeft -> BottomRight
            | TopRight -> BottomLeft
            | Right -> Left
            | BottomLeft -> TopRight
            | BottomRight -> TopLeft

        member this.Values _ =
            [| Left; TopLeft; TopRight; Right; BottomLeft; BottomRight |]

        member this.Map _ position =
            match position with
            | Position.Top -> TopLeft
            | Position.Left -> Left
            | Position.Bottom -> BottomRight
            | Position.Right -> Right

    member this.ToInterface =
        this :> IPositionHandler<BrickPosition>

    static member Instance =
        instance.ToInterface

    static member IsEven coordinate =
        coordinate.RIndex % 2 = 0

type BrickCoordinateHandler private () =

    static let instance = BrickCoordinateHandler()

    interface ICoordinateHandlerArray2D<BrickPosition> with

        member this.NeighborCoordinateAt coordinate position =

            let isEven = BrickPositionHandler.IsEven coordinate

            match position with
            | BrickPosition.Left ->  Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | BrickPosition.TopLeft -> Some { RIndex = coordinate.RIndex - 1; CIndex = if isEven then coordinate.CIndex - 1 else coordinate.CIndex }
            | BrickPosition.TopRight -> Some { RIndex = coordinate.RIndex - 1; CIndex = if isEven then coordinate.CIndex else coordinate.CIndex + 1 }
            | BrickPosition.Right -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | BrickPosition.BottomLeft -> Some { RIndex = coordinate.RIndex + 1; CIndex = if isEven then coordinate.CIndex - 1 else coordinate.CIndex }
            | BrickPosition.BottomRight -> Some { RIndex = coordinate.RIndex + 1; CIndex = if isEven then coordinate.CIndex else coordinate.CIndex + 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            BrickPositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

        member this.WeaveCoordinates coordinates =
            coordinates
            |> Seq.filter(fun c -> c.CIndex % 2 = 0)
            |> Seq.map(fun c -> (c, { RIndex = c.RIndex; CIndex = c.CIndex + 2 }))

    member this.ToInterface =
        this :> ICoordinateHandlerArray2D<BrickPosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type BrickCell =
    private
        { Connections : Connection<BrickPosition> array }

    interface ICell<BrickPosition> with
        member this.Create walls =
            ({ Connections = walls } :> ICell<BrickPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[BrickCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<BrickPosition>

    static member ConnectionPositionIndex position =
        match position with
        | Left -> 0
        | TopLeft -> 1
        | TopRight -> 2
        | Right -> 3
        | BottomLeft -> 4
        | BottomRight -> 5

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (BrickCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let isEven = (BrickPositionHandler.IsEven coordinate)
        let isOdd = not isEven

        let wallTypeLeft = getWallType (isFirstColumn coordinate.CIndex) Left

        let wallTypeTopLeft = getWallType (isFirstRow coordinate.RIndex || (isFirstColumn coordinate.CIndex && isEven)) TopLeft
        
        let wallTypeTopRight = getWallType (isFirstRow coordinate.RIndex || (isLastColumn coordinate.CIndex numberOfColumns && isOdd)) TopRight
        
        let wallTypeRight = getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right

        let wallTypeBottomLeft = getWallType (isLastRow coordinate.RIndex numberOfRows || (isFirstColumn coordinate.CIndex && isEven)) BottomLeft
        
        let wallTypeBottomRight = getWallType (isLastRow coordinate.RIndex numberOfRows || (isLastColumn coordinate.CIndex numberOfColumns && isOdd)) BottomRight
        
        {
            Connections =
                [| { ConnectionType = wallTypeLeft; ConnectionPosition = Left }
                   { ConnectionType = wallTypeTopLeft; ConnectionPosition = TopLeft }
                   { ConnectionType = wallTypeTopRight; ConnectionPosition = TopRight }
                   { ConnectionType = wallTypeRight; ConnectionPosition = Right }
                   { ConnectionType = wallTypeBottomLeft; ConnectionPosition = BottomLeft }
                   { ConnectionType = wallTypeBottomRight; ConnectionPosition = BottomRight } |]
        }.ToInterface

module Grid =

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            BrickCell.Create
            BrickPositionHandler.Instance
            BrickCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            BrickCell.Create
            BrickPositionHandler.Instance
            BrickCoordinateHandler.Instance
            canvas