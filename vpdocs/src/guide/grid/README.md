# Grid

A grid holds the connections between the cells and their configuration.

- [Grid](#grid)
  - [Orthogonal](#orthogonal)
  - [Hexagonal](#hexagonal)
  - [Triangular](#triangular)
  - [Octagonal and Square](#octagonal-and-square)
  - [Pentagonal Cairo](#pentagonal-cairo)
  - [Brick](#brick)
  - [Polar](#polar)

## Orthogonal

Classic grid where every cell has four neighbors (except on the edge)

g-ortho

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Hexagonal

Hexagonal grid where every cell has six neighbors (except on the edge)

g-hex

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Triangular

Triangular grid where every cell has three neighbors (except on the edge)

g-tri

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Octagonal and Square

Octagonal and square grid where every cell has eight or four neighbors (except on the edge)

g-octas

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Pentagonal Cairo

Pentagon 'Cairo' grid where every cell has five neighbors (except on the edge)

g-pentac

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Brick

Brick grid where every cell has six neighbors (except on the edge). Same as the hexagonal grid but the connections between the cells is different.

g-brick

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```

## Polar

Polar grid where every cell has a number of neighbors that expand toward the edge.

g-polar

```
  -e, --empty    (Default: false) If true, the grid will have no internal connections

  -s, --seed     RNG seed, if none is provided a random one is chosen

  -w, --weave    (Default: 0) Value between 0.0 and 1.0 to generate 'bridges' / weave the maze
```