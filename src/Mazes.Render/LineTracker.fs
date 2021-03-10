// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Render

open System.Collections.Generic

type LinePointsInt = (int * int) * (int * int)
type LinePointsFloat = (float * float) * (float * float)

type LineTracker<'Key> =
    private
        {
            Container : HashSet<'Key>   
        }

    member this.ContainsLine key =
        this.Container.Contains(key)

    member this.Add key =
        this.Container.Add(key) |> ignore

    static member createEmpty =
        { Container = HashSet<_>() }