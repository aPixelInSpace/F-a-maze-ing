// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

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