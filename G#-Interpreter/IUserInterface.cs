using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace G__Interpreter
{
    public interface IUserInterface
    {
        int CanvasWidth { get; }
        int CanvasHeight { get; }
        void DrawPoint(Figure.Point point, Color color);
        void DrawLine(Figure.Line line, Color color);
        void DrawSegment(Figure.Segment segment, Color color);
        void DrawRay(Figure.Ray ray, Color color);
        void DrawCircle(Figure.Circle circle, Color color);
        void DrawArc(Figure.Arc arc, Color color);
        void DrawText(string text, Figure.Point point, Color color);
        void Print(List<string> text);
        void ReportError(string message);
        void Draw(Figure figure);
    }
}
