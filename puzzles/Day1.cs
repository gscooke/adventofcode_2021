internal class Day1 : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day1));

        Part1();
        Part2();
    }

    internal void Part1() {
        var inputs = InputsList.Select(int.Parse).ToList();
        var increases = CountValueIncreases(inputs);

        Console.WriteLine($" Part 1:");
        Console.WriteLine($"  Increases: {increases}");
    }

    internal void Part2() {
        var inputs = InputsList.Select(int.Parse).ToList();
        List<int> windows = new();

        for (int index = 0; index < inputs.Count; index++) {
            if (TryGetSlidingWindow(inputs, index, out var windowValue))
                windows.Add(windowValue);
            else
                break;
        }

        var increases = CountValueIncreases(windows);

        Console.WriteLine($" Part 2:");
        Console.WriteLine($"  Increases: {increases}");
    }

    internal int CountValueIncreases(List<int> values) {
        int currentValue = values[0];
        int increases = 0;
        values.RemoveAt(0);

        foreach(int value in values)
        {
            if (value > currentValue)
                increases++;

            currentValue = value;
        }

        return increases;
    }

    internal bool TryGetSlidingWindow(List<int> inputs, int startLine, out int value)
    {
        const int windowSize = 3;
        value = 0;

        if (inputs.Count < startLine + windowSize)
            return false;

        for (int i = 0; i < windowSize; i++) {
            var lineNumber = startLine + i;
            value += inputs[lineNumber];
        }

        return true;
    }
}