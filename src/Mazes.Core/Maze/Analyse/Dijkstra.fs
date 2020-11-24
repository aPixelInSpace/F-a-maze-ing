module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Grid
open Mazes.Core.Maze
open Mazes.Core.Maze.Analyse

type private Unvisited = {
    Coordinate : Coordinate
    DistanceSoFar : int
}

let createMap root maze =
    let distancesFromRoot = Array2D.create maze.Grid.Canvas.NumberOfRows maze.Grid.Canvas.NumberOfColumns { DistanceFromRoot = None; Neighbors = None }

    let getNavigableNeighborsCoordinates = getNavigableNeighborsCoordinates maze.Grid

    let unvisited = ResizeArray<Unvisited>()
    unvisited.Add({ Coordinate = root; DistanceSoFar = - 1 })

    let mutable counter = 0
    
    while unvisited.Count > 0 do
        let unvisitedCell = unvisited.[0]
        
        let actualDistance = unvisitedCell.DistanceSoFar + 1

        let unvisitedNeighborsCoordinates =
            getNavigableNeighborsCoordinates unvisitedCell.Coordinate
            |> Seq.filter(fun neighborCoordinate -> (distancesFromRoot |> get neighborCoordinate).DistanceFromRoot.IsNone)

        distancesFromRoot.[unvisitedCell.Coordinate.RowIndex, unvisitedCell.Coordinate.ColumnIndex] <- { DistanceFromRoot = Some actualDistance; Neighbors = Some unvisitedNeighborsCoordinates }

        counter <- counter + 1

        unvisited.RemoveAt(0)

        unvisited.AddRange(unvisitedNeighborsCoordinates |> Seq.map(fun neighborCoordinate -> { Coordinate = neighborCoordinate; DistanceSoFar = actualDistance }))        

    { Root = root; DistancesFromRoot = distancesFromRoot; AccessibleFromRoot = counter }