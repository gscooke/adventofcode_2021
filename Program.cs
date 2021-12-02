// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Advent of Code Puzzles!");

List<IPuzzle> puzzles = new() {
    new Day2()
};

foreach(IPuzzle puzzle in puzzles)
    puzzle.Run();