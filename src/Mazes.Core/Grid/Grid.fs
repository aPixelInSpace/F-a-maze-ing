// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core
open Mazes.Core.Position

type Grid<'G> =
    abstract member TotalOfMazeCells : int
    abstract member NumberOfRows : int
    abstract member NumberOfColumns : int
    abstract member Dimension2Length : dimension1Index:int -> int
    /// Returns true if the cell has at least one link to another cell, false otherwise
    abstract member IsCellLinked : coordinate:Coordinate -> bool
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Indicate if the cell is part of maze
    abstract member IsCellPartOfMaze : coordinate:Coordinate -> bool
    abstract member GetRIndexes : int seq
    abstract member GetCIndexes : int seq
    /// Returns every cell that are part of the maze
    abstract member CoordinatesPartOfMaze : Coordinate seq
    /// Links two cells together (allows a passage between them)
    abstract member LinkCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Checks if the two cells can be linked, if so then link them, if not then does nothing
    abstract member IfNotAtLimitLinkCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Puts a border between two cells
    abstract member PutBorderBetweenCells : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Returns the coordinate of the neighbor, if there are multiple neighbor then returns the last one
    abstract member Neighbor : coordinate:Coordinate -> position:Position -> Coordinate option
    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY WITH the coordinate
    abstract member NeighborsThatAreLinked : isLinked:bool -> coordinate:Coordinate -> Coordinate seq
    /// Returns the neighbors coordinates that are linked WITH the coordinate
    abstract member LinkedNeighbors : coordinate:Coordinate -> Coordinate seq
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
    /// Returns the subtyped grid
    abstract member ToSpecializedGrid : 'G