// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Utils

    open System
    open Microsoft.FSharp.Reflection

    /// Returns a Key that is the same if the items are the same without considering the order
    let getKey (item1, item2) =
        if item1 >= item2 then (item1, item2) else (item2, item1)

    /// Shuffle and MODIFY the parameter array using the Fisher–Yates shuffle algorithm
    /// Returns the parameter array shuffled, NOT A NEW INSTANCE 
    let shuffle (rng : Random) (array : array<'T>) =
        for i in 0 .. array.Length - 1 do
            let rnd = rng.Next(i + 1)
            let temp = array.[rnd]
            array.[rnd] <- array.[i]
            array.[i] <- temp

        array

    // https://gist.github.com/curtnichols/67e370f21370430fcc54cf43f67273d3
    /// Let's enumerate the cases of a discriminated union.
    /// Assumes none of the cases have fields.
    let seqOfUnionCases<'UnionType> () =

        FSharpType.GetUnionCases typeof<'UnionType>
        |> Seq.map (fun caseInfo -> FSharpValue.MakeUnion(caseInfo, [||]) :?> 'UnionType)

    /// Let's enumerate the cases of a discriminated union.
    /// Assumes none of the cases have fields.
    let arrayOfUnionCases<'UnionType> () = seqOfUnionCases<'UnionType> () |> Seq.toArray