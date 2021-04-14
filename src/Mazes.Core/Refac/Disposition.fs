﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac

type OrthogonalDisposition =
    | Left
    | Top
    | Right
    | Bottom

type PolarDisposition =
    | Inward
    | Outward
    /// Clockwise
    | Cw
    /// Counter-clockwise
    | Ccw

type HexagonalDisposition =
    | TopLeft
    | Top
    | TopRight
    | BottomLeft
    | Bottom
    | BottomRight

type TriangularDisposition =
    | Left
    | Top
    | Right
    | Bottom

type OctagonalSquareDisposition =
    | Left
    | TopLeft
    | Top
    | TopRight
    | Right
    | BottomLeft
    | Bottom
    | BottomRight

/// Typical left, top ... doesn't make really sense here (because the form has four rotated forms), so
/// S is the small side of the congruent convex pentagon
/// then clockwise : A, B, C and D
type PentagonalCairoDisposition =
    /// Small side
    | S
    | A
    | B
    | C
    | D

type DispositionArray2D =
    | Orthogonal of OrthogonalDisposition
    | Hexagonal of HexagonalDisposition
    | Triangular of TriangularDisposition
    | OctagonalSquare of OctagonalSquareDisposition
    | PentagonalCairo of PentagonalCairoDisposition

type DispositionArrayOfA =
    | Polar of PolarDisposition