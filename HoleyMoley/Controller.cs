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

namespace HoleyMoley
{

    public partial class Controller : Form
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
        public Main ParentForm;

        private Bitmap ZoomBitmap;
        private Graphics ZoomGraphics;
        private int lastMouseX = -1;
        private int lastMouseY = -1;
        private int measureMouseX = 0;
        private int measureMouseY = 0;
        private int zoomWidth = -1;
        private int zoomHeight = -1;
        private int marginDepth = -1;

        private bool mouseJustPressed = false;

        public Controller()
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
            ApplicationHandler.CleanUp();
            Application.Exit();
        }


        private void Controller_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;

            LoadSettings();

            ProcessTimerRate();        
            ProcessMarginDepth();
            ProcessZoomLevel(); // Also initialises ZoomBitmap
            CopyOverZoom();
            SetScreenCrossHairPosition();

            SetCalculatedColors();
            ControlPanel.BackColor = Color.White;

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

            //ApplicationHandler.PopulateAppWindows();
            ApplicationHandler.Init();
        }

        private void ProcessTimerRate()
        {
            if (FasterRefresh.Checked)
                MouseTimer.Interval = 16;
            else
                MouseTimer.Interval = 32;
        }

        private void InitCrossHair()
        {
            CrossHairV.Width = 1;
            CrossHairV.Height = Zoom.Height;
            CrossHairV.Top = Zoom.Top;
            CrossHairV.Left = Zoom.Left + (Zoom.Width / 2);

            CrossHairH.Width = Zoom.Width;
            CrossHairH.Height = 1;
            CrossHairH.Top = Zoom.Top + (Zoom.Height / 2);
            CrossHairH.Left = Zoom.Left;
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
            ParentForm.Opacity = (double)(OpacityLevel.Value) / 10.0D;
        }

        private void HoleSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHoleSize();
        }

        private void Toggle_Click(object sender, EventArgs e)
        {
            if (ParentForm.Visible)
                ParentForm.Hide();
            else
                ParentForm.Show();
        }

        private void Margin_CheckedChanged(object sender, EventArgs e)
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

                ParentForm.margin = Margin.Checked;
                ParentForm.marginDepth = marginDepth;
                ParentForm.width = size.Width;
                ParentForm.height = size.Height;
                ParentForm.SetHole();

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
                size.Width = rect.Width - (marginDepth * 2);
                size.Height = rect.Height - (marginDepth * 2);
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

        private void ColourPicker_Click(object sender, EventArgs e)
        {
            if (BorderColour.ShowDialog(this) == DialogResult.OK)
            {
                ColourPicker.FillColor = BorderColour.Color;
                SetCalculatedColors();
                ParentForm.SetColour(BorderColour.Color);
            }
        }

        private void SetCalculatedColors()
        {
            this.BackColor = Color.FromArgb((int)(BorderColour.Color.R / 1.69), (int)(BorderColour.Color.G / 1.69), (int)(BorderColour.Color.B / 1.69));
            //CrossHairV.BackColor = this.BackColor;
            //CrossHairH.BackColor = this.BackColor;
            ZoomSeparator.BackColor = this.BackColor;
        }

        private void Logo_MouseClick(object sender, MouseEventArgs e)
        {
            ToggleEverything();
        }

        private void ToggleEverything()
        {
            ControlPanel.Visible = !ControlPanel.Visible;
            Logo.Visible = !ControlPanel.Visible;
            CopyOverZoom();
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
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

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

                            ParentForm.AlterXOffset(xdif);
                            ParentForm.AlterYOffset(ydif);

                            ParentForm.SetHole();
                        }

                        //Create a string variable that shows the current mouse coordinates.
                        //System.Diagnostics.Debug.Print("x = " + MHS.pt.x.ToString("d") + "  y = " + MHS.pt.y.ToString("d") + " wHitTestCode=" + MHS.wHitTestCode + " dwExtraInfo=" + MHS.dwExtraInfo + " wParam=" + wParam);

                    }
                    return CallNextHookEx(hHook, nCode, wParam, lParam);
                }
            }
        }


        private void Move_MouseDown(object sender, MouseEventArgs e)
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

                hHook = SetWindowsHookEx(WH_MOUSE, MouseHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId()); // System.Threading.Thread.CurrentThread.ManagedThreadId); // AppDomain.GetCurrentThreadId());
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
            ParentForm.xoffset = 0;
            ParentForm.yoffset = 0;
            ParentForm.SetHole();
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

            if (lastMouseX != Cursor.Position.X)
            {
                lastMouseX = Cursor.Position.X;
                Update = true;
            }

            if (lastMouseY != Cursor.Position.Y)
            {
                lastMouseY = Cursor.Position.Y;
                Update = true;
            }

            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                if (!mouseJustPressed)
                {
                    measureMouseX = lastMouseX;
                    measureMouseY = lastMouseY;
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

            MouseMeasure.Text = $"{lastMouseX - measureMouseX},{lastMouseY - measureMouseY}";
        }

        private void CopyOverZoom()
        {
            if (!ControlPanel.Visible)
                return;

            // zoom mode is taken care of outside of this, so we aren't continually recalculating it
            var size = new System.Drawing.Size()
            {
                Width = Zoom.Width,
                Height = Zoom.Height
            };
            int halfWidth = size.Width / 2;
            int halfHeight = size.Height / 2;

            ZoomGraphics.CopyFromScreen(lastMouseX - (zoomWidth / 2), lastMouseY - (zoomHeight / 2), 0, 0, size); //, CopyPixelOperation.SourceCopy);

            Zoom.Image = (Image)ZoomBitmap;
        }

        private void ProcessZoomLevel()
        {
            int zoomLevel = ZoomLevel.Value;

            zoomWidth = Zoom.Width / zoomLevel;
            zoomHeight = Zoom.Height / zoomLevel;

            ZoomBitmap = new Bitmap(zoomWidth, zoomHeight);
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

        private void ProcessMarginDepth()
        {
            marginDepth = MarginDepth.Value * 25;
            if (marginDepth == 0)
                marginDepth = 1;
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

            CrossHairV.Visible = state;
            CrossHairH.Visible = state;
        }

        private void ScreenCrossHairs_CheckedChanged(object sender, EventArgs e)
        {
            SetScreenCrossHairVisibility();
        }

        private void SetScreenCrossHairVisibility()
        {
            bool state = ScreenCrossHairs.Checked;

            ParentForm.SetCrosshairsVisibility(state);
        }

        private void SetScreenCrossHairPosition()
        {
            ParentForm.SetCrosshairs(lastMouseX, lastMouseY);
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

            Properties.Settings.Default.HoleSize = HoleSize.Text;
            Properties.Settings.Default.OpacityLevel = OpacityLevel.Value;
            Properties.Settings.Default.Margin = Margin.Checked;
            Properties.Settings.Default.ScreenCrossHairs = ScreenCrossHairs.Checked;
            Properties.Settings.Default.MarginDepth = MarginDepth.Value;
            Properties.Settings.Default.ZoomLevel = ZoomLevel.Value;
            Properties.Settings.Default.ConstantUpdate = ConstantUpdate.Checked;
            Properties.Settings.Default.CrossHair = CrossHair.Checked;
            Properties.Settings.Default.OverlayVisible = ParentForm.Visible;
            Properties.Settings.Default.PosX = ParentForm.xoffset;
            Properties.Settings.Default.PosY = ParentForm.yoffset;
            Properties.Settings.Default.ControllerX = this.Left;
            Properties.Settings.Default.ControllerY = this.Top;
            Properties.Settings.Default.FasterRefresh = FasterRefresh.Checked;
            Properties.Settings.Default.Save();
        }


        private void LoadSettings()
        {
            HoleSize.Text = Properties.Settings.Default.HoleSize;
            OpacityLevel.Value = Properties.Settings.Default.OpacityLevel;
            Margin.Checked = Properties.Settings.Default.Margin;
            ScreenCrossHairs.Checked = Properties.Settings.Default.ScreenCrossHairs;
            MarginDepth.Value = Properties.Settings.Default.MarginDepth;
            ZoomLevel.Value = Properties.Settings.Default.ZoomLevel;
            ConstantUpdate.Checked = Properties.Settings.Default.ConstantUpdate;
            CrossHair.Checked = Properties.Settings.Default.CrossHair;
            ParentForm.xoffset = Properties.Settings.Default.PosX;
            ParentForm.yoffset = Properties.Settings.Default.PosY;
            this.Left = Properties.Settings.Default.ControllerX;
            this.Top = Properties.Settings.Default.ControllerY;
            FasterRefresh.Checked = Properties.Settings.Default.FasterRefresh;
        }

        private void Controller_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void FasterRefresh_CheckedChanged(object sender, EventArgs e)
        {
            ProcessTimerRate();
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

