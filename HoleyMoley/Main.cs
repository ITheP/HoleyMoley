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
//using System.Windows.Controls;
//using System.Windows;
using System.Drawing.Imaging;
using Microsoft.Win32;
//using System.Windows;

namespace HoleyMoley
{

    public partial class Main : Form
    {
        private string Version = "v1.7.1";

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

        private int HotkeyId = 0;
        private string HotkeyText = "";
        private int HotkeyLeftMouseId = 0;
        private string HotkeyLeftMouseText = "";

        private bool MouseJustPressed = false;
        private bool ZoomEnabled = true;

        public string About_VersionInfo { get; set; }
        public string About_Title { get; set; }
        public string About_Product { get; set; }
        public string About_TitleInfo { get; set; }
        public string About_CopyrightInfo { get; set; }
        public string About_CompanyInfo { get; set; }
        public string About_DescriptionInfo { get; set; }
        public string About_CPU { get; set; }

        private AppInfo TrackedApp { get; set; }
        private int TrackedApp_LastX { get; set; }
        private int TrackedApp_LastY { get; set; }
        private int TrackedApp_LastW { get; set; }
        private int TrackedApp_LastH { get; set; }

        private bool UpdateMouseMeasure { get; set; }

        // Screen colour info
        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        private bool mouseJustPressed { get; set; } = false;

        private bool IsAdministrator { get; set; } = false;


        private System.Windows.Forms.ToolTip toolTip = new();

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

            // Need to handle screen locking - makes our handles go gaga
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            InitAbout();

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

            InsertMenu(sysMenuHandle, 5, Native.MF_BYPOSITION | Native.MF_SEPARATOR, 0, string.Empty);
            InsertMenu(sysMenuHandle, 6, Native.MF_BYPOSITION, Native.IDM_TEST, "TEST");

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

            if (this.Left < 0)
                this.Left = 0;

            if (this.Top < 0)
                this.Top = 0;

            toolTip.AutoPopDelay = 60000;
            toolTip.InitialDelay = 0;
            toolTip.ReshowDelay = 0;
            toolTip.ShowAlways = true;

            toolTip.SetToolTip(CH0, "Colour History");
            toolTip.SetToolTip(CH1, "Colour History");
            toolTip.SetToolTip(CH2, "Colour History");
            toolTip.SetToolTip(CH3, "Colour History");
            toolTip.SetToolTip(CH4, "Colour History");
            toolTip.SetToolTip(CH5, "Colour History");
            toolTip.SetToolTip(CH6, "Colour History");
            toolTip.SetToolTip(CH7, "Colour History");

            UpdateMouseMeasure = false;

            //ApplicationHandler.PopulateAppWindows();
            this.ResumeLayout();
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            if ((int)e.Reason == 7) // SessionLocked
                ZoomEnabled = false;
            else if ((int)e.Reason == 8)    // SessionUnlocked
                ZoomEnabled = true;
        }
        private void InitAbout()
        {
            About_CPU = (System.Runtime.InteropServices.Marshal.SizeOf(IntPtr.Zero) == 8) ? "x64" : "x86";

            Version version;
            //try
            //{
            //    //       version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            //}
            //catch
            //{
            version = Assembly.GetExecutingAssembly().GetName().Version;
            //}

            About_VersionInfo = string.Format("v{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            About_Title = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title);
            About_Product = GetAssemblyAttribute<AssemblyProductAttribute>(a => a.Product);

            if (About_Title != About_Product && !string.IsNullOrWhiteSpace(About_Product))
            {
                About_TitleInfo = String.Format("{0} - {1}", About_Title, About_Product);
            }
            else
            {
                About_TitleInfo = About_Title;
            }

            About_CopyrightInfo = GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);
            About_CompanyInfo = GetAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company);
            About_DescriptionInfo = GetAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description);

            // All that effort and all we want to do is...
            this.Text = $"{this.Text} ({About_CPU}) {About_VersionInfo}";
            // :)
        }
        public string GetAssemblyAttribute<T>(Func<T, string> value)
where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }

        public void SetHighlightingTitle(string title)
        {
            HighlightingTitle.Text = title;
        }

        private void RegisterHotkeys()
        {
            SetHotkeyFromSettings();
            SetHotkeyLeftMouseFromSettings();

            Hotkey.Text = GetHotkeyText();
            Hotkey_LeftMouse.Text = GetHotkeyLeftMouseText();
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

        private void SetHoleVisibility()
        {
            if (Properties.Settings.Default.OverlayVisible)
                HoleForm.Show();
            else
                HoleForm.Hide();
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
            if (m.Msg == Native.WM_SYSCOMMAND)
            {
                switch (m.WParam.ToInt32())
                {
                    case Native.IDM_TEST:
                        About about = new About();
                        about.ShowDialog(this);
                        return;
                    default:
                        break;
                }
            }
            else if (m.Msg == Native.WM_NCLBUTTONDBLCLK)
            {
                m.Result = IntPtr.Zero;

                ToggleEverything();

                return;
            }
            base.WndProc(ref m);

            if (m.Msg == Native.WM_HOTKEY)
            {
                // Right now, were only using 1 hotkey, so don't care about checking what might be pressed
                // To expand if we ever want to find out more about the key being pressed (e.g. multiple keys), we can drill down a bit...
                // m = 32bit int containing key and modifier info. Just need to extract relevant bits.
                // Keys key = (Keys)(((int)m.LParam >> 16) & 0xffff);
                // KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xffff);
                // int hotkeyId = m.WParam.ToInt32();
                // See code elsewhere for handling Keys stuff

                int key = (((int)m.LParam >> 16) & 0xffff);
                int modifier = ((int)m.LParam & 0xffff);
                int hotkeyId = m.WParam.ToInt32();

                if (key == Properties.Settings.Default.HotKeyHashCode && modifier == Properties.Settings.Default.HotKeyModifier)
                {
                    if (this.WindowState == FormWindowState.Normal)
                        MinimiseWindows();
                    else
                        RestoreWindows();
                }
                else if (key == Properties.Settings.Default.HotKeyLeftMouseHashCode && modifier == Properties.Settings.Default.HotKeyLeftMouseModifier)
                {
                    HandleLeftMouseClickHappenings();
                }
            }
        }

        private void MinimiseWindows()
        {
            HoleForm.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void RestoreWindows()
        {
            this.WindowState = FormWindowState.Normal;
            SetHoleVisibility();

            // Make sure controller is on top
            this.BringToFront();
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
                HoleForm.SetHole(TrackedApp);

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
                            float xdif = LastMousePos.x - MHS.pt.x;
                            float ydif = LastMousePos.y - MHS.pt.y;

                            System.Diagnostics.Debug.Print("lastx=" + LastMousePos.x + " xdif=" + xdif);

                            LastMousePos = MHS.pt;

                            //// NEEDED?
                            ////HoleForm.AlterXOffset(xdif);
                            ////HoleForm.AlterYOffset(ydif);

                            //HoleForm.SetHole(TrackedApp);

                            xdif *= AutoScaleFactor.Width;
                            ydif *= AutoScaleFactor.Height;

                            HoleForm.AlterXOffset(xdif);
                            HoleForm.AlterYOffset(ydif);

                            HoleForm.SetHole(TrackedApp);
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
            ////NEEDED?
            ////HoleForm.XOffset = 0;
            ////HoleForm.YOffset = 0;
            ////HoleForm.SetHole(TrackedApp);
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

            if (HoleForm.WindowState == FormWindowState.Minimized)
                return;

            bool Update = false;

            if (TrackedApp != null)
            {
                IntRect rec = TrackedApp.CurrentWindowRect();

                int x = rec.Left;
                int y = rec.Top;
                int w = rec.Right - rec.Left;
                int h = rec.Bottom - rec.Top;

                // WE DON'T ACTUALLY CARE ABOUT THE WIDTH/HEIGHT - just keeping the app in line with HoleyMoley
                // TODO: Add an option to scale to app?
                if (x != TrackedApp_LastX || y != TrackedApp_LastY) // || w != TrackedApp_LastW || h != TrackedApp_LastH)
                {
                    //HoleForm.Width = w;
                    //HoleForm.Height = h;
                    HoleForm.XOrigin = x; // (rec.Right - rec.Left) / 2.0f;
                    HoleForm.YOrigin = y; // (rec.Bottom - rec.Top) / 2.0f;

                    TrackedApp_LastX = x;
                    TrackedApp_LastY = y;
                    //TrackedApp_LastW = w;
                    //TrackedApp_LastH = h;

                    HoleForm.SetHole(TrackedApp);
                }
            }
            else
            {
                HoleForm.XOrigin = 0.0f;
                HoleForm.YOrigin = 0.0f;
            }

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
                    //MeasureMouseX = LastMouseX;
                    //MeasureMouseY = LastMouseY;

                    HandleLeftMouseClickHappenings();

                    mouseJustPressed = true;
                }
            }
            else
            {
                mouseJustPressed = false;
                UpdateMouseMeasure = false;
            }

            if (Update || ConstantUpdate.Checked)
            {
                MousePosition.Text = Cursor.Position.X.ToString() + "," + Cursor.Position.Y.ToString();
                CopyOverZoom();
                SetScreenCrossHairPosition();

                // Need to update so we have no highlight under the cursor pixel, and colour isn't messed with
                UpdateColourInfo(Cursor.Position.X, Cursor.Position.Y, ColourRGB, ColourHex, ColourPreview);
            }

            if (UpdateMouseMeasure || PosOnMouseDownOnly.Checked == false)
                MouseMeasure.Text = $"{LastMouseX - MeasureMouseX},{LastMouseY - MeasureMouseY}";
        }

        private void HandleLeftMouseClickHappenings()
        {
            this.SuspendLayout();

            MeasureMouseX = LastMouseX;
            MeasureMouseY = LastMouseY;

            CH7.BackColor = CH6.BackColor;
            CH6.BackColor = CH5.BackColor;
            CH5.BackColor = CH4.BackColor;
            CH4.BackColor = CH3.BackColor;
            CH3.BackColor = CH2.BackColor;
            CH2.BackColor = CH1.BackColor;
            CH1.BackColor = CH0.BackColor;

            toolTip.SetToolTip(CH7, toolTip.GetToolTip(CH6));
            toolTip.SetToolTip(CH6, toolTip.GetToolTip(CH5));
            toolTip.SetToolTip(CH5, toolTip.GetToolTip(CH4));
            toolTip.SetToolTip(CH4, toolTip.GetToolTip(CH3));
            toolTip.SetToolTip(CH3, toolTip.GetToolTip(CH2));
            toolTip.SetToolTip(CH2, toolTip.GetToolTip(CH1));
            toolTip.SetToolTip(CH1, toolTip.GetToolTip(CH0));

            MM3.Text = MM2.Text;
            MM2.Text = MM1.Text;
            MM1.Text = MM0.Text;
            MM0.Text = MouseMeasure.Text;

            UpdateColourInfo(Cursor.Position.X, Cursor.Position.Y, SnapshotColourRGB, SnapshotColourHex, SnapshotColourPreview);

            CH0.BackColor = SnapshotColourPreview.BackColor;
            toolTip.SetToolTip(CH0, $"{ColourRGB.Text} - {ColourHex.Text}");

            UpdateMouseMeasure = true;

            this.ResumeLayout();
        }

        private void UpdateColourInfo(int X, int Y, Label RGB, Label Hex, Panel Preview)
        {
            // update colour info

            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDc = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retVal = Native.BitBlt(hDC, 0, 0, 1, 1, hSrcDc, X, Y, PatBltType.SrcCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            Color c = screenPixel.GetPixel(0, 0);
            //ColourPreview.BackColor = c;
            Preview.BackColor = c;
            //ColourRGB.Text = $"{c.R},{c.G},{c.B}";
            RGB.Text = $"{c.R},{c.G},{c.B}";
            //ColourHex.Text = $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
            Hex.Text = $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
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
            settings.Grid = Grid.Checked;
            settings.GridSize = GridSize.Text;
            settings.ScreenCrossHairs = ScreenCrossHairs.Checked;
            settings.PosOnMouseDownOnly = PosOnMouseDownOnly.Checked;
            settings.MarginDepth = MarginDepth.Value;
            settings.OverlayVisible = HoleForm.Visible;
            settings.PosX = (int)HoleForm.XOffset;
            settings.PosY = (int)HoleForm.YOffset;
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

            PosOnMouseDownOnly.Checked = Properties.Settings.Default.PosOnMouseDownOnly;

            Grid.Checked = Properties.Settings.Default.Grid;
            GridSize.Text = Properties.Settings.Default.GridSize;
            ProcessGridSize();

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

        // ------------------------------------

        private void TrackAppList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TrackAppList.SelectedItem != null)
            {
                AppInfo app = (AppInfo)(TrackAppList.SelectedItem);

                if (app.Title == "")
                {
                    if (TrackedApp != null)
                    {
                        //RECT rec = TrackedApp.CurrentWindowRect();

                        //// need to make sure any position is retained so we don't jump around
                        //HoleForm.XOffset -= rec.Left;
                        //HoleForm.XOffset += HoleForm.XOrigin;

                        //HoleForm.YOffset -= rec.Top;
                        //HoleForm.YOffset += HoleForm.YOrigin;

                        ////HoleForm.XOrigin = 0;
                        ////HoleForm.YOrigin = 0;

                        TrackedApp = null;
                        CentreHole();
                    }
                    else
                    {
                        TrackedApp = null;
                        HoleForm.SetHole(TrackedApp);
                    }
                }
                else
                {
                    TrackedApp = app;

                    IntRect rec = TrackedApp.CurrentWindowRect();

                    HoleForm.XOffset = -6;
                    HoleForm.YOffset = 1;

                    TrackedApp_LastX = rec.Left;    // account for windows shadow + want to go around window, not over window
                    TrackedApp_LastY = rec.Top;     // want to go around window, not over window

                    HoleForm.XOrigin = TrackedApp_LastX;
                    HoleForm.YOrigin = TrackedApp_LastY;

                    HoleForm.SetHole(TrackedApp);

                    //RECT rec = app.CurrentWindowRect();
                    //Debug.Print($"{rec.Left},{rec.Top},{rec.Right},{rec.Bottom}");

                    //HoleForm.XOffset = 0;
                    //HoleForm.YOffset = 0;
                    //HoleForm.XOrigin = 0;
                    //HoleForm.YOrigin = 0;
                    ////HoleForm.SetHole();
                }
            }
        }

        private void ProcessGridSize()
        {
            string size = GridSize.Text;

            if (string.IsNullOrEmpty(size))
                return;

            int pos = size.IndexOf('x');

            string tx = size.Substring(0, pos - 1);
            if (tx == "")
                tx = "25.5";

            string ty = size.Substring(pos + 2, size.Length - (pos + 2));
            if (ty == "")
                ty = "25.5";

            string tsubx;
            string tsuby;

            int x;
            int y;
            int subx;
            int suby;

            pos = tx.IndexOf('.');
            if (pos > 0)
            {
                tsubx = tx.Substring(pos + 1, tx.Length - pos - 1);
                tx = tx.Substring(0, pos);

                if (int.TryParse(tx, out x) == false)
                    x = 25;

                if (int.TryParse(tsubx, out subx) == false)
                    subx = 5;
            }
            else
            {
                if (int.TryParse(tx, out x) == false)
                    x = 25;

                subx = 0;
            }

            pos = ty.IndexOf('.');
            if (pos > 0)
            {
                tsuby = ty.Substring(pos + 1, ty.Length - pos - 1);
                ty = ty.Substring(0, pos);

                if (int.TryParse(ty, out y) == false)
                    y = 25;

                if (int.TryParse(tsuby, out suby) == false)
                    suby = 5;
            }
            else
            {
                if (int.TryParse(ty, out y) == false)
                    y = 25;

                suby = 0;
            }

            HoleForm.GridEnabled = Grid.Checked;
            HoleForm.GridX = x;
            HoleForm.GridY = y;
            HoleForm.GridSubX = subx;
            HoleForm.GridSubY = suby;
            HoleForm.UpdateDrawSpace();
        }

        private void Hotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void Hotkey_KeyUp(object sender, KeyEventArgs e)
        {
            // PreviewKeyDown doesnt have a handled property, or cancel property. So KeyPress always fires as well it seems. Couldn't find a way around that.
            // Set .text in PreviewKeyDown, the key press will bubble through and add the relevant typed letter as well. Screws things up. DOH
            // So to get round that, we remembered the text pressed, and override the final results here after the controls content has settled down
            // We can't just run the whole thing here (or KeyPressed) as the Key code and modifer properties isn't around any more
            Hotkey.Text = HotkeyText;
        }

        private void SetHotkeyFromSettings()
        {
            Native.RegisterHotKey(this.Handle, HotkeyId, Properties.Settings.Default.HotKeyModifier, Properties.Settings.Default.HotKeyHashCode);
        }

        private void SetHotkeyLeftMouseFromSettings()
        {
            Native.RegisterHotKey(this.Handle, HotkeyLeftMouseId, Properties.Settings.Default.HotKeyLeftMouseModifier, Properties.Settings.Default.HotKeyLeftMouseHashCode);
        }

        private void SetHotkey(int KeyValue, int Modifiers, string ModifiersDescription)
        {
            // ignore 20 and below - those seem to be modifier keys being pressed rather than keys we are interested in!
            if (KeyValue > 20)
            {
                Native.UnregisterHotKey(this.Handle, HotkeyId);

                Properties.Settings.Default.HotKeyHashCode = KeyValue;
                Properties.Settings.Default.HotKeyModifier = Modifiers;
                Properties.Settings.Default.HotKeyModifierDescription = ModifiersDescription;

                SetHotkeyFromSettings();

                HotkeyText = GetHotkeyText();
            }
        }
        private void SetHotkey_LeftMouse(int KeyValue, int Modifiers, string ModifiersDescription)
        {
            // ignore 20 and below - those seem to be modifier keys being pressed rather than keys we are interested in!
            if (KeyValue > 20)
            {
                Native.UnregisterHotKey(this.Handle, HotkeyLeftMouseId);

                Properties.Settings.Default.HotKeyLeftMouseHashCode = KeyValue;
                Properties.Settings.Default.HotKeyLeftMouseModifier = Modifiers;
                Properties.Settings.Default.HotKeyLeftMouseModifierDescription = ModifiersDescription;

                SetHotkeyLeftMouseFromSettings();

                HotkeyLeftMouseText = GetHotkeyLeftMouseText();
            }
        }

        private string GetHotkeyText()
        {
            KeysConverter converter = new KeysConverter();

            string ModifiersDescription = Properties.Settings.Default.HotKeyModifierDescription;

            return (ModifiersDescription + (string.IsNullOrEmpty(ModifiersDescription) ? "" : "+") + converter.ConvertToString(Properties.Settings.Default.HotKeyHashCode));
        }

        private string GetHotkeyLeftMouseText()
        {
            KeysConverter converter = new KeysConverter();

            string ModifiersDescription = Properties.Settings.Default.HotKeyLeftMouseModifierDescription;

            return (ModifiersDescription + (string.IsNullOrEmpty(ModifiersDescription) ? "" : "+") + converter.ConvertToString(Properties.Settings.Default.HotKeyLeftMouseHashCode));
        }

        private void PopulateAppTrackList()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            TrackAppList.Items.Clear();

            Process[] processList = Process.GetProcesses();
            AppInfo app = new AppInfo
            {
                Title = ""
            };
            TrackAppList.Items.Add(app);

            foreach (Process process in processList)
            {

                if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle != IntPtr.Zero && Native.IsWindowVisible(process.MainWindowHandle))
                {
                    app = new AppInfo
                    {
                        Title = process.MainWindowTitle,
                        HWnd = process.MainWindowHandle // HandleRef(process, process.MainWindowHandle),
                    };

                    TrackAppList.Items.Add(app);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void TrackAppList_DropDown(object sender, EventArgs e)
        {
            PopulateAppTrackList();
        }

        private void GridSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessGridSize();
        }

        private void Grid_CheckedChanged(object sender, EventArgs e)
        {
            ProcessGridSize();
        }

        private void HotKey_LeftMouse_KeyUp(object sender, KeyEventArgs e)
        {
            // PreviewKeyDown doesnt have a handled property, or cancel property. So KeyPress always fires as well it seems. Couldn't find a way around that.
            // Set .text in PreviewKeyDown, the key press will bubble through and add the relevant typed letter as well. Screws things up. DOH
            // So to get round that, we remembered the text pressed, and override the final results here after the controls content has settled down
            // We can't just run the whole thing here (or KeyPressed) as the Key code and modifer properties isn't around any more
            Hotkey_LeftMouse.Text = HotkeyLeftMouseText;
        }

        private void HotKey_LeftMouse_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            KeysConverter converter = new KeysConverter();
            Debug.Print($"Code:{e.KeyCode}, Data:{e.KeyData}, Value:{e.KeyValue}, Modifier:{e.Modifiers}, Translation:{converter.ConvertToString(e.KeyValue)}");

            int Modifiers = 0;
            string ModifiersDescription = "";
            string sep = "";
            if (e.Control)
            {
                Modifiers = (int)Native.KeyModifiers.Ctrl;
                ModifiersDescription = "Ctrl";
                sep = "+";
            }
            if (e.Alt)
            {
                Modifiers += (int)Native.KeyModifiers.Alt;
                ModifiersDescription += sep + "Alt";
                sep = "+";
            }
            if (e.Shift)
            {
                Modifiers += (int)Native.KeyModifiers.Shift;
                ModifiersDescription += sep + "Shift";
            }

            SetHotkey_LeftMouse((int)e.KeyCode, Modifiers, ModifiersDescription);
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

