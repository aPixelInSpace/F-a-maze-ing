// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.GridNew

open Mazes.Core

type ICell<'Position> =
    abstract member Create : Connection<'Position> array -> ICell<'Position>
    abstract member Connections : Connection<'Position> array
    abstract member ConnectionTypeAtPosition : 'Position -> ConnectionType