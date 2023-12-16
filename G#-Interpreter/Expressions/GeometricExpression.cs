using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    public abstract class GeometricExpression : Expression
    {
    }
    public class MeasureExpression : GeometricExpression
    {
        public double Value { get; }
        public MeasureExpression(double value)
        {
            Value = value;
        }
    }
    public class PointExpression : GeometricExpression
    {
        public Expression X { get; }
        public Expression Y { get; }
        public PointExpression(Expression x, Expression y)
        {
            X = x;
            Y = y;
        }
        public PointExpression()
        {
            Random random = new Random();
            X = new LiteralExpression(random.Next(0, 200));
            Y = new LiteralExpression(random.Next(0, 200));
        }
    }
    public class LineExpression : GeometricExpression
    {
        public PointExpression P1 { get; }
        public PointExpression P2 { get; }
        public LineExpression(PointExpression p1, PointExpression p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public LineExpression()
        {
            P1 = new PointExpression();
            P2 = new PointExpression();
        }
    }
    public class SegmentExpression : GeometricExpression
    {
        public PointExpression P1 { get; }
        public PointExpression P2 { get; }
        public SegmentExpression(PointExpression p1, PointExpression p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public SegmentExpression()
        {
            P1 = new PointExpression();
            P2 = new PointExpression();
        }
    }
    public class CircleExpression : GeometricExpression
    {
        public PointExpression Center { get; }
        public Expression Radius { get; }
        public CircleExpression(PointExpression center, Expression radius)
        {
            Center = center;
            Radius = radius;
        }
        public CircleExpression()
        {
            Center = new PointExpression();
            Random random = new Random();
            Radius = new LiteralExpression(random.Next(0, 200));
        }
    }
    public class ArcExpression : GeometricExpression
    {
        public PointExpression Center { get; }
        public Expression Radius { get; }
        public Expression StartAngle { get; }
        public Expression EndAngle { get; }
        public ArcExpression(PointExpression center, Expression radius, Expression startAngle, Expression endAngle)
        {
            Center = center;
            Radius = radius;
            StartAngle = startAngle;
            EndAngle = endAngle;
        }
        public ArcExpression()
        {
            Center = new PointExpression();
            Random random = new Random();
            Radius = new LiteralExpression(random.Next(0, 200));
            StartAngle = new LiteralExpression(random.Next(0, 360));
            EndAngle = new LiteralExpression(random.Next(0, 360));
        }
    }
}
