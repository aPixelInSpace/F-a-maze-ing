# Render

A render will transform a maze into a displayable format.

The main way is by generating an SVG, alternativaly, there is a unicode render for orthogonal maze.

  - [SVG Orthogonal](#svg-orthogonal)
  - [SVG Polar](#svg-polar)
  - [SVG Triangle](#svg-triangle)
  - [SVG Octagonal and Square](#svg-octagonal-and-square)
  - [SVG Pentagonal Cairo](#svg-pentagonal-cairo)
  - [SVG Brick](#svg-brick)
  - [Text Orthogonal](#text-orthogonal)

## SVG Orthogonal

`rs-ortho`

```
  -s, --solution                (Default: false) Show solution ?

  -e, --entranceExit            (Default: true) Add an entrance and an exit ?

  -l, --lines                   (Default: Straight) Type of lines (*Straight, Circle, Curved or
                                 Random). In circle mode only the Width value is considered; 
                                 in curved mode you can change the curve option to obtain 
                                 various effects

  -b, --backgroundColor         (Default: Plain) Background coloration (NoColoration, *Plain, 
                                 Distance, GradientV, GradientH, GradientC, Random)

  --wallrendertype              (Default: Line) Type of rendering for 
                                 the walls (*Line or Inset)

  --seed                        RNG seed, if none is provided a random one is chosen

  --curvemultfact               (Default: 5.0) Curve multiplication factor

  --width                       (Default: 30) Width of a single cell

  --height                      (Default: 30) Height of a single cell

  --bridgewidth                 (Default: 10.0) Width of the bridge

  --bridgedistancefromcenter    (Default: 12.0) Distance of the bridge from the 
                                 center of a cell

  --margin                      (Default: 20) Margin for the entire maze

  --curve                       (Default: 0) Change the curve value when drawing a line; 
                                 only applicable in Curved mode on the lines option

  --color1                      (Default: "#FFFFFF") Color choice 1

  --color2                      (Default: "#12A4B5") Color choice 2

  --solutioncolor               (Default: "purple") Color of the solution path

  --walllinecolor               (Default: "#333333") Color of the walls when choosing
                                 the Line wallrendertype option
```

## SVG Polar

`rs-polar`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## SVG Hexagonal

`rs-hex`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## SVG Triangle

`rs-tri`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## SVG Octagonal and Square

`rs-octas`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## SVG Pentagonal Cairo

`rs-pentac`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## SVG Brick

`rs-brick`

```
  -d, --distColor       (Default: false) Distance coloration

  -s, --solution        (Default: false) Show solution ?

  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```

## Text Orthogonal

`rt-ortho`

```
  -e, --entranceExit    (Default: true) Add an entrance and an exit ?
```
