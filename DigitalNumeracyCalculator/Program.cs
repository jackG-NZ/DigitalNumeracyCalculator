using System.IO.Pipes;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalNumeracyCalculator
{
    public struct Vector
    {
        public string vectorName;
        public double x1;
        public double x2;
        public double y1;
        public double y2;
    }

    public class Program
    {
        public static int vectorCount = 0;

        public static Vector[] vectors = new Vector[8];

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

                if(list == "Menu")
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("========== Operation List ==========");

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(sr.ReadToEnd());
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("\n\n#");
                }
                else
                {
                    Console.Write(sr.ReadToEnd());
                }

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
                    case 'c':
                        list = "Vector";
                        break;
                    case 'd':
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

                                if(EvaluatePostfix(ConvertToPostfix(input)) == 1234.4321) //I'm using 1234.4321 as a make-shift error code
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"@ Input Error.");
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
                                double result = Interpreter(input);

                                if(result != 1234.4321)
                                {
                                    Console.WriteLine($"@ {result:F2}");
                                }
                            }
                            else if ("*".Contains(input[0]))
                            {
                                Dereference(input, true);
                            }
                            else
                            {
                                /*Else, an error message is thrown.*/

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"""@ Operator "{input}" is unknown.""");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        break;
                }

            } while (isInCalculator);
        }

        public static double Interpreter(string input)
        {
            /*I think this needs a pretty significant redesign for modularity in the future
             * but this method is intended to interpret operations with the '!' prefix.*/

            double result = 1234.4321; //make-shift error code. If nothing is returned the interpreter assumes an error has occurred somewhere.

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
                case "line":
                    DefineVector(input);
                    break;
                case "lengthline":
                    result = LengthLine(input);
                    break;
                case "midpointline":
                    Console.WriteLine(MidPointLine(input));
                    break;
                case "ibm": //Easter egg
                    {
                        Console.WriteLine("@ A COMPUTER");
                        Thread.Sleep(1500);
                        Console.WriteLine("@ CAN NEVER BE HELD ACCOUNTABLE");
                        Thread.Sleep(1500);
                        Console.WriteLine("@ THEREFORE A COMPUTER MUST NEVER");
                        Thread.Sleep(1500);
                        Console.WriteLine("@ MAKE A MANAGEMENT DECISION");
                        Thread.Sleep(1500);
                        Console.WriteLine("@ - IBM 1979");
                        Thread.Sleep(1500);
                        break;
                    }
                default:
                    Console.WriteLine("""@ Input "help" to view operations using the '!' prefix.""");
                    break;
            }

            return result;
        }

        public static void InputError(string error)
        {
            /*This is a small method that handles error messages.*/

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"@ {error.ToUpper()}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static double SquareRoot(double expression)
        {
            /*Fairly self explanatory. Returns the square root of an expression.*/

            return Math.Sqrt(expression);
        }

        public static long Factorial(int expression)
        {
            /*Returns the factorial of a number.*/

            long result = 1; //using a long int here for 64 bits

            for(int i = 1; i <= expression; i++)
            {
                result *= i;
            }

            return result;
        }

        public static double Modulo(double dividend, double divisor)
        {
            /*Returns modulo of a dividend and a divisor.*/

            return dividend % divisor;
        }

        public static void DefineVector(string input)
        {
            /*This method handles the creation of custom vectors.*/

            string[] vectorComponents = input.Split();

            string vectorName = vectorComponents[1];

            if(vectorCount <= 7)
            {
                try
                {
                    int x1 = Convert.ToInt32(vectorComponents[2]);
                    int y1 = Convert.ToInt32(vectorComponents[3]);
                    int x2 = Convert.ToInt32(vectorComponents[4]);
                    int y2 = Convert.ToInt32(vectorComponents[5]);

                    vectors[vectorCount].vectorName = vectorName;
                    vectors[vectorCount].x1 = x1;
                    vectors[vectorCount].y1 = y1;
                    vectors[vectorCount].x2 = x2;
                    vectors[vectorCount].y2 = y2;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"""@ "{vectorName}" initialized.""");

                    vectorCount += 1;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"@ Syntax Error.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"@ Maximum of 8 lines are permitted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        static double LengthLine(string input)
        {
            /*Returns the length of a line.*/

            double dx = 0, dy = 0;

            string line = Dereference(input, false); //basically to check that the given string actually exists

            if (line.Length > 0){ //0 0 3 2

                dx = Convert.ToDouble(line.Split()[2]) - Convert.ToDouble(line.Split()[0]);
                dy = Convert.ToDouble(line.Split()[3]) - Convert.ToDouble(line.Split()[1]);
            }

            return SquareRoot(dx * dx + dy * dy);
        }

        static string MidPointLine(string input)
        {
            //Returns the midpoint of a line.*/

            double dx = 0, dy = 0;

            string line = Dereference(input, false);
            string midPoint = "";

            if(line.Length > 0) //-2 1 4 3
            {
                dx = Convert.ToDouble(line.Split()[0]) + Convert.ToDouble(line.Split()[2]);
                dy = Convert.ToDouble(line.Split()[1]) + Convert.ToDouble(line.Split()[3]);
            }

            midPoint = "@ " + Convert.ToString(dx/2) + ", " + Convert.ToString(dy/2);

            return midPoint;
        }

        public static string Dereference(string input, bool printOutput)
        {
            /*This method is intended to function kinda like derefencing a variable in C by just printing out its values.
             * It's also used by other methods to confirm that instances of structs actually exist before trying to perform
             * operations on them. See above, LengthLine and MidPointLine.*/

            string variable = "";

            try
            {
                variable = input.Split()[1];
            }
            catch
            {
                if (printOutput)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"@ Syntax Error.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            for(int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i].vectorName == variable)
                {
                    if (printOutput)
                    {
                        Console.WriteLine($"@ {vectors[i].x1} {vectors[i].y1} {vectors[i].x2} {vectors[i].y2}");
                    }

                    variable = Convert.ToString(vectors[i].x1 + " " + vectors[i].y1 + " " + vectors[i].x2 + " " + vectors[i].y2);
                }
            }

            return variable; 
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

            /*BASICALLY: Numeric literals go straight onto the output, operators go onto the stack providing
             * they are a higher precedence that what's already on the top of the stack. Assume we have 1 2 4 on
             * the output and + * on the stack, if the next token is an operator with lesser precedence than * and +
             * for example -, the stack is flushed into the output (LIFO) and the - is moved onto the stack, becoming
             * its sole token. The stack is flushed completely when all tokens have been moved into their correct
             * place, leaving the stack empty and the translated expression in the output.*/

            string result = "";

            bool validExpression = true;

            List<string> output = new List<string>(); //basically stores the converted expression
            Stack<string> stack = new Stack<string>(); //stack is a LIFO data structure so we can use .Pop and .Push

            string[] tokens = input.Split(); //splits each digit/symbol into a unique token

            for(int i = 0; i < tokens.Length; i++) //divide by zero protection (the program won't crash without it but I thought )
            {
                if (tokens[i] == "/")
                {
                    if (tokens[i + 1] == "0")
                    {
                        validExpression = false;
                    }
                }
            }

            if(validExpression)
            {
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

                result = string.Join(" ", output);
            }

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
