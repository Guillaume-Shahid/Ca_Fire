//Créez un programme qui affiche la position de l’élément le plus en haut à gauche (dans l’ordre) d’une forme au sein d’un plateau.
using System.Collections.Generic;

internal class Program
{
    private static bool CheckIfNArgExists(string[] args, int nArgrs)
    {
        return ((args.Length == (nArgrs)));
    }

    private static string ReplaceAt(string input, int i, char newChar)
    {
        char[] chars = input.ToCharArray();
        chars[i] = newChar;
        return new string(chars);
    }

    private static bool CheckIfToFindPatternCorrespondInBoard(List<string> board, List<string> toFind, int i, int j)
    {
        for(int k = 0; k < toFind.Count; k++)
        {
            for(int l = 0; l < toFind[k].Length; l++)
            {
                try
                {
                    if (board[i + k][j + l] != toFind[k][l] && toFind[k][l] != ' ')
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
               
            }
        }
        return true;
    }

    private static List<string> FillFoundWithCorrespondanceBetweenBoardAndToFind(List<string> board, List<string> toFind, List<string> found, int i, int j)
    {
        if(CheckIfToFindPatternCorrespondInBoard(board, toFind, i, j))
        {
            for(int k = 0; k < toFind.Count; k++)
            {
                for(int l = 0; l < toFind[k].Length; l++)
                {
                    found[i + k] = ReplaceAt(found[i + k], j + l, toFind[k][l]);
                }
            }
        }
        return found;
    }

    private static List<string> FillFoundList(List<string> board)
    {
        List<string> found = new List<string>();
        string line;
        int i = 0;
        int j = 0;

        for (i = 0; i < board.Count; i++)
        {
            line = "";
            for (j = 0; j < board[i].Length; j++)
            {
                line += "-";
            }
            found.Add(line);
        }

        return (found);
    }

    private static List<string> IsThereToFindInBoard(List<string> board, List<string> toFind)
    {
        List<string> found = FillFoundList(board);

        for(int i = 0; i < board.Count; i++)
        {
            if(board[i].Contains(toFind[0][0]))
            {
                for(int j = 0; j < board[i].Length; j++)
                {
                   found = FillFoundWithCorrespondanceBetweenBoardAndToFind(board, toFind, found, i, j);
                }
            }
        }
        return found;
    }

    private static string GetCoordinate(List<string> board)
    {
        for(int i = 0; i < board.Count; i++)
        {
            for(int j = 0; j < board[i].Length; j++)
            {
                if(board[i][j] != '-')
                {
                    return ($"Coordonnées : {j}, {i}");
                }
            }
        }
        return ("error");
    }

    private static bool IsPatternToFindIsFindable(List<string> found)
    {
        foreach (var line in found)
        {
            foreach (var character in line)
            {
                if (character != '-')
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static void DisplayBoard(List<string> board)
    {
        if(IsPatternToFindIsFindable(board))
        {
            Console.WriteLine("Trouvé!");
            Console.WriteLine(GetCoordinate(board));
            foreach(var line in board)
            {
                Console.WriteLine(line);
            }
        }
        else
        {
            Console.WriteLine("Introuvable!");
        } 
    }

    static void Main(string[] args)
    {
        if(CheckIfNArgExists(args, 2))
        {
            List<string> board = new List<string>();
            List<string> toFind = new List<string>();
            List<string> found = new List<string>();            

            try
            {
                File.ReadAllLines(args[0]).ToList().ForEach(line => board.Add(line));
                File.ReadAllLines(args[1]).ToList().ForEach(line => toFind.Add(line));
                found = IsThereToFindInBoard(board, toFind);

                DisplayBoard(found);
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