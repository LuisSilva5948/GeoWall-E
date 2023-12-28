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
        /// Stack of colors that can be changed.
        /// </summary>
        public Stack<GSharpColor> Colors { get; private set; }
        /// <summary>
        /// List of all identifiers that are defined in this scope.
        /// </summary>
        public List<string> Identifiers { get; private set; }
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
            Identifiers = new List<string>();
            Arguments = new Stack<Dictionary<string, object>>();
            Constants = new Stack<Dictionary<string, object>>();
            Arguments.Push(new Dictionary<string, object>());
            Constants.Push(new Dictionary<string, object>());
        }
        /// <summary>
        /// Sets the constant with the given identifier to the given value in the current scope.
        /// </summary>
        public void SetConstant(string identifier, object value)
        {
            if (identifier == "_") return;
            Reserve(identifier);
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
            // Check if the identifier exists in the current scope to avoid creating a new variable
            if (Exists(identifier))
                Arguments.Peek()[identifier] = value;
            else
                throw new GSharpError(ErrorType.COMPILING, $"Argument '{identifier}' doesn't exist.");
        }
        /// <summary>
        /// Checks if the given identifier exists in the current scope.
        /// </summary>
        public bool Exists(string identifier)
        {
            return Identifiers.Contains(identifier);
        }
        /// <summary>
        /// Reserves the given identifier in the current scope.
        /// </summary>
        public void Reserve(string identifier)
        {
            if (Exists(identifier))
                throw new GSharpError(ErrorType.COMPILING, $"Another constant named '{identifier}' already exists and can't be altered.");
            Identifiers.Add(identifier);
        }
        /// <summary>
        /// Creates a new scope with the values of the current scope and pushes it to the stack of scopes.
        /// </summary>
        public void EnterScope()
        {
            Dictionary<string, object> newVariables = new Dictionary<string, object>();
            Dictionary<string, object> newConstants = new Dictionary<string, object>();
            foreach (var keyvaluepair in Arguments.Peek())
                newVariables[keyvaluepair.Key] = keyvaluepair.Value;
            foreach (var keyvaluepair in Constants.Peek())
                newConstants[keyvaluepair.Key] = keyvaluepair.Value;
            Arguments.Push(newVariables);
            Constants.Push(newConstants);
        }
        /// <summary>
        /// Removes the topmost scope from the stack of scopes.
        /// </summary>
        public void ExitScope()
        {
            Constants.Pop();
            Arguments.Pop();
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
