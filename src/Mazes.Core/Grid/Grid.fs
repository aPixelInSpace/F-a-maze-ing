// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System
open Mazes.Core

type Grid<'G> =
    {
        LinkCellsAtCoordinates : Coordinate -> Coordinate -> unit
        NeighborsThatAreLinked : bool -> Coordinate -> Coordinate seq
        RandomCoordinatePartOfMazeAndNotLinked : Random -> Coordinate
        ToGrid : 'G
    }