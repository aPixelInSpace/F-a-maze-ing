// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System.Collections.Generic
open Mazes.Core.Refac

type NDimensionalStructure =
    private
        {
            Structure : Dictionary<Dimension, Grid>
            CoordinateConnections : CoordinateConnections
            Obstacles : Obstacles
        }

module NDimensionalStructure =

    let slice2D n dimension =
        n.Structure.Item(dimension)

    let firstSlice2D n =
        let firstDimension =
            n.Structure.Keys
            |> Seq.sort
            |> Seq.head

        (firstDimension, slice2D n firstDimension)

    let totalOfMazeCells n =
        n.Structure
        |> Seq.sumBy(fun kv -> Grid.totalOfMazeCells kv.Value)

    let existAt n (nCoordinate : NCoordinate) =
        let dimension = nCoordinate.Dimension
        if n.Structure.ContainsKey(dimension) then
            Grid.existAt (slice2D n dimension) nCoordinate.Coordinate2D
        else
            false