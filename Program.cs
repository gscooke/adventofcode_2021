// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Advent of Code Puzzles!");

var playAll = true;

List<IPuzzle> puzzles = new() {
    new Day1(),
    new Day2()
};

if (playAll) {
foreach(IPuzzle puzzle in puzzles)
    puzzle.Run();
}
else
{
    puzzles.Last().Run();
}