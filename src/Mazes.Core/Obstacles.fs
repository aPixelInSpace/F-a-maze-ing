// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

open System.Collections.Generic
open Mazes.Core

type Cost = int

/// Additional cost for going to the given coordinate
type Obstacles =
    private
        {
            Container : Dictionary<Coordinate, Cost>
        }

    member this.AddUpdateCost cost coordinate =
        if this.Container.ContainsKey(coordinate) then
            this.Container.Item(coordinate) <- cost
        else
            this.Container.Add(coordinate, cost)

    member this.Cost coordinate =
        if this.Container.ContainsKey(coordinate) then
            this.Container.Item(coordinate)
        else
            0

    static member CreateEmpty =
        { Container = Dictionary<Coordinate, Cost>() }