// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Analyse

type DistanceSoFar = int

let createMap root maze =
    let distancesFromRoot = Array2D.create maze.Grid.Canvas.NumberOfRows maze.Grid.Canvas.NumberOfColumns { DistanceFromRoot = None; Neighbors = None }

    let getNavigableNeighborsCoordinates = getNavigableNeighborsCoordinates maze.Grid

    let mutable unvisited = Map.empty.Add(root, -1)    

    let counter = ref 0
    
    while not unvisited.IsEmpty do
        let unvisitedCoordinate = unvisited |> Seq.head

        let coordinate = unvisitedCoordinate.Key
        let actualDistance = unvisitedCoordinate.Value + 1

        let neighbors = getNavigableNeighborsCoordinates coordinate

        distancesFromRoot.[coordinate.RowIndex, coordinate.ColumnIndex] <- { DistanceFromRoot = Some actualDistance; Neighbors = Some neighbors }

        incr counter
        
        unvisited <-
                neighbors
                |> Seq.filter(fun nCoordinate -> (get distancesFromRoot nCoordinate).DistanceFromRoot.IsNone)
                |> Seq.fold
                       (fun (m : Map<Coordinate, DistanceSoFar>) neighborCoordinate -> m.Add(neighborCoordinate, actualDistance))
                       (unvisited.Remove(coordinate))

    { Root = root; DistancesFromRoot = distancesFromRoot; TotalZonesAccessibleFromRoot = counter.Value }