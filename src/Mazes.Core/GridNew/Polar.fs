// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.GridNew.Polar

open System.Text
open Mazes.Core
open Mazes.Core.ArrayOfA
open Mazes.Core.Canvas.ArrayOfA
open Mazes.Core.Grid.ArrayOfA.Polar.PolarArrayOfA

type PolarPositionHandler private () =

    static let instance = PolarPositionHandler()

    interface IPositionHandler<PolarPosition> with

        member this.Opposite _ position =
            match position with
            | Inward -> Outward
            | Outward -> Inward
            | Ccw -> Cw
            | Cw -> Ccw

        member this.Values _ =
            [| Ccw; Cw; Inward; Outward |]

        member this.Map _ position =
            match position with
            | Position.Top -> Inward
            | Position.Right -> Cw
            | Position.Bottom -> Outward
            | Position.Left -> Ccw

    member this.ToInterface =
        this :> IPositionHandler<PolarPosition>

    static member Instance =
        instance.ToInterface

type PolarCoordinateHandler private () =

    static let instance = PolarCoordinateHandler()

    member this.NeighborsCoordinateAt (arrayOfA : 'A[][]) coordinate position =
        seq {
            match position with
            | Inward ->
                if not (isFirstRing coordinate.RIndex) then
                    let inwardRingNumberOfCells = getNumberOfCellsAt arrayOfA (coordinate.RIndex - 1)
                    let currentRingNumberOfCells = getNumberOfCellsAt arrayOfA coordinate.RIndex
                    let ratio = currentRingNumberOfCells / inwardRingNumberOfCells
                    yield { RIndex = coordinate.RIndex - 1; CIndex = coordinate.CIndex / ratio }
            | Outward ->
                if not (isLastRing coordinate.RIndex (numberOfRings arrayOfA)) then
                    let currentRingNumberOfCells = getNumberOfCellsAt arrayOfA coordinate.RIndex
                    let outwardRingNumberOfCells = getNumberOfCellsAt arrayOfA (coordinate.RIndex + 1)
                    let ratio = outwardRingNumberOfCells / currentRingNumberOfCells
                    for ratioIndex in 0 .. ratio - 1 do
                        yield { RIndex = coordinate.RIndex + 1; CIndex = (coordinate.CIndex * ratio) + ratioIndex }
            | Ccw ->
                if isFirstCellOfRing coordinate.CIndex then
                    yield { RIndex = coordinate.RIndex; CIndex = maxCellsIndex arrayOfA coordinate.RIndex }
                else
                    yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex - 1 }
            | Cw ->
                if isLastCellOfRing arrayOfA coordinate.RIndex coordinate.CIndex then
                    yield { RIndex = coordinate.RIndex; CIndex = minCellIndex }
                else
                    yield { RIndex = coordinate.RIndex; CIndex = coordinate.CIndex + 1 }
        }

    member this.NeighborPositionAt (arrayOfA : 'A[][]) coordinate otherCoordinate =
        let neighborCoordinateAt = this.NeighborsCoordinateAt arrayOfA coordinate

        match otherCoordinate with
        | c when c = (neighborCoordinateAt Ccw |> Seq.head) -> Ccw
        | c when c = (neighborCoordinateAt Cw |> Seq.head) -> Cw
        | c when (neighborCoordinateAt Outward |> Seq.tryFind(fun n -> c = n)).IsSome -> Outward
        | c when (neighborCoordinateAt Inward |> Seq.tryFind(fun n -> c = n)).IsSome -> Inward
        | _ -> failwith "Unable to match the polar coordinates with a position"

    static member Instance =
        instance

[<Struct>]
type PolarCell =
    private
        { Connections : Connection<PolarPosition> array }

    interface ICell<PolarPosition> with
        member this.Create connections =
            ({ Connections = connections } :> ICell<PolarPosition>)

        member this.Connections =
            this.Connections

        member this.ConnectionTypeAtPosition position =
            (this.Connections |> Array.find(fun w -> w.ConnectionPosition = position)).ConnectionType

    member this.ToInterface =
        this :> ICell<PolarPosition>

    static member Create (canvas : Canvas) internalWallType (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate
        let neighborsCoordinateAt = PolarCoordinateHandler.Instance.NeighborsCoordinateAt canvas.Zones coordinate

        let walls = ResizeArray<Connection<PolarPosition>>()

        if not (isFirstRing coordinate.RIndex) then
            let isInwardNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Inward) |> Seq.head)
            walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isInwardNeighborPartOfMaze); ConnectionPosition = Inward })

        let isCcwNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Ccw) |> Seq.head)
        walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isCcwNeighborPartOfMaze); ConnectionPosition = Ccw })

        let isCwNeighborPartOfMaze = isCellPartOfMaze ((neighborsCoordinateAt Cw) |> Seq.head)
        walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForInternal internalWallType isCurrentCellPartOfMaze isCwNeighborPartOfMaze); ConnectionPosition = Cw })
 
        if isLastRing coordinate.RIndex canvas.NumberOfRings then
            walls.Add({ ConnectionType = (ConnectionType.getConnectionTypeForEdge isCurrentCellPartOfMaze); ConnectionPosition = Outward })

        {
            Connections = walls.ToArray()
        }.ToInterface

let toString (maze : IGrid<Grid<GridArrayOfA, PolarPosition>>) =
    let sBuilder = StringBuilder()

    let cells = maze.ToSpecializedGrid.BaseGrid.ToSpecializedStructure.Cells
    let connectionTypeAtPosition = maze.ToSpecializedGrid.BaseGrid.ToSpecializedStructure.ConnectionTypeAtPosition

    let appendHorizontalWall wallType (sBuilder : StringBuilder) =
        match wallType with
            | Close | ClosePersistent -> sBuilder.Append("‾")
            | ConnectionType.Open -> sBuilder.Append("¨")

    let appendVerticalWall wallType (sBuilder : StringBuilder) =
        match wallType with
            | Close | ClosePersistent -> sBuilder.Append("|")
            | ConnectionType.Open -> sBuilder.Append("¦")

    let appendWhiteSpace (sBuilder : StringBuilder) =
        sBuilder.Append(" ")

    let appendRing appendCell appendLastCell cellsRow =
        cellsRow
        |> Array.iter(appendCell)

        cellsRow
        |> Array.last
        |> appendLastCell

        sBuilder.Append("\n") |> ignore

    let lastCell lastCell =
        sBuilder
        |> appendVerticalWall (connectionTypeAtPosition lastCell Cw) |> ignore

    // first
    let firstRing cell =
        sBuilder
        |> appendVerticalWall (connectionTypeAtPosition cell Ccw)
        |> appendWhiteSpace
        |> ignore

    getRingByRing cells
    |> Seq.head
    |> appendRing firstRing lastCell

    // others
    let everyOtherRing cell =
        sBuilder
        |> appendVerticalWall (connectionTypeAtPosition cell Ccw)
        |> appendHorizontalWall (connectionTypeAtPosition cell Inward)
        |> ignore

    getRingByRing cells
    |> Seq.iteri(fun ringIndex cells ->
        if ringIndex > 0 then
            cells
            |> appendRing everyOtherRing lastCell
        else
            ())

    // last
    let lastRing cell =
        sBuilder
        |> appendWhiteSpace
        |> appendHorizontalWall (connectionTypeAtPosition cell Outward)
        |> ignore

    getRingByRing cells
    |> Seq.last
    |> appendRing lastRing (fun _ -> ())

    sBuilder.ToString()

let private createInternal internalConnectionType (canvas : Canvas.ArrayOfA.Canvas) =
    let cells =
            createPolar
                canvas.NumberOfRings
                canvas.WidthHeightRatio
                canvas.NumberOfCellsForCenterRing
                (fun rIndex cIndex -> PolarCell.Create canvas internalConnectionType { RIndex = rIndex; CIndex = cIndex } canvas.IsZonePartOfMaze)

    {
        Canvas = canvas
        Cells = cells
        PositionHandler = PolarPositionHandler.Instance
    }

let createBaseGrid canvas =
    createInternal Close canvas :> IAdjacentStructure<GridArrayOfA, PolarPosition>

let createEmpty canvas =
    createInternal Open canvas :> IAdjacentStructure<GridArrayOfA, PolarPosition>