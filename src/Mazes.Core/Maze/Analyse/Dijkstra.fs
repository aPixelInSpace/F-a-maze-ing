module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Maze

let createMap root maze =
    let distanceFromRoot = Array2D.zeroCreate maze.Grid.NumberOfRows maze.Grid.NumberOfColumns
    
    

    { Root = root; DistancesFromRoot = null }