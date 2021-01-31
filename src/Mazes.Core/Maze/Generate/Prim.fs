// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Priority_Queue
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

module SimplePrim =

    let createMaze rngSeed (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = HashSet<Coordinate>()
        actives.Add(randomStartCoordinate) |> ignore

        while actives.Count > 0 do
            let active = actives.ElementAt(rng.Next(actives.Count))

            let unlinked =
                active
                |> grid.NeighborsThatAreLinked false
                |> Seq.toArray

            if unlinked.Length > 0 then
                let neighbor = unlinked.[rng.Next(unlinked.Length)]
                grid.LinkCells active neighbor
                actives.Add(neighbor) |> ignore
            else
                actives.Remove(active) |> ignore

        { Grid = grid }

module TruePrim =

    let createMaze rngSeed maxWeight (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)
        //let maxWeight = 42 // grid.TotalOfMazeCells

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = SimplePriorityQueue<Coordinate, int>()
        actives.Enqueue(randomStartCoordinate, rng.Next(maxWeight))

        while actives.Count > 0 do
            let active = actives.First

            let unlinked =
                active
                |> grid.NeighborsThatAreLinked false
                |> Seq.toArray

            if unlinked.Length > 0 then
                let neighbor = unlinked.[rng.Next(unlinked.Length)]
                grid.LinkCells active neighbor
                actives.Enqueue(neighbor, rng.Next(maxWeight))
            else
                actives.Remove(active)

        { Grid = grid }