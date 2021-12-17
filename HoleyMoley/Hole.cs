using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace HoleyMoley
{
    public partial class Hole : Form
    {
        //[DllImport("user32.dll")]
        //private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        //private const int WM_SETREDRAW = 11;

        //public Main()
        //{
        //    InitializeComponent();
        //}

        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 768;
        public float XOffset { get; set; } = 0;
        public float YOffset { get; set; } = 0;
        public float XOrigin { get; set; } = 0.0f;
        public float YOrigin { get; set; } = 0.0f;
        public bool MarginEnabled { get; set; } = true;
        public int MarginDepth { get; set; } = -1;

        private int CrossHairX;
        private int CrossHairY;

        public bool GridEnabled { get; set; } = true;
        public int GridX { get; set; } = 25;
        public int GridY { get; set; } = 25;
        public int GridSubX { get; set; } = 5;
        public int GridSubY { get; set; } = 5;

        private Bitmap DrawSpace { get; set; }
        private Graphics GraphicsSpace { get; set; }
        private Pen TransPen { get; set; }
        private Color TransCol { get; set; }
        private Brush TransBrush { get; set; }
        private bool TransInitComplete { get; set; } = false;


        Main UI { get; set; }
        public HoleControls HoleControls { get; set; }

        public Hole(Main ui)
        {
            InitializeComponent();

            UI = ui;

            HoleControls = new HoleControls();
            HoleControls.Owner = this;
            HoleControls.TopMost = true;
            HoleControls.Show();
            HoleControls.Visible = false;
            HoleControls.UI = UI;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // this.WindowState = FormWindowState.Maximized;

            this.SuspendLayout();
            this.Hide();
            //SendMessage(this.Handle, WM_SETREDRAW, false, 0);
            //SendMessage(ParentForm.Handle, WM_SETREDRAW, false, 0);

            //this.Name = "Holey Moley";
            //this.Text = "Holey Moley";
            HoleArea.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));

            this.TransparencyKey = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));

            int exstyle = GetWindowLong(this.Handle, (int)GetWindowLongFlags.GWL_EXSTYLE);
            exstyle |= (int)WindowStylesEx.WS_EX_TRANSPARENT;
            SetWindowLong(this.Handle, (int)GetWindowLongFlags.GWL_EXSTYLE, exstyle);

            IntPtr hwndf = this.Handle;
            IntPtr hwndParent = GetDesktopWindow();
            SetParent(hwndf, hwndParent);

            this.Opacity = 0.40D;
            this.TopMost = true;

            //UI = new UI();
            //UI.HoleForm = this;
            //UI.Show(this);

            HighlightHandler.AddToIgnoreList(this.Handle);
            HighlightHandler.AddToIgnoreList(HoleControls.Handle);

            //SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            //this.Refresh();

            // for some reason, when this function returns, window always unhides, even if not set to :/
            //if (Properties.Settings.Default.OverlayVisible)
            //    this.Show();


            this.ResumeLayout(false);
        }

        public void SetControlVisibility(bool visible)
        {
            HoleControls.Visible = visible;
        }

        public void AlterXOffset(float offset)
        {
            XOffset += offset;
        }

        public void AlterYOffset(float offset)
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

        public void SetHole(AppInfo trackedApp)
        {
            this.SuspendLayout();

            // this. references the window (effectivley our border)

            // rect = reference to the entire screen (size)
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;

            this.HoleArea.Size = new System.Drawing.Size(Width, Height);

            if (!TransInitComplete)
            {
                TransCol = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));
                TransPen = new Pen(TransCol);
                TransBrush = new SolidBrush(TransCol);
                this.TransparencyKey = TransCol; // System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));
                TransInitComplete = true;
            }

            DrawSpace = new Bitmap(HoleArea.Width, HoleArea.Height);
            GraphicsSpace = Graphics.FromImage(DrawSpace);
            HoleArea.Image = DrawSpace;

            UpdateDrawSpace();

            if (MarginEnabled == true)
            {
                if (trackedApp == null)
                {
                    int centerX = rect.Width / 2;
                    int centerY = rect.Height / 2;

                    // Scales a margin 50 pixels around the hole
                    // We offset the parent window (this.) in this case
                    this.Size = new System.Drawing.Size(Width + (MarginDepth * 2), Height + (MarginDepth * 2));
                    //this.Left = (rect.Width / 2) - (this.Width / 2) - XOffset - MarginDepth; 
                    //this.Top = ((rect.Height / 2) - (this.Height / 2)) - YOffset - MarginDepth;
                    this.Left = centerX - (this.Width / 2) - (int)XOffset - MarginDepth;
                    this.Top = (centerY - (this.Height / 2)) - (int)YOffset - MarginDepth;

                    System.Diagnostics.Debug.Print("Left=" + this.Left);

                    // Remember the hole is placed in relation to the main window, not an absolute position on the screen
                    this.HoleArea.Top = MarginDepth;
                    this.HoleArea.Left = MarginDepth;
                }
                else
                {
                    IntRect rec = trackedApp.CurrentWindowRect();

                    this.Size = new System.Drawing.Size(Width + (MarginDepth * 2), Height + (MarginDepth * 2));
                    this.Left = rec.Left - MarginDepth - (int)XOffset;
                    this.Top = rec.Top - MarginDepth - (int)YOffset;

                    //System.Diagnostics.Debug.Print("Left=" + this.Left);

                    // Remember the hole is placed in relation to the main window, not an absolute position on the screen
                    this.HoleArea.Top = MarginDepth;
                    this.HoleArea.Left = MarginDepth;
                }
            }
            else
            {
                if (trackedApp == null)
                {
                    // Scales to full screen with the rectangle in the middle
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    this.Top = 0;
                    this.Left = 0;

                    // We offset the Hole in this case
                    //this.HoleArea.Top = ((rect.Height / 2) - (this.HoleArea.Height / 2)) - YOffset + (int)YOrigin;
                    //this.HoleArea.Left = ((rect.Width / 2) - (this.HoleArea.Width / 2)) - XOffset + (int)XOrigin;
                    
                    this.HoleArea.Top = ((rect.Height / 2) - (this.HoleArea.Height / 2)) - (int)YOffset + (int)YOrigin;
                    this.HoleArea.Left = ((rect.Width / 2) - (this.HoleArea.Width / 2)) - (int)XOffset + (int)XOrigin;
                }
                else
                {
                    IntRect rec = trackedApp.CurrentWindowRect();

                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    this.Top = 0;
                    this.Left = 0;

                    // We offset the Hole in this case
                    this.HoleArea.Top = rec.Top - (int)YOffset;
                    this.HoleArea.Left = rec.Left - (int)XOffset;
                }
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
            Native.GetWindowRect(hwnd, ref prevRect);
            PreviousAppPos = prevRect;
            PreviousHwnd = hwnd;

            var rect = new IntRect();
            Native.GetWindowRect(this.HoleArea.Handle, ref rect);

            //bool result = NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, this.Hole.Top, this.Hole.Left,this.Hole.Width, this.Hole.Height, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
            bool result = Native.SetWindowPos(hwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
        }

        public void RestoreAppPos()
        {
            if (PreviousHwnd == IntPtr.Zero)
                return;

            IntRect rect = PreviousAppPos;
            bool result = Native.SetWindowPos(PreviousHwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
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
            Native.GetWindowRect(this.HoleArea.Handle, ref rect);

            if (HoleControls != null)
                // 26 = size of move control
                HoleControls.Location = new System.Drawing.Point(rect.Left - 25, rect.Top - HoleControls.Height);
        }

        public void UpdateDrawSpace()
        {
            if (DrawSpace == null)
                return;

            GraphicsSpace.FillRectangle(TransBrush, 0, 0, DrawSpace.Width, DrawSpace.Height);

            if (GridEnabled)
            {
                int x;
                int y;

                if (GridSubX > 0)
                {
                    x = 0;
                    while (x < DrawSpace.Width)
                    {
                        GraphicsSpace.DrawLine(System.Drawing.Pens.LightGray, x, 0, x, DrawSpace.Height);
                        x += GridSubX;
                    }
                }

                if (GridSubY > 0)
                {
                    y = 0;
                    while (y < DrawSpace.Width)
                    {
                        GraphicsSpace.DrawLine(System.Drawing.Pens.LightGray, 0, y, DrawSpace.Width, y);
                        y += GridSubY;
                    }
                }

                if (GridX > 0)
                {
                    x = 0;
                    while (x < DrawSpace.Width)
                    {
                        GraphicsSpace.DrawLine(System.Drawing.Pens.Gray, x, 0, x, DrawSpace.Height);
                        x += GridX;
                    }
                }

                if (GridY > 0)
                {
                    y = 0;
                    while (y < DrawSpace.Width)
                    {
                        GraphicsSpace.DrawLine(System.Drawing.Pens.Gray, 0, y, DrawSpace.Width, y);
                        y += GridY;
                    }
                }
            }

            HoleArea.Invalidate();
        }
    }
}
