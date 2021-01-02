// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Teleport

open System.Collections.Generic
open Mazes.Core

type Teleports =
    private
        {
            Container : Dictionary<Coordinate, HashSet<Coordinate>>
        }

    member this.AddOneWayTeleport fromCoordinate toCoordinate =
        if this.Container.ContainsKey(fromCoordinate) then
            this.Container.Item(fromCoordinate).Add(toCoordinate) |> ignore
        else
            let hashset = HashSet<Coordinate>()
            hashset.Add(toCoordinate) |> ignore
            this.Container.Add(fromCoordinate, hashset)

    member this.AddTwoWayTeleport fromCoordinate toCoordinate =
        this.AddOneWayTeleport fromCoordinate toCoordinate
        this.AddOneWayTeleport toCoordinate fromCoordinate

    member this.Teleports fromCoordinate =
        seq {
            if this.Container.ContainsKey(fromCoordinate) then
                for toCoordinate in this.Container.Item(fromCoordinate) do
                    yield toCoordinate
        }

    static member createEmpty =
        { Container = Dictionary<Coordinate, HashSet<Coordinate>>() }