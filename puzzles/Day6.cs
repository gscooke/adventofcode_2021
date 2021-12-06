namespace Day6;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day6));

        Part1();
        Part2();
    }

    private void Part1() {
        Console.WriteLine($" Part 1:");
        var inputs = InputsSplitListInt;

        var fishCount = LanternfishCount(80);
        
        Console.WriteLine($"  Total fish: {fishCount}");
    }

    private void Part2() {
        Console.WriteLine($" Part 2:");
        var inputs = InputsSplitListInt;

        var fishCount = LanternfishCount(256);
        
        Console.WriteLine($"  Total fish: {fishCount}");
    }

    private decimal LanternfishCount(int days)
    {
        var intervalTimes = new decimal[] {0, 0, 0, 0, 0, 0, 0, 0, 0};
        InputsList.First().Split(',')
            .Select(f => Convert.ToInt32(f))
            .ToList()
            .ForEach(iS => intervalTimes[iS]++);

        var day = 0;
        while (day++ < days)
        {
            var numberReadyToPopAndReset = intervalTimes[0];
            var shifting = intervalTimes.ToList();
            shifting.RemoveAt(0);
            shifting.Add(0);
            intervalTimes = shifting.ToArray();
            intervalTimes[6] += numberReadyToPopAndReset;
            intervalTimes[8] = numberReadyToPopAndReset;
        }
        return intervalTimes.Sum();
    }
}
