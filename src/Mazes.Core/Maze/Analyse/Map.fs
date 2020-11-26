// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

namespace Mazes.Core.Maze.Analyse

open Mazes.Core

type CellMap = {    
    DistanceFromRoot : int option
    Neighbors :  seq<Coordinate> option
}

type Map = {
    Root : Coordinate
    DistancesFromRoot : CellMap[,]
    TotalZonesAccessibleFromRoot : int
}