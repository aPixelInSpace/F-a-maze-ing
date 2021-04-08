// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac

open System.Collections.Generic

type ConnectionState =
    | Open
    | Close
    | ClosePersistent

module ConnectionState =

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

// todo : change the use of Dictionary to the F# immutable data structure equivalent 
/// Can contain any connection between any two NCoordinate but will typically be used for non 2D adjacent connection
type CoordinateConnections = private CoordinateConnections of Dictionary<NCoordinate, Dictionary<NCoordinate, ConnectionState>>

module CoordinateConnections =
    
    let private value (CoordinateConnections cc) = cc

    let updateConnection cc connectionState fromCoordinate toCoordinate =
        let cc = value cc
        let addUpdate fromCoordinate toCoordinate wallType =
            if cc.ContainsKey(fromCoordinate) then
                if cc.Item(fromCoordinate).ContainsKey(toCoordinate) then
                    cc.Item(fromCoordinate).Item(toCoordinate) <- wallType
                else
                    cc.Item(fromCoordinate).Add(toCoordinate, wallType)
            else
                let dic = Dictionary<NCoordinate, ConnectionState>()
                dic.Add(toCoordinate, wallType)
                cc.Add(fromCoordinate, dic)
        
        addUpdate fromCoordinate toCoordinate connectionState
        addUpdate toCoordinate fromCoordinate connectionState

    let neighbors cc coordinate =
        seq {
            if (value cc).ContainsKey(coordinate) then
                let connections = (value cc).Item(coordinate)
                for connection in connections do
                    yield (connection.Key, connection.Value)
        }

    let existNeighbor cc fromCoordinate toCoordinate =
        (value cc).ContainsKey(fromCoordinate) &&
        (value cc).Item(fromCoordinate).ContainsKey(toCoordinate)

    let isCellConnected cc coordinate =
        (value cc).ContainsKey(coordinate) &&
        ((value cc).Item(coordinate) |> Seq.where(fun kv -> ConnectionState.isConnected kv.Value)) |> Seq.length > 0

    let areConnected cc fromCoordinate toCoordinate =
        existNeighbor cc fromCoordinate toCoordinate &&
        ConnectionState.isConnected ((value cc).Item(fromCoordinate).Item(toCoordinate))

    let all cc (dimension : Dimension option) =
        seq {
            for fromItem in (value cc) do
                for toItem in (value cc).Item(fromItem.Key) do
                    match dimension with
                    | Some dimension ->
                        if fromItem.Key.Dimension = dimension && toItem.Key.Dimension = dimension then 
                            yield (fromItem.Key, toItem.Key, toItem.Value)
                    | None ->
                        yield (fromItem.Key, toItem.Key, toItem.Value)
        } |> Seq.distinctBy(fun (c1, c2, _) -> Utils.getKey (c1, c2) )

    let createEmpty() =
        Dictionary<NCoordinate, Dictionary<NCoordinate, ConnectionState>>() |> CoordinateConnections