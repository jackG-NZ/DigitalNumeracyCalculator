using System.IO.Pipes;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalNumeracyCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            LaunchPad();
        }

        static void LaunchPad()
        {
            TitleScreen();

            DisplayCommands();

            GetUserInput();
        }

        static void TitleScreen()
        {
            /*This method just displays a little title screen.*/

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("====== CLI Calculator v1.0 ======");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome! Press[ENTER] to proceed.");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadLine();

            Console.Clear();
        }

        static void DisplayCommands()
        {
            /*This method displays a list of all commands to the user
             * and an example of their outputs. I decided to read from
             * text files to improve code readability.*/

            bool viewCommands = true;

            string list = "Menu";

            do
            {
                Console.Clear();

                StreamReader sr = new StreamReader(@$"..\..\..\TextFiles\DisplayCommands{list}.txt");

                Console.Write(sr.ReadToEnd());

                string inputString = Console.ReadLine().ToLower();

                char input = 'z';

                if (inputString.Length > 0)
                {
                    input = Convert.ToChar(inputString.Substring(0, 1));
                }

                switch (input)
                {
                    case 'q':
                        list = "Menu";
                        break;
                    case 'a':
                        list = "Basic";
                        break;
                    case 'b':
                        list = "Utility";
                        break;
                    case 'e':
                        viewCommands = false;
                        break;
                }

            } while (viewCommands);

            Console.Clear();
        }

        static void GetUserInput()
        {
            /*This method handles user input. The large switch statement is 
             * for performing the right actions or calling the right methods, and also
             * handling input errors.*/

            string input = "";

            bool isInCalculator = true;

            do
            {
                Console.Write("# ");
                input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "clear":
                        Console.Clear();
                        break;
                    case "exit":
                        isInCalculator = false;
                        break;
                    case "datetime":
                        Console.WriteLine($"@ {DateTime.Now}");
                        break;
                    case "help":
                        Console.Clear();
                        DisplayCommands();
                        break;
                    default:
                        if (input.Length > 0)
                        {
                            if ("-()0123456789".Contains(input[0]))
                            {
                                /*This is really bizarre but it works. If the 0th position of the
                                 * input string contains a number or a parenttheses, the program
                                 * assumes the user wants to do an arithmetic operation.*/

                                if(EvaluatePostfix(ConvertToPostfix(input)) == 1234.4321)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"@ INPUT ERROR.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    Console.WriteLine($"@ {ConvertToPostfix(input)}");
                                    Console.WriteLine($"@ {EvaluatePostfix(ConvertToPostfix(input))}");
                                }
                            }
                            else if ("!".Contains(input[0])) //the "!" prefix is used as an identifier for certain functions handled by the interpreter
                            {
                                Console.WriteLine($"@ {Interpreter(input)}");
                            }
                            else if(input == "ibm")
                            {
                                Console.WriteLine($"@ A COMPUTER\n  CAN NEVER BE HELD ACCOUNTABLE\n  THEREFORE A COMPUTER MUST NEVER\n  MAKE A MANAGEMENT DECISION\n  -IBM 1979");
                            }
                            else
                            {
                                /*Else, an error message is thrown.*/

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"""@ OPERATOR "{input}" IS UNKNOWN.""");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        break;
                }

            } while (isInCalculator);
        }

        public static double Interpreter(string input)
        {
            //I think this needs a pretty significant redesign for modularity in the future

            double result = 0;

            string[] expression = input.Split();

            string operation = expression[0].Substring(1);

            switch (operation)
            {
                case "sqrt":
                    try
                    {
                        result = SquareRoot(Convert.ToDouble(expression[1]));
                    }
                    catch (Exception error)
                    {
                        InputError(error.Message);
                    }
                    break;
                case "pow":
                    try
                    {
                        result = Pow(Convert.ToDouble(expression[1]), Convert.ToDouble(expression[2]));
                    }
                    catch (Exception error)
                    {
                        InputError(error.Message);
                    }
                    break;
                case "fact":
                    try
                    {
                        result = Convert.ToDouble(Factorial(Convert.ToInt32(expression[1])));
                    }
                    catch (Exception error)
                    {
                        InputError(error.Message);
                    }
                    break;
                case "mod":
                    try
                    {
                        result = Modulo(Convert.ToDouble(expression[1]), Convert.ToDouble(expression[2]));
                    }
                    catch (Exception error)
                    {
                        InputError(error.Message);
                    }
                    break;
            }

            return result;
        }

        public static void InputError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"@ {error.ToUpper()}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static double SquareRoot(double expression)
        {
            return Math.Sqrt(expression);
        }

        public static double Pow(double baseNum, double exponent)
        {
            return Math.Pow(baseNum, exponent);
        }

        public static long Factorial(int expression)
        {
            long result = 1; //using a long int here for 64 bits

            for(int i = 1; i <= expression; i++)
            {
                result *= i;
            }

            return result;
        }

        public static double Modulo(double dividend, double divisor)
        {
            return dividend % divisor;
        }

        public static string ConvertToPostfix(string input)
        {
            /*Infix notation to postfix notation using shunting yard algorithm.
             * The most challenging part of writing this method was sorting
             * the operators into the correct precedence.The resources I've
             * linked below were super helpful for understanding this algorithm 
             * conceptually. Thankfully I have experience working with stack
             * data structures in C so the concepts weren't entirely foreign.
             * https://youtu.be/1VjJe1PeExQ
             * https://youtu.be/Wz85Hiwi5MY
             * https://youtu.be/QM_RsQ9Yeio
             * https://en.wikipedia.org/wiki/Shunting_yard_algorithm*/

            /*  ( 1 + 2 ) * 3
             *  ( -> stack
             *  1 -> output
             *  + -> stack
             *  2 -> output
             *  empty until and discard (, leaving 1 2 + on the output
             *  * -> stack
             *  3 -> output
             *  * -> output
             *  .:. output: 1 2 + 3 *
            */

            List<string> output = new List<string>(); //basically stores the converted expression
            Stack<string> stack = new Stack<string>(); //stack is a LIFO data structure so we can use .Pop and .Push

            string[] tokens = input.Split(); //splits each digit/symbol into a unique token

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out _)) //functions like a bool by trying to convert token to an int, then adds it to the output queue if successful
                {
                    output.Add(token);
                }
                else if (token == "(") //opening parenttheses goes on the stack
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(") //empties operators from the stack into the output until the opening parenthesis is found
                    {
                        output.Add(stack.Pop());
                    }

                    stack.Pop(); //opening parentthesis is discarded
                }
                else
                {
                    while (stack.Count > 0 && FindPrecendence(stack.Peek()) >= FindPrecendence(token)) //operator on the stack with higher or equal precedence goes to the output queue first
                    {
                        if (token != "^") //the "^" operator has right associativity so it can't be moved onto the output until the other operators have been
                        {
                            output.Add(stack.Pop());
                        }
                    }
                    stack.Push(token);
                }
            }

            while (stack.Count > 0) //finishes emptying the stack
            {
                output.Add(stack.Pop());
            }

            string result = string.Join(" ", output);

            return result;
        }

        static int FindPrecendence(string operation)
        {
            /*This method returns the precendence of each operator which
             * is very important for converting to postfix. It basically
             * controls the order in which each operator is popped into
             * the output*/

            int precedence = 0;

            switch (operation)
            {
                case "+":
                    precedence = 2;
                    break;
                case "-":
                    precedence = 2;
                    break;
                case "*":
                    precedence = 3;
                    break;
                case "/":
                    precedence = 3;
                    break;
                case "^":
                    precedence = 4;
                    break;
            }

            return precedence;
        }

        public static double EvaluatePostfix(string expression)
        {
            /*This evaluates the expression generated by the ConvertToPostfix method.*/

            Stack<double> stack = new Stack<double>();

            string[] tokens = expression.Split();

            double a = 0, b = 0;

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out _)) //same as in the ConvertToPostfix method, but integers are put onto the stack
                {
                    stack.Push(Convert.ToDouble(token));
                }
                else
                {
                    try
                    {
                        a = stack.Pop();
                        b = stack.Pop();
                    }
                    catch
                    {
                        a = 0;
                        b = 0;
                    }

                    switch (token)
                    {
                        case "+":
                            stack.Push(b + a);
                            break;
                        case "-":
                            stack.Push(b - a);
                            break;
                        case "*":
                            stack.Push(b * a);
                            break;
                        case "/":
                            stack.Push(b / a);
                            break;
                        case "^":
                            stack.Push(Math.Pow(b, a)); //these have to be reversed, my intuition told me a would be the base and b the exponent
                            break;
                    }
                }
            }

            if(stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                return 1234.4321; //1234.4321 serves as an error message here
            }
        }
    }
}

/*AI USAGE
 * I used AI to figure out why my shunting algorithm wasn't ordering "^" operators correctly. It said because "^" operators have right associativity instead of left, unlike all other operators.
 * It told me I could make an extra method to check each token's associativity but I decided this was redundant and instead just added an if statement.*/
