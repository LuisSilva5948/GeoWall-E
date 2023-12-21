using GSharpInterpreter;

namespace G__ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("G# Interpreter Tester.");
            Console.WriteLine();
            Console.WriteLine("Type a command and press Enter to execute it.");
            Console.WriteLine("Type 'exit' to finish the program.");
            Console.WriteLine();
            while (true)
            {
                Console.Write(">");
                string line = Console.ReadLine();
                if (line == "exit")
                {
                    break;
                }
                else
                if (line != null && line != "")
                {
                    var results = Interpreter.Run(line);
                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }
                }
                else
                {
                    Console.WriteLine("No input received.");
                }
            }
        }
    }
}
