// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.BinaryTree

open System
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

type Direction =
    | Top
    | Right
    | Bottom
    | Left

    member this.Opposite =
        match this with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right

    member this.Position =
        match this with
        | Top -> Position.Top
        | Right -> Position.Right
        | Bottom -> Position.Bottom
        | Left -> Position.Left

    

let private carveRow
    // params
    (direction1 : Direction)
    (direction2 : Direction)
    (rng : Random)
    rngTotalWeight    
    rngPosition1Weight
    (grid : Grid<'G>)
    rIndex
    getRowInfo =
    
    let (startColumnIndex, increment, endColumnIndex) = getRowInfo
    
    for columnIndex in startColumnIndex .. increment .. endColumnIndex do

        let coordinate = { RIndex = rIndex; CIndex = columnIndex }
        let neighborCoordinate position =
            match (grid.Neighbor coordinate position) with
            | Some neighbor -> neighbor
            | None -> failwith "Binary Tree, unable to find the neighbor coordinate"
        let notExistNeighborCoordinate position = (grid.Neighbor coordinate position).IsNone
        
        // if the cell is not part of the maze, we do nothing
        if not (grid.IsCellPartOfMaze coordinate) then ()
        else

        let isPos1ALimit = (notExistNeighborCoordinate direction1.Position) || (grid.IsLimitAt coordinate (neighborCoordinate direction1.Position))
        let isPos2ALimit = (notExistNeighborCoordinate direction2.Position) || (grid.IsLimitAt coordinate (neighborCoordinate direction2.Position))
        
        let ifNotAtLimitLinkCellAtPosition = grid.IfNotAtLimitLinkCells coordinate

        // if we are in a corner 
        if isPos1ALimit &&  isPos2ALimit then
            ifNotAtLimitLinkCellAtPosition (neighborCoordinate direction1.Opposite.Position)
            ifNotAtLimitLinkCellAtPosition (neighborCoordinate direction2.Opposite.Position)
        else

        let linkCellAtPosition = grid.LinkCells coordinate

        // if the pos 1 is a limit then we always choose remove pos 2 (and the opposite pos 2 if possible)
        if isPos1ALimit then
            linkCellAtPosition (neighborCoordinate direction2.Position)
            ifNotAtLimitLinkCellAtPosition (neighborCoordinate direction2.Opposite.Position)
        else

        // if the pos 2 is a limit then we always choose remove pos 1 (and the opposite pos 1 if possible)
        if isPos2ALimit then
            linkCellAtPosition (neighborCoordinate direction1.Position)
            ifNotAtLimitLinkCellAtPosition (neighborCoordinate direction1.Opposite.Position)
        else

        // if pos 1 and pos 2 are both not a limit we flip a coin to decide which one we remove
        match rng.Next(rngTotalWeight) with
        | rng when rng < rngPosition1Weight ->
            linkCellAtPosition (neighborCoordinate direction1.Position)
        | _ ->
            linkCellAtPosition (neighborCoordinate direction2.Position)

let createMaze direction1 direction2 rngSeed rngDirection1Weight rngDirection2Weight (grid : unit -> Grid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let getRowInfo rowIndex =
        match direction1, direction2 with
        | _, Left | Left, _ -> (getIndex (grid.Dimension2Length rowIndex), -1, 0)
        | _ -> (0, 1, getIndex (grid.Dimension2Length rowIndex))

    let rngTotalWeight = rngDirection1Weight + rngDirection2Weight

    grid.GetRIndexes
    |> Seq.iter(fun rIndex ->
        carveRow
            // params
            direction1
            direction2
            rng
            rngTotalWeight
            rngDirection1Weight
            grid
            rIndex
            (getRowInfo rIndex))    

    { Grid = grid }