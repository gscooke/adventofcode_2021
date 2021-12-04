namespace Day4;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run()
    {
        StartPuzzleOutput(nameof(Day4));

        Part1();
        Part2();
    }

    private void Part1()
    {
        var inputs = InputsList;
        var calledNumbers = ExtractCalledNumbers(inputs);

        var bingoBoards = PopulateBingoBoards(inputs);

        var calledNumberIndex = 0;
        while (bingoBoards.All(e => !e.HasWinningLine))
        {
            var calledNumber = calledNumbers[calledNumberIndex];
            bingoBoards.ForEach(e => e.CallNumber(calledNumber));

            if (bingoBoards.All(e => !e.HasWinningLine))
                calledNumberIndex++;
        }

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
        // while (bingoBoards.Any(e => !e.HasWinningLine))
        // {
        //     var calledNumber = calledNumbers[calledNumberIndex];
        //     bingoBoards.ForEach(e => e.CallNumber(calledNumber));

        //     if (bingoBoards.Count > 1)
        //         bingoBoards.RemoveAll(e => e.HasWinningLine);

        //     if (bingoBoards.All(e => !e.HasWinningLine))
        //         calledNumberIndex++;
        // }
        
        bool continueLoop = true;
        BingoBoard? lastBoard = null;
        while (continueLoop)
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
    }

    private class BingoBoardValue
    {
        public BingoBoardValue(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
        public bool Marked { get; set; }
    }
}
