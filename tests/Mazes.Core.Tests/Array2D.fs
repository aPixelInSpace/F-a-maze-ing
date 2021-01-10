// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Array2D

open System.Text
open FsUnit
open Xunit
open Mazes.Core
open Mazes.Core.Array2D

type ExtractByEnum =
    | RowsAscendingColumnsAscending = 1
    | RowsAscendingColumnsDescending = 2
    | RowsDescendingColumnsAscending = 3
    | RowsDescendingColumnsDescending = 4
    | ColumnsAscendingRowsAscending = 5
    | ColumnsAscendingRowsDescending = 6
    | ColumnsDescendingRowsAscending = 7
    | ColumnsDescendingRowsDescending = 8

let mapExtractByEnumToExtractBy extractByEnum =
    match extractByEnum with
    | ExtractByEnum.RowsAscendingColumnsAscending -> RowsAscendingColumnsAscending
    | ExtractByEnum.RowsAscendingColumnsDescending -> RowsAscendingColumnsDescending
    | ExtractByEnum.RowsDescendingColumnsAscending -> RowsDescendingColumnsAscending
    | ExtractByEnum.RowsDescendingColumnsDescending -> RowsDescendingColumnsDescending
    | ExtractByEnum.ColumnsAscendingRowsAscending -> ColumnsAscendingRowsAscending
    | ExtractByEnum.ColumnsAscendingRowsDescending -> ColumnsAscendingRowsDescending
    | ExtractByEnum.ColumnsDescendingRowsAscending -> ColumnsDescendingRowsAscending
    | ExtractByEnum.ColumnsDescendingRowsDescending -> ColumnsDescendingRowsDescending
    | _ -> failwith "ExtractByEnum unknown"

[<Theory>]
[<InlineData(ExtractByEnum.RowsAscendingColumnsAscending, "0,0; 0,1; 0,2; 1,0; 1,1; 1,2; 2,0; 2,1; 2,2")>]
[<InlineData(ExtractByEnum.RowsAscendingColumnsDescending, "0,2; 0,1; 0,0; 1,2; 1,1; 1,0; 2,2; 2,1; 2,0")>]
[<InlineData(ExtractByEnum.RowsDescendingColumnsAscending, "2,0; 2,1; 2,2; 1,0; 1,1; 1,2; 0,0; 0,1; 0,2")>]
[<InlineData(ExtractByEnum.RowsDescendingColumnsDescending, "2,2; 2,1; 2,0; 1,2; 1,1; 1,0; 0,2; 0,1; 0,0")>]
[<InlineData(ExtractByEnum.ColumnsAscendingRowsAscending, "0,0; 1,0; 2,0; 0,1; 1,1; 2,1; 0,2; 1,2; 2,2")>]
[<InlineData(ExtractByEnum.ColumnsAscendingRowsDescending, "2,0; 1,0; 0,0; 2,1; 1,1; 0,1; 2,2; 1,2; 0,2")>]
[<InlineData(ExtractByEnum.ColumnsDescendingRowsAscending, "0,2; 1,2; 2,2; 0,1; 1,1; 2,1; 0,0; 1,0; 2,0")>]
[<InlineData(ExtractByEnum.ColumnsDescendingRowsDescending, "2,2; 1,2; 0,2; 2,1; 1,1; 0,1; 2,0; 1,0; 0,0")>]
let ``Given a 2d array, when getting item by item, then it should return the items in the order specified``
    (extractByEnum, expectedResult) =

    // arrange
    let array2d = Array2D.init 3 3 (fun r c -> r.ToString() + "," + c.ToString())

    // act
    let items = array2d |> getItemByItem (mapExtractByEnumToExtractBy extractByEnum) (fun _ _ -> true)

    // assert
    items
    |> Seq.map(fun item ->
        let (i, _) = item
        i)
    |> String.concat "; "
    |> should equal expectedResult

[<Fact>]
let ``Given a 2d array, when folding, then it should return the correct state`` () =

    // arrange
    let array2d = Array2D.init 2 2 (fun r c -> r.ToString() + "," + c.ToString())

    // act
    let state =
        array2d
        |> Array2D.fold (fun r c (state : StringBuilder) item -> state.Append($"{item}; ")) (StringBuilder())

    // assert
    state.ToString() |> should equal "0,0; 0,1; 1,0; 1,1; "