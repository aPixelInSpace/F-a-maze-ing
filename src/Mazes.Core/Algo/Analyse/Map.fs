namespace Mazes.Core.Algo.Analyse

open Mazes.Core

type Distance = Distance of int

type DistanceFromRoot =
    | Distance
    | Unknown

type CellMap = {    
    DistanceFromRoot : int
    Neighbors : GridCoordinate[]
}

type Map = {
    Root : GridCoordinate
    DistancesFromRoot : CellMap[,]
}