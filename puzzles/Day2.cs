namespace Day2;

internal class Puzzle : BasePuzzle, IPuzzle
{
    public void Run() {
        StartPuzzleOutput(nameof(Day2));

        Part1();
        Part2();
    }

    private void Part1() {
        var movements = ParseInputs(InputsList);

        int position = 0;
        int depth = 0;

        foreach(Movement movement in movements) {
            switch (movement.InputDirection)
            {
                case MovementType.Forward:
                    position += movement.InputAmount;
                    break;
                default:
                    depth += movement.DepthChange;
                    break;
            }
        }

        Console.WriteLine($" Part 1:");
        Console.WriteLine($"  Position: {position}");
        Console.WriteLine($"  Depth: {depth}");
        Console.WriteLine($"  Result: {position * depth}");
    }

    private void Part2() {
        var movements = ParseInputs(InputsList);

        int position = 0;
        int depth = 0;
        int aim = 0;

        foreach(Movement movement in movements) {
            switch (movement.InputDirection)
            {
                case MovementType.Forward:
                    position += movement.InputAmount;
                    depth += movement.InputAmount * aim;
                    break;
                default:
                    aim += movement.DepthChange;
                    break;
            }
        }

        Console.WriteLine($" Part 2:");
        Console.WriteLine($"  Position: {position}");
        Console.WriteLine($"  Depth: {depth}");
        Console.WriteLine($"  Result: {position * depth}");
    }

    private List<Movement> ParseInputs(List<string> inputs) {
        return inputs.Select(e => ParseMovement(e)).ToList();
    }

    private Movement ParseMovement(string input) {
        string[] inputs = input.Split(' ');
        return new((MovementType)Enum.Parse(typeof(MovementType), inputs[0], true), int.Parse(inputs[1]));
    }

    internal record Movement(MovementType InputDirection, int InputAmount) {
        public int DepthChange {
            get {
                switch (InputDirection)
                {
                    case MovementType.Up:
                        return -InputAmount;

                    default:
                        return InputAmount;
                }
            }
        }
    }
}

internal enum MovementType {
    Forward,
    Down,
    Up
}