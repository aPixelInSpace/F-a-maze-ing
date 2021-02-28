// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open System.Collections.Generic
open Mazes.Core

type ConnectionType =
    | Open
    | Close
    | ClosePersistent

module ConnectionType =

    let getConnectionTypeForEdge isEdge =
        if isEdge then ClosePersistent
        else Open

    let getConnectionTypeForInternal internalConnectionType isCurrentCellPartOfMaze isOtherCellPartOfMaze =
        match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
        | false, false -> Open
        | true, true -> internalConnectionType
        | true, false | false, true -> ClosePersistent

    let isConnected connectionType =
        connectionType = Open

[<Struct>]
type Connection<'Position> = {
    ConnectionType : ConnectionType
    ConnectionPosition : 'Position
}

/// Can contain a connection between any two NCoordinate but will typically be used for non 2D adjacent connection
type Connections =
    private
        {
            Container : Dictionary<NCoordinate, Dictionary<NCoordinate, ConnectionType>>
        }

    member this.UpdateConnection connectionType fromCoordinate toCoordinate =
        let addUpdate fromCoordinate toCoordinate wallType =
            if this.Container.ContainsKey(fromCoordinate) then
                if this.Container.Item(fromCoordinate).ContainsKey(toCoordinate) then
                    this.Container.Item(fromCoordinate).Item(toCoordinate) <- wallType
                else
                    this.Container.Item(fromCoordinate).Add(toCoordinate, wallType)
            else
                let dic = Dictionary<NCoordinate, ConnectionType>()
                dic.Add(toCoordinate, wallType)
                this.Container.Add(fromCoordinate, dic)
        
        addUpdate fromCoordinate toCoordinate connectionType
        addUpdate toCoordinate fromCoordinate connectionType

    member this.Neighbors coordinate =
        seq {
            if this.Container.ContainsKey(coordinate) then
                let connections = this.Container.Item(coordinate)
                for connection in connections do
                    yield (connection.Key, connection.Value)
        }

    member this.ExistNeighbor fromCoordinate toCoordinate =
        this.Container.ContainsKey(fromCoordinate) &&
        this.Container.Item(fromCoordinate).ContainsKey(toCoordinate)

    member this.IsCellConnected coordinate =
        this.Container.ContainsKey(coordinate) &&
        (this.Container.Item(coordinate) |> Seq.where(fun kv -> ConnectionType.isConnected kv.Value)) |> Seq.length > 0

    member this.AreConnected fromCoordinate toCoordinate =
        this.ExistNeighbor fromCoordinate toCoordinate &&
        ConnectionType.isConnected (this.Container.Item(fromCoordinate).Item(toCoordinate))

    member this.All (dimension : Dimension option) =
        seq {
            for fromItem in this.Container do
                for toItem in this.Container.Item(fromItem.Key) do
                    match dimension with
                    | Some dimension ->
                        if fromItem.Key.ToDimension = dimension && toItem.Key.ToDimension = dimension then 
                            yield (fromItem.Key, toItem.Key, toItem.Value)
                    | None ->
                        yield (fromItem.Key, toItem.Key, toItem.Value)
        } |> Seq.distinctBy(fun (c1, c2, _) -> Utils.getKey (c1, c2) )

    static member CreateEmpty =
        { Container = Dictionary<NCoordinate, Dictionary<NCoordinate, ConnectionType>>() }