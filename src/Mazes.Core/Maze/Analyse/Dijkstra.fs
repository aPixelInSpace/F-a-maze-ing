module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Analyse


type private DistanceSoFar = DistanceSoFar of int
module private DistanceSoFar =
    let value (DistanceSoFar dist) = dist

let createMap root maze =
    let distancesFromRoot = Array2D.create maze.Grid.Canvas.NumberOfRows maze.Grid.Canvas.NumberOfColumns { DistanceFromRoot = None; Neighbors = None }

    let getNavigableNeighborsCoordinates = getNavigableNeighborsCoordinates maze.Grid

    let mutable unvisited = Map.empty.Add(root, DistanceSoFar -1)    

    let mutable counter = 0
    
    while not unvisited.IsEmpty do
        let unvisitedCoordinate = unvisited |> Seq.head
        
        let coordinate = unvisitedCoordinate.Key
        let actualDistance = (unvisitedCoordinate.Value |> DistanceSoFar.value) + 1

        let neighbors = getNavigableNeighborsCoordinates coordinate

        distancesFromRoot.[coordinate.RowIndex, coordinate.ColumnIndex] <- { DistanceFromRoot = Some actualDistance; Neighbors = Some neighbors }

        counter <- counter + 1
        
        unvisited <-
                neighbors
                |> Seq.filter(fun nCoordinate -> (get distancesFromRoot nCoordinate).DistanceFromRoot.IsNone)
                |> Seq.fold
                       (fun (m : Map<Coordinate, DistanceSoFar>) i -> m.Add(i, DistanceSoFar actualDistance))
                       (unvisited.Remove(coordinate))

    { Root = root; DistancesFromRoot = distancesFromRoot; ZonesAccessibleFromRoot = counter }