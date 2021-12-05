using System.Drawing;

namespace Day5;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day5));

        Part1();
        Part2();
    }

    private void Part1() {
        var inputs = InputsList.Select(e => new Ventline(e)).Where(e => e.VentDirection != VentDirection.Diagonal).ToList();
        var map = BuildMap(inputs);
        PlotVentlines(map, inputs);

        var intersections = 0;
        for (int x = 0; x <= map.GetUpperBound(0); x++) {
            for (int y = 0; y <= map.GetUpperBound(1); y++) {
                if (map[x,y] > 1)
                    intersections++;
            }
        }

        Console.WriteLine($" Part 1:");
        Console.WriteLine($"  Intersections: {intersections}");
    }

    private void Part2() {
        var inputs = InputsList.Select(e => new Ventline(e)).ToList();
        var map = BuildMap(inputs);
        PlotVentlines(map, inputs);

        var intersections = 0;
        for (int x = 0; x <= map.GetUpperBound(0); x++) {
            for (int y = 0; y <= map.GetUpperBound(1); y++) {
                if (map[x,y] > 1)
                    intersections++;
            }
        }

        Console.WriteLine($" Part 2:");
        Console.WriteLine($"  Intersections: {intersections}");
    }

    private int[,] BuildMap(List<Ventline> ventlines) {
        var boundX = ventlines.Max(e => e.MaxBoundX + 1);
        var boundY = ventlines.Max(e => e.MaxBoundY + 1);
        return new int[boundX, boundY];
    }

    private void PlotVentlines(int[,] map, List<Ventline> ventlines) {
        ventlines.ForEach(ventLine => {
            var ventlinePoints = GetPointsOnLine(ventLine.Start.X, ventLine.Start.Y, ventLine.End.X, ventLine.End.Y).ToList();
            ventlinePoints.ForEach(point => {
                map[point.X, point.Y]++;
            });
        });
    }

    private IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            int t;
            t = x0; // swap x0 and y0
            x0 = y0;
            y0 = t;
            t = x1; // swap x1 and y1
            x1 = y1;
            y1 = t;
        }
        if (x0 > x1)
        {
            int t;
            t = x0; // swap x0 and x1
            x0 = x1;
            x1 = t;
            t = y0; // swap y0 and y1
            y0 = y1;
            y1 = t;
        }
        int dx = x1 - x0;
        int dy = Math.Abs(y1 - y0);
        int error = dx / 2;
        int ystep = (y0 < y1) ? 1 : -1;
        int y = y0;
        for (int x = x0; x <= x1; x++)
        {
            yield return new Point((steep ? y : x), (steep ? x : y));
            error = error - dy;
            if (error < 0)
            {
                y += ystep;
                error += dx;
            }
        }
        yield break;
    }
}

public class Ventline {
    public Point Start { get; set; }
    public Point End { get; set; }
    public int MaxBoundX {
        get {
            return Start.X > End.X ? Start.X : End.X;
        }
    }
    public int MaxBoundY {
        get {
            return Start.Y > End.Y ? Start.Y : End.Y;
        }
    }
    public VentDirection VentDirection {
        get {
            if (Start.X == End.X)
                return VentDirection.Vertical;
            else if (Start.Y == End.Y)
                return VentDirection.Horizontal;

            return VentDirection.Diagonal;
        }
    }

    public Ventline(string input) {
        var inputs = input.Split(" -> ");
        var start = inputs[0].Split(',').Select(int.Parse);
        var end = inputs[1].Split(',').Select(int.Parse);
        Start = new Point(start.ElementAt(0), start.ElementAt(1));
        End = new Point(end.ElementAt(0), end.ElementAt(1));
    }
}

public enum VentDirection {
    Vertical,
    Horizontal,
    Diagonal
}