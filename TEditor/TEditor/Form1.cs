using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TEditor
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int LPAR);

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            WindowBarPanel.MouseDown += new MouseEventHandler(move_window);
            label1.MouseDown += new MouseEventHandler(move_window);
        }
        #region resizing_window
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Transparent, Top());
            e.Graphics.FillRectangle(Brushes.Transparent, Left());
            e.Graphics.FillRectangle(Brushes.Transparent, Right());
            e.Graphics.FillRectangle(Brushes.Transparent, Bottom());
        }

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {
                var mp = this.PointToClient(Cursor.Position);

                if (TopLeft().Contains(mp))
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight().Contains(mp))
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft().Contains(mp))
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight().Contains(mp))
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (Top().Contains(mp))
                    m.Result = (IntPtr)HTTOP;
                else if (Left().Contains(mp))
                    m.Result = (IntPtr)HTLEFT;
                else if (Right().Contains(mp))
                    m.Result = (IntPtr)HTRIGHT;
                else if (Bottom().Contains(mp))
                    m.Result = (IntPtr)HTBOTTOM;
            }
        }

        private Random rng = new Random();
        public Color randomColour()
        {
            return Color.FromArgb(255, rng.Next(255), rng.Next(255), rng.Next(255));
        }

        const int ImaginaryBorderSize = 2;

        public new Rectangle Top()
        {
            return new Rectangle(0, 0, this.ClientSize.Width, ImaginaryBorderSize);
        }

        public new Rectangle Left()
        {
            return new Rectangle(0, 0, ImaginaryBorderSize, this.ClientSize.Height);
        }

        public new Rectangle Bottom()
        {
            return new Rectangle(0, this.ClientSize.Height - ImaginaryBorderSize, this.ClientSize.Width, ImaginaryBorderSize);
        }

        public new Rectangle Right()
        {
            return new Rectangle(this.ClientSize.Width - ImaginaryBorderSize, 0, ImaginaryBorderSize, this.ClientSize.Height);
        }

        public Rectangle TopLeft()
        {
            return new Rectangle(0, 0, ImaginaryBorderSize, ImaginaryBorderSize);
        }

        public Rectangle TopRight()
        {
            return new Rectangle(this.ClientSize.Width - ImaginaryBorderSize, 0, ImaginaryBorderSize, ImaginaryBorderSize);
        }

        public Rectangle BottomLeft()
        {
            return new Rectangle(0, this.ClientSize.Height - ImaginaryBorderSize, ImaginaryBorderSize, ImaginaryBorderSize);
        }

        public Rectangle BottomRight()
        {
            return new Rectangle(this.ClientSize.Width - ImaginaryBorderSize, this.ClientSize.Height - ImaginaryBorderSize, ImaginaryBorderSize, ImaginaryBorderSize);
        }
        #endregion
        private void move_window(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                PostMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowBarPanel.BackColor = Color.Gray;

            exitB.BackColor = Color.Red;
            minimizeB.BackColor = Color.Yellow;
            fullscreenB.BackColor = Color.Blue;

            BackColor = Color.FromArgb(24, 25, 26);
            textbox.BackColor = Color.FromArgb(34, 37, 41);
            WindowBarPanel.BackColor = Color.FromArgb(13, 13, 13);

            Color b_color = Color.FromArgb(27, 42, 56);
            button1.BackColor = b_color;
            button2.BackColor = b_color;
            button3.BackColor = b_color;
            button4.BackColor = b_color;
            ForeColor = Color.FromArgb(214, 214, 214);
            textbox.ForeColor = Color.FromArgb(214, 214, 214);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int x = Size.Width;
            int y = Size.Height;

            textbox.Width = x-10;
            textbox.Height = y-40;

            WindowBarPanel.Width = x;
            exitB.Location = new Point(x-25, exitB.Location.Y);
            fullscreenB.Location = new Point(exitB.Location.X - 20, fullscreenB.Location.Y);
            minimizeB.Location = new Point(fullscreenB.Location.X - 20, minimizeB.Location.Y);
        }
        private void exitB_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void minimizeB_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void fullscreenB_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            else WindowState = FormWindowState.Maximized;
        }

        string Openedfilepath;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var file = openFileDialog.OpenFile();
                string fileName = openFileDialog.FileName;
                if (Path.GetExtension(fileName) == ".txt")
                {
                    string file_content = File.ReadAllText(fileName);
                    textbox.Text = file_content;
                    Openedfilepath = Path.GetFullPath(fileName);
                    file.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string textbox_content = textbox.Text;
            if (File.Exists(Openedfilepath))
            {
                File.WriteAllText(Openedfilepath, textbox_content);
            } else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files|*.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), textbox_content);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string textbox_content = textbox.Text;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), textbox_content);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Openedfilepath = "";
            textbox.Text = "";
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            string text = textbox.Text;

            int letters = text.Length - text.Split(' ').Length + 1 - text.Split('\n').Length + 1;

            int words = 0;
            text = text.Trim();
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }
            words = text.Split(' ').Length;
            if (text.Length == 0) words = 0;

            label1.Text = "words: " + words + " letters: " + letters;
        }
    }
}
