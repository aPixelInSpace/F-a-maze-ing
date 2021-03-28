# Usage

## Command Line Interface

It is recommended to download [the latest release](https://github.com/aPixelInSpace/F-a-maze-ing/releases/tag/v1.0) of this project CLI. The [.Net Core 5 runtime](https://dotnet.microsoft.com/download) must also be installed.

The CLI is designed to be composable with a maximum of options on each step to create unique and beautiful mazes.

The basic idea is to *pipe* different actions to generate and visualize, in the end, a maze.

The actions are :

1. [[Choosing a canvas|Canvas]]
2. [[Choosing a type of grid|Grid]]
3. [[Choosing a maze generator algorithm|Maze-Algorithm]]
4. [[Choosing a render|Render]]
5. [[Choosing an output|Output]]

For example, the following line will generate a text maze in a file :

```
.\Mazes.exe s-rectangle -r 6 -c 10 : g-ortho : a-hk -s 1 : rt-ortho -e : o-file -p "./test.txt"
```

Each part is separated by a ``:`` wich is used as the pipe symbol.

Let's break it down :

| Actions                     | Description
|:----------------------------|:---------------------------|
| .\Mazes.exe                 | the path to the CLI itself
| s-rectangle -r 6 -c 10      | a rectangle shape with 6 rows and 10 columns
| g-ortho                     | an orthogonal grid type
| a-hk -s 1                   | use the "Hunt and Kill" algorithm with a random seed of 1 to generare the maze
| rt-ortho -e                 | the text unicode render with an entrace and an exit
| o-file -p "./test.txt"      | save on the disk into the specified file

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

To get every options for a given action, use ``--help`` on an action, for example ``s-rectangle --help``

## Website

A website in preview version is available [here](http://mazes.apixelinspace.com/). Altough only random mazes are possibles for now, the website will be expaned in the future.