namespace Day3;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day3));

        Part1();
        Part2();
    }

    private void Part1() {
        var inputs = InputsList;
        List<string> pivot = new();
        
        for (int i = 0; i < inputs[0].Length; i++) {
            pivot.Add(
                string.Join("", inputs.Select(e => e.Substring(i, 1)))
            );
        }

        var gammaRateBinary = string.Join("", pivot.Select(e => e.Count(v => v == '0') > e.Count(v => v == '1') ? 0 : 1));
        var epsilonRateBinary = string.Join("", gammaRateBinary.ToCharArray().Select(e => e == '0' ? '1' : '0'));

        var gammaRate = Convert.ToInt32(gammaRateBinary, 2);
        var epsilonRate = Convert.ToInt32(epsilonRateBinary, 2);

        Console.WriteLine($" Part 1:");
        Console.WriteLine($"  Gamma Rate: {gammaRateBinary} == {gammaRate}");
        Console.WriteLine($"  Epsilon Rate: {epsilonRateBinary} == {epsilonRate}");
        Console.WriteLine($"  Result: {gammaRate * epsilonRate}");
    }

    private void Part2() {
        var inputs = InputsList;
        List<List<char>> columns = new();
        
        for (int i = 0; i < inputs[0].Length; i++) {
            columns.Add(
                inputs.Select(e => e.Substring(i, 1).First()).ToList()
            );
        }

        int colIndex = 0;
        while(inputs.Count > 1) {
            var column = inputs.Select(e => e.Substring(colIndex, 1).First()).ToList();
            var value = GetValueFromColumn(column);
            inputs = inputs.Where(e => e.Substring(colIndex, 1).First() == value).ToList();
            colIndex++;
        }
        var oxygenRatingBinary = inputs[0];

        inputs = InputsList;
        colIndex = 0;
        while(inputs.Count > 1) {
            var column = inputs.Select(e => e.Substring(colIndex, 1).First()).ToList();
            var value = GetValueFromColumn(column, false);
            inputs = inputs.Where(e => e.Substring(colIndex, 1).First() == value).ToList();
            colIndex++;
        }
        var co2RatingBinary = inputs[0];
        
        var oxygenRating = Convert.ToInt32(oxygenRatingBinary, 2);
        var co2Rating = Convert.ToInt32(co2RatingBinary, 2);

        Console.WriteLine($" Part 2:");
        Console.WriteLine($"  Oxygen Rate: {oxygenRatingBinary} == {oxygenRating}");
        Console.WriteLine($"  CO2 Rate: {co2RatingBinary} == {co2Rating}");
        Console.WriteLine($"  Result: {oxygenRating * co2Rating}");
    }

    private char GetValueFromColumn(List<char> column, bool mostCommon = true) {
        var commonZero = mostCommon ? '0' : '1';
        var commonOne = mostCommon ? '1' : '0';

        var colval = column.Count(v => v == '1') >= column.Count(v => v == '0') ? commonOne : commonZero;

        return colval;
    }
}
