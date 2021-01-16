// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System.Collections.Generic
open Mazes.Core

type NonAdjacentNeighbors =
    private
        {
            Container : Dictionary<Coordinate, Dictionary<Coordinate, WallType>>
        }

    member this.AddUpdateOneWayNeighbor fromCoordinate toCoordinate wallType =
        if this.Container.ContainsKey(fromCoordinate) then
            if this.Container.Item(fromCoordinate).ContainsKey(toCoordinate) then
                this.Container.Item(fromCoordinate).Item(toCoordinate) <- wallType
            else
                this.Container.Item(fromCoordinate).Add(toCoordinate, wallType)
        else
            let dic = Dictionary<Coordinate, WallType>()
            dic.Add(toCoordinate, wallType)
            this.Container.Add(fromCoordinate, dic)

    member this.AddUpdateTwoWayNeighbor fromCoordinate toCoordinate wallType =
        this.AddUpdateOneWayNeighbor fromCoordinate toCoordinate wallType
        this.AddUpdateOneWayNeighbor toCoordinate fromCoordinate wallType

    member this.NonAdjacentNeighbors coordinate =
        seq {
            if this.Container.ContainsKey(coordinate) then
                for neighbor in this.Container.Item(coordinate) do
                    yield (neighbor.Key, neighbor.Value)
        }

    member this.ExistNeighbor fromCoordinate toCoordinate =
        this.Container.ContainsKey(fromCoordinate) &&
        this.Container.Item(fromCoordinate).ContainsKey(toCoordinate)

    member this.AreLinked fromCoordinate toCoordinate =
        this.ExistNeighbor fromCoordinate toCoordinate &&
        WallType.isALink (this.Container.Item(fromCoordinate).Item(toCoordinate))

    member this.NeighborsThatAreLinked isLinked coordinate =
        if this.Container.ContainsKey(coordinate) then
            this.Container.Item(coordinate)
            |> Seq.filter(fun item -> (WallType.isALink item.Value) = isLinked)
            |> Seq.map(fun item -> item.Key)
        else
            Seq.empty

    member this.All =
        seq {
            for fromItem in this.Container do
                for toItem in this.Container.Item(fromItem.Key) do
                    yield (fromItem.Key, toItem.Key)
        }

    static member CreateEmpty =
        { Container = Dictionary<Coordinate, Dictionary<Coordinate, WallType>>() }