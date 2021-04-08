// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

[<Struct>]
type Connection<'Position> = {
    State : ConnectionState
    Position : 'Position 
}

[<Struct>]
type OrthoCell = OrthoCell of Connection<OrthogonalDisposition> array

module OrthoCell =

    let value (OrthoCell c) = c

[<Struct>]
type CellArray2D =
    | OrthoCellChoice of OrthoCell

module CellArray2D =
    
    let connectionsState c =
        match c with
        | OrthoCellChoice c -> ((OrthoCell.value c) |> Array.map(fun c -> c.State))