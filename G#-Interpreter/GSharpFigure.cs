using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GSharpInterpreter
{
    public abstract class GSharpFigure: Expression
    {
    }
    public class Measure : GSharpFigure
    {
        public double Value { get; }
        public Measure(double value)
        {
            Value = value;
        }
    }
    public class Point : GSharpFigure
    {
        public double X { get; }
        public double Y { get; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public class Line : GSharpFigure
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class Segment : GSharpFigure
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Segment(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class Ray : GSharpFigure
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Ray(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class Circle : GSharpFigure
    {
        public Point Center { get; }
        public Measure Radius { get; }
        public Circle(Point center, Measure radius)
        {
            Center = center;
            Radius = radius;
        }
    }
    public class Arc : GSharpFigure
    {
        public Point Center { get; }
        public Measure Radius { get; }
        public Point InitialRayPoint { get; }
        public Point FinalRayPoint { get; }
        public Arc(Point center, Measure radius, Point initialRayPoint, Point finalRayPoint)
        {
            Center = center;
            Radius = radius;
            InitialRayPoint = initialRayPoint;
            FinalRayPoint = finalRayPoint;
        }
    }
}
