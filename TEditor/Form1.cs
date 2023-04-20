using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TEditor
{
    public partial class TEditor : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int LPAR);

        public TEditor()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            WindowBarPanel.MouseDown += new MouseEventHandler(move_window);
            label1.MouseDown += new MouseEventHandler(move_window);

            exitB.FlatAppearance.MouseOverBackColor = exitB.BackColor;
            exitB.BackColorChanged += (s, e) =>
            {
                exitB.FlatAppearance.MouseOverBackColor = Color.Red;
            };

            minimizeB.FlatAppearance.MouseOverBackColor = minimizeB.BackColor;
            minimizeB.BackColorChanged += (s, e) =>
            {
                minimizeB.FlatAppearance.MouseOverBackColor = Color.Gray;
            };

            fullscreenB.FlatAppearance.MouseOverBackColor = fullscreenB.BackColor;
            fullscreenB.BackColorChanged += (s, e) =>
            {
                fullscreenB.FlatAppearance.MouseOverBackColor = Color.Gray;
            };

            button6.FlatAppearance.MouseOverBackColor = button6.BackColor;
            button6.BackColorChanged += (s, e) =>
            {
                button6.FlatAppearance.MouseOverBackColor = Color.Transparent;
            };

            button7.FlatAppearance.MouseOverBackColor = button7.BackColor;
            button7.BackColorChanged += (s, e) =>
            {
                button7.FlatAppearance.MouseOverBackColor = Color.Transparent;
            };
            SetPadding(textbox, new Padding(5, 5, 5, 5));
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

        #region padding
        private const int EM_SETRECT = 0xB3;
        [DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }
        public void SetPadding(System.Windows.Forms.RichTextBox textBox, Padding padding)
        {
            var rect = new Rectangle(padding.Left, padding.Top, textBox.ClientSize.Width - padding.Left - padding.Right, textBox.ClientSize.Height - padding.Top - padding.Bottom);
            RECT rc = new RECT(rect);
            SendMessageRefRect(textBox.Handle, EM_SETRECT, 0, ref rc);
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
            exitB.BackColor = Color.Transparent;
            minimizeB.BackColor = Color.Transparent;
            fullscreenB.BackColor = Color.Transparent;

            BackColor = Color.FromArgb(15, 4, 61);
            panel1.BackColor = Color.FromArgb(17, 17, 18);
            textbox.BackColor = Color.FromArgb(24, 25, 26); 
            WindowBarPanel.BackColor = Color.FromArgb(13, 13, 13);

            Color b_color = Color.FromArgb(19, 16, 31);
            button1.BackColor = b_color;
            button2.BackColor = b_color;
            button3.BackColor = b_color;
            button4.BackColor = b_color;
            button5.BackColor = b_color;
            ForeColor = Color.FromArgb(209, 209, 209);
            textbox.ForeColor = Color.FromArgb(209, 209, 209);

            button7.BackColor = Color.Transparent;
            button6.BackColor = Color.Transparent;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int x = Size.Width;
            int y = Size.Height;

            textbox.Width = x-20;
            textbox.Height = y-70;
            panel1.Width = x - 4; 
            panel1.Height = y-4;
            label1.Location = new Point(label1.Location.X, panel1.Height - 18);

            WindowBarPanel.Width = x-4;
            exitB.Location = new Point(x-30, exitB.Location.Y);
            fullscreenB.Location = new Point(exitB.Location.X - 23, fullscreenB.Location.Y);
            minimizeB.Location = new Point(fullscreenB.Location.X - 23, minimizeB.Location.Y);
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
            char[] chars = new char[] { '\n', ' '};
            words = text.Split(chars).Length;
            if (text.Length == 0) words = 0;

            int lines = text.Split('\n').Length;

            label1.Text = "WORDS: " + words + " LETTERS: " + letters + " LINES: " + lines;
        }

        bool onTop = false;
        private void button5_Click(object sender, EventArgs e)
        {
            onTop = !onTop;
            if (onTop == true)
            {
                TEditor.ActiveForm.TopMost = true;
                this.TopMost = true;
            }else
            {
                TEditor.ActiveForm.TopMost = false;
                this.TopMost = false;
            }
        }

        private void TEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            string textbox_content = textbox.Text;
            if(textbox_content != "")
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save your note?", "TEditor", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(Openedfilepath))
                    {
                        File.WriteAllText(Openedfilepath, textbox_content);
                    }
                    else
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Text Files|*.txt";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(Path.GetFullPath(saveFileDialog.FileName), textbox_content);
                        }
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textbox.Undo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textbox.Redo();
        }
    }
}