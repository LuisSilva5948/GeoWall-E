using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents an error that occurs during the execution of the interpreter.
    /// </summary>
    public class GSharpError : Exception
    {
        public string Message { get; private set; }         // The error message
        public ErrorType ErrorType { get; private set; }    // The type of the error
        public int? Line { get; private set; }              // The line number where the error occurred
        public GSharpError(ErrorType errorType, string message): base(message)
        {
            ErrorType = errorType;
            Message = message;
        }
        public GSharpError(ErrorType errorType, string message, int line) : base(message)
        {
            ErrorType = errorType;
            Message = message;
            Line = line;
        }
        /// <summary>
        /// Returns a string representation of the error.
        /// </summary>
        public string Report()
        {
            if (Line != null)
                return $"! {ErrorType} ERROR at line {Line}: {Message}";
            return $"! {ErrorType} ERROR: {Message}";
        }
    }
    /// <summary>
    /// Enumerates the types of errors that can occur during the execution of the interpreter.
    /// </summary>
    public enum ErrorType
    {
        LEXICAL,    // Lexical errors occur when the lexer encounters an invalid character
        SYNTAX,     // Syntax errors occur when the parser encounters an invalid expression
        SEMANTIC,   // Semantic errors occur when the evaluator encounters an invalid expression
        RUNTIME     // Runtime errors occur when the evaluator encounters an invalid operation
    }
}