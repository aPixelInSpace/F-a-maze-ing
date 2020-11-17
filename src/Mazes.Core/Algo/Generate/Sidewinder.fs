module Mazes.Core.Algo.Generate.Sidewinder

open System
open Mazes.Core
open Mazes.Core.Extensions
open Mazes.Core.GridWall

let private getEligibleRandomTopWallFromRange (rng : Random) grid rowIndex startColumnIndex endColumnIndex =
    let eligibleCellsWithRemovableTopWall = ResizeArray<int>()
    for i in startColumnIndex .. endColumnIndex do
        if not (GridCell.isALimitAt Top rowIndex i grid) then
            eligibleCellsWithRemovableTopWall.Add(i)
    
    if eligibleCellsWithRemovableTopWall.Count > 0 then
        Some eligibleCellsWithRemovableTopWall.[rng.Next(0, eligibleCellsWithRemovableTopWall.Count - 1)]
    else
        None

let private carveRow (rng : Random) grid rowIndex row =
    let mutable runStartIndex = 0
    let mutable lastCellColumnIndexWithLeftWall = 0    

    row
    |> Array.iteri(
        fun columnIndex _ ->
            // if the cell is not part of the maze, we only update the run start index
            if not (GridCell.isPartOfMaze rowIndex columnIndex grid) then
                runStartIndex <- runStartIndex + 1
            else

            let isTopALimit = (GridCell.isALimitAt Top rowIndex columnIndex grid)
            let isRightALimit = (GridCell.isALimitAt Right rowIndex columnIndex grid)

            // if we are in a top right corner
            if isTopALimit && isRightALimit then
                // we check which of the previous cells have a wall at the top that can be removed
                let getEligibleRandomTopWallFromRange = getEligibleRandomTopWallFromRange rng grid rowIndex

                let eligibleColumnIndexForTopWallRemoval = getEligibleRandomTopWallFromRange runStartIndex (columnIndex - 1)

                match eligibleColumnIndexForTopWallRemoval with
                | Some columnIndexForTopWallRemoval -> updateWallAtPosition Top Empty rowIndex columnIndexForTopWallRemoval grid
                | None ->
                    // we absolutely have to ensure that the last wall on the left is empty if possible
                    let isLastLeftWallALimit = (GridCell.isALimitAt Left rowIndex lastCellColumnIndexWithLeftWall grid)
                    if not isLastLeftWallALimit then
                        updateWallAtPosition Left Empty rowIndex lastCellColumnIndexWithLeftWall grid
                    else
                        // one last-ditch of effort, we try to remove one of the top wall from the last column just after we removed a top wall (and thus has a left wall)
                        let lastDitchIndex = getEligibleRandomTopWallFromRange lastCellColumnIndexWithLeftWall (columnIndex - 1)
                        match lastDitchIndex with
                        | Some lastDitchIndex -> updateWallAtPosition Top Empty rowIndex lastDitchIndex grid
                        | None -> ()
            else

            // if the top is a limit then we always choose remove right
            if isTopALimit then                
                updateWallAtPosition Right Empty rowIndex columnIndex grid
                runStartIndex <- columnIndex + 1
            else

            // if the right is a limit then we always choose to randomly remove one of the top wall of the run
            if isRightALimit then
                updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                runStartIndex <- columnIndex + 1
            else

            // if top and right are both not a limit we flip a coin to decide which one we remove                            
            match rng.Next(2) with
            // we continue carving to the right
            | 0 -> updateWallAtPosition Right Empty rowIndex columnIndex grid
            // or we open to the top by choosing randomly one of the top wall
            | 1 -> updateWallAtPosition Top Empty rowIndex (rng.Next(runStartIndex, columnIndex + 1)) grid
                   lastCellColumnIndexWithLeftWall <- columnIndex + 1
                   runStartIndex <- columnIndex + 1
            | _ -> raise(Exception("Random number generation problem"))
    )

let transformIntoMaze rng grid =    
    
    grid.Cells
    |> Array2D.extractByRows
    |> Seq.iteri(fun rowIndex row -> carveRow rng grid rowIndex row)
    
    grid