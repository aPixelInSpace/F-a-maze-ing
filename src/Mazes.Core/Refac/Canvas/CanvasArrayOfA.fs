// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Canvas

open Mazes.Core.Refac
open Mazes.Core.Refac.ArrayOfA

type CanvasArrayOfA =
    private
        {
            WidthHeightRatio : float
            NumberOfCellsForCenterRing : int
            Zones : Zone[][]
        }

module CanvasArrayOfA =

    let numberOfRings c =
        c.Zones.Length

    let existAt c coordinate =
        existAt c.Zones coordinate

    let zone c coordinate =
        get c.Zones coordinate

    let getZoneByZone c filter =
        c.Zones |> getItemByItem filter

    let totalOfMazeZones c =
        getZoneByZone c (fun (zone : Zone) _ -> zone.IsAPartOfMaze)
        |> Seq.length

    let isZonePartOfMaze c coordinate =
        (zone c coordinate).IsAPartOfMaze

    // todo : change (Coordinate2D * _) seq to Coordinate2D seq
    let neighborsPartOfMazeOf c (listOfNeighborCoordinatePosition : (Coordinate2D * _) seq) =
        seq {
            for (coordinate, position) in listOfNeighborCoordinatePosition do
                if (existAt c coordinate) && (zone c coordinate).IsAPartOfMaze then
                    yield (coordinate, position)
        }

    let getFirstPartOfMazeZone c =
        c.Zones
        |> getItemByItem (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let getLastPartOfMazeZone c =
        getItemByItemDesc c.Zones (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing isZonePartOfMaze =
        
        let zones = createPolar numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun rIndex cIndex -> Zone.create (isZonePartOfMaze rIndex cIndex))
    
        {
            WidthHeightRatio = widthHeightRatio
            NumberOfCellsForCenterRing = numberOfCellsForCenterRing
            Zones = zones
        }