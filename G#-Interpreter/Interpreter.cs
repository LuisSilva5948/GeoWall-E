using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents an interpreter that processes and executes the source code.
    /// </summary>
    public class Interpreter
    {
        public static IUserInterface UI { get; private set; }

        /// <summary>
        /// Executes the interpreter by running the provided source code.
        /// </summary>
        /// <param name="source">The source code to run.</param>
        public static List<object> Run(string source)
        {
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();
                // Check for errors in the lexer
                if (lexer.Errors.Count > 0)
                {
                    return new List<object>() { lexer.Errors };
                }

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
        public static void Execute(string source, IUserInterface userInterface)
        {
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();
                // Check for errors in the lexer
                if (lexer.Errors.Count > 0)
                {
                    foreach (Error error in lexer.Errors)
                    {
                        userInterface.ReportError(error.Report());
                    }
                }

                // Parsing: Build an abstract syntax tree (AST) from the Tokens
                Parser parser = new Parser(tokens);
                List<Expression> AST = parser.Parse();
                // Check for errors in the parser
                if (parser.Errors.Count > 0)
                {
                    foreach (Error error in lexer.Errors)
                    {
                        userInterface.ReportError(error.Report());
                    }
                }

                // Evaluating: Evaluate the expressions in the AST and produce a result
                Evaluator evaluator = new Evaluator();
                //return evaluator.Evaluate(AST);
            }
            catch (Error error)
            {
                userInterface.ReportError(error.Report());
            }
            catch (Exception e)
            {
                userInterface.ReportError(e.Message);
            }
        }
    }
}