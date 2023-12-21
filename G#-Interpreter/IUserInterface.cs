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
        void DrawPoint(GSharpFigure.Point point, Color color);
        void DrawLine(GSharpFigure.Line line, Color color);
        void DrawSegment(GSharpFigure.Segment segment, Color color);
        void DrawRay(GSharpFigure.Ray ray, Color color);
        void DrawCircle(GSharpFigure.Circle circle, Color color);
        void DrawArc(GSharpFigure.Arc arc, Color color);
        void DrawText(string text, GSharpFigure.Point point, Color color);
        void Print(List<string> text);
        void ReportError(string message);
        void Draw(GSharpFigure figure);
    }
}
