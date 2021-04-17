// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

/// 2D coordinate
[<Struct>]
type Coordinate2D =
    {
        RIndex : int
        CIndex : int
    }

    override this.ToString() =
        $"{this.RIndex};{this.CIndex}"

/// Indicate the dimension, starting from the third
type Dimension = int array

/// Coordinate for N dimensions
[<Struct>]
type NCoordinate =
    {
        Coordinate2D : Coordinate2D
        Dimension : Dimension
    }

    member this.IsSame2DDimension (otherNCoordinate : NCoordinate) =
        this.Dimension = otherNCoordinate.Dimension

    override this.ToString() =
        this.Coordinate2D.ToString() + ";" +
        (("", this.Dimension) ||> Array.fold(fun s n -> s + $"{n};"))

module NCoordinate =

    let create (dimension : Dimension) (coordinate : Coordinate2D) =
        {
            Coordinate2D = coordinate
            Dimension = dimension
        }

    let createFrom2D (coordinate : Coordinate2D) =
        create [| 0 |] coordinate