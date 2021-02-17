// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.WebUI.Client.Main

open System
open System.Runtime.Serialization.Json
open Elmish
open Bolero
open Bolero.Html
open Blazorise.Sidebar
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid
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
                Rectangle.create 25 45
                TriangleIsosceles.create 25 TriangleIsosceles.Bottom 1 1
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

    let generateMaze (grid : IGrid<'G>) render =
        if rng.NextDouble() < 0.5 then
            grid.Weave rng (rng.NextDouble())
        
        let maze = maze grid
        
        //let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
        //render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastCellPartOfMaze) map
        maze.OpenMaze (maze.Grid.GetFirstCellPartOfMaze, maze.Grid.GetLastCellPartOfMaze)
        render (maze.Grid.ToSpecializedGrid) None None (Some maze.Grid.GetFirstCellPartOfMaze) (Some maze.Grid.GetLastCellPartOfMaze)

    let gridType canvas =
        match rng.Next(7) with
        | 0 -> generateMaze (canvas |> Grid.Type.Ortho.Grid.createBaseGrid |> Grid.create) SVG.OrthoGrid.render
        | 1 -> generateMaze (canvas |> Grid.Type.Hex.Grid.createBaseGrid |> Grid.create) SVG.HexGrid.render
        | 2 -> generateMaze (canvas |> Grid.Type.Tri.Grid.createBaseGrid |> Grid.create) SVG.TriGrid.render
        | 3 -> generateMaze (canvas |> Grid.Type.OctaSquare.Grid.createBaseGrid |> Grid.create) SVG.OctaSquareGrid.render
        | 4 -> generateMaze (canvas |> Grid.Type.PentaCairo.Grid.createBaseGrid |> Grid.create) SVG.PentaCairoGrid.render
        | 5 -> generateMaze (canvas |> Grid.Type.Brick.Grid.createBaseGrid |> Grid.create) SVG.BrickGrid.render
        | 6 -> generateMaze (Disk.create 16 1.0 5 |> Grid.Type.Polar.Grid.createBaseGrid |> Grid.create) SVG.PolarGrid.render
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
                  comp<SidebarLabel> [] []
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