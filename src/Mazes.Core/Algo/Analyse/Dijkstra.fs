module Mazes.Core.Algo.Analyse.Dijkstra

open Mazes.Core

let createMap root maze =
    let distanceFromRoot = Array2D.zeroCreate maze.Grid.NumberOfRows maze.Grid.NumberOfColumns
    
    

    { Root = root; DistancesFromRoot = null }