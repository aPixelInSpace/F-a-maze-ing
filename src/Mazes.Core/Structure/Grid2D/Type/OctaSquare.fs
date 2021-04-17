// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D.Type.OctaSquare

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D

type OctaSquarePosition =
    | Left
    | TopLeft
    | Top
    | TopRight
    | Right
    | BottomLeft
    | Bottom
    | BottomRight

type OctaSquarePositionHandler private () =

    static let instance = OctaSquarePositionHandler()

    interface IPositionHandler<OctaSquarePosition> with

        member this.Opposite _ position =
            match position with
            | Left -> Right
            | TopLeft -> BottomRight
            | Top -> Bottom
            | TopRight -> BottomLeft
            | Right -> Left
            | BottomLeft -> TopRight
            | Bottom -> Top
            | BottomRight -> TopLeft

        member this.Values coordinate =
            if OctaSquarePositionHandler.IsOctagon coordinate then
                [| Left; TopLeft; Top; TopRight; Right; BottomLeft; Bottom; BottomRight; |]
            else
                [| Left; Top; Right; Bottom; |]

        member this.Map _ position =
            match position with
            | Position.Top -> Top
            | Position.Left -> Left
            | Position.Bottom -> Bottom
            | Position.Right -> Right

    member this.ToInterface =
        this :> IPositionHandler<OctaSquarePosition>

    static member Instance =
        instance.ToInterface

    static member IsOctagon coordinate =
        (coordinate.RIndex + coordinate.CIndex) % 2 = 0

    static member IsSquare coordinate =
        not (OctaSquarePositionHandler.IsOctagon coordinate)

type OctaSquareCoordinateHandler private () =

    static let instance = OctaSquareCoordinateHandler()

    interface ICoordinateHandlerArray2D<OctaSquarePosition> with

        member this.NeighborCoordinateAt coordinate position =
            let isOctagon = OctaSquarePositionHandler.IsOctagon coordinate

            match (position, isOctagon) with
            | (OctaSquarePosition.Left, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.Top, _) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | (OctaSquarePosition.Right, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | (OctaSquarePosition.Bottom, _) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | (OctaSquarePosition.TopLeft, true) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.TopRight, true) -> Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex + 1 }
            | (OctaSquarePosition.BottomLeft, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex - 1 }
            | (OctaSquarePosition.BottomRight, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex + 1 }
            | _ -> None

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            OctaSquarePositionHandler.Instance.Values coordinate
            |> Array.find(fun position ->
                            let neighborCoordinate = neighborCoordinateAt position
                            match neighborCoordinate with
                            | Some neighborCoordinate -> neighborCoordinate = otherCoordinate
                            | None -> false)

        member this.WeaveCoordinates coordinates =
            let filtered =
                coordinates
                |> Seq.filter(fun c -> (c.RIndex % 2 = 0 && c.CIndex % 2 = 1))

            let vertical =
                filtered
                |> Seq.map(fun c -> (c, { RIndex = c.RIndex + 2; CIndex = c.CIndex }))
            
            let horizontal =
                filtered
                |> Seq.map(fun c -> (c, { RIndex = c.RIndex; CIndex = c.CIndex + 2 }))

            vertical
            |> Seq.append horizontal

    member this.ToInterface =
        this :> ICoordinateHandlerArray2D<OctaSquarePosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type OctaSquareCell =
    private
        { Connections : Connection<OctaSquarePosition> array }

    interface ICell<OctaSquarePosition> with
        member this.Create walls =
            ({ Connections = walls } :> ICell<OctaSquarePosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[OctaSquareCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<OctaSquarePosition>

    static member ConnectionPositionIndex position =
        match position with
        | Left -> 0
        | Top -> 1
        | Right -> 2
        | Bottom -> 3
        | TopLeft -> 4
        | TopRight -> 5
        | BottomLeft -> 6
        | BottomRight -> 7

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (OctaSquareCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let isOctagon = OctaSquarePositionHandler.IsOctagon coordinate
        let isFirstRow = isFirstRow coordinate.RIndex
        let isLastRow = isLastRow coordinate.RIndex numberOfRows
        let isFirstColumn = isFirstColumn coordinate.CIndex
        let isLastColumn = isLastColumn coordinate.CIndex numberOfColumns

        {
            Connections =
                [| { ConnectionType = (getWallType isFirstColumn Left); ConnectionPosition = Left }
                   { ConnectionType = (getWallType isFirstRow Top); ConnectionPosition = Top }
                   { ConnectionType = (getWallType isLastColumn Right); ConnectionPosition = Right }
                   { ConnectionType = (getWallType isLastRow Bottom); ConnectionPosition = Bottom }
                   
                   if isOctagon then
                       { ConnectionType = (getWallType (isFirstRow || isFirstColumn) TopLeft); ConnectionPosition = TopLeft }
                       { ConnectionType = (getWallType (isFirstRow || isLastColumn) TopRight); ConnectionPosition = TopRight }
                       { ConnectionType = (getWallType (isLastRow || isFirstColumn) BottomLeft); ConnectionPosition = BottomLeft }
                       { ConnectionType = (getWallType (isLastRow || isLastColumn) BottomRight); ConnectionPosition = BottomRight } |]
        }.ToInterface

module Grid =

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            OctaSquareCell.Create
            OctaSquarePositionHandler.Instance
            OctaSquareCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            OctaSquareCell.Create
            OctaSquarePositionHandler.Instance
            OctaSquareCoordinateHandler.Instance
            canvas