using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharpInterpreter
{
    public abstract class GeometricExpression : Expression
    {
    }
    public class MeasureDeclaration : GeometricExpression
    {
        public Expression Value { get; }
        public MeasureDeclaration(Expression value)
        {
            Value = value;
        }
    }
    public class PointDeclaration : GeometricExpression
    {
        public Expression X { get; }
        public Expression Y { get; }
        public PointDeclaration(Expression x, Expression y)
        {
            X = x;
            Y = y;
        }
    }
    public class LineDeclaration : GeometricExpression
    {
        public Expression P1 { get; }
        public Expression P2 { get; }
        public LineDeclaration(Expression p1, Expression p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class RayDeclaration : GeometricExpression
    {
        public Expression P1 { get; }
        public Expression P2 { get; }
        public RayDeclaration(Expression p1, Expression p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class SegmentDeclaration : GeometricExpression
    {
        public Expression P1 { get; }
        public Expression P2 { get; }
        public SegmentDeclaration(Expression p1, Expression p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
    public class CircleDeclaration : GeometricExpression
    {
        public Expression Center { get; }
        public Expression Radius { get; }
        public CircleDeclaration(Expression center, Expression radius)
        {
            Center = center;
            Radius = radius;
        }
    }
    public class ArcDeclaration : GeometricExpression
    {
        public Expression Center { get; }
        public Expression Radius { get; }
        public Expression InitialRayPoint { get; }
        public Expression FinalRayPoint { get; }
        public ArcDeclaration(Expression center, Expression radius, Expression initialRayPoint, Expression finalRayPoint)
        {
            Center = center;
            Radius = radius;
            InitialRayPoint = initialRayPoint;
            FinalRayPoint = finalRayPoint;
        }
    }
}
