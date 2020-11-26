﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License.md in the project root for license information.

module Mazes.Core.Canvas.Shape.Rectangle

open Mazes.Core
open Mazes.Core.Canvas

let create numberOfRows numberOfColumns =
    let zones = Array2D.create numberOfRows numberOfColumns (Zone.create true)
    
    { Zones = zones; }