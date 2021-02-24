// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core

type IAdjacentStructure<'Structure, 'Position> =
    abstract member TotalOfCells : int
    abstract member TotalOfMazeCells : int
    abstract member RIndexes : int seq
    abstract member CIndexes : int seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AdjustedCoordinate : coordinate:Coordinate -> Coordinate
    abstract member ExistAt : coordinate:Coordinate -> bool
    abstract member AdjustedExistAt : coordinate:Coordinate -> bool
    abstract member CoordinatesPartOfMaze : Coordinate seq
    abstract member RandomCoordinatePartOfMazeAndNotConnected : rng:Random -> Coordinate
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    abstract member Cell : coordinate:Coordinate -> ICell<'Position>
    /// Returns every cells in the structure
    abstract member Cells : (ICell<'Position> * Coordinate) seq
    /// Given a coordinate, returns true if the cell is marked as part of the maze, false otherwise
    abstract member IsCellPartOfMaze : coordinate:Coordinate -> bool
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : coordinate:Coordinate -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Given a coordinate, returns the coordinates of the neighbors
    abstract member Neighbors : coordinate:Coordinate -> Coordinate seq
    /// Returns the coordinate of the neighbor at the position, if there are multiple neighbors then returns the last one
    abstract member Neighbor : Coordinate -> Position -> Coordinate option
    abstract member VirtualNeighbor : Coordinate -> Position -> Coordinate option
    /// Given two coordinates, updates the connection between them
    abstract member UpdateConnection : connectionType:ConnectionType -> coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    abstract member IfNotAtLimitUpdateConnection : connectionType:ConnectionType -> coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    abstract member WeaveCoordinates : Coordinate seq -> (Coordinate * Coordinate) seq
    /// Special function to "open" the grid ie. make a connection to the outside if possible, usually to make an entrance and an exit
    abstract member OpenCell : Coordinate -> unit
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstCellPartOfMaze : Coordinate
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastCellPartOfMaze : Coordinate
    abstract member ToSpecializedStructure : 'Structure