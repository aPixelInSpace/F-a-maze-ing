// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core

type Grid<'G> =
    {
        TotalOfMazeCells : int
        Cell : Coordinate -> Cell
        CoordinatesPartOfMaze : Coordinate seq
        LinkCellsAtCoordinates : Coordinate -> Coordinate -> unit
        NeighborsThatAreLinked : bool -> Coordinate -> Coordinate seq
        RandomNeighborFrom : Random -> Coordinate -> Coordinate
        RandomCoordinatePartOfMazeAndNotLinked : Random -> Coordinate
        ToGrid : 'G
    }