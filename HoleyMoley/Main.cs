using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Security.Principal;

namespace HoleyMoley
{

    public partial class Main : Form
    {
        //[DllImport("user32.dll")]
        //private static extern int GetSystemMenu(int hwnd, int bRevert);
        //[DllImport("user32.dll")]
        //private static extern int AppendMenu(int hMenu, int Flagsw, int IDNewItem, string lpNewItem);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);
        [DllImport("user32.dll")]
        private static extern IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam);

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;
        public const Int32 IDM_TEST = 1000;
        public const Int32 WM_NCLBUTTONDBLCLK = 0x00A3;
        public Hole HoleForm { get; set; }

        private Bitmap ZoomBitmap { get; set; }
        private Graphics ZoomGraphics { get; set; }
        private int LastMouseX { get; set; } = -1;
        private int LastMouseY { get; set; } = -1;
        private int MeasureMouseX { get; set; } = 0;
        private int MeasureMouseY { get; set; } = 0;
        private int ZoomWidth { get; set; } = -1;
        private int ZoomHeight { get; set; } = -1;
        private int MarginSize { get; set; } = -1;
        private int HighlightSize { get; set; } = -1;

        private bool mouseJustPressed { get; set; } = false;

        private bool IsAdministrator { get; set; } = false;

        public Main()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                int WS_EX_TOPMOST = 0x00000008;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOPMOST;
                return cp;
            }
        }

        private void Controller_FormClosed(object sender, FormClosedEventArgs e)
        {
            HighlightHandler.CleanUp();
            Application.Exit();
        }

        private void UI_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.Hide();    // Hiding here helps stop start up flickering of controls appearing/dissapearing/resizing etc.
            this.SuspendLayout();

            HoleForm = new Hole(this);
            //HoleForm.HoleForm = this;
            HoleForm.Show(this);
            HoleForm.Visible = false;        // Hiding here stops Hole from flicking on screen during startup if not enabled
            //HoleControls.UI = UI;

            LoadSettings();

            ProcessTimerRate();
            ProcessMarginDepth();
            ProcessZoomLevel(); // Also initialises ZoomBitmap
            CopyOverZoom();
            SetScreenCrossHairPosition();

            SetCalculatedColors();

            Logo.Visible = false;

            InitCrossHair();

            SetCrossHairVisibility();
            SetScreenCrossHairVisibility();

            // http://www.codeproject.com/KB/dotnet/CustomWinFormSysMenu.aspx
            // windows hooks
            //int Menu1 = GetSystemMenu(this.Handle.ToInt32(), 0);  // get handle to system menu 
            //AppendMenu(Menu1, 0xA00, 0, null);   // makes a separator 
            //AppendMenu(Menu1, 0, 6969, "TestTest");
            IntPtr sysMenuHandle = GetSystemMenu(this.Handle, false);
            //It would be better to find the position at run time of the 'Close' item, but...

            InsertMenu(sysMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
            InsertMenu(sysMenuHandle, 6, MF_BYPOSITION, IDM_TEST, "TEST");

            UpdateHoleSize();

            // HighlightHandler.Init(); Don't need this here - will init when required when ShowHideHighlighting was called earlier
            HighlightHandler.UI = this;
            HighlightHandler.AddToIgnoreList(this.Handle);
            HighlightHandler.SetMatchList(TitleSearch1.Name, TitleSearch1.Text, TitleSearch1Colour.BackColor);
            HighlightHandler.SetMatchList(TitleSearch2.Name, TitleSearch2.Text, TitleSearch2Colour.BackColor);
            HighlightHandler.SetMatchList(TitleSearch3.Name, TitleSearch3.Text, TitleSearch3Colour.BackColor);
            HighlightHandler.SetNoMatchColor(TitleSearchNotFoundColour.BackColor);
            ProcessHighlightDepth();

            ShowHideHole();
            ShowHideZoom();
            ShowHideHighlighting();

            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                IsAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            AdministratorWarning.Visible = !IsAdministrator;

            //ApplicationHandler.PopulateAppWindows();
            this.ResumeLayout();
        }

        public void SetHighlightingTitle(string title)
        {
            HighlightingTitle.Text = title;
        }

        private void ProcessTimerRate()
        {
            if (FasterRefresh.Checked)
                MouseTimer.Interval = 16;
            else
                MouseTimer.Interval = 64;
        }

        private void InitCrossHair()
        {
            // Make sure the CrossHair's are top/left aligned in the designer
            // Setting Top/Left below (commented out) screws up with auto-resizing container panel
            // Commented code left for reference

            CrossHairV.Width = 1;
            CrossHairV.Height = Zoom.Height;
            // CrossHairV.Top = Zoom.Top;
            CrossHairV.Left = Zoom.Left + (Zoom.Width / 2);

            CrossHairH.Width = Zoom.Width;
            CrossHairH.Height = 1;
            CrossHairH.Top = Zoom.Top + (Zoom.Height / 2);
            // CrossHairH.Left = Zoom.Left;
        }

        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //    if (m.Msg == 0x112)
        //    {    // WM_SYSCOMMAND is 0x112 
        //        if (m.WParam.ToInt32() == 6969)
        //        {   // the Menu's ID is 666 
        //            //everything in here will run when menu is clicked 
        //            MessageBox.Show("Yo!");
        //        }
        //    }
        //}

        //hmmm
        //    http://bytes.com/topic/c-sharp/answers/267125-center-messagebox-c


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                switch (m.WParam.ToInt32())
                {
                    case IDM_TEST:
                        MessageBox.Show("TEST");
                        return;
                    default:
                        break;
                }
            }
            else if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                m.Result = IntPtr.Zero;

                ToggleEverything();

                return;
            }
            base.WndProc(ref m);
        }

        private void OpacityLevel_ValueChanged(object sender, EventArgs e)
        {
            HoleForm.Opacity = (double)(OpacityLevel.Value) / 10.0D;
        }

        private void HoleSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHoleSize();
        }

        private void EnableMargin_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHoleSize();
        }

        private void UpdateHoleSize()
        {
            Size size;
            if (GetHoleSize(out size) == true)
            {
                HoleW.Text = size.Width.ToString();
                HoleH.Text = size.Height.ToString();

                HoleForm.MarginEnabled = EnableMargin.Checked;
                HoleForm.MarginDepth = MarginSize;
                HoleForm.Width = size.Width;
                HoleForm.Height = size.Height;
                HoleForm.SetHole();

                //if (HoleSize.Text == "Size to screen")
                //{
                //    CentreHole();
                //}

            }
            else
                MessageBox.Show("Hole size is not in the format of WIDTHxHEIGHT");
        }

        private bool GetHoleSize(out Size size)
        {
            size = new Size();

            string tmp = HoleSize.Text;
            if (string.IsNullOrWhiteSpace(tmp))
                return false;

            if (tmp == "Size to screen")
            {
                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                size.Width = rect.Width - (MarginSize * 2);
                size.Height = rect.Height - (MarginSize * 2);
            }
            else
            {

                if (tmp == "Bad Size")
                    return false;

                string[] sizecomponents = tmp.Split('x');
                if (sizecomponents.GetUpperBound(0) != 1)
                    return false;

                int width = 0;
                if (int.TryParse(sizecomponents[0], out width) == false)
                    return false;

                int height = 0;
                if (int.TryParse(sizecomponents[1], out height) == false)
                    return false;

                size.Width = width;
                size.Height = height;
            }

            return true;
        }

        private void ProcessIndividualHoleSizes()
        {
            int width = 0;
            if (int.TryParse(HoleW.Text, out width) == false)
                width = -1;

            int height = 0;
            if (int.TryParse(HoleH.Text, out height) == false)
                height = -1;

            if (width == -1 || height == -1)
                HoleSize.Text = "Bad Size";
            else
            {
                HoleSize.Text = width.ToString() + "x" + height.ToString();
                UpdateHoleSize();
            }
        }

        private void HoleColour_Click(object sender, EventArgs e)
        {
            if (!EnableHole.Checked)
                return;

            if (ColourPicker.ShowDialog(this) == DialogResult.OK)
            {
                SetColorPickerColor(ColourPicker.Color);    // Colour can be overridden by enable/disabling holes
                SetCalculatedColors();
                HoleForm.SetColour(ColourPicker.Color);
            }
        }

        private void SetColorPickerColor(Color color)
        {
            // ColourPicker.FillColor = color;
            HoleColour.BackColor = color;
        }

        private void SetCalculatedColors()
        {
            // Make darker
            //this.BackColor = Color.FromArgb((int)(BorderColour.Color.R / 1.69), (int)(BorderColour.Color.G / 1.69), (int)(BorderColour.Color.B / 1.69));

            // Make lighter
            int r = (int)((ColourPicker.Color.R * 0.5d) + 64);
            if (r > 255)
                r = 255;
            int g = (int)((ColourPicker.Color.G * 0.5d) + 64);
            if (g > 255)
                g = 255;
            int b = (int)((ColourPicker.Color.B * 0.5d) + 64);
            if (b > 255)
                b = 255;
            this.BackColor = Color.FromArgb(r, g, b);

            //CrossHairV.BackColor = this.BackColor;
            //CrossHairH.BackColor = this.BackColor;
            //ZoomSeparator.BackColor = this.BackColor;
        }

        private void Logo_MouseClick(object sender, MouseEventArgs e)
        {
            ToggleEverything();
        }

        private void ToggleEverything()
        {
            this.SuspendLayout();

            ControlPanel.Visible = !ControlPanel.Visible;
            Logo.Visible = !ControlPanel.Visible;

            if (Logo.Visible)
            {
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                this.Text = "HM";
                //    this.AutoSize = false;
                //    this.Width = 34;
                //    this.Height = 96;
            }
            else
            {
                this.MinimizeBox = true;
                this.MaximizeBox = false;
                this.Text = "Holey Moley";
                //    this.AutoSize = true;
            }

            CopyOverZoom();

            this.ResumeLayout();
        }

        private void About_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void HoleSize_Validated(object sender, EventArgs e)
        {
            UpdateHoleSize();
        }

        #region "Mouse hooks"

        // Note that in lots of examples, wParam is listed as an Int or IntPtr
        // Research showed that this should actually be a UIntPtr (equivalent of WPARAM)
        // So i've used that here :)
        public delegate int HookProc(int nCode, UIntPtr wParam, IntPtr lParam);

        static int hHook = 0;
        public const int HC_ACTION = 0;
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MOUSEWHEEL = 0x20A;

        public static POINT LastMousePos = null;
        public static Boolean Moving = false;

        HookProc MouseHookProcedure;

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, UIntPtr wParam, IntPtr lParam);

        //public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        public int MouseHookProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            //Marshall the data from the callback.
            MouseHookStruct MHS = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            //if ((nCode == HC_ACTION) && (wParam == WM_LBUTTONUP))

            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                if ((nCode == HC_ACTION) && (WM_LBUTTONUP == (int)wParam))
                {
                    System.Diagnostics.Debug.Print("MOUSE UP!");

                    // Unhook to stop monitoring the mouse movement

                    bool ret = UnhookWindowsHookEx(hHook);
                    //If the UnhookWindowsHookEx function fails.
                    if (ret == false)
                    {
                        MessageBox.Show("UnhookWindowsHookEx Failed");
                    }
                    Moving = false;
                    hHook = 0;
                    return 0;
                }
                //if ((nCode == HC_ACTION) && (WM_LBUTTONDOWN == (int)wParam))
                //{
                //    System.Diagnostics.Debug.Print("MOUSE DOWN!");

                //    // Unhook to stop monitoring the mouse movement
                //}
                else
                {
                    // Keep track of current position
                    //int xdif = 

                    if (nCode == HC_ACTION)
                    {
                        if (MHS.pt.x != LastMousePos.x || MHS.pt.y != LastMousePos.y)
                        {
                            int xdif = LastMousePos.x - MHS.pt.x;
                            int ydif = LastMousePos.y - MHS.pt.y;

                            System.Diagnostics.Debug.Print("lastx=" + LastMousePos.x + " xdif=" + xdif);

                            LastMousePos = MHS.pt;

                            HoleForm.AlterXOffset(xdif);
                            HoleForm.AlterYOffset(ydif);

                            HoleForm.SetHole();
                        }

                        //Create a string variable that shows the current mouse coordinates.
                        //System.Diagnostics.Debug.Print("x = " + MHS.pt.x.ToString("d") + "  y = " + MHS.pt.y.ToString("d") + " wHitTestCode=" + MHS.wHitTestCode + " dwExtraInfo=" + MHS.dwExtraInfo + " wParam=" + wParam);

                    }
                    return CallNextHookEx(hHook, nCode, wParam, lParam);
                }
            }
        }


        public void Move_MouseDown(object sender, MouseEventArgs e)
        {
            if (hHook == 0)
            {
                Control control = (Control)sender;
                Point pointOnScreen = control.PointToScreen(new Point(e.X, e.Y));

                LastMousePos = new POINT();
                LastMousePos.x = pointOnScreen.X;
                LastMousePos.y = pointOnScreen.Y;

                Moving = true;

                // Create an instance of HookProc.
                MouseHookProcedure = new HookProc(this.MouseHookProc); //Controller.MouseHookProc);

                // ToDo: Process.GetCurrentProcess().Threads[0].Id instead?
                hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, (IntPtr)0, Process.GetCurrentProcess().Threads[0].Id);
                //     hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId()); // System.Threading.Thread.CurrentThread.ManagedThreadId); // AppDomain.GetCurrentThreadId());
                //If the SetWindowsHookEx function fails.
                if (hHook == 0)
                {
                    MessageBox.Show("SetWindowsHookEx Failed");
                    return;
                }
                System.Diagnostics.Debug.Print("UnHook Windows Hook");
            }
        }

        #endregion "Mouse Hooks"

        private void Centre_Click(object sender, EventArgs e)
        {
            CentreHole();
        }

        private void CentreHole()
        {
            HoleForm.XOffset = 0;
            HoleForm.YOffset = 0;
            HoleForm.SetHole();
        }

        private void Controller_MouseEnter(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.Print("Form enter");

            //ControlPanel.Visible = true;
        }

        private void Controller_MouseLeave(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.Print("Form Leave");
            //if (!Moving)
            //{
            //                ControlPanel.Visible = true;
            //}
        }

        private void Controller_Activated(object sender, EventArgs e)
        {
            //ControlPanel.Visible = true;
        }

        private void Controller_Deactivate(object sender, EventArgs e)
        {
            //ControlPanel.Visible = false;
        }

        private void HoleW_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessIndividualHoleSizes();
        }

        private void HoleH_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessIndividualHoleSizes();
        }

        private void HoleW_Validated(object sender, EventArgs e)
        {
            ProcessIndividualHoleSizes();
        }

        private void HoleH_Validated(object sender, EventArgs e)
        {
            ProcessIndividualHoleSizes();
        }

        private void MouseTimer_Tick(object sender, EventArgs e)
        {
            //if (!Properties.Settings.Default.OverlayVisible && ParentForm.Visible)
            //    ParentForm.Hide();

            bool Update = false;

            if (LastMouseX != Cursor.Position.X)
            {
                LastMouseX = Cursor.Position.X;
                Update = true;
            }

            if (LastMouseY != Cursor.Position.Y)
            {
                LastMouseY = Cursor.Position.Y;
                Update = true;
            }

            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                if (!mouseJustPressed)
                {
                    MeasureMouseX = LastMouseX;
                    MeasureMouseY = LastMouseY;
                    mouseJustPressed = true;
                }
            }
            else
            {
                mouseJustPressed = false;
            }

            if (Update || ConstantUpdate.Checked)
            {
                MousePosition.Text = Cursor.Position.X.ToString() + "," + Cursor.Position.Y.ToString();
                CopyOverZoom();
                SetScreenCrossHairPosition();
            }

            MouseMeasure.Text = $"{LastMouseX - MeasureMouseX},{LastMouseY - MeasureMouseY}";
        }

        private void CopyOverZoom()
        {
            // ToDo: Had an exception when escilation screen appeared running admin app (on the CopyFromScreen) - see if it's just the mouse position messing things up (TBC)
            if (!ControlPanel.Visible || !Zoom.Visible || LastMouseX < 0 || LastMouseY < 0)
            {
                Zoom.Image = null;
                return;
            }

            // zoom mode is taken care of outside of this, so we aren't continually recalculating it
            var size = new System.Drawing.Size()
            {
                Width = Zoom.Width,
                Height = Zoom.Height
            };
            int halfWidth = size.Width / 2;
            int halfHeight = size.Height / 2;

            ZoomGraphics.CopyFromScreen(LastMouseX - (ZoomWidth / 2), LastMouseY - (ZoomHeight / 2), 0, 0, size); //, CopyPixelOperation.SourceCopy);

            Zoom.Image = (Image)ZoomBitmap;
        }

        private void ProcessZoomLevel()
        {
            int zoomLevel = ZoomLevel.Value;

            ZoomWidth = Zoom.Width / zoomLevel;
            ZoomHeight = Zoom.Height / zoomLevel;

            ZoomBitmap = new Bitmap(ZoomWidth, ZoomHeight);
            ZoomGraphics = Graphics.FromImage(ZoomBitmap);

            //ZoomGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //ZoomGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            if (zoomLevel == 1)
                Zoom.SizeMode = PictureBoxSizeMode.Normal;
            else
                Zoom.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ZoomLevel_ValueChanged(object sender, EventArgs e)
        {
            if (ZoomBitmap != null)
            {
                ZoomBitmap.Dispose();
                ZoomGraphics.Dispose();

                ProcessZoomLevel();
                CopyOverZoom();
            }
        }

        private void ProcessHighlightDepth()
        {
            HighlightSize = HighlightDepth.Value;
            HighlightHandler.SetBorderSize(HighlightSize);
        }

        private void ProcessMarginDepth()
        {
            MarginSize = MarginDepth.Value * 25;
            if (MarginSize == 0)
                MarginSize = 1;
        }

        private void MarginDepth_ValueChanged(object sender, EventArgs e)
        {
            ProcessMarginDepth();
            UpdateHoleSize();
        }

        private void CrossHair_CheckedChanged(object sender, EventArgs e)
        {
            SetCrossHairVisibility();
        }

        private void SetCrossHairVisibility()
        {
            bool state = CrossHair.Checked;

            CrossHairV.Visible = state & Zoom.Visible;
            CrossHairH.Visible = state & Zoom.Visible;
        }

        private void ScreenCrossHairs_CheckedChanged(object sender, EventArgs e)
        {
            SetScreenCrossHairVisibility();
        }

        private void SetScreenCrossHairVisibility()
        {
            bool state = ScreenCrossHairs.Checked;

            HoleForm.SetCrosshairsVisibility(state);
        }

        private void SetScreenCrossHairPosition()
        {
            HoleForm.SetCrosshairs(LastMouseX, LastMouseY);
        }

        private void Zoom_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();

            if (Zoom.Width == 184)
            {
                Zoom.Width *= 2;
                Zoom.Height *= 2;

                ControlPanel.Width += 184;
                ControlPanel.Height += 184;

                this.Width += 184;
                this.Height += 184;
            }
            else
            {
                Zoom.Width /= 2;
                Zoom.Height /= 2;

                ControlPanel.Width -= 184;
                ControlPanel.Height -= 184;

                this.Width -= 184;
                this.Height -= 184;
            }

            InitCrossHair();

            this.ResumeLayout();
        }

        private void SaveSettings()
        {
            //string filename;
            //filename = Path.GetDirectoryName(Assembly.GetEntryAssembly().GetName().CodeBase);
            //%USERPROFILE%\AppData\Local

            var settings = Properties.Settings.Default;

            // Hole
            settings.HoleEnabled = EnableHole.Checked;
            settings.HoleSize = HoleSize.Text;
            settings.OpacityLevel = OpacityLevel.Value;
            settings.Margin = EnableMargin.Checked;
            settings.ScreenCrossHairs = ScreenCrossHairs.Checked;
            settings.MarginDepth = MarginDepth.Value;
            settings.OverlayVisible = HoleForm.Visible;
            settings.PosX = HoleForm.XOffset;
            settings.PosY = HoleForm.YOffset;
            settings.ControllerX = this.Left;
            settings.ControllerY = this.Top;
            settings.HoleControls = HoleControls.Checked;

            // Highlighting
            settings.HighlightingEnabled = EnableHilighting.Checked;
            settings.HighlightAppNames = HighlightAppNames.Text;
            settings.HighlightDepth = HighlightDepth.Value;
            settings.TitleSearch1 = TitleSearch1.Text;
            settings.TitleSearch2 = TitleSearch2.Text;
            settings.TitleSearch3 = TitleSearch3.Text;
            settings.TitleSearch1Colour = TitleSearch1Colour.BackColor;
            settings.TitleSearch2Colour = TitleSearch2Colour.BackColor;
            settings.TitleSearch3Colour = TitleSearch3Colour.BackColor;
            settings.TitleSearch4Colour = TitleSearchNotFoundColour.BackColor;

            // Zoom
            settings.ZoomEnabled = EnableZoom.Checked;
            settings.ZoomLevel = ZoomLevel.Value;
            settings.ConstantUpdate = ConstantUpdate.Checked;
            settings.CrossHair = CrossHair.Checked;
            settings.FasterRefresh = FasterRefresh.Checked;

            settings.Save();
        }


        private void LoadSettings()
        {
            var settings = HoleyMoley.Properties.Settings.Default;

            // Hole
            EnableHole.Checked = settings.HoleEnabled;
            HoleSize.Text = settings.HoleSize;
            OpacityLevel.Value = settings.OpacityLevel;
            EnableMargin.Checked = settings.Margin;
            ScreenCrossHairs.Checked = settings.ScreenCrossHairs;
            MarginDepth.Value = settings.MarginDepth;
            HoleForm.XOffset = settings.PosX;
            HoleForm.YOffset = settings.PosY;
            this.Left = settings.ControllerX;
            this.Top = settings.ControllerY;
            HoleControls.Checked = settings.HoleControls;
            HoleForm.SetControlVisibility(HoleControls.Checked);

            // Highlighting
            EnableHilighting.Checked = settings.HighlightingEnabled;
            HighlightAppNames.Text = settings.HighlightAppNames;
            HighlightDepth.Value = settings.HighlightDepth;
            TitleSearch1.Text = settings.TitleSearch1;
            TitleSearch2.Text = settings.TitleSearch2;
            TitleSearch3.Text = settings.TitleSearch3;
            TitleSearch1Colour.BackColor = settings.TitleSearch1Colour;
            TitleSearch2Colour.BackColor = settings.TitleSearch2Colour;
            TitleSearch3Colour.BackColor = settings.TitleSearch3Colour;
            TitleSearchNotFoundColour.BackColor = settings.TitleSearch4Colour;

            // Zoom
            EnableZoom.Checked = settings.ZoomEnabled;
            ZoomLevel.Value = settings.ZoomLevel;
            ConstantUpdate.Checked = settings.ConstantUpdate;
            CrossHair.Checked = settings.CrossHair;
            FasterRefresh.Checked = settings.FasterRefresh;
        }

        private void Controller_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void FasterRefresh_CheckedChanged(object sender, EventArgs e)
        {
            ProcessTimerRate();
        }

        private void EnableHole_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideHole();
            EnableDisableMoveAppToHole();
        }

        private void EnableDisableMoveAppToHole()
        {
            MoveAppToHole.Enabled = EnableHole.Checked & EnableHilighting.Checked;
            RestoreApp.Enabled = MoveAppToHole.Enabled;
            HoleForm.HoleControls.EnableDisableMoveAppToHole(MoveAppToHole.Enabled);
        }

        private void ShowHideHole()
        {
            this.SuspendLayout();

            if (EnableHole.Checked)
            {
                HoleForm.Show();
                HoleForm.HoleControls.Visible = HoleControls.Checked;
                ControlVisibility(HolePanel, "HoleVisibility", true);
                SetColorPickerColor(ColourPicker.Color);
            }
            else
            {
                HoleForm.Hide();
                HoleForm.HoleControls.Hide();
                ControlVisibility(HolePanel, "HoleVisibility", false);
                SetColorPickerColor(Color.LightGray);
            }

            SetCheckBoxText(EnableHole);

            this.ResumeLayout();
        }

        private void EnableZoom_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideZoom();
        }

        private void ShowHideZoom()
        {
            if (EnableZoom.Checked)
                ControlVisibility(ZoomPanel, "ZoomVisibility", true);
            else
                ControlVisibility(ZoomPanel, "ZoomVisibility", false);

            SetCrossHairVisibility();

            SetCheckBoxText(EnableZoom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Root control we check for visibility for - will also check all child controls</param>
        /// <param name="tag"></param>
        /// <param name="state"></param>
        private void ControlVisibility(Control root, string tag, bool state)
        {
            if ((string)root.Tag == tag)
                root.Visible = state;

            // We also have to recurse down any children
            foreach (Control child in root.Controls)
                ControlVisibility(child, tag, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root">Root control we check enablement for - will also check all child controls</param>
        /// <param name="tag"></param>
        /// <param name="state"></param>
        private void ControlEnablement(Control root, string tag, bool state)
        {
            if ((string)root.Tag == tag)
                root.Enabled = state;

            // We also have to recurse down any children
            foreach (Control child in root.Controls)
                ControlEnablement(child, tag, state);
        }

        private void SetCheckBoxText(CheckBox checkBox)
        {
            if (checkBox.Checked)
                checkBox.Text = "On";
            else
                checkBox.Text = "Off";
        }

        private void EnableHilighting_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideHighlighting();
            EnableDisableMoveAppToHole();
        }

        private void ShowHideHighlighting()
        {
            if (EnableHilighting.Checked)
            {
                HighlightHandler.Show();
                //   ControlEnablement(HighlightingPanel, "HilightingVisibility", true);
                ControlVisibility(HighlightingPanel, "HilightingVisibility", true);
            }
            else
            {
                HighlightHandler.Hide();
                ControlVisibility(HighlightingPanel, "HilightingVisibility", false);
            }

            SetCheckBoxText(EnableHilighting);
        }

        private void HighlightDepth_ValueChanged(object sender, EventArgs e)
        {
            ProcessHighlightDepth();
            HighlightHandler.ReHighlightWindow();
        }

        private void MoveAppToHole_Click(object sender, EventArgs e)
        {
            MoveApptoHole();
        }

        public void MoveApptoHole()
        {
            HoleForm.MoveWindowToHole(HighlightHandler.CurrentHwnd);
        }

        private void HoleControls_Click(object sender, EventArgs e)
        {
            HoleForm.SetControlVisibility(HoleControls.Checked);
        }

        private void RestoreApp_Click(object sender, EventArgs e)
        {
            RestoreAppPos();
        }

        public void RestoreAppPos()
        {
            HoleForm.RestoreAppPos();
        }

        private void TitleSearch1Colour_Click(object sender, EventArgs e)
        {
            HandleHighlightColourPicker(TitleSearch1Colour);
        }

        private void HandleHighlightColourPicker(Button ctrl)
        {
            if (!EnableHilighting.Checked)
                return;

            if (ColourPicker.ShowDialog(this) == DialogResult.OK)
            {
                ctrl.BackColor = ColourPicker.Color;
            }

            // Dodo - refresh any active colours here
        }

        private void TitleSearch2Colour_Click(object sender, EventArgs e)
        {
            HandleHighlightColourPicker(TitleSearch2Colour);
        }

        private void TitleSearch3Colour_Click(object sender, EventArgs e)
        {
            HandleHighlightColourPicker(TitleSearch3Colour);
        }

        private void TitleSearchNotFoundColour_Click(object sender, EventArgs e)
        {
            HandleHighlightColourPicker(TitleSearchNotFoundColour);
        }

        private void EnableDebug_Click(object sender, EventArgs e)
        {
            if (DebugPanel.Visible)
            {
                DebugPanel.Visible = false;
            }
            else
            {
                DebugPanel.Visible = true;
                AddToDebug($"Starting debug @ {DateTime.Now:HH:mm.ff}...");
            }
        }

        private List<String> DebugData { get; set; } = new(20);

        public bool DebugEnabled()
        {
            return DebugPanel.Visible;
        }

        public void AddToDebug(string content)
        {
            if (!DebugPanel.Visible)
                return;

            if (DebugData.Count == DebugData.Capacity)
            {
                DebugData.RemoveAt(DebugData.Count - 1);
            }

            DebugData.Insert(0, content);
            //DebugInfo.Text = DebugData.ToString();
            DebugInfo.Text = string.Join($"{System.Environment.NewLine}{System.Environment.NewLine}", DebugData);
        }

        private void AdministratorWarning_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Holey Moley isn't running with Administrator privaledges, so may not be able to highlight all windows (due to access restrictions). This will affect things such as Remote Desktop Connection parent windows.", "Access permission restrictions", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
        }
    }

    public class Settings
    {
        string Name { get; set; }
    }

    public class CustomPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            paintEventArgs.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            base.OnPaint(paintEventArgs);
        }
    }
}

