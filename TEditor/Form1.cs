﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
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

        Color b_color = Color.FromArgb(19, 16, 31);
        string Openedfilepath;

        public static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string myAppFolder = Path.Combine(appDataPath, "KUBIXQAZ/TEditor");
        public static string settingsFilePath = Path.Combine(myAppFolder, "settings.json");

        public class Settings
        {
            public string FontFamily { get; set; } = "Arial";
            public float Size { get; set; } = 12.0f;
            public FontStyle Style { get; set; } = FontStyle.Regular;
            public int x { get; set; }
            public int y { get; set; }
            public int width { get; set; }  
            public int height { get; set; }
        }

        Settings settings;

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

            settings = new Settings();
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
        public void SetPadding(RichTextBox textBox, Padding padding)
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
            try
            {

                string settingsJson = File.ReadAllText(settingsFilePath);

                if (File.Exists(settingsFilePath))
                {
                    settings = JsonConvert.DeserializeObject<Settings>(settingsJson);
                    settings = JsonConvert.DeserializeObject<Settings>(settingsJson);

                    Font loadedFont = new Font(settings.FontFamily, settings.Size, settings.Style);

                    textbox.Font = loadedFont;
                    button10.Text = settings.FontFamily + ", " + settings.Size + ", " + settings.Style;

                    SetDesktopLocation(settings.x, settings.y);
                    Size = new Size(settings.width, settings.height);
                }
            }
            catch { }

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string filePath = args[1];
                string file_content = File.ReadAllText(filePath);
                textbox.Text = file_content;
                Openedfilepath = Path.GetFullPath(filePath);
            }

            exitB.BackColor = Color.Transparent;
            minimizeB.BackColor = Color.Transparent;
            fullscreenB.BackColor = Color.Transparent;

            BackColor = Color.FromArgb(15, 4, 61);
            panel1.BackColor = Color.FromArgb(17, 17, 18);
            textbox.BackColor = Color.FromArgb(24, 25, 26);
            WindowBarPanel.BackColor = Color.FromArgb(13, 13, 13);

            button1.BackColor = b_color;
            button2.BackColor = b_color;
            button3.BackColor = b_color;
            button4.BackColor = b_color;
            button5.BackColor = b_color;
            button10.BackColor = b_color;
            ForeColor = Color.FromArgb(209, 209, 209);
            textbox.ForeColor = Color.FromArgb(209, 209, 209);

            button7.BackColor = Color.Transparent;
            button6.BackColor = Color.Transparent;
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                string textbox_content = textbox.Text;
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
                        Openedfilepath = Path.GetFullPath(saveFileDialog.FileName);
                    }
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int x = Size.Width;
            int y = Size.Height;

            textbox.Width = x - 20;
            textbox.Height = y - 70;
            panel1.Width = x - 4;
            panel1.Height = y - 4;
            label1.Location = new Point(label1.Location.X, panel1.Height - 18);

            WindowBarPanel.Width = x - 4;
            exitB.Location = new Point(x - 30, exitB.Location.Y);
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
            if (WindowState == FormWindowState.Maximized) WindowState = FormWindowState.Normal;
            else WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = Path.GetFullPath(openFileDialog.FileName);
                string file_content = File.ReadAllText(filePath);
                textbox.Text = file_content;
                Openedfilepath = Path.GetFullPath(filePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string textbox_content = textbox.Text;
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
                    Openedfilepath = Path.GetFullPath(saveFileDialog.FileName);
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
                Openedfilepath = Path.GetFullPath(saveFileDialog.FileName);
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
            char[] chars = new char[] { '\n', ' ' };
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
            }
            else
            {
                TEditor.ActiveForm.TopMost = false;
                this.TopMost = false;
            }
        }

        public static bool InTextBox(string oldString, string newString)
        {
            string oldText = oldString.Replace("\r\n", "\n");
            string newText = newString.Replace("\r\n", "\n");

            if (oldText == newText) return true;
            else return false;
        }

        private void TEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.x = Location.X;
            settings.y = Location.Y;
            settings.width = Width;
            settings.height = Height;

            settings.FontFamily = textbox.Font.FontFamily.Name.ToString();
            settings.Size = textbox.Font.Size;
            settings.Style = textbox.Font.Style;

            if (!Directory.Exists(myAppFolder))
            {
                Directory.CreateDirectory(myAppFolder);
            }

            string settingsJson = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsFilePath, settingsJson);

            string textbox_content = textbox.Text;

            if (File.Exists(Openedfilepath))
            {
                string file_content = File.ReadAllText(Openedfilepath);

                if (InTextBox(file_content, textbox_content) == false)
                {

                    DialogResult dialogResult = MessageBox.Show("Do you want to save your note?", "TEditor", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        File.WriteAllText(Openedfilepath, textbox_content);
                    }
                }
            }
            else
            {
                textbox_content = textbox_content.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                if(textbox_content != "")
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save your note?", "TEditor", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        string ext = Path.GetExtension(Openedfilepath);
                        saveFileDialog.Filter = "Text Files|" + ext;

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

        private void button10_Click(object sender, EventArgs e)
        {
            FontDialog fontdialog = new FontDialog();

            Font loadedFont = new Font(settings.FontFamily, settings.Size, settings.Style);
            Debug.WriteLine(loadedFont);
            fontdialog.Font = loadedFont; 

            if (fontdialog.ShowDialog() == DialogResult.OK)
            {
                button10.Text = fontdialog.Font.FontFamily.Name + ", " + fontdialog.Font.Size + ", " + fontdialog.Font.Style;
                textbox.Font = fontdialog.Font;

                settings.FontFamily = fontdialog.Font.FontFamily.Name.ToString();
                settings.Size = fontdialog.Font.Size;
                settings.Style = fontdialog.Font.Style;
            }
        }
    }
}