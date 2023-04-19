namespace TEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textbox = new System.Windows.Forms.RichTextBox();
            this.WindowBarPanel = new System.Windows.Forms.Panel();
            this.fullscreenB = new System.Windows.Forms.Button();
            this.minimizeB = new System.Windows.Forms.Button();
            this.exitB = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.WindowBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textbox
            // 
            this.textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textbox.Font = new System.Drawing.Font("Tahoma", 9F);
            this.textbox.Location = new System.Drawing.Point(5, 35);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(790, 410);
            this.textbox.TabIndex = 0;
            this.textbox.Text = "";
            this.textbox.TextChanged += new System.EventHandler(this.textbox_TextChanged);
            // 
            // WindowBarPanel
            // 
            this.WindowBarPanel.Controls.Add(this.label1);
            this.WindowBarPanel.Controls.Add(this.button4);
            this.WindowBarPanel.Controls.Add(this.button3);
            this.WindowBarPanel.Controls.Add(this.button2);
            this.WindowBarPanel.Controls.Add(this.button1);
            this.WindowBarPanel.Controls.Add(this.fullscreenB);
            this.WindowBarPanel.Controls.Add(this.minimizeB);
            this.WindowBarPanel.Controls.Add(this.exitB);
            this.WindowBarPanel.Location = new System.Drawing.Point(0, 0);
            this.WindowBarPanel.Name = "WindowBarPanel";
            this.WindowBarPanel.Size = new System.Drawing.Size(800, 30);
            this.WindowBarPanel.TabIndex = 1;
            // 
            // fullscreenB
            // 
            this.fullscreenB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fullscreenB.FlatAppearance.BorderSize = 0;
            this.fullscreenB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fullscreenB.Location = new System.Drawing.Point(755, 8);
            this.fullscreenB.Name = "fullscreenB";
            this.fullscreenB.Size = new System.Drawing.Size(15, 15);
            this.fullscreenB.TabIndex = 2;
            this.fullscreenB.TabStop = false;
            this.fullscreenB.UseVisualStyleBackColor = false;
            this.fullscreenB.Click += new System.EventHandler(this.fullscreenB_Click);
            // 
            // minimizeB
            // 
            this.minimizeB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.minimizeB.FlatAppearance.BorderSize = 0;
            this.minimizeB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeB.Location = new System.Drawing.Point(735, 8);
            this.minimizeB.Name = "minimizeB";
            this.minimizeB.Size = new System.Drawing.Size(15, 15);
            this.minimizeB.TabIndex = 1;
            this.minimizeB.TabStop = false;
            this.minimizeB.UseVisualStyleBackColor = false;
            this.minimizeB.Click += new System.EventHandler(this.minimizeB_Click);
            // 
            // exitB
            // 
            this.exitB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.exitB.FlatAppearance.BorderSize = 0;
            this.exitB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitB.Location = new System.Drawing.Point(775, 8);
            this.exitB.Name = "exitB";
            this.exitB.Size = new System.Drawing.Size(15, 15);
            this.exitB.TabIndex = 0;
            this.exitB.TabStop = false;
            this.exitB.UseVisualStyleBackColor = false;
            this.exitB.Click += new System.EventHandler(this.exitB_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button1.Location = new System.Drawing.Point(5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "open";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button2.Location = new System.Drawing.Point(163, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 22);
            this.button2.TabIndex = 4;
            this.button2.Text = "save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button3.Location = new System.Drawing.Point(242, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 22);
            this.button3.TabIndex = 5;
            this.button3.Text = "save as";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button4.Location = new System.Drawing.Point(84, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 22);
            this.button4.TabIndex = 6;
            this.button4.Text = "close";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(404, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "words: 0 letters: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WindowBarPanel);
            this.Controls.Add(this.textbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.WindowBarPanel.ResumeLayout(false);
            this.WindowBarPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textbox;
        private System.Windows.Forms.Panel WindowBarPanel;
        private System.Windows.Forms.Button exitB;
        private System.Windows.Forms.Button minimizeB;
        private System.Windows.Forms.Button fullscreenB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
    }
}

