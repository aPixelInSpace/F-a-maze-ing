// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure
open Mazes.Core.Refac.Structure.NDimensionalStructure
open Mazes.Core.Refac.Maze

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

    let baseAlgorithmNDimensionalStructure ndStruct =

        baseAlgorithm
            (connectedWithNeighbors ndStruct false)
            (updateConnectionState ndStruct ConnectionState.Open)

    let baseAlgorithmAdjacentStructure grid =

        let unConnectedNeighbors coordinate =
            Grid.neighbors grid coordinate
            |> Seq.filter(fun neighbor ->
                not (Grid.isCellConnected grid neighbor))
            |> Seq.distinct

        baseAlgorithm
            unConnectedNeighbors
            (Grid.updateConnectionState grid Open)

module GrowingTreeMixRandomAndLast =

    let createMaze rngSeed longPassages ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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

        GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeMixOldestAndLast =

    let createMaze rngSeed longPassages ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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

        GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeMixChosenRandomAndLast =

    let createMaze rngSeed longPassages ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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

        GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module GrowingTreeDirection =

    let createMaze rngSeed toRightWeight toBottomWeight toLeftWeight ndStruct =

        let slice2D = snd (firstSlice2D ndStruct)

        let bottomWeight = toRightWeight + toBottomWeight
        let leftWeight = bottomWeight + toLeftWeight

        let rng = Random(rngSeed)

        let randomStartCoordinate = Grid.randomCoordinatePartOfMazeAndNotConnected slice2D rng

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

        GrowingTree.baseAlgorithmAdjacentStructure slice2D randomStartCoordinate count add next remove chooseNeighbor

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

    let toDisposition g spiralDirection =
        match Grid.gridStructure g with
        | GridStructureArray2D gridStructure ->
            match gridStructure with
            | GridArray2DOrthogonal _ ->
                match spiralDirection with
                | Right -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Right)
                | Bottom -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Bottom)
                | Left -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Left)
                | Top -> DispositionArray2DChoice (DispositionArray2D.Orthogonal OrthogonalDisposition.Top)

    let toSpiralDirection pos =
        match pos with
        | DispositionArray2DChoice pos ->
            match pos with
            | DispositionArray2D.Orthogonal pos ->
                match pos with
                | OrthogonalDisposition.Right -> Right
                | OrthogonalDisposition.Bottom -> Bottom
                | OrthogonalDisposition.Left -> Left
                | OrthogonalDisposition.Top -> Top

    let createMaze rngSeed spiralWeight spiralUniformity spiralMaxLength spiralRevolution ndStruct =

        let slice2D = snd (firstSlice2D ndStruct)
        
        let toDisposition = toDisposition slice2D

        let rng = Random(rngSeed)

        let randomStartCoordinate = Grid.randomCoordinatePartOfMazeAndNotConnected slice2D rng

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

        let chooseNeighbor coordinate (unlinked : array<_>) =

            let tryFind spiralDirection =
                let pos = toDisposition spiralDirection
                match Grid.neighbor slice2D coordinate pos with
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

                    if nextPosition <> (toDisposition currentDirection) then
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

        GrowingTree.baseAlgorithmAdjacentStructure slice2D randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }