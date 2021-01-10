// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Canvas.ArrayOfA

open Mazes.Core
open Mazes.Core.ArrayOfA
open Mazes.Core.Canvas

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
        this.Zones |> getItemByItem filter

    member this.NeighborsPartOfMazeOf (listOfNeighborCoordinatePosition : (Coordinate * 'P) seq) =
        seq {
            for (coordinate, position) in listOfNeighborCoordinatePosition do
                if (this.ExistAt coordinate) && (this.Zone coordinate).IsAPartOfMaze then
                    yield (coordinate, position)
        }

    member this.GetFirstPartOfMazeZone =
        this.Zones
        |> getItemByItem (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    member this.GetLastPartOfMazeZone =
        getItemByItemDesc this.Zones (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

module Canvas =
    let createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing isZonePartOfMaze =
        
        let zones = createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun rIndex cIndex -> Zone.create (isZonePartOfMaze rIndex cIndex))
    
        {
            WidthHeightRatio = widthHeightRatio
            NumberOfCellsForCenterRing = numberOfCellsForCenterRing
            Zones = zones
        }