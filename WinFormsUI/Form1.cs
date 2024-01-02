using GSharpInterpreter;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WinFormsUI
{
    public partial class Form1 : Form, IUserInterface
    {
        
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            panel1.Refresh();
            string code = richTextBox1.Text;
            Interpreter.Execute(code, this);
        }
        
        private void panel_Paint()
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 2);
            // Dibujar líneas y círculos según el texto ingresado
            // Aquí puedes implementar la lógica para interpretar el texto y dibujar las formas correspondientes

            // Ejemplo: Dibujar una línea diagonal
            g.DrawLine(pen, 0, 0, panel1.Width, panel1.Height);

            // Ejemplo: Dibujar un círculo
            int radius = 50;
            int x = panel1.Width / 2 - radius;
            int y = panel1.Height / 2 - radius;
            g.DrawEllipse(Pens.Red, x, y, 2 * radius, 2 * radius);

            Brush brush = new SolidBrush(Color.DarkRed);
            Font font = new Font("Arial", 5);
            g.DrawString("punto A", font, brush, 100, 40);
            g.DrawEllipse(pen, 100, 40, 4, 4);
        }
        private Color GetColor(GSharpColor color)
        {
            switch (color)
            {
                case GSharpColor.BLACK:
                    return Color.Black;
                case GSharpColor.BLUE:
                    return Color.Blue;
                case GSharpColor.CYAN:
                    return Color.Cyan;
                case GSharpColor.GRAY:
                    return Color.Gray;
                case GSharpColor.GREEN:
                    return Color.Green;
                case GSharpColor.MAGENTA:
                    return Color.Magenta;
                case GSharpColor.RED:
                    return Color.Red;
                case GSharpColor.WHITE:
                    return Color.White;
                case GSharpColor.YELLOW:
                    return Color.Yellow;
                default:
                    return Color.Black;
            }
        }
        public int CanvasWidth => panel1.Size.Width;

        public int CanvasHeight => panel1.Size.Height;

        public void DrawPoint(GSharpInterpreter.Point point, GSharpColor color, string label = "")
        {
            using (Graphics graph = panel1.CreateGraphics())
            using (Pen pen = new Pen(GetColor(color), 4F))
            {
                graph.DrawEllipse(pen, (float)point.X, (float)point.Y, 2F, 2F);
            }
            if (label != "")
            {
                DrawText(label, point, color, point);
            }

        }

        public void DrawLine(Line line, GSharpColor color, string label = "")
        {
            //el objeto linea ya viene con pendiente.
            if (line.P1.X == line.P2.X) //pendiente infinita
            {
                // Dibujar una línea vertical con coordenada X constante
                float x = (float)line.P1.X;
                using (Graphics graphics = panel1.CreateGraphics())
                using (Pen pen = new Pen(GetColor(color)))
                {
                    graphics.DrawLine(pen, x, 0, x, CanvasWidth);
                }

            }
            else
            {

              float m = GetSlope(line.P1, line.P2);
              float b = (float)line.P1.Y - (float)(m * line.P1.X);

              Point point1 = new Point(0, (int)b);
              Point point2 = new Point(CanvasWidth, (int)(m * CanvasWidth + b));

              using (Graphics graphics = panel1.CreateGraphics())
              using (Pen pen = new Pen(GetColor(color)))
              {
                graphics.DrawLine(pen, (float)point1.X, (float)point1.Y, (float)point2.X, (float)point2.Y);
              }
            }
            if (label != "")
            {
                DrawText(label, line.P1, color, line);
            }
        }

        public void DrawSegment(Segment segment, GSharpColor color, string label = "")
        {
            using (Graphics graph = panel1.CreateGraphics())
            using (Pen pen = new Pen(GetColor(color), 4F))
            {
                graph.DrawLine(pen, (float)segment.P1.X, (float)segment.P1.Y, (float)segment.P2.X, (float)segment.P2.Y);
            }
            if (label != "")
            {
                DrawText(label, segment.P1, color, segment);
            }
        }

        public void DrawRay(Ray ray, GSharpColor color, string label = "")
        {   using (Pen pen = new Pen(GetColor(color)))
            using (Graphics graphics = panel1.CreateGraphics())
            {

            if (ray.P1.X == ray.P2.X)
            {
                    //pendiente infnita, rayo vertical
                    if (ray.P1.Y > ray.P2.Y)
                    {
                        float x = (float)ray.P1.X;
                        float y = 0;
                        graphics.DrawLine(pen, (float)ray.P1.X, (float)ray.P1.Y, x, y);
                    }
                    else if (ray.P1.Y < ray.P2.Y)
                    {
                        float x = (float)ray.P1.X;
                        float y = CanvasHeight;
                        graphics.DrawLine(pen, (float)ray.P1.X, (float)ray.P1.Y, x, y);
                    }
                    else
                    {
                        DrawPoint(ray.P1, color);
                    }

            }
            else // rayo normal
            {
                float m = GetSlope(ray.P1, ray.P2);

                // Si el punto final está a la derecha del punto inicial
                if (ray.P2.X >= ray.P1.X)
                {
                    float x = CanvasWidth; // Coordenada X en el borde del PictureBox
                    float y = (float)(ray.P1.Y + m * (x - ray.P1.X)); // Calcular Y correspondiente al borde del PictureBox
                    graphics.DrawLine(pen, (float)ray.P1.X,(float)ray.P1.Y, x, y); // Dibujar el rayo
                }
                // Si el punto final está a la izquierda del punto inicial
                else
                {
                    float x = 0; // Coordenada X en el borde del PictureBox
                    float y = (float)(ray.P1.Y + m * (x - ray.P1.X)); // Calcular Y correspondiente al borde del PictureBox
                    graphics.DrawLine(pen,(float)ray.P1.X, (float)ray.P1.Y, x, y); // Dibujar el rayo
                }
            }
                if (label != "")
                {
                    DrawText(label, ray.P1, color, ray);
                }
            }
        }

        public void DrawCircle(Circle circle, GSharpColor color, string label = "")
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                Pen pen = new Pen(GetColor(color));
                g.DrawEllipse(pen, (float)circle.Center.X - (float)circle.Radius.Value, (float)(circle.Center.Y - circle.Radius.Value), (float)circle.Radius.Value * 2, (float)circle.Radius.Value * 2);
            }
            if (label != "")
            {
                DrawText(label, circle.Center, color,circle);
            }
        }

        public void DrawArc(Arc arc, GSharpColor color, string label = "")
        {
            using (Graphics graphics = panel1.CreateGraphics())
            using (Pen pen = new Pen(GetColor(color)))
            {   if (arc.InitialRayPoint.Y == arc.FinalRayPoint.Y && arc.InitialRayPoint.X == arc.FinalRayPoint.X)
                {
                    Circle circle = new Circle(arc.Center, arc.Radius);
                    DrawCircle(circle, color);
                }
                else
                {

                    float x = (float)(arc.Center.X - arc.Radius.Value);
                    float y = (float)(arc.Center.Y - arc.Radius.Value);
                    float diametro = (float)(2 * arc.Radius.Value);

                    // Calcula el ángulo inicial y el ángulo de extensión basado en los puntos de inicio y finalización
                    float startAngle = (float)Math.Atan2(arc.InitialRayPoint.Y - arc.Center.Y, arc.InitialRayPoint.X - arc.Center.X) * 180 / (float)Math.PI;
                    float endAngle = (float)Math.Atan2(arc.FinalRayPoint.Y - arc.Center.Y, arc.FinalRayPoint.X - arc.Center.X) * 180 / (float)Math.PI;

                    if (startAngle > endAngle)
                        endAngle += 360;
                    float sweepAngle = endAngle - startAngle;
                    sweepAngle = sweepAngle % 360;

                    // Dibuja el arco utilizando el método DrawArc
                    graphics.DrawArc(pen, x, y, diametro, diametro, startAngle, sweepAngle);
                }
            }
            if (label != "")
            {
                DrawText(label, arc.Center, color, arc);
            }
        }

        public void DrawText(string text, GSharpInterpreter.Point point, GSharpColor color, GSharpFigure figure)
        {

                if (figure is Circle || figure is Arc)
                {
                    DrawTextForCirle(text, point, color, (Circle)figure);
                }

            using (Graphics graphics = panel1.CreateGraphics())
            using (Pen pen = new Pen(GetColor(color)))
            {
                int offset = 10; // Espacio entre el texto y la figura
                graphics.DrawString(text, new Font("Arial", 5), Brushes.Black, (float)(point.X + offset), (float)(point.Y + offset));
            }
        }
        void DrawTextForCirle(string text, GSharpInterpreter.Point point, GSharpColor color, Circle circle)
        { 
            using (Graphics graphics = panel1.CreateGraphics())
            using (Pen pen = new Pen(GetColor(color)))
            {
                int offset = 10; // Espacio entre el texto y el círculo
                graphics.DrawString(text, new Font("Arial", 5), Brushes.Black, (float)(point.X + circle.Radius.Value + offset), (float)(point.Y - Font.Height / 2));
            }
        }
        public void Print(string text)
        {
            richTextBox2.Text += text + "\n";
        }

        public void ReportError(string message)
        {
            richTextBox2.Text += message + "\n";
        }
        private float GetSlope(GSharpInterpreter.Point p1, GSharpInterpreter.Point p2)
        {
            return (float)((p2.Y - p1.Y) / (p2.X - p1.X));
        }
    }
}
