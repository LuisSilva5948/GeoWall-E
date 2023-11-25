using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    /// <summary>
    /// Represents an interpreter that processes and executes the source code.
    /// </summary>
    public class Interpreter
    {
        public Interpreter()
        {
            // Initialize the Memory class
            Memory.Initialize();
        }

        /// <summary>
        /// Executes the interpreter by running the provided source code.
        /// </summary>
        /// <param name="source">The source code to run.</param>
        public void Run(string source)
        {
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                // Parsing: Build an abstract syntax tree (AST) from the Tokens
                Parser parser = new Parser(tokens);
                Expression AST = parser.Parse();

                // Evaluating: Evaluate the expressions in the AST and produce a result
                Evaluator evaluator = new Evaluator();
                object result = evaluator.Evaluate(AST);

                Console.WriteLine(result);
            }
            catch (Error error)
            {
                // Print the error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error.Report());
                Console.ForegroundColor = ConsoleColor.Green;
                return;
            }
        }
    }
}