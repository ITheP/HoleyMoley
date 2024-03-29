﻿using System;
using System.Windows.Media.Imaging;

namespace HoleyMoley
{
    public class AppInfo
    {
        public override string ToString()
        {
            return Title;
        }

        public string Title { get; set; }
        public IntPtr HWnd { get; set; }
        public BitmapSource Preview { get; set; }
        public string Debug { get; set; }
        public bool BoundsSet { get; set; } = false;
        public IntRect Bounds { get; set; }

        public IntRect CurrentWindowRect()
        {
            if (BoundsSet)
                return Bounds;

            var rect = new IntRect();
            Native.GetWindowRect(HWnd, ref rect);
            return rect;
        }
    }
}