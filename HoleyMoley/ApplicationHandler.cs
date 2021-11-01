using System;
using System.Collections.Generic;
using System.Diagnostics;

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

       
    }
}
