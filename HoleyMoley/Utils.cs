using System;
using System.Windows.Interop;
using System.Windows;


namespace HoleyMoley
{
    public static class Utils
    {
        public static Size GetSizeOfWindow(AppInfo appInfo)
        {
            Size size;
            IntPtr hWnd = appInfo.HWnd;

            if (hWnd == IntPtr.Zero)
            {
                System.Windows.Media.Matrix toDevice;

                using (var source = new HwndSource(new HwndSourceParameters()))
                    toDevice = source.CompositionTarget.TransformToDevice;

                size.Width = (int)Math.Round(SystemParameters.PrimaryScreenWidth * toDevice.M11);
                size.Height = (int)Math.Round(SystemParameters.PrimaryScreenHeight * toDevice.M22);
            }
            else
            {
                //      var rect = new IntRect();
                //      User32.GetWindowRect(hWnd, ref rect);

                IntRect rect = appInfo.CurrentWindowRect(); // User32.GetWindowRect(hWnd, ref rect);

                size.Width = rect.Right - rect.Left;
                size.Height = rect.Bottom - rect.Top;

                // we HAVE to have an even width and height, otherwise the codec gives us a big phat middle finger
                if (size.Width % 2 == 1)
                    size.Width--;
                if (size.Height % 2 == 1)
                    size.Height--;
            }

            return size;
        }
    }
}
