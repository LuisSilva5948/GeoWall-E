using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents a scope or context in which variables and constants are defined.
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Stack of functions that can be called.
        /// </summary>
        public Stack<Dictionary<string, Function>> Functions { get; private set; }
        /// <summary>
        /// Stack of colors that can be changed.
        /// </summary>
        public Stack<GSharpColor> Colors { get; private set; }
        /// <summary>
        /// Arguments of a function that can change.
        /// </summary>
        public Stack<Dictionary<string, object>> Arguments { get; private set; }
        /// <summary>
        /// Constant values that can't be changed.
        /// </summary>
        public Stack<Dictionary<string, object>> Constants { get; private set; }
        public Scope()
        {
            Colors = new Stack<GSharpColor>();
            Arguments = new Stack<Dictionary<string, object>>();
            Constants = new Stack<Dictionary<string, object>>();
            Functions = new Stack<Dictionary<string, Function>>();
            Arguments.Push(new Dictionary<string, object>());
            Constants.Push(new Dictionary<string, object>());
            Functions.Push(new Dictionary<string, Function>());
        }
        /// <summary>
        /// Sets the constant with the given identifier to the given value in the current scope.
        /// </summary>
        public void SetConstant(string identifier, object value)
        {
            if (identifier == "_") return;
            Constants.Peek()[identifier] = value;
        }
        public object GetValue(string identifier)
        {
            if (Arguments.Peek().ContainsKey(identifier))
                return Arguments.Peek()[identifier];
            else if (Constants.Peek().ContainsKey(identifier))
                return Constants.Peek()[identifier];
            else
                throw new GSharpError(ErrorType.COMPILING, $"Constant '{identifier}' doesn't exist.");
        }
        /// <summary>
        /// Sets the argument with the given identifier to the given value in the current scope.
        /// </summary>
        public void SetArgument(string identifier, object value)
        {
            Arguments.Peek()[identifier] = value;
        }
        /// <summary>
        /// Checks if the given identifier exists in the current scope.
        /// </summary>
        public bool Exists(string identifier)
        {
            return Constants.Peek().ContainsKey(identifier) || Arguments.Peek().ContainsKey(identifier);
        }
        /// <summary>
        /// Reserves the given identifier in the current scope.
        /// </summary>
        public void Reserve(string identifier)
        {
            if (Exists(identifier))
                throw new GSharpError(ErrorType.COMPILING, $"Another constant named '{identifier}' already exists and can't be altered.");
            SetArgument(identifier, new Undefined());
        }
        /// <summary>
        /// Creates a new scope with the values of the current scope and pushes it to the stack of scopes.
        /// </summary>
        public void EnterScope()
        {
            Dictionary<string, object> newVariables = new Dictionary<string, object>();
            Dictionary<string, object> newConstants = new Dictionary<string, object>();
            Dictionary<string, Function> newFunctions = new Dictionary<string, Function>();
            foreach (var keyvaluepair in Arguments.Peek())
                newVariables[keyvaluepair.Key] = keyvaluepair.Value;
            foreach (var keyvaluepair in Constants.Peek())
                newConstants[keyvaluepair.Key] = keyvaluepair.Value;
            foreach (var keyvaluepair in Functions.Peek())
                newFunctions[keyvaluepair.Key] = keyvaluepair.Value;
            Arguments.Push(newVariables);
            Constants.Push(newConstants);
            Functions.Push(newFunctions);
        }
        /// <summary>
        /// Removes the topmost scope from the stack of scopes.
        /// </summary>
        public void ExitScope()
        {
            Constants.Pop();
            Arguments.Pop();
            Functions.Pop();
        }
        /// <summary>
        /// Adds a function to the current scope. If the function already exists, it throws an error.
        /// </summary>
        public void AddFunction(Function function)
        {
            if (StandardLibrary.PredefinedFunctions.ContainsKey(function.Identifier) || !Functions.Peek().TryAdd(function.Identifier, function))
            {
                throw new GSharpError(ErrorType.COMPILING, $"Function '{function.Identifier}' already exists and can't be redeclared.");
            }
        }
        /// <summary>
        /// Gets the function with the given identifier from the current scope. If the function doesn't exist, it throws an error.
        /// </summary>
        public Function GetFunction(string identifier)
        {
            if (Functions.Peek().TryGetValue(identifier, out Function function))
                return function;
            else
                throw new GSharpError(ErrorType.COMPILING, $"Function '{identifier}' doesn't exist.");
        }
        /// <summary>
        /// Sets the color of the drawing.
        /// </summary>
        public void SetColor(GSharpColor color)
        {
            Colors.Push(color);
        }
        /// <summary>
        /// Gets the color of the drawing. If there is no color, it returns black.
        /// </summary>
        public GSharpColor GetColor()
        {
            if (Colors.Count == 0)
                return GSharpColor.BLACK;
            else
            return Colors.Peek();
        }
        /// <summary>
        /// Restores the color of the drawing to the previous one.
        /// </summary>
        public void RestoreColor()
        {
            if (Colors.Count > 1)
                Colors.Pop();
        }
    }
}
