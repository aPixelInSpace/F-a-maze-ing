module Mazes.Core.Canvas.Shape.Rectangle

open Mazes.Core
open Mazes.Core.Canvas

let create numberOfRows numberOfColumns =
    let zones = Array2D.create numberOfRows numberOfColumns (Zone.create true)
    
    { Zones = zones; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }