// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Utility.Canvas

open SixLabors.ImageSharp
open Mazes.Core.Canvas.Array2D

module Convert =
    let fromImage tolerance (imagePath : string) =
        use (image : Image<SixLabors.ImageSharp.PixelFormats.Byte4>) = Image<SixLabors.ImageSharp.PixelFormats.Byte4>.Load(imagePath)

        let black = 0.0f
        let grey = black + tolerance
        let isZoneImagePartOfMaze rowIndex columnIndex =
            let pixelRow = image.GetPixelRowSpan(rowIndex)
            let pixel = pixelRow.Item(columnIndex).ToVector4()
            if pixel.X <= grey && pixel.Y <= grey && pixel.Z <= grey then
                true
            else
                false     

        Canvas.create image.Height image.Width isZoneImagePartOfMaze