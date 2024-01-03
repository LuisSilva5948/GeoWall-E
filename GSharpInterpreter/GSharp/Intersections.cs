using GSharpInterpreter;
using System.Diagnostics;
using System.Runtime.InteropServices;

public static class Intersections
{
    public static object Intersect(GSharpFigure figure1, GSharpFigure figure2)
    {
        switch(figure1)
        {
            case Point point1:
                switch(figure2)
                {
                    case Point point2:
                        return Intersect(point1, point2);
                    case Line line2:
                        return Intersect(point1, line2);
                    case Segment segment2:
                        return Intersect(point1, segment2);
                    case Ray ray2:
                        return Intersect(point1, ray2);
                    case Arc arc2:
                        return Intersect(point1, arc2);
                    case Circle circle2:
                        return Intersect(point1, circle2);
                    default:
                        return new Undefined();
                }
            case Line line1:
                switch(figure2)
                {
                    case Point point2:
                        return Intersect(point2, line1);
                    case Line line2:
                        return Intersect(line1, line2);
                    case Segment segment2:
                        return Intersect(line1, segment2);
                    case Ray ray2:
                        return Intersect(line1, ray2);
                    case Arc arc2:
                        return Intersect(line1, arc2);
                    case Circle circle2:
                        return Intersect(line1, circle2);
                    default:
                        return new Undefined();
                }
            case Segment segment1:
                switch(figure2)
                {
                    case Point point2:
                        return Intersect(point2, segment1);
                    case Line line2:
                        return Intersect(line2, segment1);
                    case Segment segment2:
                        return Intersect(segment1, segment2);
                    case Ray ray2:
                        return Intersect(ray2, segment1);
                    case Arc arc2:
                        return Intersect(segment1, arc2);
                    case Circle circle2:
                        return Intersect(segment1, circle2);
                    default:
                        return new Undefined();
                }
            case Ray ray1:
                switch(figure2)
                {
                    case Point point2:
                        return Intersect(point2, ray1);
                    case Line line2:
                        return Intersect(line2, ray1);
                    case Segment segment2:
                        return Intersect(ray1, segment2);
                    case Ray ray2:
                        return Intersect(ray1, ray2);
                    case Arc arc2:
                        return Intersect(ray1, arc2);
                    case Circle circle2:
                        return Intersect(ray1, circle2);
                    default:
                        return new Undefined();
                }
            case Circle circle1:
                switch (figure2)
                {
                    case Point point2:
                        return Intersect(point2, circle1);
                    case Line line2:
                        return Intersect(line2, circle1);
                    case Segment segment2:
                        return Intersect(segment2, circle1);
                    case Ray ray2:
                        return Intersect(ray2, circle1);
                    case Arc arc2:
                        return Intersect(arc2, circle1);
                    case Circle circle2:
                        return Intersect(circle1, circle2);
                    default:
                        return new Undefined();
                }
            case Arc arc1:
                switch (figure2)
                {
                    case Point point2:
                        return Intersect(point2, arc1);
                    case Line line2:
                        return Intersect(line2, arc1);
                    case Segment segment2:
                        return Intersect(segment2, arc1);
                    case Ray ray2:
                        return Intersect(ray2, arc1);
                    case Arc arc2:
                        return Intersect(arc1, arc2);
                    case Circle circle2:
                        return Intersect(arc1, circle2);
                    default:
                        return new Undefined();
                }
        }
        return new Undefined();
    }

    #region Point Intersections
    public static object Intersect(Point point1, Point point2)
    {
        if (point1.Equals(point2))
            return new FiniteSequence(new List<Expression>() { point1 });
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Point point, Line recta)
    {
        var (m, n) = GetLineEquation(recta);
        if (point.Y == (m * point.X + n))
            return new FiniteSequence(new List<Expression>() { point });
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Point point, Segment segment)
    {
        if (IsInSegment(point, segment))
            return new FiniteSequence(new List<Expression>() { point });
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Point point, Ray ray)
    {
        if (IsInRay(point, ray))
            return new FiniteSequence(new List<Expression>() { point });
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Point point, Arc arc)
    {
        if (IsInArc(point, arc))
            return new FiniteSequence(new List<Expression>() { point });
        return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Point point, Circle circle)
    {
        double distance = Math.Pow(circle.Center.X - point.X, 2) + Math.Pow(circle.Center.Y - point.Y, 2);
        if (distance == Math.Pow(circle.Radius.Value, 2))
            return new FiniteSequence(new List<Expression>() { point });
        return new FiniteSequence(new List<Expression>());
    }
    #endregion

    #region Line Intersections
    public static object Intersect(Line line, Point point)
    {
        return Intersect(point, line);
    }
    public static object Intersect(Line line1, Line line2)
    {
        var (m1, n1) = GetLineEquation(line1);
        var (m2, n2) = GetLineEquation(line2);
        if (m1 == m2 && n1 == n2) // Lines are the same
            return new Undefined();
        else if (m1 == m2) // Lines are parallel
            return new FiniteSequence(new List<Expression>());
        else // 2x2 system of equations
        {
            double x1 = line1.P1.X;
            double y1 = line1.P1.Y;
            double x2 = line1.P2.X;
            double y2 = line1.P2.Y;

            double x3 = line2.P1.X;
            double y3 = line2.P1.Y;
            double x4 = line2.P2.X;
            double y4 = line2.P2.Y;
            double denominador = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            double x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominador;
            double y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominador;
            return new FiniteSequence(new List<Expression>() { new Point(x, y) });
        }
    }
    public static object Intersect(Line line, Segment segment)
    {
        Line newLine = new Line(segment.P1, segment.P2);
        object possibleIntersections = Intersect(line, newLine);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections)
            {
                if (!IsInSegment((Point)point, segment))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        // Line and segment overlap
        else return new Undefined();
    }
    public static object Intersect(Line line, Ray ray)
    {
        Line newLine = new Line(ray.P1, ray.P2);
        object possibleIntersections = Intersect(line, newLine);

        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections.ToList())
            {
                if (!IsInRay((Point)point, ray))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        // Line and ray overlap
        else return new Undefined();
    }

    public static object Intersect(Line line, Circle circle)
    {
        var (m, c) = GetLineEquation(line);
        var center = circle.Center;
        var radius = circle.Radius.Value;

        double centerX = center.X;
        double centerY = center.Y;

        // Discriminant equation: b^2 -4*a*c.
        double A = 1 + Math.Pow(m, 2);
        double B = 2 * (m * c - m * centerY - centerX);
        double C = Math.Pow(centerY, 2) - Math.Pow(radius, 2) + Math.Pow(centerX, 2) - 2 * c * centerY + Math.Pow(c, 2);

        double discriminant = Math.Pow(B, 2) - 4 * A * C;
        // Distance from center to line
        double distance = DistancePointLine(center, line);

        List<Expression> intersections = new List<Expression>();
        if (distance > radius)
        {
            return new FiniteSequence(intersections);
        }
        else if (distance == radius)
        {
            double x = (-B) / (2 * A);
            double y = m * x + c;
            intersections.Add(new Point(x, y));
        }
        else if (discriminant >= 0)
        {
            double x1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
            double x2 = (-B - Math.Sqrt(discriminant)) / (2 * A);

            double y1 = m * x1 + c;
            double y2 = m * x2 + c;

            intersections.Add(new Point(x1, y1));
            intersections.Add(new Point(x2, y2));
        }
        return new FiniteSequence(intersections);
    }
    public static object Intersect(Line line, Arc arc)
    {
        Circle circle = new Circle(arc.Center, arc.Radius);
        object possibleIntersections = Intersect(line, circle);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections)
            {
                if (!IsInArc((Point)point, arc))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new Undefined();
    }
    #endregion

    #region Ray Intersections
    public static object Intersect(Ray ray, Point point)
    {
        return Intersect(point, ray);
    }
    public static object Intersect(Ray ray, Line line)
    {
        return Intersect(line, ray);
    }
    public static object Intersect(Ray ray1, Ray ray2)
    {
        Line line1 = new Line(ray1.P1, ray1.P2);
        Line line2 = new Line(ray2.P1, ray2.P2);
        object possibleIntersections = Intersect(line1, line2);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections.ToList())
            {
                if (!IsInRay((Point)point, ray1) || !IsInRay((Point)point, ray2))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        // Rays overlap
        else return new Undefined();
    }

    public static object Intersect(Ray ray, Circle circle)
    {
        Line line = new Line(ray.P1, ray.P2);
        object possibleIntersections = Intersect(line, circle);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections.ToList())
            {
                if (!IsInRay((Point)point, ray))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new FiniteSequence(new List<Expression>());
    }

    public static object Intersect(Ray rayo, Arc arco)
    {
        Line line = new Line(rayo.P1, rayo.P2);
        object possibleIntersections = Intersect(line, arco);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections)
            {
                if (!IsInRay((Point)point, rayo))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Ray ray, Segment segment)
    {
        Line line = new Line(ray.P1, ray.P2);
        object possibleIntersections = Intersect(line, segment);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections)
            {
                if (!IsInRay((Point)point, ray))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new FiniteSequence(new List<Expression>());
    }
    #endregion

    #region Segment Intersections

    public static object Intersect(Segment segment, Point point)
    {
        return Intersect(point, segment);
    }
    public static object Intersect(Segment segment, Line line)
    {
        return Intersect(line, segment);
    }
    public static object Intersect(Segment segment, Ray ray)
    {
        return Intersect(ray, segment);
    }
    public static object Intersect(Segment segment1, Segment segment2)
    {
        Line line1 = new Line(segment1.P1, segment1.P2);
        Line line2 = new Line(segment2.P1, segment2.P2);
        object possibleIntersections = Intersect(line1, line2);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections.ToList())
            {
                if (!IsInSegment((Point)point, segment1) || !IsInSegment((Point)point, segment2))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        // Segments overlap
        else return new Undefined();
    }

    public static object Intersect(Segment segment, Circle circle)
    {
        Line line = new Line(segment.P1, segment.P2);
        object possibleIntersections = Intersect(line, circle);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections.ToList())
            {
                if (!IsInSegment((Point)point, segment))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new FiniteSequence(new List<Expression>());
    }
    public static object Intersect(Segment segment, Arc arc)
    {
        Circle circle = new Circle(arc.Center, arc.Radius);
        object possibleIntersections = Intersect(segment, circle);
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            foreach (var point in intersections)
            {
                if (!IsInArc((Point)point, arc))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        else return new FiniteSequence(new List<Expression>());
    }
    #endregion

    #region Circle Intersections
    public static object Intersect(Circle circle, Point point)
    {
        return Intersect(point, circle);
    }
    public static object Intersect(Circle circle, Line line)
    {
        return Intersect(line, circle);
    }
    public static object Intersect(Circle circle, Ray ray)
    {
        return Intersect(ray, circle);
    }
    public static object Intersect(Circle circle, Segment segment)
    {
        return Intersect(segment, circle);
    }
    public static object Intersect(Circle circle1, Circle circle2)
    {
        List<Expression> intersections = new List<Expression>();

        double x1 = circle1.Center.X;
        double y1 = circle1.Center.Y;
        double r1 = circle1.Radius.Value;
        double x2 = circle2.Center.X;
        double y2 = circle2.Center.Y;
        double r2 = circle2.Radius.Value;
        double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        if (circle1.Equals(circle2))
            return new Undefined();
        else if (distance > r1 + r2 || distance < Math.Abs(r1 - r2) || distance == 0)
            return new FiniteSequence(new List<Expression>()); // Circles do not intersect cause they are too far apart or one is contained in the other
        else if (distance == r1 + r2 || distance == Math.Abs(r1 - r2))
        {
            // Tangent circles that intersect in one point
            double x = (r1 * x2 + r2 * x1) / (r1 + r2);
            double y = (r1 * y2 + r2 * y1) / (r1 + r2);
            intersections.Add(new Point(x, y));
        }
        else
        {
            // Circles intersect in two points
            double a = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(distance, 2)) / (2 * distance);
            double h = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(a, 2));

            double x = x1 + (a * (x2 - x1)) / distance;
            double y = y1 + (a * (y2 - y1)) / distance;

            double intersectX1 = x + (h * (y2 - y1)) / distance;
            double intersectY1 = y - (h * (x2 - x1)) / distance;

            double intersectX2 = x - (h * (y2 - y1)) / distance;
            double intersectY2 = y + (h * (x2 - x1)) / distance;

            intersections.Add(new Point(intersectX1, intersectY1));
            intersections.Add(new Point(intersectX2, intersectY2));
        }
        return new FiniteSequence(intersections);
    }
    public static object Intersect(Circle circle, Arc arc)
    {
        Circle c = new Circle(arc.Center, arc.Radius);
        object possibleIntersections = Intersect(circle, c);
        // Circle and arc do not overlap
        if (possibleIntersections is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)possibleIntersections;
            List<Expression> intersections = seq.GetElements();
            // Remove intersections that are not in the arc
            foreach (var point in intersections)
            {
                if (!IsInArc((Point)point, arc))
                {
                    intersections.Remove(point);
                }
            }
            return new FiniteSequence(intersections);
        }
        // Circle and arc overlap
        else return new Undefined();
    }
    #endregion

    #region Arc Intersections
    public static object Intersect(Arc arc, Point point)
    {
        return Intersect(point, arc);
    }
    public static object Intersect(Arc arc, Line line)
    {
        return Intersect(line, arc);
    }
    public static object Intersect(Arc arc, Ray ray)
    {
        return Intersect(ray, arc);
    }
    public static object Intersect(Arc arc, Segment segment)
    {
        return Intersect(segment, arc);
    }
    public static object Intersect(Arc arc, Circle circle)
    {
        return Intersect(circle, arc);
    }
    public static object Intersect(Arc arc1, Arc arc2)
    {
        Circle circle = new Circle(arc1.Center, arc1.Radius);
        object posibleInterseccion = Intersect(circle, arc2);
        if (posibleInterseccion is FiniteSequence)
        {
            FiniteSequence seq = (FiniteSequence)posibleInterseccion;
            List<Expression> intersecciones = seq.GetElements();
            foreach (var point in intersecciones)
            {
                if (!IsInArc((Point)point, arc1) || !IsInArc((Point)point, arc2))
                {
                    intersecciones.Remove(point);
                }
            }
            return new FiniteSequence(intersecciones);
        }
        else return new Undefined();
    }
    #endregion


    #region Auxiliary Methods
    public static double DistancePointLine(Point point, Line line)
    {
        double x1 = line.P1.X;
        double y1 = line.P1.Y;
        double x2 = line.P2.X;
        double y2 = line.P2.Y;
        double x0 = point.X;
        double y0 = point.Y;

        return Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) / Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
    }

    public static (double, double) GetLineEquation(Line line)
    {
        double m = (line.P2.Y - line.P1.Y) / (line.P2.X - line.P1.X);
        double n = line.P1.Y - m * line.P1.X;
        return (m, n);
    }
    public static bool IsInRay(Point point, Ray ray)
    {
        double rayDirectionX = ray.P2.X - ray.P1.X;
        double rayDirectionY = ray.P2.Y - ray.P1.Y;

        double pointDirectionX = point.X - ray.P1.X;
        double pointDirectionY = point.Y - ray.P1.Y;

        double crossProduct = rayDirectionX * pointDirectionY - rayDirectionY * pointDirectionX;

        if (crossProduct == 0 && (rayDirectionX * pointDirectionX + rayDirectionY * pointDirectionY >= 0))
        {
            return true;
        }

        return false;
    }
    public static bool IsInArc(Point point, Arc arc)
    {
        double pointAngle = Math.Atan2(point.Y - arc.Center.Y, point.Y - arc.Center.X);
        double startAngle = Math.Atan2(arc.InitialRayPoint.Y - arc.Center.Y, arc.InitialRayPoint.X - arc.Center.X);
        double endAngle = Math.Atan2(arc.FinalRayPoint.Y - arc.Center.Y, arc.FinalRayPoint.X - arc.Center.X);

        if (startAngle < endAngle)
        {
            return startAngle <= pointAngle && pointAngle <= endAngle;
        }
        else
        {
            return startAngle <= pointAngle || pointAngle <= endAngle;
        }
    }

    public static bool IsInSegment(Point point, Segment segment)
    {
        double minX = Math.Min(segment.P1.X, segment.P2.X);
        double maxX = Math.Max(segment.P1.X, segment.P2.X);
        double minY = Math.Min(segment.P1.Y, segment.P2.Y);
        double maxY = Math.Max(segment.P1.Y, segment.P2.Y);

        // Check if point is inside the bounding box of the segment
        if (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY)
        {
            // Parametric equation of the segment
            double t = ((point.X - segment.P1.X) * (segment.P2.X - segment.P1.X) +
                        (point.Y - segment.P1.Y) * (segment.P2.Y - segment.P1.Y)) /
                        (Math.Pow(segment.P2.X - segment.P1.X, 2) + Math.Pow(segment.P2.Y - segment.P1.Y, 2));

            // Check if point is inside the segment
            if (t >= 0 && t <= 1)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

}