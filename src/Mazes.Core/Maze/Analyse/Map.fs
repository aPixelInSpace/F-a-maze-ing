// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Analyse

open Mazes.Core
open Mazes.Core.Array2D

type Distance = int

type MapZone = {    
    DistanceFromRoot : int
    Neighbors :  seq<Coordinate>
}

type FarthestFromRoot = {
    Distance : int
    Coordinates : Coordinate array
}

type Map =
    {
        Root : Coordinate
        MapZones : (MapZone option)[,]
        TotalZonesAccessibleFromRoot : int
        FarthestFromRoot : FarthestFromRoot
    }

    member this.MapZone coordinate =
        get this.MapZones coordinate

    member this.PathFromGoalToRoot (goalCoordinate : Coordinate option) =
        let nextCoordinate mapZone =
            mapZone.Neighbors
            |> Seq.tryFind(
                fun coordinateNeighbor ->

                let mapZoneNeighbor = this.MapZone coordinateNeighbor
                match mapZoneNeighbor with
                | Some mapZoneNeighbor -> mapZoneNeighbor.DistanceFromRoot < mapZone.DistanceFromRoot
                | None -> false)

        seq {
            let mutable currentCoordinate = goalCoordinate
            while currentCoordinate.IsSome do

                let currentCoordinateValue = currentCoordinate.Value

                yield currentCoordinateValue

                let currentMapZone = this.MapZone currentCoordinateValue                   

                match currentMapZone with
                | Some currentMapZone ->
                    currentCoordinate <- nextCoordinate currentMapZone                        
                | None ->
                    currentCoordinate <- None
        }

    member this.PathFromRootTo (goalCoordinate : Coordinate option) =
        this.PathFromGoalToRoot goalCoordinate
        |> Seq.rev