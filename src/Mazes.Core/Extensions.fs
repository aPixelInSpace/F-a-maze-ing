namespace Mazes.Core.Extensions

module Array2D =
    let extractByRows array2d =
        let numberRowsIndex = (array2d |> Array2D.length1) - 1
        seq {
            for i in 0 .. numberRowsIndex do
                yield array2d.[i, *]
        }

    let extractByColumns array2d =
        let numberColumnsIndex = (array2d |> Array2D.length2) - 1
        seq {
            for i in 0 .. numberColumnsIndex do
                yield array2d.[*, i]
        }