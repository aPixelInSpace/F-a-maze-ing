module Mazes.Core.Position

type Position =
    | Top
    | Right
    | Bottom
    | Left

module Position =
    let getOpposite position =
        match position with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right