using System;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalNumeracyCalculator
{
    public struct Line
    {
        public string lineName;
        public double x1;
        public double x2;
        public double y1;
        public double y2;
    }

    public struct Vector
    {
        public string vectorName;
        public double x;
        public double y;
    }

    public struct Matrix
    {
        public string matrixName;
        public int a;
        public int b;
        public int c;
        public int d;
    }

    public class Program
    {
        /*These ints are used to keep track of which instance of a struct
         * is the next to be initialized, and to prevent more than 8 of each
         * from being created (hopefully 8 will be enough)*/

        public static int lineCount = 0, vectorCount = 0, matrixCount = 0;

        public static Line[] lines = new Line[8];

        public static Vector[] vectors = new Vector[8];

        public static Matrix[] matrices = new Matrix[8];

        static void Main(string[] args)
        {
            LaunchPad();
        }

        static void LaunchPad()
        {
            DisplayCommands();

            GetUserInput();
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
                    DisplayMainMenu(sr);

                    Console.Write("\n\n#");
                }
                else
                {
                    DisplaySubMenu(sr);

                    Console.Write("\n\n# ");
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
                        list = "Geometry";
                        break;
                    case 'd':
                        list = "Vector";
                        break;
                    case 'e':
                        list = "Matrix";
                        break;
                    case 'f':
                        viewCommands = false;
                        break;
                }

            } while (viewCommands);

            Console.Clear();
        }

        static void GetUserInput()
        {
            /*This method handles user input. The huge ugly switch statement is 
             * for performing the right actions or calling the right methods, and also
             * handling input errors. I'm not a big fan of having every method call
             * inside a try/except but I think it's the cleanest way to do input
             * protection.*/

            string input = "";

            bool isInCalculator = true;

            do
            {
                Console.Write("# ");
                input = Console.ReadLine().ToLower();

                switch (input.Split()[0])
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
                    case "*":
                        try
                        {
                            Dereference(input, "deref", true);
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "sqrt":
                        try
                        {
                            Console.WriteLine($"@ {SquareRoot(Convert.ToDouble(input.Split()[1]))}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "fact":
                        try
                        {
                            Console.WriteLine($"@ {Convert.ToDouble(Factorial(Convert.ToInt32(input.Split()[1])))}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "mod":
                        try
                        {
                            Console.WriteLine($"@ {Modulo(Convert.ToDouble(input.Split()[1]), Convert.ToDouble(input.Split()[2]))}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "line":
                        try
                        {
                            DefineLine(input);
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "vec":
                        try
                        {
                            DefineVector(input);
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "lengthline":
                        try
                        {
                            Console.WriteLine($"@ {LengthLine(input):F2}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "midpointline":
                        try
                        {
                            Console.WriteLine($"@ {MidPointLine(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "gradientline":
                        try
                        {
                            Console.WriteLine($"@ {GradientLine(input):F2}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "rad2deg":
                        try
                        {
                            Console.WriteLine($"@ {Rad2Deg(input):F2}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "deg2rad":
                        try
                        {
                            Console.WriteLine($"@ {Deg2Rad(input):F2}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "addvec":
                        try
                        {
                            Console.WriteLine($"@ {AddVec(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "subvec":
                        try
                        {
                            Console.WriteLine($"@ {SubVec(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "dotvec":
                        try
                        {
                            Console.WriteLine($"@ {DotVec(input)}");

                            if(Convert.ToInt32(DotVec(input)) > 0)
                            {
                                Console.WriteLine("@ These vectors are pointing in the same direction (acute).");
                            }
                            else if (Convert.ToInt32(DotVec(input)) == 0)
                            {
                                Console.WriteLine("@ These vectors are perpendicular to each other.");
                            }
                            else
                            {
                                Console.WriteLine("@ These vectors are pointing in opposite directions (obtuse).");
                            }

                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "scalvec":
                        try
                        {
                            Console.WriteLine($"@ {ScalVec(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "mat":
                        try
                        {
                            DefineMatrix(input);
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "addmat":
                        try
                        {
                            Console.WriteLine($"@ {AddMat(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
                        break;
                    case "dotmat":
                        try
                        {
                            Console.WriteLine($"@ {DotMat(input)}");
                        }
                        catch (Exception error)
                        {
                            InputError(error.Message);
                        }
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
                        if(input.Length > 0)
                        {
                            if ("0123456789(".Contains(input.Substring(0, 1)))
                            {
                                if (EvaluatePostfix(ConvertToPostfix(input)) != 1234.4321)
                                {
                                    Console.WriteLine($"@ {ConvertToPostfix(input)}");
                                    Console.WriteLine($"@ {EvaluatePostfix(ConvertToPostfix(input))}");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"""@ Syntax error or bad expression. Please leave whitespaces between tokens.""");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"""@ "{input}" is unknown. Enter "help" to see a list of operations.""");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        break;
                }

            } while (isInCalculator);
        }

        public static void InputError(string error)
        {
            /*This is a small method that handles error messages*/

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"""@ Syntax error or bad expression. Input "more" for more information.""");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("# ");
            string more = Console.ReadLine().ToLower();

            if(more == "more")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"""@ Exception error: {error}"""); //usually the error message is pretty vague but thought I'd include it anyway
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static double SquareRoot(double expression)
        {
            /*Fairly self explanatory. Returns the square root of an expression*/

            return Math.Sqrt(expression);
        }

        public static long Factorial(int expression)
        {
            /*Returns the factorial of a number*/

            long result = 1; //using a long int here for 64 bits

            for(int i = 1; i <= expression; i++)
            {
                result *= i;
            }

            return result;
        }

        public static double Modulo(double dividend, double divisor)
        {
            /*Returns modulo of a dividend and a divisor*/

            return dividend % divisor;
        }

        public static void DefineLine(string input)
        {
            /*This method handles the creation of custom lines*/

            string[] lineComponents = input.Split(); //I love the .split method it has been extremely useful for this assignment

            string lineName = lineComponents[1];

            if(lineCount < 8)
            {
                int x1 = Convert.ToInt32(lineComponents[2]);
                int y1 = Convert.ToInt32(lineComponents[3]);
                int x2 = Convert.ToInt32(lineComponents[4]);
                int y2 = Convert.ToInt32(lineComponents[5]);

                lines[lineCount].lineName = lineName;
                lines[lineCount].x1 = x1;
                lines[lineCount].y1 = y1;
                lines[lineCount].x2 = x2;
                lines[lineCount].y2 = y2;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"""@ "{lineName}" initialized.""");

                lineCount += 1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"@ Maximum of 8 lines are permitted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DefineVector(string input)
        {
            /*This method handles the creation of custom vectors*/

            string[] vectorComponents = input.Split();

            string vectorName = vectorComponents[1];

            if(vectorCount < 8)
            {
                int x = Convert.ToInt32(vectorComponents[2]);
                int y = Convert.ToInt32(vectorComponents[3]);

                vectors[vectorCount].vectorName = vectorName;
                vectors[vectorCount].x = x;
                vectors[vectorCount].y = y;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"""@ "{vectorName}" initialized.""");

                vectorCount += 1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"@ Maximum of 8 vectors are permitted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DefineMatrix(string input)
        {
            /*This method handles the creation of custom vectors. Very similar to the
             * DefineVector method*/

            string[] matrixComponents = input.Split();

            string matrixName = matrixComponents[1];

            if(matrixCount < 8)
            {
                int a = Convert.ToInt32(matrixComponents[2]);
                int b = Convert.ToInt32(matrixComponents[3]);
                int c = Convert.ToInt32(matrixComponents[4]);
                int d = Convert.ToInt32(matrixComponents[5]);

                matrices[matrixCount].matrixName = matrixName;
                matrices[matrixCount].a = a;
                matrices[matrixCount].b = b;
                matrices[matrixCount].c = c;
                matrices[matrixCount].d = d;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"""@ "{matrixName}" initialized.""");

                matrixCount += 1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"@ Maximum of 8 matrices are permitted.");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string Dereference(string input, string type, bool printOutput)
        {
            /*This method is intended to function kinda like derefencing a variable in C by just printing out its values.
             * It's also used by other methods to confirm that instances of a struct actually exist before trying to perform
             * an operation on it. See above, LengthLine and MidPointLine etc.*/

            string variable = "";

            if(type == "line" || type == "deref")
            {
                variable = input.Split()[1];
            }

            if(type == "vector")
            {
                variable = input;
            }

            if (type == "matrix")
            {
                variable = input;
            }


            for (int i = 0; i < 8; i++) //this is probably really horrible and inefficient because it has to look through every struct for a match
            {
                if (lines[i].lineName == variable)
                {
                    if (printOutput)
                    {
                        Console.WriteLine($"@ {lines[i].x1} {lines[i].y1} {lines[i].x2} {lines[i].y2}");
                    }

                    variable = Convert.ToString(lines[i].x1 + " " + lines[i].y1 + " " + lines[i].x2 + " " + lines[i].y2);
                }

                if (vectors[i].vectorName == variable)
                {
                    if (printOutput)
                    {
                        Console.WriteLine($"@ {vectors[i].x} {vectors[i].y}");
                    }

                    variable = Convert.ToString(vectors[i].x + " " + vectors[i].y);
                }

                if (matrices[i].matrixName == variable)
                {
                    if (printOutput)
                    {
                        Console.WriteLine($"@ {matrices[i].a} {matrices[i].b} {matrices[i].c} {matrices[i].d}");
                    }

                    variable = Convert.ToString(matrices[i].a + " " + matrices[i].b + " " + matrices[i].c + " " + matrices[i].d);
                }
            }

            return variable;
        }

        static double LengthLine(string input)
        {
            /*Returns the length of a line*/

            double dx = 0, dy = 0;

            string line = Dereference(input, "line", false); //basically to check that the given line actually exists

            if (line.Length > 0){ //0 0 3 2

                dx = Convert.ToDouble(line.Split()[2]) - Convert.ToDouble(line.Split()[0]);
                dy = Convert.ToDouble(line.Split()[3]) - Convert.ToDouble(line.Split()[1]);
            }

            return SquareRoot(dx * dx + dy * dy);
        }

        static string MidPointLine(string input)
        {
            /*Returns the midpoint of a line*/

            double dx = 0, dy = 0;

            string line = Dereference(input, "line", false);
            string midPoint = "";

            if(line.Length > 0) //-2 1 4 3
            {
                dx = Convert.ToDouble(line.Split()[0]) + Convert.ToDouble(line.Split()[2]);
                dy = Convert.ToDouble(line.Split()[1]) + Convert.ToDouble(line.Split()[3]);
            }

            midPoint = Convert.ToString(dx/2) + ", " + Convert.ToString(dy/2);

            return midPoint;
        }

        static double GradientLine(string input)
        {
            /*Returns the gradient of a line*/

            double dx = 0, dy = 0;

            string line = Dereference(input, "line", false);
            double gradient = 0;

            if (line.Length > 0) //
            {
                dx = Convert.ToDouble(line.Split()[2]) - Convert.ToDouble(line.Split()[0]);
                dy = Convert.ToDouble(line.Split()[3]) - Convert.ToDouble(line.Split()[1]);
            }

            gradient = dy / dx;

            return gradient;
        }

        public static double Rad2Deg(string input)
        {
            /*Returns degrees converted from radians*/

            double radians = Convert.ToDouble(input.Split()[1]);

            double degrees = radians * 180 / Math.PI;

            return degrees;
        }

        public static double Deg2Rad(string input)
        {
            /*Returns radians converted from degrees*/

            double degrees = Convert.ToDouble(input.Split()[1]);

            double radians = degrees * Math.PI / 180;

            return radians;
        }

        public static string AddVec(string input)
        {
            /*This method adds two vectors together*/

            double sumX = Convert.ToDouble(SplitIntoVectorComponents(input)[0]) + Convert.ToDouble(SplitIntoVectorComponents(input)[2]);
            double sumY = Convert.ToDouble(SplitIntoVectorComponents(input)[1]) + Convert.ToDouble(SplitIntoVectorComponents(input)[3]);

            string sumVectors = Convert.ToString(sumX) + ", " + Convert.ToString(sumY);

            return sumVectors;
        }

        public static string SubVec(string input)
        {
            /*This method subtracts two vectors*/

            double sumX = Convert.ToDouble(SplitIntoVectorComponents(input)[0]) - Convert.ToDouble(SplitIntoVectorComponents(input)[2]);
            double sumY = Convert.ToDouble(SplitIntoVectorComponents(input)[1]) - Convert.ToDouble(SplitIntoVectorComponents(input)[3]);

            string sumVectors = Convert.ToString(sumX) + ", " + Convert.ToString(sumY);

            return sumVectors;
        }

        public static string DotVec(string input)
        {
            /*This method returns the dot product of two vectors*/

            double multiplyA = Convert.ToDouble(SplitIntoVectorComponents(input)[0]) * Convert.ToDouble(SplitIntoVectorComponents(input)[2]);
            double multiplyB = Convert.ToDouble(SplitIntoVectorComponents(input)[1]) * Convert.ToDouble(SplitIntoVectorComponents(input)[3]);

            string dotProduct = Convert.ToString(multiplyA + multiplyB);

            return dotProduct;
        }

        public static string ScalVec(string input)
        {
            /*This method returns a vector multiplied by a scalar*/

            int scalar = Convert.ToInt32(input.Split()[1]);

            string vectorOne = Dereference(input.Split()[2], "vector", false);

            int x = Convert.ToInt32(vectorOne.Split()[0]) * scalar;
            int y = Convert.ToInt32(vectorOne.Split()[1]) * scalar;

            return Convert.ToString(x) + ", " + Convert.ToString(y);
        }

        public static string[] SplitIntoVectorComponents(string input)
        {
            /*This method splits a string into vector components and confirms its existence by calling the dereference method
             * it then returns a string array that the AddVec and SubVec methods use to perform their calculations*/

            string vectorOne = Dereference(input.Split()[1], "vector", false);
            string vectorTwo = Dereference(input.Split()[2], "vector", false);

            string x1 = vectorOne.Split()[0];
            string y1 = vectorOne.Split()[1];
            string x2 = vectorTwo.Split()[0];
            string y2 = vectorTwo.Split()[1];

            return new string[] { x1, y1, x2, y2 };
        }

        public static string AddMat(string input)
        {
            /*This method returns the addition of two 2x2 matrices*/

            string matrixOne = Dereference(input.Split()[1], "matrix", false);
            string matrixTwo = Dereference(input.Split()[2], "matrix", false);

            string sumA = Convert.ToString(Convert.ToInt32(matrixOne.Split()[0]) + Convert.ToInt32(matrixTwo.Split()[0]));
            string sumB = Convert.ToString(Convert.ToInt32(matrixOne.Split()[1]) + Convert.ToInt32(matrixTwo.Split()[1]));
            string sumC = Convert.ToString(Convert.ToInt32(matrixOne.Split()[2]) + Convert.ToInt32(matrixTwo.Split()[2]));
            string sumD = Convert.ToString(Convert.ToInt32(matrixOne.Split()[3]) + Convert.ToInt32(matrixTwo.Split()[3]));

            return sumA + " " + sumB + " " + sumC + " " + sumD;
        }

        public static string DotMat(string input)
        {
            /*This method returns the dot product of two matrices (matrix multiplication)*/
                                                                               //0  1  2  3
            string matrixOne = Dereference(input.Split()[1], "matrix", false); //A1 B2 C3 D4
            string matrixTwo = Dereference(input.Split()[2], "matrix", false); //W5 X6 Y7 Z8

            string dotAW = Convert.ToString(Convert.ToInt32(matrixOne.Split()[0]) * Convert.ToInt32(matrixTwo.Split()[0])); //this is asinine but it works
            string dotBY = Convert.ToString(Convert.ToInt32(matrixOne.Split()[1]) * Convert.ToInt32(matrixTwo.Split()[2]));
            string dotAX = Convert.ToString(Convert.ToInt32(matrixOne.Split()[0]) * Convert.ToInt32(matrixTwo.Split()[1]));
            string dotBZ = Convert.ToString(Convert.ToInt32(matrixOne.Split()[1]) * Convert.ToInt32(matrixTwo.Split()[3]));
            string dotCW = Convert.ToString(Convert.ToInt32(matrixOne.Split()[2]) * Convert.ToInt32(matrixTwo.Split()[0]));
            string dotDY = Convert.ToString(Convert.ToInt32(matrixOne.Split()[3]) * Convert.ToInt32(matrixTwo.Split()[2]));
            string dotCX = Convert.ToString(Convert.ToInt32(matrixOne.Split()[2]) * Convert.ToInt32(matrixTwo.Split()[1]));
            string dotDZ = Convert.ToString(Convert.ToInt32(matrixOne.Split()[3]) * Convert.ToInt32(matrixTwo.Split()[3]));

            string sumAWBY = Convert.ToString(Convert.ToInt32(dotAW) + Convert.ToInt32(dotBY));
            string sumAXBZ = Convert.ToString(Convert.ToInt32(dotAX) + Convert.ToInt32(dotBZ));
            string sumCWDY = Convert.ToString(Convert.ToInt32(dotCW) + Convert.ToInt32(dotDY));
            string sumCXDZ = Convert.ToString(Convert.ToInt32(dotCX) + Convert.ToInt32(dotDZ));

            return sumAWBY + " " + sumAXBZ + " " + sumCWDY + " " + sumCXDZ;
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
             * https://en.wikipedia.org/wiki/Shunting_yard_algorithm
             * https://youtu.be/unh6aK8WMwM <- this video was particularly good.*/

            /*BASICALLY: Numeric literals go straight onto the output, operators go onto the stack providing
             * they are a higher precedence that what's already on the top of the stack. Assume we have 1 2 4 on
             * the output and + * on the stack, if the next token is an operator with lesser precedence than * and +
             * for example -, the stack is flushed into the output (LIFO) and the - is moved onto the stack, becoming
             * its sole token. The stack is flushed completely when all tokens have been moved into their correct
             * place, leaving the stack empty and the translated expression in the output. If tokens have the same
             * precedence, the stack is flushed if the incoming token has left associativity (for example * and /)
             * have the same precedence but / is left associative, so the stack would be flushed. ^ is the rare
             * exception in that it is right associative.*/

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

        static void DisplayMainMenu(StreamReader sr)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("============ CLI CALCULATOR ============");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(sr.ReadToEnd());
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n======= Input your choice below. =======");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void DisplaySubMenu(StreamReader sr)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"================================= OPERATIONS =================================");
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(sr.ReadToEnd());
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n============================ Input 'q' to return. ============================");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

/*AI USAGE
 * I used AI to figure out why my shunting algorithm wasn't ordering "^" operators correctly. It said because "^" operators have right associativity instead of left, unlike all other operators.
 * It told me I could make an extra method to check each token's associativity but I decided this was redundant and instead just added an if statement.*/
