// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core
open Mazes.Core.Position

type Grid<'G> =
    {
        TotalOfMazeCells : int
        NumberOfRows : int
        NumberOfColumns : int
        Cell : Coordinate -> Cell
        IsLimitAt : Coordinate -> Position -> bool
        IsCellPartOfMaze : Coordinate -> bool
        GetCellsByRows : Cell[] seq
        GetCellsByColumns : Cell[] seq
        CoordinatesPartOfMaze : Coordinate seq
        LinkCellAtPosition : Coordinate -> Position -> unit
        IfNotAtLimitLinkCellAtPosition : Coordinate -> Position -> unit
        LinkCellsAtCoordinates : Coordinate -> Coordinate -> unit
        NeighborsThatAreLinked : bool -> Coordinate -> Coordinate seq
        LinkedNeighborsWithCoordinates : Coordinate -> Coordinate seq
        RandomNeighborFrom : Random -> Coordinate -> Coordinate
        RandomCoordinatePartOfMazeAndNotLinked : Random -> Coordinate
        GetFirstTopLeftPartOfMazeZone : Coordinate
        GetFirstBottomRightPartOfMazeZone : Coordinate
        ToGrid : 'G
    }

type IGrid =
    abstract member TotalOfMazeCells : int
    abstract member NumberOfRows : int
    abstract member NumberOfColumns : int
    abstract member Cell : coordinate:Coordinate -> Cell
    abstract member IsLimitAt : coordinate:Coordinate -> position:Position -> bool
    abstract member IsCellPartOfMaze : coordinate:Coordinate -> bool
    abstract member GetCellsByRows : Cell[] seq
    abstract member GetCellsByColumns : Cell[] seq
    abstract member CoordinatesPartOfMaze : Coordinate seq
    abstract member LinkCellAtPosition : coordinate:Coordinate -> position:Position -> unit
    abstract member IfNotAtLimitLinkCellAtPosition : coordinate:Coordinate -> position:Position -> unit
    abstract member LinkCellsAtCoordinates : coordinate:Coordinate -> otherCoordinate:Coordinate -> unit
    /// Returns the neighbors coordinates that are linked, NOT NECESSARILY WITH the coordinate
    abstract member NeighborsThatAreLinked : isLinked:bool -> coordinate:Coordinate -> Coordinate seq
    abstract member LinkedNeighborsWithCoordinates : coordinate:Coordinate -> Coordinate seq
    abstract member RandomNeighborFrom : rng:Random -> coordinate:Coordinate -> Coordinate
    abstract member RandomCoordinatePartOfMazeAndNotLinked : rng:Random -> Coordinate
    abstract member GetFirstTopLeftPartOfMazeZone : Coordinate
    abstract member GetFirstBottomRightPartOfMazeZone : Coordinate