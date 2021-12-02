internal class BasePuzzle {
    internal void StartPuzzleOutput(string puzzleName) {
        Console.WriteLine($"{Environment.NewLine}Starting {nameof(Day2)}");
    }

    internal List<string> LoadInputs(string fileName) {
        return File.ReadAllLines($"inputs/{fileName}").ToList();
    }
}