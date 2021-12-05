namespace Day4;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run()
    {
        Console.ResetColor();
        StartPuzzleOutput(nameof(Day4));

        Part1().Wait();
        Part2();
    }

    private async Task Part1()
    {
        ColorPallette colorPallette = new();

        var inputs = InputsList;
        var calledNumbers = ExtractCalledNumbers(inputs);

        var bingoBoards = PopulateBingoBoards(inputs);

        var calledNumberIndex = 0;
        while (bingoBoards.All(e => !e.HasWinningLine))
        {
            VisualiseBoards(bingoBoards, calledNumbers, calledNumberIndex, colorPallette);
            var calledNumber = calledNumbers[calledNumberIndex];
            bingoBoards.ForEach(e => e.CallNumber(calledNumber));

            if (bingoBoards.All(e => !e.HasWinningLine))
                calledNumberIndex++;

            await Task.Delay(500);
        }

        VisualiseBoards(bingoBoards, calledNumbers, calledNumberIndex, colorPallette);
        var winningBoard = bingoBoards.First(e => e.HasWinningLine);

        Console.WriteLine($" Part 1:");
        Console.WriteLine($"  Winning Board Score: {winningBoard.Score}");
    }

    private void Part2()
    {
        var inputs = InputsList;
        var calledNumbers = ExtractCalledNumbers(inputs);

        var bingoBoards = PopulateBingoBoards(inputs);

        var calledNumberIndex = 0;
        BingoBoard? lastBoard = null;
        while (bingoBoards.Any(e => !e.HasWinningLine))
        {
            var calledNumber = calledNumbers[calledNumberIndex];
            bingoBoards.ForEach(e => e.CallNumber(calledNumber));

            if (bingoBoards.All(e => e.HasWinningLine)) {
                lastBoard = bingoBoards.First(e => e.LastNumberCalled == calledNumber);
                break;
            }

            if (bingoBoards.Any(e => !e.HasWinningLine))
                calledNumberIndex++;
        }

        Console.WriteLine($" Part 2:");
        Console.WriteLine($"  Winning Board Score: {lastBoard?.Score}");
    }

    private void VisualiseBoards(List<BingoBoard> bingoBoards, List<int> CalledNumbers, int CalledNumberIndex, ColorPallette colorPallette) {
        Console.Clear();
        for (int skip = 0; skip < bingoBoards.Count; skip += 12) {
            var bingoBoardsToRender = bingoBoards.Skip(skip).Take(12);
            for (int lineIndex = 0; lineIndex < 5; lineIndex++) {
                foreach(var bingoBoard in bingoBoardsToRender) {
                    bingoBoard.Render(lineIndex, colorPallette);
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        Console.ResetColor();

        var numbersCalled = string.Join(", ", CalledNumbers.GetRange(0, CalledNumberIndex + 1));
        Console.WriteLine($"Numbers Called: {numbersCalled}");
    }

    private List<BingoBoard> PopulateBingoBoards(List<string> inputs)
    {
        List<BingoBoard> bingoBoards = new();
        foreach (var input in inputs)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                bingoBoards.Add(new BingoBoard());
                continue;
            }

            var lineNumbers = input.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(int.Parse);
            bingoBoards.Last().AddLine(lineNumbers);
        }

        return bingoBoards;
    }

    private List<int> ExtractCalledNumbers(List<string> inputs)
    {
        var calledNumbers = inputs[0].Split(',').Where(e => !string.IsNullOrWhiteSpace(e)).Select(int.Parse).ToList();
        inputs.RemoveAt(0);
        return calledNumbers;
    }

    private class BingoBoard
    {
        public BingoBoard()
        {
            Lines = new();
            Columns = new()
            {
                new BingoLine(),
                new BingoLine(),
                new BingoLine(),
                new BingoLine(),
                new BingoLine()
            };
        }

        public List<BingoLine> Lines { get; set; }
        public List<BingoLine> Columns { get; set; }
        public int LastNumberCalled { get; private set; }

        public void AddLine(IEnumerable<int> values)
        {
            BingoLine bingoLine = new();
            foreach (var (value, index) in values.WithIndex())
            {
                bingoLine.Add(new BingoBoardValue(value));
                Columns[index].Add(new BingoBoardValue(value));
            }

            Lines.Add(bingoLine);
        }

        public void CallNumber(int calledNumber)
        {
            if (!HasWinningLine)
            {
                Lines.ForEach(e => e.CallNumber(calledNumber));
                Columns.ForEach(e => e.CallNumber(calledNumber));

                if (HasWinningLine)
                    LastNumberCalled = calledNumber;
            }
        }

        public bool HasWinningLine
        {
            get
            {
                return Lines.Any(e => e.IsWinningLine) || Columns.Any(e => e.IsWinningLine);
            }
        }

        public List<int> UnmarkedValues
        {
            get
            {
                List<int> unmarkedValues = new();
                Lines.ForEach(e => unmarkedValues.AddRange(e.UnmarkedValues));
                return unmarkedValues;
            }
        }

        public int Score
        {
            get
            {
                if (!HasWinningLine)
                    return 0;

                return UnmarkedValues.Sum() * LastNumberCalled;
            }
        }

        public void Render(int lineIndex, ColorPallette colorPallette) {
            ConsoleColor? backgroundcolor = HasWinningLine ? ConsoleColor.Green : null;
            foreach(var value in Lines[lineIndex])
            {
                value.Render(HasWinningLine, Lines[lineIndex].IsWinningLine, colorPallette);
            }
        }
    }

    private class BingoLine : List<BingoBoardValue>
    {
        public void CallNumber(int calledNumber)
        {
            this.Where(e => e.Value == calledNumber).ToList().ForEach(e => e.Marked = true);
        }

        public bool IsWinningLine
        {
            get
            {
                return this.All(e => e.Marked);
            }
        }

        public List<int> UnmarkedValues
        {
            get
            {
                return this.Where(e => !e.Marked).Select(e => e.Value).ToList();
            }
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(e => e.Value));
        }
    }

    private class BingoBoardValue
    {
        public BingoBoardValue(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
        public bool Marked { get; set; }

        public override string ToString()
        {
            return $"{Value} {(Marked ? "[X]" : "[0]")}";
        }

        public void Render(bool IsWinningBoard, bool IsWinningLine, ColorPallette colorPallette) {
            var value = Value.ToString().PadLeft(2) + " ";
            colorPallette.ResetColor();

            if (IsWinningBoard) {
                Console.BackgroundColor = colorPallette.WinningBoardBackgroundColor;
                Console.ForegroundColor = colorPallette.DefaultForegroundColor;

                if (Marked && IsWinningLine) {
                    Console.ForegroundColor = colorPallette.WinningLineNumberColor;
                }
                else if (Marked) {
                    Console.ForegroundColor = colorPallette.WinningMatchedNumberColor;
                }
            }
            else {
                Console.ForegroundColor = colorPallette.DefaultForegroundColor;
                if (Marked) {
                    Console.ForegroundColor = colorPallette.DefaultMatchedNumberColor;
                }
            }
            
            Console.Write(value);
            colorPallette.ResetColor();
        }
    }

    private class ColorPallette {
        public ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor DefaultMatchedNumberColor { get; set; } = ConsoleColor.White;
        public ConsoleColor WinningLineNumberColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor WinningMatchedNumberColor { get; set; } = ConsoleColor.White;
        public ConsoleColor WinningBoardBackgroundColor { get; set; } = ConsoleColor.DarkGreen;

        public void ResetColor() {
            Console.ResetColor();
            Console.ForegroundColor = DefaultForegroundColor;
        }
    }
}
