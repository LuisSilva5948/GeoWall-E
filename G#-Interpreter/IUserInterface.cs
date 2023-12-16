using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G__Interpreter
{
    internal interface IUserInterface
    {
        void DrawPoint(PointExpression point, Color color);
        void DrawLine(LineExpression line, Color color);
        void DrawSegment(SegmentExpression segment, Color color);
        void DrawCircle(CircleExpression circle, Color color);
        void DrawArc(ArcExpression arc, Color color);
        void Print(List<string> text);
        void ReportError(string message);
        void DrawGeometricExpression(GeometricExpression geometricExpression);
    }
}
