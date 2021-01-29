// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core

// todo : clean the interface and remove the unnecessary functions
type IGrid<'G> =
    abstract member TotalOfMazeCells : int
    abstract member EveryCoordinatesPartOfMaze : Coordinate seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AddCostForCoordinate : Cost -> coordinate:Coordinate -> unit
    abstract member CostOfCoordinate : coordinate:Coordinate -> Cost
    abstract member AdjacentNeighborAbstractCoordinate : coordinate:Coordinate -> position:Position -> Coordinate option
    /// Returns true if the cell has at least one link to another cell, false otherwise
    abstract member IsCellLinked : coordinate:Coordinate -> bool
    abstract member ExistAt : coordinate:Coordinate -> bool
    abstract member GetAdjustedExistAt : coordinate:Coordinate -> bool
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Indicate if the cell is part of maze
    abstract member IsCellPartOfMaze : coordinate:Coordinate -> bool
    abstract member GetRIndexes : int seq
    abstract member GetCIndexes : int seq
    abstract member GetAdjustedCoordinate : coordinate:Coordinate -> Coordinate
    /// Returns every cell that are part of the maze
    abstract member CoordinatesPartOfMaze : Coordinate seq
    /// Links two cells together (allows a passage between them)
    abstract member LinkCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Checks if the two cells can be linked, if so then link them, if not then does nothing
    abstract member IfNotAtLimitLinkCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Puts a border between two cells
    abstract member PutBorderBetweenCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Returns the coordinate of the neighbor, if there are multiple neighbor then returns the last one
    abstract member AdjacentNeighbor : coordinate:Coordinate -> position:Position -> Coordinate option
    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY WITH the coordinate
    abstract member NeighborsThatAreLinked : isLinked:bool -> coordinate:Coordinate -> Coordinate seq
    abstract member AddUpdateNonAdjacentNeighbor : Coordinate -> Coordinate -> WallType -> unit
    /// Returns the neighbors coordinates that are linked WITH the coordinate
    abstract member LinkedNeighbors : coordinate:Coordinate -> Coordinate seq
    /// Returns the neighbors coordinates that are NOT linked WITH the coordinate
    abstract member NotLinkedNeighbors : coordinate:Coordinate -> Coordinate seq
    /// Returns a random neighbor that is inside the bound of the grid (doesn't check if the neighbor is linked or not)
    abstract member RandomNeighbor : rng:Random -> coordinate:Coordinate -> Coordinate
    /// Returns a random coordinate from the grid that is part of the maze but isn't yet linked
    abstract member RandomCoordinatePartOfMazeAndNotLinked : rng:Random -> Coordinate
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstPartOfMazeZone : Coordinate
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastPartOfMazeZone : Coordinate
    /// Returns the string representation of the grid
    abstract member ToString : string
    /// Returns the sub-typed grid
    abstract member ToSpecializedGrid : 'G