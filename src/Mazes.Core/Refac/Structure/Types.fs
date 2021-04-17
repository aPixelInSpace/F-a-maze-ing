// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System.Collections.Generic
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

type GridArray2D =
    private
        {
            CanvasArray2D : Canvas.CanvasArray2D
            TypeArray2D : GridArray2DType
            CellsArray2D : CellArray2D[,]
        }

type GridArrayOfA =
    private
        {
            CanvasArrayOfA : Canvas.CanvasArrayOfA
            TypeArrayOfA : GridArrayOfAType
            CellsArrayOfA : CellArrayOfA[][]
        }

type Grid =
    | GridArray2DChoice of GridArray2D
    | GridArrayOfAChoice of GridArrayOfA