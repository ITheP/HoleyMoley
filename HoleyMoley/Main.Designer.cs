using System.Runtime.InteropServices;
using System;
namespace HoleyMoley
{
    // Not used but might be interesting for other alternatives -> http://stackoverflow.com/questions/4387680/transparent-background-on-winforms

    partial class Main
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(
        [MarshalAs(UnmanagedType.LPTStr)] string lpClassName,
        [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(
             IntPtr hWndChild,      // handle to window
             IntPtr hWndNewParent   // new parent window
           );
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


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
            this.CrossHairV = new System.Windows.Forms.Panel();
            this.CrossHairH = new System.Windows.Forms.Panel();
            this.Hole = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Hole)).BeginInit();
            this.SuspendLayout();
            // 
            // CrossHairV
            // 
            this.CrossHairV.BackColor = System.Drawing.Color.Black;
            this.CrossHairV.Location = new System.Drawing.Point(466, 300);
            this.CrossHairV.Name = "CrossHairV";
            this.CrossHairV.Size = new System.Drawing.Size(46, 100);
            this.CrossHairV.TabIndex = 36;
            this.CrossHairV.Visible = false;
            // 
            // CrossHairH
            // 
            this.CrossHairH.BackColor = System.Drawing.Color.Black;
            this.CrossHairH.Location = new System.Drawing.Point(389, 300);
            this.CrossHairH.Name = "CrossHairH";
            this.CrossHairH.Size = new System.Drawing.Size(46, 100);
            this.CrossHairH.TabIndex = 35;
            this.CrossHairH.Visible = false;
            // 
            // Hole
            // 
            this.Hole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Hole.Location = new System.Drawing.Point(50, 50);
            this.Hole.Name = "Hole";
            this.Hole.Size = new System.Drawing.Size(800, 600);
            this.Hole.TabIndex = 0;
            this.Hole.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.CrossHairV);
            this.Controls.Add(this.CrossHairH);
            this.Controls.Add(this.Hole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Hole)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Hole;
        private System.Windows.Forms.Panel CrossHairV;
        private System.Windows.Forms.Panel CrossHairH;
    }
}

