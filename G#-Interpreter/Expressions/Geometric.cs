using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public abstract class Geometric : Expression
    {
        public class Measure : Geometric
        {
            public double Value { get; }
            public Measure(double value)
            {
                Value = value;
            }
        }
        public class Point : Geometric
        {
            public Expression X { get; }
            public Expression Y { get; }
            public Point()
            {
                X = new LiteralExpression(StandardLibrary.RandomXCoordinate());
                Y = new LiteralExpression(StandardLibrary.RandomYCoordinate());
            }
        }
        public class Line : Geometric
        {
            public Point P1 { get; }
            public Point P2 { get; }
            public Line(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;
            }
            public Line()
            {
                P1 = new Point();
                P2 = new Point();
            }
        }
        public class Segment : Geometric
        {
            public Point P1 { get; }
            public Point P2 { get; }
            public Segment(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;
            }
            public Segment()
            {
                P1 = new Point();
                P2 = new Point();
            }
        }
        public class Circle : Geometric
        {
            public Point Center { get; }
            public Expression Radius { get; }
            public Circle(Point center, Expression radius)
            {
                Center = center;
                Radius = radius;
            }
            public Circle()
            {
                Center = new Point();
                Radius = new LiteralExpression(StandardLibrary.Random.Next(0, 200));
            }
        }
        public class Arc : Geometric
        {
            public Point Center { get; }
            public Expression Radius { get; }
            public Expression StartAngle { get; }
            public Expression EndAngle { get; }
            public Arc(Point center, Expression radius, Expression startAngle, Expression endAngle)
            {
                Center = center;
                Radius = radius;
                StartAngle = startAngle;
                EndAngle = endAngle;
            }
            public Arc()
            {
                Center = new Point();
                Radius = new LiteralExpression(StandardLibrary.Random.Next(0, 200));
                StartAngle = new LiteralExpression(StandardLibrary.Random.Next(0, 360));
                EndAngle = new LiteralExpression(StandardLibrary.Random.Next(0, 360));
            }
        }
    }
}
