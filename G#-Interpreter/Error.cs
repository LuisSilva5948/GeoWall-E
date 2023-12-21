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
    public class Error : Exception
    {
        public string Message { get; private set; }         // The error message
        public ErrorType ErrorType { get; private set; }    // The type of the error
        public int? Line { get; private set; }               // The line number where the error occurred
        public Error(ErrorType errorType, string message): base(message)
        {
            ErrorType = errorType;
            Message = message;
        }
        public Error(ErrorType errorType, string message, int line) : base(message)
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
        COMPILING,  // Compiling errors occur when the interpreter encounters an invalid statement
        RUNTIME     // Runtime errors occur when the evaluator encounters an invalid operation
    }
}