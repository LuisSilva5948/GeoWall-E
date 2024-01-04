using System;
using System.Collections.Generic;

namespace GSharpInterpreter
{
    public abstract class GSharpFigure: Expression
    {
    }
    public class Measure : GSharpFigure
    {
        public GSharpType Type => GSharpType.MEASURE;
        public double Value { get; }
        public Measure(double value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public override bool Equals(object? obj)
        {
            if (obj is Measure measure)
            {
                return Value.Equals(measure.Value);
            }
            else return false;
        }
        public static Measure operator *(Measure m, double num)
        {
            return new Measure(m.Value * double.Floor(num));
        }
        public static Measure operator -(Measure m1, Measure m2)
        {
            return new Measure(m1.Value - m2.Value);
        }
    }
    public class Point : GSharpFigure
    {
        public GSharpType Type => GSharpType.POINT;
        public double X { get; }
        public double Y { get; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"Point({X}, {Y})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Point point)
            {
                return X.Equals(point.X) && Y.Equals(point.Y);
            }
            else return false;
        }
    }
    public class Line : GSharpFigure
    {
        public GSharpType Type => GSharpType.LINE;
        public Point P1 { get; }
        public Point P2 { get; }
        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public override string ToString()
        {
            return $"Line({P1}, {P2})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Line line)
            {
                return StandardLibrary.Slope(P1, P2).Equals(StandardLibrary.Slope(line.P1, line.P2)) && StandardLibrary.Intercept(P1, P2).Equals(StandardLibrary.Intercept(line.P1, line.P2));
            }
            else return false;
        }
    }
    public class Segment : GSharpFigure
    {
        public GSharpType Type => GSharpType.SEGMENT;
        public Point P1 { get; }
        public Point P2 { get; }
        public Segment(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public override string ToString()
        {
            return $"Segment({P1}, {P2})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Segment segment)
            {
                return (P1.Equals(segment.P1) && P2.Equals(segment.P2)) || (P1.Equals(segment.P2) && P2.Equals(segment.P1));
            }
            else return false;
        }
    }
    public class Ray : GSharpFigure
    {
        public GSharpType Type => GSharpType.RAY;
        public Point P1 { get; }
        public Point P2 { get; }
        public Ray(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public override string ToString()
        {
            return $"Ray({P1}, {P2})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Ray ray)
            {
                return StandardLibrary.Slope(P1, P2).Equals(StandardLibrary.Slope(ray.P1, ray.P2)) && P1.Equals(ray.P1) && StandardLibrary.Orientation(this).Equals(StandardLibrary.Orientation(ray));
            }
            else return false;
        }
    }
    public class Circle : GSharpFigure
    {
        public GSharpType Type => GSharpType.CIRCLE;
        public Point Center { get; }
        public Measure Radius { get; }
        public Circle(Point center, Measure radius)
        {
            Center = center;
            Radius = radius;
        }
        public override string ToString()
        {
            return $"Circle({Center}, {Radius})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Circle circle)
            {
                return Center.Equals(circle.Center) && Radius.Equals(circle.Radius);
            }
            else return false;
        }
    }
    public class Arc : GSharpFigure
    {
        public GSharpType Type => GSharpType.ARC;
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
        public override string ToString()
        {
            return $"Arc({Center}, {Radius}, {InitialRayPoint}, {FinalRayPoint})";
        }
        public override bool Equals(object? obj)
        {
            if (obj is Arc arc)
            {
                return Center.Equals(arc.Center) && Radius.Equals(arc.Radius) && InitialRayPoint.Equals(arc.InitialRayPoint) && FinalRayPoint.Equals(arc.FinalRayPoint);
            }
            else return false;
        }
    }
}
