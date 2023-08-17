//Créez un programme qui remplace les caractères vides par des caractères plein pour représenter le plus grand carré possible sur un plateau. Le plateau sera transmis dans un fichier. La première ligne du fichier contient les informations pour lire la carte : nombre de lignes du plateau, caractères pour “vide”, “obstacle” et “plein”.
using System.Globalization;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Drawing;

internal class Program
{
    public class Square
    {
        public int x { get; set; }
        public int y { get; set; }
        public int size { get; set; }

        public Square(int x, int y, int size)
        {
            this.x = x;
            this.y = y;
            this.size = size;
        }
    }

    private static bool CheckIfNArgExists(string[] args, int nArgrs)
    {
        return ((args.Length == (nArgrs)));
    }

    private static string GetFirstParameter(List<string> board)
    {
        string numberInChar = "";

        foreach(var character in board[0])
        {
            if (character >= '0' && character <= '9')
            {
               numberInChar += character;
            }
            else
            {
                return numberInChar;
            }
        }
        return numberInChar;
    }

    private static List<string> GetParameters(List<string> board)
    {
        List<string> parameters = new List<string>();
        string firstParameter = GetFirstParameter(board);

        parameters.Add(firstParameter);
        parameters.Add(board[0][firstParameter.Length].ToString());
        parameters.Add(board[0][firstParameter.Length + 1].ToString());
        parameters.Add(board[0][firstParameter.Length + 2].ToString());

        return parameters;
    }

    private static bool IsNumberOfLineEqualToNumberInFirstLine(List<string> board)
    {
        List<string> parameters = GetParameters(board);
        int numberAtFirstLine = int.Parse(parameters[0]);
        return (board.Count - 1 == numberAtFirstLine);
    }

    private static bool IsLineHaveSameLength(List<string> board)
    {
        int lineLength = board[1].Length;
        for(int i = 2; i < board.Count; i++)
        {
            if (board[i].Length != lineLength)
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsthereOneLineAndOneSquareMinimum(List<string> board)
    {
        int lineLength = board[1].Length;
        int lineCount = board.Count;
        if (lineCount >= 1 && lineLength >= 1)
        {
            return true;
        }
        return false;
    }

    private static bool IsCharAreTheSameThanFirstLine(List<string> board)
    {
        List<string> parameters = GetParameters(board);
        char firstChar = parameters[1][0];
        char secondChar = parameters[2][0];
        char thirdChar = parameters[3][0];

        for(int i = 1; i < board.Count; i++)
        {
            foreach (var character in board[i])
            {
                if (character != firstChar && character != secondChar && character != thirdChar)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsboardValid(List<string> board)
    {
        if(IsNumberOfLineEqualToNumberInFirstLine(board) &&
        IsLineHaveSameLength(board) && 
        IsthereOneLineAndOneSquareMinimum(board) && 
        IsCharAreTheSameThanFirstLine(board))
        {
            return true;
        }
        return false;
    }

    private static bool IsSquareOnlyContainTheValidParameter(List<string> board, int i , int j, int length)
    {
        List<string> parameters = GetParameters(board);
        try
        {
            for (int k = i; k < i + length; k++)
            {
                for (int l = j; l < j + length; l++)
                {
                    if (board[k][l] != parameters[1][0])
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        catch
        {
            return false;
        }
    }

    private static List<Square> GetListOfPossibleSquare(List<string> board, List<Square> squareList, int iIndex, int jIndex)
    {
        List<string> parameters = GetParameters(board);
        int tempLength = 1;

        if (board[iIndex][jIndex] == parameters[1][0])
        {
            tempLength = 1;

            while (IsSquareOnlyContainTheValidParameter(board, iIndex, jIndex, tempLength))
            {
                tempLength++;
            }

            squareList.Add(new Square(iIndex, jIndex, tempLength - 1));
        }

        return squareList;
    }

    private static Square FindTheBiggerSquare(List<string> board)
    {
        List<Square> squareList = new List<Square>();

        for (int i = 1; i < board.Count; i++)
        {
            for(int j = 0; j < board[i].Length; j++)
            {             
                squareList = GetListOfPossibleSquare(board, squareList, i, j);               
            }
        }
        
        Square biggerSquare = squareList.OrderByDescending(x => x.size).First();
        return biggerSquare;
    }

    private static List<string> ReplaceSquare(List<string> board, Square square)
    {
        List<string> newBoard = board;
        List<string> parameters = GetParameters(board);

        for (int i = square.x; i < square.x + square.size; i++)
        {
            for (int j = square.y; j < square.y + square.size; j++)
            {
                newBoard[i] = newBoard[i].Remove(j, 1).Insert(j, parameters[3]);
            }
        }
        return newBoard;
    } 

    private static void DisplayBoard(List<string> board)
    {
        for(int i = 1; i < board.Count; i++)
        {
            System.Console.WriteLine(board[i]);
        }
    }

    private static void Main(string[] args)
    {
        if(CheckIfNArgExists(args, 1))
        {
            try
            {
                List<string> board = File.ReadAllLines(args[0]).ToList();

                if(IsboardValid(board))
                {
                    Square square = FindTheBiggerSquare(board);
                    List<string> newBoard = ReplaceSquare(board, square);
                    DisplayBoard(newBoard);
                }
                else
                {
                    Console.WriteLine("error");
                }
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