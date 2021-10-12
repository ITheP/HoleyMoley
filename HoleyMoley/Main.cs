using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace HoleyMoley
{
    public partial class Main : Form
    {
        //[DllImport("user32.dll")]
        //private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        //private const int WM_SETREDRAW = 11;

        public Main()
        {
            InitializeComponent();
        }

        public int width { get; set; } = 1024;
        public int height { get; set; } = 768;
        public int xoffset { get; set; } = 0;
        public int yoffset { get; set; } = 0;
        public bool margin { get; set; } = true;
        public int marginDepth { get; set; } = -1;

        private int CrossHairX;
        private int CrossHairY;

        private void Main_Load(object sender, EventArgs e)
        {
            // this.WindowState = FormWindowState.Maximized;

            this.SuspendLayout();
            this.Hide();
            //SendMessage(this.Handle, WM_SETREDRAW, false, 0);
            //SendMessage(ParentForm.Handle, WM_SETREDRAW, false, 0);

            //this.Name = "Holey Moley";
            //this.Text = "Holey Moley";
            Hole.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));

            this.TransparencyKey = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));

            int exstyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            exstyle |= WS_EX_TRANSPARENT;
            SetWindowLong(this.Handle, GWL_EXSTYLE, exstyle);

            IntPtr hwndf = this.Handle;
            IntPtr hwndParent = GetDesktopWindow();
            SetParent(hwndf, hwndParent);

            this.Opacity = 0.40D;
            this.TopMost = true;

            Controller controller = new Controller();
            controller.ParentForm = this;
            controller.Show(this);

            //SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            //this.Refresh();

            // for some reason, when this function returns, window always unhides, even if not set to :/
            if (Properties.Settings.Default.OverlayVisible)
                this.Show();

            this.ResumeLayout(false);
        }

        public void AlterXOffset(int offset)
        {
            xoffset += offset;
        }

        public void AlterYOffset(int offset)
        {
            yoffset += offset;
        }

        public void ResetOffset()
        {
            xoffset = 0;
            yoffset = 0;
        }

        public void SetColour(Color color)
        {
            this.BackColor = color;
        }

        public void SetHole()
        {
            this.SuspendLayout();

            // this. references the window (effectivley our border)

            // rect = reference to the entire screen (size)
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;

            this.Hole.Size = new System.Drawing.Size(width, height);

            if (margin == true)
            {
                // Scales a margin 50 pixels around the hole
                // We offset the parent window (this.) in this case
                this.Size = new System.Drawing.Size(width + (marginDepth * 2), height + (marginDepth * 2));
                this.Top = ((rect.Height / 2) - (this.Height / 2)) - yoffset;
                this.Left = (rect.Width / 2) - (this.Width / 2) - xoffset;

                System.Diagnostics.Debug.Print("Left=" + this.Left);

                // Remember the hole is placed in relation to the main window, not an absolute position on the screen
                this.Hole.Top = marginDepth;
                this.Hole.Left = marginDepth;
            }
            else
            {
                // Scales to full screen with the rectangle in the middle
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                this.Top = 0;
                this.Left = 0;

                // We offset the Hole in this case
                this.Hole.Top = ((rect.Height / 2) - (this.Hole.Height / 2)) - yoffset;
                this.Hole.Left = ((rect.Width / 2) - (this.Hole.Width / 2)) - xoffset;
            }

            this.ResumeLayout(false);
        }


        public void SetCrosshairs(int x, int y)
        {
            CrossHairX = x - this.Left;
            CrossHairY = y - this.Top;

            CrossHairV.BackColor = this.BackColor;
            CrossHairH.BackColor = this.BackColor;

            this.SuspendLayout();

            CrossHairV.Top = 0;
            CrossHairV.Left = CrossHairX;
            CrossHairV.Height = this.Size.Height;
            CrossHairV.Width = 1;

            CrossHairH.Top = CrossHairY;
            CrossHairH.Left = 0;
            CrossHairH.Height = 1;
            CrossHairH.Width = this.Size.Width;

            this.ResumeLayout(false);

            //Pen p = new Pen(Color.FromArgb(255,128,0,0));
            //p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            //Graphics g = this.CreateGraphics();
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

            ////this.SuspendLayout();
            //g.DrawLine(p, CrossHairX, 0, CrossHairX, this.Size.Height);
            //g.DrawLine(p, 0, CrossHairY, this.Size.Width, CrossHairY);
            ////this.ResumeLayout(false);
            //g.Dispose();
        }

        public void SetCrosshairsVisibility(bool IsVisible)
        {
            CrossHairV.Visible = IsVisible;
            CrossHairH.Visible = IsVisible;
        }

        private void MoveHandle_MouseDown(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.Print("Here");
        }
    }
}
