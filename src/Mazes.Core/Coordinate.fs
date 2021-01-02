// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

[<Struct>]
type Coordinate =
    {
        RIndex : int
        CIndex : int
    }

    override this.ToString() =
        $"{this.RIndex};{this.CIndex}"