// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

module GrowingTree =

    let baseAlgorithm
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor
            (grid : IGrid<'G>) =

        add startCoordinate

        while count() > 0 do
            let active = next()

            let unlinked =
                active
                |> grid.NeighborsThatAreLinked false
                |> Seq.toArray

            if unlinked.Length > 0 then
                let neighbor = chooseNeighbor active unlinked
                grid.ConnectCells active neighbor
                add neighbor
            else
                remove active

        grid

module GrowingTreeMixRandomAndLast =

    let createMaze rngSeed longPassages (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        // SortedList is not a great replacement for a Stack,
        // but at least we can remove an item at a given index
        let actives = SortedList<int, Coordinate>()

        let count () = actives.Count

        let uniqueIncreasingIndex = ref 0
        let add coordinate =
            actives.Add(uniqueIncreasingIndex.Value, coordinate)
            incr uniqueIncreasingIndex

        let next () =
            if rng.NextDouble() < longPassages then
                actives.Last().Value
            else
                actives.ElementAt(rng.Next(actives.Count)).Value

        let remove coordinate =
            actives.RemoveAt(actives.IndexOfValue(coordinate))

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }

module GrowingTreeMixOldestAndLast =

    let createMaze rngSeed longPassages (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = SortedList<int, Coordinate>()

        let count () = actives.Count

        let uniqueIncreasingIndex = ref 0
        let add coordinate =
            actives.Add(uniqueIncreasingIndex.Value, coordinate)
            incr uniqueIncreasingIndex

        let next () =
            if rng.NextDouble() < longPassages then
                actives.Last().Value
            else
                actives.First().Value

        let remove coordinate =
            actives.RemoveAt(actives.IndexOfValue(coordinate))

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }

module GrowingTreeMixChosenRandomAndLast =

    let createMaze rngSeed longPassages (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = SortedList<int, Coordinate>()
        let mutable chosenCoordinate : Coordinate option = None

        let count () = actives.Count

        let uniqueIncreasingIndex = ref 0
        let add coordinate =
            actives.Add(uniqueIncreasingIndex.Value, coordinate)
            incr uniqueIncreasingIndex

        let next () =
            match chosenCoordinate with
            | Some coordinate -> coordinate
            | None ->
                let coordinate =
                    if rng.NextDouble() < longPassages then
                        actives.Last().Value
                    else
                        actives.ElementAt(rng.Next(actives.Count)).Value
                    
                chosenCoordinate <- Some coordinate
                coordinate

        let remove coordinate =
            actives.RemoveAt(actives.IndexOfValue(coordinate))
            chosenCoordinate <- None

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }

module GrowingTreeDirection =

    let createMaze rngSeed toRightWeight toBottomWeight toLeftWeight (grid : unit -> IGrid<'G>) =

        let bottomWeight = toRightWeight + toBottomWeight
        let leftWeight = bottomWeight + toLeftWeight

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = Stack<Coordinate>()

        let count () = actives.Count

        let add coordinate =
            actives.Push(coordinate)

        let next () =
            actives.Peek()

        let remove _ =
            actives.Pop() |> ignore

        let chooseNeighbor _ (unlinked : array<Coordinate>) =
            let rnd = rng.NextDouble()
            if rnd < toRightWeight then
                unlinked
                |> Array.maxBy(fun c -> c.CIndex)
            elif rnd >= toRightWeight && rnd < bottomWeight then
                unlinked
                |> Array.maxBy(fun c -> c.RIndex)
            elif rnd >= bottomWeight && rnd < leftWeight then
                unlinked
                |> Array.minBy(fun c -> c.CIndex)
            else // what remains is 'top'
                unlinked
                |> Array.minBy(fun c -> c.RIndex)

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }

module GrowingTreeSpiral =

    let createMaze rngSeed spiralWeight spiralUniformity spiralMaxLength (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = Stack<Coordinate>()

        let count () = actives.Count

        let add coordinate =
            actives.Push(coordinate)

        let next () =
            actives.Peek()

        let remove _ =
            actives.Pop() |> ignore

        let mutable spiralLength = 0
        let mutable currentDirection = Right
        let nextDirections direction =
            if direction = Bottom then [Bottom; Left; Top; Right]
            elif direction = Left then [Left; Top; Right; Bottom]
            elif direction = Top then [Top; Right; Bottom; Left]
            else [Right; Bottom; Left; Top]

        let chooseNeighbor coordinate (unlinked : array<Coordinate>) =
                
            let tryFind pos =
                match grid.AdjacentNeighborAbstractCoordinate coordinate pos with
                | Some next -> unlinked |> Array.tryFind(fun c -> c = next), pos
                | _ -> None, pos

            if rng.NextDouble() < spiralWeight then
                if spiralLength >= spiralMaxLength then
                    let nextDirections = (nextDirections currentDirection)
                    currentDirection <- nextDirections.[1]
                    spiralLength <- 0

                let next =
                    nextDirections currentDirection
                    |> List.map(tryFind)
                    |> List.where(fun (c, _) -> c.IsSome)
                    |> List.map(fun (c, d) -> (c.Value, d))
                    |> List.tryHead

                match next with
                | Some (nextCoordinate, nextDirection) ->
                    if nextDirection <> currentDirection then
                        currentDirection <- nextDirection
                        spiralLength <- 0
                    if rng.NextDouble() > spiralUniformity then
                        let nextDirections = (nextDirections currentDirection)
                        currentDirection <- nextDirections.[rng.Next(nextDirections.Length)]
                        spiralLength <- 0
                    
                    spiralLength <- spiralLength + 1

                    nextCoordinate
                | None ->
                    unlinked.[rng.Next(unlinked.Length)]
            else
                unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }