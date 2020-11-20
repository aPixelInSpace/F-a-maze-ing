namespace Mazes.Core

open Mazes.Core.Position

type WallType =
    | Normal
    | Border
    | Empty

type Wall = {
    WallType : WallType
    WallPosition : Position
}