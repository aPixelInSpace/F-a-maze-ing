module Mazes.WebUI.Client.Main

open System
open System.Runtime.Serialization.Json
open Elmish
open Bolero
open Bolero.Html
open Mazes.Core
open Mazes.Core.Canvas.Array2D
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Maze.Generate
open Mazes.Render
open Microsoft.JSInterop

/// Our application has three URL endpoints.
type Page =
    | [<EndPoint "/random">] RandomMaze

type Model =
    {
        page: Page
        displayedMaze : string
    }

let initModel =
    {
        page = RandomMaze
        displayedMaze = ""
    }

type Message =
    | SetPage of Page
    | GenerateRandomMaze
    | Ping

type GridChoice =
    | Ortho
    | Hex
    | Tri
    | OctaSquare
    | PentaCairo
    | Brick
    | Polar

let newRandomMaze () =
    let rng = Random()

    let canvas =
        match rng.Next(6) with
        | 0 -> Rectangle.create 15 24
        | 1 -> TriangleIsosceles.create 25 TriangleIsosceles.Bottom 1 1
        | 2 -> Ellipse.create 11 12 0.0 0.0 0 0 None Ellipse.Side.Inside
        | 3 -> Hexagon.create 11.0
        | 4 -> Pentagon.create 22.0
        | 5 -> PentagonStar.create 22.0 15.0        
        | _ -> failwith "rng problem"
        
    let gridChoice =
        match rng.Next(7) with
        | 0 -> GridChoice.Ortho
        | 1 -> GridChoice.Hex
        | 2 -> GridChoice.Tri
        | 3 -> GridChoice.OctaSquare
        | 4 -> GridChoice.PentaCairo
        | 5 -> GridChoice.Brick
        | 6 -> GridChoice.Polar
        | _ -> failwith "rng problem"

    let rndSeed = rng.Next()

    let generate grid render =
        let maze = HuntAndKill.createMaze rndSeed grid
        let map = maze.createMap maze.Grid.GetFirstCellPartOfMaze
        render (maze.Grid.ToSpecializedGrid) (map.ShortestPathGraph.PathFromRootTo maze.Grid.GetLastCellPartOfMaze) map

    let gridType canvas gridChoice =
        match gridChoice with
        | GridChoice.Ortho -> generate (canvas |> Grid.Type.Ortho.Grid.createBaseGrid |> Grid.Grid.create) SVG.OrthoGrid.render
        | GridChoice.Hex -> generate (canvas |> Grid.Type.Hex.Grid.createBaseGrid |> Grid.Grid.create) SVG.HexGrid.render
        | GridChoice.Tri -> generate (canvas |> Grid.Type.Tri.Grid.createBaseGrid |> Grid.Grid.create) SVG.TriGrid.render
        | GridChoice.OctaSquare -> generate (canvas |> Grid.Type.OctaSquare.Grid.createBaseGrid |> Grid.Grid.create) SVG.OctaSquareGrid.render
        | GridChoice.PentaCairo -> generate (canvas |> Grid.Type.PentaCairo.Grid.createBaseGrid |> Grid.Grid.create) SVG.PentaCairoGrid.render
        | GridChoice.Brick -> generate (canvas |> Grid.Type.Brick.Grid.createBaseGrid |> Grid.Grid.create) SVG.BrickGrid.render
        | GridChoice.Polar -> generate (Disk.create 16 1.0 5 |> Grid.Type.Polar.Grid.createBaseGrid |> Grid.Grid.create) SVG.PolarGrid.render

    gridType canvas gridChoice

let update (jsRuntime : IJSRuntime) message model =
    match message with
    | SetPage page ->
        { model with page = page }
    | GenerateRandomMaze ->
        let newRandomMaze = newRandomMaze()
        jsRuntime.InvokeAsync("displaySVG", "svgContainer", newRandomMaze) |> ignore
        { model with displayedMaze = newRandomMaze }
    | Ping -> model

let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let randomMaze (model : Model) dispatch =
    Main.SVG()
        .GenerateRandomMaze(fun _ -> dispatch GenerateRandomMaze)
        .Elt()

let view model dispatch =
    Main()
        .Body(
            randomMaze model dispatch
        )
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) (update this.JSRuntime) view
        |> Program.withRouter router