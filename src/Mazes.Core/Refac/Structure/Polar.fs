// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open Mazes.Core.Refac

[<Struct>]
type PolarCell = PolarCell of Connection<OrthogonalDisposition> array