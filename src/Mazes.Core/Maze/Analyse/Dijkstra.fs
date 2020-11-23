module Mazes.Core.Maze.Analyse.Dijkstra

open Mazes.Core
open Mazes.Core.Maze

let createMap root maze =
    let distanceFromRoot = Array2D.zeroCreate maze.Grid.Canvas.NumberOfRows maze.Grid.Canvas.NumberOfColumns
    
    

    { Root = root; DistancesFromRoot = null }