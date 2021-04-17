// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Tests

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D
open Mazes.Utility.Canvas.Convert

[<Fact>]
let ``Given an image with black pixels, when creating a canvas from it, then it should give the canvas that match the pixels of the image`` () =
    // arrange
    let imagePath = "Resources/MicrochipBlack.png"

    // act
    let canvas = fromImage 0.0f imagePath

    // assert
    canvas.NumberOfRows |> should equal 16
    canvas.NumberOfColumns |> should equal 16
    canvas.TotalOfMazeZones |> should equal 196

    let expectedCanvasLayout =
        "Type=Canvas\n" +
        ".*.*.*.**.*.*.*.\n" +
        "****************\n" +
        ".******..******.\n" +
        "*******..*******\n" +
        ".**************.\n" +
        "**.**.****.**.**\n" +
        ".*.**.****.**.*.\n" +
        "**.**.****.**.**\n" +
        "**.**.****.**.**\n" +
        ".*.**.****.**.*.\n" +
        "**.**.****.**.**\n" +
        ".**************.\n" +
        "*******..*******\n" +
        ".******..******.\n" +
        "****************\n" +
        ".*.*.*.**.*.*.*.\n" +
        "end"
    Convert.toString canvas |> should equal expectedCanvasLayout

[<Fact>]
let ``Given an image with grey pixels, when creating a canvas with tolerance from it, then it should give the canvas that match the pixels of the image`` () =
    // arrange
    let imagePath = "Resources/FaceGrey.png"

    // act
    let canvas = fromImage 63.0f imagePath

    // assert
    canvas.NumberOfRows |> should equal 16
    canvas.NumberOfColumns |> should equal 16
    canvas.TotalOfMazeZones |> should equal 106

    let expectedCanvasLayout =
        "Type=Canvas\n" +
        "...**********...\n" +
        "..************..\n" +
        ".**.*.****.*.**.\n" +
        ".**....**....**.\n" +
        ".**..........**.\n" +
        ".*****....*****.\n" +
        ".****......****.\n" +
        ".**..........**.\n" +
        "..*..........*..\n" +
        "..*..........*..\n" +
        "..*....**....*..\n" +
        "..*...****...*..\n" +
        "..*...****...*..\n" +
        "..*....**....*..\n" +
        "..************..\n" +
        ".....******.....\n" +
        "end"
    Convert.toString canvas |> should equal expectedCanvasLayout