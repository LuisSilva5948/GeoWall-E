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
            // Initialize the StandardLibrary class
            StandardLibrary.Initialize();
        }

        /// <summary>
        /// Executes the interpreter by running the provided source code.
        /// </summary>
        /// <param name="source">The source code to run.</param>
        public List<object> Run(string source)
        {
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                // Parsing: Build an abstract syntax tree (AST) from the Tokens
                Parser parser = new Parser(tokens);
                List<Expression> AST = parser.Parse();

                // Evaluating: Evaluate the expressions in the AST and produce a result
                Evaluator evaluator = new Evaluator();
                return evaluator.Evaluate(AST);
            }
            catch (Error error)
            {
                return new List<object>(){ error.Report() };
            }
        }
    }
}