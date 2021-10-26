using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace HoleyMoley
{
    // Helpfull info/research...
    // https://devblogs.microsoft.com/oldnewthing/20210104-00/?p=104656
    // https://docs.microsoft.com/en-us/windows/win32/winauto/event-constants
    // https://stackoverflow.com/questions/15682543/drawing-and-clearing-on-screen-with-graphics-fromhwnd

    // Handle WM_NCPAINT? https://www.codeproject.com/questions/1129299/how-do-i-draw-on-windows-form-titlebar-without-get
    // WM_NCACTIVATE + extra info https://docs.microsoft.com/en-us/windows/win32/gdi/nonclient-area

    // Backing window https://stackoverflow.com/questions/2232727/how-to-draw-outside-a-window

    public static class ApplicationHandler
    {

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
    //    static NativeMethods.WinEventDelegate NameChangeDelegate = new NativeMethods.WinEventDelegate(TitleChangeProc);
        static NativeMethods.WinEventDelegate LocationChangeDelegate = new NativeMethods.WinEventDelegate(LocationChangeProc);

        private static IntPtr TitleTextHook { get; set; }
        private static IntPtr LocationChangeHook { get; set; }

        public static void Init()
        {
            // Listen for name change changes across all processes/threads on current desktop...
         //   TitleTextHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, (uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, NameChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            LocationChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, (uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, LocationChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            // MessageBox provides the necessary mesage loop that SetWinEventHook requires.
            // In real-world code, use a regular message loop (GetMessage/TranslateMessage/
            // DispatchMessage etc or equivalent.)
            Debug.Print("Tracking name changes on HWNDs, close message box to exit.");
        }

        public static void CleanUp()
        {
            NativeMethods.UnhookWinEvent(LocationChangeHook);
   //         NativeMethods.UnhookWinEvent(TitleTextHook);
        }

        //static void TitleChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        //{
        //    // filter out non-HWND namechanges... (eg. items within a listbox)
        //    if (idObject != 0 || idChild != 0)
        //    {
        //        return;
        //    }
        //    Debug.Print("Text of hwnd changed {0:x8}", hwnd.ToInt32());
        //}

        private static Random Rnd { get; set; } = new Random();

        private static IntPtr GetParent(IntPtr hwnd)
        {
            IntPtr parent = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);
            
            if (parent == null || parent == IntPtr.Zero || parent == NativeMethods.GetDesktopWindow())
                return hwnd;

            return parent;

            //IntPtr nextParent = NativeMethods.GetAncestor(parent, GetAncestorFlags.GetParent);

            //if (nextParent == null || nextParent == IntPtr.Zero || nextParent == NativeMethods.GetDesktopWindow())
            //    return parent;

            //return nextParent;
        }

        static void LocationChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (idObject == (int)SystemObjectIDs.OBJID_WINDOW ) //&& idChild == CHILDID_SELF)
            {
                // hwnd may be a child window
                // We check for parents, and use if available. Note that we stop searching if parent = desktop window
                //     int capacity = NativeMethods.GetWindowTextLength( hwnd) * 2;
                //     StringBuilder stringBuilder = new StringBuilder(capacity);
                //     NativeMethods.GetWindowText( hwnd, stringBuilder, stringBuilder.Capacity);
                //     Debug.Print($"Original '{stringBuilder}': {hwnd.ToString("x4")}, Parent: {GetParent(hwnd).ToString("x4")} with desktop={NativeMethods.GetDesktopWindow()}");

                ////     hwnd = GetParent(hwnd);
                
                long style = (long)NativeMethods.GetWindowLongPtr(hwnd, (int)GetWindowLongFlags.GWL_STYLE);
                long isTopLevel = style & ((long)WindowStyles.WS_CHILD);

                // Only parents
                if (isTopLevel != 0)
                    return;

                // Windows we are interested in have captions too (it seems)
                long isCaptioned = style & ((long)WindowStyles.WS_CAPTION);
                if (isCaptioned == 0)
                    return;

                IntPtr parent = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);
                if (parent != hwnd)
                    return; // child window, ignore

                var rect = new IntRect();
                NativeMethods.GetWindowRect(hwnd, ref rect);
                //var relativeRect = new System.Drawing.Rectangle(0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top);
                int halfBorderSize = 3;
                int borderSize = halfBorderSize + halfBorderSize;
                //var borderedRect = new System.Drawing.Rectangle(halfBorderSize, halfBorderSize, rect.Right - rect.Left - borderSize, rect.Bottom - rect.Top - borderSize);


                HighlightHandler.SetPosition(hwnd, rect.Left - halfBorderSize, rect.Top - halfBorderSize, rect.Right - rect.Left + borderSize, rect.Bottom - rect.Top + borderSize);

              ////  if (relativeRect.Width > 100 && relativeRect.Height > 100)
              ////  {

              //      int capacity = NativeMethods.GetWindowTextLength( hwnd) * 2;
              //      StringBuilder stringBuilder = new StringBuilder(capacity);
              //      NativeMethods.GetWindowText( hwnd, stringBuilder, stringBuilder.Capacity);

              //      // using (Graphics g = Graphics.FromHwnd(hwnd))
              //      IntPtr dc = NativeMethods.GetWindowDC(hwnd);

              //      Debug.Print($"Window info {(uint)style} {hwnd.ToString("x8")}.{idObject} DC={dc} - {rect.Left},{rect.Top},{rect.Right},{rect.Bottom} - {rect.Bottom - rect.Top}x{rect.Right - rect.Left} - Child: {idChild} - '{stringBuilder}'");

              //      if (dc != IntPtr.Zero)
              //      {
              //          using (Graphics g = Graphics.FromHdc(dc))
              //          {
              //              Pen pen = new Pen(Color.FromArgb(128, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255)), borderSize);
              //             // SolidBrush brush = new SolidBrush(Color.FromArgb(128, Rnd.Next(255), Rnd.Next(255), Rnd.Next(255)));
              //              g.DrawRectangle(pen, borderedRect);
              //          }
              //      }
              //      NativeMethods.ReleaseDC(hwnd, dc);
              ////  }
            }


            //// filter out mouse movement/cursor
            //if (hwnd == IntPtr.Zero || idObject == (int)SystemObjectIDs.OBJID_CURSOR)
            //{
            //    return;
            //}
            //Debug.Print("Location of hwnd changed {0:x8}", hwnd.ToInt32());
        }

        public static void PopulateAppWindows()
        {
            Cursors.CursorWait();
            //    SetInfoText(InfoIcon_Hourglass, "Scanning capture targets...");

            List<AppInfo> windows = new List<AppInfo>();

            Process[] processList = Process.GetProcesses();
            AppInfo app;

            foreach (Process process in processList)
            {

                if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle != IntPtr.Zero && NativeMethods.IsWindowVisible(process.MainWindowHandle))
                {
                    //Debug.Print((process.MainWindowTitle.Length < 32 ? process.MainWindowTitle : process.MainWindowTitle.Substring(32) + "..."));
                    app = new AppInfo
                    {
                        Title = (process.MainWindowTitle.Length < 48 ? process.MainWindowTitle : process.MainWindowTitle.Substring(0, 48) + "..."),
                        HWnd = process.MainWindowHandle
                    };

                    var size = Utils.GetSizeOfWindow(app);

                    // Screenshot of window!
                    //app.Preview = ScreenCapture.SnapScreen(app.HWnd, 0, 0, size.Width, size.Height, 128, 128, SizeHandling.ScaleToFit, true);
                    windows.Add(app);
                }
            }

            windows.Sort((x, y) => x.Title.CompareTo(y.Title));

            // Add these in reverse order at the start so everything appears as it should do
            windows.Insert(0, new AppInfo() { Title = "Windows...", HWnd = IntPtr.Zero });
            // windows.Insert(0, new AppInfo() { Title = "", HWnd = IntPtr.Zero });

            IntPtr hWnd = NativeMethods.GetDesktopWindow();

            for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)
            {
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[i];

                IntRect rect = new IntRect();
                rect.Left = screen.Bounds.Left;
                rect.Right = screen.Bounds.Right;
                rect.Top = screen.Bounds.Top;
                rect.Bottom = screen.Bounds.Bottom;
                System.Drawing.Rectangle r;
                windows.Insert(0, new AppInfo() { Title = $"Display {i} - {screen.DeviceName}", HWnd = hWnd, Bounds = rect, BoundsSet = true });
            }

            windows.Insert(0, new AppInfo() { Title = "Entire Desktop", HWnd = hWnd });
            windows.Insert(0, new AppInfo() { Title = "Displays...", HWnd = IntPtr.Zero });
            windows.Insert(0, new AppInfo() { Title = "Define custom area", HWnd = hWnd });
            windows.Insert(0, new AppInfo() { Title = "Custom...", HWnd = IntPtr.Zero });

            //UI_SelectedAppWindow.ItemsSource = windows;

            //SetInfoText(InfoIcon_Smile, "Select capture target from drop down above");

            Cursors.CursorRestore();
        }


        public enum SystemEventContants : uint
        {
            EVENT_SYSTEM_SOUND = 0x1,
            EVENT_SYSTEM_ALERT = 0x2,
            EVENT_SYSTEM_FOREGROUND = 0x3,
            EVENT_SYSTEM_MENUSTART = 0x4,
            EVENT_SYSTEM_MENUEND = 0x5,
            EVENT_SYSTEM_MENUPOPUPSTART = 0x6,
            EVENT_SYSTEM_MENUPOPUPEND = 0x7,
            EVENT_SYSTEM_CAPTURESTART = 0x8,
            EVENT_SYSTEM_CAPTUREEND = 0x9,
            EVENT_SYSTEM_MOVESIZESTART = 0xa,
            EVENT_SYSTEM_MOVESIZEEND = 0xb,
            EVENT_SYSTEM_CONTEXTHELPSTART = 0xc,
            EVENT_SYSTEM_CONTEXTHELPEND = 0xd,
            EVENT_SYSTEM_DRAGDROPSTART = 0xe,
            EVENT_SYSTEM_DRAGDROPEND = 0xf,
            EVENT_SYSTEM_DIALOGSTART = 0x10,
            EVENT_SYSTEM_DIALOGEND = 0x11,
            EVENT_SYSTEM_SCROLLINGSTART = 0x12,
            EVENT_SYSTEM_SCROLLINGEND = 0x13,
            EVENT_SYSTEM_SWITCHSTART = 0x14,
            EVENT_SYSTEM_SWITCHEND = 0x15,
            EVENT_SYSTEM_MINIMIZESTART = 0x16,
            EVENT_SYSTEM_MINIMIZEEND = 0x17
        }

        public enum ObjectEventContants : uint
        {
            EVENT_OBJECT_CREATE = 0x8000,
            EVENT_OBJECT_DESTROY = 0x8001,
            EVENT_OBJECT_SHOW = 0x8002,
            EVENT_OBJECT_HIDE = 0x8003,
            EVENT_OBJECT_REORDER = 0x8004,
            EVENT_OBJECT_FOCUS = 0x8005,
            EVENT_OBJECT_SELECTION = 0x8006,
            EVENT_OBJECT_SELECTIONADD = 0x8007,
            EVENT_OBJECT_SELECTIONREMOVE = 0x8008,
            EVENT_OBJECT_SELECTIONWITHIN = 0x8009,
            EVENT_OBJECT_STATECHANGE = 0x800A,
            EVENT_OBJECT_LOCATIONCHANGE = 0x800B,
            EVENT_OBJECT_NAMECHANGE = 0x800C,
            EVENT_OBJECT_DESCRIPTIONCHANGE = 0x800D,
            EVENT_OBJECT_VALUECHANGE = 0x800E,
            EVENT_OBJECT_PARENTCHANGE = 0x800F,
            EVENT_OBJECT_HELPCHANGE = 0x8010,
            EVENT_OBJECT_DEFACTIONCHANGE = 0x8011,
            EVENT_OBJECT_ACCELERATORCHANGE = 0x8012
        }

        //// possible marshaling unmanaged type conflict/problem between 32/64 bit
        //public enum SystemObjectIDs : long
        //{
        //    OBJID_WINDOW = 0x00000000,
        //    OBJID_SYSMENU = 0xFFFFFFFF,
        //    OBJID_TITLEBAR = 0xFFFFFFFE,
        //    OBJID_MENU = 0xFFFFFFFD,
        //    OBJID_CLIENT = 0xFFFFFFFC,
        //    OBJID_VSCROLL = 0xFFFFFFFB,
        //    OBJID_HSCROLL = 0xFFFFFFFA,
        //    OBJID_SIZEGRIP = 0xFFFFFFF9,
        //    OBJID_CARET = 0xFFFFFFF8,
        //    OBJID_CURSOR = 0xFFFFFFF7,
        //    OBJID_ALERT = 0xFFFFFFF6,
        //    OBJID_SOUND = 0xFFFFFFF5,
        //    OBJID_QUERYCLASSNAMEIDX = 0xFFFFFFF4,
        //    OBJID_NATIVEOM = 0xFFFFFFF0
        //}

        public enum SystemObjectIDs : int
        {
            OBJID_WINDOW = 0x0000,
            OBJID_SYSMENU = 0xFFFF,
            OBJID_TITLEBAR = 0xFFFE,
            OBJID_MENU = 0xFFFD,
            OBJID_CLIENT = 0xFFFC,
            OBJID_VSCROLL = 0xFFFB,
            OBJID_HSCROLL = 0xFFFA,
            OBJID_SIZEGRIP = 0xFFF9,
            OBJID_CARET = 0xFFF8,
            OBJID_CURSOR = 0xFFF7,
            OBJID_ALERT = 0xFFF6,
            OBJID_SOUND = 0xFFF5,
            OBJID_QUERYCLASSNAMEIDX = 0xFFF4,
            OBJID_NATIVEOM = 0xFFF0
        }


        public enum Flags : uint
        {
            WINEVENT_OUTOFCONTEXT = 0x0000,
            WINEVENT_SKIPOWNTHREAD = 0x0001,
            WINEVENT_SKIPOWNPROCESS = 0x0002,
            WINEVENT_INCONTEXT = 0x0004
        }
    }
}
