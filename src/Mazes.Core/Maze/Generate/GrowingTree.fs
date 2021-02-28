// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Structure

module GrowingTree =

    let baseAlgorithm
            notConnectedNeighbors
            openConnection
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor =

        add startCoordinate

        while count() > 0 do
            let active = next()

            let unlinked =
                active
                |> notConnectedNeighbors
                |> Seq.toArray

            if unlinked.Length > 0 then
                let neighbor = chooseNeighbor active unlinked
                openConnection active neighbor
                add neighbor
            else
                remove active

    let baseAlgorithmNDimensionalStructure
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor
            (ndStruct : NDimensionalStructure<_,_>) =

        baseAlgorithm
            (ndStruct.ConnectedNeighbors false)
            (ndStruct.UpdateConnection Open)
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor

    let baseAlgorithmAdjacentStructure
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor
            (ndStruct : IAdjacentStructure<_,_>) =

        let unConnectedNeighbors coordinate =
            ndStruct.Neighbors coordinate
            |> Seq.filter(fun neighbor ->
                not (ndStruct.IsCellConnected neighbor))
            |> Seq.distinct
            
            
        baseAlgorithm
            unConnectedNeighbors
            (ndStruct.UpdateConnection Open)
            startCoordinate
            count
            add
            next
            remove
            chooseNeighbor

module GrowingTreeMixRandomAndLast =

    let createMaze rngSeed longPassages (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        // SortedList is not a great replacement for a Stack,
        // but at least we can remove an item at a given index
        let actives = SortedList<int, _>()

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

        ndStruct |> GrowingTree.baseAlgorithmNDimensionalStructure randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeMixOldestAndLast =

    let createMaze rngSeed longPassages (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = SortedList<int, _>()

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

        ndStruct |> GrowingTree.baseAlgorithmNDimensionalStructure randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeMixChosenRandomAndLast =

    let createMaze rngSeed longPassages (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = SortedList<int, _>()
        let mutable chosenCoordinate : _ option = None

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

        ndStruct |> GrowingTree.baseAlgorithmNDimensionalStructure randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeDirection =

    let createMaze rngSeed toRightWeight toBottomWeight toLeftWeight (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let slice2D = snd ndStruct.FirstSlice2D

        let bottomWeight = toRightWeight + toBottomWeight
        let leftWeight = bottomWeight + toLeftWeight

        let rng = Random(rngSeed)

        let randomStartCoordinate = slice2D.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = Stack<_>()

        let count () = actives.Count

        let add coordinate =
            actives.Push(coordinate)

        let next () =
            actives.Peek()

        let remove _ =
            actives.Pop() |> ignore

        let chooseNeighbor _ (unlinked : array<_>) =
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

        slice2D |> GrowingTree.baseAlgorithmAdjacentStructure randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeSpiral =

    type SpiralDirection =
        | Right
        | Bottom
        | Left
        | Top

    type SpiralRevolution =
        | Clockwise
        | CounterClockwise

    let createMaze rngSeed spiralWeight spiralUniformity spiralMaxLength spiralRevolution (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let slice2D = snd ndStruct.FirstSlice2D

        let rng = Random(rngSeed)

        let randomStartCoordinate = slice2D.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = Stack<_>()

        let count () = actives.Count

        let add coordinate =
            actives.Push(coordinate)

        let next () =
            actives.Peek()

        let remove _ =
            actives.Pop() |> ignore

        let mutable spiralLength = 0
        let mutable currentDirection = Right
        let mutable currentRevolution = Clockwise

        let baseDirectionsCw = [Right; Bottom; Left; Top]
        let baseDirectionsCcw = [Right; Top; Left; Bottom]

        let nextDirections direction revolution =
            let shift list n =
                list |> List.permute (fun index -> (index + n) % list.Length)

            match direction, revolution with
            | Right, Clockwise -> baseDirectionsCw
            | Right, CounterClockwise -> baseDirectionsCcw
            | Bottom, Clockwise -> shift baseDirectionsCw 3
            | Bottom, CounterClockwise -> shift baseDirectionsCcw 1
            | Left, Clockwise -> shift baseDirectionsCw 2
            | Left, CounterClockwise -> shift baseDirectionsCcw 2
            | Top, Clockwise -> shift baseDirectionsCw 1
            | Top, CounterClockwise -> shift baseDirectionsCcw 3

        let toPosition spiralDirection =
            match spiralDirection with
            | Right -> Position.Right
            | Bottom -> Position.Bottom
            | Left -> Position.Left
            | Top -> Position.Top

        let toSpiralDirection position =
            match position with
            | Position.Right -> Right
            | Position.Bottom -> Bottom
            | Position.Left -> Left
            | Position.Top -> Top

        let chooseNeighbor coordinate (unlinked : array<_>) =

            let tryFind spiralDirection =
                let pos = toPosition spiralDirection
                match slice2D.Neighbor coordinate pos with
                | Some next -> unlinked |> Array.tryFind(fun c -> c = next), pos
                | _ -> None, pos

            if rng.NextDouble() < spiralWeight then

                if rng.NextDouble() > spiralRevolution then
                    currentRevolution <- Clockwise
                else
                    currentRevolution <- CounterClockwise

                if spiralLength >= spiralMaxLength then
                    let nextDirections = nextDirections currentDirection currentRevolution
                    currentDirection <- nextDirections.[1]
                    spiralLength <- 0

                let next =
                    nextDirections currentDirection currentRevolution
                    |> List.map(tryFind)
                    |> List.where(fun (c, _) -> c.IsSome)
                    |> List.map(fun (c, d) -> (c.Value, d))
                    |> List.tryHead

                match next with
                | Some (nextCoordinate, nextPosition) ->

                    if nextPosition <> (toPosition currentDirection) then
                        currentDirection <- toSpiralDirection nextPosition
                        spiralLength <- 0

                    if rng.NextDouble() > spiralUniformity then
                        let nextDirections = nextDirections currentDirection currentRevolution
                        currentDirection <- nextDirections.[rng.Next(nextDirections.Length)]
                        spiralLength <- 0
                    
                    spiralLength <- spiralLength + 1

                    nextCoordinate
                | None ->
                    unlinked.[rng.Next(unlinked.Length)]
            else
                unlinked.[rng.Next(unlinked.Length)]

        slice2D |> GrowingTree.baseAlgorithmAdjacentStructure randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }