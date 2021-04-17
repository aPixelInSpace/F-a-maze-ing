// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Structure

open System
open Mazes.Core

// todo : this interface does way too much, it must be broken down
type IAdjacentStructure<'Structure, 'Position> =
    abstract member TotalOfCells : int
    abstract member TotalOfMazeCells : int
    abstract member RIndexes : int seq
    abstract member CIndexes : int seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AdjustedCoordinate : coordinate:Coordinate2D -> Coordinate2D
    abstract member ExistAt : coordinate:Coordinate2D -> bool
    abstract member AdjustedExistAt : coordinate:Coordinate2D -> bool
    abstract member CoordinatesPartOfMaze : Coordinate2D seq
    abstract member RandomCoordinatePartOfMazeAndNotConnected : rng:Random -> Coordinate2D
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> bool
    abstract member Cell : coordinate:Coordinate2D -> ICell<'Position>
    /// Returns every cells in the structure
    abstract member Cells : (ICell<'Position> * Coordinate2D) seq
    /// Given a coordinate, returns true if the cell is marked as part of the maze, false otherwise
    abstract member IsCellPartOfMaze : coordinate:Coordinate2D -> bool
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : coordinate:Coordinate2D -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> bool
    /// Given a coordinate, returns the coordinates of the neighbors
    abstract member Neighbors : coordinate:Coordinate2D -> Coordinate2D seq
    /// Returns the coordinate of the neighbor at the position, if there are multiple neighbors then returns the last one
    abstract member Neighbor : Coordinate2D -> Position -> Coordinate2D option
    abstract member VirtualNeighbor : Coordinate2D -> Position -> Coordinate2D option
    /// Given two coordinates, updates the connection between them
    abstract member UpdateConnection : connectionType:ConnectionType -> coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> unit
    abstract member IfNotAtLimitUpdateConnection : connectionType:ConnectionType -> coordinate:Coordinate2D -> otherCoordinate:Coordinate2D -> unit
    abstract member WeaveCoordinates : Coordinate2D seq -> (Coordinate2D * Coordinate2D) seq
    /// Special function to "open" the grid ie. make a connection to the outside if possible, usually to make an entrance and an exit
    abstract member OpenCell : Coordinate2D -> unit
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstCellPartOfMaze : Coordinate2D
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastCellPartOfMaze : Coordinate2D
    abstract member ToSpecializedStructure : 'Structure