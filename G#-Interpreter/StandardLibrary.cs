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
        public static double RandomXCoordinate()
        {
            return Random.Next(0, Interpreter.UI.CanvasWidth);
        }
        /// <summary>
        /// Random Y coordinate generator.
        /// </summary>
        /// <returns> A random number between 0 and the height of the canvas.</returns>
        public static double RandomYCoordinate()
        {
            return Random.Next(0, Interpreter.UI.CanvasHeight);
        }
        /// <summary>
        /// Random point generator.
        /// </summary>
        public static Point RandomPoint()
        {
            return new Point(RandomXCoordinate(), RandomYCoordinate());
        }
        /// <summary>
        /// Random measure generator.
        /// </summary>
        public static Measure RandomMeasure()
        {
            int limit = Math.Min(Interpreter.UI.CanvasWidth, Interpreter.UI.CanvasHeight) / 2;
            return new Measure(Random.Next(0, limit));
        }
        /// <summary>
        /// Random line generator.
        /// </summary>
        public static Line RandomLine()
        {
            return new Line(RandomPoint(), RandomPoint());
        }
        /// <summary>
        /// Random ray generator.
        /// </summary>
        public static Ray RandomRay()
        {
            return new Ray(RandomPoint(), RandomPoint());
        }
        /// <summary>
        /// Random segment generator.
        /// </summary>
        public static Segment RandomSegment()
        {
            return new Segment(RandomPoint(), RandomPoint());
        }
        /// <summary>
        /// Random circle generator.
        /// </summary>
        public static Circle RandomCircle()
        {
            return new Circle(RandomPoint(), RandomMeasure());
        }
        /// <summary>
        /// Random arc generator.
        /// </summary>
        public static Arc RandomArc()
        {
            return new Arc(RandomPoint(), RandomMeasure(), RandomPoint(), RandomPoint());
        }
        /// <summary>
        /// Random sequence of points generator.
        /// </summary>
        /// <returns></returns>
        public static FiniteSequence RandomPointSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> points = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                points.Add(RandomPoint());
            }
            return new FiniteSequence(points);
        }
        /// <summary>
        /// Random sequence of lines generator.
        /// </summary>
        public static FiniteSequence RandomLineSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> lines = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                lines.Add(RandomLine());
            }
            return new FiniteSequence(lines);
        }
        /// <summary>
        /// Random sequence of rays generator.
        /// </summary>
        public static FiniteSequence RandomRaySequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> rays = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                rays.Add(RandomRay());
            }
            return new FiniteSequence(rays);
        }
        /// <summary>
        /// Random sequence of segments generator.
        /// </summary>
        public static FiniteSequence RandomSegmentSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> segments = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                segments.Add(RandomSegment());
            }
            return new FiniteSequence(segments);
        }
        /// <summary>
        /// Random sequence of circles generator.
        /// </summary>
        public static FiniteSequence RandomCircleSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> circles = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                circles.Add(RandomCircle());
            }
            return new FiniteSequence(circles);
        }
        /// <summary>
        /// Random sequence of arcs generator.
        /// </summary>
        /// <returns></returns>
        public static FiniteSequence RandomArcSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> arcs = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                arcs.Add(RandomArc());
            }
            return new FiniteSequence(arcs);
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
        /// Random point in circle generator.
        /// </summary>
        public static Point RandomPointInCircle(Circle circle)
        {
            double angle = Random.NextDouble() * 2 * Math.PI;
            double x = circle.Center.X + circle.Radius.Value * Math.Cos(angle);
            double y = circle.Center.Y + circle.Radius.Value * Math.Sin(angle);
            return new Point(x, y);
        }
        public static Point RandomPointInArc(Arc arc)
        {
            double angle = Random.NextDouble() * 2 * Math.PI;
            double x = arc.Center.X + arc.Radius.Value * Math.Cos(angle);
            double y = arc.Center.Y + arc.Radius.Value * Math.Sin(angle);
            return new Point(x, y);
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
