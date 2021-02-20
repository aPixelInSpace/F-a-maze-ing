// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open System.Collections.Generic
open Mazes.Core

type NDimensionalStructure<'Grid, 'Position> =
    private
        {
            Dimensions : Dimension
            Slices2D : Dictionary<Dimension, IAdjacentStructure<'Grid, 'Position>>
            NonAdjacent2DNeighbors : N_NonAdjacentNeighbors
            Obstacles : N_Obstacles
        }

    member this.TotalOfMazeCells =
        this.Slices2D
        |> Seq.sumBy(fun kv -> kv.Value.TotalOfMazeCells)

    member this.ExistAt (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.ToDimension
        if this.Slices2D.ContainsKey(dimension) then
            this.Slices2D.Item(dimension).ExistAt nCoordinate.ToCoordinate2D
        else
            false

    member this.CoordinatesPartOfMaze =
        let cells (dimension : Dimension) (adjStruct : IAdjacentStructure<'Grid, 'Position>) =
            adjStruct.Cells
            |> Seq.filter(fun (_, coordinate) -> adjStruct.IsCellPartOfMaze coordinate)
            |> Seq.map(fun (_, coordinate) -> NCoordinate.create coordinate dimension)

        this.Slices2D
        |> Seq.collect(fun kv -> cells kv.Key kv.Value)