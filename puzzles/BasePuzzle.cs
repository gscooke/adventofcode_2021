internal class BasePuzzle {
    internal string inputFilename = "";

    internal void StartPuzzleOutput(string puzzleName) {
        Console.WriteLine($"{Environment.NewLine}Starting {puzzleName}:");
        inputFilename = $"inputs/{puzzleName}.txt";
    }

    internal List<string> InputsList => File.ReadAllLines(inputFilename).Select(e => e.Trim()).ToList();
    internal byte[] InputsBytearray => File.ReadAllBytes(inputFilename);
    internal List<char> InputsListChar => InputsBytearray.Select(e => (char)e).ToList();
    internal List<int> InputsListInt => InputsList.Select(int.Parse).ToList();
    internal List<int> InputsSplitListInt => InputsList[0].Split(',').Select(int.Parse).ToList();
}