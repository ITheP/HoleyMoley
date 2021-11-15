using System;
using System.Drawing;
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

        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 768;
        public int XOffset { get; set; } = 0;
        public int YOffset { get; set; } = 0;
        public bool MarginEnabled { get; set; } = true;
        public int MarginDepth { get; set; } = -1;

        private int CrossHairX;
        private int CrossHairY;

        UI UI { get; set; }
        public HoleControls HoleControls { get; set; }


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

            int exstyle = GetWindowLong(this.Handle, (int)GetWindowLongFlags.GWL_EXSTYLE);
            exstyle |= (int)WindowStylesEx.WS_EX_TRANSPARENT;
            SetWindowLong(this.Handle, (int)GetWindowLongFlags.GWL_EXSTYLE, exstyle);

            IntPtr hwndf = this.Handle;
            IntPtr hwndParent = GetDesktopWindow();
            SetParent(hwndf, hwndParent);

            this.Opacity = 0.40D;
            this.TopMost = true;

            HoleControls = new HoleControls();
            HoleControls.Owner = this;
            HoleControls.TopMost = true;
            HoleControls.Show();
            HoleControls.Visible = false;

            UI = new UI();
            UI.HoleForm = this;
            UI.Show(this);
            HoleControls.UI = UI;

            HighlightHandler.AddToIgnoreList(this.Handle);
            HighlightHandler.AddToIgnoreList(HoleControls.Handle);

            //SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            //this.Refresh();

            // for some reason, when this function returns, window always unhides, even if not set to :/
            if (Properties.Settings.Default.OverlayVisible)
                this.Show();

            
            this.ResumeLayout(false);
        }

        public void SetControlVisibility(bool visible)
        {
            HoleControls.Visible = visible;
        }

        public void AlterXOffset(int offset)
        {
            XOffset += offset;
        }

        public void AlterYOffset(int offset)
        {
            YOffset += offset;
        }

        public void ResetOffset()
        {
            XOffset = 0;
            YOffset = 0;
        }

        public void SetColour(Color color)
        {
            this.BackColor = color;
        }

        public Color GetCurrentColour()
        {
            return this.BackColor;
        }

        public void SetHole()
        {
            this.SuspendLayout();

            // this. references the window (effectivley our border)

            // rect = reference to the entire screen (size)
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;

            this.Hole.Size = new System.Drawing.Size(Width, Height);

            if (MarginEnabled == true)
            {
                // Scales a margin 50 pixels around the hole
                // We offset the parent window (this.) in this case
                this.Size = new System.Drawing.Size(Width + (MarginDepth * 2), Height + (MarginDepth * 2));
                this.Top = ((rect.Height / 2) - (this.Height / 2)) - YOffset - MarginDepth;
                this.Left = (rect.Width / 2) - (this.Width / 2) - XOffset - MarginDepth;

                System.Diagnostics.Debug.Print("Left=" + this.Left);

                // Remember the hole is placed in relation to the main window, not an absolute position on the screen
                this.Hole.Top = MarginDepth;
                this.Hole.Left = MarginDepth;
            }
            else
            {
                // Scales to full screen with the rectangle in the middle
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                this.Top = 0;
                this.Left = 0;

                // We offset the Hole in this case
                this.Hole.Top = ((rect.Height / 2) - (this.Hole.Height / 2)) - YOffset;
                this.Hole.Left = ((rect.Width / 2) - (this.Hole.Width / 2)) - XOffset;
            }


            MoveHoleControls();

            this.ResumeLayout(false);
        }

        IntRect PreviousAppPos { get; set; }
        IntPtr PreviousHwnd { get; set; }

        public void MoveWindowToHole(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return;

            var prevRect = new IntRect();
            NativeMethods.GetWindowRect(hwnd, ref prevRect);
            PreviousAppPos = prevRect;
            PreviousHwnd = hwnd;

            var rect = new IntRect();
            NativeMethods.GetWindowRect(this.Hole.Handle, ref rect);

            //bool result = NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, this.Hole.Top, this.Hole.Left,this.Hole.Width, this.Hole.Height, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
            bool result = NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
        }

        public void RestoreAppPos()
        {
            if (PreviousHwnd == IntPtr.Zero)
                return;

            IntRect rect = PreviousAppPos;
            bool result = NativeMethods.SetWindowPos(PreviousHwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
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

        private void MoveHoleControls()
        {
            var rect = new IntRect();
            NativeMethods.GetWindowRect(this.Hole.Handle, ref rect);

            if (HoleControls != null)
                // 26 = size of move control
                HoleControls.Location = new System.Drawing.Point(rect.Left - 25, rect.Top - HoleControls.Height);
        }
    }
}
