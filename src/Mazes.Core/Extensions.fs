namespace Mazes.Core.Extensions

module Array2D =
    let extractByRows array2d =
        let numberRows = (array2d |> Array2D.length1) - 1
        seq {
            for i in 0 .. numberRows do
                yield array2d.[i, *]
        }