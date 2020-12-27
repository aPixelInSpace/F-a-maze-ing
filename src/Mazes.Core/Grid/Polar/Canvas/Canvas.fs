// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Polar.Canvas

open Mazes.Core
open Mazes.Core.ArrayOfA
open Mazes.Core.Canvas
open Mazes.Core.Grid.Polar

type Canvas =
    {
        WidthHeightRatio : float
        NumberOfCellsForCenterRing : int
        Zones : Zone[][]
    }

    member this.NumberOfRings =
        this.Zones.Length

    member this.ExistAt coordinate =
        existAt this.Zones coordinate

    member this.Zone coordinate =
        get this.Zones coordinate

    member this.TotalOfMazeZones =
        this.GetZoneByZone (fun (zone : Zone) _ -> zone.IsAPartOfMaze)
        |> Seq.length

    member this.IsZonePartOfMaze coordinate =
        (this.Zone coordinate).IsAPartOfMaze

    member this.GetZoneByZone filter =
        getItemByItem this.Zones filter

    member this.NeighborsPartOfMazeOf (coordinate : Coordinate) =
        let neighborCoordinateAt = PolarCoordinate.neighborsCoordinateAt this.Zones coordinate 
        seq {
            let leftCoordinate = neighborCoordinateAt Left |> Seq.head
            if (this.Zone leftCoordinate).IsAPartOfMaze then
                yield (leftCoordinate, Left)

            let rightCoordinate = neighborCoordinateAt Right |> Seq.head
            if (this.Zone rightCoordinate).IsAPartOfMaze then
                yield (rightCoordinate, Right)

            let inwardCoordinate = neighborCoordinateAt Inward
            if not (inwardCoordinate |> Seq.isEmpty) then
                let inwardCoordinate = inwardCoordinate |> Seq.head
                if (this.Zone inwardCoordinate).IsAPartOfMaze then
                    yield (inwardCoordinate, Inward)

            let outwardCoordinates = (neighborCoordinateAt Outward) |> Seq.toArray
            for i in 0 .. (outwardCoordinates.Length - 1) do
                yield (outwardCoordinates.[i], Outward)
        }

    member this.GetFirstPartOfMazeZone =
        getItemByItem this.Zones (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    member this.GetLastPartOfMazeZone =
        getItemByItemDesc this.Zones (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

module Canvas =
    let create numberOfRings widthHeightRatio numberOfCellsForCenterRing isZonePartOfMaze =
        let zones = PolarArrayOfA.create numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun rIndex cIndex -> Zone.create (isZonePartOfMaze rIndex cIndex))
    
        {
            WidthHeightRatio = widthHeightRatio
            NumberOfCellsForCenterRing = numberOfCellsForCenterRing
            Zones = zones
        }