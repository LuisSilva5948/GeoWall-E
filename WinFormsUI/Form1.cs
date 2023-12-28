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
            string code = richTextBox1.Text;
            Interpreter.Execute(code, this);
        }
        
        private void panel_Paint()
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 2);
            // Dibujar l�neas y c�rculos seg�n el texto ingresado
            // Aqu� puedes implementar la l�gica para interpretar el texto y dibujar las formas correspondientes

            // Ejemplo: Dibujar una l�nea diagonal
            g.DrawLine(pen, 0, 0, panel1.Width, panel1.Height);

            // Ejemplo: Dibujar un c�rculo
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

        public void DrawArc(Arc arc, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(Circle circle, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Line line, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawPoint(GSharpInterpreter.Point point, GSharpColor color)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(GetColor(color), 2);
            g.DrawEllipse(pen, (float)point.X, (float)point.Y, 4, 4);
        }

        public void DrawRay(Ray ray, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawSegment(Segment segment, GSharpColor color)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(GetColor(color), 2);
            g.DrawLine(pen, (float)segment.P1.X, (float)segment.P1.Y, (float)segment.P2.X, (float)segment.P2.Y);
        }

        public void DrawText(string text, GSharpInterpreter.Point point, GSharpColor color)
        {
            throw new NotImplementedException();
        }
        public void Print(string text)
        {
            richTextBox2.Text += text + "\n";
        }

        public void ReportError(string message)
        {
            richTextBox2.Text += message + "\n";
        }
    }
}
