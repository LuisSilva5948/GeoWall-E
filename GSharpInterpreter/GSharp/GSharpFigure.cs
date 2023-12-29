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
        public virtual void Draw(IUserInterface userInterface, GSharpColor color)
        {
            if (this is Arc arc)
            {
                userInterface.DrawArc(arc, color);
            }
            else if (this is Circle circle)
            {
                userInterface.DrawCircle(circle, color);
            }
            else if (this is Line line)
            {
                userInterface.DrawLine(line, color);
            }
            else if (this is Point point)
            {
                userInterface.DrawPoint(point, color);
            }
            else if (this is Segment segment)
            {
                userInterface.DrawSegment(segment, color);
            }
            else if (this is Ray ray)
            {
                userInterface.DrawRay(ray, color);
            }
            else
            {
                throw new GSharpError(ErrorType.RUNTIME, $"Unknown figure type: {this.GetType().Name}");
            }
        }
        public override string ToString()
        {
            if (this is Arc arc)
            {
                return $"Arc({arc.Center}, {arc.Radius}, {arc.InitialRayPoint}, {arc.FinalRayPoint})";
            }
            else if (this is Circle circle)
            {
                return $"Circle({circle.Center}, {circle.Radius})";
            }
            else if (this is Line line)
            {
                return $"Line({line.P1}, {line.P2})";
            }
            else if (this is Point point)
            {
                return $"Point({point.X}, {point.Y})";
            }
            else if (this is Segment segment)
            {
                return $"Segment({segment.P1}, {segment.P2})";
            }
            else if (this is Ray ray)
            {
                return $"Ray({ray.P1}, {ray.P2})";
            }
            else
            {
                throw new GSharpError(ErrorType.RUNTIME, $"Unknown figure type: {this.GetType().Name}");
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj is GSharpFigure figure)
            {
                if (this is Arc arc && figure is Arc arc2)
                {
                    return arc.Center.Equals(arc2.Center) && arc.Radius.Equals(arc2.Radius) && arc.InitialRayPoint.Equals(arc2.InitialRayPoint) && arc.FinalRayPoint.Equals(arc2.FinalRayPoint);
                }
                else if (this is Circle circle && figure is Circle circle2)
                {
                    return circle.Center.Equals(circle2.Center) && circle.Radius.Equals(circle2.Radius);
                }
                else if (this is Line line && figure is Line line2)
                {
                    return line.P1.Equals(line2.P1) && line.P2.Equals(line2.P2);
                }
                else if (this is Point point && figure is Point point2)
                {
                    return point.X.Equals(point2.X) && point.Y.Equals(point2.Y);
                }
                else if (this is Segment segment && figure is Segment segment2)
                {
                    return segment.P1.Equals(segment2.P1) && segment.P2.Equals(segment2.P2);
                }
                else if (this is Ray ray && figure is Ray ray2)
                {
                    return ray.P1.Equals(ray2.P1) && ray.P2.Equals(ray2.P2);
                }
                else
                {
                    throw new GSharpError(ErrorType.RUNTIME, $"Unknown figure type: {this.GetType().Name}");
                }
            }
            else return false;
        }
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
