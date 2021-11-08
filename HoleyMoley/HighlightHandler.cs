using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace HoleyMoley
{
    /// <summary>
    /// Highlights windows by putting another window underneath the target window, slightly bigger, to give an outline effect.
    /// Note that the highlighting window only exists while this is enabled - else it is closed (cuts down with needless processing and events)
    /// </summary>
    public static class HighlightHandler
    {
        public static Highlight Highlight { get; set; } = null;
        public static HashSet<IntPtr> IgnoreHwnds { get; set; } = new HashSet<IntPtr>();

        private static Dictionary<string, MatchMaker> MatchLists { get; set; } = new Dictionary<string, MatchMaker>();
        private static Color NoMatchColor { get; set; } = Color.Goldenrod;
        public static Controller Controller { get; set; }
        private static int BorderSize { get; set; } = 5;
        private static int DoubleBorderSize { get; set; } = 10;

        private static Random Rnd { get; set; } = new Random();

        public static IntPtr CurrentHwnd { get; private set; } = IntPtr.Zero;

        static HighlightHandler()
        {
            // Standard `ignore` windows...
            //  "Program Manager" window (windows background etc.)
            IntPtr programManagerHwnd = NativeMethods.FindWindowByCaption(IntPtr.Zero, "Program Manager");
            if (programManagerHwnd != IntPtr.Zero)
                AddToIgnoreList(programManagerHwnd);
        }

        public static void Init()
        {
            Highlight = new Highlight();
            Highlight.Show();
            Highlight.Visible = false;

            // Stuff to ignore by default
            // HoleyMoley controller window, main window and itself
            AddToIgnoreList(Highlight.Handle);

            // Listen for name change changes across all processes/threads on current desktop...
            TitleTextHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, (uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, NameChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            LocationChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, (uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, LocationChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            FocusChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_FOCUS, (uint)ObjectEventContants.EVENT_OBJECT_FOCUS, IntPtr.Zero, FocusChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            DestroyChangeHook = NativeMethods.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_DESTROY, (uint)ObjectEventContants.EVENT_OBJECT_DESTROY, IntPtr.Zero, DestroyChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
        }

        public static void CleanUp()
        {
            if (DestroyChangeHook != IntPtr.Zero)
                NativeMethods.UnhookWinEvent(DestroyChangeHook);
            if (FocusChangeHook != IntPtr.Zero)
                NativeMethods.UnhookWinEvent(FocusChangeHook);
            if (LocationChangeHook != IntPtr.Zero)
                NativeMethods.UnhookWinEvent(LocationChangeHook);
            if (TitleTextHook != IntPtr.Zero)
                NativeMethods.UnhookWinEvent(TitleTextHook);

            DestroyChangeHook = IntPtr.Zero;
            FocusChangeHook = IntPtr.Zero;
            LocationChangeHook = IntPtr.Zero;
            TitleTextHook = IntPtr.Zero;

            if (Highlight != null)
            {
                RemoveFromIgnoreList(Highlight.Handle);

                Highlight.Close();
                Highlight = null;

                CurrentHwnd = IntPtr.Zero;

                if (Controller != null)
                    Controller.SetHighlightingTitle(null);
            }
        }

        public static void SetBorderSize(int size)
        {
            BorderSize = size;
            DoubleBorderSize = size + size;
        }

        public static void SetNoMatchColor(Color color)
        {
            NoMatchColor = color;
        }

        public static void SetMatchList(string name, string matches, Color color)
        {
            MatchMaker matchMaker;

            if (!MatchLists.TryGetValue(name, out matchMaker))
            {
                matchMaker = new MatchMaker() { Name = name, Color = color };
                MatchLists.Add(name, matchMaker);
            }

            matchMaker.SetMatches(matches);

        }

        public static void AddToIgnoreList(IntPtr hwnd)
        {
            IgnoreHwnds.Add(hwnd);
        }

        public static void RemoveFromIgnoreList(IntPtr hwnd)
        {
            IgnoreHwnds.Remove(hwnd);
        }

        public static void SetPosition(IntPtr parent, int x, int y, int w, int h, Color color)
        {
            // Note that this won't be called unless an event is triggered.
            // We don't need to worry about only making window visible when all is enabled.

            // ToDo: Will not setting color here if it's not changed save us any performance?
            Highlight.BackColor = color;
            bool result = NativeMethods.SetWindowPos(Highlight.Handle, parent, x, y, w, h, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
        }

        public static void Hide()
        {
            //CleanUp();
            Highlight.Visible = false;
        }

        public static void Show()
        {
            if (Highlight == null)
                Init();
            //else
            //    Highlight.Visible = true;

            // We don't actually show anything till a window event occures to trigger highlighting to actually do something
        }

        static NativeMethods.WinEventDelegate NameChangeDelegate = new NativeMethods.WinEventDelegate(TitleChangeProc);
        static NativeMethods.WinEventDelegate LocationChangeDelegate = new NativeMethods.WinEventDelegate(LocationChangeProc);
        static NativeMethods.WinEventDelegate FocusChangeDelegate = new NativeMethods.WinEventDelegate(FocusChangeProc);
        static NativeMethods.WinEventDelegate DestroyChangeDelegate = new NativeMethods.WinEventDelegate(DestroyChangeProc);

        private static IntPtr TitleTextHook { get; set; } = IntPtr.Zero;
        private static IntPtr LocationChangeHook { get; set; } = IntPtr.Zero;
        private static IntPtr FocusChangeHook { get; set; } = IntPtr.Zero;
        private static IntPtr DestroyChangeHook { get; set; } = IntPtr.Zero;

        static void TitleChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // filter out non-HWND namechanges... (eg. items within a listbox)
            if (idObject != 0 || idChild != 0)
                return;

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
            //if (idObject != 0 || idChild != 0)
            //{
            //    return;
            //}

            if (hwnd == CurrentHwnd)
                return; // No need to do anything if currently selected!

#if DEBUG
            Debug.Print($"{DateTime.Now} FOCUS: {NativeMethods.WindowInfo(hwnd, idObject)}");
#endif

            // Only Client calls seem to be relevant (and a focus event may be raised when a client + several children all raise focus events - only want the client, not the children)
            if (idObject != (int)SystemObjectIDs.OBJID_CLIENT)
                return;

            // Ignore ToolWindow windows (covers things like combobox pop ups)
            // Note that viewing e.g. combobox popups, every single row seems to generate a focus event as you move a mouse over them
            long exStyle = (long)NativeMethods.GetWindowLongPtr(hwnd, (int)GetWindowLongFlags.GWL_EXSTYLE);
            if ((exStyle & (long)WindowStylesEx.WS_EX_TOOLWINDOW) != 0)
                return;

            // Pallet windows are used for valid windows AND system windows (such as pop up windows for task bar). Checking for nondirectionalbitmap seems to sort this...
            if ((exStyle & (long)WindowStylesEx.WS_EX_PALETTEWINDOW) != 0 && (exStyle & (long)WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP) != 0)
                return;

            IntPtr parentHwnd = GetParent(hwnd);

            Debug.Print($"{DateTime.Now} ...: hwnd={hwnd}, parent={parentHwnd}");

            if (IgnoreHwnds.Contains(parentHwnd))
                return;

            CurrentHwnd = parentHwnd;
            HighlightWindow(parentHwnd);
        }

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

            if (hwnd == IntPtr.Zero)
                Highlight.Visible = false;

            HighlightWindow(GetParent(hwnd), true);
            //}
        }

        static void DestroyChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != CurrentHwnd)
                return;

            if ((SystemObjectIDs)idObject != SystemObjectIDs.OBJID_WINDOW)
                return;

            // Destroy can be nothing more than minimizing to task bar - not closing a window. Obviously does the same thing :)

            // Byebye to the currently highlighted window - so we hide our highlighting
            Debug.Print($"DESTROY: {hwnd.ToString("x8")}  - {idObject}");
            HighlightHandler.Hide();
        }

        private static Color CurrentColor;

        // ToDo: Make ignore title change a configurable option?
        /// <summary>
        /// Sets highlighting around a window. Just give it a hwnd! Can be set to ignore checking of title string - e.g. during a resize operation, it's unlikely to have a title change, so don't bother.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ignoreTitleCheck"></param>
        private static void HighlightWindow(IntPtr hwnd, bool ignoreTitleCheck = false)
        {
            // Only bother if this is for currently focused window (should never actually not be)
            if (hwnd != CurrentHwnd || hwnd == IntPtr.Zero)
                return;

            if (!ignoreTitleCheck)
            {
                // Get caption
                int capacity = NativeMethods.GetWindowTextLength(hwnd) * 2;
                StringBuilder stringBuilder = new StringBuilder(capacity);
                NativeMethods.GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);
                //Debug.Print($"Original '{stringBuilder}': {hwnd.ToString("x4")}, Parent: {GetParent(hwnd).ToString("x4")} with desktop={NativeMethods.GetDesktopWindow()}");

                string title = stringBuilder.ToString().ToLower();

                CurrentColor = NoMatchColor;

                // ToDo: What is performance variants of...
                // below with GoTo
                // below with escape variable
                // equivalent of var color = MatchLists.SelectMany(ml => ml.Value).FirstOrDefault(mm => mm.MatchList.Any(m => title.Contains(m)))?.Color ?? Color.Gold;

                // Check for title matches
                //bool searching = true;
                foreach (KeyValuePair<string, MatchMaker> entry in MatchLists)
                {
                    MatchMaker matchMaker = entry.Value;

                    foreach (var match in matchMaker.MatchList)
                    {
                        if (title.Contains(match))
                        {
                            CurrentColor = matchMaker.Color;
                            //                searching = false;
                            //                break;
                            goto ColorSet;              // :)
                        }
                    }

                    //        if (searching == false)
                    //            break;
                }

                Controller.SetHighlightingTitle(title);
            }
        ColorSet:

            var rect = new IntRect();
            NativeMethods.GetWindowRect(hwnd, ref rect);
            //var relativeRect = new System.Drawing.Rectangle(0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top);

            SetPosition(hwnd, rect.Left - BorderSize, rect.Top - BorderSize, rect.Right - rect.Left + DoubleBorderSize, rect.Bottom - rect.Top + DoubleBorderSize, CurrentColor);
        }

        /// <summary>
        /// Resets the highlight window - handy e.g. after changing border size
        /// </summary>
        public static void ReHighlightWindow()
        {
            if (CurrentHwnd == IntPtr.Zero)
                return;

            var rect = new IntRect();
            NativeMethods.GetWindowRect(CurrentHwnd, ref rect);

            SetPosition(CurrentHwnd, rect.Left - BorderSize, rect.Top - BorderSize, rect.Right - rect.Left + DoubleBorderSize, rect.Bottom - rect.Top + DoubleBorderSize, CurrentColor);
        }
    }

    public class MatchMaker
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public List<string> MatchList { get; set; } = new List<string>();

        public void SetMatches(string matchList)
        {
            MatchList.Clear();

            string[] entries = matchList.Split(';');

            foreach (var entry in entries)
            {
                string cleanedEntry = entry.Trim().ToLower();

                if (!string.IsNullOrWhiteSpace(cleanedEntry))

                    MatchList.Add(cleanedEntry);
            }
        }
    }
}
