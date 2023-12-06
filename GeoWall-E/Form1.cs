using G__Interpreter;

namespace GeoWall_E
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = richTextBox1.Text;
            Interpreter interpreter = new Interpreter();
            List<object> results = interpreter.Run(code);
            foreach (object result in results)
            {
                richTextBox2.Text += result.ToString() + "\n";
            }
        }
    }
}
