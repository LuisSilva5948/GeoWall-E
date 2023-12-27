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
        /*public static List<object> Run(string source)
        {
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();
                // Check for errors in the lexer
                if (lexer.Errors.Count > 0)
                {
                    List<string> errors = new List<string>();
                    foreach (Error error in lexer.Errors)
                    {
                        errors.Add(error.Report());
                    }
                    return new List<object>() { errors };
                }

                // Parsing: Build an abstract syntax tree (AST) from the Tokens
                Parser parser = new Parser(tokens);
                List<Expression> AST = parser.Parse();
                if (parser.Errors.Count > 0)
                {
                    List<string> errors = new List<string>();
                    foreach (Error error in parser.Errors)
                    {
                        errors.Add(error.Report());
                    }
                    return new List<object>() { errors };
                }

                // Evaluating: Evaluate the expressions in the AST and produce a result
                Evaluator evaluator = new Evaluator();
                //return evaluator.Evaluate(AST);
            }
            catch (Error error)
            {
                return new List<object>(){ error.Report() };
            }
        }*/
        public static void Execute(string source, IUserInterface userInterface)
        {
            StandardLibrary.Reset();
            UI = userInterface;
            try
            {
                // Lexing: Convert the source code into a sequence of Tokens
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();
                // Check for errors in the lexer
                if (lexer.Errors.Count > 0)
                {
                    foreach (GSharpError error in lexer.Errors)
                    {
                        userInterface.ReportError(error.Report());
                    }
                    return;
                }

                // Parsing: Build an abstract syntax tree (AST) from the Tokens
                Parser parser = new Parser(tokens);
                List<Expression> AST = parser.Parse();
                // Check for errors in the parser
                if (parser.Errors.Count > 0)
                {
                    foreach (GSharpError error in parser.Errors)
                    {
                        userInterface.ReportError(error.Report());
                    }
                    return;
                }

                // Evaluating: Evaluate the expressions in the AST and produce a result
                Evaluator evaluator = new Evaluator();
                evaluator.Evaluate(AST);
                if (evaluator.Errors.Count > 0)
                {
                    foreach (GSharpError error in evaluator.Errors)
                    {
                        userInterface.ReportError(error.Report());
                    }
                    return;
                }
            }
            catch (GSharpError error)
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