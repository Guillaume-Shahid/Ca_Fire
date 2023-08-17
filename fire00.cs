//Créez un programme qui affiche un rectangle dans le terminal.

internal class Program
{

    public static bool CheckIfNArgsExists(string[] args, int NumberOfArgs)
    {
        return args.Length == NumberOfArgs;
    }

    private static void DisplayRectangle(int length, int width)
    {
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= length; j++)
            {
                if ((i == 1 && j == 1) ||
                    (i == 1 && j == length) ||
                    (i == width && j == 1) ||
                    (i == width && j == length))
                {
                    Console.Write("o");
                }
                else if (i == 1 || i == width)
                {
                    Console.Write("-");
                }
                else if (j == 1 || j == length)
                {
                    Console.Write("|");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    private static void Main(string[] args)
    {
        try
        {
            int length = int.Parse(args[0]);
            int width = int.Parse(args[1]);

            if(CheckIfNArgsExists(args, 2))
            {
                DisplayRectangle(length, width);
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
}