// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

/// 2D coordinate
[<Struct>]
type Coordinate =
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
    private
        {
            DIndexes : int array
        }

    member this.ToCoordinate2D =
        { RIndex = this.DIndexes.[0]; CIndex = this.DIndexes.[1] }

    member this.ToDimension : Dimension =
        Array.sub this.DIndexes 2 (this.DIndexes.Length - 2)

    member this.IsSame2DDimension (otherNCoordinate : NCoordinate) =
        this.ToDimension = otherNCoordinate.ToDimension

    override this.ToString() =
        ("", this.DIndexes) ||> Array.fold(fun s n -> s + $"{n};")

module NCoordinate =

    let create (dimension : Dimension) (coordinate : Coordinate) =
        {
            DIndexes = Array.append [| coordinate.RIndex; coordinate.CIndex |] dimension
        }