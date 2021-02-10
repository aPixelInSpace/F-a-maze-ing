// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.GridNew

open System
open Mazes.Core

type IGrid<'Grid> =
    abstract member TotalOfMazeCells : int
    abstract member RIndexes : int seq
    abstract member CIndexes : int seq
    abstract member Dimension1Boundaries : dimension2Index:int -> (int * int)
    abstract member Dimension2Boundaries : dimension1Index:int -> (int * int)
    abstract member AdjustedCoordinate : coordinate:Coordinate -> Coordinate
    abstract member ExistAt : coordinate:Coordinate -> bool
    abstract member AdjustedExistAt : coordinate:Coordinate -> bool
    abstract member CoordinatesPartOfMaze : Coordinate seq
    abstract member RandomCoordinatePartOfMazeAndNotConnected : rng : Random -> Coordinate
    /// Returns true if it is not possible to navigate from a coordinate to another coordinate (for example if there is a border between the two cells) 
    abstract member IsLimitAt : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Returns all the neighbors adjacent and non adjacent
    abstract member Neighbors : Coordinate -> Coordinate seq
    /// Returns all the adjacent neighbors
    abstract member AdjacentNeighbors : Coordinate -> Coordinate seq
    /// Returns the coordinates of the adjacent neighbors at the position
    abstract member AdjacentNeighbor : Coordinate -> Position -> Coordinate option
    abstract member AdjacentVirtualNeighbor : Coordinate -> Position -> Coordinate option
    /// Given a coordinate, returns true if the cell is part of the maze, false otherwise
    abstract member IsCellPartOfMaze : Coordinate -> bool
    /// Given a coordinate, returns true if the cell has at least one connection open, false otherwise
    abstract member IsCellConnected : Coordinate -> bool
    /// Given two coordinates, returns true if they have their connection open, false otherwise
    abstract member AreConnected : coordinate:Coordinate -> otherCoordinate:Coordinate -> bool
    /// Returns the neighbors coordinates that are or not connected NOT NECESSARILY WITH the coordinate
    abstract member ConnectedNeighbors : isConnected:bool -> coordinate:Coordinate -> Coordinate seq
    /// Returns the neighbors coordinates that are or not connected WITH the coordinate
    abstract member ConnectedWithNeighbors : isConnected:bool -> coordinate:Coordinate -> Coordinate seq
    abstract member UpdateConnection : ConnectionType -> Coordinate -> Coordinate -> unit
    abstract member IfNotAtLimitUpdateConnection : ConnectionType -> Coordinate -> Coordinate -> unit
    abstract member CostOfCoordinate : coordinate:Coordinate -> Cost
    /// Returns the first (arbitrary) coordinate that is part of the maze
    abstract member GetFirstCellPartOfMaze : Coordinate
    /// Returns the last (arbitrary) coordinate that is part of the maze
    abstract member GetLastCellPartOfMaze : Coordinate
    abstract member ToSpecializedGrid : 'Grid