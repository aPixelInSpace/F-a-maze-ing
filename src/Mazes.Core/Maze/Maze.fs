// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core.Analysis
open Mazes.Core.Grid

type Maze =
    {    
        Grid : Grid
    }

    member this.createDijkstraMap rootCoordinate =
        Dijkstra.Map.create this.Grid.NavigableNeighborsCoordinates this.Grid.Canvas.NumberOfRows this.Grid.Canvas.NumberOfColumns rootCoordinate

type MazeInfo = {
    Name : string
}