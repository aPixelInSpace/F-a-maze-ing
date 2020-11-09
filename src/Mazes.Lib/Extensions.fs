namespace Mazes.Lib.Extensions

module Array2D =
    let extractRows array2d =
        let numberRows = (array2d |> Array2D.length1) - 1
        seq {
            for i in 0 .. numberRows do
                yield array2d.[i, *]
        }