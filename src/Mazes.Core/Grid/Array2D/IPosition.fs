﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D

open Mazes.Core

type IPosition<'Position> =

    abstract member Opposite : 'Position
    abstract member Values : 'Position array
    abstract member Map : Position -> 'Position