using GSharpInterpreter;

namespace GeoWall_E
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string code = richTextBox1.Text;
            Interpreter.Execute(code, new UI());
        }
        private void panel_Paint()
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Red, 2);

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
    }
    public class UI : IUserInterface
    {
        public int CanvasWidth => throw new NotImplementedException();

        public int CanvasHeight => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }

        public void DrawRay(Ray ray, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawSegment(Segment segment, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void DrawText(string text, GSharpInterpreter.Point point, GSharpColor color)
        {
            throw new NotImplementedException();
        }

        public void Print(string text)
        {
            throw new NotImplementedException();
        }

        public void ReportError(string message)
        {
            throw new NotImplementedException();
        }
    }
}
