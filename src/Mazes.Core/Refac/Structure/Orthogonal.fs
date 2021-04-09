// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

[<Struct>]
type OrthoCell = OrthoCell of Connection<OrthogonalDisposition> array

module OrthoCell =

    let value (OrthoCell c) = c

    let listOfPossiblePositionsCoordinates coordinate =
        [|
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }, OrthogonalDisposition.Left
            { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex }, OrthogonalDisposition.Top
            { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }, OrthogonalDisposition.Right
            { RIndex = coordinate.RIndex + 1; CIndex = coordinate.CIndex }, OrthogonalDisposition.Bottom
        |]        
    
    let cellWithStateAtPosition cell connectionState position =
         (value cell)
         |> Array.map(fun c -> if c.Position = position then { State = connectionState; Position = position } else c)
         |> OrthoCell
    
    let neighborCoordinateAt coordinate position =        
        (listOfPossiblePositionsCoordinates coordinate)
        |> Array.tryFind(fun pc -> (snd pc) = position)
        |> Option.map(fst)

    let neighborPositionAt coordinate otherCoordinate =
        snd
            ((listOfPossiblePositionsCoordinates coordinate)
            |> Array.find(fun pc -> (fst pc) = otherCoordinate))
        |> Orthogonal    

    let connectionStateAtPosition c position =
         (value c
         |> Array.find(fun c ->  c.Position = position)).State