// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System.Collections.Generic
open Mazes.Core

type NonAdjacentNeighbors =
    private
        {
            Container : Dictionary<Coordinate, Dictionary<Coordinate, ConnectionType>>
        }

    member this.AddUpdate fromCoordinate toCoordinate wallType =
        let addUpdate fromCoordinate toCoordinate wallType =
            if this.Container.ContainsKey(fromCoordinate) then
                if this.Container.Item(fromCoordinate).ContainsKey(toCoordinate) then
                    this.Container.Item(fromCoordinate).Item(toCoordinate) <- wallType
                else
                    this.Container.Item(fromCoordinate).Add(toCoordinate, wallType)
            else
                let dic = Dictionary<Coordinate, ConnectionType>()
                dic.Add(toCoordinate, wallType)
                this.Container.Add(fromCoordinate, dic)
        
        addUpdate fromCoordinate toCoordinate wallType
        addUpdate toCoordinate fromCoordinate wallType

    member this.NonAdjacentNeighbors coordinate =
        seq {
            if this.Container.ContainsKey(coordinate) then
                let neighbors = this.Container.Item(coordinate)
                for neighbor in neighbors do
                    yield (neighbor.Key, neighbor.Value)
        }

    member this.ExistNeighbor fromCoordinate toCoordinate =
        this.Container.ContainsKey(fromCoordinate) &&
        this.Container.Item(fromCoordinate).ContainsKey(toCoordinate)

    member this.IsLinked coordinate =
        this.Container.ContainsKey(coordinate) &&
        (this.Container.Item(coordinate) |> Seq.where(fun kv -> ConnectionType.isConnected kv.Value)) |> Seq.length > 0

    member this.AreLinked fromCoordinate toCoordinate =
        this.ExistNeighbor fromCoordinate toCoordinate &&
        ConnectionType.isConnected (this.Container.Item(fromCoordinate).Item(toCoordinate))

    member this.All =
        seq {
            for fromItem in this.Container do
                for toItem in this.Container.Item(fromItem.Key) do
                    yield (fromItem.Key, toItem.Key, toItem.Value)
        } |> Seq.distinctBy(fun (c1, c2, _) -> Utils.getKey (c1, c2) )

    static member CreateEmpty =
        { Container = Dictionary<Coordinate, Dictionary<Coordinate, ConnectionType>>() }