// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Utils

    open System
    open System.Collections.Generic

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

    let memoize fn =
      let cache = Dictionary<_,_>()
      (fun a ->
        match cache.TryGetValue (a) with
        | true, v -> v
        | false, _ -> let v = fn a
                      cache.Add(a,v)
                      v)

    let memoize2 fn =
      let cache = Dictionary<_,_>()
      (fun a b ->
        match cache.TryGetValue ((a, b)) with
        | true, v -> v
        | false, _ -> let v = fn a b
                      cache.Add((a, b),v)
                      v)