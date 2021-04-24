﻿// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

module CellArrayOfAM =

    let listOfPossibleCoordinate gridArrayOfAType coordinate =
        match gridArrayOfAType with
        | GridArrayOfAType.Polar ->
            PolarCellM.listOfPossiblePositionsCoordinates coordinate