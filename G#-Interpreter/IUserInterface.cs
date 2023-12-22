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
        void DrawPoint(Point point, Color color);
        void DrawLine(Line line, Color color);
        void DrawSegment(Segment segment, Color color);
        void DrawRay(Ray ray, Color color);
        void DrawCircle(Circle circle, Color color);
        void DrawArc(Arc arc, Color color);
        void DrawText(string text, Point point, Color color);
        void Print(List<string> text);
        void ReportError(string message);
        void Draw(GSharpFigure figure);
    }
}
