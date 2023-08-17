//Créez un programme qui reçoit une expression arithmétique dans une chaîne de caractères et en retourne le résultat après l’avoir calculé.

using System;
using System.Collections.Generic;

internal class Program
{
    private static bool CheckIfNArgExists(string[] args, int nArgrs)
    {
        return ((args.Length == (nArgrs)));
    }

    private static List<string> SplitedArg(string arg)
    {
        List<string> splitedArg = new List<string>();

       foreach (var item in arg.Split(' '))
        {
            splitedArg.Add(item);
        }

        return splitedArg;
    }

    private static string GetvaluesBetweenParenthesis(string arg)
    {
        int posFrom = arg.IndexOf('(');
        if (posFrom != -1) 
        {
            int posTo = arg.IndexOf(')', posFrom + 1);
            if (posTo != -1)
            {
                string expressionBetweenParenthesis = arg.Substring(posFrom + 1, posTo - posFrom - 1);
                string expressionToReplace = arg.Substring(posFrom, posTo - posFrom + 1);
                int newExpression = AssessExpresion(expressionBetweenParenthesis);
                return arg.Replace(expressionToReplace, newExpression.ToString());
            }
        }

        return string.Empty;
    }

    private static bool CheckIfThereAreParenthesis(string arg)
    {
        return (arg.Contains('(') && arg.Contains(')'));
    }

    private static string CalculateValueWhileParenthesisExists(string arg)
    {
        while(CheckIfThereAreParenthesis(arg))
        {
            arg = GetvaluesBetweenParenthesis(arg);
        }

        return arg;
    }

   private static List<string> AssessExpresionWithMultiplicationOrDivision(List<string> args)
    {
        if(args.Contains("*") || args.Contains("/"))
        {
            for (int i = 0; i < args.Count; i++)
            {
                if(args[i] == "*" || args[i] == "/" || args[i] == "%")
                {
                    int leftValue = int.Parse(args[i - 1]);;
                    int rightValue = int.Parse(args[i + 1]);
                    int resultOperation = 0;
                    string operation = args[i];
                    
                    if(operation == "*")
                    {
                        resultOperation = leftValue * rightValue;
                    }
                    else if(operation == "/")
                    {
                        resultOperation = leftValue / rightValue;
                    }
                    else if(operation == "%")
                    {
                        resultOperation = leftValue % rightValue;
                    }

                    args[i - 1] = resultOperation.ToString();
                    args.RemoveAt(i);
                    args.RemoveAt(i);
                    i--;
                }
            }
        }
        return args;
    }

       private static List<string> AssessExpresionWithAdditionOrSubstraction(List<string> args)
    {
        if(args.Contains("+") || args.Contains("-"))
        {
            for (int i = 0; i < args.Count; i++)
            {
                if(args[i] == "+" || args[i] == "-")
                {
                    int leftValue = int.Parse(args[i - 1]);;
                    int rightValue = int.Parse(args[i + 1]);
                    int resultOperation = 0;
                    string operation = args[i];

                    if(operation == "+")
                    {
                        resultOperation = leftValue + rightValue;
                    }
                    else if(operation == "-")
                    {
                        resultOperation = leftValue - rightValue;
                    }

                    args[i - 1] = resultOperation.ToString();
                    args.RemoveAt(i);
                    args.RemoveAt(i);
                    i--;
                }
            }
        }
        return args;
    }

    private static int AssessExpresion(string expression)
    {
        List<string> arg = SplitedArg(expression);
        arg = AssessExpresionWithMultiplicationOrDivision(arg);
        arg = AssessExpresionWithAdditionOrSubstraction(arg);

        return int.Parse(arg[0]);
    }

    private static void Main(string[] args)
    {
        if(CheckIfNArgExists(args, 1))
        {
            try
            {
                int result = 0;
                string expression = args[0];

                expression = CalculateValueWhileParenthesisExists(expression);
                result = AssessExpresion(expression);

                Console.WriteLine(result);
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