using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HoleyMoley
{
    internal static class NativeMethods
    {
        private const int WS_EX_TRANSPARENT = 0x20;

        #region User32
        public const int CURSOR_SHOWING = 0x00000001;

        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags gaFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref IntRect rect);

        //[DllImport("user32.dll")]
        //internal static extern IntPtr GetWindowRect(IntPtr hWnd, ref IntRect rect);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out CursorInfo pci);

        [DllImport("user32.dll")]
        internal static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        internal static extern bool GetIconInfo(IntPtr hIcon, out IconInfo piconinfo);

        [DllImport("user32.dll")]
        public static extern bool DrawIcon(IntPtr hdc, int x, int y, IntPtr hIcon);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        internal static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        internal static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        #endregion

        #region Gdi32

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, PatBltType dwRop);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        #endregion

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IconInfo
    {
        public bool fIcon;          // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
        public int xHotspot;        // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
        public int yHotspot;        // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
        public IntPtr hbmMask;      // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
        public IntPtr hbmColor;     // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
    }

    public struct Size
    {
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public int cbSize;          // Specifies the size, in bytes, of the structure. 
        public int flags;           // Specifies the cursor state. This parameter can be one of the following values:
        public IntPtr hCursor;      // Handle to the cursor. 
        public POINT ptScreenPos;   // A POINT structure that receives the screen coordinates of the cursor. 
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// Blitter types
    /// </summary>
    public enum PatBltType
    {
        SrcCopy = 0x00CC0020,
        SrcPaint = 0x00EE0086,
        SrcAnd = 0x008800C6,
        SrcInvert = 0x00660046,
        SrcErase = 0x00440328,
        NotSrcCopy = 0x00330008,
        NotSrcErase = 0x001100A6,
        MergeCopy = 0x00C000CA,
        MergePaint = 0x00BB0226,
        PatCopy = 0x00F00021,
        PatPaint = 0x00FB0A09,
        PatInvert = 0x005A0049,
        DstInvert = 0x00550009,
        Blackness = 0x00000042,
        Whiteness = 0x00FF0062
    }

    public enum GetAncestorFlags
    {
        /// <summary>
        /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
        /// </summary>
        GetParent = 1,
        /// <summary>
        /// Retrieves the root window by walking the chain of parent windows.
        /// </summary>
        GetRoot = 2,
        /// <summary>
        /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
        /// </summary>
        GetRootOwner = 3
    }

    public enum GetWindowLongFlags
    {
        GWL_WNDPROC = (-4),
        GWL_HINSTANCE = (-6),
        GWL_HWNDPARENT = (-8),
        GWL_STYLE = (-16),
        GWL_EXSTYLE = (-20),
        GWL_USERDATA = (-21),
        GWL_ID = (-12)
    }

    // http://www.pinvoke.net/default.aspx/Enums/WindowStyles.html
    public enum WindowStyles : long
    {
        /// <summary>The window has a thin-line border.</summary>
        WS_BORDER = 0x800000,

        /// <summary>The window has a title bar (includes the WS_BORDER style).</summary>
        WS_CAPTION = 0xc00000,

        /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>
        WS_CHILD = 0x40000000,

        /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
        WS_CLIPCHILDREN = 0x2000000,

        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.
        /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        WS_CLIPSIBLINGS = 0x4000000,

        /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
        WS_DISABLED = 0x8000000,

        /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
        WS_DLGFRAME = 0x400000,

        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.
        /// The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// </summary>
        WS_GROUP = 0x20000,

        /// <summary>The window has a horizontal scroll bar.</summary>
        WS_HSCROLL = 0x100000,

        /// <summary>The window is initially maximized.</summary>
        WS_MAXIMIZE = 0x1000000,

        /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MAXIMIZEBOX = 0x10000,

        /// <summary>The window is initially minimized.</summary>
        WS_MINIMIZE = 0x20000000,

        /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MINIMIZEBOX = 0x20000,

        /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
        WS_OVERLAPPED = 0x0,

        /// <summary>The window is an overlapped window.</summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

        /// <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>
        WS_POPUP = 0x80000000u,

        /// <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

        /// <summary>The window has a sizing border.</summary>
        WS_SIZEFRAME = 0x40000,

        /// <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>
        WS_SYSMENU = 0x80000,

        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
        /// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.  
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        /// </summary>
        WS_TABSTOP = 0x10000,

        /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
        WS_VISIBLE = 0x10000000,

        /// <summary>The window has a vertical scroll bar.</summary>
        WS_VSCROLL = 0x200000
    }

    public enum WindowStylesEx : uint
    {
        /// <summary>Specifies a window that accepts drag-drop files.</summary>
        WS_EX_ACCEPTFILES = 0x00000010,

        /// <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
        WS_EX_APPWINDOW = 0x00040000,

        /// <summary>Specifies a window that has a border with a sunken edge.</summary>
        WS_EX_CLIENTEDGE = 0x00000200,

        /// <summary>
        /// Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
        /// </summary>
        /// <remarks>
        /// With WS_EX_COMPOSITED set, all descendants of a window get bottom-to-top painting order using double-buffering.
        /// Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        /// but only if the descendent window also has the WS_EX_TRANSPARENT bit set.
        /// Double-buffering allows the window and its descendents to be painted without flicker.
        /// </remarks>
        WS_EX_COMPOSITED = 0x02000000,

        /// <summary>
        /// Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
        /// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
        /// The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
        /// The Help application displays a pop-up window that typically contains help for the child window.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        WS_EX_CONTEXTHELP = 0x00000400,

        /// <summary>
        /// Specifies a window which contains child windows that should take part in dialog box navigation.
        /// If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
        /// such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        WS_EX_CONTROLPARENT = 0x00010000,

        /// <summary>Specifies a window that has a double border.</summary>
        WS_EX_DLGMODALFRAME = 0x00000001,

        /// <summary>
        /// Specifies a window that is a layered window.
        /// This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// </summary>
        WS_EX_LAYERED = 0x00080000,

        /// <summary>
        /// Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_LAYOUTRTL = 0x00400000,

        /// <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
        WS_EX_LEFT = 0x00000000,

        /// <summary>
        /// Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_LEFTSCROLLBAR = 0x00004000,

        /// <summary>
        /// Specifies a window that displays text using left-to-right reading-order properties. This is the default.
        /// </summary>
        WS_EX_LTRREADING = 0x00000000,

        /// <summary>
        /// Specifies a multiple-document interface (MDI) child window.
        /// </summary>
        WS_EX_MDICHILD = 0x00000040,

        /// <summary>
        /// Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
        /// The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        /// </summary>
        WS_EX_NOACTIVATE = 0x08000000,

        /// <summary>
        /// Specifies a window which does not pass its window layout to its child windows.
        /// </summary>
        WS_EX_NOINHERITLAYOUT = 0x00100000,

        /// <summary>
        /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        /// </summary>
        WS_EX_NOPARENTNOTIFY = 0x00000004,

        /// <summary>
        /// The window does not render to a redirection surface.
        /// This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        /// </summary>
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,

        /// <summary>Specifies an overlapped window.</summary>
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,

        /// <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,

        /// <summary>
        /// Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        /// The shell language must support reading-order alignment for this to take effect.
        /// Using the WS_EX_RIGHT style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
        /// </summary>
        WS_EX_RIGHT = 0x00001000,

        /// <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
        WS_EX_RIGHTSCROLLBAR = 0x00000000,

        /// <summary>
        /// Specifies a window that displays text using right-to-left reading-order properties.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        WS_EX_RTLREADING = 0x00002000,

        /// <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
        WS_EX_STATICEDGE = 0x00020000,

        /// <summary>
        /// Specifies a window that is intended to be used as a floating toolbar.
        /// A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
        /// A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
        /// If a tool window has a system menu, its icon is not displayed on the title bar.
        /// However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        WS_EX_TOOLWINDOW = 0x00000080,

        /// <summary>
        /// Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        /// To add or remove this style, use the SetWindowPos function.
        /// </summary>
        WS_EX_TOPMOST = 0x00000008,

        /// <summary>
        /// Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        /// The window appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        WS_EX_TRANSPARENT = 0x00000020,

        /// <summary>Specifies a window that has a border with a raised edge.</summary>
        WS_EX_WINDOWEDGE = 0x00000100
    }

    public enum SetWindowPosFlags : uint
    {
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOREDRAW = 0x0008,
        NOACTIVATE = 0x0010,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        SHOWWINDOW = 0x0040,
        HIDEWINDOW = 0x0080,
        NOCOPYBITS = 0x0100,
        NOOWNERZORDER = 0x0200,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        DEFERERASE = 0x2000,
        ASYNCWINDOWPOS = 0x4000
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
