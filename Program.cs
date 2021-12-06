// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Advent of Code Puzzles!");

var playAll = false;

List<IPuzzle> puzzles = new() {
    new Day1.Puzzle(),
    new Day2.Puzzle(),
    new Day3.Puzzle(),
    new Day4.Puzzle(),
    new Day5.Puzzle(),
    new Day6.Puzzle()
};

if (playAll) {
foreach(IPuzzle puzzle in puzzles)
    puzzle.Run();
}
else
{
    puzzles.Last().Run();
}