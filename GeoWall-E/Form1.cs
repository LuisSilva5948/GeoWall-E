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
            List<object> results = Interpreter.Run(code);
            foreach (object result in results)
            {
                richTextBox2.Text += result.ToString() + "\n";
            }
            panel_Paint();
        }
        private void panel_Paint()
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Red, 2);

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
    }
}
