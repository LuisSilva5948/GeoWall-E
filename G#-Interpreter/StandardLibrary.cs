using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    /// <summary>
    /// Represents the standard library of the G# language.
    /// </summary>
    public static class StandardLibrary
    {
        /// <summary>
        /// Random number generator.
        /// </summary>
        public static Random Random { get; } = new Random();
        /// <summary>
        /// The dictionary of predefined functions of the G# language.
        /// </summary>
        public static Dictionary<string, Func<List<object>, object>> PredefinedFunctions { get; } = new Dictionary<string, Func<List<object>, object>>()
        {
            { "sqrt", Sqrt },
            { "sin", Sin },
            { "cos", Cos },
            { "log", Log },
            { "exp", Exp }
        };  
        /// <summary>
        /// The dictionary of declared functions during the execution of the program.
        /// </summary>
        public static Dictionary<string, Function> DeclaredFunctions { get; private set; } = new Dictionary<string, Function>();
        /// <summary>
        /// The dictionary of global variables.
        /// </summary>
        public static Dictionary<string, object> GlobalVariables { get; private set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Adds a function declaration to the dictionary of declared functions.
        /// </summary>
        /// <param name="function">The function declaration to add.</param>

        /// <summary>
        /// Resets the standard library.
        public static void Reset()
        {
            DeclaredFunctions = new Dictionary<string, Function>();
            GlobalVariables = new Dictionary<string, object>();
        }

        public static void AddFunction(Function function)
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



        #region Auxiliary Functions

        /// <summary>
        /// Random X coordinate generator.
        /// </summary>
        /// <returns> A random number between 0 and the width of the canvas.</returns>
        public static int RandomXCoordinate()
        {
            return Random.Next(0, Interpreter.UI.CanvasWidth);
        }
        /// <summary>
        /// Random Y coordinate generator.
        /// </summary>
        /// <returns> A random number between 0 and the height of the canvas.</returns>
        public static int RandomYCoordinate()
        {
            return Random.Next(0, Interpreter.UI.CanvasHeight);
        }
        /// <summary>
        /// Distance between two points.
        /// </summary>
        /// <returns> Returns the distance between two points.</returns>
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        /// <summary>
        /// Random measure generator.
        /// </summary>
        public static Measure RandomMeasure()
        {
            int limit = Math.Min(Interpreter.UI.CanvasWidth, Interpreter.UI.CanvasHeight)/2;
            return new Measure(Random.Next(0, limit));
        }


        #endregion

        #region Predefined G# Functions

        /// <summary>
        /// Counts the number of elements in a sequence.
        /// </summary>
        public static object Count(Sequence seq)
        {
            if (seq is FiniteSequence)
                return ((FiniteSequence)seq).Count;
            if (seq is RangeSequence)
            {
                return ((RangeSequence)seq).Count;
            }
            return new Undefined();
        }
        /// <summary>
        /// Returns the measure between two points.
        /// </summary>
        public static Measure Measure(Point p1, Point p2)
        {
            return new Measure(Distance(p1, p2));
        }


        #endregion

        #region Predefined Math Functions
        /// <summary>
        /// Square root function.
        /// </summary>
        public static object Sqrt(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new Error(ErrorType.COMPILING, "The sqrt function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Sqrt((double)arguments[0]);
            else
                throw new Error(ErrorType.COMPILING, "The sqrt function expects a numeric argument.");
        }
        /// <summary>
        /// Sine function.
        /// </summary>
        public static object Sin(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new Error(ErrorType.COMPILING, "The sin function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Sin((double)arguments[0]);
            else
                throw new Error(ErrorType.COMPILING, "The sin function expects a numeric argument.");
        }
        /// <summary>
        /// Cosine function.
        /// </summary>
        public static object Cos(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new Error(ErrorType.COMPILING, "The cos function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Cos((double)arguments[0]);
            else
                throw new Error(ErrorType.COMPILING, "The cos function expects a numeric argument.");
        }
        /// <summary>
        /// Logarithm function.
        /// </summary>
        public static object Log(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new Error(ErrorType.COMPILING, "The log function expects exactly one argument.");
            if (arguments[0] is double a && arguments[1] is double b)
            {
                if (a > 0 && b > 0 && b != 1)
                    return Math.Log(a, b);
                else
                    throw new Error(ErrorType.COMPILING, "The log function was called with invalid arguments.");
            }
            else
                throw new Error(ErrorType.COMPILING, "The log function expects a numeric argument.");
        }
        /// <summary>
        /// Exponential function.
        /// </summary>
        public static object Exp(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new Error(ErrorType.COMPILING, "The exp function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Exp((double)arguments[0]);
            else 
                throw new Error(ErrorType.COMPILING, "The exp function expects a numeric argument.");
        }

        #endregion
    }
}
