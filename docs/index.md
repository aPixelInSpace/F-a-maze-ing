# Table of contents

- [Table of contents](#table-of-contents)
- [Usage](#usage)
  - [Command Line Interface](#command-line-interface)
    - [Canvas](#canvas)
      - [s-rectangle](#s-rectangle)
      - [s-triangle](#s-triangle)
      - [s-ellipse](#s-ellipse)
      - [s-hexagon](#s-hexagon)
      - [s-pentagon](#s-pentagon)
      - [s-pentagonStar](#s-pentagonstar)
      - [s-image](#s-image)
  - [Website](#website)

# Usage

## Command Line Interface

To use the CLI you first need to download [the app](https://github.com/aPixelInSpace/F-a-maze-ing) and to have on your computer the [.Net Core 5 runtime](https://dotnet.microsoft.com/download).

The CLI is designed to be composable with a maximum of options on each step to create unique and beautiful mazes.

The basic idea is to *pipe* different actions to generate and visualize, in the end, a maze.

The actions are :

1. Choosing a canvas
2. Choosing a type of grid
3. Choosing a maze generator algorithm
4. Choosing a render
5. Choosing an output

For example, the following line will generate a text maze in a file :

```
.\Mazes.exe s-rectangle -r 6 -c 10 : g-ortho : a-hk -s 1 : rt-ortho : o-file -p "./test.txt"
```

Each part is separated by a '**:**' wich is used as the pipe symbol.

Let's break it down :

- **.\Mazes.exe** is the path to the CLI itself
- **s-rectangle -r 6 -c 10** use a rectangle shape with 6 rows and 10 columns
- **g-ortho** use an orthogonal grid type
- **a-hk -s 1** use the "Hunt and Kill" algorithm with a random seed of 1 to generare the maze
- **rt-ortho** use the text unicode render
- **o-file -p "./test.txt"** save on the disk into the specified file

The result might look like this :

```
 ━┯━━━━━┯━━━━━┯━━━━━┓
┳ ┴ ╭─╴ │ ┬ ╶─╯ ┬ ┬ ┃
┠───╯ ╶─┴─┴─┬─╴ │ │ ┃
┃ ╶─╮ ╭───╴ ┴ ╭─┴─╯ ┃
┠───┤ │ ╭───┬─┴─┬─╴ ┃
┃ ┬ ┴ │ ┴ ┬ ╰─╴ │ ╶─┚
┗━┷━━━┷━━━┷━━━━━┷━━━ 
```

There are a number of actions available for each step. Note that *not* every combination will yield a valid maze.

It is possible to get every option for a given action by using ``--help`` so for example ``s-rectangle --help``

### Canvas

There are two types of canvas : Array2d and ArrayOfA.

Array2d canvas cannot be used on polar maze, only ArrayOfA canvas can be used for a polar maze.

#### s-rectangle

- Array2d type

```
  -r, --rows       Required. The number of rows.

  -c, --columns    Required. The number of columns.
```

#### s-triangle

- Array2d type

```
  -b, --base           Required. The length of the base.

  --baseat             (Default: 0) The position of the base Bottom, Top, Left or Right.

  --basedecrement      (Default: 1) The decrement value for the base.
```

#### s-ellipse

- Array2d type

```
  -r, --rowRadiusLength        Required. The length for the horizontal radius.

  -c, --columnRadiusLength     Required. The length for the vertical radius.

  --rowenlargingfactor         (Default: 0) Zoom factor on the horizontal axis.

  --columnenlargingfactor      (Default: 0) Zoom factor on the vertical axis.

  --rowtranslationfactor       (Default: 0) Translation factor on the horizontal axis.

  --columntranslationfactor    (Default: 0) Translation factor on the vertical axis.

  --ellipsefactor              (Default: 0) Inside ellipse factor.

  --side                       (Default: 0) Indicate where the ellipse is Inside or Outside.
```

#### s-hexagon

- Array2d type

```
  -s, --edgeSize    Required. The length of one side of the hexagon.
```

#### s-pentagon

- Array2d type

```
  -s, --edgeSize    Required. The length of one side of the pentagon.
```

#### s-pentagonStar

- Array2d type

```
  -g, --greatEdgeSize    Required. The length of the great side of the pentagon star.

  -s, --smallEdgeSize    Required. The length of the small side of the pentagon star.
```

#### s-image

- Array2d type

Get a canvas/shape from an image. It works on the levels of black (with a parametrable tolerance) on each pixel.


```
  -p, --path         Required. The full path of the image file

  -t, --tolerance    (Default: 0) The tolerance on the pixel color.
```

## Website

A website is available [here](http://mazes.apixelinspace.com/) which is in preview version and where it is possible to generate random mazes.
