// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D.Type.Hex

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D

type HexPosition =
    | TopLeft
    | Top
    | TopRight
    | BottomLeft
    | Bottom
    | BottomRight

type HexPositionHandler private () =

    static let instance = HexPositionHandler()

    interface IPositionHandler<HexPosition> with

        member this.Opposite _ position =
            match position with
            | TopLeft -> BottomRight
            | Top -> Bottom
            | TopRight -> BottomLeft
            | BottomLeft -> TopRight
            | Bottom -> Top
            | BottomRight -> TopLeft

        member this.Values _ =
            [| TopLeft; Top; TopRight; BottomLeft; Bottom; BottomRight |]

        member this.Map coordinate position =
            let cIndexEven = (HexPositionHandler.IsEven coordinate)
            match position with
            | Position.Top -> Top
            | Position.Left -> if cIndexEven then TopLeft else BottomLeft
            | Position.Bottom -> Bottom
            | Position.Right -> if cIndexEven then TopRight else BottomRight

    member this.ToInterface =
        this :> IPositionHandler<HexPosition>

    static member Instance =
        instance.ToInterface

    static member IsEven coordinate =
        coordinate.CIndex % 2 = 0

type HexCoordinateHandler private () =

    static let instance = HexCoordinateHandler()

    interface ICoordinateHandlerArray2D<HexPosition> with

        member this.NeighborCoordinateAt coordinate position =

            let (rIndexTopLeftRight, rIndexBottomLeftRight) =
                match (HexPositionHandler.IsEven coordinate) with
                | true -> (coordinate.RIndex, coordinate.RIndex + 1)
                | false -> (coordinate.RIndex - 1, coordinate.RIndex)

            match position with
            | HexPosition.TopLeft ->  Some { RIndex = rIndexTopLeftRight; CIndex = coordinate.CIndex - 1 }
            | HexPosition.Top -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | HexPosition.TopRight -> Some { RIndex = rIndexTopLeftRight; CIndex = coordinate.CIndex + 1 }
            | HexPosition.BottomLeft -> Some { RIndex = rIndexBottomLeftRight; CIndex = coordinate.CIndex - 1 }
            | HexPosition.Bottom -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | HexPosition.BottomRight -> Some { RIndex = rIndexBottomLeftRight; CIndex = coordinate.CIndex + 1 }

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            HexPositionHandler.Instance.Values coordinate
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
            
            let diagonal =
                filtered
                |> Seq.map(fun c -> (c, { RIndex = c.RIndex + 1; CIndex = c.CIndex + 2 }))

            vertical
            |> Seq.append diagonal

    member this.ToInterface =
        this :> ICoordinateHandlerArray2D<HexPosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type HexCell =
    private
        { Connections : Connection<HexPosition> array }

    interface ICell<HexPosition> with
        member this.Create walls =
            ({ Connections = walls } :> ICell<HexPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[HexCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<HexPosition>

    static member ConnectionPositionIndex position =
        match position with
        | TopLeft -> 0
        | Top -> 1
        | TopRight -> 2
        | BottomLeft -> 3
        | Bottom -> 4
        | BottomRight -> 5

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (HexCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let isEven = (HexPositionHandler.IsEven coordinate)
        let isOdd = not isEven

        let wallTypeTopLeft = getWallType ((isFirstRow coordinate.RIndex && isOdd) || isFirstColumn coordinate.CIndex) TopLeft

        let wallTypeTop = getWallType (isFirstRow coordinate.RIndex) Top

        let wallTypeTopRight = getWallType ((isFirstRow coordinate.RIndex && isOdd) || isLastColumn coordinate.CIndex numberOfColumns) TopRight

        let wallTypeBottomLeft = getWallType (isFirstColumn coordinate.CIndex || (isLastRow coordinate.RIndex numberOfRows && isEven)) BottomLeft

        let wallTypeBottom = getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom

        let wallTypeBottomRight = getWallType ((isLastRow coordinate.RIndex numberOfRows && isEven) || isLastColumn coordinate.CIndex numberOfColumns) BottomRight

        {
            Connections =
                [| { ConnectionType = wallTypeTopLeft; ConnectionPosition = TopLeft }
                   { ConnectionType = wallTypeTop; ConnectionPosition = Top }
                   { ConnectionType = wallTypeTopRight; ConnectionPosition = TopRight }
                   { ConnectionType = wallTypeBottomLeft; ConnectionPosition = BottomLeft }
                   { ConnectionType = wallTypeBottom; ConnectionPosition = Bottom }
                   { ConnectionType = wallTypeBottomRight; ConnectionPosition = BottomRight } |]
        }.ToInterface

module Grid =

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            HexCell.Create
            HexPositionHandler.Instance
            HexCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            HexCell.Create
            HexPositionHandler.Instance
            HexCoordinateHandler.Instance
            canvas