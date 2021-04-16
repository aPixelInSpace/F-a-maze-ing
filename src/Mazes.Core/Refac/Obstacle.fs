// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac

open System.Collections.Generic

type Cost = Cost of int

type Cost with
    static member (+) (Cost c1, Cost c2) =
        Cost (c1 + c2)

module Cost =
    
    let value (Cost c) = c

// todo : change the use of Dictionary to the F# immutable data structure equivalent
/// Additional cost for going to the given coordinate
type Obstacles = private Obstacles of Dictionary<NCoordinate, Cost>

module Obstacles =
    
    let value (Obstacles c) = c
    
    let addUpdateCost obs cost coordinate =
        let obs = value obs
        if obs.ContainsKey(coordinate) then
            obs.Item(coordinate) <- cost
        else
            obs.Add(coordinate, cost)

    let cost obs coordinate =
        let obs = value obs
        if obs.ContainsKey(coordinate) then
            obs.Item(coordinate)
        else
            0 |> Cost

    let createEmpty() =
        Dictionary<NCoordinate, Cost>() |> Obstacles