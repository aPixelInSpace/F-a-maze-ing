// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core

[<Struct>]
type Connection<'Position> = {
    ConnectionType : ConnectionType
    ConnectionPosition : 'Position
}