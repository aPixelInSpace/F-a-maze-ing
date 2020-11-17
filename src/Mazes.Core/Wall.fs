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