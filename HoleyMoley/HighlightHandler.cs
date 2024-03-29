﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

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
        public static Main UI { get; set; }
        private static int BorderSize { get; set; } = 5;
        private static int DoubleBorderSize { get; set; } = 10;

        private static Random Rnd { get; set; } = new Random();

        public static IntPtr CurrentHwnd { get; private set; } = IntPtr.Zero;

        static HighlightHandler()
        {
            // Standard `ignore` windows...
            //  "Program Manager" window (windows background etc.)
            IntPtr programManagerHwnd = Native.FindWindowByCaption(IntPtr.Zero, "Program Manager");
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
            TitleTextHook = Native.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, (uint)ObjectEventContants.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, NameChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            LocationChangeHook = Native.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, (uint)ObjectEventContants.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, LocationChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            FocusChangeHook = Native.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_FOCUS, (uint)ObjectEventContants.EVENT_OBJECT_FOCUS, IntPtr.Zero, FocusChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
            DestroyChangeHook = Native.SetWinEventHook((uint)ObjectEventContants.EVENT_OBJECT_DESTROY, (uint)ObjectEventContants.EVENT_OBJECT_DESTROY, IntPtr.Zero, DestroyChangeDelegate, 0, 0, (uint)Flags.WINEVENT_OUTOFCONTEXT);
        }

        public static void CleanUp()
        {
            if (DestroyChangeHook != IntPtr.Zero)
                Native.UnhookWinEvent(DestroyChangeHook);
            if (FocusChangeHook != IntPtr.Zero)
                Native.UnhookWinEvent(FocusChangeHook);
            if (LocationChangeHook != IntPtr.Zero)
                Native.UnhookWinEvent(LocationChangeHook);
            if (TitleTextHook != IntPtr.Zero)
                Native.UnhookWinEvent(TitleTextHook);

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

                if (UI != null)
                    UI.SetHighlightingTitle(null);
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

        public static void ChangeMatchlistColor(string name, Color color)
        {
            MatchMaker matchMaker;

            if (MatchLists.TryGetValue(name, out matchMaker))
            {
                matchMaker.Color = color;
            }
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
            bool result = Native.SetWindowPos(Highlight.Handle, parent, x, y, w, h, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
            if (result)
            {
                if (UI.DebugEnabled())
                    UI.AddToDebug($"SetPosition Success @ [{x},{y}] [{w},{h}] (Parent {parent.ToString("x8")})");
            }
            else
            {
                int error = Marshal.GetLastWin32Error();
                //result = NativeMethods.SetWindowPos(Highlight.Handle, (IntPtr)(-2), x, y, w, h, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);

                if (UI.DebugEnabled())
                {
                    UI.AddToDebug($"SetPosition {(result ? "Success" : "Failed")} @ [{x},{y}] [{w},{h}] (Error {error})");
                }
            }

        }
        public static void Hide()
        {
            //CleanUp();
            if (Highlight != null)
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

        static Native.WinEventDelegate NameChangeDelegate = new Native.WinEventDelegate(TitleChangeProc);
        static Native.WinEventDelegate LocationChangeDelegate = new Native.WinEventDelegate(LocationChangeProc);
        static Native.WinEventDelegate FocusChangeDelegate = new Native.WinEventDelegate(FocusChangeProc);
        static Native.WinEventDelegate DestroyChangeDelegate = new Native.WinEventDelegate(DestroyChangeProc);

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
            long style = (long)Native.GetWindowLongPtr(hwnd, (int)GetWindowLongFlags.GWL_STYLE);
            long isTopLevel = style & ((long)WindowStyles.WS_CHILD);

            // Only parents
            if (isTopLevel != 0)
                return;

            if (UI.DebugEnabled())
                UI.AddToDebug($"{DateTime.Now:HH:mm:ss.ff} Highlight.TitleChange: {Native.WindowInfo(hwnd, idObject)}");

            HighlightWindow(hwnd);
        }

        private static DateTime FocusDebugTimestamp { get; set; } = DateTime.Now;

        static void FocusChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            //if (idObject != 0 || idChild != 0)
            //{
            //    return;
            //}

            IntPtr parentHwnd = IntPtr.Zero;
            parentHwnd = GetParent(hwnd);

            // Debug.Print($"Focus In - hwnd = {hwnd.ToString("x8")}, CurrentHwnd = {CurrentHwnd.ToString("x8")}, ParentHwnd = {parentHwnd.ToString("x8")}");

            bool ignore = false;
            string ignoreReason = string.Empty;

            // No need to do anything if currently selected!
            if (parentHwnd == CurrentHwnd)
            {
                ignore = true;
                ignoreReason = "Ignored: Re-Focus of already highlighted window";
            }
            // Only Client calls seem to be relevant (and a focus event may be raised when a client + several children all raise focus events - only want the client, not the children)
            else if (idObject != (int)SystemObjectIDs.OBJID_CLIENT)
            {
                ignore = true;
                ignoreReason = "Ignored: Not a Client call";
            }
            else
            {
                // Ignore ToolWindow windows (covers things like combobox pop ups)
                // Note that viewing e.g. combobox popups, every single row seems to generate a focus event as you move a mouse over them
                // Did use hwnd, seems happy enough checking the parentHwnd
                long style = (long)Native.GetWindowLongPtr(parentHwnd, (int)GetWindowLongFlags.GWL_STYLE);
                long exStyle = (long)Native.GetWindowLongPtr(parentHwnd, (int)GetWindowLongFlags.GWL_EXSTYLE);

                if ((exStyle & (long)WindowStylesEx.WS_EX_TOOLWINDOW) != 0 && (style & (long)WindowStyles.WS_POPUPWINDOW) != 0)
                {
                    ignore = true;
                    ignoreReason = "Ignored: WS_EX_ToolWindow with WS_PopUpWindow set";
                }
                // Pallet windows are used for valid windows AND system windows (such as pop up windows for task bar). Checking for nondirectionalbitmap seems to sort this...
                else if ((exStyle & (long)WindowStylesEx.WS_EX_PALETTEWINDOW) != 0 && (exStyle & (long)WindowStylesEx.WS_EX_NOREDIRECTIONBITMAP) != 0)
                {
                    ignore = true;
                    ignoreReason = "Ignored: WS_EX_PaletteWindow with WS_EX_NoRedirectionBitmap set";
                }
            }

            if (!ignore)
            {
                if (IgnoreHwnds.Contains(parentHwnd))
                {
                    ignore = true;
                    ignoreReason = "Ignored: parentHwnd part of ignore list";
                }
            }

            if (UI.DebugEnabled())
            {
                string debug = $"{DateTime.Now:HH:mm:ss.ff} Highlight.Focus (Current {CurrentHwnd.ToString("x8")}): {ignoreReason}{System.Environment.NewLine}{Native.WindowInfo(parentHwnd, idObject)}";

                if (parentHwnd != IntPtr.Zero)
                    debug = $"{debug}{System.Environment.NewLine}   Parent: {parentHwnd.ToString("x8")} (highlight target hwnd)";

                if ((DateTime.Now - FocusDebugTimestamp).TotalSeconds > 1)
                {
                    debug = $"{debug}{System.Environment.NewLine}{System.Environment.NewLine}################################";
                    FocusDebugTimestamp = DateTime.Now;
                }

                UI.AddToDebug(debug);
            }

            if (ignore)
                return;
            Debug.Print($"Current = {CurrentHwnd.ToString("x8")}, new parent = {parentHwnd.ToString("x8")}");
            CurrentHwnd = parentHwnd;
            HighlightWindow(parentHwnd);
        }

        private static IntPtr GetParent(IntPtr hwnd)
        {
            IntPtr parent = Native.GetAncestor(hwnd, GetAncestorFlags.GetRoot);

            if (parent == IntPtr.Zero || parent == Native.GetDesktopWindow())
                return hwnd;

            return parent;
        }

        static void LocationChangeProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd != CurrentHwnd || (SystemObjectIDs)idObject == SystemObjectIDs.OBJID_CARET || (SystemObjectIDs)idObject == SystemObjectIDs.OBJID_CURSOR)
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
            Debug.Print("Location change");
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

            if (UI.DebugEnabled())
                UI.AddToDebug($"{DateTime.Now:HH:mm:ss.ff} Highlight.Destroy: {Native.WindowInfo(hwnd, idObject)}");

            // Byebye to the currently highlighted window - so we hide our highlighting

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

            // Get .exe information


            if (!ignoreTitleCheck)
            {
                // Get caption
                int capacity = Native.GetWindowTextLength(hwnd) * 2;
                StringBuilder stringBuilder = new StringBuilder(capacity);
                Native.GetWindowText(hwnd, stringBuilder, stringBuilder.Capacity);
                //Debug.Print($"Original '{stringBuilder}': {hwnd.ToString("x4")}, Parent: {GetParent(hwnd).ToString("x4")} with desktop={NativeMethods.GetDesktopWindow()}");

                string title = stringBuilder.ToString();
                string lcaseTitle = title.ToLower();

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
                        if (lcaseTitle.Contains(match))
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

                UI.SetHighlightingTitle(title);
            }
        ColorSet:

            var rect = new IntRect();
            Native.GetWindowRect(hwnd, ref rect);
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
            Native.GetWindowRect(CurrentHwnd, ref rect);

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
