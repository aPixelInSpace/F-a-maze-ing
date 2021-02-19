// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core

type ConnectionType =
    | Open
    | Close
    | ClosePersistent

module ConnectionType =

    let getConnectionTypeForEdge isEdge =
        if isEdge then ClosePersistent
        else Open

    let getConnectionTypeForInternal internalConnectionType isCurrentCellPartOfMaze isOtherCellPartOfMaze =
        match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
        | false, false -> Open
        | true, true -> internalConnectionType
        | true, false | false, true -> ClosePersistent

    let isConnected connectionType =
        connectionType = Open