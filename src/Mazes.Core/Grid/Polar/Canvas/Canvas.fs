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

    member this.Zone coordinate =
        get this.Zones coordinate

    member this.IsZonePartOfMaze coordinate =
        (this.Zone coordinate).IsAPartOfMaze

module Canvas =
    let create numberOfRings widthHeightRatio numberOfCellsForCenterRing isZonePartOfMaze =
        let zones = ArrayOfA.create numberOfRings widthHeightRatio numberOfCellsForCenterRing (fun rIndex cIndex -> Zone.create (isZonePartOfMaze rIndex cIndex))
    
        {
            WidthHeightRatio = widthHeightRatio
            NumberOfCellsForCenterRing = numberOfCellsForCenterRing
            Zones = zones
        }