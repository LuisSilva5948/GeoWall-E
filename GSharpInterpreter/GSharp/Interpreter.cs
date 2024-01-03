using System;
using System.Collections.Generic;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents an interpreter that processes and executes the source code.
    /// </summary>
    public class Interpreter
    {
        public static IUserInterface UI { get; private set; }

        /// <summary>
        /// Executes the given source code.
        /// </summary>
        /// <param name="source"> Code to be executed. </param>
        /// <param name="userInterface"> Reference to the user interface that will be used. </param>
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