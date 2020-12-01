// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Analyse

let createMap maze rootCoordinate =

    let mutable farthestDistance = 0
    let farthestCoordinates = ResizeArray<Coordinate>()

    let updateFarthest actualDistance actualCoordinate =
        if actualDistance > farthestDistance then
            farthestCoordinates.Clear()
            farthestCoordinates.Add(actualCoordinate)
            farthestDistance <- actualDistance
        elif actualDistance = farthestDistance then
            farthestCoordinates.Add(actualCoordinate)            
        else
            ()

    let mapZones = Array2D.create maze.Grid.Canvas.NumberOfRows maze.Grid.Canvas.NumberOfColumns None

    let mutable unvisited = Map.empty.Add(rootCoordinate, -1)    

    let counter = ref 0

    while not unvisited.IsEmpty do
        let unvisitedCoordinate = unvisited |> Seq.head

        let coordinate = unvisitedCoordinate.Key
        let actualDistance = unvisitedCoordinate.Value + 1

        // update the map zones
        let neighbors = maze.Grid.NavigableNeighborsCoordinates coordinate
        mapZones.[coordinate.RowIndex, coordinate.ColumnIndex] <- Some { DistanceFromRoot = actualDistance; Neighbors = neighbors }

        // update the others useful infos
        incr counter
        updateFarthest actualDistance coordinate

        // update the unvisited with the coordinates that are not yet visited
        unvisited <-
                neighbors
                |> Seq.filter(fun nCoordinate -> (get mapZones nCoordinate).IsNone)
                |> Seq.fold
                       (fun (m : Map<Coordinate, Distance>) neighborCoordinate -> m.Add(neighborCoordinate, actualDistance))
                       (unvisited.Remove(coordinate))

    { Root = rootCoordinate
      MapZones = mapZones
      TotalZonesAccessibleFromRoot = counter.Value
      FarthestFromRoot = { Distance = farthestDistance; Coordinates = farthestCoordinates.ToArray() } }

let longestPaths maze map =    
    seq {
        for farthestCoordinate in map.FarthestFromRoot.Coordinates do
            let mapFromFarthest = createMap maze farthestCoordinate
            for newFarthestCoordinate in mapFromFarthest.FarthestFromRoot.Coordinates do
                yield mapFromFarthest.PathFromGoalToRoot (Some newFarthestCoordinate)
    }