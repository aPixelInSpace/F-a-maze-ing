﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

type GridArray2DType =
    | Orthogonal
    | Hexagonal
    | Triangular
    | OctagonalSquare
    | PentagonalCairo

type GridArrayOfAType =
    | Polar