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

/// Coordinate for N dimensions
[<Struct>]
type NCoordinate =
    {
        DIndexes : int array
    }

    override this.ToString() =
        ("", this.DIndexes) ||> Array.fold(fun s n -> s + $"{n};")