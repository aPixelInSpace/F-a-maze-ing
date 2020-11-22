namespace Mazes.Core.Maze.Analyse

open Mazes.Core

type Distance = Distance of int

type DistanceFromRoot =
    | Distance
    | Unknown

type CellMap = {    
    DistanceFromRoot : int
    Neighbors : Coordinate[]
}

type Map = {
    Root : Coordinate
    DistancesFromRoot : CellMap[,]
}