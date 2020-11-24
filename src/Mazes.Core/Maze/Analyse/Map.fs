namespace Mazes.Core.Maze.Analyse

open Mazes.Core

type CellMap = {    
    DistanceFromRoot : int option
    Neighbors :  seq<Coordinate> option
}

type Map = {
    Root : Coordinate
    DistancesFromRoot : CellMap[,]
    AccessibleFromRoot : int
}