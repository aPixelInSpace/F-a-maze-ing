// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.HuntAndKill

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed grid =

    let rng = Random(rngSeed)

    let zonesPartOfMaze = grid.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
    let randomStartCoordinate = snd (zonesPartOfMaze.ElementAt(rng.Next(zonesPartOfMaze.Count())))

    let edges = HashSet<Coordinate>()
    edges.Add(randomStartCoordinate) |> ignore

    while edges.Count > 0 do
        let mutable headCoordinate = edges.ElementAt(rng.Next(edges.Count))
        edges.Remove(headCoordinate) |> ignore

        let headLinkedNeighbors = grid.LinkedNeighbors true headCoordinate |> Seq.toArray
        if headLinkedNeighbors.Length > 0 then
            let randomLinkedNeighbor = headLinkedNeighbors.[rng.Next(headLinkedNeighbors.Length)]
            grid.LinkCellsAtCoordinates headCoordinate randomLinkedNeighbor

        let mutable unlinkedNeighbors = grid.LinkedNeighbors false headCoordinate |> Seq.toArray

        while unlinkedNeighbors.Length > 0 do
            let nextCoordinate = unlinkedNeighbors.[rng.Next(unlinkedNeighbors.Length)]

            grid.LinkCellsAtCoordinates headCoordinate nextCoordinate

            edges.Remove(nextCoordinate) |> ignore

            unlinkedNeighbors <- grid.LinkedNeighbors false nextCoordinate |> Seq.toArray

            edges.UnionWith(grid.LinkedNeighbors false headCoordinate)
            edges.UnionWith(unlinkedNeighbors)

            headCoordinate <- nextCoordinate
            

    { Grid = grid }
    