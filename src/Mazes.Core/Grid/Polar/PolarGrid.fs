﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Polar.PolarGrid

open Mazes.Core.Grid.Polar.PolarCell

type PolarGrid =
    {
        Cells : PolarCell[][]
    }