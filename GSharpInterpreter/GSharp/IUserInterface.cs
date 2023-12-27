using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GSharpInterpreter
{
    public interface IUserInterface
    {
        int CanvasWidth { get; }
        int CanvasHeight { get; }
        void DrawPoint(Point point, GSharpColor color);
        void DrawLine(Line line, GSharpColor color);
        void DrawSegment(Segment segment, GSharpColor color);
        void DrawRay(Ray ray, GSharpColor color);
        void DrawCircle(Circle circle, GSharpColor color);
        void DrawArc(Arc arc, GSharpColor color);
        void DrawText(string text, Point point, GSharpColor color);
        void Print(string text);
        void ReportError(string message);
    }
}
