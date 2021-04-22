// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

type OrthoCell = OrthoCell of Connection<OrthogonalDisposition> array

type CellArray2D =
    | OrthoCellChoice of OrthoCell

//

type PolarCell = PolarCell of Connection<PolarDisposition> array

type CellArrayOfA =
    | PolarCellChoice of PolarCell

//

type GridArray2DType =
    | Orthogonal
    | Hexagonal
    | Triangular
    | OctagonalSquare
    | PentagonalCairo

type GridArrayOfAType =
    | Polar

type GridType =
    | GridArray2DTypeChoice of GridArray2DType
    | GridArrayOfATypeChoice of GridArrayOfAType