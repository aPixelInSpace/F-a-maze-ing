![build](https://github.com/apixelinspace/F-a-maze-ing/workflows/build/badge.svg)
[![Build Status](https://apat21.visualstudio.com/aPixelInSpace/_apis/build/status/aPixelInSpace.F-a-maze-ing?branchName=main)](https://apat21.visualstudio.com/aPixelInSpace/_build/latest?definitionId=1&branchName=main)
[![codecov](https://codecov.io/gh/aPixelInSpace/F-a-maze-ing/branch/main/graph/badge.svg?token=K6FQOQZ8BS)](https://codecov.io/gh/aPixelInSpace/F-a-maze-ing)

# F-a-maze-ing
An easy-to-use, composable and configurable maze generator and solver. Several types of grid tiles with multiple possible shapes.

## Usage
You may visit https://mazes.apixelinspace.com to randomly generate a few examples (website in preview version, work in progress)

Documentation for the CLI is coming soon.

## Examples

### Orthogonal maze

With squares

<img src="docs/resources/Examples/Ortho/SVG/RectangularSquareMaze.svg" width="400">

With circles

<img src="docs/resources/Examples/Ortho/SVG/RectangularCircleMaze.svg" width="400">

With a 'hand-drawn feel'

<img src="docs/resources/Examples/Ortho/SVG/RectangularHandDrawnMaze.svg" width="400">

With a shape from an image

<img src="docs/resources/Examples/Ortho/SVG/TreeMaze.svg" width="400">

### Penta maze

With the "Cairo" pentagonal tiling

<img src="docs/resources/Examples/Penta/SVG/PentaMaze.svg" width="400">

### Theta maze

<img src="docs/resources/Examples/Theta/SVG/CircleMaze.svg" width="400">

### Sigma maze

<img src="docs/resources/Examples/Sigma/SVG/HexagonMaze.svg" width="400">

### Delta maze

<img src="docs/resources/Examples/Delta/SVG/TriangularMaze.svg" width="400">

### Upsilon maze

<img src="docs/resources/Examples/Upsilon/SVG/UpsilonMaze.svg" width="400">

### Brick maze

<img src="docs/resources/Examples/Brick/SVG/BrickMaze.svg" width="400">

### Customizable shapes

<img src="docs/resources/Examples/Ortho/SVG/TriangularMaze.svg" width="400">

<img src="docs/resources/Examples/Ortho/SVG/EllipseMaze.svg" width="400">

<img src="docs/resources/Examples/Ortho/SVG/DoughnutMaze.svg" width="400">

<img src="docs/resources/Examples/Ortho/SVG/CompositeMaze.svg" width="400">

### ASCII
The text render is simpler and only displays orthogonal mazes (using the Unicode characters [link](https://en.wikipedia.org/wiki/Box-drawing_character)) :
<div style="font-family: 'DejaVu Sans Mono', monospace">
    <pre class="maze-line">                ┏━┓            
          ┏━┯━━━┛ ┗━━━━━┓      
      ┏━━━┩ ┴ ╭─┬───────╄━━━┓  
      ┃ ┬ ┴ ╶─╯ │ ╶───╮ ╰─╴ ┃  
    ┏━┛ ╰─┬───┲━┷━━━┓ ├───╮ ┗━┓
    ┗━┓ ╶─╯ ┬ ┗━━━━━┩ │ ┬ ├─┲━┛
      ┃ ╶─╮ ╰─┬─╴ ╭─╯ │ │ ┴ ┃  
      ┗━━━╅─╴ │ ╶─╯ ┬ ┴ ┢━━━┛  
          ┗━━━┷━┓ ┏━┷━━━┛      
                ┗━┛            </pre>
</div>

## Book

This project is inspired by working through the book [Mazes for Programmers](https://pragprog.com/book/jbmaze/mazes-for-programmers) by Jamis Buck.

<img src="docs/resources/Book/mazes-for-programmers.jpg" width="300">

The code in the book is written in Ruby and leans on Object-Oriented design.

My goal is two fold : give the project a functional spin and learn F# in the process. To do so, I freely change or adapt much of the original code and implementation.