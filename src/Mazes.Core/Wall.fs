namespace Mazes.Core

type Position =
    | Top
    | Right
    | Bottom
    | Left

type WallType =
    | Normal
    | Border
    | Empty

type Wall = {
    WallType : WallType
    WallPosition : Position
}

module Position =
    let getOpposite position =
        match position with
        | Top -> Bottom
        | Right -> Left
        | Bottom -> Top
        | Left -> Right