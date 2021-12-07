namespace Day7;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day7));

        Part1();
        Part2();
    }

    private void Part1() {
        Console.WriteLine($" Part 1:");
        var crabs = InputsSplitListInt;

        var min = crabs.Min();
        var max = crabs.Max();

        Tuple<int, int> bestAlignment = new(int.MaxValue, int.MaxValue);
        for (int i = min; i <= max; i++) {
            var fuelUsed = crabs.Select(startPosition => (i < startPosition) ? startPosition - i : i - startPosition).ToList();
            if (fuelUsed.Sum() < bestAlignment.Item2)
                bestAlignment = new(i, fuelUsed.Sum());
        }
        
        Console.WriteLine($"  Best Position: {bestAlignment.Item1}");
        Console.WriteLine($"  Fuel Used: {bestAlignment.Item2}");
    }

    private void Part2() {
        Console.WriteLine($" Part 2:");
        var crabs = InputsSplitListInt;

        var min = crabs.Min();
        var max = crabs.Max();

        Tuple<int, int> bestAlignment = new(int.MaxValue, int.MaxValue);
        for (int i = 5; i <= max; i++) {
            var fuelUsed = crabs.Select(startPosition => (i < startPosition) ? startPosition - i : i - startPosition)
                                .Select((movements) => {
                                    var fuelRequired = 0;
                                    for(int m = 1; m <= movements; m++)
                                        fuelRequired += m;
                                    return fuelRequired;
                                }).ToList();
            if (fuelUsed.Sum() < bestAlignment.Item2)
                bestAlignment = new(i, fuelUsed.Sum());
        }
        
        Console.WriteLine($"  Best Position: {bestAlignment.Item1}");
        Console.WriteLine($"  Fuel Used: {bestAlignment.Item2}");
    }
}
