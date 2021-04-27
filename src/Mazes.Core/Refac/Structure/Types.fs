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

type GridStructureArray2D =
    | GridArray2DOrthogonal of CellArray2D[,]

type GridStructureArrayOfA =
    | GridArrayOfAPolar of CellArrayOfA[][]

type GridStructure =
    | GridStructureArray2D of GridStructureArray2D
    | GridStructureArrayOfA of GridStructureArrayOfA

module GridStructure =

    let createArray2DOrthogonal() =
        Array2D.zeroCreate 0 0 |> GridArray2DOrthogonal |> GridStructureArray2D