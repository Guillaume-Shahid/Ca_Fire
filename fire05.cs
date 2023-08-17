//Créez un programme qui trouve le plus court chemin entre l’entrée et la sortie d’un labyrinthe en évitant les obstacles.
using System.Collections;

namespace _06_NewLabyrinthe;

internal abstract class Labyrinthe
{
    private class Parameters
    {
        public string Lines { get; }
        public string Columns { get; }
        public char WallCharacter { get; }
        public char EmptyCharacter { get; }
        public char PathCharacter { get; }
        public char StartCharacter { get; }
        public char EndCharacter { get; }

        public Parameters(IReadOnlyList<string> parameters)
        {
            Lines = parameters[0];
            Columns = parameters[1];
            WallCharacter = parameters[2][0];
            EmptyCharacter = ' ';
            PathCharacter = parameters[3][0];
            StartCharacter = parameters[4][0];
            EndCharacter = parameters[5][0];
        }
    }

    private class Square
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; init; }
        private int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Square? Parent { get; init; }

        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - this.X) + Math.Abs(targetY - this.Y);
        }
    }

    private static List<Square> GetWalkableSquares(IReadOnlyList<string> map, Square currentSquare, Square targetSquare)
    {
        var possibleSquares = new List<Square>()
        {
            new()
            {
                X = currentSquare.X, Y = currentSquare.Y - 1, Parent = currentSquare,
                Cost = currentSquare.Cost + 1
            },
            new()
            {
                X = currentSquare.X, Y = currentSquare.Y + 1, Parent = currentSquare,
                Cost = currentSquare.Cost + 1
            },
            new()
            {
                X = currentSquare.X - 1, Y = currentSquare.Y, Parent = currentSquare,
                Cost = currentSquare.Cost + 1
            },
            new()
            {
                X = currentSquare.X + 1, Y = currentSquare.Y, Parent = currentSquare,
                Cost = currentSquare.Cost + 1
            }
        };

        possibleSquares.ForEach(square => square.SetDistance(targetSquare.X, targetSquare.Y));

        var maxX = map.First().Length - 1;
        var maxY = map.Count - 1;

        return possibleSquares
            .Where(square => square.X >= 0 && square.X <= maxX)
            .Where(square => square.Y >= 0 && square.Y <= maxY)
            .Where(square => map[square.Y][square.X] == ' ' || map[square.Y][square.X] == '2')
            .ToList();
    }

    private static int GetYSquare(List<string> map, char parameters)
    {
        return map.FindIndex(y => y.Contains(parameters));
    }

    private static int GetXSquare(Square square, IReadOnlyList<string> map, char parameters)
    {
        return map[square.Y].IndexOf(parameters);
    }

    private static bool CheckIfNArgExists(IReadOnlyCollection<string> args, int nArgs)
    {
        return ((args.Count == (nArgs)));
    }

    private static List<string> OpenFile(IReadOnlyList<string> args)
    {
        var listWithOpenedFile = new List<string>();
        try
        {
            using var fileReader = new StreamReader(args[0]);
            while (fileReader.ReadLine() is { } line)
            {
                listWithOpenedFile.Add(line);
            }
        }
        catch
        {
            Console.WriteLine("error");
        }

        return listWithOpenedFile;
    }

    private static List<string> GetWidthHeightAndWallCharacter(IReadOnlyList<string> board)
    {
        var listWithWidthHeightAndWallCharacter = new List<string>();
        string numberInChar = "";

        foreach (var character in board[0])
        {
            if (character >= '0' && character <= '9' && listWithWidthHeightAndWallCharacter.Count < 2)
            {
                numberInChar += character;
            }
            else if (listWithWidthHeightAndWallCharacter.Count < 2)
            {
                listWithWidthHeightAndWallCharacter.Add(numberInChar);
                numberInChar = "";

                if (listWithWidthHeightAndWallCharacter.Count == 2)
                {
                    listWithWidthHeightAndWallCharacter.Add(character.ToString());
                    return listWithWidthHeightAndWallCharacter;
                }
            }
        }
        return listWithWidthHeightAndWallCharacter;
    }

     private static List<string> GetParameters(IReadOnlyList<string> board)
    {
        List<string> listWithParameters = GetWidthHeightAndWallCharacter(board);
        List<string> splitedListFromFile = board[0].Split(' ').ToList();

        foreach (var character in splitedListFromFile[1])
        {
            listWithParameters.Add(character.ToString());
        }


        return listWithParameters;

    }

    private static bool IsNumberOfLineEqualToNumberInFirstLine(IReadOnlyList<string> board, Parameters parameters)
    {
        int numberAtFirstLine = int.Parse(parameters.Lines);
        return (board.Count - 1 == numberAtFirstLine);
    }

    private static bool IsNumberOfColumnEqualToNumberInFirstLine(IReadOnlyList<string> board, Parameters parameters)
    {
        int numberAtFirstLine = int.Parse(parameters.Columns);
        return (board[1].Length == numberAtFirstLine);
    }

    private static bool IsLineHaveSameLength(IReadOnlyList<string> board)
    {
        int lineLength = board[1].Length;

        for (int i = 2; i < board.Count; i++)
        {
            if (board[i].Length != lineLength)
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsThereOneLineAndOneSquareMinimum(IReadOnlyList<string> board)
    {
        int lineLength = board[1].Length;
        int lineCount = board.Count;
        if (lineCount >= 1 && lineLength >= 1)
        {
            return true;
        }
        return false;
    }

    private static bool IsCharAreTheSameThanFirstLine(IReadOnlyList<string> board, Parameters parameters)
    {
        for (int i = 1; i < board.Count; i++)
        {
            foreach (var character in board[i])
            {
                if (character != parameters.WallCharacter &&
                    character != parameters.EmptyCharacter &&
                    character != parameters.StartCharacter &&
                    character != parameters.EndCharacter &&
                    character != parameters.EmptyCharacter)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsBoardValid(List<string> board, Parameters parameters)
    {
        if (IsNumberOfLineEqualToNumberInFirstLine(board, parameters) &&
            IsNumberOfColumnEqualToNumberInFirstLine(board, parameters) &&
            IsLineHaveSameLength(board) &&
            IsThereOneLineAndOneSquareMinimum(board) &&
            IsCharAreTheSameThanFirstLine(board, parameters))
        {
            return true;
        }
        return false;
    }

    private static List<string> GetBoard(IReadOnlyList<string> board)
    {
        List<string> listWithBoard = new List<string>();

        for (int i = 1; i < board.Count; i++)
        {
            listWithBoard.Add(board[i]);
        }

        return listWithBoard;
    }

    private static void PutPathCharacterInBoard(IList<string> map, Square square, Parameters parameters)
    {
        if (map[square.Y][square.X] == parameters.EmptyCharacter)
        {
            var newMapRow = map[square.Y].ToCharArray();
            newMapRow[square.X] = parameters.PathCharacter;
            map[square.Y] = new string(newMapRow);
        }
    }

    private static void PrintLabyrintheIfWeAreAtTheEndCharacter(List<string> map, Square checkSquare, Square finish, Parameters parameters)
    {
        if (checkSquare.X == finish.X && checkSquare.Y == finish.Y)
        {
            var square = checkSquare;
            int countWayToExit = -2;

            while (true)
            {
                PutPathCharacterInBoard(map, square, parameters);
                square = square.Parent;
                countWayToExit++;

                if (square == null)
                {
                    map.ForEach(Console.WriteLine);
                    Console.WriteLine($"=> SORTIE ATTEINTE EN {countWayToExit} COUPS !");
                    return;
                }
            }
        }
    }

    private static void FindNextSquareToGo(List<Square> walkableSquares, ICollection<Square> activeSquare, IReadOnlyCollection<Square> visitedSquare)
    {
        foreach (var walkableSquare in walkableSquares)
        {
            if (visitedSquare.Any(square => square.X == walkableSquare.X && square.Y == walkableSquare.Y))
            {
                continue;
            }

            if (activeSquare.Any(square => square.X == walkableSquare.X && square.Y == walkableSquare.Y))
            {
                var existingSquare = activeSquare.First(square =>  square.X == walkableSquare.X && square.Y == walkableSquare.Y);
                if (existingSquare.CostDistance > walkableSquare.CostDistance)
                {
                    activeSquare.Remove(existingSquare);
                    activeSquare.Add(walkableSquare);
                }
            }
            else
            {
                activeSquare.Add(walkableSquare);
            }
        }
    }

    private static void ResoleLabyrinthe(List<string> map, Parameters parameters)
    {
        var start = new Square();
        start.Y = GetYSquare(map, parameters.StartCharacter);
        start.X = GetXSquare(start, map, parameters.StartCharacter);

        var finish = new Square();
        finish.Y = GetYSquare(map, parameters.EndCharacter);
        finish.X = GetXSquare(finish, map, parameters.EndCharacter);

        start.SetDistance(finish.X, finish.Y);

        var activeSquare = new List<Square> { start };
        var visitedSquare = new List<Square>();

        while (activeSquare.Any())
        {
            var checkSquare = activeSquare.OrderBy(square => square.CostDistance).First();

            PrintLabyrintheIfWeAreAtTheEndCharacter(map, checkSquare, finish, parameters);

            visitedSquare.Add(checkSquare);
            activeSquare.Remove(checkSquare);

            var walkableSquares = GetWalkableSquares(map, checkSquare, finish);

            FindNextSquareToGo(walkableSquares, activeSquare, visitedSquare);   
        }
    }

    public static void Main(string[] args)
    {
        if (!CheckIfNArgExists(args, 1))
        {
            Console.WriteLine("error");
            return;
        }

        var board = OpenFile(args);
        var parametersList = GetParameters(board);
        var parameters = new Parameters(parametersList);
        var map = GetBoard(board);

       if (!IsBoardValid(board, parameters))
       {
            Console.WriteLine("error");
            return;
       }

        ResoleLabyrinthe(map, parameters);
    }
}