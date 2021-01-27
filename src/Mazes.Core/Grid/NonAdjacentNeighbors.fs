// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System.Collections.Generic
open Mazes.Core

type NonAdjacentNeighbors =
    private
        {
            Container : Dictionary<Coordinate, Dictionary<Coordinate, WallType>>
        }

    member this.AddUpdate fromCoordinate toCoordinate wallType =
        let addUpdate fromCoordinate toCoordinate wallType =
            if this.Container.ContainsKey(fromCoordinate) then
                if this.Container.Item(fromCoordinate).ContainsKey(toCoordinate) then
                    this.Container.Item(fromCoordinate).Item(toCoordinate) <- wallType
                else
                    this.Container.Item(fromCoordinate).Add(toCoordinate, wallType)
            else
                let dic = Dictionary<Coordinate, WallType>()
                dic.Add(toCoordinate, wallType)
                this.Container.Add(fromCoordinate, dic)
        
        addUpdate fromCoordinate toCoordinate wallType
        addUpdate toCoordinate fromCoordinate wallType

    member this.NonAdjacentNeighbors coordinate =
        seq {
            if this.Container.ContainsKey(coordinate) then
                for neighbor in this.Container.Item(coordinate) do
                    yield (neighbor.Key, neighbor.Value)
        }

    member this.ExistNeighbor fromCoordinate toCoordinate =
        this.Container.ContainsKey(fromCoordinate) &&
        this.Container.Item(fromCoordinate).ContainsKey(toCoordinate)

    member this.IsLinked coordinate =
        this.Container.ContainsKey(coordinate) &&
        (this.Container.Item(coordinate) |> Seq.where(fun kv -> WallType.isALink kv.Value)) |> Seq.length > 0

    member this.AreLinked fromCoordinate toCoordinate =
        this.ExistNeighbor fromCoordinate toCoordinate &&
        WallType.isALink (this.Container.Item(fromCoordinate).Item(toCoordinate))

    member this.All =
        seq {
            for fromItem in this.Container do
                for toItem in this.Container.Item(fromItem.Key) do
                    yield (fromItem.Key, toItem.Key, toItem.Value)
        } |> Seq.distinctBy(fun (coordinate1, coordinate2, _) ->
            if coordinate1 >= coordinate2 then $"{coordinate1.GetHashCode()}-{coordinate2.GetHashCode()}" else $"{coordinate2.GetHashCode()}-{coordinate1.GetHashCode()}")

    static member CreateEmpty =
        { Container = Dictionary<Coordinate, Dictionary<Coordinate, WallType>>() }