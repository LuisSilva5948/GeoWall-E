using GSharpInterpreter;
using static System.Net.Mime.MediaTypeNames;

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
            IUserInterface userInterface = new ConsoleUI();
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
                    /*var results = Interpreter.Run(line);
                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }*/
                    Interpreter.Execute(line, userInterface);
                }
                else
                {
                    Console.WriteLine("No input received.");
                }
            }
        }
        public class ConsoleUI : IUserInterface
        {
            public int CanvasWidth => 100;

            public int CanvasHeight => 100;

            public void DrawArc(Arc arc, GSharpColor color)
            {
                Console.WriteLine($"arc drawed");
            }

            public void DrawCircle(Circle circle, GSharpColor color)
            {
                Console.WriteLine($"circle drawed");
            }

            public void DrawLine(Line line, GSharpColor color)
            {
                Console.WriteLine($"line drawed");
            }

            public void DrawPoint(GSharpInterpreter.Point point, GSharpColor color)
            {
                Console.WriteLine($"point drawed");
            }

            public void DrawRay(Ray ray, GSharpColor color)
            {
                Console.WriteLine($"ray drawed");
            }

            public void DrawSegment(Segment segment, GSharpColor color)
            {
                Console.WriteLine($"segment drawed");
            }

            public void DrawText(string text, GSharpInterpreter.Point point, GSharpColor color)
            {
                Console.WriteLine($"{text} dibujado");
            }

            public void Print(string text)
            {
                Console.WriteLine(text);
            }

            public void ReportError(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
}
