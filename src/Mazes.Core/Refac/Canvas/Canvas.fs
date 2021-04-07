// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Canvas

open Mazes.Core.Refac
open Mazes.Core.Refac.Array2D

type CanvasArray2D =
    private
        {
            Zones : Zone[,]
        }

module CanvasArray2D =
    
    let numberOfRows c =
        Array2D.length1 c.Zones
        
    let numberOfColumns c =
        Array2D.length2 c.Zones

    let maxRowIndex c =
        maxRowIndex c.Zones

    let maxColumnIndex c =
        maxColumnIndex c.Zones

    let existAt c coordinate =
        existAt c.Zones coordinate

    let zone c coordinate =
        get c.Zones coordinate

    let getZoneByZone c extractBy filter =
        c.Zones |> getItemByItem extractBy filter

    let totalOfMazeZones c =
        getZoneByZone c RowsAscendingColumnsAscending (fun (zone : Zone) _ -> zone.IsAPartOfMaze)
        |> Seq.length

    let isZonePartOfMaze c coordinate =
        (zone c coordinate).IsAPartOfMaze

    let neighborsPartOfMazeOf c (listOfNeighborCoordinatePosition : (Coordinate2D * _) seq) =
        seq {
            for (coordinate, position) in listOfNeighborCoordinatePosition do
                if (existAt c coordinate) && (zone c coordinate).IsAPartOfMaze then
                    yield (coordinate, position)
        }

    let getFirstPartOfMazeZone c =
        getZoneByZone c RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let getLastPartOfMazeZone c =
        getZoneByZone c RowsDescendingColumnsDescending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    let create numberOfRows numberOfColumns isZonePartOfMaze =
        let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create (isZonePartOfMaze rowIndex columnIndex))
    
        { Zones = zones }

type CanvasArrayOfA =
    private
        {
            WidthHeightRatio : float
            NumberOfCellsForCenterRing : int
            Zones : Zone[][]
        }

type Canvas =
    | Array2D of CanvasArray2D
    | ArrayOfA of CanvasArrayOfA
