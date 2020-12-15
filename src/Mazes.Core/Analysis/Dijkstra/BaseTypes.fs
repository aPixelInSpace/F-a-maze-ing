// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Analysis.Dijkstra

open System.Collections.Generic
open Mazes.Core

type Distance = int

[<Struct>]
type Node = {
        DistanceFromRoot : Distance
        Neighbors :  Coordinate seq
    }

[<Struct>]
type Node2 = {
        Coordinate :  Coordinate
        DistanceFromRoot : Distance
    }

[<Struct>]
type FarthestFromRoot = {
        Distance : Distance
        Coordinates : Coordinate array
    }

type CoordinatesByDistance =
    {
        Container : Dictionary<Distance, HashSet<Coordinate>>
    }

    member this.Remove distance coordinate =
        if this.Container.ContainsKey(distance) then
            let distanceArray = this.Container.Item(distance)
            if distanceArray.Remove(coordinate) then
                if distanceArray.Count = 0 then
                    this.Container.Remove(distance) |> ignore

    member this.AddUpdate distance coordinate =
        if this.Container.ContainsKey(distance) then
            let distanceArray = this.Container.Item(distance)
            distanceArray.Add(coordinate) |> ignore
        else
            let distanceArray = HashSet<Coordinate>()
            distanceArray.Add(coordinate) |> ignore
            this.Container.Add(distance, distanceArray)

    member this.MaxDistance =
        this.Container.Keys |> Seq.max

    member this.CoordinatesWithDistance distance =
        let coordinates = Array.zeroCreate<Coordinate>(this.Container.Item(distance).Count)
        this.Container.Item(distance).CopyTo(coordinates)

        coordinates

    static member createEmpty =
        { Container = Dictionary<Distance, HashSet<Coordinate>>() }