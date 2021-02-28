// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.WebUI.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Blazorise.Sidebar
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Structure
open Mazes.Core.Maze.Generate
open Mazes.Render
open Microsoft.JSInterop

/// Our application has three URL endpoints.
type Page =
    | [<EndPoint "/">] HomePage
    | [<EndPoint "/random">] RandomMaze

type Model =
    {
        page: Page
        displayedMaze : string
    }

let initModel =
    {
        page = HomePage
        displayedMaze = ""
    }

type Message =
    | SetPage of Page
    | GenerateRandomMaze
    | Ping

let newRandomMaze () =
    let rng = Random()

    let canvas =
        let shapes =
            [|
                Rectangle.create (rng.Next(15, 30)) (rng.Next(15, 45))
                TriangleIsosceles.create 33 TriangleIsosceles.Bottom 1 1
                Ellipse.create 12 15 0.1 0.0 0 0 None Ellipse.Side.Inside
                Hexagon.create 11.0
                Pentagon.create 22.0
                PentagonStar.create 22.0 15.0
            |]
        shapes.[rng.Next(shapes.Length)]

    let rngSeed = rng.Next()

    let maze grid =
        let algo =
            [|
                AldousBroder.createMaze rngSeed
                Wilson.createMaze rngSeed
                HuntAndKill.createMaze rngSeed
                RecursiveBacktracker.createMaze rngSeed
                Kruskal.createMaze rngSeed
                PrimSimple.createMaze rngSeed
                PrimSimpleModified.createMaze rngSeed
                PrimWeighted.createMaze rngSeed (rng.Next(50))
                GrowingTreeMixRandomAndLast.createMaze rngSeed (rng.NextDouble())
                GrowingTreeMixChosenRandomAndLast.createMaze rngSeed (rng.NextDouble())
                GrowingTreeMixOldestAndLast.createMaze rngSeed (rng.NextDouble())
                GrowingTreeDirection.createMaze rngSeed (rng.NextDouble()) (rng.NextDouble()) (rng.NextDouble())
                GrowingTreeSpiral.createMaze rngSeed (rng.NextDouble()) (rng.NextDouble()) (rng.Next(3, 8)) (rng.NextDouble())
            |]

        algo.[rng.Next(algo.Length)] grid

    let generateMaze (grid : NDimensionalStructure<_,_>) render =
        if rng.NextDouble() < 0.5 then
            grid.Weave rng (rng.NextDouble())
        
        let maze = maze grid
        
        //let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
        //render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastCellPartOfMaze) map
        maze.OpenMaze (maze.NDStruct.GetFirstCellPartOfMaze, maze.NDStruct.GetLastCellPartOfMaze)
        render maze.NDStruct None None (Some maze.NDStruct.GetFirstCellPartOfMaze) (Some maze.NDStruct.GetLastCellPartOfMaze)

    let gridType canvas =
        match rng.Next(7) with
        | 0 -> generateMaze (canvas |> Grid2D.Type.Ortho.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.OrthoGrid.render
        | 1 -> generateMaze (canvas |> Grid2D.Type.Hex.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.HexGrid.render
        | 2 -> generateMaze (canvas |> Grid2D.Type.Tri.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.TriGrid.render
        | 3 -> generateMaze (canvas |> Grid2D.Type.OctaSquare.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.OctaSquareGrid.render
        | 4 -> generateMaze (canvas |> Grid2D.Type.PentaCairo.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.PentaCairoGrid.render
        | 5 -> generateMaze (canvas |> Grid2D.Type.Brick.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.BrickGrid.render
        | 6 -> generateMaze (Disk.create (rng.Next(15, 25)) 1.0 (rng.Next(1, 7)) |> Grid2D.Type.Polar.Grid.createBaseGrid |> NDimensionalStructure.create2D) SVG.PolarGrid.render
        | _ -> failwith "rng problem"

    gridType canvas

let update (jsRuntime : IJSRuntime) message model =
    match message with
    | SetPage page ->
        { model with page = page }
    | GenerateRandomMaze ->
        try
            let newRandomMaze = newRandomMaze()
            jsRuntime.InvokeAsync("displaySVG", "svgContainer", newRandomMaze) |> ignore
            { model with displayedMaze = newRandomMaze }
        with
        | ex ->
            printfn "%A" ex
            model

    | Ping -> model

let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let myButton model dispatch =
    comp<Blazorise.Button>
        [ "Color" => Blazorise.Color.Primary
          "class" => "random-maze-button"
          attr.callback "onclick" (fun _ -> (dispatch GenerateRandomMaze)) ]
        [ text "Generate" ]

let myButtonWithTooltip model dispatch =
    comp<Blazorise.Tooltip>
        [
            "Text" => "Generate a random maze\nwith a random shape and tiling"
            "Placement" => Blazorise.Placement.Right
        ]
        [ myButton model dispatch ]

let homePage model dispatch =
    Main.Home().Elt()

let randomMazePage (model : Model) dispatch =
    Main.SVG()
        .ButtonRandomMaze(myButtonWithTooltip model dispatch)
        .Elt()

let mySidebar model dispatch =
    comp<Sidebar>
        []
        [
          comp<SidebarContent> [] [
              comp<SidebarBrand> [] [
                  Main.Brand().Elt()
              ]
              comp<SidebarNavigation> [] [
                  comp<SidebarItem> [] [
                      comp<SidebarLink> [
                          "To" => router.Link HomePage
                          "Title" => "Home"
                      ] [
                          comp<Blazorise.Icon> [
                              "Name" => Blazorise.IconName.Home
                              "Margin" => Blazorise.Margin.Is3.FromRight
                          ] []
                          text "Home"
                      ]
                  ]
                  comp<SidebarItem> [] [
                      comp<SidebarLink> [
                          "To" => router.Link RandomMaze
                          "Title" => "Random"
                      ] [
                          comp<Blazorise.Icon> [
                              "Name" => Blazorise.IconName.Random
                              "Margin" => Blazorise.Margin.Is3.FromRight
                          ] []
                          text "Random maze"
                      ]
                  ]
              ]
          ]
        ]

let view model dispatch =
    Main()
        .Menu(
            mySidebar model dispatch
        )
        .Body(
            cond model.page <| function
            | HomePage -> homePage model dispatch
            | RandomMaze -> randomMazePage model dispatch
        )
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) (update this.JSRuntime) view
        |> Program.withRouter router