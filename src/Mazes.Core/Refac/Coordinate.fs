// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac

[<Struct>]
type Coordinate2D =
    {
        RIndex : int
        CIndex : int
    }

    override this.ToString() =
        $"{this.RIndex};{this.CIndex}"

/// Dimension, starting from the third
type Dimension = Dimension of int array

module Dimension =
    
    let value (Dimension d) = d

/// N dimensions coordinate
[<Struct>]
type NCoordinate =
    {
        Coordinate2D : Coordinate2D
        Dimension : Dimension
    }

    override this.ToString() =
        this.Coordinate2D.ToString() + ";" +
        (("", (this.Dimension |> Dimension.value)) ||> Array.fold(fun s n -> s + $"{n};"))

module NCoordinate =

    let create (dimension : Dimension) (coordinate : Coordinate2D) =
        {
            Coordinate2D = coordinate
            Dimension = dimension
        }

    let isSame2DDimension nCoordinate otherNCoordinate =
        nCoordinate.Dimension = otherNCoordinate.Dimension

    let createFrom2D (coordinate : Coordinate2D) =
        create ([| 0 |] |> Dimension) coordinate