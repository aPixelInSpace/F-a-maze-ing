# Canvas

The basic shape of the maze.

There are two types of canvas : Array2d and ArrayOfA.

Array2d canvas can be used on all but the polar maze, only ArrayOfA canvas can be used for a polar maze.

  - [Rectangle](#rectangle)
  - [Triangle](#triangle)
  - [Ellipse](#ellipse)
  - [Hexagon](#hexagon)
  - [Pentagon](#pentagon)
  - [Pentagon Star](#pentagon-star)
  - [Image](#image)
  - [Disk](#disk)

## Rectangle

`s-rectangle`

```
  -r, --rows       Required. The number of rows.

  -c, --columns    Required. The number of columns.
```

(Array2d type)

## Triangle

`s-triangle`

```
  -b, --base           Required. The length of the base.

  --baseat             (Default: Bottom) The position of the base (*Bottom, Top, Left or Right).

  --basedecrement      (Default: 1) The decrement value for the base.

  --heightincrement    (Default: 1) The increment value for the height.
```

(Array2d type)

## Ellipse

`s-ellipse`

```
  -r, --rowRadiusLength        Required. The length for the horizontal radius.

  -c, --columnRadiusLength     Required. The length for the vertical radius.

  --rowenlargingfactor         (Default: 0.0) Zoom factor on the horizontal axis.

  --columnenlargingfactor      (Default: 0.0) Zoom factor on the vertical axis.

  --rowtranslationfactor       (Default: 0) Translation factor on the horizontal axis.

  --columntranslationfactor    (Default: 0) Translation factor on the vertical axis.

  --ellipsefactor              (Default: 0.0) Inside ellipse factor.

  --side                       (Default: Inside) Indicate whether the ellipse is *Inside or Outside.
```

(Array2d type)

## Hexagon

`s-hexagon`

```
  -s, --edgeSize    Required. The length of one side of the hexagon.
```

(Array2d type)

## Pentagon

`s-pentagon`

```
  -s, --edgeSize    Required. The length of one side of the pentagon.
```

(Array2d type)

## Pentagon Star

`s-pentagonStar`

```
  -g, --greatEdgeSize    Required. The length of the great side of the pentagon star.

  -s, --smallEdgeSize    Required. The length of the small side of the pentagon star.
```

(Array2d type)

## Image

`s-image`

Get a canvas/shape from an image. It works on the levels of black (with a parametrable tolerance) on each pixel.


```
  -p, --path         Required. The full path of the image file

  -t, --tolerance    (Default: 0) The tolerance on the pixel color.
```

(Array2d type)

## Disk

`s-disk`

```
  -r, --rings     Required. The number of rings.

  -w, --ratio     (Default: 1) Width/height ratio.

  -c, --center    (Default: 3) Number of cells for the central ring.
```

(ArrayOfA type)