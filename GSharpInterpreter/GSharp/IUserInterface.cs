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
        void DrawPoint(Point point, GSharpColor color, string label = "");
        void DrawLine(Line line, GSharpColor color, string label = "");
        void DrawSegment(Segment segment, GSharpColor color, string label = "");
        void DrawRay(Ray ray, GSharpColor color, string label = "");
        void DrawCircle(Circle circle, GSharpColor color, string label = "");
        void DrawArc(Arc arc, GSharpColor color, string label = "");
        void Print(string text);
        void ReportError(string message);
    }
}
