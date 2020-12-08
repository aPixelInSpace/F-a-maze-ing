// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.AldousBroder

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed grid =

    let rng = Random(rngSeed)

    // todo : remove the dictionary because it's overkill : add a Cell.HasEmptyWall to know if a cell has been linked
    let unvisited = 
        grid.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.fold
               (fun (dic : Dictionary<Coordinate, bool>) (_, coordinate) ->
                    dic.Add(coordinate, false)
                    dic)
               (Dictionary<Coordinate, bool>(grid.Canvas.NumberOfRows * grid.Canvas.NumberOfColumns))

    let mutable currentCoordinate = unvisited.ElementAt(rng.Next(unvisited.Count)).Key

    while unvisited.Count > 0 do
        if unvisited.ContainsKey(currentCoordinate) then
            unvisited.Remove(currentCoordinate) |> ignore

        let neighbors = grid.Canvas.NeighborsPartOfMazeOf currentCoordinate |> Seq.toArray 
        let nextCoordinate = neighbors.[rng.Next(neighbors.Length)]

        if unvisited.ContainsKey(nextCoordinate) then
            grid.UpdateWallAtCoordinates currentCoordinate nextCoordinate Empty

        currentCoordinate <- nextCoordinate

    { Grid = grid }