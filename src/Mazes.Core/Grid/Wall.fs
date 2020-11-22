namespace Mazes.Core.Grid

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

type WallType =
    | Normal
    | Border
    | Empty

type Wall = {
    WallType : WallType
    WallPosition : Position
}