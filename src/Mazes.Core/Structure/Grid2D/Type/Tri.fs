// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure.Grid2D.Type.Tri

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Structure
open Mazes.Core.Structure.Grid2D

type TriPosition =
    | Left
    | Top
    | Right
    | Bottom

type TriPositionHandler private () =

    static let instance = TriPositionHandler()

    interface IPositionHandler<TriPosition> with

        // todo : use the coordinate to better handle the opposite position ?
        member this.Opposite _ position =
            match position with
            | Left -> Right
            | Top -> Bottom
            | Right -> Left
            | Bottom -> Top

        member this.Values coordinate =
            let isUpright = TriPositionHandler.IsUpright coordinate
            [|
                Left
                Right
                if not isUpright then
                    Top
                else
                    Bottom
            |]

        // todo : use the coordinate to better handle the position ?
        member this.Map _ position =
            match position with
            | Position.Left -> Left
            | Position.Top -> Top
            | Position.Right -> Right
            | Position.Bottom -> Bottom

    member this.ToInterface =
        this :> IPositionHandler<TriPosition>

    static member Instance =
        instance.ToInterface

    static member IsUpright coordinate =
        (coordinate.RIndex + coordinate.CIndex) % 2 = 0

type TriCoordinateHandler private () =

    static let instance = TriCoordinateHandler()

    interface ICoordinateHandlerArray2D<TriPosition> with

        member this.NeighborCoordinateAt coordinate position =
            let isUpright = TriPositionHandler.IsUpright coordinate
            match (position, isUpright) with
            | (TriPosition.Top, false) ->  Some { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }
            | (TriPosition.Right, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
            | (TriPosition.Bottom, true) -> Some { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }
            | (TriPosition.Left, _) -> Some { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | _ -> None

        member this.NeighborPositionAt coordinate otherCoordinate =
            let neighborCoordinateAt = this.ToInterface.NeighborCoordinateAt coordinate

            TriPositionHandler.Instance.Values coordinate
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
        this :> ICoordinateHandlerArray2D<TriPosition>

    static member Instance =
        instance.ToInterface

[<Struct>]
type TriCell =
    private
        { Connections : Connection<TriPosition> array }

    interface ICell<TriPosition> with
        member this.Create walls =
            ({ Connections = walls } :> ICell<TriPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            this.Connections.[TriCell.ConnectionPositionIndex position].ConnectionType

    member this.ToInterface =
        this :> ICell<TriPosition>

    static member ConnectionPositionIndex position =
        match position with
        | Left -> 0
        | Right -> 1
        | Top -> 2
        | Bottom -> 2

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate2D) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze
            else
                match (TriCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        {
            Connections =
                [|
                   { ConnectionType = (getWallType (isFirstColumn coordinate.CIndex) Left); ConnectionPosition = Left }
                   { ConnectionType = (getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right); ConnectionPosition = Right }

                   if TriPositionHandler.IsUpright coordinate then
                       { ConnectionType = (getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom); ConnectionPosition = Bottom }
                   else
                       { ConnectionType = (getWallType (isFirstRow coordinate.RIndex) Top); ConnectionPosition = Top } |]                
        }.ToInterface

module Grid =

    let createBaseGrid canvas =
        GridArray2D.createBaseGrid
            TriCell.Create
            TriPositionHandler.Instance
            TriCoordinateHandler.Instance
            canvas

    let createEmptyBaseGrid canvas =
        GridArray2D.createEmptyBaseGrid
            TriCell.Create
            TriPositionHandler.Instance
            TriCoordinateHandler.Instance
            canvas