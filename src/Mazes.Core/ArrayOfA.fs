// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core


module ArrayOfA =
    
    let getIndex number =
        number - 1

    let minD1Index =
        0

    let minD2Index =
        0

    let isFirstD1 d1Index =
        d1Index = minD1Index

    let isLastD1 d1Index d1Length =
        d1Index = (getIndex d1Length)
    
    let maxD1Index (arrayOfA : 'A[][]) =
        getIndex arrayOfA.Length

    let getD1LengthAt (arrayOfA : 'A[][]) =
        arrayOfA.Length

    let getD2LengthAt (arrayOfA : 'A[][]) d1Index =
        arrayOfA.[d1Index].Length

    let maxD2Index (arrayOfA : 'A[][]) d1Index =
        getIndex (getD2LengthAt arrayOfA d1Index)

    let isFirstD2 d2Index =
        d2Index = minD2Index

    let isLastD2 (arrayOfA : 'A[][]) d1Index d2Index =
        d2Index = maxD2Index arrayOfA d1Index

    let get (arrayOfA : 'A[][]) coordinate =
        arrayOfA.[coordinate.RIndex].[coordinate.CIndex]

    let existAt (arrayOfA : 'A[][]) coordinate =
        coordinate.RIndex >= minD1Index && coordinate.CIndex >= minD2Index &&
        coordinate.RIndex <= maxD1Index arrayOfA && coordinate.CIndex <= maxD2Index arrayOfA coordinate.RIndex