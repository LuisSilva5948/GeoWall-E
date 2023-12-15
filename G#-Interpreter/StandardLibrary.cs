using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    /// <summary>
    /// Represents the standar library of the G# language.
    /// </summary>
    public static class StandardLibrary
    {
        public static Dictionary<string, FunctionDeclaration> DeclaredFunctions { get; private set; }   // The dictionary of declared functions
        public static Dictionary<string, object> GlobalVariables { get; private set; }                  // The dictionary of global variables
        /// <summary>
        /// Initializes a dictionary of declared functions and adds the predefined functions to it.
        /// </summary>
        public static void Initialize()
        {
            DeclaredFunctions = new Dictionary<string, FunctionDeclaration>();
            DeclaredFunctions["sqrt"] = null;
            DeclaredFunctions["sin"] = null;
            DeclaredFunctions["cos"] = null;
            DeclaredFunctions["log"] = null;
            DeclaredFunctions["exp"] = null;
            GlobalVariables = new Dictionary<string, object>();
        }
        /// <summary>
        /// Adds a function declaration to the dictionary of declared functions.
        /// </summary>
        /// <param name="function">The function declaration to add.</param>
        public static void AddFunction(FunctionDeclaration function)
        {
            DeclaredFunctions[function.Identifier] = function;
        }
        public static void AddGlobalVariable(string identifier, object value)
        {
            GlobalVariables[identifier] = value;
        }
        public static object GetGlobalVariable(string identifier)
        {
            return GlobalVariables[identifier];
        }
        public static object Sqrt(List<object> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new Error(ErrorType.SEMANTIC, "The sqrt function expects exactly one argument.");
            }
            if (arguments[0] is double)
            {
                return Math.Sqrt((double)arguments[0]);
            }
            else
            {
                throw new Error(ErrorType.SEMANTIC, "The sqrt function expects a numeric argument.");
            }
        }
        public static object Sin(List<object> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new Error(ErrorType.SEMANTIC, "The sin function expects exactly one argument.");
            }
            if (arguments[0] is double)
            {
                return Math.Sin((double)arguments[0]);
            }
            else
            {
                throw new Error(ErrorType.SEMANTIC, "The sin function expects a numeric argument.");
            }
        }
        public static object Cos(List<object> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new Error(ErrorType.SEMANTIC, "The cos function expects exactly one argument.");
            }
            if (arguments[0] is double)
            {
                return Math.Cos((double)arguments[0]);
            }
            else
            {
                throw new Error(ErrorType.SEMANTIC, "The cos function expects a numeric argument.");
            }
        }
        public static object Log(List<object> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new Error(ErrorType.SEMANTIC, "The log function expects exactly one argument.");
            }
            if (arguments[0] is double)
            {
                return Math.Log((double)arguments[0]);
            }
            else
            {
                throw new Error(ErrorType.SEMANTIC, "The log function expects a numeric argument.");
            }
        }
        public static object Exp(List<object> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new Error(ErrorType.SEMANTIC, "The exp function expects exactly one argument.");
            }
            if (arguments[0] is double)
            {
                return Math.Exp((double)arguments[0]);
            }
            else
            {
                throw new Error(ErrorType.SEMANTIC, "The exp function expects a numeric argument.");
            }
        }
    }
}
