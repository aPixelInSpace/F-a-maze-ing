![build](https://github.com/apixelinspace/F-a-maze-ing/workflows/build/badge.svg)
[![codecov](https://codecov.io/gh/aPixelInSpace/F-a-maze-ing/branch/main/graph/badge.svg?token=K6FQOQZ8BS)](https://codecov.io/gh/aPixelInSpace/F-a-maze-ing)

# F-a-maze-ing
A simple, composable and configurable maze generator made with F#

This project is inspired by working through the book [Mazes for Programmers](https://pragprog.com/book/jbmaze/mazes-for-programmers) by Jamis Buck.

<img src="resources/Book/mazes-for-programmers.jpg" width="300">

The code in the book is written in Ruby and leans on Object-Oriented design.

My goal is two fold : give the project a functional spin and learn F# in the process. To do so, I freely changed or adapted much of the original code.

## Examples

### SVG
The SVG render can color the maze and show the path(s) between two zones that are the most distant :
#### Polar
* Circular

<img src="resources/Examples/Polar/SVG/CircleMaze.svg" width="400">

#### Orthogonal
* Rectangular

<img src="resources/Examples/Ortho/SVG/RectangularMaze.svg" width="400">

* Triangular

<img src="resources/Examples/Ortho/SVG/TriangularMaze.svg" width="400">

* Ellipse

<img src="resources/Examples/Ortho/SVG/EllipseMaze.svg" width="400">

* Doughnut

<img src="resources/Examples/Ortho/SVG/DoughnutMaze.svg" width="400">

* Composite

<img src="resources/Examples/Ortho/SVG/CompositeMaze.svg" width="400">

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

## Usage
###### (more to come) 
```
mazes

  -n, --name         A name for the maze. If empty, a default one is provided.

  -r, --rows         (Default: 50) The number of rows of the maze.

  -c, --columns      (Default: 80) The number of columns of the maze.

  -a, --algo         The algorithm to use to generate the maze. If empty, a random one is chosen.
                     Options are :
                     - BinaryTree
                     - Sidewinder
                     - AldousBroder
                     - Wilson
                     - HuntAndKill
                     - RecursiveBackTracker

  -s, --seed         The seed number to use for the random number generator. If empty, a random seed is picked.

  -d, --directory    The directory where to output the maze. If empty, the directory of the program is used.

  -q, --quiet        (Default: false) Automatically exit the program when finished

  --help             Display this help screen.

  --version          Display version information.
```