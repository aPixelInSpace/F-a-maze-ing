# Maze Algorithm

A maze algorithm will operate on a grid to open or close connections between cells.

Some algorithm works by choosing a 'direction' instead of choosing a neighbor. These 'direction' algorithms might not give a valid maze (on some grid configuration) where every cell is reachable.

  - [Binary Tree](#binary-Tree)
  - [Sidewinder](#sidewinder)
  - [Aldous Broder](#aldous-broder)
  - [Wilson](#wilson)
  - [Hunt and Kill](#hunt-and-kill)
  - [Recursive Backtracker](#recursive-backtracker)
  - [Kruskal](#kruskal)
  - [Prim simple](#prim-simple)
  - [Prim simple modified](#prim-simple-modified)
  - [Prim weighted](#prim-weighted)
  - [Eller](#eller)
  - [Recursive Division](#recursive-division)
  - [Growing Tree mix random and last](#growing-tree-mix-random-and-last)
  - [Growing Tree mix chosen random and last](#growing-tree-mix-chosen-random-and-Last)
  - [Growing Tree mix oldest and last](#growing-tree-mix-oldest-and-last)
  - [Growing Tree direction](#growing-tree-direction)
  - [Growing Tree spiral](#growing-tree-spiral)
  - [No maze](#no-maze)

## Binary Tree

`a-bt`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  --direction1          (Default: Top) First direction (*Top, Left, Bottom or Right)

  --direction2          (Default: Right) Second direction (Top, Left, Bottom or *Right)

  --direction1weight    (Default: 1) Weight for the direction 1

  --direction2weight    (Default: 1) Weight for the direction 2
```

## Sidewinder

`a-sw`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  --direction1          (Default: Top) First direction (*Top, Left, Bottom or Right)

  --direction2          (Default: Right) Second direction (Top, Left, Bottom or *Right)

  --direction1weight    (Default: 1) Weight for the direction 1

  --direction2weight    (Default: 1) Weight for the direction 2
```

## Aldous Broder

`a-ab`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Wilson

`a-ws`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Hunt and Kill

`a-hk`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Recursive Backtracker

`a-rb`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Kruskal

`a-kr`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Prim simple

`a-ps`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Prim simple modified

`a-psm`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen
```

## Prim weighted

`a-pw`

```
  -s, --seed      RNG seed, if none is provided a random one is chosen

  -w, --weight    Weight, if none is provided a random one is chosen
```

## Eller

`a-el`

```
  -s, --seed      RNG seed, if none is provided a random one is chosen
```

## Recursive Division

`a-rd`

```
  -s, --seed       RNG seed, if none is provided a random one is chosen

  -r, --rooms      (Default: 0.0) Probability to generate a room (a space with no wall), 
                    range from 0.0 no room to 1.0 always generate a room. If this parameter 
                    is greater than 0.0 do not forget to create the grid empty with -e

  --roomsheight    (Default: 3) Room height

  --roomswidth     (Default: 3) Room width
```

## Eller

`a-el`

```
  -s, --seed      RNG seed, if none is provided a random one is chosen
```

## Growing Tree mix random and last

`a-gtmrl`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  -l, --longPassages    (Default: 0.5) Probability to generate long passages from 0.0 
                         always choose a random neighbor to 1.0 always choose the last
                         (same as recursive backtracker)
```

## Growing Tree mix chosen random and last

`a-gtmrl`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  -l, --longPassages    (Default: 0.5) Probability to generate long passages from 0.0
                         always choose a random neighbor (stick to it until it has no 
                         neighbor) to 1.0 always choose the last
                         (same as recursive backtracker)
```

## Growing Tree mix oldest and last

`a-gtmrl`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  -l, --longPassages    (Default: 0.5) Probability to generate long passages from 0.0
                         always choose the oldest neighbor to 1.0 always choose the last 
                         (same as recursive backtracker)
```

## Growing Tree direction

`a-gtd`

```
  -s, --seed          RNG seed, if none is provided a random one is chosen

  --torightweight     (Default: 0.5) Probability between 0.0 and 1.0 to choose the neighbor
                       on the right (all the probability cannot be > 1.0)

  --tobottomweight    (Default: 0.3) Probability between 0.0 and 1.0 to choose the neighbor
                       on the bottom (all the probability cannot be > 1.0)

  --toleftweight      (Default: 0.1) Probability between 0.0 and 1.0 to choose the neighbor
                       on the left (all the probability cannot be > 1.0)
```

## Growing Tree spiral

`a-gts`

```
  -s, --seed            RNG seed, if none is provided a random one is chosen

  --spiralweight        (Default: 1.0) Probability between 0.0 and 1.0 to choose the neighbor 
                         that will make a spiral

  --spiraluniformity    (Default: 1.0) Probability between 0.0 and 1.0 to make perfect spiral 
                         (as best as possible)

  --spiralmaxlength     (Default: 4) Max length for the spiral

  --spiralrevolution    (Default: 0.0) Probability between 0.0 (counter-clockwise)
                         and 1.0 (clockwise) to choose the revolution of the spiral
```

## No maze

`a-nm`

```
  --nooption
```
