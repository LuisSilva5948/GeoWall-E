using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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
        /// Random sequence of numbers between 0 and 1 generated at the beginning of the program.
        /// </summary>
        public static FiniteSequence RandomNumericSequence = RandomNumberSequence();
        /// <summary>
        /// The dictionary of predefined functions of the G# language.
        /// </summary>
        public static Dictionary<string, Func<List<object>, object>> PredefinedFunctions { get; } = new Dictionary<string, Func<List<object>, object>>()
        {
            { "sqrt", Sqrt },
            { "sin", Sin },
            { "cos", Cos },
            { "log", Log },
            { "exp", Exp },
            { "count", Count },
            { "measure", Measure },
            { "point", Point },
            { "line", Line },
            { "ray", Ray },
            { "segment", Segment },
            { "circle", Circle },
            { "arc", Arc },
            { "randoms", Randoms },
            { "samples", Samples },
            { "points", Points }
        };  
        /// <summary>
        /// The dictionary of declared functions during the execution of the program.
        /// </summary>
        public static Dictionary<string, Function> DeclaredFunctions { get; private set; } = new Dictionary<string, Function>();
        
        /// <summary>
        /// Adds a function declaration to the dictionary of declared functions.
        /// </summary>
        /// <param name="function">The function declaration to add.</param>

        /// <summary>
        /// Resets the standard library.
        public static void Reset()
        {
            RandomNumericSequence = RandomNumberSequence();
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
        /// Random sequence of numbers generator between 0 and 1.
        /// </summary>
        public static FiniteSequence RandomNumberSequence()
        {
            int amount = Random.Next(2, 20);
            List<Expression> numbers = new List<Expression>();
            for (int i = 0; i < amount; i++)
            {
                numbers.Add(new LiteralExpression(Random.NextDouble()));
            }
            return new FiniteSequence(numbers);
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
        /// Slope of a line between two points in relation to the x axis.
        /// </summary>
        public static double Slope(Point p1, Point p2)
        {
            if (p2.X == p1.X)
                return double.NaN;
            else
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }
        /// <summary>
        /// Intercept of a line between two points with the y axis.
        /// </summary>
        public static double Intercept(Point p1, Point p2)
        {
            double slope = Slope(p1, p2);
            if (double.IsNaN(slope))
                return double.NaN;
            else
                return p1.Y - slope * p1.X;
        }
        /// <summary>
        /// Random point in segment generator.
        /// </summary>
        public static Point RandomPointInSegment(Segment segment)
        {
            // If the segment is a point, return the point itself.
            if (segment.P1.Equals(segment.P2))
                return segment.P1;
            // Calculate the slope.
            double slope = Slope(segment.P1, segment.P2);
            if (double.IsNaN(slope))
            {
                // If the segment is vertical, X is the same as the X of the points.
                // Y is a random number between the Y of the points.
                double minY = Math.Min(segment.P1.Y, segment.P2.Y);
                double maxY = Math.Max(segment.P1.Y, segment.P2.Y);
                double y = Random.NextDouble() * (maxY - minY) + minY;
                return new Point(segment.P1.X, y);
            }
            else
            {
                // Calculate the intercept of the line that contains the segment and the coordinates of the random point.
                // X is a random number between the X of the points.
                // Y is the result of the equation of the line evaluated in X.
                double intercept = Intercept(segment.P1, segment.P2);
                double x = Random.NextDouble() * (segment.P2.X - segment.P1.X) + segment.P1.X;
                double y = slope * x + intercept;
                return new Point(x, y);
            }
        }
        /// <summary>
        /// Random point in ray generator.
        /// </summary>
        public static Point RandomPointInRay(Ray ray)
        {
            // If the ray is a point, return the point itself.
            if (ray.P1.Equals(ray.P2))
                return ray.P1;
            // Calculate the slope.
            double slope = Slope(ray.P1, ray.P2);
            // Calculate the orientation of the ray.
            double orientation = Orientation(ray);
            if (double.IsNaN(slope))
            {
                // If the ray is vertical, X is the same as the X of the points.
                // Y is a random number between the Y of the points.
                double y = Random.NextDouble() * Interpreter.UI.CanvasHeight * orientation + ray.P1.Y;
                return new Point(ray.P1.X, y);
            }
            else
            {
                // Calculate the intercept of the line that contains the ray and the coordinates of the random point.
                // X is a random number between the X of the points.
                // Y is the result of the equation of the line evaluated in X.
                double intercept = Intercept(ray.P1, ray.P2);
                double x = Random.NextDouble() * Interpreter.UI.CanvasWidth * orientation + ray.P1.X;
                double y = slope * x + intercept;
                return new Point(x, y);
            }
        }
        /// <summary>
        /// Random point in line generator.
        /// </summary>
        public static Point RandomPointInLine(Line line)
        {
            // If the line is a point, the random point is the point itself.
            if (line.P1.Equals(line.P2))
                return line.P1;
            // Calculate the slope.
            double slope = Slope(line.P1, line.P2);
            if (double.IsNaN(slope))
            {
                // If the ray is vertical, X is the same as the X of the points.
                // Y is a random number between the Y of the points.
                double y = Random.NextDouble() * Interpreter.UI.CanvasHeight;
                return new Point(line.P1.X, y);
            }
            else
            {
                // Calculate the intercept of the line that contains the ray and the coordinates of the random point.
                // X is a random number between 0 and the width of the canvas.
                // Y is the result of the equation of the line evaluated in X.
                double intercept = Intercept(line.P1, line.P2);
                double x = Random.NextDouble() * Interpreter.UI.CanvasWidth;
                double y = slope * x + intercept;
                return new Point(x, y);
            }
        }
        /// <summary>
        /// Random point in circle generator.
        /// </summary>
        public static Point RandomPointInCircle(Circle circle)
        {
            // If the circle is a point, return the point itself.
            if (circle.Radius.Value == 0)
                return circle.Center;
            // Get a random angle between 0 and 360 degrees in radians.
            double angle = Random.NextDouble() * 2 * Math.PI;
            // Calculate the x and y coordinates of the point by using polar coordinates.
            double x = circle.Center.X + circle.Radius.Value * Math.Cos(angle);
            double y = circle.Center.Y + circle.Radius.Value * Math.Sin(angle);
            return new Point(x, y);
        }
        /// <summary>
        /// Random point in arc generator.
        /// </summary>
        public static Point RandomPointInArc(Arc arc)
        {
            // If the arc is a point, return the point itself.
            if (arc.Radius.Value == 0 || arc.InitialRayPoint.Equals(arc.Center) || arc.FinalRayPoint.Equals(arc.Center))
                return arc.Center;
            // If the arc is a circle, return a random point in the circle.
            if (arc.InitialRayPoint.Equals(arc.FinalRayPoint))
                return RandomPointInCircle(new Circle(arc.Center, arc.Radius));
            // Get the angles of the initial and final rays respect to the x axis.
            double startAngle = GetLineAngle(arc.Center, arc.InitialRayPoint);
            double endAngle = GetLineAngle(arc.Center, arc.FinalRayPoint);

            if (startAngle > endAngle)
                endAngle += 2 * Math.PI;

            double sweepAngle = endAngle - startAngle;
            sweepAngle -= 2 * Math.PI;

            double angle = Random.NextDouble() * (sweepAngle) + startAngle;
            double x = arc.Center.X + arc.Radius.Value * Math.Cos(angle);
            double y = arc.Center.Y + arc.Radius.Value * Math.Sin(angle);
            return new Point(x, y);
        }
        /// <summary>
        /// Gets the angle of the line between two points in radians in relation to the x axis.
        /// </summary>
        public static double GetLineAngle(Point p1, Point p2)
        {
            if (p1.Equals(p2))
                return 0;
            double y = (p2.Y - p1.Y);
            double x = (p2.X - p1.X);
            double angle = Math.Atan2(y, x);
            return angle;
        }
        /// <summary>
        /// Gets the orientation of a ray.
        /// </summary>
        public static double Orientation(Ray ray)
        {
            return Slope(ray.P1, ray.P2) == 0 ? Math.Sign(ray.P2.X - ray.P1.X) : Math.Sign(ray.P2.Y - ray.P1.Y);
        }


        #endregion

        #region Predefined G# Functions

        /// <summary>
        /// Counts the number of elements in a sequence.
        /// </summary>
        public static object Count(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The count function expects exactly one argument.");
            if (arguments[0] is FiniteSequence)
                return ((FiniteSequence)arguments[0]).Count;
            if (arguments[0] is RangeSequence)
            {
                return ((RangeSequence)arguments[0]).Count;
            }
            if (arguments[0] is InfiniteSequence)
            {
                return new Undefined();
            }
            else throw new GSharpError(ErrorType.COMPILING, "The count function expects a sequence as argument.");
        }
        /// <summary>
        /// Returns the random sequence of numbers between 0 and 1 generated at the beginning of the program.
        /// </summary>
        public static object Randoms(List<object> arguments)
        {
            if (arguments.Count != 0)
                throw new GSharpError(ErrorType.COMPILING, "The randoms function doesn't expect any arguments.");
            return RandomNumericSequence;
        }
        /// <summary>
        /// Returns a sequence of random points in the canvas.
        /// </summary>
        public static object Samples(List<object> arguments)
        {
            if (arguments.Count != 0)
                throw new GSharpError(ErrorType.COMPILING, "The samples function doesn't expect any arguments.");
            return RandomPointSequence();
        }
        /// <summary>
        /// Returns a sequence of random points in a figure.
        /// </summary>
        public static object Points(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The points function expects exactly one argument.");
            if (arguments[0] is not GSharpFigure)
                throw new GSharpError(ErrorType.COMPILING, "The points function expects a figure as argument.");
            switch (arguments[0])
            {
                case Segment segment:
                    return RandomPointInSegment(segment);
                case Ray ray:
                    return RandomPointInRay(ray);
                case Line line:
                    return RandomPointInLine(line);
                case Point point:
                    return point;
                case Circle circle:
                    return RandomPointInCircle(circle);
                case Arc arc:
                    return RandomPointInArc(arc);
                default:
                    throw new GSharpError(ErrorType.COMPILING, "The points function expects a figure as argument.");
            }
        }

        /// <summary>
        /// Returns the measure between two points.
        /// </summary>
        public static Measure Measure(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The measure function expects exactly two arguments.");
            if (arguments[0] is Point && arguments[1] is Point)
            return new Measure(Distance((Point)arguments[0], (Point)arguments[1]));
            else
                throw new GSharpError(ErrorType.COMPILING, "The measure function expects two points as arguments.");
        }
        /// <summary>
        /// Returns a point with the given coordinates.
        /// </summary>
        public static Point Point(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The point function expects exactly two arguments.");
            if (arguments[0] is double && arguments[1] is double)
                return new Point((double)arguments[0], (double)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The point function expects two numeric arguments.");
        }
        /// <summary>
        /// Returns a line that passes through the given points.
        /// </summary>
        public static Line Line(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The line function expects exactly two arguments.");
            if (arguments[0] is Point && arguments[1] is Point)
                return new Line((Point)arguments[0], (Point)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The line function expects two points as arguments.");
        }
        /// <summary>
        /// Returns a ray that starts at the first point and passes through the second point.
        /// </summary>
        public static Ray Ray(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The ray function expects exactly two arguments.");
            if (arguments[0] is Point && arguments[1] is Point)
                return new Ray((Point)arguments[0], (Point)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The ray function expects two points as arguments.");
        }
        /// <summary>
        /// Returns a segment that starts at the first point and ends at the second point.
        /// </summary>
        public static Segment Segment(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The segment function expects exactly two arguments.");
            if (arguments[0] is Point && arguments[1] is Point)
                return new Segment((Point)arguments[0], (Point)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The segment function expects two points as arguments.");
        }
        /// <summary>
        /// Returns a circle with the given center and radius.
        /// </summary>
        public static Circle Circle(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The circle function expects exactly two arguments.");
            if (arguments[0] is Point && arguments[1] is Measure)
                return new Circle((Point)arguments[0], (Measure)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The circle function expects a point and a measure as arguments.");
        }
        /// <summary>
        /// Returns an arc with the given center, radius, and two points.
        /// </summary>
        public static Arc Arc(List<object> arguments)
        {
            if (arguments.Count != 4)
                throw new GSharpError(ErrorType.COMPILING, "The arc function expects exactly four arguments.");
            if (arguments[0] is Point && arguments[1] is Point && arguments[2] is Point && arguments[3] is Measure)
                return new Arc((Point)arguments[0], (Measure)arguments[3], (Point)arguments[2], (Point)arguments[1]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The arc function expects three points and a measure as arguments.");
        }


        #endregion

        #region Predefined Math Functions
        /// <summary>
        /// Square root function.
        /// </summary>
        public static object Sqrt(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The sqrt function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Sqrt((double)arguments[0]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The sqrt function expects a numeric argument.");
        }
        /// <summary>
        /// Sine function.
        /// </summary>
        public static object Sin(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The sin function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Sin((double)arguments[0]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The sin function expects a numeric argument.");
        }
        /// <summary>
        /// Cosine function.
        /// </summary>
        public static object Cos(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The cos function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Cos((double)arguments[0]);
            else
                throw new GSharpError(ErrorType.COMPILING, "The cos function expects a numeric argument.");
        }
        /// <summary>
        /// Logarithm function.
        /// </summary>
        public static object Log(List<object> arguments)
        {
            if (arguments.Count != 2)
                throw new GSharpError(ErrorType.COMPILING, "The log function expects exactly one argument.");
            if (arguments[0] is double a && arguments[1] is double b)
            {
                if (a > 0 && b > 0 && b != 1)
                    return Math.Log(a, b);
                else
                    throw new GSharpError(ErrorType.COMPILING, "The log function was called with invalid arguments.");
            }
            else
                throw new GSharpError(ErrorType.COMPILING, "The log function expects a numeric argument.");
        }
        /// <summary>
        /// Exponential function.
        /// </summary>
        public static object Exp(List<object> arguments)
        {
            if (arguments.Count != 1)
                throw new GSharpError(ErrorType.COMPILING, "The exp function expects exactly one argument.");
            if (arguments[0] is double)
                return Math.Exp((double)arguments[0]);
            else 
                throw new GSharpError(ErrorType.COMPILING, "The exp function expects a numeric argument.");
        }


        #endregion
    }
}
