// Learn more about F# at http://fsharp.org

open System
open Mazes.Lib
open Mazes.Lib.SimpleTypes
open Mazes.Lib.Grid.Wall
open Mazes.Lib.Algo
open Mazes.Render.Text

[<EntryPoint>]
let main _ =
    let mazeAlgo = Sidewinder.transformIntoMaze 1984
    
    printfn "Mazes !"
    
    let grid00 = (Grid.create 0 0)
    let grid11 = (Grid.create 1 1)
    let grid12 = (Grid.create 1 2)
    let grid21 = (Grid.create 2 1)
    let grid13 = (Grid.create 1 3)
    let grid31 = (Grid.create 3 1)
    let grid22 = (Grid.create 2 2)
    let grid33 = (Grid.create 3 3)
    let grid55 = (Grid.create 5 5)
    let grid105 = (Grid.create 10 5)
    let grid510 = (Grid.create 5 10)
    
    let carvedCornerGrid = (Grid.create 3 4)
    carvedCornerGrid.Cells.[0, 0] <-
            {
                CellType = NotPartOfMaze
                WallTop = { WallType = Empty; WallPosition = WallPosition.Top }
                WallRight = { WallType = Border; WallPosition = WallPosition.Right }
                WallBottom = { WallType = Border; WallPosition = WallPosition.Bottom }
                WallLeft = { WallType = Empty; WallPosition = WallPosition.Left }
            }
    updateWallAtPosition Left Border 0 1 carvedCornerGrid
    updateWallAtPosition Top Border 1 0 carvedCornerGrid
    
    updateWallAtPosition Left Border 1 1 carvedCornerGrid
    updateWallAtPosition Top Border 1 1 carvedCornerGrid
    updateWallAtPosition Right Empty 1 1 carvedCornerGrid
    updateWallAtPosition Bottom Border 1 1 carvedCornerGrid
        
    updateWallAtPosition Top Border 1 2 carvedCornerGrid
    updateWallAtPosition Right Border 1 2 carvedCornerGrid
    updateWallAtPosition Bottom Border 1 2 carvedCornerGrid
        
    let maze00 = mazeAlgo (Grid.create 0 0)
    let maze11 = mazeAlgo (Grid.create 1 1)
    let maze12 = mazeAlgo (Grid.create 1 2)
    let maze21 = mazeAlgo (Grid.create 2 1)
    let maze13 = mazeAlgo (Grid.create 1 3)
    let maze31 = mazeAlgo (Grid.create 3 1)
    let maze22 = mazeAlgo (Grid.create 2 2)
    let maze33 = mazeAlgo (Grid.create 3 3)
    let maze55 = mazeAlgo (Grid.create 5 5)
    let maze105 = mazeAlgo (Grid.create 10 5)
    let maze510 = mazeAlgo (Grid.create 5 10)    
    let maze5080 = mazeAlgo (Grid.create 50 80)
    
    
    //let mazeMega = mazeAlgo (Grid.create 1000 1000)
    
    //let printedMazeMega = printGrid mazeMega
    
    let output =
        sprintf "%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n%s"
            (printGrid grid00)
            (printGrid grid11)
            (printGrid grid12)
            (printGrid grid21)
            (printGrid grid13)
            (printGrid grid31)
            (printGrid grid22)
            (printGrid grid33)
            (printGrid grid55)
            (printGrid grid105)
            (printGrid grid510)
            
            (printGrid carvedCornerGrid)
            
            (printGrid maze00)
            (printGrid maze11)
            (printGrid maze12)
            (printGrid maze21)
            (printGrid maze13)
            (printGrid maze31)
            (printGrid maze22)
            (printGrid maze33)
            (printGrid maze55)
            (printGrid maze105)
            (printGrid maze510)
            
            (printGrid maze5080)
            
            //printedMazeMega
    
    System.IO.File.WriteAllText("D:\\TEMP\\MazePrint.txt", output, Text.Encoding.UTF8)
    printf "%s\n" output
        
    printfn "Mazes creation finished"    
    
    0 // return an integer exit code
