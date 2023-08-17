//Créez un programme qui trouve et affiche la solution d’un Sudoku.
using System.Collections.Generic;
internal class Program
{
    private static bool CheckIfNArgExists(string[] args, int nArgrs)
    {
        return ((args.Length == (nArgrs)));
    }

    private static string ReplaceAt(string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    private static bool IsBoardIsNotFull(List<string> board)
    {
        foreach(var line in board)
        {
            if(line.Contains('.'))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsLineContainsOnlyOneNumberToFind(string line)
    {
        int count = 0;

        foreach(var character in line)
        {
            if(character == '.')
            {
                count++;
            }
        }

        return (count == 1);
    }

    private static List<string> PutColumnInLine(List<string> board)
    {
        List<string> tempBoard = new List<string>();
        string tempLine = "";

        for(int i = 0; i < 9; i++)
        {
            tempLine = "";
            for(int j = 0; j < 9; j++)
            {
                tempLine += board[j][i];
            }
            tempBoard.Add(tempLine);
        }
        return tempBoard;
    }

    private static List<string> PutSquareInLine(List<string> board)
    {
        List<string> tempBoard = new List<string>();
        string tempLine = "";

        for(int i = 0; i < 9; i++)
        {
            tempLine = "";
            for(int j = 0; j < 9; j++)
            {
                tempLine += board[(i / 3) * 3 + j / 3][(i % 3) * 3 + j % 3];
            }
            tempBoard.Add(tempLine);
        }
        return tempBoard;
    }



    private static List<string> ReplaceDotByNumberAtGivenIndex(List<string> board, string line)
    {
        if (IsLineContainsOnlyOneNumberToFind(line))
        {
            for (int i = 1; i <= 9; i++)
            {
                if (!line.Contains(i.ToString()))
                {
                    board[board.IndexOf(line)] = ReplaceAt(line, line.IndexOf('.'), i.ToString()[0]);
                    return board;
                }
            }
        }
        return board;
    }
        

    private static List<string> ResolveSudokuPerLine(List<string> board)
    {
        foreach (var line in board)
        {
            if (line.Contains("."))
            {
               return ReplaceDotByNumberAtGivenIndex(board, line); 
            }
        }
        return board;
    }

    private static List<string> ResolveSudokuPerColumn(List<string> board)
    {
        List<string> tempBoard = PutColumnInLine(board);
        tempBoard = ResolveSudokuPerLine(tempBoard);
        board = PutColumnInLine(tempBoard);
        return board;
    }

    private static List<string> ReplaceDotIfOnlyOnePossibility(List<string> board, List<string> column, int i, int j)
    {
        int count = 0;
        string temp = "";
        for(int k = 1; k <= 9; k++)
        {
            if(!board[i].Contains(k.ToString()) && !column[j].Contains(k.ToString()))
            {
                count++;
                temp = k.ToString();
            }
        }
        if(count == 1)
        {
            board[i] = ReplaceAt(board[i], j, temp[0]);
        }
        return board;
    }

    private static List<string> CompareLineAndColumn(List<string> board)
    {
        List<string> column = PutColumnInLine(board);

        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if(board[i][j] == '.')
                {
                   board = ReplaceDotIfOnlyOnePossibility(board, column, i, j); 
                }
            }
        }
        return board;
    }

    private static List<string> ResolveSudokuPerSquare(List<string> board)
    {
        List<string> tempboard = PutSquareInLine(board);
        tempboard = ResolveSudokuPerLine(tempboard);
        board = PutSquareInLine(tempboard);
        return board;
    }

    private static List<string> ResolveSquareComparingLineAndColumn(List<string> board)
    {
        List<string> square = PutSquareInLine(board);
        List<string> column = PutColumnInLine(board);

        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if (square[i][j] == '.')
                {
                    board = ReplaceDotIfOnlyOnePossibility(square, column, (i / 3) * 3 + j / 3, (i % 3) * 3 + j % 3);
                }
            }
        }   
        board = PutSquareInLine(square);
        return board;
    }   

    private static List<string> ResolveSudoku(List<string> board)
    {
        int i = 0;
        while(IsBoardIsNotFull(board) && i < 50)
        {
            board = ResolveSudokuPerLine(board);
            board = ResolveSudokuPerColumn(board);
            board = CompareLineAndColumn(board);
            board = ResolveSudokuPerSquare(board);
            board = ResolveSquareComparingLineAndColumn(board);
            i++;
        }
        return board;
    }

    private static void DisplayBoard(List<string> board)
    {
        if(IsBoardIsNotFull(board))
        {
            Console.WriteLine("error");
            return;
        }

        foreach(var line in board)
        {
            Console.WriteLine(line);
        }
    }

    static void Main(string[] args)
    {
        if(CheckIfNArgExists(args, 1))
        {
            List<string> board = new List<string>();

            try
            {
                board = File.ReadAllLines(args[0]).ToList();
                board = ResolveSudoku(board);
                DisplayBoard(board);   
            }
            catch
            {
                Console.WriteLine("error");
            }
        }
        else
        {
            Console.WriteLine("error");
        }
    }
}