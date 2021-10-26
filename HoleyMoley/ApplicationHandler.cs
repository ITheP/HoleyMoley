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
        // Store delegates in class field so not collected during use
        static NativeMethods.WinEventDelegate NameChangeDelegate = new NativeMethods.WinEventDelegate(TitleChangeProc);
        static NativeMethods.WinEventDelegate LocationChangeDelegate = new NativeMethods.WinEventDelegate(LocationChangeProc);
        static NativeMethods.WinEventDelegate FocusChangeDelegate = new NativeMethods.WinEventDelegate(FocusChangeProc);
        static NativeMethods.WinEventDelegate DestroyChangeDelegate = new NativeMethods.WinEventDelegate(DestroyChangeProc);

        private static IntPtr TitleTextHook { get; set; }
        private static IntPtr LocationChangeHook { get; set; }
        private static IntPtr FocusChangeHook { get; set; }
        private static IntPtr DestroyChangeHook { get; set; }

        public static void Init()
        {
            // Listen for name change changes across all processes/threads on current desktop...
            TitleTextHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, (uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, NameChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            LocationChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, (uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, LocationChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            FocusChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_FOCUS, (uint)ObjectEventContants.EVENT_OBJECT_FOCUS, IntPtr.Zero, FocusChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            DestroyChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_DESTROY, (uint)ObjectEventContants.EVENT_OBJECT_DESTROY, IntPtr.Zero, DestroyChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);

            // MessageBox provides the necessary mesage loop that SetWinEventHook requires.
            // In real-world code, use a regular message loop (GetMessage/TranslateMessage/
            // DispatchMessage etc or equivalent.)
            //Debug.Print("Tracking name changes on HWNDs, close message box to exit.");
        }

        public static void CleanUp()
        {
            NativeMethods.UnhookWinEvent(DestroyChangeHook);
            NativeMethods.UnhookWinEvent(FocusChangeHook);
            NativeMethods.UnhookWinEvent(LocationChangeHook);
            NativeMethods.UnhookWinEvent(TitleTextHook);
        }

        static void TitleChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // filter out non-HWND namechanges... (eg. items within a listbox)
            if (idObject != 0 || idChild != 0)
            {
                return;
            }
            //Debug.Print("Text of hwnd changed {0:x8}", hwnd.ToInt32());

            // Always check on title changes, it might change the colour of the highlighting

            long style = (long)NativeMethods.GetWindowLongPtr(hwnd, (int)GetWindowLongFlags.GWL_STYLE);
            long isTopLevel = style & ((long)WindowStyles.WS_CHILD);

            // Only parents
            if (isTopLevel != 0)
                return;

            //Debug.Print($"Title: {hwnd.ToString("x8")}");

            HighlightWindow(hwnd);
        }

        static void FocusChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //     if (idObject != 0 || idChild != 0)
            //      {
            //          return;
            //     }

            if (hwnd == CurrentHwnd)
                return; // No need to do anything if currently selected!

            //// Ignore the desktop!
            //if (hwnd == NativeMethods.GetDesktopWindow())
            //    return;

            //string info = string.Empty;
            //if ((idObject == (int)SystemObjectIDs.OBJID_ALERT))
            //    info = " Alert";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_CARET))
            //    info = " Caret";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_CLIENT))
            //    info = " Client";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_CURSOR))
            //    info = " Cursor";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_HSCROLL))
            //    info = " HSccroll";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_MENU))
            //    info = " Menu";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_NATIVEOM))
            //    info = " NativeOM";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_QUERYCLASSNAMEIDX))
            //    info = " QueryClassNameIDX";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_SIZEGRIP))
            //    info = " SizeGrip";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_SOUND))
            //    info = " Sound";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_SYSMENU))
            //    info = " SysMenu";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_TITLEBAR))
            //    info = " TitleBar";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_VSCROLL))
            //    info = " VScroll";
            //else if ((idObject == (int)SystemObjectIDs.OBJID_WINDOW))
            //    info = " Window";
            ////Debug.Print($"Focus of hwnd changed {hwnd.ToString("x8")} idO={idObject} 0x{idObject.ToString("X4")} idC={idChild} ET={eventType} - {info}");
            //int capacity = NativeMethods.GetWindowTextLength(hwnd) * 2;
            //StringBuilder stringBuilder = new StringBuilder(capacity);
            //NativeMethods.GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);
            //Debug.Print($"FOCUS: {hwnd.ToString("x8")} {stringBuilder}.{info}");




            // Only Client calls seem to be relevant (and a focus event may be raised when a client + several children all raise focus events - only want the client, not the children)
            if (idObject != (int)SystemObjectIDs.OBJID_CLIENT)
                return;

            IntPtr parentHwnd = GetParent(hwnd);

            // ToDo: Cache the Program Manager? Have an ignore list?
            int capacity = NativeMethods.GetWindowTextLength(parentHwnd) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            NativeMethods.GetWindowText(parentHwnd, stringBuilder, stringBuilder.Capacity);

            // Ignore the desktop
            if (stringBuilder.ToString() == "Program Manager")
                return;

            CurrentHwnd = parentHwnd;
            HighlightWindow(parentHwnd);
        }

        private static Random Rnd { get; set; } = new Random();

        private static IntPtr CurrentHwnd { get; set; } = IntPtr.Zero;

        private static IntPtr GetParent(IntPtr hwnd)
        {
            IntPtr parent = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);

            if (parent == null || parent == IntPtr.Zero || parent == NativeMethods.GetDesktopWindow())
                return hwnd;

            return parent;
        }

        static void LocationChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != CurrentHwnd)
                return;

            //if ((SystemObjectIDs)idObject == SystemObjectIDs.OBJID_WINDOW) //&& idChild == CHILDID_SELF)
            //{
            //    long style = (long)NativeMethods.GetWindowLongPtr(hwnd, (int)GetWindowLongFlags.GWL_STYLE);
            //    long isTopLevel = style & ((long)WindowStyles.WS_CHILD);

            //    // Only parents
            //    if (isTopLevel != 0)
            //        return;

            //    // Windows we are interested in have captions too (it seems)
            //    long isCaptioned = style & ((long)WindowStyles.WS_CAPTION);
            //    if (isCaptioned == 0)
            //        return;

            //    IntPtr parent = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);
            //    if (parent != hwnd)
            //        return; // child window, ignore

            //    // Final check - is what we are seeing the currently focused window?
            //    if (hwnd != CurrentHwnd)
            //        return;

            //Debug.Print($"LOCATION: {hwnd.ToString("x8")}");
            HighlightWindow(GetParent(hwnd));
            //}
        }

        static void DestroyChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != CurrentHwnd)
                return;

            if ((SystemObjectIDs)idObject != SystemObjectIDs.OBJID_WINDOW)
                return;

            // Byebye to the currently highlighted window - so we hide our highlighting
            Debug.Print($"DESTROY: {hwnd.ToString("x8")}  - {idObject}");
            HighlightHandler.Hide();
        }

        private static void HighlightWindow(IntPtr hwnd)
        {
            // Only bother if this is for currently focused window (should never actually not be)
            if (hwnd != CurrentHwnd)
                return;

            // Get caption
            int capacity = NativeMethods.GetWindowTextLength(hwnd) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            NativeMethods.GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);
            //Debug.Print($"Original '{stringBuilder}': {hwnd.ToString("x4")}, Parent: {GetParent(hwnd).ToString("x4")} with desktop={NativeMethods.GetDesktopWindow()}");

            string title = stringBuilder.ToString().ToLower();

            Color color;
            if (title.Contains("live"))
                color = Color.Red;
            else if (title.Contains("test"))
                color = Color.LimeGreen;
            else if (title.Contains("dev"))
                color = Color.DodgerBlue;
            else
                color = Color.Gold;     // Goldenrod

            var rect = new IntRect();
            NativeMethods.GetWindowRect(hwnd, ref rect);
            //var relativeRect = new System.Drawing.Rectangle(0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top);
            int borderSize = 5;
            int doubleBorderSize = borderSize + borderSize;

            HighlightHandler.SetPosition(hwnd, rect.Left - borderSize, rect.Top - borderSize, rect.Right - rect.Left + doubleBorderSize, rect.Bottom - rect.Top + doubleBorderSize, color);
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

        //public enum SystemObjectIDs : int
        //{
        //    OBJID_WINDOW = 0x0000,              // 0
        //    OBJID_SYSMENU = 0xFFFF,             // -1
        //    OBJID_TITLEBAR = 0xFFFE,            // -2
        //    OBJID_MENU = 0xFFFD,                // -3
        //    OBJID_CLIENT = 0xFFFC,              // -4
        //    OBJID_VSCROLL = 0xFFFB,             // -5
        //    OBJID_HSCROLL = 0xFFFA,             // -6
        //    OBJID_SIZEGRIP = 0xFFF9,            // -7
        //    OBJID_CARET = 0xFFF8,               // -8
        //    OBJID_CURSOR = 0xFFF7,              // -9
        //    OBJID_ALERT = 0xFFF6,               // -10
        //    OBJID_SOUND = 0xFFF5,               // -11
        //    OBJID_QUERYCLASSNAMEIDX = 0xFFF4,   // -12
        //    OBJID_NATIVEOM = 0xFFF0             // -16
        //}

        public enum SystemObjectIDs : int
        {
            OBJID_WINDOW = 0,              // 0
            OBJID_SYSMENU = -1,             // -1
            OBJID_TITLEBAR = -2,            // -2
            OBJID_MENU = -3,                // -3
            OBJID_CLIENT = -4,              // -4
            OBJID_VSCROLL = -5,             // -5
            OBJID_HSCROLL = -6,             // -6
            OBJID_SIZEGRIP = -7,            // -7
            OBJID_CARET = -8,               // -8
            OBJID_CURSOR = -9,              // -9
            OBJID_ALERT = -10,               // -10
            OBJID_SOUND = -11,               // -11
            OBJID_QUERYCLASSNAMEIDX = -12,   // -12
            OBJID_NATIVEOM = -16             // -16
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
